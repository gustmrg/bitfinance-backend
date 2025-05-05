using BitFinance.Domain.Repositories;
using BitFinance.Domain.Services;
using Microsoft.Extensions.Logging;

namespace BitFinance.Infrastructure.Services;

public class OrganizationsService : IOrganizationsService
{
    private readonly IOrganizationsRepository _organizationsRepository;
    private readonly ILogger<OrganizationsService> _logger;
    
    public OrganizationsService(IOrganizationsRepository organizationsRepository, ILogger<OrganizationsService> logger)
    {
        _organizationsRepository = organizationsRepository;
        _logger = logger;
    }
    
    public async Task<bool> ValidateOrganizationExistsAsync(Guid organizationId)
    {
        try
        {
            return await _organizationsRepository.GetByIdAsync(organizationId) != null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating organization existence: {OrganizationId}", organizationId);
            return false;
        }
    }

    public async Task<bool> ValidateUserBelongsToOrganizationAsync(string userId, Guid organizationId)
    {
        try
        {
            var organization = await _organizationsRepository.GetByIdAsync(organizationId);
            
            if (organization == null)
            {
                return false;
            }

            return organization.Members.Any(m => m.Id == userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating user organization membership: User {UserId}, Organization {OrganizationId}", 
                userId, organizationId);
            
            return false;
        }
    }
}