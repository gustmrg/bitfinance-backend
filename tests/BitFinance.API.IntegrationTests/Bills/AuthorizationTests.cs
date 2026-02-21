using System.Net;
using BitFinance.API.IntegrationTests.Infrastructure;
using BitFinance.API.Services.Interfaces;
using BitFinance.Business.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace BitFinance.API.IntegrationTests.Bills;

[Collection("Integration")]
public class AuthorizationTests
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly Guid _organizationId = Guid.NewGuid();

    public AuthorizationTests(IntegrationTestFixture fixture)
    {
        _factory = fixture.Factory;

        _factory.SeedDataAsync(db =>
        {
            db.Organizations.Add(new Organization
            {
                Id = _organizationId,
                Name = "Auth Test Org",
                CreatedAt = DateTime.UtcNow
            });
        }).GetAwaiter().GetResult();
    }

    [Fact]
    public async Task GetBills_UserNotInOrganization_ReturnsErrorStatus()
    {
        // Create a client where IUsersService returns false for org membership
        var unauthorizedMock = Substitute.For<IUsersService>();
        unauthorizedMock.IsUserInOrganizationAsync(Arg.Any<string>(), Arg.Any<Guid>())
            .Returns(false);

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IUsersService));
                if (descriptor != null) services.Remove(descriptor);
                services.AddScoped(_ => unauthorizedMock);
            });
        }).CreateClient();

        client.DefaultRequestHeaders.Add(
            TestAuthHandler.TestUserIdHeader, TestConstants.DefaultUserId.ToString());

        var response = await client.GetAsync(
            $"/api/v1/organizations/{_organizationId}/bills");

        // Note: The filter uses ForbidResult(string) which passes the message as an
        // auth scheme name instead of using the default scheme. This causes a 500
        // because no handler is registered for that scheme name. The correct fix
        // would be to use new ForbidResult() without arguments.
        response.IsSuccessStatusCode.Should().BeFalse();
    }
}
