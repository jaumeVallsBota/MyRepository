using Xunit;
using Moq;
using EstadiosApi.Controllers;
using EstadiosApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using EstadiosApi.Models;
using System;

public class EquiposControllerTests
{
    [Fact]
    public async Task GetEquipos_ReturnsOk_WithListOfEquipos()
    {
        var mockService = new Mock<IEquiposService>();
        mockService.Setup(s => s.GetEquiposAsync())
            .ReturnsAsync(new List<Equipo> { new Equipo { Id = 1, Nombre = "Equipo1" } });

        var controller = new EquiposController(mockService.Object);

        var result = await controller.GetEquipos();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var equipos = Assert.IsAssignableFrom<IEnumerable<Equipo>>(okResult.Value);
        Assert.Single(equipos);
    }

    [Fact]
    public async Task GetEquipos_ReturnsBadRequest_OnException()
    {
        var mockService = new Mock<IEquiposService>();
        mockService.Setup(s => s.GetEquiposAsync())
            .ThrowsAsync(new Exception("Error simulada"));

        var controller = new EquiposController(mockService.Object);

        var result = await controller.GetEquipos();

        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Error simulada", badRequest.Value);
    }

    [Fact]
    public async Task GetEquipo_ReturnsEquipo_WhenFound()
    {
        var equipoEsperado = new Equipo { Id = 1, Nombre = "Equipo1" };

        var mockService = new Mock<IEquiposService>();
        mockService.Setup(s => s.GetEquipoAsync(1))
            .ReturnsAsync(equipoEsperado);

        var controller = new EquiposController(mockService.Object);

        var result = await controller.GetEquipo(1);

        var okResult = Assert.IsType<ActionResult<Equipo>>(result);
        Assert.Equal(equipoEsperado, result.Value);
    }

    [Fact]
    public async Task GetEquipo_ReturnsNotFound_WhenNoExiste()
    {
        var mockService = new Mock<IEquiposService>();
        mockService.Setup(s => s.GetEquipoAsync(1))
            .ReturnsAsync((Equipo?)null);

        var controller = new EquiposController(mockService.Object);

        var result = await controller.GetEquipo(1);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task PostEquipo_ReturnsCreatedAtAction()
    {
        var nuevoEquipo = new Equipo { Id = 1, Nombre = "Equipo1" };

        var mockService = new Mock<IEquiposService>();
        mockService.Setup(s => s.CreateEquipoAsync(It.IsAny<Equipo>()))
            .ReturnsAsync(nuevoEquipo);

        var controller = new EquiposController(mockService.Object);

        var result = await controller.PostEquipo(new Equipo { Nombre = "Equipo1" });

        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(controller.GetEquipo), createdAtActionResult.ActionName);
        Assert.Equal(nuevoEquipo, createdAtActionResult.Value);
    }

    [Fact]
    public async Task PutEquipo_ReturnsNoContent_WhenActualizado()
    {
        var mockService = new Mock<IEquiposService>();
        mockService.Setup(s => s.UpdateEquipoAsync(1, It.IsAny<Equipo>()))
            .ReturnsAsync(true);

        var controller = new EquiposController(mockService.Object);

        var result = await controller.PutEquipo(1, new Equipo { Id = 1, Nombre = "Equipo Actualizado" });

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task PutEquipo_ReturnsNotFound_WhenNoExiste()
    {
        var mockService = new Mock<IEquiposService>();
        mockService.Setup(s => s.UpdateEquipoAsync(1, It.IsAny<Equipo>()))
            .ReturnsAsync(false);

        var controller = new EquiposController(mockService.Object);

        var result = await controller.PutEquipo(1, new Equipo { Id = 1, Nombre = "Equipo" });

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteEquipo_ReturnsNoContent_WhenEliminado()
    {
        var mockService = new Mock<IEquiposService>();
        mockService.Setup(s => s.DeleteEquipoAsync(1))
            .ReturnsAsync(true);

        var controller = new EquiposController(mockService.Object);

        var result = await controller.DeleteEquipo(1);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteEquipo_ReturnsNotFound_WhenNoExiste()
    {
        var mockService = new Mock<IEquiposService>();
        mockService.Setup(s => s.DeleteEquipoAsync(1))
            .ReturnsAsync(false);

        var controller = new EquiposController(mockService.Object);

        var result = await controller.DeleteEquipo(1);

        Assert.IsType<NotFoundResult>(result);
    }
}
