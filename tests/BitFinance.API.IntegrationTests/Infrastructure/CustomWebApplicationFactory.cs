using BitFinance.API.Services;
using BitFinance.API.Services.Interfaces;
using BitFinance.Data.Caching;
using BitFinance.Data.Contexts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSubstitute;
using Testcontainers.PostgreSql;

namespace BitFinance.API.IntegrationTests.Infrastructure;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly PostgreSqlContainer _postgresContainer = new PostgreSqlBuilder()
        .WithImage("postgres:16-alpine")
        .Build();

    public async Task StartDatabaseAsync()
    {
        await _postgresContainer.StartAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:Database"] = _postgresContainer.GetConnectionString(),
                ["ConnectionStrings:Cache"] = "",
                ["Jwt:Key"] = "test-key-that-is-at-least-32-characters-long-for-hmac-sha256",
                ["Jwt:Issuer"] = "test-issuer",
                ["Jwt:Audience"] = "test-audience",
                ["Jwt:ExpirationInMinutes"] = "60",
                ["AppSettings:CacheEnabled"] = "false",
                ["Storage:Provider"] = "Local",
                ["Storage:LocalPath"] = "/tmp/bitfinance-test-docs",
            });
        });

        builder.ConfigureTestServices(services =>
        {
            // Replace DbContext to use Testcontainers PostgreSQL
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            if (dbContextDescriptor != null) services.Remove(dbContextDescriptor);

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(_postgresContainer.GetConnectionString()));

            // Replace authentication with test scheme
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = TestAuthHandler.AuthenticationScheme;
                    options.DefaultChallengeScheme = TestAuthHandler.AuthenticationScheme;
                })
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                    TestAuthHandler.AuthenticationScheme, _ => { });

            // Replace IUsersService with mock that always authorizes
            var usersServiceDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IUsersService));
            if (usersServiceDescriptor != null) services.Remove(usersServiceDescriptor);

            var mockUsersService = Substitute.For<IUsersService>();
            mockUsersService.IsUserInOrganizationAsync(Arg.Any<string>(), Arg.Any<Guid>())
                .Returns(true);
            services.AddScoped(_ => mockUsersService);

            // Replace IFileStorageService with in-memory implementation
            var fileStorageDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IFileStorageService));
            if (fileStorageDescriptor != null) services.Remove(fileStorageDescriptor);
            services.AddSingleton<IFileStorageService, InMemoryFileStorageService>();

            // Replace ICacheService with no-op
            var cacheServiceDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(ICacheService));
            if (cacheServiceDescriptor != null) services.Remove(cacheServiceDescriptor);
            services.AddSingleton<ICacheService, NoOpCacheService>();

            // Remove hosted background services
            var hostedServiceDescriptors = services
                .Where(d => d.ServiceType == typeof(IHostedService) &&
                           (d.ImplementationType == typeof(BillStatusWorkerService) ||
                            d.ImplementationType == typeof(RefreshTokenCleanupService)))
                .ToList();
            foreach (var descriptor in hostedServiceDescriptors)
            {
                services.Remove(descriptor);
            }
        });
    }

    public HttpClient CreateAuthenticatedClient(Guid? userId = null, Guid? organizationId = null)
    {
        var client = CreateClient();
        client.DefaultRequestHeaders.Add(
            TestAuthHandler.TestUserIdHeader,
            (userId ?? TestConstants.DefaultUserId).ToString());
        return client;
    }

    public async Task SeedDataAsync(Action<ApplicationDbContext> seedAction)
    {
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        seedAction(db);
        await db.SaveChangesAsync();
    }
}
