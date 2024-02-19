using AirStatusesDomain;
using MediatR;

namespace AirStatusesApp.App.Roles.CreateRole
{
    public class CreateRoleCommand : IRequest<Role>
    {
        public string RoleCode { get; set; }
    }
}
