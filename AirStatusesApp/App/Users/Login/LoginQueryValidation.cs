using FluentValidation;

namespace AirStatusesApp.App.Users.Login
{
    public class LoginQueryValidation : AbstractValidator<CredentialDto>
    {
        public LoginQueryValidation()
        {
            RuleFor(x => x.UserName).NotEmpty();

            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
