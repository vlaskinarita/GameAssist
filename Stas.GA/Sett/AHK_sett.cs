using Newtonsoft.Json;
namespace Stas.GA;

public partial class AHKSettings : iSett {
    [JsonProperty]
    public Keys DumpStatusEffectOnMe = Keys.K;
    [JsonProperty]
    public bool EnableAutoQuit = false;
    [JsonProperty]
    public VitalsCondition AutoQuitCondition { get; set; } = new(OperatorType.LESS_THAN, VitalType.LIFE_PERCENT, 30);
    [JsonProperty]
    public Keys AutoQuitKey = Keys.F9;

    [JsonProperty]
    public readonly Dictionary<string, Profile> Profiles =   new();

    [JsonProperty]
    public string CurrentProfile = string.Empty;

    [JsonProperty]
    public bool ahk_DebugMode = false;

    [JsonProperty]
    public bool ShouldRunInHideout = true;
}
