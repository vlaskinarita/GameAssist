#region using
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using ImGuiNET;
using Color = System.Drawing.Color;
#endregion
namespace Stas.GA { 
    partial class DrawMain {
        void DrawSettings() {
            ImGui.PushStyleColor(ImGuiCol.CheckMark, Color.Green.ToImgui());
            if (ImGui.Checkbox("top", ref ui.sett.b_auto_top)) {
                ui.sett.Save();
            }
            ImGuiExt.ToolTip("When a danger occurs, brings the game window to the foreground");

            ImGui.SameLine();
            if (ImGui.Checkbox("Buff", ref ui.sett.b_auto_buff)) {
                ui.sett.Save();
            }
            ImGuiExt.ToolTip("use buff after dead(checked only on maps)");

            ImGui.SameLine();
            if (ImGui.Checkbox("Rise", ref ui.sett.b_auto_rise)) {
                ui.sett.Save();
            }
            ImGuiExt.ToolTip("Auto rise after death");

            //ImGui.SameLine();
            //if (ImGui.Checkbox("Alt", ref ui.sett.b_alt_reset)) {
            //    ui.sett.Save();
            //}
            //ImGuiExt.ToolTip("Alt downing stop curr_task /reset tasker...");

            ImGui.SameLine();
            if (ImGui.Checkbox("Close modal", ref ui.sett.b_auto_close_modals)) {
                ui.sett.Save();
            }
            ImGuiExt.ToolTip("Auto close same modals dialog like esc menu");

            ImGui.SameLine();
            if (ImGui.Checkbox("Sound", ref ui.sett.b_can_play_sound)) {
                ui.sett.Save();
            }
            ImGuiExt.ToolTip("play sounds on this PC, otherwise on master server");
                      

            //================>new line
            if (ui.curr_role == Role.Master) {
                ImGui.Checkbox("bots", ref ui.b_draw_bots);
                ImGuiExt.ToolTip("show/hide botts F12");
                ImGui.SameLine();
            }

            if (ImGui.Checkbox("Door", ref ui.sett.b_open_door)) {
                ui.worker.Save();
            }
            ImGuiExt.ToolTip("Open doors");

            ImGui.SameLine();
            if (ImGui.Checkbox("Loot", ref ui.sett.b_auto_loot)) {
                ui.sett.Save();
            }
            ImGuiExt.ToolTip("pick up items according to the type configured in the loot settings");

            ImGui.SameLine();
            if (ImGui.Checkbox("Chest", ref ui.sett.b_get_chest)) {
                ui.sett.Save();
            }
            ImGuiExt.ToolTip("Open chest");

            ImGui.SameLine();
            if (ImGui.Checkbox("Barrel", ref ui.sett.b_hit_barrels)) {
                ui.sett.Save();
            }
            ImGuiExt.ToolTip("Hit barrel");

            ImGui.SameLine();
            if (ImGui.Checkbox("smooth map", ref ui.sett.b_map_interpolate)) {
                ui.sett.Save();
            }
            ImGuiExt.ToolTip("smooth out map panning");

            //================>new line
            if (ImGui.Checkbox("KbrdZoom", ref ui.sett.b_use_keybord_for_zoom)) {
                ui.sett.Save();
            }
            ImGuiExt.ToolTip("use the keyboard to zoom the map " +
                "\ninstead of the side mouse buttons (by default)\n" +
                "Buttons edit in the confg");
           
            //ImGui.SameLine();
            //if (ImGui.Checkbox("IngameMep", ref ui.sett.b_use_ingame_map)) {
            //    ui.gui.MakeNeedCheckVisList();
            //    ui.sett.Save();
            //}
            //ImGuiExt.ToolTip("Display enemies on the in-game map(old style)");

            ImGui.SameLine();
            if (ImGui.Checkbox("Log first", ref ui.sett.b_draw_log_first)) {
                ui.sett.Save();
            }
            ImGuiExt.ToolTip("show PA/Log tab as default");

            //================>new line
            if (ui.worker != null) {
                if (ImGui.Checkbox("Pull", ref ui.sett.b_can_pull_alone)) {
                    if (!ui.sett.b_can_pull_alone) {
                        ui.sett.b_get_next_pack = false;
                        ui.sett.b_get_not_visited = false;
                    }
                    ui.sett.Save();
                }
                ImGuiExt.ToolTip("Can body pull/move to closest danger enemy...");

                if (ui.sett.b_can_pull_alone) {
                    ImGui.SameLine();
                    if (ImGui.Checkbox("Next", ref ui.sett.b_get_next_pack)) {
                        ui.sett.Save();
                    }
                    ImGuiExt.ToolTip("Auto get next pack...");

                    ImGui.SameLine();
                    if (ImGui.Checkbox("Vis", ref ui.sett.b_get_not_visited)) {
                        ui.sett.Save();
                    }
                    ImGuiExt.ToolTip("Auto get not visited...");
                }

            }

            //================>new line
            ImGui.PopStyleColor();
            ImGui.PushItemWidth(70);
           
            if (ImGuiExt.NonContinuousEnumComboBox("<-role", ref ui.sett.role)) {
                ui.SetDebugPossible(() => {
                    ui.SetRole(); 
                });
                ui.warning = "last SetDebugPossible time=[" + ui.w8_top + "]"; //for debug only
                ui.sett.Save();
            }

            ImGui.SameLine();
            if (ImGuiExt.EnumComboBox("<-master", ref ui.sett.master_name)) {
                ui.sett.Save();
            }

            ImGui.SameLine();
            if (ImGui.Button("Save Sett")) {
                ui.sett.Save();
                ui.nav.SaveVisited(true);
            }
        }
    }
}
