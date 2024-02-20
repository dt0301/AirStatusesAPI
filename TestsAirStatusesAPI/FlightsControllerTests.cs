using FakeItEasy;
using AirStatusesAPI.Controllers;
using AirStatusesApp.App.Flights.CreateFlight;
using AirStatusesApp.App.Flights.Queries;
using AirStatusesApp.App.Helpers;
using AirStatusesData.Services.Dto;
using AirStatusesDomain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using AirStatusesApp.App.Flights.UpdateFlight;

public class FlightsControllerTests
{
    private readonly IFligtsQuery _flightQuery = A.Fake<IFligtsQuery>();
    private readonly IMediator _mediator = A.Fake<IMediator>();
    private readonly IUserProps _userProps = A.Fake<IUserProps>();
    private readonly ILogger _logger = A.Fake<ILogger>();

    private readonly FlightsController _controller;

    /// <summary>
    /// Конструктор для класса FlightsControllerTests.
    /// </summary>
    /// <param name="_flightQuery">Объект IFligtsQuery для выполнения запросов к данным о рейсах.</param>
    /// <param name="_mediator">Объект IMediator для обработки команд и запросов.</param>
    /// <param name="_userProps">Объект IUserProps для работы с данными пользователя.</param>
    /// <param name="_logger">Объект ILogger для ведения журнала.</param>
    public FlightsControllerTests()
    {
        _controller = new FlightsController(_flightQuery, _mediator, _userProps, _logger);
    }

    /// <summary>
    /// Тест проверяет, что метод GetFlights возвращает OkResult, когда есть доступные рейсы.
    /// </summary>
    [Fact]
    public async Task GetFlights_ReturnsOkResult_WhenFlightsExist()
    {
        // Arrange
        A.CallTo(() => _flightQuery.GetFlightsAsync()).Returns(new List<Flight>());

        // Act
        var result = await _controller.GetFlights();

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    /// <summary>
    /// Тест проверяет, что метод GetFlight возвращает NotFoundResult, когда рейса не существует.
    /// </summary>
    //[Fact]
    //public async Task GetFlight_ReturnsNotFoundResult_WhenFlightDoesNotExist()
    //{
    //    // Arrange
    //    int flightId = 1;
    //    A.CallTo(() => _flightQuery.GetFlightByIdAsync(flightId)).ReturnsLazily(null);

    //    // Act
    //    var result = await _controller.GetFlight(flightId);

    //    // Assert
    //    Assert.IsType<NotFoundResult>(result);
    //}

    /// <summary>
    /// Тест проверяет, что метод CreateFlight возвращает UnauthorizedResult, когда пользователь не является Writer или Admin.
    /// </summary>
    [Fact]
    public async Task CreateFlight_ReturnsUnauthorizedResult_WhenUserIsNotWriterOrAdmin()
    {
        // Arrange
        var command = new CreateFlightCommand();
        A.CallTo(() => _userProps.GetUserPropsAsync()).Returns(new UserDto { RoleCode = "Reader" });

        // Act
        var result = await _controller.CreateFlight(command);

        // Assert
        Assert.IsType<UnauthorizedResult>(result);
    }

    /// <summary>
    /// Тест проверяет, что метод UpdateFlight возвращает NotFoundResult, когда рейса не существует.
    /// </summary>
    //[Fact]
    //public async Task UpdateFlight_ReturnsNotFoundResult_WhenFlightDoesNotExist()
    //{
    //    // Arrange
    //    int flightId = 1;
    //    var command = new UpdateFlightCommand();
    //    A.CallTo(() => _userProps.GetUserPropsAsync()).Returns(new UserDto { RoleCode = "Admin" });
    //    //A.CallTo(() => _mediator.Send(command)).ReturnsLazily(null);

    //    // Act
    //    var result = await _controller.UpdateFlight(flightId, command);

    //    // Assert
    //    Assert.IsType<NotFoundResult>(result);
    //}

    /// <summary>
    /// Тест проверяет, что метод UpdateFlight возвращает NoContentResult, когда обновление прошло успешно.
    /// </summary>
    //[Fact]
    //public async Task UpdateFlight_ReturnsNoContentResult_WhenUpdateIsSuccessful()
    //{
    //    // Arrange
    //    int flightId = 1;
    //    var command = new UpdateFlightCommand();
    //    var flight = new Flight();
    //    A.CallTo(() => _userProps.GetUserPropsAsync()).Returns(new UserDto { RoleCode = "Admin" });
    //    //A.CallTo(() => _mediator.Send(command)).Returns(flight);

    //    // Act
    //    var result = await _controller.UpdateFlight(flightId, command);

    //    // Assert
    //    Assert.IsType<NoContentResult>(result);
    //}

    /// <summary>
    /// Тест проверяет, что метод CreateFlight возвращает OkObjectResult, когда создание рейса прошло успешно.
    /// </summary>
    //[Fact]
    //public async Task CreateFlight_ReturnsOkObjectResult_WhenCreationIsSuccessful()
    //{
    //    // Arrange
    //    var command = new CreateFlightCommand();
    //    int newFlightId = 1;
    //    A.CallTo(() => _userProps.GetUserPropsAsync()).Returns(new UserDto { RoleCode = "Admin" });
    //    //A.CallTo(() => _mediator.Send(command)).Returns(newFlightId);

    //    // Act
    //    var result = await _controller.CreateFlight(command);

    //    // Assert
    //    Assert.IsType<OkObjectResult>(result);
    //    var okResult = result as OkObjectResult;
    //    Assert.Equal(newFlightId, okResult.Value);
    //}

    /// <summary>
    /// Тест проверяет, что метод GetFlight возвращает OkResult, когда рейс существует.
    /// </summary>
    [Fact]
    public async Task GetFlight_ReturnsOkResult_WhenFlightExists()
    {
        // Arrange
        int flightId = 1;
        var flight = new Flight();
        A.CallTo(() => _flightQuery.GetFlightByIdAsync(flightId)).Returns(flight);

        // Act
        var result = await _controller.GetFlight(flightId);

        // Assert
        Assert.IsType<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        Assert.Equal(flight, okResult.Value);
    }

    /// <summary>
    /// Тест проверяет, что метод UpdateFlight возвращает UnauthorizedResult, когда пользователь не является Writer или Admin.
    /// </summary>
    [Fact]
    public async Task UpdateFlight_ReturnsUnauthorizedResult_WhenUserIsNotWriterOrAdmin()
    {
        // Arrange
        int flightId = 1;
        var command = new UpdateFlightCommand();
        A.CallTo(() => _userProps.GetUserPropsAsync()).Returns(new UserDto { RoleCode = "Reader" });

        // Act
        var result = await _controller.UpdateFlight(flightId, command);

        // Assert
        Assert.IsType<UnauthorizedResult>(result);
    }
}
