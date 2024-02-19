using AirStatusesData.Services;
using AirStatusesDomain;
using MediatR;

namespace AirStatusesApp.App.Roles.UpdateRole
{
    public class UpdateRoleHandler : IRequestHandler<UpdateRoleCommand, Role>
    {
        private readonly IRolesDataSevice _rolesDataSevice;

        /// <summary>
        /// Конструктор обработчика обновления роли.
        /// </summary>
        /// <param name="rolesDataSevice">Сервис данных ролей.</param>
        public UpdateRoleHandler(IRolesDataSevice rolesDataSevice)
        {
            _rolesDataSevice = rolesDataSevice;
        }

        /// <summary>
        /// Обрабатывает команду обновления роли.
        /// </summary>
        /// <param name="request">Команда обновления роли.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Обновленная роль.</returns>
        public async Task<Role> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            return await _rolesDataSevice.UpdateRoleAsync(request.RoleCode, request.RoleId);
        }
    }
}
