using AirStatusesDomain;

namespace AirStatusesData.Services
{
    public interface IRolesDataSevice
    {
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<Role> GetRoleByIdAsync(int id);
        Task<Role> CreateRoleAsync(string roleCode);
        Task<Role> UpdateRoleAsync(string roleCode, int roleId);
        Task<bool> DeleteRoleAsync(int id);
    }
}
