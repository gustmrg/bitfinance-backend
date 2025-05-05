using BitFinance.API.Models.Bills;

namespace BitFinance.API.Models.Dashboard;

internal record UpcomingBillsResponse(List<BillResponse> Data);