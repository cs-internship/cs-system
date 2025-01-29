﻿var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddClientConfigurations();

// The following line (using the * in the URL), allows the emulators and mobile devices to access the app using the host IP address.
if (BuildConfiguration.IsDebug() && OperatingSystem.IsWindows())
{
    builder.WebHost.UseUrls("http://localhost:5030", "http://*:5030");
}

CrystallineSociety.Server.Startup.Services.Add(builder.Services, builder.Environment, builder.Configuration);

var app = builder.Build();

CrystallineSociety.Server.Startup.Middlewares.Use(app, builder.Environment, builder.Configuration);

app.Run();
