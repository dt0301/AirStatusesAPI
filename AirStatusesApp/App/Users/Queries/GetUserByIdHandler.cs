using AirStatusesData.Services;
using AirStatusesData.Services.Dto;
using AirStatusesDomain;
using MediatR;

namespace AirStatusesApp.App.Users.Queries
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserDto>
    {
        private readonly IUserDataService _userService;

        public GetUserByIdHandler(IUserDataService userService)
        {
            _userService = userService;
        }

        public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            return await _userService.GetUserByIdAsync(request.Id);
        }
    }
}
