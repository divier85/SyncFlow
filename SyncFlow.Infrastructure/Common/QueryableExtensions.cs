using SyncFlow.Application.DTOs.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Linq;

namespace SyncFlow.Infrastructure.Common
{
    public static class QueryableExtensions
    {
        public static async Task<Application.Common.Models.PagedResult<T>> ToPagedResultAsync<T>(
            this System.Linq.IQueryable<T> query,
            PagedFilterDto filter)
        {
            // Ordenamiento dinámico múltiple
            if (filter.OrderBy != null && filter.OrderBy.Length > 0)
            {
                var orderExpressions = new List<string>();

                for (int i = 0; i < filter.OrderBy.Length; i++)
                {
                    var field = filter.OrderBy[i];
                    var direction = (filter.OrderDirection != null && i < filter.OrderDirection.Length)
                        ? filter.OrderDirection[i]
                        : "asc";

                    direction = direction.ToLower() == "desc" ? "descending" : "ascending";

                    orderExpressions.Add($"{field} {direction}");
                }

                var fullOrdering = string.Join(", ", orderExpressions);
                query = query.OrderBy(fullOrdering); // Usa Dynamic LINQ
            }
            else if (typeof(T).GetProperty("CreatedAt") != null)
            {
                query = query.OrderBy("CreatedAt descending");
            }

            var totalItems = await query.CountAsync();

            var items = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return new Application.Common.Models.PagedResult<T>
            {
                Items = items,
                TotalItems = totalItems,
                Page = filter.Page,
                PageSize = filter.PageSize
            };
        }
    }
}
