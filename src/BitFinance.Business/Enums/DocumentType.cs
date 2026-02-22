namespace BitFinance.Business.Enums;

/// <summary>
/// Classifies the type of document attached to a bill.
/// </summary>
public enum DocumentType
{
    /// <summary>
    /// A Brazilian boleto bancário (bank payment slip).
    /// </summary>
    Boleto = 1,

    /// <summary>
    /// A payment receipt or proof of payment.
    /// </summary>
    Receipt = 2,

    /// <summary>
    /// Any other document type not specifically categorized.
    /// </summary>
    Other = 99
}
