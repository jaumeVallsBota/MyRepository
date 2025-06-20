using BCrypt.Net;

namespace EstadiosApi.Services
{
    public static class PasswordHasher
    {
        // Hashea la contraseña
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Verifica si una contraseña introducida coincide con el hash guardado
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}