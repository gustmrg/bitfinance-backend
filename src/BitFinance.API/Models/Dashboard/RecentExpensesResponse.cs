using BitFinance.API.Models.Expenses;

namespace BitFinance.API.Models.Dashboard;

public record RecentExpensesResponse(List<ExpenseResponse> Data);