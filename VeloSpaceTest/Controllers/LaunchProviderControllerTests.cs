using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VeloSpace.Controllers;
using VeloSpace.DTOs;
using VeloSpace.DTOs.LaunchProvidersDTOS;
using VeloSpace.Services.LaunchProvidersServices;
using Xunit;

namespace VeloSpace.Tests.Controllers;

public class LaunchProviderControllerTests
{
    private readonly Mock<ILaunchProvidersService> _launchProviderServiceMock;
    private readonly LaunchProviderController _controller;

    public LaunchProviderControllerTests()
    {
        _launchProviderServiceMock = new Mock<ILaunchProvidersService>();
        _controller = new LaunchProviderController(_launchProviderServiceMock.Object);
    }

    [Fact]
    public async Task GetById_WhenLaunchProviderExists_ShouldReturnOk()
    {
        // Arrange
        var launchProviderId = 1L;

        var launchProviderRequestDto = new LaunchProviderRequestDTO
        {
            LaunchProviderDto = new LaunchProviderDTO
            {
                LaunchProviderId = launchProviderId,
                CorporateName = "Space Launch Brasil LTDA",
                Cnpj = "12345678912345",
                UserAccountId = 1
            },
            UserAccountDto = new UserAccountDTO
            {
                UserAccountId = 1,
                Email = "launch.provider@email.com",
                Phone = "11999998888",
                UserRoleId = 2
            }
        };

        _launchProviderServiceMock
            .Setup(service => service.GetByIdAsync(launchProviderId))
            .ReturnsAsync(launchProviderRequestDto);

        // Act
        var result = await _controller.GetById(launchProviderId);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(launchProviderRequestDto);
    }

    [Fact]
    public async Task GetById_WhenLaunchProviderDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var launchProviderId = 999L;

        _launchProviderServiceMock
            .Setup(service => service.GetByIdAsync(launchProviderId))
            .ThrowsAsync(new LaunchProvidersService.NotFoundException($"Launch Provider with id {launchProviderId} not found"));

        // Act
        var result = await _controller.GetById(launchProviderId);

        // Assert
        var notFoundResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        notFoundResult.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task AddLaunchProvider_WhenPayloadIsValid_ShouldReturnCreated()
    {
        // Arrange
        var launchProviderRequestDto = new LaunchProviderRequestDTO
        {
            LaunchProviderDto = new LaunchProviderDTO
            {
                CorporateName = "Space Launch Brasil LTDA",
                Cnpj = "12345678912345"
            },
            UserAccountDto = new UserAccountDTO
            {
                Email = "launch.provider@email.com",
                HashedPassword = "Senha123",
                Phone = "11999998888",
                UserRoleId = 2
            }
        };

        _launchProviderServiceMock
            .Setup(service => service.AddAsync(launchProviderRequestDto))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.AddLaunchProvider(launchProviderRequestDto);

        // Assert
        var createdResult = result.Should().BeOfType<CreatedResult>().Subject;
        createdResult.StatusCode.Should().Be(201);

        _launchProviderServiceMock.Verify(
            service => service.AddAsync(launchProviderRequestDto),
            Times.Once
        );
    }
}