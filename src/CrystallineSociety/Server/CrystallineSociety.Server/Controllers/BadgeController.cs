﻿using CrystallineSociety.Client.Core.Controllers;

namespace CrystallineSociety.Server.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[AllowAnonymous]
public partial class BadgeController : AppControllerBase,IBadgeController
{

}
