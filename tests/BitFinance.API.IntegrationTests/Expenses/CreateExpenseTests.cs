using System.Net;
using System.Net.Http.Json;
using BitFinance.API.IntegrationTests.Infrastructure;
using BitFinance.API.Models.Request;
using BitFinance.API.Models.Response;
using BitFinance.Business.Entities;
using BitFinance.Business.Enums;
using FluentAssertions;
using Xunit;

namespace BitFinance.API.IntegrationTests.Expenses;

[Collection("Integration")]
public class CreateExpenseTests
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly Guid _organizationId = Guid.NewGuid();
    private readonly string _userId = Guid.NewGuid().ToString();

    public CreateExpenseTests(IntegrationTestFixture fixture)
    {
        _factory = fixture.Factory;
        _client = _factory.CreateAuthenticatedClient();

        _factory.SeedDataAsync(db =>
        {
            db.Organizations.Add(new Organization
            {
                Id = _organizationId,
                Name = "CreateExpense Test Org",
                CreatedAt = DateTime.UtcNow
            });

            db.Users.Add(new User
            {
                Id = _userId,
                FirstName = "Test",
                LastName = "User",
                UserName = "testuser@test.com",
                Email = "testuser@test.com"
            });
        }).GetAwaiter().GetResult();
    }

    [Fact]
    public async Task CreateExpense_WithValidData_Returns201WithExpense()
    {
        var request = new CreateExpenseRequest(
            Description: "Office Supplies",
            Category: "Personal",
            Amount: 75.50m,
            Status: "Pending",
            OccurredAt: new DateTime(2025, 3, 15, 0, 0, 0, DateTimeKind.Utc),
            CreatedBy: _userId);

        var response = await _client.PostAsJsonAsync(
            $"/api/v1/organizations/{_organizationId}/expenses", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var expense = await response.Content.ReadFromJsonAsync<CreateExpenseResponse>();
        expense.Should().NotBeNull();
        expense!.Id.Should().NotBeEmpty();
        expense.Description.Should().Be("Office Supplies");
        expense.Category.Should().Be(ExpenseCategory.Personal);
        expense.Status.Should().Be(ExpenseStatus.Pending);
        expense.Amount.Should().Be(75.50m);
        expense.CreatedBy.Should().Be("Test User");
    }

    [Fact]
    public async Task CreateExpense_WithInvalidCategory_Returns422()
    {
        var request = new CreateExpenseRequest(
            Description: "Test Expense",
            Category: "InvalidCategory",
            Amount: 100m,
            Status: "Pending",
            OccurredAt: null,
            CreatedBy: _userId);

        var response = await _client.PostAsJsonAsync(
            $"/api/v1/organizations/{_organizationId}/expenses", request);

        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task CreateExpense_WithInvalidStatus_Returns422()
    {
        var request = new CreateExpenseRequest(
            Description: "Test Expense",
            Category: "Food",
            Amount: 100m,
            Status: "NotAStatus",
            OccurredAt: null,
            CreatedBy: _userId);

        var response = await _client.PostAsJsonAsync(
            $"/api/v1/organizations/{_organizationId}/expenses", request);

        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }
}
