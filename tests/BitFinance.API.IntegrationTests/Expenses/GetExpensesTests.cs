using System.Net;
using System.Net.Http.Json;
using BitFinance.API.IntegrationTests.Infrastructure;
using BitFinance.API.Models;
using BitFinance.API.Models.Response;
using BitFinance.Business.Entities;
using BitFinance.Business.Enums;
using FluentAssertions;
using Xunit;

namespace BitFinance.API.IntegrationTests.Expenses;

[Collection("Integration")]
public class GetExpensesTests
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly Guid _organizationId = Guid.NewGuid();
    private readonly string _userId = Guid.NewGuid().ToString();

    public GetExpensesTests(IntegrationTestFixture fixture)
    {
        _factory = fixture.Factory;
        _client = _factory.CreateAuthenticatedClient();

        _factory.SeedDataAsync(db =>
        {
            db.Organizations.Add(new Organization
            {
                Id = _organizationId,
                Name = "GetExpenses Test Org",
                CreatedAt = DateTime.UtcNow
            });

            db.Users.Add(new User
            {
                Id = _userId,
                FirstName = "List",
                LastName = "Tester",
                UserName = "listtester@test.com",
                Email = "listtester@test.com"
            });
        }).GetAwaiter().GetResult();
    }

    [Fact]
    public async Task GetExpenses_ReturnsPaginatedList()
    {
        var orgId = Guid.NewGuid();
        var userId = Guid.NewGuid().ToString();
        await _factory.SeedDataAsync(db =>
        {
            db.Organizations.Add(new Organization
            {
                Id = orgId,
                Name = "Paginated Expenses Org",
                CreatedAt = DateTime.UtcNow
            });

            db.Users.Add(new User
            {
                Id = userId,
                FirstName = "Paginated",
                LastName = "User",
                UserName = "paginated@test.com",
                Email = "paginated@test.com"
            });

            for (int i = 1; i <= 3; i++)
            {
                db.Expenses.Add(new Expense
                {
                    Id = Guid.NewGuid(),
                    Description = $"Expense {i}",
                    Category = ExpenseCategory.Food,
                    Amount = 10m * i,
                    Status = ExpenseStatus.Pending,
                    OccurredAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    CreatedByUserId = userId,
                    OrganizationId = orgId
                });
            }
        });

        var response = await _client.GetAsync(
            $"/api/v1/organizations/{orgId}/expenses");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<PagedResponse<GetExpenseResponse>>();
        result.Should().NotBeNull();
        result!.Data.Should().HaveCount(3);
        result.Page.Should().Be(1);
    }

    [Fact]
    public async Task GetExpenses_WithPagination_ReturnsCorrectPage()
    {
        var orgId = Guid.NewGuid();
        var userId = Guid.NewGuid().ToString();
        await _factory.SeedDataAsync(db =>
        {
            db.Organizations.Add(new Organization
            {
                Id = orgId,
                Name = "Pagination Expenses Org",
                CreatedAt = DateTime.UtcNow
            });

            db.Users.Add(new User
            {
                Id = userId,
                FirstName = "Page",
                LastName = "Tester",
                UserName = "pagetester@test.com",
                Email = "pagetester@test.com"
            });

            for (int i = 1; i <= 5; i++)
            {
                db.Expenses.Add(new Expense
                {
                    Id = Guid.NewGuid(),
                    Description = $"Paginated Expense {i}",
                    Category = ExpenseCategory.Transportation,
                    Amount = 20m,
                    Status = ExpenseStatus.Pending,
                    OccurredAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    CreatedByUserId = userId,
                    OrganizationId = orgId
                });
            }
        });

        var response = await _client.GetAsync(
            $"/api/v1/organizations/{orgId}/expenses?page=1&pageSize=2");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<PagedResponse<GetExpenseResponse>>();
        result.Should().NotBeNull();
        result!.Data.Should().HaveCount(2);
        result.PageSize.Should().Be(2);
    }

    [Fact]
    public async Task GetExpenses_WithDateRangeFilter_ReturnsFilteredResults()
    {
        var orgId = Guid.NewGuid();
        var userId = Guid.NewGuid().ToString();
        await _factory.SeedDataAsync(db =>
        {
            db.Organizations.Add(new Organization
            {
                Id = orgId,
                Name = "Date Filter Expenses Org",
                CreatedAt = DateTime.UtcNow
            });

            db.Users.Add(new User
            {
                Id = userId,
                FirstName = "Filter",
                LastName = "Tester",
                UserName = "filtertester@test.com",
                Email = "filtertester@test.com"
            });

            db.Expenses.Add(new Expense
            {
                Id = Guid.NewGuid(),
                Description = "January Expense",
                Category = ExpenseCategory.Utilities,
                Amount = 100m,
                Status = ExpenseStatus.Paid,
                OccurredAt = new DateTime(2025, 1, 15, 0, 0, 0, DateTimeKind.Utc),
                CreatedAt = new DateTime(2025, 1, 15, 0, 0, 0, DateTimeKind.Utc),
                CreatedByUserId = userId,
                OrganizationId = orgId
            });

            db.Expenses.Add(new Expense
            {
                Id = Guid.NewGuid(),
                Description = "March Expense",
                Category = ExpenseCategory.Utilities,
                Amount = 200m,
                Status = ExpenseStatus.Pending,
                OccurredAt = new DateTime(2025, 3, 15, 0, 0, 0, DateTimeKind.Utc),
                CreatedAt = new DateTime(2025, 3, 15, 0, 0, 0, DateTimeKind.Utc),
                CreatedByUserId = userId,
                OrganizationId = orgId
            });

            db.Expenses.Add(new Expense
            {
                Id = Guid.NewGuid(),
                Description = "June Expense",
                Category = ExpenseCategory.Utilities,
                Amount = 300m,
                Status = ExpenseStatus.Pending,
                OccurredAt = new DateTime(2025, 6, 15, 0, 0, 0, DateTimeKind.Utc),
                CreatedAt = new DateTime(2025, 6, 15, 0, 0, 0, DateTimeKind.Utc),
                CreatedByUserId = userId,
                OrganizationId = orgId
            });
        });

        var from = new DateTime(2025, 2, 1, 0, 0, 0, DateTimeKind.Utc).ToString("O");
        var to = new DateTime(2025, 4, 30, 0, 0, 0, DateTimeKind.Utc).ToString("O");

        var response = await _client.GetAsync(
            $"/api/v1/organizations/{orgId}/expenses?from={from}&to={to}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<PagedResponse<GetExpenseResponse>>();
        result.Should().NotBeNull();
        result!.Data.Should().HaveCount(1);
        result.Data.First().Description.Should().Be("March Expense");
    }
}
