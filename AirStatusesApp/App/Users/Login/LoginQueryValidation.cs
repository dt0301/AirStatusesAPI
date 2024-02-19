using FluentValidation;

namespace AirStatusesApp.App.Users.Login
{
    /// <summary>
    /// Валидатор для проверки учетных данных пользователя при входе в систему.
    /// </summary>
    public class LoginQueryValidation : AbstractValidator<CredentialDto>
    {
        /// <summary>
        /// Конструктор валидатора, устанавливающий правила валидации.
        /// </summary>
        public LoginQueryValidation()
        {
            // Правило валидации: имя пользователя не должно быть пустым.
            RuleFor(x => x.UserName).NotEmpty();

            // Правило валидации: пароль не должен быть пустым.
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
