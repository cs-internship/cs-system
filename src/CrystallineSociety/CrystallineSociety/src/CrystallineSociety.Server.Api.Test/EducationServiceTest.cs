using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystallineSociety.Server.Api.Data;
using CrystallineSociety.Server.Api.Models;
using CrystallineSociety.Server.Api.Services.Contracts;
using CrystallineSociety.Server.Api.Services.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace CrystallineSociety.Server.Api.Test;

[TestClass]
public class EducationServiceTest
{
    public TestContext TestContext { get; set; } = default!;

}
