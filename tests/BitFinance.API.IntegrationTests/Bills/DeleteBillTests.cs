using System.Net;
using BitFinance.API.IntegrationTests.Infrastructure;
using BitFinance.Business.Entities;
using BitFinance.Business.Enums;
using FluentAssertions;
using Xunit;

namespace BitFinance.API.IntegrationTests.Bills;

[Collection("Integration")]
public class DeleteBillTests
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly Guid _organizationId = Guid.NewGuid();

    public DeleteBillTests(IntegrationTestFixture fixture)
    {
        _factory = fixture.Factory;
        _client = _factory.CreateAuthenticatedClient();

        _factory.SeedDataAsync(db =>
        {
            db.Organizations.Add(new Organization
            {
                Id = _organizationId,
                Name = "Delete Test Org",
                CreatedAt = DateTime.UtcNow
            });
        }).GetAwaiter().GetResult();
    }

    [Fact]
    public async Task DeleteBill_ExistingBill_Returns204AndBillIsGone()
    {
        var billId = Guid.NewGuid();
        await _factory.SeedDataAsync(db =>
        {
            db.Bills.Add(new Bill
            {
                Id = billId,
                Description = "Bill to delete",
                Category = BillCategory.Housing,
                Status = BillStatus.Created,
                AmountDue = 100m,
                DueDate = new DateOnly(2025, 5, 1),
                CreatedAt = DateTime.UtcNow,
                OrganizationId = _organizationId
            });
        });

        var deleteResponse = await _client.DeleteAsync(
            $"/api/v1/organizations/{_organizationId}/bills/{billId}");

        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify the bill is gone
        var getResponse = await _client.GetAsync(
            $"/api/v1/organizations/{_organizationId}/bills/{billId}");

        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteBill_NonExistentBill_Returns404()
    {
        var response = await _client.DeleteAsync(
            $"/api/v1/organizations/{_organizationId}/bills/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
