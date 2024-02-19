using AirStatusesData.Services;
using AirStatusesData.Services.Dto;
using AirStatusesDomain;
using MediatR;

namespace AirStatusesApp.App.Users.Queries
{
    public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, List<UserDto>>
    {
        private readonly IUserDataService _userService;

        public GetAllUsersHandler(IUserDataService userService)
        {
            _userService = userService;
        }

        public async Task<List<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            return await _userService.GetAllUsersAsync();
        }
    }
}
