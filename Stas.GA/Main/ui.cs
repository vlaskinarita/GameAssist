using ImGuiNET;
using Stas.GA.Main;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Xml.Linq;
using V2 = System.Numerics.Vector2;
namespace Stas.GA;
public partial class ui {
    public static ServerData server_data => curr_map.server_data;
    static string tName = "ui";

    //public static ExpedSett exped_sett;
    public static V2 my_last_gpos;
    static uint next_task_id = 0;
    public static uint GetNextTaskId => next_task_id += 1;
    public static void ResetNextTaskId() { next_task_id = 0; }
    public static ICollection<Entity> entities => curr_map.AwakeEntities.Values;
    public static Entity curr_link;
    [Obsolete("try use b_can_safe_hit_ent")]
    public static bool b_can_safe_hit_gp(V2 tgp) {
        var isee = b_i_see_safe_tgp(tgp);
        var can_hit = nav.b_can_hit(tgp);
        return isee && can_hit;
    }
    public static int w8_top { get; private set; } = 0;
    public static bool b_w8_top;
    public static void SetDebugPossible(Action act) {
        w8_top = 0;
        var w8 = 1;
        var top_error = false;
        while (!b_game_top) {
            SetTop(game_ptr, w8);//"prevent for debug"
            w8_top += w8;
            if (w8_top > 1000) {
                top_error = true;
                return;
            }
        }
        if (!top_error && act != null)
            act.Invoke();
        else
            ui.AddToLog("SetDebugPossible err for:" + act.ToString(), MessType.Error);
    }
    /// <summary>
    /// its temporary only method who wold make alot of exeption, 
    /// coz we have f bad back conversion and get Z coorfinate from my pos - dont use it!!
    /// </summary>
    /// <param name="tgp"></param>
    /// <returns></returns>
    public static bool b_i_see_safe_tgp(V2 tgp) {
        var sp = TgpToSP(tgp);
        return b_sp_is_safe(sp);
    }
    public static bool b_sp_is_safe(V2 sp) {
        foreach (var b in gui.safe_screen.Blocks.Values) {
            if (b != null && b.Insade(sp))
                return false;
        }
        return sp.PointInRectangle(game_window_rect);
    }
    public static Leader leader = new Leader();
    /// <summary>
    /// disables the overlay's ability to lock the cursor - for further debugging
    /// </summary>
    public static bool SetTop(IntPtr ptr, int one_w8=1) {
        Debug.Assert(ptr != IntPtr.Zero);
        if (ptr == game_ptr) {
            DrawMain.scene.sdl_window.SetOverlayClickable(false);
        }
        int try_count = 1;
        var max_try = 500 / one_w8;
        IntPtr curr_ptr() => EXT.GetForegroundWindow();
        while (curr_ptr() != ptr) {
            EXT.SetWindowToTop(ptr);
            Thread.Sleep(one_w8);
            try_count += 1;
            AddToLog("Try set top...[" + try_count + "]", MessType.Warning);
            if (try_count > max_try) {//one sec
                AddToLog("Try set top for", MessType.Error);
                b_w8_top = false;
                return false;
            }
        }
        //AddToLog("SetTop was OK");
        return true;
    }
    public static int curr_life_percent {
        get {
            if (me == null || !me.IsValid || worker == null || life == null)
                return 0;
            if (worker.b_use_low_life)
                return life.EnergyShield.CurrentInPercent;
            return life.EnergyShield.CurrentInPercent + life.Health.CurrentInPercent;
        }
    }
    public static Life life {
        get {
            me.GetComp<Life>(out var _life);
            //Debug.Assert(_life != null);
            return _life;
        }
    }
    public static int min_new_skill_id { get; set; } = -1;
    public static Element test_elem;
    static public List<RemoteObjectBase> need_upd_per_frame = new List<RemoteObjectBase>();
    public static uint frame_count { get; private set; }
    public static SoundPlayer sound_player = new SoundPlayer();
    public static UdpSound udp_sound;
    public static PoeNinja ninja = new PoeNinja();
    public static aTasker tasker { get; private set; }
    static UdpListener udp_master;
    static UdpBot udp_bot;

    #region Quest(Importand map zone loading here)
    static public Quest quest;
    /// <summary>
    /// Must be running in new thred mb
    /// </summary>
    static public void LoadQuest() {
        quest = new Quest().Load<Quest>(); ;
    }

    static public void SaveQuest() {
        quest?.Save();
    } 
    #endregion
   
    public static AreaInstance curr_map => states.ingame_state.area_instance;
    public static WorldData curr_world => states.ingame_state.world_data;
    public static string curr_map_id {
        get {
            if (curr_world.world_area.Address == default) {
                AddToLog(tName + ".curr_map_id can't read=> worng ptr");
                return "Err id";
            }
            return curr_world.world_area.Id; //name
        }
    }
    /// <summary>
    /// we need it for save visited nav rout/lot/etc on map if we planning TP to HO and than Back to map
    /// </summary>
    public static uint curr_map_hash {
        get {
            return curr_map.AreaHash;
        }
    }
    
