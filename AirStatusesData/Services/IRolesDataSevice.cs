using AirStatusesDomain;

namespace AirStatusesData.Services
{
    /// <summary>
    /// Интерфейс IRolesDataSevice представляет собой контракт для службы работы с данными о ролях.
    /// </summary>
    public interface IRolesDataSevice
    {
        /// <summary>
        /// Получает список всех ролей.
        /// </summary>
        /// <returns>Возвращает задачу, результатом которой является список всех ролей.</returns>
        Task<IEnumerable<Role>> GetAllRolesAsync();

        /// <summary>
        /// Получает роль по ее идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор роли.</param>
        /// <returns>Возвращает задачу, результатом которой является роль или null, если роль не найдена.</returns>
        Task<Role> GetRoleByIdAsync(int id);

        /// <summary>
        /// Создает новую роль.
        /// </summary>
        /// <param name="roleCode">Код роли для создания.</param>
        /// <returns>Возвращает задачу, результатом которой является созданная роль.</returns>
        Task<Role> CreateRoleAsync(string roleCode);

        /// <summary>
        /// Обновляет роль.
        /// </summary>
        /// <param name="roleCode">Новый код роли.</param>
        /// <param name="roleId">Идентификатор роли для обновления.</param>
        /// <returns>Возвращает задачу, результатом которой является обновленная роль или null, если роль не найдена.</returns>
        Task<Role> UpdateRoleAsync(string roleCode, int roleId);

        /// <summary>
        /// Удаляет роль.
        /// </summary>
        /// <param name="id">Идентификатор роли для удаления.</param>
        /// <returns>Возвращает задачу, результатом которой является булево значение, указывающее, была ли роль успешно удалена.</returns>
        Task<bool> DeleteRoleAsync(int id);
    }
}

