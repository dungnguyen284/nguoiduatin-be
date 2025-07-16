using NDT.BusinessModels.Entities;

namespace NDT.BusinessLogic.Services.Interfaces
{
    public interface IJwtService
    {
        Task<string> GenerateToken(AppUser user);
    }
} 