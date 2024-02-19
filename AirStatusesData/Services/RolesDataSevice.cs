using AirStatusesDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AirStatusesData.Services
{
    public static class RolesDataSeviceExtensions
    {
        public static void AddRolesDataSevice(this IServiceCollection services)
        {
            services.AddScoped<IRolesDataSevice, RolesDataSevice>();
        }
    }
    public class RolesDataSevice : IRolesDataSevice
    {
        private readonly AppDbContext _context;

        public RolesDataSevice(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Role> GetRoleByIdAsync(int id)
        {
            return await _context.Roles.FindAsync(id);
        }

        public async Task<Role> CreateRoleAsync(string roleCode)
        {
            var maxRoleId = await _context.Roles.MaxAsync(r => r.Id);
            var role = new Role
            {
                Id = ++maxRoleId,
                Code = roleCode
            };
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return role;
        }

        public async Task<Role> UpdateRoleAsync(string roleCode, int roleId)
        {
            Role? role = await _context.Roles.FindAsync(roleId);
            if (role != null)
            {
                role.Code = roleCode;
                _context.Entry(role).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return role;
            }
            else
            {
                throw new InvalidOperationException($"Роль с Id: {roleId} не найдена, ошибка обновления");
            }
        }

        public async Task<bool> DeleteRoleAsync(int id)
        {
            var deleted = false;
            var role = await _context.Roles.FindAsync(id);
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            deleted = true;
            return deleted;
        }
    }
}
