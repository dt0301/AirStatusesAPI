using AirStatusesData.Services.Dto;
using AirStatusesDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Authentication;

namespace AirStatusesData.Services
{
    /// <summary>
    /// Статический класс расширений для добавления сервиса данных пользователей в коллекцию сервисов.
    /// </summary>
    public static class UserDataExtensions
    {
        /// <summary>
        /// Добавляет сервис данных пользователей в коллекцию сервисов.
        /// </summary>
        /// <param name="services">Коллекция сервисов для добавления сервиса данных пользователей.</param>
        public static void AddUserDataService(this IServiceCollection services)
        {
            services.AddScoped<IUserDataService, UsersDataService>();
        }
    }

    /// <summary>
    /// Сервис для работы с данными пользователей.
    /// </summary>
    public class UsersDataService : IUserDataService
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Конструктор сервиса данных пользователей.
        /// </summary>
        /// <param name="context">Контекст базы данных приложения.</param>
        public UsersDataService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Добавляет пользователя асинхронно.
        /// </summary>
        /// <param name="user">Пользователь для добавления.</param>
        /// <returns>Идентификатор добавленного пользователя.</returns>
        public async Task<int> AddUserAsync(User user)
        {
            //TODO: Настроить EF to auto-increment
            var maxUserId = await _context.Users.MaxAsync(u => u.Id);
            user.Id = ++maxUserId;
            var item = await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return item.Entity.Id;
        }

        /// <summary>
        /// Получает пользователя по идентификатору асинхронно.
        /// </summary>
        /// <param name="Id">Идентификатор пользователя.</param>
        /// <returns>DTO пользователя с указанным идентификатором.</returns>
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

        /// <summary>
        /// Получает всех пользователей асинхронно.
        /// </summary>
        /// <returns>Список DTO пользователей.</returns>
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

        /// <summary>
        /// Устанавливает роль пользователя асинхронно.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="roleId">Идентификатор роли.</param>
        /// <returns>Идентификатор установленной роли.</returns>
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

        /// <summary>
        /// Получает пользователя по имени и паролю асинхронно.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <param name="password">Пароль пользователя.</param>
        /// <returns>Пользователь с указанным именем и паролем.</returns>
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

