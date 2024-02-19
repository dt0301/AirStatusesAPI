namespace AirStatusesData.Services.Dto
{
    /// <summary>
    /// Класс UserDto представляет собой модель данных пользователя для службы.
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Получает или задает идентификатор пользователя.
        /// </summary>
        /// <value>Целочисленное значение, представляющее идентификатор пользователя.</value>
        public int Id { get; set; }

        /// <summary>
        /// Получает или задает имя пользователя.
        /// </summary>
        /// <value>Строка, представляющая имя пользователя.</value>
        public string UserName { get; set; }

        /// <summary>
        /// Получает или задает код роли пользователя.
        /// </summary>
        /// <value>Строка, представляющая код роли пользователя.</value>
        public string RoleCode { get; set; }
    }
}
