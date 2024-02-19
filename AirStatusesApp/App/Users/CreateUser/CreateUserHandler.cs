using AirStatusesData.Services;
using MediatR;
using AirStatusesDomain;

namespace AirStatusesApp.App.Users.CreateUser
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, int>
    {
        private readonly IUserDataService _userService;
        //private readonly ICache _cache;
        public CreateUserHandler(IUserDataService userService/*, ICache cache*/)
        {
            _userService = userService;
            //_cache = cache;
        }

        public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            User user = new User();
            user.UserName = request.Username;
            user.Password = request.Password;
            user.RoleId = request.RoleId;
            var newUserId = await _userService.AddUserAsync(user);
            return newUserId;
        }
    }
}
