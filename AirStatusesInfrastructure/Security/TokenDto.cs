namespace AirStatusesInfrastructure.Security
{
    /// <summary>
    /// Класс TokenDto представляет собой модель данных для информации о токене.
    /// </summary>
    public class TokenDto
    {
        /// <summary>
        /// Токен, который может быть использован для аутентификации или других целей.
        /// </summary>
        public string Token { get; set; }
    }
}
