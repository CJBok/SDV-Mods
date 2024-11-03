using System.Collections.Generic;
using CJBCheatsMenu.Framework.Components;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.Cheats.FarmAndFishing;

/// <summary>A cheat which makes building construction complete instantly.</summary>
internal class InstantBuildCheat : BaseCheat
{
    /*********
    ** Public methods
    *********/
    /// <inheritdoc />
    public override IEnumerable<OptionsElement> GetFields(CheatContext context)
    {
        yield return new CheatsOptionsCheckbox(
            label: I18n.Farm_InstantBuild(),
            value: context.Config.InstantBuild,
            setValue: value => context.Config.InstantBuild = value
        );
    }

    /// <inheritdoc />
    public override void OnConfig(CheatContext context, out bool needsInput, out bool needsUpdate, out bool needsRendering)
    {
        needsInput = false;
        needsUpdate = context.Config.InstantBuild;
        needsRendering = false;
    }

    /// <inheritdoc />
    public override void OnUpdated(CheatContext context, UpdateTickedEventArgs e)
    {
        if (!e.IsOneSecond || !Context.IsWorldReady)
            return;

        foreach (GameLocation location in context.GetAllLocations())
        {
            foreach (Building building in location.buildings)
            {
                if (building.daysOfConstructionLeft.Value > 0)
                    building.dayUpdate(0);
                if (building.daysUntilUpgrade.Value > 0)
                    building.dayUpdate(0);
            }
        }
    }
}
