using AirStatusesDomain;

namespace AirStatusesData
{
    /// <summary>
    /// Класс RoleSeeder используется для заполнения базы данных начальными данными о ролях.
    /// </summary>
    public class RoleSeeder
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Конструктор класса RoleSeeder.
        /// </summary>
        /// <param name="context">Контекст базы данных.</param>
        public RoleSeeder(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Метод SeedAsync используется для асинхронного заполнения базы данных начальными данными о ролях.
        /// </summary>
        /// <returns>Задача, представляющая асинхронную операцию.</returns>
        public async Task SeedAsync()
        {
            // Проверка, есть ли уже данные о ролях в базе данных
            if (!_context.Roles.Any())
            {
                // Добавление начальных данных о ролях в базу данных
                _context.Roles.Add(new Role { Id = 1, Code = "Reader" });
                _context.Roles.Add(new Role { Id = 2, Code = "Writer" });
                _context.Roles.Add(new Role { Id = 3, Code = "Administrator" });
                _context.Roles.Add(new Role { Id = 4, Code = "Admin" });

                // Асинхронное сохранение изменений в базе данных
                await _context.SaveChangesAsync();
            }
        }
    }
}

