using System.Net;
using BitFinance.API.IntegrationTests.Infrastructure;
using BitFinance.Business.Entities;
using BitFinance.Business.Enums;
using FluentAssertions;
using Xunit;

namespace BitFinance.API.IntegrationTests.Expenses;

[Collection("Integration")]
public class DeleteExpenseTests
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly Guid _organizationId = Guid.NewGuid();
    private readonly string _userId = Guid.NewGuid().ToString();

    public DeleteExpenseTests(IntegrationTestFixture fixture)
    {
        _factory = fixture.Factory;
        _client = _factory.CreateAuthenticatedClient();

        _factory.SeedDataAsync(db =>
        {
            db.Organizations.Add(new Organization
            {
                Id = _organizationId,
                Name = "DeleteExpense Test Org",
                CreatedAt = DateTime.UtcNow
            });

            db.Users.Add(new User
            {
                Id = _userId,
                FirstName = "Delete",
                LastName = "Tester",
                UserName = "deletetester@test.com",
                Email = "deletetester@test.com"
            });
        }).GetAwaiter().GetResult();
    }

    [Fact]
    public async Task DeleteExpense_ExistingExpense_Returns204AndExpenseIsGone()
    {
        var expenseId = Guid.NewGuid();
        await _factory.SeedDataAsync(db =>
        {
            db.Expenses.Add(new Expense
            {
                Id = expenseId,
                Description = "Expense to delete",
                Category = ExpenseCategory.Entertainment,
                Amount = 30m,
                Status = ExpenseStatus.Pending,
                OccurredAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                CreatedByUserId = _userId,
                OrganizationId = _organizationId
            });
        });

        var deleteResponse = await _client.DeleteAsync(
            $"/api/v1/organizations/{_organizationId}/expenses/{expenseId}");

        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify the expense is gone
        var getResponse = await _client.GetAsync(
            $"/api/v1/organizations/{_organizationId}/expenses/{expenseId}");

        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteExpense_NonExistentExpense_Returns404()
    {
        var response = await _client.DeleteAsync(
            $"/api/v1/organizations/{_organizationId}/expenses/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