    public static string curr_map_name {
        get {
            if (curr_world.world_area.Address == default) {
                AddToLog(tName + ".curr_map_name can't read=> worng ptr");
                return "Err name";
            }
            return curr_world.world_area.Name; //name
        }
    }
    public static aWorker worker { get; private set; }
    public static gState curr_state => states.curr_gState;
    public static Role curr_role { get; }
    public static int max_pp { get; private set; }
    public static Looter looter { get; } 
    public static GameUiElements gui => states.ingame_state.gui;
    static InputChecker input_check = new InputChecker();
    public static string warning;
    public static Tests test = new Tests();
    public static NavMesh nav = new NavMesh();
    public static Entity me { get { return curr_map.player; } }
    //TileStructure TileToGridConversion & TileToWorldConversion
    public static float gridToWorldScale => 10.869565f;
    public static float worldToGridScale => 0.092f; //250f /23 
    public static float aura_gdist => 55f;
  
    public static Camera camera => curr_world.camera;
    public static unsafe IntPtr GetPtrFromImageData(Bitmap bmp) {
        return DrawMain.scene.LoadImageToPtr(bmp).Item1;
    }
    public static void ReloadGameState() {
        ThreadPool.QueueUserWorkItem(new WaitCallback(curr_map.UpdateMap));
    }
    #region Bots
    public static BotInfo bi;
    public static ConcurrentBag<BotInfo> bots = new ConcurrentBag<BotInfo>();
    public static void SendToBots(Opcode opc) {
        if (curr_role != Role.Master)
            return;
        foreach (var b in bots)
            b.Send(opc);
    }
    public static void SendToBots(Opcode opc, bool bval) {
        if (curr_role != Role.Master)
            return;
        var ba = new byte[] { 0 };
        if (bval)
            ba[0] = 1;
        foreach (var b in bots)
            b.Send(opc, ba);
    }

    public static void SendToBots(Opcode opc, byte[] ba, bool b_same_map = true) {
        if (curr_role != Role.Master)
            return;
        foreach (var b in bots) {
            if (!b_same_map || (b_same_map && b.map_hash == curr_map_hash))
                b.Send(opc, ba);
        }
    }

    #endregion
    #region LOG
    public static FixedSizedLog log { get; } = new FixedSizedLog(15);
    public static void ClearLog() {
        log.Clear();
    }

    public static void AddToLog(string str, MessType _mt = MessType.Ok) {
        log.Add(str, _mt);
    }
    #endregion

    internal static Settings sett { get; private set; }

    /// <summary>
    ///     Gets the GameStates instance [0]
    /// </summary>
    public static GameStates states { get; } = new(IntPtr.Zero);

    /// <summary>
    ///     Gets the files loaded for the current area.
    /// </summary>
    public static LoadedFiles curr_loaded_files { get; private set; } 
    public static PreloadAlert alert => curr_loaded_files.alert;

    /// <summary>
    ///     Gets the AreaChangeCounter instance. For details read class description.
    /// </summary>
    internal static AreaChangeCounter area_change_counter { get; private set; } 

    /// <summary>
    ///     Gets the values associated with the Game Window Scale.
    /// </summary>
    internal static GameWindowScale GameScale { get; } = new(IntPtr.Zero);

    /// <summary>
    ///     Gets the values associated with the terrain rotation selector.
    /// </summary>
    internal static TerrainHeightHelper RotationSelector { get; } = new(IntPtr.Zero, 8);

    /// <summary>
    ///     Gets the values associated with the terrain rotator helper.
    /// </summary>
    internal static TerrainHeightHelper RotatorHelper { get; } = new(IntPtr.Zero, 24);

   
    static List<double> elapsed = new();

    /// <summary>
    ///     Converts the RemoteObjects to ImGui Widgets.
    /// </summary>
    internal static void RemoteObjectsToImGuiCollapsingHeader() {
        const BindingFlags propertyFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static;
        foreach (var property in RemoteObjectBase.GetToImGuiMethods(typeof(ui), propertyFlags, null)) {
            if (ImGui.CollapsingHeader(property.Name)) {
                property.ToImGui.Invoke(property.Value, null);
            }
        }
    }
}
public class aButtonDebug {
    public Keys key;
    public int down_count;
    public int up_count;
    public FixedSizedLog log { get; protected private set; }

    public aButtonDebug(Keys _key) {
        key = _key;
        log = new FixedSizedLog(10);
    }
}

public class CodeAtt : Attribute {
    public string info;
    public CodeAtt(string _info) {
        this.info = _info;
    }
}
