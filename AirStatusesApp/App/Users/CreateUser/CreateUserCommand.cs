using MediatR;
using System.ComponentModel.DataAnnotations;

namespace AirStatusesApp.App.Users.CreateUser
{
    public class CreateUserCommand : IRequest<int>
    {
        [MaxLength(256)]
        public string Username { get; set; }

        [MaxLength(256)]
        public string Password { get; set; }

        public int RoleId { get; set; }

    }
}
