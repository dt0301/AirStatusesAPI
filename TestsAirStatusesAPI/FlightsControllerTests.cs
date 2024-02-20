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
    /// ����������� ��� ������ FlightsControllerTests.
    /// </summary>
    /// <param name="_flightQuery">������ IFligtsQuery ��� ���������� �������� � ������ � ������.</param>
    /// <param name="_mediator">������ IMediator ��� ��������� ������ � ��������.</param>
    /// <param name="_userProps">������ IUserProps ��� ������ � ������� ������������.</param>
    /// <param name="_logger">������ ILogger ��� ������� �������.</param>
    public FlightsControllerTests()
    {
        _controller = new FlightsController(_flightQuery, _mediator, _userProps, _logger);
    }

    /// <summary>
    /// ���� ���������, ��� ����� GetFlights ���������� OkResult, ����� ���� ��������� �����.
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
    /// ���� ���������, ��� ����� GetFlight ���������� NotFoundResult, ����� ����� �� ����������.
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
    /// ���� ���������, ��� ����� CreateFlight ���������� UnauthorizedResult, ����� ������������ �� �������� Writer ��� Admin.
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
    /// ���� ���������, ��� ����� UpdateFlight ���������� NotFoundResult, ����� ����� �� ����������.
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
    /// ���� ���������, ��� ����� UpdateFlight ���������� NoContentResult, ����� ���������� ������ �������.
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
    /// ���� ���������, ��� ����� CreateFlight ���������� OkObjectResult, ����� �������� ����� ������ �������.
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
    /// ���� ���������, ��� ����� GetFlight ���������� OkResult, ����� ���� ����������.
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
    /// ���� ���������, ��� ����� UpdateFlight ���������� UnauthorizedResult, ����� ������������ �� �������� Writer ��� Admin.
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
