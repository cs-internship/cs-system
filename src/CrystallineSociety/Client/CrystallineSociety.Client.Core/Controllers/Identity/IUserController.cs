﻿using CrystallineSociety.Shared.Dtos.Identity;

namespace CrystallineSociety.Client.Core.Controllers.Identity;

[Route("api/[controller]/[action]/")]
public interface IUserController : IAppController
{
    [HttpGet]
    Task<UserDto> GetCurrentUser(CancellationToken cancellationToken = default);

    [HttpPut]
    Task<UserDto> Update(EditUserDto body, CancellationToken cancellationToken = default);

    [HttpDelete]
    Task Delete(CancellationToken cancellationToken = default);
}
