using System.Net;
using System.Net.Http.Json;
using BitFinance.API.IntegrationTests.Infrastructure;
using BitFinance.API.Models.Response;
using BitFinance.Business.Entities;
using BitFinance.Business.Enums;
using FluentAssertions;
using Xunit;

namespace BitFinance.API.IntegrationTests.Expenses;

[Collection("Integration")]
public class GetExpenseByIdTests
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly Guid _organizationId = Guid.NewGuid();
    private readonly string _userId = Guid.NewGuid().ToString();

    public GetExpenseByIdTests(IntegrationTestFixture fixture)
    {
        _factory = fixture.Factory;
        _client = _factory.CreateAuthenticatedClient();

        _factory.SeedDataAsync(db =>
        {
            db.Organizations.Add(new Organization
            {
                Id = _organizationId,
                Name = "GetExpenseById Test Org",
                CreatedAt = DateTime.UtcNow
            });

            db.Users.Add(new User
            {
                Id = _userId,
                FirstName = "Jane",
                LastName = "Doe",
                UserName = "janedoe@test.com",
                Email = "janedoe@test.com"
            });
        }).GetAwaiter().GetResult();
    }

    [Fact]
    public async Task GetExpenseById_ExistingExpense_Returns200WithExpense()
    {
        var expenseId = Guid.NewGuid();
        await _factory.SeedDataAsync(db =>
        {
            db.Expenses.Add(new Expense
            {
                Id = expenseId,
                Description = "Lunch Meeting",
                Category = ExpenseCategory.Food,
                Amount = 45.00m,
                Status = ExpenseStatus.Paid,
                OccurredAt = new DateTime(2025, 3, 10, 0, 0, 0, DateTimeKind.Utc),
                CreatedAt = DateTime.UtcNow,
                CreatedByUserId = _userId,
                OrganizationId = _organizationId
            });
        });

        var response = await _client.GetAsync(
            $"/api/v1/organizations/{_organizationId}/expenses/{expenseId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var expense = await response.Content.ReadFromJsonAsync<GetExpenseResponse>();
        expense.Should().NotBeNull();
        expense!.Id.Should().Be(expenseId);
        expense.Description.Should().Be("Lunch Meeting");
        expense.Category.Should().Be(ExpenseCategory.Food);
        expense.Status.Should().Be(ExpenseStatus.Paid);
        expense.Amount.Should().Be(45.00m);
        expense.CreatedBy.Should().Be("Jane Doe");
    }

    [Fact]
    public async Task GetExpenseById_NonExistentExpense_Returns404()
    {
        var response = await _client.GetAsync(
            $"/api/v1/organizations/{_organizationId}/expenses/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
