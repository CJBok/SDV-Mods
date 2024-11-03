using System.Collections.Generic;
using CJBCheatsMenu.Framework.Components;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.Cheats.FarmAndFishing;

/// <summary>A cheat which enables infinite hay.</summary>
internal class InfiniteHayCheat : BaseCheat
{
    /*********
    ** Public methods
    *********/
    /// <inheritdoc />
    public override IEnumerable<OptionsElement> GetFields(CheatContext context)
    {
        yield return new CheatsOptionsCheckbox(
            label: I18n.Farm_InfiniteHay(),
            value: context.Config.InfiniteHay,
            setValue: value => context.Config.InfiniteHay = value
        );
    }

    /// <inheritdoc />
    public override void OnConfig(CheatContext context, out bool needsInput, out bool needsUpdate, out bool needsRendering)
    {
        needsInput = false;
        needsUpdate = context.Config.InfiniteHay;
        needsRendering = false;
    }

    /// <inheritdoc />
    public override void OnUpdated(CheatContext context, UpdateTickedEventArgs e)
    {
        if (!e.IsOneSecond || !Context.IsWorldReady)
            return;

        Utility.ForEachLocation(
            location =>
            {
                if (location.buildings.Count > 0)
                {
                    int addHay = location.GetHayCapacity() - location.piecesOfHay.Value;
                    location.tryToAddHay(addHay);
                }

                return true;
            },
            includeInteriors: false
        );
    }
}
