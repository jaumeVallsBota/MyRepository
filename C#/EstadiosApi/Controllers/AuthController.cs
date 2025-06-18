using EstadiosApi.Data;
using EstadiosApi.Models;
using EstadiosApi.Models.DTOs;
using EstadiosApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EstadiosApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly EstadiosContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(EstadiosContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterDto request)
        {
            if (_context.Usuarios.Any(u => u.NombreUsuario == request.NombreUsuario))
            {
                return BadRequest("Este nombre de usuario ya existe.");
            }

            var usuario = new Usuario
            {
                NombreUsuario = request.NombreUsuario,
                PasswordHash = PasswordHasher.HashPassword(request.Password)
            };

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            return Ok("Usuario registrado correctamente.");
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto request)
        {
            var usuario = _context.Usuarios.SingleOrDefault(u => u.NombreUsuario == request.NombreUsuario);

            if (usuario == null)
                return Unauthorized("Usuario no encontrado.");

            var passwordHash = PasswordHasher.HashPassword(request.Password);

            if (usuario.PasswordHash != passwordHash)
                return Unauthorized("Contraseña incorrecta.");

            var token = CreateToken(usuario);
            return Ok(new { token });
        }

        private string CreateToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                new Claim(ClaimTypes.Role, usuario.Rol)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}