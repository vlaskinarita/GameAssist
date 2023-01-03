using ImGuiNET;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Numerics;


namespace Stas.GA;

/// <summary>
///     Abstraction for the rule condition list
/// </summary>
public class Rule {
    string tName => GetType().Name;
    private static bool expand = false;
    private ConditionType newConditionType = ConditionType.AILMENT;
    private readonly Stopwatch cooldownStopwatch = Stopwatch.StartNew();

    [JsonProperty("Conditions", NullValueHandling = NullValueHandling.Ignore)]
    private readonly List<ICondition> conditions = new();

    [JsonProperty] 
    float delayBetweenRuns = 0;

    /// <summary>
    ///     Enable/Disable the rule.
    /// </summary>
    public bool Enabled;

    /// <summary>
    ///     User friendly name given to a rule.
    /// </summary>
    public string Name;

    /// <summary>
    ///     Rule key to press on success.
    /// </summary>
    public Keys Key;

    /// <summary>
    ///     Initializes a new instance of the <see cref="Rule" /> class.
    /// </summary>
    /// <param name="name"></param>
    public Rule(string name) {
        this.Name = name;
    }

    /// <summary>
    ///     Creates default rules that are only valid for flasks on the newly created character.
    /// </summary>
    /// <returns>List of rules that are valid for newly created player.</returns>
    public static Rule[] CreateDefaultRules() {
        var rules = new Rule[3];
        rules[0] = new($"QuickSilver");
        rules[0].Enabled = false;
        rules[0].Key = Keys.D4;
        rules[0].conditions.Add(new AnimationCondition(OperatorType.EQUAL_TO, Animation.Run, new Wait(1)));
        rules[0].conditions.Add(new FlaskChargesCondition(OperatorType.BIGGER_THAN, 1, 29));
        rules[0].conditions.Add(new FlaskEffectCondition(1));

        rules[1] = new("Mana");
        rules[1].Enabled = true;
        rules[1].Key = Keys.D3;
        rules[1].conditions.Add(new VitalsCondition(OperatorType.LESS_THAN, VitalType.MANA_PERCENT, 20));
        rules[1].conditions.Add(new FlaskChargesCondition(OperatorType.BIGGER_THAN, 2, 5));
        rules[1].conditions.Add(new FlaskEffectCondition(2));

        rules[2] = new($"Life");
        rules[2].Enabled = true;
        rules[2].Key = Keys.D3;
        rules[2].conditions.Add(new VitalsCondition(OperatorType.LESS_THAN, VitalType.LIFE_PERCENT, 70));
        rules[2].conditions.Add(new FlaskChargesCondition(OperatorType.BIGGER_THAN, 3, 6));
        rules[2].conditions.Add(new FlaskEffectCondition(3));

        //rules[3] = new("Clarity");
        //rules[3].Enabled = false;
        //rules[3].Key = Keys.F6;
        //rules[3].conditions.Add(new VitalsCondition(OperatorType.LESS_THAN, VitalType.MANA_PERCENT, 20));
        //rules[3].conditions.Add(new FlaskChargesCondition(OperatorType.BIGGER_THAN, 4, 5));
        //rules[3].conditions.Add(new FlaskEffectCondition(6));

        return rules;
    }

    /// <summary>
    ///     Clears the list of conditions
    /// </summary>
    public void Clear() {
        this.conditions.Clear();
    }

    /// <summary>
    ///     Displays the rule settings
    /// </summary>
    public void DrawSettings() {
        ImGui.Checkbox("<-Enable", ref this.Enabled);
        ImGui.SameLine();
        ImGui.SetNextItemWidth(130);
        ImGui.InputText("<-Name", ref this.Name, 20);

        ImGui.SameLine();
        ImGui.SetNextItemWidth(60);
        var tmpKey = this.Key;
        if (ImGuiExt.NonContinuousEnumComboBox("<-Key", ref tmpKey)) {
            this.Key = tmpKey;
            ui.ahk.sett.Save();
        }

        ImGui.SetNextItemWidth(60);
        ImGui.DragFloat("<=Cooldown", ref delayBetweenRuns, 0.1f, 0.0f, 30.0f);
        ImGuiExt.ToolTip("Cooldown time  - in seconds");
        ImGui.SameLine();
        var cd_left = delayBetweenRuns <= 0f ? 1f :
            MathF.Min((float)cooldownStopwatch.Elapsed.TotalSeconds, delayBetweenRuns) / delayBetweenRuns;
        ImGui.PushStyleColor(ImGuiCol.PlotHistogram, cd_left < 1f ?
            ImGuiExt.Color(255, 0, 0, 255) : ImGuiExt.Color(0, 255, 0, 255));
        ImGui.ProgressBar((float)cd_left, new Vector2(100, 0), cd_left < 1f ? "Cooling" : "Ready"); 
        ImGui.PopStyleColor();

        ImGui.SameLine();
        if (ImGui.Button("+New cond")) {
            ImGui.OpenPopup("AddNewConditionPopUp");
        }
        ImGuiExt.ToolTip("add New Condition to current set");

        
        if (ImGui.BeginPopup("AddNewConditionPopUp")) {
            ImGui.Text("NOTE: Click outside popup to close");
            ImGuiExt.EnumComboBox("Condition", ref this.newConditionType);
            ImGui.Separator();
            this.Add(this.newConditionType);
            ImGui.EndPopup();
        }
        this.DrawExistingConditions();
    }

