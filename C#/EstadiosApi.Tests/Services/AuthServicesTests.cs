using System;
using System.Collections.Generic;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;
using EstadiosApi.Data;
using EstadiosApi.Services;
using EstadiosApi.Models;
using EstadiosApi.Models.DTOs;
using System.Security.Claims;

public class AuthServiceTests
{
    private readonly EstadiosContext _context;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
       var options = new DbContextOptionsBuilder<EstadiosContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new EstadiosContext(options);

            // Seed a user for login tests
            var existingUser = new Usuario
            {
                NombreUsuario = "testuser",
                PasswordHash = PasswordHasher.HashPassword("correct_password"),
                Rol = "Usuario"
            };
            _context.Usuarios.Add(existingUser);
            _context.SaveChanges();

            // In-memory configuration
            var inMemorySettings = new Dictionary<string, string>
            {
                {"Jwt:Key", "abcdefghijklmnopqrstuvwxyz123456"},
                {"Jwt:Issuer", "TestIssuer"},
                {"Jwt:Audience", "TestAudience"}
            };
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _authService = new AuthService(_context, configuration);
        }

        [Fact]
        public void Register_WhenNewUser_ReturnsSuccessMessage()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                NombreUsuario = "nuevo",
                Password = "1234"
            };

            // Act
            var result = _authService.Register(registerDto);

            // Assert
            Assert.Equal("Usuario registrado correctamente.", result);
            Assert.Contains(_context.Usuarios, u => u.NombreUsuario == "nuevo");
        }

        [Fact]
        public void Register_WhenUserExists_ThrowsException()
        {
            // Arrange: user "testuser" was seeded in constructor
            var registerDto = new RegisterDto
            {
                NombreUsuario = "testuser",
                Password = "any"
            };

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => _authService.Register(registerDto));
            Assert.Equal("Este nombre de usuario ya existe.", ex.Message);
        }

        [Fact]
        public void Login_WithValidCredentials_ReturnsToken()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                NombreUsuario = "testuser",
                Password = "correct_password"
            };

            // Act
            var tokenString = _authService.Login(loginDto);

            // Assert
            Assert.False(string.IsNullOrEmpty(tokenString));
            var handler = new JwtSecurityTokenHandler();
            Assert.True(handler.CanReadToken(tokenString));

            var jwtToken = handler.ReadJwtToken(tokenString);
            Assert.Equal("testuser", jwtToken.Claims.First(c => c.Type == ClaimTypes.Name).Value);
            Assert.Equal("Usuario", jwtToken.Claims.First(c => c.Type == ClaimTypes.Role).Value);
        }

        [Fact]
        public void Login_WithNonexistentUser_ThrowsException()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                NombreUsuario = "nouser",
                Password = "any"
            };

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => _authService.Login(loginDto));
            Assert.Equal("Usuario no encontrado.", ex.Message);
        }

        [Fact]
        public void Login_WithWrongPassword_ThrowsException()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                NombreUsuario = "testuser",
                Password = "wrong_password"
            };

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => _authService.Login(loginDto));
            Assert.Equal("Contraseña incorrecta.", ex.Message);
        }
}