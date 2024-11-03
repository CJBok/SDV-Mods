using System.Collections.Generic;
using CJBCheatsMenu.Framework.Components;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Tools;

namespace CJBCheatsMenu.Framework.Cheats.FarmAndFishing;

/// <summary>A cheat which enables instant catches when fishing.</summary>
internal class InstantFishCatchCheat : BaseCheat
{
    /*********
    ** Public methods
    *********/
    /// <inheritdoc />
    public override IEnumerable<OptionsElement> GetFields(CheatContext context)
    {
        yield return new CheatsOptionsCheckbox(
            label: I18n.Fishing_InstantCatch(),
            value: context.Config.InstantCatch,
            setValue: value => context.Config.InstantCatch = value
        );
    }

    /// <inheritdoc />
    public override void OnConfig(CheatContext context, out bool needsInput, out bool needsUpdate, out bool needsRendering)
    {
        needsInput = false;
        needsUpdate = context.Config.InstantCatch;
        needsRendering = false;
    }

    /// <inheritdoc />
    public override void OnUpdated(CheatContext context, UpdateTickedEventArgs e)
    {
        if (Game1.player?.CurrentTool is FishingRod && Game1.activeClickableMenu is BobberBar bobberMenu)
        {
            context.Reflection.GetField<float>(bobberMenu, "distanceFromCatching").SetValue(1);
            if (context.Reflection.GetField<bool>(bobberMenu, "treasure").GetValue())
                context.Reflection.GetField<bool>(bobberMenu, "treasureCaught").SetValue(true);
        }
    }
}
