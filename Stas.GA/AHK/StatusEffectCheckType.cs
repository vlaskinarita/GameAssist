using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Stas.GA;


/// <summary>
///     Check type for the condition
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
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
