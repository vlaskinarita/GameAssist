using ImGuiNET;
using System.Drawing;

namespace Stas.GA;
partial class DrawMain {
    public void DrawAHKSettings() {

        var impTextColor = Color.Gray.ToImguiVec4();
        //ImGui.TextColored(impTextColor, "Do not trust Settings.txt files for Auto Hokey Trigger from sources you have not personally verified. " +
        //                  "They may contain malicious content that can compromise your computer. " +
        //                  "Using profiles with incorrectly configured rules may also lead to you being kicked from the server, " +
        //                  "or your account being banned as a result of preforming to many actions repeatedly.");
        //ImGui.NewLine();
        //ImGui.TextColored(impTextColor, "Again, all profiles/rules created to use a specified flask(s) should have at a minimum " +
        //                  "the FLASK_EFFECT and an appropriate number of FLASK_CHARGES defined as part of the use condition of a given profile rule. " +
        //                  "Failing to to include these two conditions as part of a rule will likely result in Auto Hotkey Trigger spamming the flask(s), " +
        //                  "resulting in a possible kick or ban from the game servers because of sending to many actions to the server. " +
        //                  "You have been warrned, use common sense when creating profiles/rulse with this tool.");
        ImGui.NewLine();
        //ImGui.PopTextWrapPos();
        ImGui.Checkbox("Debug Mode", ref ui.ahk.sett.ahk_DebugMode);
        ImGuiExt.ToolTip("The debug mode may prove to be a helpful tool in troubleshooting Auto HotKey Trigger profile rules that are not preforming as expected. " +
                            "It can also be used to verify if AutoHotKeyTrigger is spamming the profile rule action or not based on the included conditions of a given profile rule. " +
                            "It is highly suggested to create and test all new profiles/rules with the debug mode turned on to insure that all rules are preforming as expected.");
        ImGui.NewLine();
        ImGuiExt.NonContinuousEnumComboBox("Dump Player Status Effects",
            ref ui.ahk.sett.DumpStatusEffectOnMe);
        ImGuiExt.ToolTip($"This hotkey will dump the current active player's buff(s), debuff(s) into a text file in the GameHelper -> Plugins -> " +
                            $"AutoHotKeyTrigger folder. Use this hotkey if the AutoHotKeyTrigger plugin fails to detect for example: " +
                            $"bleeds, corrupting blood, poison, freeze, ignites or other de(buffs) currently active on the character.");

        ImGui.NewLine();
        ImGui.Checkbox("Should Run In Hideout", ref ui.ahk.sett.ShouldRunInHideout);
        ImGuiExt.IEnumerableComboBox("Profile", ui.ahk.sett.Profiles.Keys, ref ui.ahk.sett.CurrentProfile);
        ImGui.NewLine();
        if (ImGui.Button("Add/Reset and Activate League Start Default Profile")) {
            ui.ahk.CreateDefaultProfile();
        }

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
            ImGui.PushItemWidth(ImGui.GetContentRegionAvail().X / 6);
            ImGui.Checkbox("Enable AutoQuit", ref ui.ahk.sett.EnableAutoQuit);
            ui.ahk.sett.AutoQuitCondition.Display(true);
            ImGui.TextWrapped($"Current AutoQuit Condition Evaluates to {ui.ahk.sett.AutoQuitCondition.Evaluate()}");
            ImGui.Separator();
            ImGui.Text("Hotkey to manually quit game connection: ");
            ImGui.SameLine();
            ImGuiExt.NonContinuousEnumComboBox("##Manual Quit HotKey", ref ui.ahk.sett.AutoQuitKey);
            ImGui.PopItemWidth();
        }
    }
}
