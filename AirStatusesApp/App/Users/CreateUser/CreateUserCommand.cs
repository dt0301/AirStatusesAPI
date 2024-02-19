using MediatR;
using System.ComponentModel.DataAnnotations;

namespace AirStatusesApp.App.Users.CreateUser
{
    /// <summary>
    /// Команда для создания нового пользователя.
    /// </summary>
    public class CreateUserCommand : IRequest<int>
    {
        /// <summary>
        /// Имя пользователя. Максимальная длина - 256 символов.
        /// </summary>
        [MaxLength(256)]
        public string Username { get; set; }

        /// <summary>
        /// Пароль пользователя. Максимальная длина - 256 символов.
        /// </summary>
        [MaxLength(256)]
        public string Password { get; set; }

        /// <summary>
        /// Идентификатор роли пользователя.
        /// </summary>
        public int RoleId { get; set; }
    }
}



