using System.Collections.Generic;
using CJBCheatsMenu.Framework.Components;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Tools;

namespace CJBCheatsMenu.Framework.Cheats.FarmAndFishing;

/// <summary>A cheat which enables instant bites when fishing.</summary>
internal class InstantFishBiteCheat : BaseCheat
{
    /*********
    ** Public methods
    *********/
    /// <inheritdoc />
    public override IEnumerable<OptionsElement> GetFields(CheatContext context)
    {
        yield return new CheatsOptionsCheckbox(
            label: I18n.Fishing_InstantBite(),
            value: context.Config.InstantBite,
            setValue: value => context.Config.InstantBite = value
        );
    }

    /// <inheritdoc />
    public override void OnConfig(CheatContext context, out bool needsInput, out bool needsUpdate, out bool needsRendering)
    {
        needsInput = false;
        needsUpdate = context.Config.InstantBite;
        needsRendering = false;
    }

    /// <inheritdoc />
    public override void OnUpdated(CheatContext context, UpdateTickedEventArgs e)
    {
        if (Game1.player?.CurrentTool is FishingRod { isFishing: true, hit: false, timeUntilFishingBite: > 0 } rod)
            rod.timeUntilFishingBite = 0;
    }
}
