using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VeloSpace.Controllers;
using VeloSpace.DTOs.Auth;
using VeloSpace.Services.Auth;
using Xunit;

namespace VeloSpace.Tests.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _authServiceMock = new Mock<IAuthService>();
        _controller = new AuthController(_authServiceMock.Object);
    }

    [Fact]
    public async Task Login_WhenCredentialsAreValid_ShouldReturnOk()
    {
        // Arrange
        var loginRequest = new LoginRequestDTO
        {
            Email = "user@email.com",
            HashedPassword = "Senha123"
        };

        var loginResponse = new LoginResponseDTO
        {
            Token = "jwt-token-test",
            UserAccountId = 1,
            Email = "user@email.com",
            Phone = "11999998888",
            UserRoleId = 1
        };

        _authServiceMock
            .Setup(service => service.LoginAsync(loginRequest))
            .ReturnsAsync(loginResponse);

        // Act
        var result = await _controller.Login(loginRequest);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(loginResponse);
    }

    [Fact]
    public async Task Login_WhenCredentialsAreInvalid_ShouldReturnUnauthorized()
    {
        // Arrange
        var loginRequest = new LoginRequestDTO
        {
            Email = "user@email.com",
            HashedPassword = "senhaerrada"
        };

        _authServiceMock
            .Setup(service => service.LoginAsync(loginRequest))
            .ThrowsAsync(new UnauthorizedAccessException("Invalid email or password"));

        // Act
        var result = await _controller.Login(loginRequest);

        // Assert
        var unauthorizedResult = result.Should()
            .BeOfType<UnauthorizedObjectResult>()
            .Subject;

        unauthorizedResult.StatusCode.Should().Be(401);
    }
}