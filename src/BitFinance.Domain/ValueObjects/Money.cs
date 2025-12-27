using BitFinance.Domain.Exceptions;

namespace BitFinance.Domain.ValueObjects;

public record Money
{
    public decimal Amount { get; }
    public string Currency { get; } = "BRL";

    public Money(decimal amount, string currency = "BRL")
    {
        if (amount < 0)
            throw new DomainException("Amount cannot be negative");
        
        Amount = amount;
        Currency = currency;
    }
}