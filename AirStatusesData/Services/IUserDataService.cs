
using AirStatusesData.Services.Dto;
using AirStatusesDomain;

namespace AirStatusesData.Services
{
    public interface IUserDataService
    {
        Task<int> AddUserAsync(User user);
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(int Id);
        Task<int> SetUserRoleAsync(int userId, int roleId);
        Task<User?> GetUserByNamePassword(string userName, string password);

    }
}
