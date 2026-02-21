using System.Net;
using System.Net.Http.Json;
using BitFinance.API.IntegrationTests.Infrastructure;
using BitFinance.API.Models.Response;
using BitFinance.Business.Entities;
using BitFinance.Business.Enums;
using FluentAssertions;
using Xunit;

namespace BitFinance.API.IntegrationTests.Bills;

[Collection("Integration")]
public class GetBillByIdTests
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly Guid _organizationId = Guid.NewGuid();

    public GetBillByIdTests(IntegrationTestFixture fixture)
    {
        _factory = fixture.Factory;
        _client = _factory.CreateAuthenticatedClient();

        _factory.SeedDataAsync(db =>
        {
            db.Organizations.Add(new Organization
            {
                Id = _organizationId,
                Name = "GetById Test Org",
                CreatedAt = DateTime.UtcNow
            });
        }).GetAwaiter().GetResult();
    }

    [Fact]
    public async Task GetBillById_ExistingBill_Returns200WithBillAndDocuments()
    {
        var billId = Guid.NewGuid();
        await _factory.SeedDataAsync(db =>
        {
            db.Bills.Add(new Bill
            {
                Id = billId,
                Description = "Internet Bill",
                Category = BillCategory.Utilities,
                Status = BillStatus.Created,
                AmountDue = 89.99m,
                DueDate = new DateOnly(2025, 4, 1),
                CreatedAt = DateTime.UtcNow,
                OrganizationId = _organizationId
            });
        });

        var response = await _client.GetAsync(
            $"/api/v1/organizations/{_organizationId}/bills/{billId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var bill = await response.Content.ReadFromJsonAsync<GetBillResponse>();
        bill.Should().NotBeNull();
        bill!.Id.Should().Be(billId);
        bill.Description.Should().Be("Internet Bill");
        bill.Category.Should().Be(BillCategory.Utilities);
        bill.Status.Should().Be(BillStatus.Created);
        bill.AmountDue.Should().Be(89.99m);
        bill.Documents.Should().BeEmpty();
    }

    [Fact]
    public async Task GetBillById_NonExistentBill_Returns404()
    {
        var randomId = Guid.NewGuid();

        var response = await _client.GetAsync(
            $"/api/v1/organizations/{_organizationId}/bills/{randomId}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
