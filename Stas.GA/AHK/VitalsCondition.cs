using ImGuiNET;
using Newtonsoft.Json;
using System.Text.Json;


namespace Stas.GA;

public class VitalsCondition : ICondition {

    static readonly OperatorType[] SupportedOperatorTypes = { OperatorType.BIGGER_THAN, OperatorType.LESS_THAN };
    static readonly VitalsCondition ConfigurationInstance = new(OperatorType.BIGGER_THAN, VitalType.MANA, 0);

    [JsonProperty] public OperatorType @operator;
    [JsonProperty] public VitalType vitalType;
    [JsonProperty] public int threshold;
    IComponent component;

    /// <summary>
    ///     Initializes a new instance of the <see cref="VitalsCondition" /> class.
    /// </summary>
    /// <param name="operator"><see cref="OperatorType" /> to use in this condition.</param>
    /// <param name="vital">Player vital type to use in this condition.</param>
    /// <param name="threshold">Vital threshold to use in this condition.</param>
    public VitalsCondition(OperatorType @operator, VitalType vitalType, int threshold) {
        this.@operator = @operator;
        this.vitalType = vitalType;
        this.threshold = threshold;
        this.component = null;
    }

    /// <summary>
    ///     Draws the ImGui widget for adding the condition.
    /// </summary>
    /// <returns>
    ///     <see cref="ICondition" /> if user wants to add the condition, otherwise null.
    /// </returns>
    public static VitalsCondition Add() {
        ConfigurationInstance.ToImGui();
        ImGui.SameLine();
        if (ImGui.Button("Add##Vitals")) {
            return new VitalsCondition(
                ConfigurationInstance.@operator,
                ConfigurationInstance.vitalType,
                ConfigurationInstance.threshold);
        }

        return null;
    }

    /// <inheritdoc />
    public void Display(bool expand) {
        this.ToImGui(expand);
        this.component?.Display(expand);
    }

    /// <inheritdoc/>
    public void Add(IComponent component) {
        this.component = component;
    }

    /// <inheritdoc />
    public bool Evaluate() {
        var isConditionValid = false;
        if (ui.me.GetComp<Life>(out var lifeComponent)) {
            isConditionValid = this.@operator switch {
                OperatorType.BIGGER_THAN => this.GetVitalValue(lifeComponent) > this.threshold,
                OperatorType.LESS_THAN => this.GetVitalValue(lifeComponent) < this.threshold,
                _ => throw new Exception($"VitalCondition doesn't support {this.@operator}.")
            };
        }

        return this.component == null ? isConditionValid : this.component.execute(isConditionValid);
    }

    private void ToImGui(bool expand = true) {
        ImGui.Text("Me");
        ImGui.SameLine();
        if (expand) {
            ImGui.SetNextItemWidth(100);
            ImGuiExt.EnumComboBox("is##VitalSelector", ref this.vitalType);
            ImGui.SameLine();
            ImGui.SetNextItemWidth(100);
            ImGuiExt.EnumComboBox("##VitalOperator", ref this.@operator, SupportedOperatorTypes);
            ImGui.SameLine();
            ImGui.SetNextItemWidth(40);
            ImGui.InputInt("##VitalThreshold", ref this.threshold);
        }
        else {
            ImGui.TextColored(new System.Numerics.Vector4(255, 255, 0, 255), $"{this.vitalType}");
            ImGui.SameLine();
            if (this.@operator == OperatorType.BIGGER_THAN) {
                ImGui.Text("is more than");
            }
            else {
                ImGui.Text("is less than");
            }

            ImGui.SameLine();
            ImGui.TextColored(new System.Numerics.Vector4(255, 255, 0, 255), $"{this.threshold}");
        }
    }

    private int GetVitalValue(Life component) {
        return this.vitalType switch {
            VitalType.MANA => component.Mana.Current,
            VitalType.MANA_PERCENT => component.Mana.CurrentInPercent,
            VitalType.LIFE => component.Health.Current,
            VitalType.LIFE_PERCENT => component.Health.CurrentInPercent,
            VitalType.ENERGYSHIELD => component.EnergyShield.Current,
            VitalType.ENERGYSHIELD_PERCENT => component.EnergyShield.CurrentInPercent,
            _ => throw new Exception($"Invalid Vital Type {this.vitalType}")
        };
    }
}

