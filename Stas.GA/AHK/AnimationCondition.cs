using ImGuiNET;
using Newtonsoft.Json;

namespace Stas.GA; 



/// <summary>
///     For triggering an action on player animation changes.
/// </summary>
public class AnimationCondition : ICondition
{
    private static readonly OperatorType[] SupportedOperatorTypes = { OperatorType.EQUAL_TO, OperatorType.NOT_EQUAL_TO };
    private static readonly AnimationCondition ConfigurationInstance = new(OperatorType.EQUAL_TO, Animation.Idle, null);

    [JsonProperty] private OperatorType @operator;
    [JsonProperty] private Animation animation;
    [JsonProperty] private IComponent component;

    /// <summary>
    ///     Initializes a new instance of the <see cref="AnimationCondition" /> class.
    /// </summary>
    /// <param name="operator"><see cref="OperatorType" /> to use in this condition.</param>
    /// <param name="animation">player animation to use for this condition.</param>
    /// <param name="component">component associated with this condition.</param>
    public AnimationCondition(OperatorType @operator, Animation animation, IComponent component)
    {
        this.@operator = @operator;
        this.animation = animation;
        this.component = component;
    }

    /// <summary>
    ///     Draws the ImGui widget for adding the condition.
    /// </summary>
    /// <returns>
    ///     <see cref="ICondition" /> if user wants to add the condition, otherwise null.
    /// </returns>
    public static AnimationCondition Add()
    {
        ConfigurationInstance.ToImGui();
        ImGui.SameLine();
        if (ImGui.Button("Add##Animation"))
        {
            return new AnimationCondition(ConfigurationInstance.@operator, ConfigurationInstance.animation, null);
        }

        return null;
    }

    /// <inheritdoc/>
    public void Add(IComponent component)
    {
        this.component = component;
    }

    /// <inheritdoc />
    public void Display(bool expand)
    {
        this.ToImGui(expand);
        this.component?.Display(expand);
    }

    /// <inheritdoc />
    public bool Evaluate()
    {
        var isConditionValid = false;
        if (ui.me.GetComp<Actor>(out var actorComponent))
        {
            isConditionValid = this.@operator switch
            {
                OperatorType.EQUAL_TO => actorComponent.Animation == this.animation,
                OperatorType.NOT_EQUAL_TO => actorComponent.Animation != this.animation,
                _ => throw new Exception($"AnimationCondition doesn't support {this.@operator}.")
            };
        }

        return this.component == null ? isConditionValid : this.component.execute(isConditionValid);
    }

    private void ToImGui(bool expand = true)
    {
        ImGui.Text("Player animation is");
        ImGui.SameLine();
        if (expand)
        {
            ImGuiExt.EnumComboBox("##AnimationOperator", ref this.@operator, SupportedOperatorTypes);
            ImGui.SameLine();
            ImGuiExt.EnumComboBox("##AnimationRHS", ref this.animation);
        }
        else
        {
            if (this.@operator != OperatorType.EQUAL_TO)
            {
                ImGui.Text("not");
                ImGui.SameLine();
            }

            ImGui.TextColored(new System.Numerics.Vector4(255, 255, 0, 255), $"{this.animation}");
        }
    }
}
