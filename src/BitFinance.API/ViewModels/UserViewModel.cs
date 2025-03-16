namespace BitFinance.API.ViewModels;

public record UserViewModel(string Id, string FirstName, string LastName, string Email, string UserName, List<OrganizationViewModel> Organizations);