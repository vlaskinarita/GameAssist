using ImGuiNET;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using V2 = System.Numerics.Vector2;
namespace Stas.GA {
    partial class DrawMain {
        void DrawFlasks() {
            var flist = ui.flask_keys.Select(x => x.ToString()).ToArray();
            ImGui.Text("Assign the buttons you like to the flask slots. \nLater, it may be read from the game");
            ImGui.Columns(5, "flask", true);
            ImGui.SetColumnWidth(0, 76);
            ImGui.SetColumnWidth(1, 76);
            ImGui.SetColumnWidth(2, 76);
            ImGui.SetColumnWidth(3, 76);
            ImGui.SetColumnWidth(4, 76);

            ImGui.SetNextItemWidth(40);
            if (ImGuiExt.NonContinuousEnumComboBox("<=1", ref ui.sett.flask_0_key)) {
                ui.sett.Save();
            }

            ImGui.NextColumn();
            ImGui.SetNextItemWidth(40);
            if (ImGuiExt.NonContinuousEnumComboBox("<=2", ref ui.sett.flask_1_key)){
                ui.sett.Save();
            }

            ImGui.NextColumn(); 
            ImGui.SetNextItemWidth(40);
            if (ImGuiExt.NonContinuousEnumComboBox("<=3", ref ui.sett.flask_2_key)){
                ui.sett.Save();
            }

            ImGui.NextColumn();
            ImGui.SetNextItemWidth(40);
            if (ImGuiExt.NonContinuousEnumComboBox("<=4", ref ui.sett.flask_3_key)){
                ui.sett.Save();
            }

            ImGui.NextColumn();
            ImGui.SetNextItemWidth(40);
            if(ImGuiExt.NonContinuousEnumComboBox("<=5", ref ui.sett.flask_4_key)) {
                ui.sett.Save();
            }

            ImGui.Columns(1, "", false);

            ImGui.Separator();
            if (ImGui.Checkbox("2Left ", ref ui.sett.b_use_left_flasks)) {
                ui.sett.Save();
            }
            ImGuiExt.ToolTip("Pressing on this will click the two left flasks" +
          "\nI use them to press [Silver] +[mana] flask " +
          "\nbefore pooling the next group of monsters");
            ImGui.SameLine();
            ImGui.SetNextItemWidth(40);
            if (ImGuiExt.NonContinuousEnumComboBox("=[" + ui.sett.flask_0_key 
                + "]+[" + ui.sett.flask_1_key + "]   ", ref ui.sett.two_left_flask_key)) {
                ui.sett.Save();
            }

            ImGui.SameLine();
            if (ImGui.Checkbox("2Right", ref ui.sett.b_use_right_flasks)) {
                ui.sett.Save();
            }
            ImGuiExt.ToolTip("Pressing on this button will click the two right flasks" +
               "\nI use them to press [Armor] +[Dodge] flasks " +
               "\nwhen I'm inside a group of monsters");
            ImGui.SameLine();
            ImGui.SetNextItemWidth(40);
            if (ImGuiExt.NonContinuousEnumComboBox("=[" + ui.sett.flask_3_key + "]+["
                + ui.sett.flask_4_key + "]", ref ui.sett.two_right_flask_key)) {
                ui.sett.Save();
            }
         
           

            ImGui.Separator();
            if (ImGui.Checkbox("Life  ", ref ui.sett.b_use_life_flask)) {
                ui.sett.Save();
            }
            ImGuiExt.ToolTip("Will also use this flask if you left [%] life");
            var life_index = ui.flask_keys.IndexOf(ui.sett.life_flask_key);
            ImGui.SameLine();
            ImGui.SetNextItemWidth(40);
            if (ImGui.Combo("<=lKey ", ref life_index, flist, flist.Length)) {
                ui.sett.life_flask_key = ui.flask_keys[life_index];
                ui.sett.Save();
            }
            ImGui.SetNextItemWidth(60);
            ImGui.SameLine();
            if (ImGui.SliderInt("<=trigger last %   ", ref ui.sett.trigger_life_left_persent, 5, 80)) {
                ui.sett.Save();
            }
            ImGuiExt.ToolTip("Set the triggering percentage (default==50)");
          


            ImGui.Separator();
            if (ImGui.Checkbox("Mana  ", ref ui.sett.b_use_mana_flask)) {
                ui.sett.Save();
            }
            ImGuiExt.ToolTip("Will also use this flask if you do't have" +
                "\n enough mana to use the mainskill");
            var mana_index = ui.flask_keys.IndexOf(ui.sett.mana_flask_key);
            ImGui.SameLine();
            ImGui.SetNextItemWidth(40);
            if (ImGui.Combo("<=mKey ", ref mana_index, flist, flist.Length)) {
                ui.sett.mana_flask_key = ui.flask_keys[mana_index];
                ui.sett.Save();
            }
            ImGui.SetNextItemWidth(60);
            ImGui.SameLine();
            if (ImGui.SliderInt("<=Cast price   ", ref ui.sett.mana_cast_price, 5, 140)) {
                ui.sett.Save();
            }
            ImGuiExt.ToolTip("Set the remaining amount of mana " +
                "\nbelow which the flask will be used (default==20)");


            ImGui.Separator();
            if (ImGui.Checkbox("Silver", ref ui.sett.b_use_silver_flask)) {
                ui.sett.Save();
            }
            ImGuiExt.ToolTip("Automatically uses this flask if there are charges on it" +
                "\n and the nearest enemies further than 100 grid points");
            int silver_index = ui.flask_keys.IndexOf(ui.sett.silver_flask_key);
            ImGui.SetNextItemWidth(50);
            ImGui.SameLine();
            if (ImGui.Combo("<=sKey ", ref silver_index, flist, flist.Length)) {
                ui.sett.silver_flask_key = ui.flask_keys[silver_index];
                ui.sett.Save();
            }
            ImGui.SameLine();
            ImGui.SetNextItemWidth(60);
            if (ImGui.SliderInt("<=Set grid dist  ", ref ui.sett.silver_gdist, 50, 250)) {
                ui.sett.Save();
            }
            ImGuiExt.ToolTip("sets the triggering distance (80gp =~ 1 screen)");
           
        }
    }
}
