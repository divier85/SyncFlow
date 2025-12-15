using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SyncFlow.Application.Common.Email;
using SyncFlow.Application.DTOs.Businesses;
using SyncFlow.Application.DTOs.Users;
using SyncFlow.Application.Interfaces.Services;
using SyncFlow.Application.Interfaces.Services.Identity;
using SyncFlow.Persistence.Auth;
using SyncFlow.Persistence.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace SyncFlow.Infrastructure.Services;

public class BusinessService : IBusinessService
{
    private readonly ISyncFlowDbContext _context;

    private readonly UserManager<ApplicationUser> _userMgr;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IEmailSender _mail;
    private readonly RoleManager<ApplicationRole> _roleMgr;
    private readonly IRoleService _roleService;
    private readonly IUserRoleService _userRoleService;

    public BusinessService(ISyncFlowDbContext context,
        UserManager<ApplicationUser> userMgr,
        IHttpContextAccessor httpContextAccessor,
        IEmailSender mail,
        RoleManager<ApplicationRole> roleMgr,
        IRoleService roleService,
        IUserRoleService userRoleService)
    {
        _context = context;
        _userMgr = userMgr;
        _httpContextAccessor = httpContextAccessor;
        _mail = mail;
        _roleMgr = roleMgr;
        _roleService = roleService;
        _userRoleService = userRoleService;
    }

    public async Task<IEnumerable<Business>> GetAllAsync()
    {
        var businesses = await _context.Businesses
            .Include(b => b.Users)
            .Include(b => b.Projects)
            .Include(b => b.Roles)
            .ToListAsync();

        return businesses.Select(b => MapToBusinessDto(b)).ToList();
    }

    public async Task<Business?> GetByIdAsync(Guid id)
    {
        var business = await _context.Businesses
            .Include(b => b.Users)
            .Include(b => b.Projects)
            .Include(b => b.Roles)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (business == null)
        {
            return null;
        }

        return MapToBusinessDto(business);
    }

    public async Task<Business> CreateAsync(Business business, CancellationToken cancellationToken)
    {
        if (business == null)
        {
            ArgumentNullException.ThrowIfNull(business);
        }

        _context.Businesses.Add(MapToBusinessEntity(business));
        await _context.SaveChangesAsync(cancellationToken);
        return business;
    }

    public async Task<Business> CreateBusinessWithOwnerAsync(CreateBusinessWithOwnerRequest request, CancellationToken cancellationToken)
    {
        var business = new Domain.Entities.Business
        {
            Name = request.Name,
            Address = request.Address,
            Industry = request.Industry
        };

        await _context.Businesses.AddAsync(business);
        await _context.SaveChangesAsync(cancellationToken);

        var user = new ApplicationUser
        {
            UserName = request.OwnerEmail,
            Email = request.OwnerEmail,
            BusinessId = business.Id
        };

        var result = await _userMgr.CreateAsync(user, request.OwnerPassword);

        if (!result.Succeeded)
        {
            // Manejo de errores en la creación del usuario
            throw new ApplicationException(string.Join(";", result.Errors.Select(e => e.Description)));
        }

        // 🔹 Verificar si el rol Owner existe (y crearlo si no)
        var role = await _roleService.GetByName("Owner", business.Id);
        if (role == null)
        {
            role = await _roleService.CreateAsync(new CreateRoleRequest("Owner", business.Id));
        }

        // 🔹 Asignar rol Owner al usuario
        //await _userMgr.AddToRoleAsync(user, "Owner");

        await _userRoleService.AddRoleAsync(new AddRoleToUserRequest(user.Id, role.Id, business.Id), cancellationToken);


        var token = await _userMgr.GenerateEmailConfirmationTokenAsync(user);
        var url = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/confirm-email.html" +
                  $"?uid={user.Id}&token={Uri.EscapeDataString(token)}";

        var body = $"<p>Bienvenido. Confirma tu correo haciendo clic " +
                   $"<a href='{url}'>aquí</a>.</p>";

        await _mail.SendAsync(user.Email!, "Confirma tu cuenta", body);

        return MapToBusinessDto(business);
    }

    public async Task<Business> UpdateAsync(Business business, CancellationToken cancellationToken)
    {
        var entity = _context.Businesses.FirstOrDefault(s => s.Id == business.Id);

        if (entity == null)
        {
            ArgumentNullException.ThrowIfNull(entity);
        }

        entity.Name = business.Name;
        entity.Address = business.Address;
        entity.BusinessType = business.BusinessType;
        entity.City = business.City;
        entity.Country = business.Country;
        entity.Email = business.Email;
        entity.Industry = business.Industry;
        entity.IsActive = business.IsActive;
        entity.LegalName = business.LegalName;
        entity.LogoUrl = business.LogoUrl;
        entity.PhoneNumber = business.PhoneNumber;
        entity.RegistrationNumber = business.RegistrationNumber;
        entity.SettingsJson = business.SettingsJson;
        entity.StorageLimitMb = business.StorageLimitMb;
        entity.SubscriptionPlan = business.SubscriptionPlan;
        entity.WebsiteUrl = business.WebsiteUrl;
        entity.TaxId = business.TaxId;

        _context.Businesses.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return business;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var business = await _context.Businesses.FindAsync(id);
        if (business == null) return false;

        _context.Businesses.Remove(business);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static Business MapToBusinessDto(Domain.Entities.Business b)
    {
        return new Business()
        {
            Address = b.Address,
            BusinessType = b.BusinessType,
            City = b.City,
            Country = b.Country,
            Email = b.Email,
            Id = b.Id,
            Industry = b.Industry,
            IsActive = b.IsActive,
            LegalName = b.LegalName,
            LogoUrl = b.LogoUrl,
            Name = b.Name,
            PhoneNumber = b.PhoneNumber,
            RegistrationNumber = b.RegistrationNumber,
            SettingsJson = b.SettingsJson,
            StorageLimitMb = b.StorageLimitMb,
            SubscriptionPlan = b.SubscriptionPlan,
            TaxId = b.TaxId,
            WebsiteUrl = b.WebsiteUrl

        };
    }

    private static Domain.Entities.Business MapToBusinessEntity(Business b)
    {
        return new Domain.Entities.Business()
        {
            Address = b.Address,
            BusinessType = b.BusinessType,
            City = b.City,
            Country = b.Country,
            Email = b.Email,
            Id = b.Id,
            Industry = b.Industry,
            IsActive = b.IsActive,
            LegalName = b.LegalName,
            LogoUrl = b.LogoUrl,
            Name = b.Name,
            PhoneNumber = b.PhoneNumber,
            RegistrationNumber = b.RegistrationNumber,
            SettingsJson = b.SettingsJson,
            StorageLimitMb = b.StorageLimitMb,
            SubscriptionPlan = b.SubscriptionPlan,
            TaxId = b.TaxId,
            WebsiteUrl = b.WebsiteUrl

        };
    }

}
