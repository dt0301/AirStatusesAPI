using AirStatusesDomain;
using MediatR;

namespace AirStatusesApp.App.Roles.GetRoles
{
    public class GetRoleByIdQuery : IRequest<Role>
    {
        public int roleId { get; set; }
    }
}
