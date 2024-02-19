using AirStatusesApp.App.Dto;
using MediatR;

namespace AirStatusesApp.App.Users.Login
{
    public class CredentialDto : IRequest<TokenDto>
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
