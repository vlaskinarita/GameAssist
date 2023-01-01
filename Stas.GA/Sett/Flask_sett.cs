using System.Text.Json.Serialization;
namespace Stas.GA;

public partial class Settings : iSett {
    [JsonInclude]
    public bool b_use_left_flasks = false;
    [JsonInclude]
    public bool b_use_right_flasks = false;
    [JsonInclude]
    public bool b_use_life_flask = false;
    [JsonInclude]
    public bool b_use_mana_flask = false;
    [JsonInclude]
    public bool b_use_silver_flask = false;
    [JsonInclude]
    public int trigger_life_left_persent = 50;
    [JsonInclude]
    public int cast_price = 20;
    [JsonInclude]
    public int silver_gdist = 100;
    [JsonInclude]
    public Keys two_left_flask_key = Keys.Q;
    [JsonInclude]
    public Keys two_right_flask_key = Keys.W;
    public Keys life_flask_key { get; set; } = Keys.E;
    public Keys mana_flask_key { get; set; } = Keys.F6;
    public Keys silver_flask_key { get; set; } = Keys.F5;
    [JsonInclude]
    public Keys flask_0_key = Keys.F5;
    [JsonInclude]
    public Keys flask_1_key = Keys.F6;
    [JsonInclude]
    public Keys flask_2_key = Keys.E;
    [JsonInclude]
    public Keys flask_3_key = Keys.F7;
    [JsonInclude]
    public Keys flask_4_key = Keys.F8;
}
