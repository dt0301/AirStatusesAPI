using AirStatusesDomain;
using MediatR;

namespace AirStatusesApp.App.Roles.CreateRole
{
    /// <summary>
    /// Команда для создания новой роли.
    /// </summary>
    public class CreateRoleCommand : IRequest<Role>
    {
        /// <summary>
        /// Код роли, который нужно создать.
        /// </summary>
        public string RoleCode { get; set; }
    }
}

