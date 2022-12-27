using ImGuiNET;
using System.Drawing;
using V2 = System.Numerics.Vector2;
using sh = Stas.GA.SpriteHelper;
namespace Stas.GA;
partial class DrawMain {
    //https://github.com/ocornut/imgui/blob/169e3981fdf037b179f1c7296548892ba7837dae/imgui_demo.cpp#L3871-L3978

    bool on_top => ui.b_game_top || ui.b_imgui_top;
    bool b_map => ui.curr_map != null && ui.curr_map.b_ready;
    V2 my_display_res;
    void DrawMap() {
        my_display_res = new V2(ui.game_window_rect.Width, ui.game_window_rect.Height);
        ImGui.SetNextWindowContentSize(my_display_res);
        ImGui.SetNextWindowPos(new V2(ui.w_offs.X, ui.w_offs.Y));
        ImGui.Begin(
            "Background Screen",
                ImGuiWindowFlags.NoInputs |
                ImGuiWindowFlags.NoBackground |
                ImGuiWindowFlags.NoBringToFrontOnFocus |
                ImGuiWindowFlags.NoCollapse |
                ImGuiWindowFlags.NoMove |
                ImGuiWindowFlags.NoScrollbar |
                ImGuiWindowFlags.NoSavedSettings |
                ImGuiWindowFlags.NoResize |
                ImGuiWindowFlags.NoTitleBar);

        map_ptr = ImGui.GetWindowDrawList();
        if (ui.b_game_top || ui.b_imgui_top) { // && ui.b_debug
            DrawDebug();
        }
       
        if (b_map && on_top && !ui.b_busy//&& !ui.sett.test_draw
            && !ui.sett.b_draw_bad_centr  && !ui.b_draw_save_screen) { 
            DrawMePos();
            DrawMapContent();
            b_cant_draw_map = false;
        }
        else {
            b_cant_draw_map = true;
            ui.AddToLog("can't draw map..", MessType.Warning);
        }
        ImGui.End();
    }
    void DrawMePos() {
        if (!ui.sett.b_draw_me_pos)
            return;
        var cpa = ui.curr_map.me_pos.ToArray();//thread safe copy of
        if (cpa.Length < 4)
            return;
        var rm = ui.MTransform();
        for (int i = 0; i < cpa.Length - 1; i++) {
            var p1 = V2.Transform(cpa[i], rm);
            var p2 = V2.Transform(cpa[i + 1], rm);
            map_ptr.AddLine(p1, p2, Color.Red.ToImgui(), 2f);
        }
    }
    SW sw_map = new SW("Map");
    void DrawMapContent() {
        if (ui.me.Address==default) {
            ui.AddToLog("draw Map Err: ui.me==null", MessType.Error);
            return;
        }
        sw_map.Restart();
        //ui.me.Tick(ui.me.Address);//we tick here mb for more precision
      
        var rm = ui.MTransform(); //true
        var lt = V2.Transform(new V2(0f, 0f), rm);
        var rt = V2.Transform(new V2(ui.curr_map.cols, 0f), rm);
        var rb = V2.Transform(new V2(ui.curr_map.cols, ui.curr_map.rows), rm);
        var lb = V2.Transform(new V2(0f, ui.curr_map.rows), rm);
        if (ui.curr_map.map_ptr != IntPtr.Zero)
            map_ptr.AddImageQuad(ui.curr_map.map_ptr, lt, rt, rb, lb);

        var di = ui.curr_map.mi_debug;//local snap this thred. mb need use lock(){}
        if (di != null && ui.sett.b_develop) {
            DrawDebugMapItem(map_ptr, di);
            return;
        }
        DrawNavVisited();

        var mia = ui.curr_map.map_items.OrderBy(e => e.priority);
        foreach (var mi in mia) {
            DrawMapItem(mi);
        }

        DtawStaticItems();

        foreach (var it in ui.curr_map.iTasks) {
            var to = V2.Transform(it.to, ui.MTransform());
            var his = 12;

            var from = V2.Transform(it.from, ui.MTransform());
            map_ptr.AddLine(from, to, it.color.ToImgui(), it.line);
            map_ptr.AddCircleFilled(to, 5, Color.Gray.ToImgui());

            map_ptr.AddText(to.Increase(his, -his / 2), Color.LightGreen.ToImgui(), it.info);
        }
    }
    void DtawStaticItems() {
        var sorted = ui.curr_map.static_items.Values.OrderBy(i => i.priority)
            .ThenBy(i => i.gdist_to_me).ToArray();
        foreach (var mi in sorted) {
            var exped = mi.m_type == miType.ExpedArtifact
                    || mi.m_type == miType.ExpedMarker
                    || mi.m_type == miType.ExpedRemnant;
            if (exped && ui.curr_map.danger > 0)
                continue;
            if (!mi.WasDeleted())
                DrawMapItem(mi);
        }
    }
}