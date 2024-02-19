using MediatR;

namespace AirStatusesApp.App.Users.SetUserRole
{
    /// <summary>
    /// Команда для установки роли пользователя.
    /// </summary>
    public class SetUserRoleCommand : IRequest<int>
    {
        /// <summary>
        /// Идентификатор пользователя, для которого устанавливается роль.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Идентификатор роли, которую нужно установить пользователю.
        /// </summary>
        public int RoleId { get; set; }
    }
}

