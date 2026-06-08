using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VeloSpace.Controllers;
using VeloSpace.DTOs;
using VeloSpace.DTOs.Shippers;
using VeloSpace.Services.ShippersServices;
using Xunit;

namespace VeloSpace.Tests.Controllers;

public class ShipperControllerTests
{
    private readonly Mock<IShipperService> _shipperServiceMock;
    private readonly ShipperController _controller;

    public ShipperControllerTests()
    {
        _shipperServiceMock = new Mock<IShipperService>();
        _controller = new ShipperController(_shipperServiceMock.Object);
    }

    [Fact]
    public async Task GetById_WhenShipperExists_ShouldReturnOk()
    {
        // Arrange
        var shipperId = 1L;

        var shipperRequestDto = new ShipperRequestDTO
        {
            ShipperDto = new ShipperDTO
            {
                ShipperId = shipperId,
                Name = "Cliente Remetente LTDA",
                ShipperDocument = "12345678912345",
                Type = "PJ",
                UserAccountId = 1
            },
            UserAccountDto = new UserAccountDTO
            {
                UserAccountId = 1,
                Email = "shipper@email.com",
                Phone = "11999998888",
                UserRoleId = 1
            }
        };

        _shipperServiceMock
            .Setup(service => service.GetByIdAsync(shipperId))
            .ReturnsAsync(shipperRequestDto);

        // Act
        var result = await _controller.GetById(shipperId);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(shipperRequestDto);
    }

    [Fact]
    public async Task GetById_WhenShipperDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var shipperId = 999L;

        _shipperServiceMock
            .Setup(service => service.GetByIdAsync(shipperId))
            .ThrowsAsync(new ShipperService.NotFoundException($"Shipper with id {shipperId} not found"));

        // Act
        var result = await _controller.GetById(shipperId);

        // Assert
        var notFoundResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        notFoundResult.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task AddShipper_WhenEmailAlreadyExists_ShouldReturnConflict()
    {
        // Arrange
        var shipperRequestDto = new ShipperRequestDTO
        {
            ShipperDto = new ShipperDTO
            {
                Name = "Cliente Remetente LTDA",
                ShipperDocument = "12345678912345",
                Type = "PJ"
            },
            UserAccountDto = new UserAccountDTO
            {
                Email = "shipper@email.com",
                HashedPassword = "Senha123",
                Phone = "11999998888",
                UserRoleId = 1
            }
        };

        _shipperServiceMock
            .Setup(service => service.AddAsync(shipperRequestDto))
            .ThrowsAsync(new ShipperService.ConflictException("Email already registered"));

        // Act
        var result = await _controller.AddShipper(shipperRequestDto);

        // Assert
        var conflictResult = result.Should().BeOfType<ConflictObjectResult>().Subject;
        conflictResult.StatusCode.Should().Be(409);
    }
}