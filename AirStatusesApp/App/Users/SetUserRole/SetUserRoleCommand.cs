using MediatR;

namespace AirStatusesApp.App.Users.SetUserRole
{
    public class SetUserRoleCommand : IRequest<int>
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}
