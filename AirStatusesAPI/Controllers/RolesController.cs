using AirStatusesApp.App.Helpers;
using AirStatusesApp.App.Roles.CreateRole;
using AirStatusesApp.App.Roles.GetRoles;
using AirStatusesApp.App.Roles.RemoveRole;
using AirStatusesApp.App.Roles.UpdateRole;
using AirStatusesDomain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AirStatusesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserProps _userProps;
        private readonly Serilog.ILogger _logger;

        /// <summary>
        /// Конструктор контроллера ролей.
        /// </summary>
        /// <param name="_mediator">Объект медиатора для обработки команд и запросов.</param>
        /// <param name="_userProps">Объект для работы с свойствами пользователя.</param>
        /// <param name="_logger">Объект для логирования.</param>
        public RolesController(IMediator mediator, IUserProps userProps, Serilog.ILogger logger)
        {
            _mediator = mediator;
            _userProps = userProps;
            _logger = logger.ForContext<RolesController>(); // Для контекстного логирования
        }

        /// <summary>
        /// Создает новую роль.
        /// </summary>
        /// <param name="command">Команда создания роли.</param>
        /// <returns>Созданная роль.</returns>
        [HttpPost]
        public async Task<ActionResult<Role>> CreateRole(CreateRoleCommand command)
        {
            _logger.Information("Creating a new role");
            var role = await _mediator.Send(command);
            _logger.Information("Created a new role with ID: {RoleId}", role.Id);
            return Ok(role);
        }

        /// <summary>
        /// Получает все роли.
        /// </summary>
        /// <returns>Список всех ролей.</returns>
        [HttpGet]
        public async Task<ActionResult<List<Role>>> GetAllRoles()
        {
            _logger.Information("Getting all roles");
            var query = new GetAllRolesQuery();
            var roles = await _mediator.Send(query);
            _logger.Information("Retrieved {RoleCount} roles", roles.Count);
            return Ok(roles);
        }

        /// <summary>
        /// Получает роль по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор роли.</param>
        /// <returns>Роль с указанным идентификатором.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetRoleById(int id)
        {
            _logger.Information("Getting role with ID: {RoleId}", id);
            var query = new GetRoleByIdQuery { roleId = id };
            var role = await _mediator.Send(query);
            _logger.Information("Retrieved role with ID: {RoleId}", id);
            return Ok(role);
        }

        /// <summary>
        /// Удаляет роль по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор роли.</param>
        /// <returns>Результат удаления роли.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteRole(int id)
        {
            var userDto = await _userProps.GetUserPropsAsync();
            if (userDto != null && _userProps.IsAdmin(userDto))
            {
                _logger.Information("Deleting role with ID: {RoleId}", id);
                var command = new RemoveRoleCommand { RoleId = id };
                var result = await _mediator.Send(command);
                _logger.Information("Deleted role with ID: {RoleId}", id);
                return Ok(result);
            }
            _logger.Warning("Unauthorized attempt to delete a role");
            return Unauthorized();
        }

        /// <summary>
        /// Обновляет роль по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор роли.</param>
        /// <param name="command">Команда обновления роли.</param>
        /// <returns>Обновленная роль.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<Role>> UpdateRole(int id, UpdateRoleCommand command)
        {
            var userDto = await _userProps.GetUserPropsAsync();
            if (userDto != null && _userProps.IsAdmin(userDto))
            {
                _logger.Information("Updating role with ID: {RoleId}", id);
                command.RoleId = id;
                var role = await _mediator.Send(command);
                _logger.Information("Updated role with ID: {RoleId}", id);
                return Ok(role);
            }
            _logger.Warning("Unauthorized attempt to update a role");
            return Unauthorized();
        }
    }
}
