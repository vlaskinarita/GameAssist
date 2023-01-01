using System;
using System.Text.Json.Serialization;
namespace Stas.GA;

public partial class AHKSettings : iSett {
  
    [JsonInclude]
    public Keys DumpStatusEffectOnMe = Keys.K;
    [JsonInclude]
    public bool EnableAutoQuit = false;

    public VitalsCondition AutoQuitCondition { get; set; } = new(OperatorType.LESS_THAN, VitalType.LIFE_PERCENT, 30);
    [JsonInclude]
    public Keys AutoQuitKey = Keys.F9;

    /// <summary>
    ///     Gets all the profiles containing rules on when to perform the action.
    /// </summary>
    public readonly Dictionary<string, Profile> Profiles = new();

    /// <summary>
    ///     Gets the currently selected profile.
    /// </summary>
    public string CurrentProfile = string.Empty;

    /// <summary>
    ///     Gets a value indicating weather the debug mode is enabled or not.
    /// </summary>
    public bool ahk_DebugMode = false;

    /// <summary>
    ///     Gets a value indicating weather this plugin should work in hideout or not.
    /// </summary>
    public bool ShouldRunInHideout = true;
}
