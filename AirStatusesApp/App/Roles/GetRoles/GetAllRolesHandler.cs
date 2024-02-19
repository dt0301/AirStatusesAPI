using AirStatusesData.Services;
using AirStatusesDomain;
using MediatR;

namespace AirStatusesApp.App.Roles.GetRoles
{
    public class GetAllRolesHandler : IRequestHandler<GetAllRolesQuery, List<Role>>
    {
        private readonly IRolesDataSevice _rolesDataSevice;

        /// <summary>
        /// Конструктор обработчика получения всех ролей.
        /// </summary>
        /// <param name="rolesDataSevice">Сервис данных ролей.</param>
        public GetAllRolesHandler(IRolesDataSevice rolesDataSevice)
        {
            _rolesDataSevice = rolesDataSevice;
        }

        /// <summary>
        /// Обрабатывает запрос на получение всех ролей.
        /// </summary>
        /// <param name="request">Запрос на получение всех ролей.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Список всех ролей.</returns>
        public async Task<List<Role>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await _rolesDataSevice.GetAllRolesAsync();
            return roles.ToList();
        }
    }
}
