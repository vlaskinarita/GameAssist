using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Stas.GA;
public partial class ui {
    [DllImport("Stas.GA.Native.dll", SetLastError = true, EntryPoint = "Start")]
    public static extern int Start(int pid, bool debug);
    public static IntPtr first_children_offset = 0x30;
    public static int elem_text_offs = 0x3F8;
    public static int IsVisibleLocalOffs = 0x161;
    static readonly Dictionary<string, MapIconsIndex> Icons;
    public static ConcurrentDictionary<IntPtr, Element> elements = new();
    public static ConcurrentDictionary<IntPtr, string> texts = new();
    public static ConcurrentDictionary<IntPtr, DateTime> w8ting_click_until = new();
    public static ConcurrentDictionary<StdWString, string> std_wstrings = new();
    public static MapIconsIndex IconIndexByName(string name) {
        name = name.Replace(" ", "").Replace("'", "");
        Icons.TryGetValue(name, out var result);
        return result;
    }
    static Thread frame_thread;
    internal static Memory m { get; private set; }
    public static int w8 { get; } = 16;////1000 / 60 = 16(60 frame sec)
    public static List<Keys> flask_keys = new();
    public static AHK ahk { get; private set; }
    static ui() {
        Icons = new Dictionary<string, MapIconsIndex>(200);
        foreach (var icon in Enum.GetValues(typeof(MapIconsIndex))) {
            Icons[icon.ToString()] = (MapIconsIndex)icon;
        }
       
        int w8_err = 300;
        sett = new Settings().Load<Settings>();
        sett.b_use_ingame_map = false;
        sett.b_develop = true; //<<==change if you're a developer
        flask_keys.AddRange(new List<Keys>(){sett.flask_0_key, sett.flask_1_key,
            sett.flask_2_key, sett.flask_3_key, sett.flask_4_key});
        var need_ea = new List<Element>() {gui.map_devise, gui.KiracMission, gui.open_left_panel, gui.open_right_panel,
                        gui.passives_tree, gui.NpcDialog, gui.LeagueNpcDialog, gui.BetrayalWindow, gui.large_map,
                        gui.AtlasPanel, gui.AtlasSkillPanel,gui.DelveWindow,gui.TempleOfAtzoatl };
        if (!sett.b_use_ingame_map)
            need_ea.Add(gui.large_map);
        gui.AddToNeedCheck(need_ea);
        udp_sound = new UdpSound();
        StartGameWatcher();
        SetRole();
        looter = new Looter();
        ahk = new AHK();
        need_upd_per_frame = new List<RemoteObjectBase>() {   }; //camera//gui
        var game_not_loadin = 0;

        frame_thread = new Thread(() => {
            while (b_running) {
                frame_count += 1;
                if (game_ptr == IntPtr.Zero) {
                    game_not_loadin += 1;
                    if (game_not_loadin > 100) {
                        AddToLog("w8 game not loading... ", MessType.Critical);
                        AddToLog("Use [Alt]+[Shift] to activate this window", MessType.Critical);
                    }
                    Thread.Sleep(200);
                    continue;
                }
                if(states.b_ready)
                    states.Tick(states.Address, "frame thread");

                if (curr_state == gState.InGameState) {
                    foreach (var n in need_upd_per_frame)
                        n?.Tick(n.Address, "frame thread");
                    CheckWorker();
                    if (worker == null) {
                        ui.AddToLog("Frame err: worker need be setup", MessType.Warning);
                        Thread.Sleep(w8);
                        continue;
                    }
                    CheckFlasks(false);
                    CheckMapPlayers();
                }

                #region tick timer & w8ting for relax CPU
                var d_elaps = sw.Elapsed.TotalMilliseconds;
                elapsed.Add(d_elaps);
                if (elapsed.Count > 60)
                    elapsed.RemoveAt(0);
                var frame_time = elapsed.Sum() / elapsed.Count;
                if (frame_time < w8) {
                    Thread.Sleep(w8 - (int)frame_time);
                }
                else {
                    Thread.Sleep(1);
                    AddToLog("Input: Big Tick Time", MessType.Error);
                }
                #endregion
            }
        });
        frame_thread.IsBackground = true;
        frame_thread.Start();
    }
   
    public static void Dispose() {
        CloseGame();
        try {
            b_running = false;
            Thread.Sleep(w8 * 10);
          
        }
        catch (Exception ex) {
        }
    }
}


