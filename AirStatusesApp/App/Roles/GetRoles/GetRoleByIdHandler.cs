using AirStatusesData.Services;
using AirStatusesDomain;
using MediatR;

namespace AirStatusesApp.App.Roles.GetRoles
{
    public class GetRoleByIdHandler : IRequestHandler<GetRoleByIdQuery, Role>
    {
        private readonly IRolesDataSevice _rolesDataSevice;

        /// <summary>
        /// Конструктор обработчика получения роли по идентификатору.
        /// </summary>
        /// <param name="rolesDataSevice">Сервис данных ролей.</param>
        public GetRoleByIdHandler(IRolesDataSevice rolesDataSevice)
        {
            _rolesDataSevice = rolesDataSevice;
        }

        /// <summary>
        /// Обрабатывает запрос на получение роли по идентификатору.
        /// </summary>
        /// <param name="request">Запрос на получение роли по идентификатору.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Роль с указанным идентификатором.</returns>
        public async Task<Role> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            return await _rolesDataSevice.GetRoleByIdAsync(request.roleId);
        }
    }
}
