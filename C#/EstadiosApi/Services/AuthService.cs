using EstadiosApi.Data;
using EstadiosApi.Models;
using EstadiosApi.Models.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EstadiosApi.Services;

namespace EstadiosApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly EstadiosContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(EstadiosContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public string Register(RegisterDto request)
        {
            if (_context.Usuarios.Any(u => u.NombreUsuario == request.NombreUsuario))
            {
                throw new Exception("Este nombre de usuario ya existe.");
            }

            var usuario = new Usuario
            {
                NombreUsuario = request.NombreUsuario,
                PasswordHash = PasswordHasher.HashPassword(request.Password),
                Rol = "Usuario" // por defecto
            };

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            return "Usuario registrado correctamente.";
        }

        public string Login(LoginDto request)
        {
            var usuario = _context.Usuarios.SingleOrDefault(u => u.NombreUsuario == request.NombreUsuario);
            if (usuario == null)
                throw new Exception("Usuario no encontrado.");

             if (!PasswordHasher.VerifyPassword(request.Password, usuario.PasswordHash))
                throw new Exception("Contraseña incorrecta.");

            return CreateToken(usuario);
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