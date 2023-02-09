using CrystallineSociety.Shared.Dtos.Account;

namespace CrystallineSociety.Client.Shared.Services.Contracts;

public interface IAuthenticationService
{
    Task SignIn(SignInRequestDto dto);

    Task SignOut();
}
