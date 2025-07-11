using RealEstate.Domain.Dto;

namespace RealEstate.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse?> LoginAsync(AuthRequest request);
        Task<bool> RegisterAsync(AuthRequest request);
    }
}
