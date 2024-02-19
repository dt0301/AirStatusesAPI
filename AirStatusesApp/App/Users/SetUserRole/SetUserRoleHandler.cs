using AirStatusesData.Services;
using MediatR;

namespace AirStatusesApp.App.Users.SetUserRole
{
    /// <summary>
    /// Обработчик команды для установки роли пользователя.
    /// </summary>
    public class SetUserRoleHandler : IRequestHandler<SetUserRoleCommand, int>
    {
        private readonly IUserDataService _userService;
        //private readonly ICache _cache;

        /// <summary>
        /// Конструктор обработчика команды.
        /// </summary>
        /// <param name="userService">Сервис данных пользователя.</param>
        public SetUserRoleHandler(IUserDataService userService/*, ICache cache*/)
        {
            _userService = userService;
            //_cache = cache;
        }

        /// <summary>
        /// Обрабатывает команду установки роли пользователя.
        /// </summary>
        /// <param name="request">Команда установки роли пользователя.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Возвращает идентификатор роли, установленной для пользователя.</returns>
        public Task<int> Handle(SetUserRoleCommand request, CancellationToken cancellationToken)
        {
            return _userService.SetUserRoleAsync(request.UserId, request.RoleId);
        }
    }
}

