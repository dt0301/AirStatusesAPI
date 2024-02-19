using AirStatusesData.Services.Dto;
using MediatR;

namespace AirStatusesApp.App.Users.Queries
{
    /// <summary>
    /// Запрос на получение пользователя по его идентификатору.
    /// </summary>
    public class GetUserByIdQuery : IRequest<UserDto>
    {
        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public int Id { get; set; }
    }
}
