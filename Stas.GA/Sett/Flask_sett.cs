
using Newtonsoft.Json;

namespace Stas.GA;

public partial class Settings : iSett {
    [JsonProperty]
    public bool b_use_left_flasks = false;
    [JsonProperty]
    public bool b_use_right_flasks = false;
    [JsonProperty]
    public bool b_use_life_flask = false;
    [JsonProperty]
    public bool b_use_mana_flask = false;
    [JsonProperty]
    public bool b_use_silver_flask = false;
    [JsonProperty]
    public int trigger_life_left_persent = 50;
    [JsonProperty]
    public int cast_price = 20;
    [JsonProperty]
    public int silver_gdist = 100;
    [JsonProperty]
    public Keys two_left_flask_key = Keys.Q;
    [JsonProperty]
    public Keys two_right_flask_key = Keys.W;
    public Keys life_flask_key { get; set; } = Keys.E;
    public Keys mana_flask_key { get; set; } = Keys.F6;
    public Keys silver_flask_key { get; set; } = Keys.F5;
    [JsonProperty]
    public Keys flask_0_key = Keys.F5;
    [JsonProperty]
    public Keys flask_1_key = Keys.F6;
    [JsonProperty]
    public Keys flask_2_key = Keys.E;
    [JsonProperty]
    public Keys flask_3_key = Keys.F7;
    [JsonProperty]
    public Keys flask_4_key = Keys.F8;
}
