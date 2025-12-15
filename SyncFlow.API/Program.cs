// Entry point for ASP.NET Core application
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Models;
using SyncFlow.Application.Common.Email;
using SyncFlow.Application.Common.Identity;
using SyncFlow.Application.Common.Tenant;
using SyncFlow.Application.DTOs.Emails;
using SyncFlow.Domain.Entities;
using SyncFlow.Domain.Enums;
using SyncFlow.Infrastructure.Common.Identity;
using SyncFlow.Infrastructure.Common.Tenant;
using SyncFlow.Infrastructure.Email;
using SyncFlow.Infrastructure.Extensions;
using SyncFlow.Infrastructure.Hubs;
using SyncFlow.Persistence.Auth;
using SyncFlow.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SyncFlowDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "SyncFlow API", Version = "v1" });

    // 🔑 1) Definir el esquema JWT Bearer
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Introduce el token JWT con el prefijo **Bearer** (ej. 'Bearer eyJhbGciOi...')"
    });

    // 🔑 2) Exigir el esquema a todas las operaciones por defecto
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


// Registrar los servicios de los distintos proyectos
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IBusinessContext, BusinessContext>();
builder.Services.AddScoped<ICurrentUser, CurrentUser>();
builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();

// Identity
builder.Services.AddIdentityCore<ApplicationUser>(opt =>
{
    opt.User.RequireUniqueEmail = true;
    opt.SignIn.RequireConfirmedEmail = true;
    opt.Password.RequireDigit = false;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequireUppercase = false;
})
.AddRoles<ApplicationRole>()
.AddEntityFrameworkStores<SyncFlowDbContext>()
.AddDefaultTokenProviders();

// JWT
var jwtKey = builder.Configuration["Jwt:Key"] ?? "SuperSecretKey-ChangeMe!";
var issuer = builder.Configuration["Jwt:Issuer"] ?? "SyncFlow";
var signingKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtKey));

var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtKey));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(opt =>
  {
      opt.TokenValidationParameters = new TokenValidationParameters
      {
          ValidIssuer = issuer,
          ValidAudience = issuer,
          IssuerSigningKey = key,
          ValidateIssuerSigningKey = true,
          ValidateIssuer = true,
          ValidateAudience = true
      };
  });

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthorization();

builder.Services.AddSignalR();

builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
    p.WithOrigins("null").AllowAnyHeader().AllowAnyMethod())); // "null" para file://

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SyncFlow API v1");
        c.RoutePrefix = string.Empty; // Swagger en raíz (opcional)
    });
}

app.MapGet("/", () => "SyncFlow API Running");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors();


app.MapPost("/auth/register", async (
    RegisterRequest dto,
    UserManager<ApplicationUser> userMgr,
    SyncFlowDbContext db,
    IEmailSender mail,   // 👈 DI
        HttpContext http) =>
{
    // 1. crea (o busca) el negocio
    var biz = await db.Set<Business>()
        .FirstOrDefaultAsync(b => b.Name == dto.BusinessName)
        ?? new Business { Name = dto.BusinessName };
    if (biz.Id == Guid.Empty) db.Add(biz);

    // 2. crea usuario
    var user = new ApplicationUser
    {
        UserName = dto.Email,
        Email = dto.Email,
        BusinessId = biz.Id
    };

    var result = await userMgr.CreateAsync(user, dto.Password);

    var token = await userMgr.GenerateEmailConfirmationTokenAsync(user);
    var url = $"{http.Request.Scheme}://{http.Request.Host}/confirm-email.html" +
              $"?uid={user.Id}&token={Uri.EscapeDataString(token)}";

    var body = $"<p>Bienvenido. Confirma tu correo haciendo clic " +
               $"<a href='{url}'>aquí</a>.</p>";

    await mail.SendAsync(user.Email!, "Confirma tu cuenta", body);


    return result.Succeeded ? Results.Ok() : Results.BadRequest(result.Errors);
});

app.MapPost("/auth/login", async (
    LoginRequest dto,
    UserManager<ApplicationUser> userMgr,
    RoleManager<ApplicationRole> roleMgr) =>
{
    var user = await userMgr.FindByEmailAsync(dto.Email);
    if (user is null || !await userMgr.CheckPasswordAsync(user, dto.Password))
        return Results.Unauthorized();

    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<SyncFlowDbContext>();

        var roleNames = await userMgr.GetRolesIgnoreFiltersAsync(user, db);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim("business_id", user.BusinessId.ToString())
        };

        foreach (var role in roleNames)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: issuer,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(12),
            signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return Results.Ok(new { token = tokenString });
    }
})
.AllowAnonymous();

// 1. Confirmar e-mail
app.MapPost("/auth/confirmEmail", async (
    ConfirmEmailRequest dto,
    UserManager<ApplicationUser> userMgr) =>
{
    var user = await userMgr.FindByIdAsync(dto.UserId.ToString());
    if (user is null) return Results.NotFound();

    var decoded = Uri.UnescapeDataString(dto.Token);
    var res = await userMgr.ConfirmEmailAsync(user, decoded);
    return res.Succeeded ? Results.Ok("Email confirmado") :
                           Results.BadRequest(res.Errors);
}).AllowAnonymous();

// 2. Solicitar reset
app.MapPost("/auth/forgotPassword", async (
    ForgotPasswordRequest dto,
    UserManager<ApplicationUser> userMgr,
    IEmailSender mail,
    IConfiguration cfg,
    HttpContext http) =>
{
    var user = await userMgr.FindByEmailAsync(dto.Email);
    if (user is null || !(await userMgr.IsEmailConfirmedAsync(user)))
        return Results.Ok(); // evitar enumeración de usuarios

    var token = await userMgr.GeneratePasswordResetTokenAsync(user);
    var url = $"{http.Request.Scheme}://{http.Request.Host}/reset-password.html" +
              $"?uid={user.Id}&token={Uri.EscapeDataString(token)}";

    var body = $"<p>Haz clic <a href='{url}'>aquí</a> para restablecer tu contraseña.</p>";
    await mail.SendAsync(user.Email!, "Restablecer contraseña", body);
    return Results.Ok();
}).AllowAnonymous();

// 3. Restablecer contraseña
app.MapPost("/auth/resetPassword", async (
    ResetPasswordRequest dto,
    UserManager<ApplicationUser> userMgr) =>
{
    var user = await userMgr.FindByIdAsync(dto.UserId.ToString());
    if (user is null) return Results.NotFound();

    var decoded = Uri.UnescapeDataString(dto.Token);
    var res = await userMgr.ResetPasswordAsync(user, decoded, dto.NewPassword);
    return res.Succeeded ? Results.Ok("Contraseña actualizada") :
                           Results.BadRequest(res.Errors);
}).AllowAnonymous();

app.MapGet("/auth/resetPassword", (Guid uid, string token)
        => Results.Redirect($"/reset-password.html?uid={uid}&token={Uri.EscapeDataString(token)}"));

app.MapHub<NotificationHub>("/hubs/notify");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SyncFlowDbContext>();
    await db.SeedCoreStatusesAsync(db);
}

app.MapControllers();
app.UseStaticFiles();
app.Run();



public record RegisterRequest(string Email, string Password, string BusinessName);
public record LoginRequest(string Email, string Password);