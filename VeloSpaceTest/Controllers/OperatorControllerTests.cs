using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VeloSpace.Controllers;
using VeloSpace.DTOs;
using VeloSpace.DTOs.OperatorDTOS;
using VeloSpace.Services.OperatorServices;
using Xunit;

namespace VeloSpace.Tests.Controllers;

public class OperatorControllerTests
{
    private readonly Mock<IOperatorService> _operatorServiceMock;
    private readonly OperatorController _controller;

    public OperatorControllerTests()
    {
        _operatorServiceMock = new Mock<IOperatorService>();
        _controller = new OperatorController(_operatorServiceMock.Object);
    }

    [Fact]
    public async Task GetById_WhenOperatorExists_ShouldReturnOk()
    {
        // Arrange
        var operatorId = 1L;

        var operatorRequestDto = new OperatorRequestDTO
        {
            OperatorDto = new OperatorDTO
            {
                OperatorId = operatorId,
                Name = "Carlos Operador",
                Cpf = "123456789",
                OperatorStatusId = 1,
                LaunchProviderId = 1,
                UserAccountId = 1
            },
            UserAccountDto = new UserAccountDTO
            {
                UserAccountId = 1,
                Email = "carlos.operador@email.com",
                Phone = "11999998888",
                UserRoleId = 3
            }
        };

        _operatorServiceMock
            .Setup(service => service.GetByIdAsync(operatorId))
            .ReturnsAsync(operatorRequestDto);

        // Act
        var result = await _controller.GetById(operatorId);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(operatorRequestDto);
    }

    [Fact]
    public async Task GetById_WhenOperatorDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var operatorId = 999L;

        _operatorServiceMock
            .Setup(service => service.GetByIdAsync(operatorId))
            .ThrowsAsync(new OperatorService.NotFoundException($"Operator with id {operatorId} not found"));

        // Act
        var result = await _controller.GetById(operatorId);

        // Assert
        var notFoundResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        notFoundResult.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task AddOperator_WhenEmailAlreadyExists_ShouldReturnConflict()
    {
        // Arrange
        var operatorRequestDto = new OperatorRequestDTO
        {
            OperatorDto = new OperatorDTO
            {
                Name = "Carlos Operador",
                Cpf = "123456789",
                OperatorStatusId = 1,
                LaunchProviderId = 1
            },
            UserAccountDto = new UserAccountDTO
            {
                Email = "carlos.operador@email.com",
                HashedPassword = "Senha123",
                Phone = "11999998888",
                UserRoleId = 3
            }
        };

        _operatorServiceMock
            .Setup(service => service.AddAsync(operatorRequestDto))
            .ThrowsAsync(new OperatorService.ConflictException("Email already registered"));

        // Act
        var result = await _controller.AddOperator(operatorRequestDto);

        // Assert
        var conflictResult = result.Should().BeOfType<ConflictObjectResult>().Subject;
        conflictResult.StatusCode.Should().Be(409);
    }
}