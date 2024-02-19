using MediatR;

namespace AirStatusesApp.App.Roles.RemoveRole
{
    public class RemoveRoleCommand :IRequest<bool>
    {
        public int RoleId { get; set; }
    }
}
