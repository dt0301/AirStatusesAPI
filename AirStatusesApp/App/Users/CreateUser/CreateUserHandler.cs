using AirStatusesData.Services;
using MediatR;
using AirStatusesDomain;

namespace AirStatusesApp.App.Users.CreateUser
{
    /// <summary>
    /// Обработчик команды для создания нового пользователя.
    /// </summary>
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, int>
    {
        private readonly IUserDataService _userService;
        //private readonly ICache _cache;

        /// <summary>
        /// Конструктор обработчика команды CreateUserHandler.
        /// </summary>
        /// <param name="userService">Сервис для работы с данными пользователя.</param>
        public CreateUserHandler(IUserDataService userService/*, ICache cache*/)
        {
            _userService = userService;
            //_cache = cache;
        }

        /// <summary>
        /// Обрабатывает команду создания нового пользователя.
        /// </summary>
        /// <param name="request">Команда для создания нового пользователя.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Возвращает идентификатор созданного пользователя.</returns>
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
