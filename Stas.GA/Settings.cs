using System.Text.Json.Serialization;
namespace Stas.GA;

public partial class Settings : iSett {

    #region Flasks setup
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
    #endregion

    #region AHK
    [JsonInclude]
    public bool b_use_gh_map = false;
    [JsonInclude]
    public bool b_use_gh_flask = true;
    [JsonInclude]
    public Keys DumpStatusEffectOnMe = Keys.K;
    [JsonInclude]
    public bool EnableAutoQuit= false;

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
    #endregion

    [JsonInclude]
    public Role role = Role.None;
    public string pp_name = "PathOfExile"; //PathOfExile_KG PathOfExileSteam PathOfExile_x64 if we support it
    public string poe_w_name = "Path of Exile";
    public string icons_fname { get; set; } = @"Icons.png";
    public string title_name { get; set; } = "Master_3.19.txt";
    public string curr_league { get; set; } = "Kalandra";
    public string ninja_price { get; set; } = @"priceQueue.txt";
    public string preload_fname { get; set; } = @"preloads.txt";
    public string sounds_dir { get; set; } = @"Sounds\";
    public string log_fname { get; set; } = @"bot.log";
    public string master_IP { get; set; } = "192.168.1.2";

    [JsonInclude]
    public bool b_develop = false;
    [JsonInclude]
    public float info_font_size = 1f;
   
    [JsonInclude]
    public bool b_use_Edge_only = false;

    public int master_port { get; set; } = 8888;

    public Keys rotate_map_clockwise { get; set; } = Keys.F5;
    public Keys rotate_map_counterclockwise { get; set; } = Keys.F6;
    public Keys zoom_in { get; set; } = Keys.F7;
    public Keys zoom_out { get; set; } = Keys.F8;
 
    [JsonInclude]
    public bool b_use_keybord_for_zoom =false;
    [JsonInclude]
    public int visited_persent = 10;
    [JsonInclude]
    public bool b_gui_debug_on_top = false;
    [JsonInclude]
    public bool b_draw_map_fps = true;
    [JsonInclude]
    public bool b_draw_ent_fps = true;
    [JsonInclude]
    public bool b_draw_log_first = true;
    [JsonInclude]
    public bool b_log_info = false;
    [JsonInclude]
    public bool b_log_warn = true;
    [JsonInclude]
    public bool b_log_error = true;
    [JsonInclude]
    public bool b_map_interpolate = true;
    [JsonInclude]
    public bool b_show_unknow = true;
    [JsonInclude]
    public bool b_show_frendly_mobs = true;
    [JsonInclude]
    public bool b_draw_me_pos = false;
    public float def_aura_range = 50;
    public string last_leader_name { get; set; }
    public float max_entyty_valid_gdistance = 235; //150
  
    [JsonInclude]//only one party member can pull and be a tank
    public bool b_can_pull_alone;
    [JsonInclude]
    public bool b_open_door;
    [JsonInclude]
    public bool b_auto_loot = false;
    public bool b_get_wp { get; set; } = false;
   
    public float close_gdist { get; set; } = 6f;
    public float home_gdist { get; set; } = 25f;
    public float danger_gdist = 60f;
    public float buff_gdist = 50f;
    public float loot_gdist = 80f;
    [JsonInclude]
    public MasterNams master_name = MasterNams.GF1030;
   
    /// <summary>
    /// current POE window index(if running two of POE on one system)
    /// </summary>
    public int last_pp_index { get; set; } = 0;
   
    [JsonInclude]
    public bool b_show_iTask = false;
    [JsonInclude]
    public bool b_can_play_sound = false;
    [JsonInclude]
    public float center_fix_dist = -18f;
    [JsonInclude]
    public float center_fix_k = 1f;
    [JsonInclude]
    public float icon_size = 10;
    [JsonInclude]
    public bool b_draw_mouse_moving = false;
    [JsonInclude]
    public bool b_show_info_over = false;
    [JsonInclude]
    public bool b_draw_misk = false;
    [JsonInclude]
    public bool b_draw_proj = false;
    [JsonInclude]
    public bool b_draw_useles = false;
    [JsonInclude]
    public bool b_draw_bad_centr = false;
    [JsonInclude]
    public bool b_get_next_pack;
    [JsonInclude]
    public bool b_get_not_visited;
    [JsonInclude]
    public bool b_auto_top;
    [JsonInclude]
    public bool b_alt_reset = false;
    [JsonInclude]
    public bool b_auto_close_modals = false;
    [JsonInclude]
    public bool b_debug = false;
    [JsonInclude]
    public bool b_get_chest;
    [JsonInclude]
    public bool b_hit_barrels;
    [JsonInclude]
    public bool b_auto_buff = true;
    [JsonInclude]
    public bool b_auto_rise = false;
       
    public float map_angle = 50f;             
    public float map_scale_def = 1.8f;
    public float map_scale = 1.8f;
    public bool DrawWhenForeground = false;
    public bool DrawWhenNotInHideoutOrTown = false;


    /// <summary>
    ///     Gets or sets a value indicating whether to hide
    ///     the performance stats window when game is in background.
    /// </summary>
    public bool HidePerfStatsWhenBg = false;

    /// <summary>
    ///     Gets a value indicating whether user wants to hide the overlay on start or not.
    /// </summary>
    public bool HideSettingWindowOnStart = false;

    /// <summary>
    ///     Gets or sets a value indicating whether the overlay is running or not.
    /// </summary>
    [JsonIgnore]
    public bool IsOverlayRunning = true;

    /// <summary>
    ///     Gets a value indicating how much time to wait between key presses.
    /// </summary>
    public int KeyPressTimeout = 80;

    /// <summary>
    ///     Gets the font pathname to load in ImGui.
    /// </summary>
    public string FontPathName = @"C:\Windows\Fonts\msyh.ttc";

    /// <summary>
    ///     Gets the font size to load in ImGui.
    /// </summary>
    public int FontSize = 18;

    /// <summary>
    ///     Gets the custom glyph range to load from the font texture. This is useful in case
    ///     <see cref="FontGlyphRangeType"/> isn't enough. Set it to empty string to disable this
    ///     feature.
    /// </summary>
    public string FontCustomGlyphRange = string.Empty;

    /// <summary>
    ///     Gets or sets hotKey to show/hide the main menu.
    /// </summary>
    public ConsoleKey MainMenuHotKey = ConsoleKey.F12;

    /// <summary>
    ///     Gets or sets a value indicating whether
    ///     to show DataVisualization window or not.
    /// </summary>
    public bool ShowDataVisualization = false;

    /// <summary>
    ///     Gets or sets a value indicating whether
    ///     to show Game Ui Explorer or not.
    /// </summary>
    public bool ShowGameUiExplorer = false;

    /// <summary>
    ///     Gets or sets a value indicating whether to show
    ///     the performance stats or not.
    /// </summary>
    public bool ShowPerfStats = false;

    /// <summary>
    ///     Gets or sets a value indicating what nearby means to the user.
    /// </summary>
    public int NearbyMeaning = 70;

    /// <summary>
    ///     Gets a value indicating whether user wants to close the Game Helper when
    ///     the game exit or not.
    /// </summary>
    public bool CloseWhenGameExit = false;

}

public enum MasterNams {
    GF1030, //18:C0:4D:E4:B9:99
    LARS, //a4:50:56:3c:2b:22 192.168.1.13
    DAMAGE, //54:04:A6:B1:DC:1B
    CURSE, //A8:5E:45:E6:5F:42
    MANA, //18:C0:4D:96:37:20
    VOVA //12:50:56:20:75:51
}