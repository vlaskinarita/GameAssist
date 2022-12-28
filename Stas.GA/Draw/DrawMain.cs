using ImGuiNET;
using Stas.ImGuiNet;
using System.IO;
using System.Reflection;
using System.Text;
namespace Stas.GA;
public partial class DrawMain {
    public IntPtr icons;
    public string title_name { get; }
    public static SimpleImGuiScene scene;
    public static bool b_ready = false;
    public DrawMain() {
        title_name = ui.sett.title_name + " - Notepad";
        var exe_dir = Assembly.GetExecutingAssembly().Location;
        var dir = Path.GetDirectoryName(exe_dir);
        var fname = Path.Combine(dir, ui.sett.icons_fname);
        scene = SimpleImGuiScene.CreateOverlay(title_name);
        icons = scene.LoadImageToPtr(fname).Item1;
        scene.sdl_window.SetOverlayClickable(false);
        b_ready = true;

        //scene.OnBuildUI += ImGui.ShowDemoWindow;
        scene.OnBuildUI += Draw;
        scene.Run();
    }
    ImDrawListPtr map_ptr;
    StringBuilder sp_warn = new StringBuilder();
    SW sw_main = new SW("Draw Map:");
    int fi = 0;
    public void Draw() {
        var b_top = ui.b_game_top || ui.b_imgui_top;
        if (ui.b_w8_top && ui.b_game_top)
            ui.b_w8_top = false;
        var b_only_alt = ui.b_alt && !ui.b_shift;
        var test_ui_elem = ui.test_elem != null && ui.sett.b_gui_debug_on_top && !ui.b_alt;
        if (test_ui_elem || (b_top && ui.b_alt_shift && !b_only_alt) || (ui.b_ga_menu_top && !b_only_alt)) { //  &&
            scene.sdl_window.SetOverlayClickable(true);
        }
        else {
            scene.sdl_window.SetOverlayClickable(false);
        }
        if (ui.b_vs) {
            return;
        }
        if (ui.test_elem != null && ui.sett.b_develop) { // && ui.tasker.ui_root.IsValid
            ui.test_elem.Tick(ui.test_elem.Address, "debug_gui");
            GetUiFrames();
            DrawUiFrames();
            DrawInfo();
            return;
        }
        sw_main.Restart();
        DrawMap(); //we need init map_ptr for debug same on it window, so check map visible cond insade
        sw_main.MakeRes();
        sp_warn.Clear();
        var me_wrong = ui.me == null || ui.me.Address == 0 || !ui.me.IsValid;
        if (!on_top) sp_warn.Append("NOT on top... ");
        if (me_wrong) sp_warn.Append("me is Wrong... ");
        if (ui.sett.b_debug) sp_warn.Append("w8 debug... ");
        if (!ui.b_ingame) sp_warn.Append("NOT in game... ");

        ui.warning = sp_warn.ToString();
        if ((on_top && !ui.b_busy) || ui.b_show_info_over)
            DrawInfo();

    }
}