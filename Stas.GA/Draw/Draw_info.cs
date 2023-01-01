using ImGuiNET;
using System.Diagnostics;
using Color = System.Drawing.Color;
using V2 = System.Numerics.Vector2;
namespace Stas.GA {
    partial class DrawMain {
        string input = "";
        void DrawInfo() {
            bool b_pushed = false;
            if (ui.b_contr_alt) {
                ImGui.PushStyleColor(ImGuiCol.WindowBg, Color.FromArgb(255, 1, 1, 1).ToImgui());
                b_pushed = true;
            }

            if (ui.b_minimize) {
                ImGui.SetNextWindowSize(new V2(50, 30));
            }

            ImGui.Begin("Master INFO", ImGuiWindowFlags.NoCollapse |
                ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.AlwaysAutoResize);
            if (ImGui.Button("_")) {
                ui.b_minimize = !ui.b_minimize;
            }
            ImGuiExt.ToolTip("Minimize/Maximize [F11]");
            ImGui.SameLine();
            if (ImGui.Checkbox("", ref ui.b_ga_menu_top)) {
                ui.sett.Save();
            }
            ImGuiExt.ToolTip("The mouse can interact with this window");

            ImGui.SetWindowFontScale(ui.sett.info_font_size);
            ImGui.SameLine();
            var warn = ui.warning;
            if (!string.IsNullOrEmpty(warn)) {
                ImGui.PushStyleColor(ImGuiCol.Button, Color.DarkMagenta.ToImgui());
                ImGui.Button(warn);
                ImGui.PopStyleColor();
            }
            if (ui.max_pp > 1) {
                //todo need inplement here
                //if (ImGui.Button("pi=" + ui.sett.curr_gp_index.ToString())) {
                //    if (ui.sett.curr_gp_index == 0)
                //        ui.sett.curr_gp_index = 1;
                //    else
                //        ui.sett.curr_gp_index = 0;
                //    ui.sett.Save();
                //    Environment.Exit(1);
                //}
                ImGuiExt.ToolTip("indicate the active POE process by its Id");
                ImGui.SameLine();
            }

            if (ImGui.Button(ui.curr_map_name)) {
                ui.ReloadGameState();
            }
            ImGuiExt.ToolTip("Click for reload the map");

            if (ui.curr_map?.danger > 0) {
                ImGui.PushStyleColor(ImGuiCol.Button, Color.Red.ToImgui());
                ImGui.SameLine();
                ImGui.Button(ui.curr_map.danger.ToRoundStr(2));
                ImGui.PopStyleColor();
            }
            ImGui.SameLine();
            ImGui.Button("cpu=" + ui.cpu.ToRoundStr());
            ImGui.SameLine();
            ImGui.Button("mem=" + ui.mem.ToRoundStr());

          //new libe here
            if (ui.looter != null && ImGui.Button(ui.looter.debug_info)) {
                ui.looter.LoadSett();
            }
            if (ImGui.IsItemHovered()) {
                ImGui.BeginTooltip();
                ImGui.Text("all/close/need... Click for reload filters");
                ImGui.EndTooltip();
            }

            if (ui.curr_map != null) {
                ImGui.SameLine(0);
                ImGui.Button(ui.curr_map.debug_info);
            }
           // DrawTaskerState();
            //ImGui.SameLine();
            //if (ImGui.Button("Quit")) {
            //    ui.sett.Save();
            //    ui.SetDebugPossible(() => {
            //        ui.Dispose();
            //        HardTerminalExit();
            //        Environment.Exit(0);
            //    });
             
            //}
            //ImGuiExt.ToolTip("Close application");

            if (ui.sett.b_draw_map_fps) {
                var res = sw_main.GetRes;
                if(res.Item1 !=null)
                    ImGui.Button(res.Item1);
            }
            if (ui.sett.b_draw_ent_fps) {
                var res = ui.curr_map.sw_ent_upd.GetRes;
                if (res.Item1 != null)
                    ImGui.Button(res.Item1);
            }
            DrawTabs();
            ImGui.SetWindowFontScale(1f);
            ImGui.End();
            if (b_pushed)
                ImGui.PopStyleColor();
          
        }
        // When in Debug mode running on a development computer, this will not run to avoid shutting down the dev computer
        // When in release mode the Remote Connection or other computer this is run on will be shut down.
        [Conditional("RELEASE")]
        private void HardTerminalExit() {
           
        }
        /*
        [ImGuiCol_Text] = The color for the text that will be used for the whole menu.
        [ImGuiCol_TextDisabled] = Color for "not active / disabled text". 
        [ImGuiCol_WindowBg] = Background color.
        [ImGuiCol_PopupBg] = The color used for the background in ImGui::Combo and ImGui::MenuBar.
        [ImGuiCol_Border] = The color that is used to outline your menu.
        [ImGuiCol_BorderShadow] = Color for the stroke shadow.
        [ImGuiCol_FrameBg] = Color for ImGui::InputText and for background ImGui :: Checkbox
        [ImGuiCol_FrameBgHovered] = The color that is used in almost the same way as the one above, except that it changes color when guiding it to ImGui :: Checkbox.
        [ImGuiCol_FrameBgActive] = Active color.
        [ImGuiCol_TitleBg] = The color for changing the main place at the very top of the menu (where the name of your "top-of-the-table" is shown.
        ImGuiCol_TitleBgCollapsed = ImguiCol_TitleBgActive
        = The color of the active title window, ie if you have a menu with several windows , this color will be used for the window in which you will be at the moment.
        [ImGuiCol_MenuBarBg] = The color for the bar menu. (Not all sawes saw this, but still)
        [ImGuiCol_ScrollbarBg] = The color for the background of the "strip", through which you can "flip" functions in the software vertically.
        [ImGuiCol_ScrollbarGrab] = Color for the scoll bar, ie for the "strip", which is used to move the menu vertically.
        [ImGuiCol_ScrollbarGrabHovered] = Color for the "minimized / unused" scroll bar. 
        [ImGuiCol_ScrollbarGrabActive] = The color for the "active" activity in the window where the scroll bar is located.
        [ImGuiCol_ComboBg] = Color for the background for ImGui::Combo.
        [ImGuiCol_CheckMark] = Color for your ImGui :: Checkbox.
        [ImGuiCol_SliderGrab] = Color for the slider ImGui::SliderInt and ImGui::SliderFloat.
        [ImGuiCol_SliderGrabActive] = Color of the slider,
        [ImGuiCol_Button] = the color for the button. 
        [ImGuiCol_ButtonHovered] = Color when hovering over the button.
        [ImGuiCol_ButtonActive] = Button color used. 
        [ImGuiCol_Header] = Color for ImGui::CollapsingHeader.
        [ImGuiCol_HeaderHovered] = Color, when hovering over ImGui :: CollapsingHeader.
        [ImGuiCol_HeaderActive] = Used color ImGui :: CollapsingHeader.
        [ImGuiCol_Column] = Color for the "separation strip" ImGui::Column and ImGui::NextColumn.
        [ImGuiCol_ColumnHovered] = Color, when hovering on the "strip strip" ImGui::Column and ImGui::NextColumn.
        [ImGuiCol_ColumnActive] = The color used for the "separation strip" ImGui::Column and ImGui::NextColumn.
        [ImGuiCol_ResizeGrip] = The color for the "triangle" in the lower right corner, which is used to increase or decrease the size of the menu. 
        [ImGuiCol_ResizeGripHovered] = Color, when hovering to the "triangle" in the lower right corner, which is used to increase or decrease the size of the menu. 
        [ImGuiCol_ResizeGripActive] = The color used for the "triangle" in the lower right corner, which is used to increase or decrease the size of the menu. 
        [ImGuiCol_CloseButton] = The color for the button-closing menu. 
        [ImGuiCol_CloseButtonHovered] = Color, when you hover over the button-close menu. 
        [ImGuiCol_CloseButtonActive] = The color used for the button-closing menu.
        [ImGuiCol_TextSelectedBg] = The color of the selected text, in ImGui::MenuBar.
        [ImGuiCol_ModalWindowDarkening] = The color of the "Blackout Window" of your menu.
        I rarely see these designations, but still decided to put them here.
        [ImGuiCol_Tab] = The color for tabs in the menu. 
        [ImGuiCol_TabActive] = The active color of tabs, ie when you click on the tab you will have this color.
        [ImGuiCol_TabHovered] = The color that will be displayed when hovering on the table. 
        [ImGuiCol_TabSelected] = The color that is used when you are in one of the tabs. 
        [ImGuiCol_TabText] = Text color that only applies to tabs. 
        [ImGuiCol_TabTextActive] = Active text color for tabs.

        */

    }
}
