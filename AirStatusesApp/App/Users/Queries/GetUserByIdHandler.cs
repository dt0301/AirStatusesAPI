using AirStatusesData.Services;
using AirStatusesData.Services.Dto;
using MediatR;

namespace AirStatusesApp.App.Users.Queries
{
    /// <summary>
    /// Обработчик запроса на получение пользователя по идентификатору. Возвращает DTO пользователя.
    /// </summary>
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserDto>
    {
        private readonly IUserDataService _userService;

        /// <summary>
        /// Конструктор обработчика запроса на получение пользователя по идентификатору.
        /// </summary>
        /// <param name="_userService">Сервис данных пользователя.</param>
        public GetUserByIdHandler(IUserDataService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Обрабатывает запрос на получение пользователя по идентификатору.
        /// </summary>
        /// <param name="request">Запрос на получение пользователя по идентификатору.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Возвращает DTO пользователя, если пользователь с таким идентификатором существует. В противном случае возвращает null.</returns>
        public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            return await _userService.GetUserByIdAsync(request.Id);
        }
    }
}

