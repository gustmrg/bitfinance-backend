using BitFinance.Application.Interfaces;
using BitFinance.Domain.Entities;
using BitFinance.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace BitFinance.Application.Services;

public class OrganizationService : IOrganizationService
{
    private readonly IOrganizationRepository _organizationRepository;
    private readonly ILogger<OrganizationService> _logger;

    public OrganizationService(IOrganizationRepository organizationRepository, 
        ILogger<OrganizationService> logger)
    {
        _organizationRepository = organizationRepository;
        _logger = logger;
    }
    
    public async Task<Organization?> GetByIdAsync(Guid id)
    {
        return await _organizationRepository.GetByIdAsync(id);
    }

    public async Task<List<Organization>> GetByUserIdAsync(string userId)
    {
        return await _organizationRepository.GetByUserIdAsync(userId);
    }

    public async Task<Organization> CreateAsync(Organization organization)
    {
        if (organization.Members.Count == 0)
            throw new ArgumentException("Organization must have at least one member");
        
        return await _organizationRepository.CreateAsync(organization);
    }

    public async Task UpdateAsync(Organization organization)
    {
        await _organizationRepository.UpdateAsync(organization);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _organizationRepository.DeleteAsync(id);
    }

    public async Task<bool> UserHasAccessAsync(string userId, Guid organizationId)
    {
        var organization =  await _organizationRepository.GetByIdAsync(organizationId);
        
        var member = organization?.Members.FirstOrDefault(u => u.Id == userId);
        
        return member != null;
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _organizationRepository.GetByIdAsync(id) != null;
    }
}