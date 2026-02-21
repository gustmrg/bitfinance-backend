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
public class UpdateExpenseTests
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly Guid _organizationId = Guid.NewGuid();
    private readonly string _userId = Guid.NewGuid().ToString();

    public UpdateExpenseTests(IntegrationTestFixture fixture)
    {
        _factory = fixture.Factory;
        _client = _factory.CreateAuthenticatedClient();

        _factory.SeedDataAsync(db =>
        {
            db.Organizations.Add(new Organization
            {
                Id = _organizationId,
                Name = "UpdateExpense Test Org",
                CreatedAt = DateTime.UtcNow
            });

            db.Users.Add(new User
            {
                Id = _userId,
                FirstName = "Update",
                LastName = "Tester",
                UserName = "updatetester@test.com",
                Email = "updatetester@test.com"
            });
        }).GetAwaiter().GetResult();
    }

    [Fact]
    public async Task UpdateExpense_WithValidData_Returns200WithUpdatedFields()
    {
        var expenseId = Guid.NewGuid();
        await _factory.SeedDataAsync(db =>
        {
            db.Expenses.Add(new Expense
            {
                Id = expenseId,
                Description = "Original Expense",
                Category = ExpenseCategory.Food,
                Amount = 50m,
                Status = ExpenseStatus.Pending,
                OccurredAt = new DateTime(2025, 3, 1, 0, 0, 0, DateTimeKind.Utc),
                CreatedAt = DateTime.UtcNow,
                CreatedByUserId = _userId,
                OrganizationId = _organizationId
            });
        });

        var updateRequest = new UpdateExpenseRequest(
            Description: "Updated Expense",
            Category: "Transportation",
            Amount: 120m,
            Status: "Paid",
            OccurredAt: new DateTime(2025, 3, 15, 0, 0, 0, DateTimeKind.Utc));

        var response = await _client.PatchAsJsonAsync(
            $"/api/v1/organizations/{_organizationId}/expenses/{expenseId}", updateRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var expense = await response.Content.ReadFromJsonAsync<UpdateExpenseResponse>();
        expense.Should().NotBeNull();
        expense!.Description.Should().Be("Updated Expense");
        expense.Category.Should().Be(ExpenseCategory.Transportation);
        expense.Amount.Should().Be(120m);
        expense.Status.Should().Be(ExpenseStatus.Paid);
        expense.CreatedBy.Should().Be("Update Tester");
    }

    [Fact]
    public async Task UpdateExpense_NonExistentExpense_Returns404()
    {
        var updateRequest = new UpdateExpenseRequest(
            Description: "Does not matter",
            Category: "Food",
            Amount: 100m,
            Status: "Pending",
            OccurredAt: null);

        var response = await _client.PatchAsJsonAsync(
            $"/api/v1/organizations/{_organizationId}/expenses/{Guid.NewGuid()}", updateRequest);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateExpense_WithInvalidEnum_Returns422()
    {
        var expenseId = Guid.NewGuid();
        await _factory.SeedDataAsync(db =>
        {
            db.Expenses.Add(new Expense
            {
                Id = expenseId,
                Description = "Expense to update",
                Category = ExpenseCategory.Food,
                Amount = 100m,
                Status = ExpenseStatus.Pending,
                OccurredAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                CreatedByUserId = _userId,
                OrganizationId = _organizationId
            });
        });

        var updateRequest = new UpdateExpenseRequest(
            Description: "Updated",
            Category: "BadCategory",
            Amount: 100m,
            Status: "Pending",
            OccurredAt: null);

        var response = await _client.PatchAsJsonAsync(
            $"/api/v1/organizations/{_organizationId}/expenses/{expenseId}", updateRequest);

        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }
}
