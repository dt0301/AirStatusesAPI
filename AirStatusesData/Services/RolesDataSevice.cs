using AirStatusesDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AirStatusesData.Services
{
    /// <summary>
    /// Статический класс расширений для добавления сервиса данных ролей в коллекцию сервисов.
    /// </summary>
    public static class RolesDataSeviceExtensions
    {
        /// <summary>
        /// Добавляет сервис данных ролей в коллекцию сервисов.
        /// </summary>
        /// <param name="services">Коллекция сервисов для добавления сервиса данных ролей.</param>
        public static void AddRolesDataSevice(this IServiceCollection services)
        {
            services.AddScoped<IRolesDataSevice, RolesDataSevice>();
        }
    }

    /// <summary>
    /// Сервис для работы с данными ролей.
    /// </summary>
    public class RolesDataSevice : IRolesDataSevice
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Конструктор сервиса данных ролей.
        /// </summary>
        /// <param name="context">Контекст базы данных приложения.</param>
        public RolesDataSevice(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получает все роли асинхронно.
        /// </summary>
        /// <returns>Перечисление всех ролей.</returns>
        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _context.Roles.AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Получает роль по идентификатору асинхронно.
        /// </summary>
        /// <param name="id">Идентификатор роли.</param>
        /// <returns>Роль с указанным идентификатором.</returns>
        public async Task<Role> GetRoleByIdAsync(int id)
        {
            return await _context.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
        }

        /// <summary>
        /// Создает роль асинхронно.
        /// </summary>
        /// <param name="roleCode">Код роли.</param>
        /// <returns>Созданная роль.</returns>
        public async Task<Role> CreateRoleAsync(string roleCode)
        {
            //TODO: Настроить EF to auto-increment
            var maxRoleId = await _context.Roles.MaxAsync(r => r.Id);
            var role = new Role
            {
                Id = ++maxRoleId,
                Code = roleCode
            };
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
            return role;
        }

        /// <summary>
        /// Обновляет роль асинхронно.
        /// </summary>
        /// <param name="roleCode">Новый код роли.</param>
        /// <param name="roleId">Идентификатор роли для обновления.</param>
        /// <returns>Обновленная роль.</returns>
        public async Task<Role> UpdateRoleAsync(string roleCode, int roleId)
        {
            var role = await _context.Roles.FindAsync(roleId);
            if (role == null)
            {
                throw new InvalidOperationException($"Роль с Id: {roleId} не найдена, ошибка обновления");
            }
            role.Code = roleCode;
            _context.Entry(role).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return role;
        }

        /// <summary>
        /// Удаляет роль асинхронно.
        /// </summary>
        /// <param name="id">Идентификатор роли для удаления.</param>
        /// <returns>True, если роль была успешно удалена, иначе false.</returns>
        public async Task<bool> DeleteRoleAsync(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return false;
            }
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}


