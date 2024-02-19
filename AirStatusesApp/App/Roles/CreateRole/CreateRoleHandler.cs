using AirStatusesData.Services;
using AirStatusesDomain;
using MediatR;

namespace AirStatusesApp.App.Roles.CreateRole
{
    /// <summary>
    /// Обработчик команды создания роли.
    /// </summary>
    public class CreateRoleHandler : IRequestHandler<CreateRoleCommand, Role>
    {
        // Сервис для работы с данными о ролях.
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
        /// <returns>Возвращает созданную роль.</returns>
        public async Task<Role> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            // Создание роли с использованием сервиса данных ролей.
            return await _rolesDataSevice.CreateRoleAsync(request.RoleCode);
        }
    }
}

