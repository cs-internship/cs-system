﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystaLearn.Shared.Dtos.Crysta;
using CrystaLearn.Shared.Dtos.Statistics;

namespace CrystaLearn.Shared.Controllers.Crysta;
[Route("api/[controller]/[action]/")]

public interface IDocumentController : IAppController
{
    //[HttpGet("{packageId}")]
    //Task<NugetStatsDto> GetNugetStats(string packageId, CancellationToken cancellationToken);

    //[HttpGet, Route("https://api.github.com/repos/bitfoundation/bitplatform")]
    //Task<GitHubStats> GetGitHubStats(CancellationToken cancellationToken) => default!;

    [HttpGet("{programCode}")]
    Task<List<DocumentDto>> GetDocuments(string programCode, CancellationToken cancellationToken);

    [HttpGet("{programCode}/{code}")]
    Task<DocumentDto?> GetDocumentByCode(string programCode, string code, CancellationToken cancellationToken);
}
