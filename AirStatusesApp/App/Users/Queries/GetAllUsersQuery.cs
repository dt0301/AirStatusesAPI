using AirStatusesData.Services.Dto;
using MediatR;

namespace AirStatusesApp.App.Users.Queries
{
    public class GetAllUsersQuery : IRequest<List<UserDto>>
    {
    }
}
