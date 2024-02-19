using AirStatusesData.Services.Dto;
using AirStatusesDomain;
using MediatR;

namespace AirStatusesApp.App.Users.Queries
{
    public class GetUserByIdQuery : IRequest<UserDto>
    {
        public int Id { get; set; }
    }
}
