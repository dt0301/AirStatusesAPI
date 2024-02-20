namespace AirStatusesAPI.Providers
{
    public class JwtUserDto
    {
        /// <summary>
        /// Конструктор, создающий новый объект JwtUserDto с заданными параметрами.
        /// </summary>
        /// <param name="userId">Целочисленный идентификатор пользователя.</param>
        /// <param name="userName">Строковое имя пользователя.</param>
        /// <param name="role">Строковая роль пользователя.</param>
        public JwtUserDto(int userId, string userName, string role)
        {
            UserId = userId;
            UserName = userName;
            Role = role;
        }

        /// <summary>
        /// Пустой конструктор, создающий новый объект JwtUserDto без параметров.
        /// </summary>
        public JwtUserDto() { }

        /// <summary>
        /// Свойство для получения имени пользователя. Значение устанавливается только внутри класса.
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// Свойство для получения идентификатора пользователя. Значение устанавливается только внутри класса.
        /// </summary>
        public int UserId { get; private set; }

        /// <summary>
        /// Свойство для получения роли пользователя. Значение устанавливается только внутри класса.
        /// </summary>
        public string Role { get; private set; }
    }

}