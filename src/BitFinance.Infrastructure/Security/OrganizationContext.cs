using BitFinance.Infrastructure.Security.Interfaces;

namespace BitFinance.Infrastructure.Security;

public class OrganizationContext : IOrganizationContext
{
    private Guid _organizationId;

    public Guid OrganizationId => _organizationId;

    public void SetOrganization(Guid organizationId)
    {
        _organizationId = organizationId;
    }
}