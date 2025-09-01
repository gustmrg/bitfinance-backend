using BitFinance.Application.Interfaces;
using BitFinance.Business.Entities;

namespace BitFinance.Application.Services;

public class OrganizationService : IOrganizationService
{
    public async Task<Organization> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}