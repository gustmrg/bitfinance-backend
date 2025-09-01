using BitFinance.Business.Entities;

namespace BitFinance.Application.Interfaces;

public interface IOrganizationService
{
    Task<Organization> GetByIdAsync(int id);
}