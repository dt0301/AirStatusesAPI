using AirStatusesApp.App.Helpers;
using AirStatusesApp.App.Users.CreateUser;
using AirStatusesApp.App.Users.Login;
using AirStatusesApp.App.Users.Queries;
using AirStatusesApp.App.Users.SetUserRole;
using AirStatusesData.Services.Dto;
using AirStatusesInfrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace AirStatusesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserProps _userProps;
        private readonly ILogger _logger;

        /// <summary>
        /// Конструктор контроллера пользователя.
        /// </summary>
        /// <param name="mediator">Объект IMediator для обработки запросов.</param>
        /// <param name="userProps">Объект IUserProps для получения свойств пользователя.</param>
        /// <param name="logger">Объект ILogger для логирования.</param>
        public UserController(IMediator mediator, IUserProps userProps, ILogger logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _userProps = userProps ?? throw new ArgumentNullException(nameof(userProps));
            _logger = logger.ForContext<UserController>();
        }

        /// <summary>
        /// Получает пользователя по его ID.
        /// </summary>
        /// <param name="id">ID пользователя.</param>
        /// <returns>Возвращает UserDto пользователя или NotFound, если пользователь не найден.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            _logger.Information("Getting user with ID: {UserId}", id);

            var user = await _mediator.Send(new GetUserByIdQuery { Id = id });
            if (user == null)
            {
                _logger.Warning("User with ID: {UserId} not found", id);
                return NotFound();
            }

            _logger.Information("Retrieved user with ID: {UserId}", id);
            return Ok(user);
        }

        /// <summary>
        /// Получает всех пользователей.
        /// </summary>
        /// <returns>Возвращает список всех UserDto пользователей.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<UserDto>>> GetAllUsers()
        {
            _logger.Information("Getting all users");

            var usersDto = await _mediator.Send(new GetAllUsersQuery());

            _logger.Information("Retrieved all users");
            return Ok(usersDto.OrderBy(u => u.Id));
        }

        /// <summary>
        /// Авторизует пользователя.
        /// </summary>
        /// <param name="credentialDto">Учетные данные пользователя.</param>
        /// <returns>Возвращает TokenDto с токеном пользователя или BadRequest, если авторизация не удалась.</returns>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TokenDto>> Login(CredentialDto credentialDto)
        {
            _logger.Information("Logging in user");

            var token = await _mediator.Send(credentialDto);
            if (string.IsNullOrEmpty(token.Token))
            {
                _logger.Warning($"Failed to log in user: {credentialDto.UserName}");
                return BadRequest();
            }

            _logger.Information($"User logged in successfully: {credentialDto.UserName}");
            return Ok(token);
        }

        /// <summary>
        /// Создает нового пользователя.
        /// </summary>
        /// <param name="createUserCommand">Команда создания пользователя.</param>
        /// <returns>Возвращает ID созданного пользователя или Unauthorized, если текущий пользователь не имеет прав на создание.</returns>
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> CreateUser(CreateUserCommand createUserCommand)
        {
            _logger.Information("Creating a new user");

            var userDto = await _userProps.GetUserPropsAsync();
            if (userDto == null && !_userProps.IsAdmin(userDto))
            {
                _logger.Warning("Unauthorized attempt to create a user");
                return Unauthorized();
            }

            var userId = await _mediator.Send(createUserCommand);

            _logger.Information("Created a new user with ID: {UserId}", userId);
            return Ok(userId);
        }

        /// <summary>
        /// Устанавливает роль пользователя.
        /// </summary>
        /// <param name="setUserRoleCommand">Команда установки роли пользователя.</param>
        /// <returns>Возвращает ID установленной роли или Unauthorized, если текущий пользователь не имеет прав на установку роли.</returns>
        [HttpPost("setRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> SetUserRole(SetUserRoleCommand setUserRoleCommand)
        {
            _logger.Information("Setting user role");
            var userDto = await _userProps.GetUserPropsAsync();
            if (userDto == null && !_userProps.IsAdmin(userDto))
            {
                _logger.Warning("Unauthorized attempt to set user role");
                return Unauthorized();
            }

            var roleId = await _mediator.Send(setUserRoleCommand);

            _logger.Information("Set user role with ID: {RoleId}", roleId);
            return Ok(roleId);
        }
    }
}

