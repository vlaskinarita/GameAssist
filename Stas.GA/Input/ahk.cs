using System.Diagnostics;
using System.IO;
using V2 = System.Numerics.Vector2;
using V3 = System.Numerics.Vector3;
namespace Stas.GA;

public partial class InputChecker : aMouseChecker {
    void Ahk() {
        if (ui.ahk.ShouldExecuteAutoQuit || Keyboard.b_Try_press_key(ui.ahk.sett.AutoQuitKey)) {
            MiscHelper.KillTCPConnectionForProcess(ui.game_process.Id);
        }
        if (Keyboard.b_Try_press_key(ui.ahk.sett.DumpStatusEffectOnMe)) {
            if (ui.me.GetComp<Buffs>(out var buff)) {
                var data = string.Empty;
                foreach (var statusEffect in buff.StatusEffects) {
                    data += $"{statusEffect.Key} {statusEffect.Value}\n";
                }

                if (!string.IsNullOrEmpty(data)) {
                    File.AppendAllText(Path.Join("player_status_effect.txt"), data);
                }
            }
        }
    }
}
