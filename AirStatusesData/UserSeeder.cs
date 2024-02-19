using AirStatusesDomain;
using Newtonsoft.Json;

namespace AirStatusesData
{
    /// <summary>
    /// Класс UserSeeder используется для заполнения базы данных начальными данными о пользователях.
    /// </summary>
    public class UserSeeder
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Конструктор класса UserSeeder.
        /// </summary>
        /// <param name="context">Контекст базы данных.</param>
        public UserSeeder(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Метод Seed используется для заполнения базы данных начальными данными о пользователях.
        /// </summary>
        public void Seed()
        {
            // Проверка, есть ли уже данные о пользователях в базе данных
            if (!_context.Users.Any())
            {
                // Чтение данных о пользователях из JSON-файла
                var usersData = ReadUsersFromJson();
                // Добавление данных о пользователях в базу данных
                _context.Users.AddRange(usersData);
                // Сохранение изменений в базе данных
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Метод ReadUsersFromJson используется для чтения данных о пользователях из JSON-файла.
        /// </summary>
        /// <returns>Список пользователей.</returns>
        private List<User> ReadUsersFromJson()
        {
            // Путь к JSON-файлу с данными о пользователях
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "users.json");
            // Чтение JSON-файла
            var json = File.ReadAllText(path);
            // Десериализация JSON-файла в список пользователей
            var users = JsonConvert.DeserializeObject<List<User>>(json);
            return users;
        }
    }
}