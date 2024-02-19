using AirStatusesData.Services;
using AirStatusesInfrastructure.Security;
using MediatR;
using StatusAirControl.Exeptions;

namespace AirStatusesApp.App.Users.Login
{
    /// TODO: Вынести в сервис Infrastructure
    /// <summary>
    /// Обработчик входа в систему, который обрабатывает учетные данные пользователя и генерирует JWT-токен.
    /// </summary>
    public class LoginHandler : IRequestHandler<CredentialDto, TokenDto>
    {
        private readonly IUserDataService _userContext;
        private readonly IJwtGenerator _jwtGenerator;

        /// <summary>
        /// Конструктор обработчика входа в систему.
        /// </summary>
        /// <param name="_userContext">Сервис данных пользователя.</param>
        /// <param name="_jwtGenerator">Сервис JWT-генерации по данным пользователя.</param>
        public LoginHandler(IUserDataService userContext, IJwtGenerator jwtGenerator)
        {
            _userContext = userContext;
            _jwtGenerator = jwtGenerator;
        }

        public async Task<TokenDto> Handle(CredentialDto request, CancellationToken cancellationToken)
        {
            try
            {
                // Валидация пользователя по логину и паролю
                // TODO: Требуется доработка, приведена в примитивном виде
                var user = await _userContext.GetUserByNamePassword(request.UserName, request.Password);

                // JWT-generation
                return _jwtGenerator.GenerateJwtToken(user);
            }
            catch (Exception ex)
            {
                throw new InvalidCredentialException(ex);
            }
        }
    }
}
