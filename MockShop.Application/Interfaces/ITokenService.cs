using MockShop.Domain.Entities;

namespace MockShop.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
