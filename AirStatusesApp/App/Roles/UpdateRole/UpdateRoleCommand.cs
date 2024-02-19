using AirStatusesDomain;
using MediatR;

namespace AirStatusesApp.App.Roles.UpdateRole
{
    public class UpdateRoleCommand : IRequest<Role>
    {
        public int RoleId { get; set; }
        public string RoleCode { get; set; }
    }
}
