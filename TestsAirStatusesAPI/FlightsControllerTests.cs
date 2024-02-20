using Microsoft.Extensions.Logging;
using AirStatusesAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using AirStatusesApp.App.Helpers;
using AirStatusesApp.App.Flights.Queries;
using AirStatusesDomain;
using AirStatusesApp.App.Flights.CreateFlight;
using AirStatusesData.Services.Dto;
using AirStatusesApp.App.Flights.UpdateFlight;
using Moq;
using ILogger = Serilog.ILogger;
using System.Reflection.PortableExecutable;

namespace TestsAirStatusesAPI
{
    public class FlightsControllerTests
    {
        private readonly Mock<IFligtsQuery> _flightQueryMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IUserProps> _userPropsMock;
        private readonly Mock<ILogger> _loggerMock;
        private readonly FlightsController _controller;

        public FlightsControllerTests()
        {
            _flightQueryMock = new Mock<IFligtsQuery>();
            _mediatorMock = new Mock<IMediator>();
            _userPropsMock = new Mock<IUserProps>();
            _loggerMock = new Mock<ILogger>();
            _controller = new FlightsController(_flightQueryMock.Object, _mediatorMock.Object, _userPropsMock.Object, _loggerMock.Object);
        }

        [Fact]
        ///<summary>
        /// Тест проверяет, что метод GetFlights возвращает список всех рейсов.
        ///</summary>
        public async Task GetFlights_ReturnsAllFlights()
        {
            // Arrange
            var flights = new List<Flight> { new Flight(), new Flight() };
            _flightQueryMock.Setup(f => f.GetFlightsAsync()).ReturnsAsync(flights);

            // Act
            var result = await _controller.GetFlights();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Flight>>(okResult.Value);
            Assert.Equal(flights.Count, returnValue.Count);
        }

