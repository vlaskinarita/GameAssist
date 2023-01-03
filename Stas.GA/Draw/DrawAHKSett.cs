using ImGuiNET;
using System.Drawing;

namespace Stas.GA;
partial class DrawMain {
    public void DrawAHKSettings() {
        var impTextColor = Color.White.ToImguiVec4();
        var info_one = "Do not trust Settings.txt files for Auto Hokey Trigger \n" +
                        "from sources you have not personally verified. \n" +
                        "They may contain malicious content that can compromise your PC.\n" +
                        "Using profiles with incorrectly configured rules\n" +
                        "may also lead to you being kicked from the server, \n" +
                        "or your account being banned as a result of preforming \n" +
                        "to many actions repeatedly.";
        var info_two = "Again, all profiles/rules created to use a specified flask(s)\n" +
                        "should have at a minimum the FLASK_EFFECT and \n" +
                        "an appropriate number of FLASK_CHARGES\n" +
                        " defined as part of the use condition of a given profile rule. \n" +
                        "Failing to to include these two conditions as part of a rule\n" +
                        "will likely result in Auto Hotkey Trigger spamming the flask(s), \n" +
                        "resulting in a possible kick or ban from the game servers\n" +
                        "because of sending to many actions to the server. \n" +
                        "You have been warrned, use common sense when creating\n" +
                        "profiles/rulse with this tool.";
        if ( ImGui.CollapsingHeader("Setup information - study pls")){
            DrawInfo(info_one);
            ImGui.Separator();
            DrawInfo(info_two);
        }
        void DrawInfo(string text) {
            var sp = ImGui.GetCursorScreenPos();
            var ts = ImGui.CalcTextSize(info_one);
            var lt = sp;
            var rt = sp.Increase(ts.X, 0);
            var rb = sp.Increase(ts.X, ts.Y);
            var lb = sp.Increase(0, ts.Y);
            info_ptr.AddQuadFilled(lt, rt, rb, lb, Color.Black.ToImgui());
            ImGui.TextColored(impTextColor, text);
        }
        ImGui.Checkbox("<=Debug", ref ui.ahk.sett.ahk_DebugMode);
        ImGuiExt.ToolTip("The debug mode may prove to be a helpful tool in troubleshooting Auto HotKey Trigger profile rules that are not preforming as expected. " +
                            "It can also be used to verify if AutoHotKeyTrigger is spamming the profile rule action or not based on the included conditions of a given profile rule. " +
                            "It is highly suggested to create and test all new profiles/rules with the debug mode turned on to insure that all rules are preforming as expected.");
        ImGui.SameLine();
        ImGui.SetNextItemWidth(60);
        if (ImGuiExt.NonContinuousEnumComboBox("<=Dump my (de)buffs", ref ui.ahk.sett.DumpStatusEffectOnMe)){
            ui.ahk.sett.Save();
        }
        ImGuiExt.ToolTip($"This hotkey will dump the current active player's buff(s),\n" +
                        "debuff(s) into a file curr_buff_debufs.txt in Bin\n" +
                        "Use this hotkey if AHK fails to detect for example:\n" +
                        $"bleeds, corrupting blood, poison, freeze, ignites \n" +
                        $"or other de(buffs) currently active on the character.");

        //new line here
        ImGui.SameLine();
        if (ImGui.Checkbox("<=HO", ref ui.ahk.sett.ShouldRunInHideout)) {
            ui.ahk.sett.Save();
        }
        ImGuiExt.ToolTip("Use this if you want to cast auras before entering the map.");

        ImGuiExt.IEnumerableComboBox("<=Curr profile", ui.ahk.sett.Profiles.Keys, ref ui.ahk.sett.CurrentProfile);
        ImGuiExt.ToolTip("Your current profile");

        ImGui.SameLine();
        if (ImGui.Button("GetDef")) {
            ui.ahk.CreateDefaultProfile();
        }
        ImGuiExt.ToolTip("Deletes the old one profile and adds a default profile ");

        //new line here
        if (ImGui.CollapsingHeader("Add New Profile")) {
            ImGui.InputText("Name", ref ui.ahk.newProfileName, 50);
            ImGui.SameLine();
            if (ImGui.Button("Add")) {
                if (!string.IsNullOrEmpty(ui.ahk.newProfileName)) {
                    ui.ahk.sett.Profiles.Add(ui.ahk.newProfileName, new Profile());
                    ui.ahk.newProfileName = string.Empty;
                }
            }
        }

        //separate update to allow settings to draw correctly, does not really hurt performance and only called when the settings window is open
        DynamicCondition.UpdateState();
        if (ImGui.CollapsingHeader("Profiles")) {
            foreach (var (key, profile) in ui.ahk.sett.Profiles) {
                if (ImGui.TreeNode($"{key}")) {
                    ImGui.SameLine();
                    if (ImGui.SmallButton("Delete Profile")) {
                        ui.ahk.sett.Profiles.Remove(key);
                        if (ui.ahk.sett.CurrentProfile == key) {
                            ui.ahk.sett.CurrentProfile = string.Empty;
                        }
                    }
                    profile.DrawSettings();
                    ImGui.TreePop();
                }
            }
        }

        if (ImGui.CollapsingHeader("Auto Quit")) {
            ImGui.PushItemWidth(60);
            ImGui.Checkbox("Enable", ref ui.ahk.sett.EnableAutoQuit);
            ImGui.PopItemWidth();

            ui.ahk.sett.AutoQuitCondition.Display(true);
            ImGui.TextWrapped($"Current AutoQuit Condition Evaluates to {ui.ahk.sett.AutoQuitCondition.Evaluate()}");
            ImGui.Separator();
            ImGui.Text("Hotkey to manually quit game connection: ");
            ImGui.SameLine();
            ImGui.PushItemWidth(60);
            if (ImGuiExt.NonContinuousEnumComboBox("##Manual Quit HotKey", 
                ref ui.ahk.sett.AutoQuitKey)) {
                ui.ahk.sett.Save();
            }
           
            ImGui.PopItemWidth();
        }
    }
}
