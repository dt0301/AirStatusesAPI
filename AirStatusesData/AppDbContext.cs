using AirStatusesDomain;
using Microsoft.EntityFrameworkCore;

namespace AirStatusesData
{
    /// <summary>
    /// Класс AppDbContext представляет контекст базы данных, используемый для взаимодействия с базой данных.
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Конструктор класса AppDbContext.
        /// </summary>
        /// <param name="options">Параметры контекста базы данных.</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        /// <summary>
        /// Свойство DbSet для работы с данными о рейсах.
        /// </summary>
        public DbSet<Flight> Flights { get; set; }

        /// <summary>
        /// Свойство DbSet для работы с данными о ролях.
        /// </summary>
        public DbSet<Role> Roles { get; set; }

        /// <summary>
        /// Свойство DbSet для работы с данными о пользователях.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Метод для настройки модели базы данных.
        /// </summary>
        /// <param name="modelBuilder">Строитель модели для создания модели базы данных.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Создание уникального индекса для UserName в таблице User
            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique();

            // Создание уникального индекса для Code в таблице Role
            modelBuilder.Entity<Role>()
                .HasIndex(r => r.Code)
                .IsUnique();

            // Установка генерации значения Id при добавлении новой записи в таблицу Role
            modelBuilder.Entity<Role>()
                .Property(r => r.Id)
                .ValueGeneratedOnAdd();

            // Установка генерации значения Id при добавлении новой записи в таблицу User
            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd();

            // Установка генерации значения Id при добавлении новой записи в таблицу Flight
            modelBuilder.Entity<Flight>()
                .Property(f => f.Id)
                .ValueGeneratedOnAdd();
        }
    }
}
