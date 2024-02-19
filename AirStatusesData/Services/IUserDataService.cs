
using AirStatusesData.Services.Dto;
using AirStatusesDomain;

namespace AirStatusesData.Services
{
    /// <summary>
    /// Интерфейс IUserDataService представляет собой контракт для службы работы с данными о пользователях.
    /// </summary>
    public interface IUserDataService
    {
        /// <summary>
        /// Добавляет нового пользователя.
        /// </summary>
        /// <param name="user">Объект пользователя для добавления.</param>
        /// <returns>Возвращает задачу, результатом которой является идентификатор добавленного пользователя.</returns>
        Task<int> AddUserAsync(User user);

        /// <summary>
        /// Получает список всех пользователей.
        /// </summary>
        /// <returns>Возвращает задачу, результатом которой является список всех пользователей.</returns>
        Task<List<UserDto>> GetAllUsersAsync();

        /// <summary>
        /// Получает пользователя по его идентификатору.
        /// </summary>
        /// <param name="Id">Идентификатор пользователя.</param>
        /// <returns>Возвращает задачу, результатом которой является пользователь или null, если пользователь не найден.</returns>
        Task<UserDto?> GetUserByIdAsync(int Id);

        /// <summary>
        /// Устанавливает роль пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="roleId">Идентификатор роли.</param>
        /// <returns>Возвращает задачу, результатом которой является идентификатор установленной роли.</returns>
        Task<int> SetUserRoleAsync(int userId, int roleId);

        /// <summary>
        /// Получает пользователя по его имени и паролю.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <param name="password">Пароль пользователя.</param>
        /// <returns>Возвращает задачу, результатом которой является пользователь или null, если пользователь не найден.</returns>
        Task<User?> GetUserByNamePassword(string userName, string password);
    }
}

