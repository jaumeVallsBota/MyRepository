using Xunit;
using Moq;
using EstadiosApi.Controllers;
using EstadiosApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using EstadiosApi.Models;
using System;

public class EstadiosControllerTests
{
    [Fact]
    public async Task GetEstadios_ReturnsOk_WithList()
    {
        var mockService = new Mock<IEstadiosService>();
        mockService.Setup(s => s.GetEstadiosAsync())
            .ReturnsAsync(new List<Estadio> { new Estadio { Id = 1, Nombre = "Camp Nou" } });

        var controller = new EstadiosController(mockService.Object);

        var result = await controller.GetEstadios();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var estadios = Assert.IsAssignableFrom<IEnumerable<Estadio>>(okResult.Value);
        Assert.Single(estadios);
    }

    [Fact]
    public async Task GetEstadios_ReturnsBadRequest_OnException()
    {
        var mockService = new Mock<IEstadiosService>();
        mockService.Setup(s => s.GetEstadiosAsync())
            .ThrowsAsync(new Exception("Error obteniendo estadios"));

        var controller = new EstadiosController(mockService.Object);

        var result = await controller.GetEstadios();

        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Error obteniendo estadios", badRequest.Value);
    }

    [Fact]
    public async Task GetEstadio_ReturnsOk_WhenFound()
    {
        var estadioEsperado = new Estadio { Id = 1, Nombre = "Camp Nou" };

        var mockService = new Mock<IEstadiosService>();
        mockService.Setup(s => s.GetEstadioAsync(1))
            .ReturnsAsync(estadioEsperado);

        var controller = new EstadiosController(mockService.Object);

        var result = await controller.GetEstadio(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(estadioEsperado, okResult.Value);
    }

    [Fact]
    public async Task GetEstadio_ReturnsNotFound_WhenNull()
    {
        var mockService = new Mock<IEstadiosService>();
        mockService.Setup(s => s.GetEstadioAsync(1))
            .ReturnsAsync((Estadio?)null);

        var controller = new EstadiosController(mockService.Object);

        var result = await controller.GetEstadio(1);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task PostEstadio_ReturnsCreatedAtAction()
    {
        var nuevoEstadio = new Estadio { Id = 1, Nombre = "Camp Nou" };

        var mockService = new Mock<IEstadiosService>();
        mockService.Setup(s => s.CreateEstadioAsync(It.IsAny<Estadio>()))
            .ReturnsAsync(nuevoEstadio);

        var controller = new EstadiosController(mockService.Object);

        var result = await controller.PostEstadio(new Estadio { Nombre = "Camp Nou" });

        var createdAtAction = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(controller.GetEstadio), createdAtAction.ActionName);
        Assert.Equal(nuevoEstadio, createdAtAction.Value);
    }

    [Fact]
    public async Task PutEstadio_ReturnsNoContent_WhenUpdated()
    {
        var mockService = new Mock<IEstadiosService>();
        mockService.Setup(s => s.UpdateEstadioAsync(1, It.IsAny<Estadio>()))
            .ReturnsAsync(true);

        var controller = new EstadiosController(mockService.Object);

        var result = await controller.PutEstadio(1, new Estadio { Id = 5, Nombre = "Actualizado" });

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task PutEstadio_ReturnsNotFound_WhenNotUpdated()
    {
        var mockService = new Mock<IEstadiosService>();
        mockService.Setup(s => s.UpdateEstadioAsync(1, It.IsAny<Estadio>()))
            .ReturnsAsync(false);

        var controller = new EstadiosController(mockService.Object);

        var result = await controller.PutEstadio(1, new Estadio { Id = 5, Nombre = "Actualizado" });

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteEstadio_ReturnsNoContent_WhenDeleted()
    {
        var mockService = new Mock<IEstadiosService>();
        mockService.Setup(s => s.DeleteEstadioAsync(1))
            .ReturnsAsync(true);

        var controller = new EstadiosController(mockService.Object);

        var result = await controller.DeleteEstadio(1);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteEstadio_ReturnsNotFound_WhenNotDeleted()
    {
        var mockService = new Mock<IEstadiosService>();
        mockService.Setup(s => s.DeleteEstadioAsync(1))
            .ReturnsAsync(false);

        var controller = new EstadiosController(mockService.Object);

        var result = await controller.DeleteEstadio(1);

        Assert.IsType<NotFoundResult>(result);
    }
}
