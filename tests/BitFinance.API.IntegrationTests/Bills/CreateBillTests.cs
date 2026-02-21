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
public class CreateBillTests
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly Guid _organizationId = Guid.NewGuid();

    public CreateBillTests(IntegrationTestFixture fixture)
    {
        _factory = fixture.Factory;
        _client = _factory.CreateAuthenticatedClient();

        // Seed an organization for this test class
        _factory.SeedDataAsync(db =>
        {
            db.Organizations.Add(new Organization
            {
                Id = _organizationId,
                Name = "Test Organization",
                CreatedAt = DateTime.UtcNow
            });
        }).GetAwaiter().GetResult();
    }

    [Fact]
    public async Task SmokeTest_NonExistentEndpoint_Returns404()
    {
        var response = await _client.GetAsync("/api/v1/nonexistent");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateBill_WithValidData_Returns201WithBill()
    {
        var request = new CreateBillRequest(
            Description: "Electricity Bill",
            Category: "Utilities",
            Status: "Created",
            DueDate: new DateTime(2025, 3, 15, 0, 0, 0, DateTimeKind.Utc),
            PaymentDate: null,
            AmountDue: 150.50m,
            AmountPaid: null);

        var response = await _client.PostAsJsonAsync(
            $"/api/v1/organizations/{_organizationId}/bills", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var bill = await response.Content.ReadFromJsonAsync<CreateBillResponse>();
        bill.Should().NotBeNull();
        bill!.Id.Should().NotBeEmpty();
        bill.Description.Should().Be("Electricity Bill");
        bill.Category.Should().Be(BillCategory.Utilities);
        bill.Status.Should().Be(BillStatus.Created);
        bill.AmountDue.Should().Be(150.50m);
        bill.AmountPaid.Should().BeNull();
    }

    [Fact]
    public async Task CreateBill_WithInvalidCategory_Returns422()
    {
        var request = new CreateBillRequest(
            Description: "Test Bill",
            Category: "InvalidCategory",
            Status: "Created",
            DueDate: DateTime.UtcNow.AddDays(30),
            PaymentDate: null,
            AmountDue: 100m,
            AmountPaid: null);

        var response = await _client.PostAsJsonAsync(
            $"/api/v1/organizations/{_organizationId}/bills", request);

        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task CreateBill_WithInvalidStatus_Returns422()
    {
        var request = new CreateBillRequest(
            Description: "Test Bill",
            Category: "Housing",
            Status: "NotAStatus",
            DueDate: DateTime.UtcNow.AddDays(30),
            PaymentDate: null,
            AmountDue: 100m,
            AmountPaid: null);

        var response = await _client.PostAsJsonAsync(
            $"/api/v1/organizations/{_organizationId}/bills", request);

        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }
}
