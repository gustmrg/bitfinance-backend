namespace BitFinance.API.Services.Interfaces;

public interface IUsersService
{
    public Task<bool> IsUserInOrganizationAsync(string userId, Guid organizationId);
}