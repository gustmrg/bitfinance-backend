using BitFinance.Domain.Exceptions;

namespace BitFinance.Domain.ValueObjects;

public record TimeZoneId
{
    public string Value { get; }

    public TimeZoneId(string value)
    {
        if (!TimeZoneInfo.TryFindSystemTimeZoneById(value, out _))
            throw new DomainException($"Invalid timezone: {value}");
        
        Value = value;
    }
}