    /// <summary>
    ///     Checks the rule conditions and presses its key if conditions are satisfied
    /// </summary>
    /// <param name="logger"></param>
    public void Execute(Action<string> logger) {
        if (this.Enabled && this.Evaluate()) {
            Keyboard.KeyUp(this.Key, tName + ".Execute");
            this.cooldownStopwatch.Restart();
        }
    }

    /// <summary>
    ///     Adds a new condition
    /// </summary>
    /// <param name="conditionType"></param>
    private void Add(ConditionType conditionType) {
        var condition = EnumToObject(conditionType);
        if (condition != null) {
            this.conditions.Add(condition);
        }
    }

    /// <summary>
    ///     Removes a condition at a specific index.
    /// </summary>
    /// <param name="index">index of the condition to remove.</param>
    private void RemoveAt(int index) {
        this.conditions.RemoveAt(index);
    }

    /// <summary>
    ///     Swap two conditions.
    /// </summary>
    /// <param name="i">index of the condition to swap.</param>
    /// <param name="j">index of the condition to swap.</param>
    private void Swap(int i, int j) {
        (this.conditions[i], this.conditions[j]) = (this.conditions[j], this.conditions[i]);
    }

    /// <summary>
    ///     Checks the specified conditions, shortcircuiting on the first unsatisfied one
    /// </summary>
    /// <returns>true if all the rules conditions are true otherwise false.</returns>
    private bool Evaluate() {
        if (this.cooldownStopwatch.Elapsed.TotalSeconds > this.delayBetweenRuns) {
            if (this.conditions.TrueForAll(x => x.Evaluate())) {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    ///     Converts the <see cref="ConditionType" /> to the appropriate <see cref="ICondition" /> object.
    /// </summary>
    /// <param name="conditionType">Condition type to create.</param>
    /// <returns>
    ///     Returns <see cref="ICondition" /> if user wants to create it or null.
    ///     Throws an exception in case it doesn't know how to create a specific Condition.
    /// </returns>
    private static ICondition EnumToObject(ConditionType conditionType) {
        ICondition p = conditionType switch {
            ConditionType.VITALS => VitalsCondition.Add(),
            ConditionType.ANIMATION => AnimationCondition.Add(),
            ConditionType.STATUS_EFFECT => StatusEffectCondition.Add(),
            ConditionType.FLASK_EFFECT => FlaskEffectCondition.Add(),
            ConditionType.FLASK_CHARGES => FlaskChargesCondition.Add(),
            ConditionType.AILMENT => AilmentCondition.Add()
        };

        return p;
    }

   

    private void DrawExistingConditions() {
        if (ImGui.TreeNodeEx("Existing Conditions (all of them have to be true)", ImGuiTreeNodeFlags.DefaultOpen)) {
            ImGui.PushItemWidth(ImGui.GetContentRegionAvail().X / 6);
            for (var i = 0; i < this.conditions.Count; i++) {
                ImGui.PushID($"ConditionNo{i}");
                if (i != 0) {
                    ImGui.Separator();
                }

                ImGui.PushStyleColor(ImGuiCol.Button, 0);
                if (ImGui.ArrowButton("###ExpandHideButton", (expand) ? ImGuiDir.Down : ImGuiDir.Right)) {
                    expand = !expand;
                }

                ImGui.PopStyleColor();
                ImGui.SameLine();
                if (expand && ImGui.Button("Delete")) {
                    this.RemoveAt(i);
                    ImGui.PopID();
                    break;
                }

                ImGui.SameLine();
                if (expand && ImGui.Button("Add")) {
                    this.conditions[i].Add(new Wait(0));
                }

                ImGui.SameLine();
                ImGui.BeginDisabled(i == 0);
                if (expand && ImGui.ArrowButton("up", ImGuiDir.Up)) {
                    this.Swap(i, i - 1);
                    ImGui.PopID();
                    break;
                }

                ImGui.EndDisabled();
                ImGui.SameLine();
                ImGui.BeginDisabled(i == this.conditions.Count - 1);
                if (expand && ImGui.ArrowButton("down", ImGuiDir.Down)) {
                    this.Swap(i, i + 1);
                    ImGui.PopID();
                    break;
                }

                ImGui.EndDisabled();
                ImGui.SameLine();
                ImGui.BeginGroup();
                this.conditions[i].Display(expand);
                ImGui.EndGroup();
                if (!expand || this.conditions[i] is not DynamicCondition) {
                    ImGui.SameLine();
                    var evaluationResult = this.conditions[i].Evaluate();
                    ImGui.TextColored(
                        evaluationResult ? new Vector4(0, 1, 0, 1) : new Vector4(1, 0, 0, 1),
                        evaluationResult ? "(true)" : "(false)");
                }

                ImGui.PopID();
            }

            ImGui.PopItemWidth();
            ImGui.TreePop();
        }
    }

}
