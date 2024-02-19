using AirStatusesData.Services;
using AirStatusesDomain;
using MediatR;

namespace AirStatusesApp.App.Roles.CreateRole
{
    public class CreateRoleHandler : IRequestHandler<CreateRoleCommand, Role>
    {
        private readonly IRolesDataSevice _rolesDataSevice;

        /// <summary>
        /// Конструктор обработчика создания роли.
        /// </summary>
        /// <param name="rolesDataSevice">Сервис данных ролей.</param>
        public CreateRoleHandler(IRolesDataSevice rolesDataSevice)
        {
            _rolesDataSevice = rolesDataSevice;
        }

        /// <summary>
        /// Обрабатывает команду создания роли.
        /// </summary>
        /// <param name="request">Команда создания роли.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Созданная роль.</returns>
        public async Task<Role> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            return await _rolesDataSevice.CreateRoleAsync(request.RoleCode);
        }
    }
}
