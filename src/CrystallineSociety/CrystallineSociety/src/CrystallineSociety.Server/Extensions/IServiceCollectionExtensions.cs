using System.Security.Cryptography.X509Certificates;
using CrystallineSociety.Server;
using CrystallineSociety.Server.Api.AppHooks;
using CrystallineSociety.Server.Api.Services.Implementations;
using CrystallineSociety.Server.Models.Identity;
using CrystallineSociety.Server.Services;
using CrystallineSociety.Server.Services.Implementations;

using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Octokit;
using Swashbuckle.AspNetCore.SwaggerGen;
using CrystallineSociety.Server.Models.Identity;
using CrystallineSociety.Server.Services.Contracts;

namespace Microsoft.Extensions.DependencyInjection;

public static class IServiceCollectionExtensions
{
    public static void AddServerServices(this IServiceCollection services)
    {
        services.AddTransient<IGitHubBadgeService, GitHubBadgeService>();
        //services.AddAppHook<ServerBadgeSystemAppHook>();
        services.AddTransient<ILearnerService, ServerLearnerService>();
        // ToDo: Complete.
        services.AddTransient(CreateGitHubClient);
        services.AddTransient<IOrganizationService, OrganizationService>();
        services.AddTransient<IProgramDocumentService, ProgramDocumentService>();
    }
    
    public static void AddBlazor(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IAuthTokenProvider, ServerSideAuthTokenProvider>();

        services.AddTransient(sp =>
        {
            Uri.TryCreate(configuration.GetApiServerAddress(), UriKind.RelativeOrAbsolute, out var apiServerAddress);

            if (apiServerAddress!.IsAbsoluteUri is false)
            {
                apiServerAddress = new Uri(sp.GetRequiredService<IHttpContextAccessor>().HttpContext!.Request.GetBaseUrl(), apiServerAddress);
            }

            return new HttpClient(sp.GetRequiredKeyedService<HttpMessageHandler>("DefaultMessageHandler"))
            {
                BaseAddress = apiServerAddress
            };
        });

        services.AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents();

        services.AddMvc();

        services.AddClientWebServices();
    }

    public static void AddIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        var appSettings = configuration.GetSection(nameof(AppSettings)).Get<AppSettings>()!;
        var settings = appSettings.IdentitySettings;

        var certificatePath = Path.Combine(Directory.GetCurrentDirectory(), "IdentityCertificate.pfx");
        var certificate = new X509Certificate2(certificatePath, appSettings.IdentitySettings.IdentityCertificatePassword, OperatingSystem.IsWindows() ? X509KeyStorageFlags.EphemeralKeySet : X509KeyStorageFlags.DefaultKeySet);

        services.AddDataProtection()
            .PersistKeysToDbContext<AppDbContext>()
            .ProtectKeysWithCertificate(certificate);

        services.AddIdentity<CrystallineSociety.Server.Models.Identity.User, Role>(options =>
        {
            options.User.RequireUniqueEmail = settings.RequireUniqueEmail;
            options.SignIn.RequireConfirmedEmail = true;
            options.Password.RequireDigit = settings.PasswordRequireDigit;
            options.Password.RequireLowercase = settings.PasswordRequireLowercase;
            options.Password.RequireUppercase = settings.PasswordRequireUppercase;
            options.Password.RequireNonAlphanumeric = settings.PasswordRequireNonAlphanumeric;
            options.Password.RequiredLength = settings.PasswordRequiredLength;
        })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders()
            .AddErrorDescriber<AppIdentityErrorDescriber>()
            .AddApiEndpoints();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = IdentityConstants.BearerScheme;
            options.DefaultChallengeScheme = IdentityConstants.BearerScheme;
            options.DefaultScheme = IdentityConstants.BearerScheme;
        })
        .AddBearerToken(IdentityConstants.BearerScheme, options =>
        {
            options.BearerTokenExpiration = settings.BearerTokenExpiration;
            options.RefreshTokenExpiration = settings.RefreshTokenExpiration;

            var validationParameters = new TokenValidationParameters
            {
                ClockSkew = TimeSpan.Zero,
                RequireSignedTokens = true,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new X509SecurityKey(certificate),

                RequireExpirationTime = true,
                ValidateLifetime = true,

                ValidateAudience = true,
                ValidAudience = settings.Audience,

                ValidateIssuer = true,
                ValidIssuer = settings.Issuer,

                AuthenticationType = IdentityConstants.BearerScheme
            };

            options.BearerTokenProtector = new AppSecureJwtDataFormat(appSettings, validationParameters);

            options.Events = new()
            {
                OnMessageReceived = async context =>
                {
                    // The server accepts the access_token from either the authorization header, the cookie, or the request URL query string
                    context.Token ??= context.Request.Cookies["access_token"] ?? context.Request.Query["access_token"];
                }
            };
        });

        services.AddAuthorization();
    }

    public static void AddSwaggerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "CrystallineSociety.Server.xml"));
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "CrystallineSociety.Shared.xml"));

            options.OperationFilter<ODataOperationFilter>();

            options.AddSecurityDefinition("bearerAuth", new()
            {
                Name = "Authorization",
                Description = "Enter the Bearer Authorization string as following: `Bearer Generated-Bearer-Token`",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new()
            {
                {
                    new()
                    {
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    []
                }
            });
        });
    }

    public static IServiceCollection AddHealthChecks(this IServiceCollection services, IWebHostEnvironment env, IConfiguration configuration)
    {
        var appSettings = configuration.GetSection(nameof(AppSettings)).Get<AppSettings>()!;

        var healthCheckSettings = appSettings.HealthCheckSettings;

        if (healthCheckSettings.EnableHealthChecks is false)
            return services;

        services.AddHealthChecksUI(setupSettings: setup =>
        {
            setup.AddHealthCheckEndpoint("BPHealthChecks", env.IsDevelopment() ? "http://localhost:5030/healthz" : "/healthz");
        }).AddInMemoryStorage();

        var healthChecksBuilder = services.AddHealthChecks()
            .AddProcessAllocatedMemoryHealthCheck(maximumMegabytesAllocated: 6 * 1024)
            .AddDiskStorageHealthCheck(opt =>
                opt.AddDrive(Path.GetPathRoot(Directory.GetCurrentDirectory())!, minimumFreeMegabytes: 5 * 1024))
            .AddDbContextCheck<AppDbContext>();

        var emailSettings = appSettings.EmailSettings;

        if (emailSettings.UseLocalFolderForEmails is false)
        {
            healthChecksBuilder
                .AddSmtpHealthCheck(options =>
                {
                    options.Host = emailSettings.Host;
                    options.Port = emailSettings.Port;

                    if (emailSettings.HasCredential)
                    {
                        options.LoginWith(emailSettings.UserName, emailSettings.Password);
                    }
                });
        }

        return services;
    }
    
    private static GitHubClient CreateGitHubClient(IServiceProvider serviceProvider)
    {
        var productHeaderValue = new ProductHeaderValue("CS-System");
        var gitHubToken = serviceProvider.GetRequiredService<IConfiguration>().GetSection("GitHub")["GitHubAccessToken"];
        var tokenAuth = new Credentials(gitHubToken);
        var client = new GitHubClient(productHeaderValue)
        {
            Credentials = tokenAuth
        };
        return client;
    }
}
