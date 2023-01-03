using ImGuiNET;
using Color = System.Drawing.Color;
using V2 = System.Numerics.Vector2;
namespace Stas.GA; 

partial class DrawMain {

    public void DrawAHK() {
        if (ui.ahk.sett.ahk_DebugMode) {
            ImGui.SetNextWindowSizeConstraints(ui.ahk.size, ui.ahk.size * 2);
            ImGui.Begin("Debug Mode Window", ref ui.ahk.sett.ahk_DebugMode);
            ImGui.TextWrapped($"Current Issue: {ui.ahk.debugMessage}");
            if (ImGui.Button("Clear History")) {
                ui.ahk.keyPressInfo.Clear();
            }

            ImGui.BeginChild("KeyPressesInfo");
            for (var i = 0; i < ui.ahk.keyPressInfo.Count; i++) {
                ImGui.Text($"{i}-{ui.ahk.keyPressInfo[i]}");
            }

            ImGui.SetScrollHereY();
            ImGui.EndChild();
            ImGui.End();
        }

        ui.ahk.AutoQuitWarningUi();       
        if (!ui.ahk.ShouldExecutePlugin()) {
            return;
        }
       
        DynamicCondition.UpdateState();

        if (string.IsNullOrEmpty(ui.ahk.sett.CurrentProfile)) {
            ui.ahk.debugMessage = "No Profile Selected.";
            return;
        }
        
        if (!ui.ahk.sett.Profiles.ContainsKey(ui.ahk.sett.CurrentProfile)) {
            ui.ahk.debugMessage = $"{ui.ahk.sett.CurrentProfile} not found.";
            return;
        }

        foreach (var rule in ui.ahk.sett.Profiles[ui.ahk.sett.CurrentProfile].Rules) {
            rule.Execute(ui.ahk.DebugLog);
        }
    }
}
