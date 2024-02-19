using AirStatusesData.Services.Dto;
using AirStatusesDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Authentication;

namespace AirStatusesData.Services
{
    public static class UserDataExtensions
    {
        public static void AddUserDataService(this IServiceCollection services)
        {
            services.AddScoped<IUserDataService, UsersDataService>();
        }
    }

    public class UsersDataService : IUserDataService
    {
        private readonly AppDbContext _context;
        public UsersDataService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddUserAsync(User user)
        {
            var lastUserId = await _context.Users.MaxAsync(u => u.Id);
            user.Id = ++lastUserId;
            var item = await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return item.Entity.Id;
        }

        public async Task<UserDto?> GetUserByIdAsync(int Id)
        {
            return await _context.Users.Where(u => u.Id == Id)
                                        .Include(u => u.Role)
                                        .Select(u => new UserDto
                                        {
                                            Id = u.Id,
                                            UserName = u.UserName,
                                            RoleCode = u.Role.Code
                                        })
                                        .FirstOrDefaultAsync(u => u.Id == Id);
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            return await _context.Users.Include(u => u.Role)
                                        .Select(u => new UserDto
                                        {
                                            Id = u.Id,
                                            UserName = u.UserName,
                                            RoleCode = u.Role.Code
                                        }).ToListAsync();
        }

        public async Task<int> SetUserRoleAsync(int userId, int roleId)
        {
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == userId);

            if (user != null)
            {
                user.RoleId = roleId;
                await _context.SaveChangesAsync();
                return roleId;
            }

            return 0;
        }

        public async Task<User?> GetUserByNamePassword(string userName, string password)
        {
            try
            {
                var user = await _context.Users
                                .Include(u => u.Role)
                                .Where(x => x.UserName == userName && x.Password == password) //.ToSha512()
                                .FirstOrDefaultAsync();

                return user;
            }
            catch (Exception ex)
            {
                //_logger.LogError($"Login exception: {ex.Message}");
                throw new InvalidCredentialException(ex.Message);
            }
        }
    }
}
