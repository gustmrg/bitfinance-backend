using System.Net;
using System.Net.Http.Json;
using BitFinance.API.IntegrationTests.Infrastructure;
using BitFinance.API.Models.Request;
using BitFinance.API.Models.Response;
using BitFinance.Business.Entities;
using BitFinance.Business.Enums;
using FluentAssertions;
using Xunit;

namespace BitFinance.API.IntegrationTests.Bills;

[Collection("Integration")]
public class UpdateBillTests
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly Guid _organizationId = Guid.NewGuid();

    public UpdateBillTests(IntegrationTestFixture fixture)
    {
        _factory = fixture.Factory;
        _client = _factory.CreateAuthenticatedClient();

        _factory.SeedDataAsync(db =>
        {
            db.Organizations.Add(new Organization
            {
                Id = _organizationId,
                Name = "Update Test Org",
                CreatedAt = DateTime.UtcNow
            });
        }).GetAwaiter().GetResult();
    }

    [Fact]
    public async Task UpdateBill_WithValidData_Returns200WithUpdatedFields()
    {
        var billId = Guid.NewGuid();
        await _factory.SeedDataAsync(db =>
        {
            db.Bills.Add(new Bill
            {
                Id = billId,
                Description = "Original Description",
                Category = BillCategory.Housing,
                Status = BillStatus.Created,
                AmountDue = 100m,
                DueDate = new DateOnly(2025, 3, 1),
                CreatedAt = DateTime.UtcNow,
                OrganizationId = _organizationId
            });
        });

        var updateRequest = new UpdateBillRequest(
            Description: "Updated Description",
            Category: "Utilities",
            Status: "Paid",
            DueDate: new DateTime(2025, 3, 15, 0, 0, 0, DateTimeKind.Utc),
            PaymentDate: new DateTime(2025, 3, 10, 0, 0, 0, DateTimeKind.Utc),
            AmountDue: 150m,
            AmountPaid: 150m);

        var response = await _client.PatchAsJsonAsync(
            $"/api/v1/organizations/{_organizationId}/bills/{billId}", updateRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var bill = await response.Content.ReadFromJsonAsync<UpdateBillResponse>();
        bill.Should().NotBeNull();
        bill!.Description.Should().Be("Updated Description");
        bill.Category.Should().Be(BillCategory.Utilities);
        bill.Status.Should().Be(BillStatus.Paid);
        bill.AmountDue.Should().Be(150m);
        bill.AmountPaid.Should().Be(150m);
    }

    [Fact]
    public async Task UpdateBill_NonExistentBill_Returns404()
    {
        var updateRequest = new UpdateBillRequest(
            Description: "Does not matter",
            Category: "Housing",
            Status: "Created",
            DueDate: DateTime.UtcNow.AddDays(30),
            PaymentDate: null,
            AmountDue: 100m,
            AmountPaid: null);

        var response = await _client.PatchAsJsonAsync(
            $"/api/v1/organizations/{_organizationId}/bills/{Guid.NewGuid()}", updateRequest);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateBill_WithInvalidEnum_Returns422()
    {
        var billId = Guid.NewGuid();
        await _factory.SeedDataAsync(db =>
        {
            db.Bills.Add(new Bill
            {
                Id = billId,
                Description = "Bill to update",
                Category = BillCategory.Housing,
                Status = BillStatus.Created,
                AmountDue = 100m,
                DueDate = new DateOnly(2025, 3, 1),
                CreatedAt = DateTime.UtcNow,
                OrganizationId = _organizationId
            });
        });

        var updateRequest = new UpdateBillRequest(
            Description: "Updated",
            Category: "BadCategory",
            Status: "Created",
            DueDate: DateTime.UtcNow.AddDays(30),
            PaymentDate: null,
            AmountDue: 100m,
            AmountPaid: null);

        var response = await _client.PatchAsJsonAsync(
            $"/api/v1/organizations/{_organizationId}/bills/{billId}", updateRequest);

        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }
}
