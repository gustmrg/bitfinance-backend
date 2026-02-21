namespace BitFinance.Business.Enums;

/// <summary>
/// Defines the recurrence frequency for recurring bills or expenses.
/// </summary>
public enum Frequency
{
    /// <summary>
    /// Occurs every day.
    /// </summary>
    Daily = 0,

    /// <summary>
    /// Occurs every week.
    /// </summary>
    Weekly = 1,

    /// <summary>
    /// Occurs every month.
    /// </summary>
    Monthly = 2,

    /// <summary>
    /// Occurs once a year.
    /// </summary>
    Annually = 3
}
