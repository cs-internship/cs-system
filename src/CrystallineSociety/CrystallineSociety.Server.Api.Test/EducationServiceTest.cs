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

    [TestMethod]
    public async Task EducationSync_MustWork()
    {
        var testHost = Host.CreateDefaultBuilder()
            .ConfigureServices((_,
                    services) =>
                {
                    services.AddSharedServices();
                    services.AddServerServices();
                    // ToDo: add dbcontext here for inject.
                    //services.AddDbContext<AppDbContext>(options =>
                    //{
                    //    options
                    //        .UseSqlServer(configuration.GetConnectionString("SqlServerConnectionString"), sqlOpt =>
                    //        {
                    //            sqlOpt.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    //        });
                    //});
                }
            )
            .Build();

        var educationService = testHost.Services.GetRequiredService<IEducationProgramService>();

        // sample education 
        var education = new EducationProgram
        {
            Id = Guid.Parse("466620a1-a6cd-4643-a0ec-ec8235ce93a3"),
            // ToDo : replace with original cs system url
            BadgeSystemUrl =
                "https://github.com/hootanht/cs-system/tree/main/src/CrystallineSociety/Shared/CrystallineSociety.Shared.Test/BadgeSystem/SampleBadgeDocs/github-sample-folder/documentation-validations",
            Code = "test",
            Title = "test",
            LastSyncDateTime = DateTimeOffset.Parse("2023-10-09 00:00:00.0000000 +03:30"),
            LastCommitHash = "test",
            IsActive = true,
        };

       var isEducationExist = await educationService.IsEducationExistAsync(education.Code, CancellationToken.None);

        if (isEducationExist)
        {
            await educationService.SyncBadgeSystemAsync(education.Code, CancellationToken.None);
        }
        else
        {
            await educationService.AddEducationAsync(education, CancellationToken.None);
            await educationService.SyncBadgeSystemAsync(education.Code, CancellationToken.None);
        }

        var educationPrograms = await educationService.GetAllEducationProgramsAsync(CancellationToken.None);

        Assert.AreEqual(education , educationPrograms.FirstOrDefault());
    }
}
