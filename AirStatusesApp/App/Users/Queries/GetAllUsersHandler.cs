using AirStatusesData.Services;
using AirStatusesData.Services.Dto;
using AirStatusesDomain;
using MediatR;

namespace AirStatusesApp.App.Users.Queries
{
    /// <summary>
    /// Обработчик запроса на получение всех пользователей. Возвращает список DTO пользователей.
    /// </summary>
    public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, List<UserDto>>
    {
        private readonly IUserDataService _userService;

        /// <summary>
        /// Конструктор обработчика запроса на получение всех пользователей.
        /// </summary>
        /// <param name="_userService">Сервис данных пользователя.</param>
        public GetAllUsersHandler(IUserDataService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Обрабатывает запрос на получение всех пользователей.
        /// </summary>
        /// <param name="request">Запрос на получение всех пользователей.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Возвращает список DTO пользователей.</returns>
        public async Task<List<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            return await _userService.GetAllUsersAsync();
        }
    }
}

