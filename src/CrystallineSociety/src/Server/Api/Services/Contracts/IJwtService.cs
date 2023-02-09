using CrystallineSociety.Server.Api.Models.Account;
using CrystallineSociety.Shared.Dtos.Account;

namespace CrystallineSociety.Server.Api.Services.Contracts;

public interface IJwtService
{
    Task<SignInResponseDto> GenerateToken(User user);
}
