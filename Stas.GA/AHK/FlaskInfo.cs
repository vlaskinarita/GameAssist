namespace Stas.GA; 
using System;
using System.Linq;

/// <summary>
///     The structure describing a flask state
/// </summary>
/// <param name="Active">Whether the flask effect is active now</param>
/// <param name="Charges">Current charge amount of a flask</param>
public record FlaskInfo(bool Active, int Charges) : IFlaskInfo
{
    /// <summary>
    ///     Create a new instance from the state and flask data
    /// </summary>
    /// <param name="state">State to build the structure from</param>
    /// <param name="flaskItem">The flask entity</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static FlaskInfo From(InGameState state, Item flaskItem)
    {
        if (flaskItem.Address == IntPtr.Zero)
        {
            return new FlaskInfo(false, 0);
        }

        var active = false;
        if (flaskItem.GetComp<Base>(out var @base))
        {
            if (!AHK.FlaskNameToBuffGroups.TryGetValue(@base.ItemBaseName, out var buffNames))
            {
                throw new Exception($"New (IsValid={flaskItem.IsValid}) flask base found " +
                    $"{@base.ItemBaseName}. Please let the developer know.");
            }

            if (ui.me.GetComp<Buffs>(out var playerBuffs))
            {
                active = buffNames.Any(playerBuffs.StatusEffects.ContainsKey);
            }
        }

        var charges = flaskItem.GetComp<Charges>(out var chargesComponent) ? chargesComponent.Current : 0;
        return new FlaskInfo(active, charges);
    }
}

