using EstadiosApi.Models;
using EstadiosApi.Models.DTOs;

namespace EstadiosApi.Services
{
    public interface IAuthService
    {
        string Login(LoginDto request);
        string Register(RegisterDto request);
    }
}