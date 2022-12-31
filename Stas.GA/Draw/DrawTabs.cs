using ImGuiNET;
using System.Drawing;

namespace Stas.GA;
partial class DrawMain {
    bool b_cant_draw_map;
    void DrawTabs() {
        void _draw_alert() {
            if (ImGui.BeginTabItem("PA")) {
                DrawAllert();
                ImGui.EndTabItem();
            }
            ImGuiExt.ToolTip("Show/hide preload alerts");
        }
        void _draw_log() {
            if (ImGui.BeginTabItem("Log")) {
                DrawLog(ui.log);
                ImGui.EndTabItem();
            }
            ImGuiExt.ToolTip("Show/hide log lines");
        }
        if (ImGui.BeginTabBar("Tabs", ImGuiTabBarFlags.AutoSelectNewTabs)) {// | ImGuiTabBarFlags.Reorderable
         
            if (ui.sett.b_draw_log_first) {
                _draw_log();
                _draw_alert();
            }
            else {
                _draw_alert();
                _draw_log();
            }
            if (ImGui.BeginTabItem("Flasks")) {
                DrawFlasks();
                ImGui.EndTabItem();
            }
            if (ImGui.BeginTabItem("Sett")) {
                DrawSettings();
                ImGui.EndTabItem();
            }
            ImGuiExt.ToolTip("Show/hide settings panel");

            if (ImGui.BeginTabItem("Loot")) {
                DrawLootSett();
                ImGui.EndTabItem();
            }
            ImGuiExt.ToolTip("Show/hide loot settings");

            //if (ImGui.BeginTabItem("Exped")) {
            //    DrawExpedSett();
            //    ImGui.EndTabItem();
            //}
            //ImGuiExt.ToolTip("Show/hide exped settings");

            if (ImGui.BeginTabItem("Visual")) {
                DrawVisual();
                ImGui.EndTabItem();
            }
            ImGuiExt.ToolTip("Show/hide Shows the visual effects settings");

            if (ImGui.BeginTabItem("Debug")) {
                DrawDebugSett();
                ImGui.EndTabItem();
            }
            ImGuiExt.ToolTip("Show/hide debug setting");

            DrawDebugInfo();

            ImGui.EndTabBar();
        }

    }
}