using System.Text.Json.Serialization;
namespace Stas.GA;

public partial class AHKSettings : iSett {
    [JsonInclude]
    public Keys DumpStatusEffectOnMe = Keys.K;
    [JsonInclude]
    public bool EnableAutoQuit = false;
    [JsonInclude]
    public VitalsCondition AutoQuitCondition { get; set; } = new(OperatorType.LESS_THAN, VitalType.LIFE_PERCENT, 30);
    [JsonInclude]
    public Keys AutoQuitKey = Keys.F9;

    [JsonInclude]
    public readonly Dictionary<string, Profile> Profiles =   new();

    [JsonInclude]
    public string CurrentProfile = string.Empty;

    [JsonInclude]
    public bool ahk_DebugMode = false;

    [JsonInclude]
    public bool ShouldRunInHideout = true;
}
