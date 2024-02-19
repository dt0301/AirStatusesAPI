using AirStatusesApp.App.Flights.CreateFlight;
using AirStatusesApp.App.Flights.Queries;
using AirStatusesApp.App.Flights.UpdateFlight;
using AirStatusesApp.App.Helpers;
using AirStatusesDomain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

using Serilog;
using ILogger = Serilog.ILogger;

namespace AirStatusesAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FlightsController : ControllerBase
    {
        private readonly IFligtsQuery _flightQuery;
        private readonly IMediator _mediator;
        private readonly IUserProps _userProps;
        private readonly ILogger _logger;

        /// <summary>
        /// Конструктор контроллера FlightsController.
        /// </summary>
        /// <param name="flightQuery">Объект для выполнения запросов к данным о рейсах.</param>
        /// <param name="mediator">Объект для обработки команд и запросов.</param>
        /// <param name="userProps">Объект для получения свойств пользователя.</param>
        /// <param name="logger">Объект для логирования.</param>
        public FlightsController(IFligtsQuery flightQuery, IMediator mediator, IUserProps userProps, ILogger logger)
        {
            _flightQuery = flightQuery;
            _mediator = mediator;
            _userProps = userProps;
            _logger = logger.ForContext<FlightsController>();
        }

        /// <summary>
        /// Получает все рейсы.
        /// </summary>
        /// <returns>Возвращает список всех рейсов.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFlights()
        {
            _logger.Information("Getting all flights");

            var flights = await _flightQuery.GetFlightsAsync();

            _logger.Information("Retrieved all flights");
            return Ok(flights);
        }

        /// <summary>
        /// Получает рейс по его ID.
        /// </summary>
        /// <param name="flightId">ID рейса.</param>
        /// <returns>Возвращает рейс с указанным ID или NotFound, если рейс не найден.</returns>
        [HttpGet("{flightId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFlight(int flightId)
        {
            _logger.Information("Getting flight with ID: {FlightId}", flightId);

            var flight = await _flightQuery.GetFlightByIdAsync(flightId);
            if (flight == null)
            {
                _logger.Warning("Flight with ID: {FlightId} not found", flightId);
                return NotFound();
            }

            _logger.Information("Retrieved flight with ID: {FlightId}", flightId);
            return Ok(flight);
        }

        /// <summary>
        /// Создает новый рейс.
        /// </summary>
        /// <param name="command">Команда создания рейса.</param>
        /// <returns>Возвращает CreatedAtAction, если рейс успешно создан, или Unauthorized, если пользователь не авторизован.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateFlight(CreateFlightCommand command)
        {
            _logger.Information("Creating a new flight");

            var userDto = await _userProps.GetUserPropsAsync();
            if (userDto != null && _userProps.IsWriter(userDto))
            {
                var newFlightId = await _mediator.Send(command);
                _logger.Information("Created a new flight with ID: {FlightId}", newFlightId);

                return Ok(newFlightId);
            }

            _logger.Warning("Unauthorized attempt to create a flight");
            return Unauthorized();
        }

        /// <summary>
        /// Обновляет рейс с указанным ID.
        /// </summary>
        /// <param name="flightId">ID рейса.</param>
        /// <param name="command">Команда обновления рейса.</param>
        /// <returns>Возвращает NoContent, если рейс успешно обновлен, NotFound, если рейс не найден, или Unauthorized, если пользователь не авторизован.</returns>
        [HttpPut("{flightId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateFlight(int flightId, UpdateFlightCommand command)
        {
            _logger.Information("Updating flight with ID: {FlightId}", flightId);
            var userDto = await _userProps.GetUserPropsAsync();

            if (userDto != null && _userProps.IsWriter(userDto))
            {
                var updatedFlight = await _mediator.Send(command);
                if (updatedFlight == null)
                {
                    _logger.Warning("Flight with ID: {FlightId} not found", flightId);
                    return NotFound();
                }
                _logger.Information("Updated flight with ID: {FlightId}", flightId);
                return NoContent();
            }
            _logger.Warning("Unauthorized attempt to update a flight");
            return Unauthorized();
        }
    }
}

