using CrystaLearn.Console.Extensions;
using CrystaLearn.Core.Extensions;
using CrystaLearn.Core.Services.Contracts;
using Microsoft.Extensions.Hosting;

var builder = new HostApplicationBuilder();
builder.AddCrystaServices();
builder.AddConsoleServices();

var host = builder.Build();

var services = host.Services;
var programsRepo = services.GetRequiredService<ICrystaProgramRepository>();
var programs = await programsRepo.GetCrystaProgramsAsync(CancellationToken.None); ;
Console.WriteLine($"Programs: {programs.Count()}");
host.Run();
