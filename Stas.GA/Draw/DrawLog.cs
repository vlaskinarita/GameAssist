using ImGuiNET;
using Color = System.Drawing.Color;
namespace Stas.GA;
partial class DrawMain {
    void DrawLog(FixedSizedLog log) {
        var bptr = ImGui.GetWindowDrawList();
        foreach (var l in log) {
            if (l.mtype == MessType.Ok && !ui.sett.b_log_info)
                continue;
            if (l.mtype == MessType.Warning && !ui.sett.b_log_warn)
                continue;
            if (l.mtype == MessType.Error && !ui.sett.b_log_error)
                continue;
            //if (l.mtype == MessType.Critical && !ui.sett.b_log_critical)
            //    continue;
            var ac = Color.FromArgb(255, 10, 10, 10).ToImgui();
            if (l.mtype == MessType.Warning)
                ac = Color.FromArgb(100, Color.Orange).ToImgui();
            if (l.mtype == MessType.Error)
                ac = Color.FromArgb(100, Color.Red).ToImgui();
            if (l.mtype == MessType.Critical)
                ac = Color.FromArgb(100, Color.Purple).ToImgui();
            var text = l.info + "\n";
            if (l.count != 0)
                text = l.info + " (" + l.count + ")\n";
            var sp = ImGui.GetCursorScreenPos();
            var ts = ImGui.CalcTextSize(text);
            var lt = sp;
            var rt = sp.Increase(ts.X, 0);
            var rb = sp.Increase(ts.X, ts.Y);
            var lb = sp.Increase(0, ts.Y);
            bptr.AddQuadFilled(lt, rt, rb, lb, ac);
            ImGui.Text(text);
        }
        //=================== new line =======================
        if (ImGui.Button("Clear")) {
            ui.log.Clear();
        }
        ImGuiExt.ToolTip("cleare all current messages");

        ImGui.SameLine();
        if (ImGui.Checkbox("Info", ref ui.sett.b_log_info)) {
            ui.sett.Save();
        }
        ImGuiExt.ToolTip("Show/hide info messages[gray]");

        ImGui.SameLine();
        if (ImGui.Checkbox("Warn", ref ui.sett.b_log_warn)) {
            ui.sett.Save();
        }
        ImGuiExt.ToolTip("Show/hide warning messages[yellow]");

        ImGui.SameLine();
        if (ImGui.Checkbox("Err", ref ui.sett.b_log_error)) {
            ui.sett.Save();
        }
        ImGuiExt.ToolTip("Show/hide Error messages[red]");

        //ImGui.SameLine();
        //ImGui.PushStyleColor(ImGuiCol.CheckMark, Color.Gray.ToImgui());
        //ImGui.Checkbox("Critical", ref ui.sett.b_log_critical);
        //ImGui.PopStyleColor();
        //ImGuiExt.ToolTip("Critical logs[purple] - cant be disabled");
    }
}