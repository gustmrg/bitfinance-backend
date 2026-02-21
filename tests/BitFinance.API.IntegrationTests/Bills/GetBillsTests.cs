using System.Net;
using System.Net.Http.Json;
using BitFinance.API.IntegrationTests.Infrastructure;
using BitFinance.API.Models;
using BitFinance.API.Models.Response;
using BitFinance.Business.Entities;
using BitFinance.Business.Enums;
using FluentAssertions;
using Xunit;

namespace BitFinance.API.IntegrationTests.Bills;

[Collection("Integration")]
public class GetBillsTests
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly Guid _organizationId = Guid.NewGuid();

    public GetBillsTests(IntegrationTestFixture fixture)
    {
        _factory = fixture.Factory;
        _client = _factory.CreateAuthenticatedClient();

        _factory.SeedDataAsync(db =>
        {
            db.Organizations.Add(new Organization
            {
                Id = _organizationId,
                Name = "GetBills Test Org",
                CreatedAt = DateTime.UtcNow
            });
        }).GetAwaiter().GetResult();
    }

    [Fact]
    public async Task GetBills_ReturnsPaginatedList()
    {
        await _factory.SeedDataAsync(db =>
        {
            for (int i = 1; i <= 3; i++)
            {
                db.Bills.Add(new Bill
                {
                    Id = Guid.NewGuid(),
                    Description = $"Bill {i}",
                    Category = BillCategory.Housing,
                    Status = BillStatus.Created,
                    AmountDue = 100m * i,
                    DueDate = new DateOnly(2025, 3, i),
                    CreatedAt = DateTime.UtcNow,
                    OrganizationId = _organizationId
                });
            }
        });

        var response = await _client.GetAsync(
            $"/api/v1/organizations/{_organizationId}/bills");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<PagedResponse<GetBillResponse>>();
        result.Should().NotBeNull();
        result!.Data.Should().HaveCount(3);
        result.Page.Should().Be(1);
    }

    [Fact]
    public async Task GetBills_WithPagination_ReturnsCorrectPage()
    {
        var orgId = Guid.NewGuid();
        await _factory.SeedDataAsync(db =>
        {
            db.Organizations.Add(new Organization
            {
                Id = orgId,
                Name = "Pagination Test Org",
                CreatedAt = DateTime.UtcNow
            });

            for (int i = 1; i <= 5; i++)
            {
                db.Bills.Add(new Bill
                {
                    Id = Guid.NewGuid(),
                    Description = $"Paginated Bill {i}",
                    Category = BillCategory.Food,
                    Status = BillStatus.Created,
                    AmountDue = 50m,
                    DueDate = new DateOnly(2025, 6, i),
                    CreatedAt = DateTime.UtcNow,
                    OrganizationId = orgId
                });
            }
        });

        var response = await _client.GetAsync(
            $"/api/v1/organizations/{orgId}/bills?page=1&pageSize=2");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<PagedResponse<GetBillResponse>>();
        result.Should().NotBeNull();
        result!.Data.Should().HaveCount(2);
        result.PageSize.Should().Be(2);
    }

    [Fact]
    public async Task GetBills_WithDateRangeFilter_ReturnsFilteredResults()
    {
        var orgId = Guid.NewGuid();
        await _factory.SeedDataAsync(db =>
        {
            db.Organizations.Add(new Organization
            {
                Id = orgId,
                Name = "Date Filter Test Org",
                CreatedAt = DateTime.UtcNow
            });

            db.Bills.Add(new Bill
            {
                Id = Guid.NewGuid(),
                Description = "January Bill",
                Category = BillCategory.Utilities,
                Status = BillStatus.Paid,
                AmountDue = 100m,
                DueDate = new DateOnly(2025, 1, 15),
                CreatedAt = DateTime.UtcNow,
                OrganizationId = orgId
            });

            db.Bills.Add(new Bill
            {
                Id = Guid.NewGuid(),
                Description = "March Bill",
                Category = BillCategory.Utilities,
                Status = BillStatus.Created,
                AmountDue = 200m,
                DueDate = new DateOnly(2025, 3, 15),
                CreatedAt = DateTime.UtcNow,
                OrganizationId = orgId
            });

            db.Bills.Add(new Bill
            {
                Id = Guid.NewGuid(),
                Description = "June Bill",
                Category = BillCategory.Utilities,
                Status = BillStatus.Created,
                AmountDue = 300m,
                DueDate = new DateOnly(2025, 6, 15),
                CreatedAt = DateTime.UtcNow,
                OrganizationId = orgId
            });
        });

        var from = new DateTime(2025, 2, 1).ToString("O");
        var to = new DateTime(2025, 4, 30).ToString("O");

        var response = await _client.GetAsync(
            $"/api/v1/organizations/{orgId}/bills?from={from}&to={to}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<PagedResponse<GetBillResponse>>();
        result.Should().NotBeNull();
        result!.Data.Should().HaveCount(1);
        result.Data.First().Description.Should().Be("March Bill");
    }
}
