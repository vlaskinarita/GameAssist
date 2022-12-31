namespace Stas.GA;


/// <summary>
///     Check type for the condition
/// </summary>
public enum StatusEffectCheckType
{
    /// <summary>
    ///     Check remaining buff duration
    /// </summary>
    DURATION,

    /// <summary>
    ///     Check remaning buff duration in percent
    /// </summary>
    DURATION_PERCENT,

    /// <summary>
    ///     Check buff charges
    /// </summary>
    CHARGES
}
