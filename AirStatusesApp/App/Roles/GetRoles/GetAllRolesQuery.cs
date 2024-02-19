using AirStatusesData.Services.Dto;
using AirStatusesDomain;
using MediatR;

namespace AirStatusesApp.App.Roles.GetRoles
{
    public class GetAllRolesQuery : IRequest<List<Role>>
    {
    }
}
