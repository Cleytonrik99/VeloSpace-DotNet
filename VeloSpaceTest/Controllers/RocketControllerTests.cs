using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VeloSpace.Controllers;
using VeloSpace.DTOs.RocketDTOS;
using VeloSpace.Services.RocketServices;
using Xunit;

namespace VeloSpace.Tests.Controllers;

public class RocketControllerTests
{
    private readonly Mock<IRocketService> _rocketServiceMock;
    private readonly RocketController _controller;

    public RocketControllerTests()
    {
        _rocketServiceMock = new Mock<IRocketService>();
        _controller = new RocketController(_rocketServiceMock.Object);
    }

    [Fact]
    public async Task GetById_WhenRocketExists_ShouldReturnOk()
    {
        // Arrange
        var rocketId = 1L;

        var rocketDto = new RocketDTO
        {
            RocketId = rocketId,
            Name = "Falcon Test",
            CapacityHeight = 70,
            CapacityWidth = 12,
            CapacityLength = 30,
            CapacityWeight = 500,
            LaunchDate = DateTime.Now,
            RocketStatusId = 1
        };

        _rocketServiceMock
            .Setup(service => service.GetByIdAsync(rocketId))
            .ReturnsAsync(rocketDto);

        // Act
        var result = await _controller.GetById(rocketId);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(rocketDto);
    }

    [Fact]
    public async Task GetById_WhenRocketDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var rocketId = 999L;

        _rocketServiceMock
            .Setup(service => service.GetByIdAsync(rocketId))
            .ThrowsAsync(new RocketService.NotFoundException($"Rocket with id {rocketId} not found"));

        // Act
        var result = await _controller.GetById(rocketId);

        // Assert
        var notFoundResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        notFoundResult.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task AddRocket_WhenPayloadIsValid_ShouldReturnCreated()
    {
        // Arrange
        var rocketDto = new RocketDTO
        {
            Name = "Falcon Test",
            CapacityHeight = 70,
            CapacityWidth = 12,
            CapacityLength = 30,
            CapacityWeight = 500,
            RocketStatusId = 1
        };

        _rocketServiceMock
            .Setup(service => service.AddAsync(rocketDto))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.AddRocket(rocketDto);

        // Assert
        var createdResult = result.Should().BeOfType<CreatedResult>().Subject;
        createdResult.StatusCode.Should().Be(201);

        _rocketServiceMock.Verify(
            service => service.AddAsync(rocketDto),
            Times.Once
        );
    }
}