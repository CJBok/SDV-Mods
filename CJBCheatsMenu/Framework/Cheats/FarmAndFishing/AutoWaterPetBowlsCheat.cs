using System.Collections.Generic;
using System.Linq;
using CJBCheatsMenu.Framework.Components;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Menus;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;

namespace CJBCheatsMenu.Framework.Cheats.FarmAndFishing;

/// <summary>A cheat which automatically waters pet bowls.</summary>
internal class AutoWaterPetBowlsCheat : BaseCheat
{
    /*********
    ** Public methods
    *********/
    /// <inheritdoc />
    public override IEnumerable<OptionsElement> GetFields(CheatContext context)
    {
        yield return new CheatsOptionsCheckbox(
            label: I18n.Farm_AutoWaterPetBowls(),
            value: context.Config.AutoWaterPetBowls,
            setValue: value => context.Config.AutoWaterPetBowls = value
        );
    }

    /// <inheritdoc />
    public override void OnConfig(CheatContext context, out bool needsInput, out bool needsUpdate, out bool needsRendering)
    {
        needsInput = false;
        needsUpdate = context.Config.AutoWaterPetBowls;
        needsRendering = false;
    }

    /// <inheritdoc />
    public override void OnUpdated(CheatContext context, UpdateTickedEventArgs e)
    {
        if (!e.IsOneSecond || !Context.IsWorldReady)
            return;

        Utility.ForEachBuilding(building =>
        {
            if (building is PetBowl bowl)
                bowl.watered.Value = true;

            return true;
        });
    }
}
