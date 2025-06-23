using Xunit;
using Moq;
using EstadiosApi.Controllers;
using EstadiosApi.Services;
using EstadiosApi.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;

public class AuthControllerTests
{
    [Fact]
    public void Register_ReturnsOk_WhenSuccess()
    {
        var mockService = new Mock<IAuthService>();
        mockService.Setup(s => s.Register(It.IsAny<RegisterDto>()))
            .Returns("Usuario registrado correctamente");

        var controller = new AuthController(mockService.Object);

        var result = controller.Register(new RegisterDto
        {
            NombreUsuario = "jaume",
            Password = "secreta"
        });

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Usuario registrado correctamente", okResult.Value);
    }

    [Fact]
    public void Register_ReturnsBadRequest_OnException()
    {
        var mockService = new Mock<IAuthService>();
        mockService.Setup(s => s.Register(It.IsAny<RegisterDto>()))
            .Throws(new Exception("Error de registro"));

        var controller = new AuthController(mockService.Object);

        var result = controller.Register(new RegisterDto());

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Error de registro", badRequest.Value);
    }

    [Fact]
    public void Login_ReturnsOk_WithToken()
    {
        var tokenEsperado = "fake-jwt-token";

        var mockService = new Mock<IAuthService>();
        mockService.Setup(s => s.Login(It.IsAny<LoginDto>()))
            .Returns(tokenEsperado);

        var controller = new AuthController(mockService.Object);

        var result = controller.Login(new LoginDto
        {
            NombreUsuario = "jaume",
            Password = "secreta"
        });

        var okResult = Assert.IsType<OkObjectResult>(result);
        var responseBody = Assert.IsAssignableFrom<Dictionary<string, string>>(okResult.Value);
        Assert.Equal(tokenEsperado, responseBody["token"]);
    }

    [Fact]
    public void Login_ReturnsUnauthorized_OnException()
    {
        var mockService = new Mock<IAuthService>();
        mockService.Setup(s => s.Login(It.IsAny<LoginDto>()))
            .Throws(new Exception("Credenciales inválidas"));

        var controller = new AuthController(mockService.Object);

        var result = controller.Login(new LoginDto());

        var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result);
        Assert.Equal("Credenciales inválidas", unauthorized.Value);
    }
}