namespace BitFinance.Infrastructure.Security.Interfaces;

public interface IOrganizationContext
{
    Guid OrganizationId { get; }
    void SetOrganization(Guid organizationId);
}