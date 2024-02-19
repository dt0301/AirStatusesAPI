using AirStatusesDomain;

namespace AirStatusesInfrastructure.Security
{
    /// <summary>
    /// Интерфейс для генерации JWT (JSON Web Token).
    /// </summary>
    public interface IJwtGenerator
    {
        /// <summary>
        /// Создает JWT для указанного пользователя.
        /// </summary>
        /// <param name="user">Пользователь, для которого создается токен.</param>
        /// <returns>Строка, представляющая собой JWT.</returns>
        TokenDto? GenerateJwtToken(User user);
        bool ValidateToken(string token);
    }
}