        [Fact]
        ///<summary>
        /// Тест проверяет, что метод GetFlight возвращает рейс по его ID.
        /// Входные данные: ID рейса.
        /// Выходные данные: Рейс с указанным ID или NotFound, если рейс не найден.
        ///</summary>
        public async Task GetFlight_ReturnsFlight_WhenFlightExists()
        {
            // Arrange
            var flight = new Flight { Id = 1 };
            _flightQueryMock.Setup(f => f.GetFlightByIdAsync(1)).ReturnsAsync(flight);

            // Act
            var result = await _controller.GetFlight(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Flight>(okResult.Value);
            Assert.Equal(flight.Id, returnValue.Id);
        }

        //[Fact]
        /////<summary>
        ///// Тест проверяет, что метод CreateFlight создает новый рейс, если пользователь имеет права на запись или является администратором.
        ///// Входные данные: Команда создания рейса и пользователь с правами на запись или администратор.
        ///// Выходные данные: ID нового рейса или Unauthorized, если у пользователя нет прав на создание рейса.
        /////</summary>
        //public async Task CreateFlight_CreatesFlight_WhenUserIsWriterOrAdmin()
        //{
        //    // Arrange
        //    var command = new CreateFlightCommand();
        //    var userDto = new UserDto { RoleCode = "Writer" };
        //    _userPropsMock.Setup(u => u.GetUserPropsAsync()).ReturnsAsync(userDto);
        //    _userPropsMock.Setup(u => u.IsWriter(userDto)).Returns(true);
        //    _mediatorMock.Setup(m => m.Send(command)).ReturnsAsync(1);

        //    // Act
        //    var result = await _controller.CreateFlight(command);

        //    // Assert
        //    var okResult = Assert.IsType<OkObjectResult>(result);
        //    var returnValue = Assert.IsType<int>(okResult.Value);
        //    Assert.Equal(1, returnValue);
        //}

        //[Fact]
        /////<summary>
        ///// Тест проверяет, что метод UpdateFlight обновляет рейс, если пользователь имеет права на запись или является администратором.
        ///// Входные данные: ID рейса, команда обновления рейса и пользователь с правами на запись или администратор.
        ///// Выходные данные: NoContent, если рейс успешно обновлен, NotFound, если рейс не найден, или Unauthorized, если у пользователя нет прав на обновление рейса.
        /////</summary>
        //public async Task UpdateFlight_UpdatesFlight_WhenUserIsWriterOrAdmin()
        //{
        //    // Arrange
        //    var command = new UpdateFlightCommand();
        //    var userDto = new UserDto { RoleCode = "Writer" };
        //    _userPropsMock.Setup(u => u.GetUserPropsAsync()).ReturnsAsync(userDto);
        //    _userPropsMock.Setup(u => u.IsWriter(userDto)).Returns(true);
        //    _mediatorMock.Setup(m => m.Send(command)).ReturnsAsync(new Flight());

        //    // Act
        //    var result = await _controller.UpdateFlight(1, command);

        //    // Assert
        //    var noContentResult = Assert.IsType<NoContentResult>(result);
        //}

        //[Fact]
        /////<summary>
        ///// Тест проверяет, что метод UpdateFlight возвращает NotFound, если рейс не найден.
        ///// Входные данные: ID рейса, команда обновления рейса и пользователь с правами на запись или администратор.
        ///// Выходные данные: NotFound, если рейс не найден.
        /////</summary>
        //public async Task UpdateFlight_ReturnsNotFound_WhenFlightDoesNotExist()
        //{
        //    // Arrange
        //    var command = new UpdateFlightCommand();
        //    var userDto = new UserDto { RoleCode = "Writer" };
        //    _userPropsMock.Setup(u => u.GetUserPropsAsync()).ReturnsAsync(userDto);
        //    _userPropsMock.Setup(u => u.IsWriter(userDto)).Returns(true);
        //    _mediatorMock.Setup(m => m.Send(command)).ReturnsAsync((Flight)null);

        //    // Act
        //    var result = await _controller.UpdateFlight(1, command);

        //    // Assert
        //    Assert.IsType<NotFoundResult>(result);
        //}

        [Fact]
        ///<summary>
        /// Тест проверяет, что метод UpdateFlight возвращает Unauthorized, если у пользователя нет прав на обновление рейса.
        /// Входные данные: ID рейса, команда обновления рейса и пользователь без прав на запись.
        /// Выходные данные: Unauthorized, если у пользователя нет прав на обновление рейса.
        ///</summary>
        public async Task UpdateFlight_ReturnsUnauthorized_WhenUserIsNotWriterOrAdmin()
        {
            // Arrange
            var command = new UpdateFlightCommand();
            var userDto = new UserDto { RoleCode = "Reader" };
            _userPropsMock.Setup(u => u.GetUserPropsAsync()).ReturnsAsync(userDto);
            _userPropsMock.Setup(u => u.IsWriter(userDto)).Returns(false);

            // Act
            var result = await _controller.UpdateFlight(1, command);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        ///<summary>
        /// Тест проверяет, что метод CreateFlight возвращает Unauthorized, если у пользователя нет прав на создание рейса.
        /// Входные данные: Команда создания рейса и пользователь без прав на запись.
        /// Выходные данные: Unauthorized, если у пользователя нет прав на создание рейса.
        ///</summary>
        public async Task CreateFlight_ReturnsUnauthorized_WhenUserIsNotWriterOrAdmin()
        {
            // Arrange
            var command = new CreateFlightCommand();
            var userDto = new UserDto { RoleCode = "Reader" };
            _userPropsMock.Setup(u => u.GetUserPropsAsync()).ReturnsAsync(userDto);
            _userPropsMock.Setup(u => u.IsWriter(userDto)).Returns(false);

            // Act
            var result = await _controller.CreateFlight(command);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        ///<summary>
        /// Тест проверяет, что метод GetFlight возвращает NotFound, если рейс не найден.
        /// Входные данные: ID рейса.
        /// Выходные данные: NotFound, если рейс не найден.
        ///</summary>
        public async Task GetFlight_ReturnsNotFound_WhenFlightDoesNotExist()
        {
            // Arrange
            _flightQueryMock.Setup(f => f.GetFlightByIdAsync(1)).ReturnsAsync((Flight)null);

            // Act
            var result = await _controller.GetFlight(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}