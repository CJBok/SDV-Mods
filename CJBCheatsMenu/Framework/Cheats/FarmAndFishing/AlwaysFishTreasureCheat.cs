using System.Collections.Generic;
using CJBCheatsMenu.Framework.Components;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Constants;
using StardewValley.Menus;
using StardewValley.Tools;

namespace CJBCheatsMenu.Framework.Cheats.FarmAndFishing;

/// <summary>A cheat which always catches treasure when fishing.</summary>
internal class AlwaysFishTreasureCheat : BaseCheat
{
    /*********
    ** Public methods
    *********/
    /// <inheritdoc />
    public override IEnumerable<OptionsElement> GetFields(CheatContext context)
    {
        yield return new CheatsOptionsCheckbox(
            label: I18n.Fishing_AlwaysTreasure(),
            value: context.Config.AlwaysTreasure,
            setValue: value => context.Config.AlwaysTreasure = value
        );
    }

    /// <inheritdoc />
    public override void OnConfig(CheatContext context, out bool needsInput, out bool needsUpdate, out bool needsRendering)
    {
        needsInput = false;
        needsUpdate = context.Config.AlwaysTreasure;
        needsRendering = false;
    }

    /// <inheritdoc />
    public override void OnUpdated(CheatContext context, UpdateTickedEventArgs e)
    {
        if (Game1.player?.CurrentTool is FishingRod && Game1.activeClickableMenu is BobberBar { treasure: false } bobberMenu)
        {
            bobberMenu.treasure = true;
            bobberMenu.goldenTreasure = Game1.player.stats.Get(StatKeys.Mastery(Farmer.fishingSkill)) > 0 && Game1.random.NextDouble() < .25 + Game1.player.team.AverageDailyLuck();
        }
    }
}
