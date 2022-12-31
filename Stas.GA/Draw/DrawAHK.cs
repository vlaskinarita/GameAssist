using ImGuiNET;
using Color = System.Drawing.Color;
using V2 = System.Numerics.Vector2;
namespace Stas.GA; 

partial class DrawMain {

    public void DrawAHK() {
        if (ui.sett.ahk_DebugMode) {
            ImGui.SetNextWindowSizeConstraints(ui.ahk.size, ui.ahk.size * 2);
            ImGui.Begin("Debug Mode Window", ref ui.sett.ahk_DebugMode);
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
        if (ui.ahk.ShouldExecuteAutoQuit || Keyboard.IsKeyPressed(ui.sett.AutoQuitKey)) {
            MiscHelper.KillTCPConnectionForProcess(ui.game_process.Id);
        }

        if (string.IsNullOrEmpty(ui.sett.CurrentProfile)) {
            ui.ahk.debugMessage = "No Profile Selected.";
            return;
        }

        if (!ui.sett.Profiles.ContainsKey(ui.sett.CurrentProfile)) {
            ui.ahk.debugMessage = $"{ui.sett.CurrentProfile} not found.";
            return;
        }

        foreach (var rule in ui.sett.Profiles[ui.sett.CurrentProfile].Rules) {
            rule.Execute(ui.ahk.DebugLog);
        }
    }
}
