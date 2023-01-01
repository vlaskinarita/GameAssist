using System.Data;
using System.IO;
using System.Numerics;
using System.Xml;
using ImGuiNET;
using NAudio.Utils;

namespace Stas.GA;

public class AHK {
    public AHKSettings sett;
    public AHK() {
        sett = new AHKSettings().Load<AHKSettings>();
        FlaskNameToBuffGroups = FILE.LoadJson<Dictionary<string, 
            List<string>>> (@"FlaskNameToBuff.json"); 
        StatusEffectGroups = FILE.LoadJson<Dictionary<string, 
            List<string>>>(@"StatusEffectGroup.json");

        try {
            AutoQuitCondition = sett.AutoQuitCondition;
        }
        catch (Exception) {
            ui.AddToLog("AHK AutoQuitCondition load... ", MessType.Error);
            AutoQuitCondition = new(OperatorType.LESS_THAN, VitalType.LIFE_PERCENT, 30);
                ui.sett.Save();
        };
    }
    bool stopShowingAutoQuitWarning = false;
    void EnableAutoQuitWarningUiOnAreaChange() {
        this.stopShowingAutoQuitWarning = false;
    }
    /// <summary>
    ///     Gets or sets the flask buff name by looking at flask base name.
    /// </summary>
    public static Dictionary<string, List<string>> FlaskNameToBuffGroups { get; set; } = new();

    /// <summary>
    ///     Gets or sets the status effects in a group and the group name.
    /// </summary>
    public static Dictionary<string, List<string>> StatusEffectGroups { get; set; } =    new();
    public readonly List<string> keyPressInfo = new();
    public Vector2 size = new(400, 200);
    public string debugMessage = "None";
    public string newProfileName = string.Empty;

    public bool ShouldExecuteAutoQuit =>
        sett.EnableAutoQuit &&
        sett.AutoQuitCondition.Evaluate();

    public void DebugLog(string logText) {
        if (sett.ahk_DebugMode) {
            this.keyPressInfo.Add($"{DateTime.Now.TimeOfDay}: {logText}");
        }
    }

    /// <summary>
    ///     Condition on which user want to auto-quit.
    /// </summary>
    public readonly VitalsCondition AutoQuitCondition;

    public bool ShouldExecutePlugin() {
        if (ui.curr_state != gState.InGameState) {
            this.debugMessage = $"Current game state isn't InGameState, it's {ui.curr_state}.";
            return false;
        }

        if (!ui.b_game_top) {
            this.debugMessage = "Game is minimized.";
            return false;
        }

        if (ui.b_town) {
            this.debugMessage = "Player is in town.";
            return false;
        }

        if (!sett.ShouldRunInHideout && ui.b_home) {
            this.debugMessage = "Player is in hideout.";
            return false;
        }

        if (ui.b_I_died) {
            this.debugMessage = "Player is dead.";
            return false;
        }

        if (ui.me.GetComp<Buffs>(out var buffComp)) {
            if (buffComp.StatusEffects.ContainsKey("grace_period")) {
                this.debugMessage = "Player has Grace Period.";
                return false;
            }
        }
        else {
            this.debugMessage = "Can not find player Buffs component.";
            return false;
        }

        if (!ui.me.GetComp<Actor>(out var _)) {
            this.debugMessage = "Can not find player Actor component.";
            return false;
        }

        this.debugMessage = "None";
        return true;
    }

    /// <summary>
    ///     Creates a default profile that is only valid for flasks on newly created character.
    /// </summary>
    internal void CreateDefaultProfile() {
        Profile profile = new();
        foreach (var rule in Rule.CreateDefaultRules()) {
            profile.Rules.Add(rule);
        }

        sett.Profiles["LeagueStartDefaultProfile"] = profile;
        sett.CurrentProfile = "LeagueStartDefaultProfile";

    }

    internal void AutoQuitWarningUi() {

        if (!this.stopShowingAutoQuitWarning && ui.b_town && this.ShouldExecuteAutoQuit) {
            ImGui.OpenPopup("AutoQuitWarningUi");
        }

        if (ImGui.BeginPopup("AutoQuitWarningUi")) {
            ImGui.Text("Please fix your AutoQuit Condition, it's evaluating to true in town.\n" +
                "You will logout automatically as soon as you leave town.");
            if (ImGui.Button("Ok", new Vector2(400f, 50f))) {
                this.stopShowingAutoQuitWarning = true;
                ImGui.CloseCurrentPopup();
            }

            ImGui.EndPopup();
        }
    }
}