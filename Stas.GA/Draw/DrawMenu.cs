
using ImGuiNET;
using System.Diagnostics;
using Color = System.Drawing.Color;
using V2 = System.Numerics.Vector2;
namespace Stas.GA {
    partial class DrawMain {
        void DrawMenu() {

            if (ImGui.BeginMenuBar()) {
                if (ImGui.Checkbox("", ref ui.b_ga_menu_top)) {
                    ui.sett.Save();
                }
                ImGuiExt.ToolTip("The mouse can interact with this window");
                if (ImGui.Button("Donate (捐)")) {
                    Process.Start(new ProcessStartInfo() {
                        FileName = "https://boosty.to/gameassist",
                        UseShellExecute = true
                    });
                }

               

                ImGui.EndMenuBar();
            }
        }
    }
}
