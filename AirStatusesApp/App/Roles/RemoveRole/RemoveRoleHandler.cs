using AirStatusesData.Services;
using MediatR;

namespace AirStatusesApp.App.Roles.RemoveRole
{
    public class RemoveRoleHandler : IRequestHandler<RemoveRoleCommand, bool>
    {
        private readonly IRolesDataSevice _rolesDataSevice;

        /// <summary>
        /// Конструктор обработчика удаления роли.
        /// </summary>
        /// <param name="rolesDataSevice">Сервис данных ролей.</param>
        public RemoveRoleHandler(IRolesDataSevice rolesDataSevice)
        {
            _rolesDataSevice = rolesDataSevice;
        }

        /// <summary>
        /// Обрабатывает команду удаления роли.
        /// </summary>
        /// <param name="request">Команда удаления роли.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Результат удаления роли.</returns>
        public Task<bool> Handle(RemoveRoleCommand request, CancellationToken cancellationToken)
        {
            return _rolesDataSevice.DeleteRoleAsync(request.RoleId);
        }
    }
}
