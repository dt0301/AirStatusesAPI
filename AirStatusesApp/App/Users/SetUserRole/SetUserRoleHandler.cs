using AirStatusesData.Services;
using MediatR;

namespace AirStatusesApp.App.Users.SetUserRole
{
    public class SetUserRoleHandler : IRequestHandler<SetUserRoleCommand, int>
    {
        private readonly IUserDataService _userService;
        //private readonly ICache _cache;
        public SetUserRoleHandler(IUserDataService userService/*, ICache cache*/)
        {
            _userService = userService;
            //_cache = cache;
        }

        public Task<int> Handle(SetUserRoleCommand request, CancellationToken cancellationToken)
        {
            return _userService.SetUserRoleAsync(request.UserId, request.RoleId);
        }
    }
}
