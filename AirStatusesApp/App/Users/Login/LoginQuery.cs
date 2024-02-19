using AirStatusesInfrastructure.Security;
using MediatR;

namespace AirStatusesApp.App.Users.Login
{
    /// <summary>
    /// DTO (Data Transfer Object) для учетных данных пользователя. Используется при входе в систему.
    /// </summary>
    public class CredentialDto : IRequest<TokenDto>
    {
        /// <summary>
        /// Имя пользователя для входа в систему.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Пароль пользователя для входа в систему.
        /// </summary>
        public string Password { get; set; }
    }
}
