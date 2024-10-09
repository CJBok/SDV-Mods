using System.Collections.Generic;
using CJBCheatsMenu.Framework.Components;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Tools;

namespace CJBCheatsMenu.Framework.Cheats.FarmAndFishing
{
    /// <summary>A cheat which always casts the fishing rod at the maximum distance.</summary>
    internal class AlwaysCastMaxDistanceCheat : BaseCheat
    {
        /*********
        ** Public methods
        *********/
        /// <inheritdoc />
        public override IEnumerable<OptionsElement> GetFields(CheatContext context)
        {
            yield return new CheatsOptionsCheckbox(
                label: I18n.Fishing_AlwaysThrowMaxDistance(),
                value: context.Config.ThrowBobberMax,
                setValue: value => context.Config.ThrowBobberMax = value
            );
        }

        /// <inheritdoc />
        public override void OnConfig(CheatContext context, out bool needsInput, out bool needsUpdate, out bool needsRendering)
        {
            needsInput = false;
            needsUpdate = context.Config.ThrowBobberMax;
            needsRendering = false;
        }

        /// <inheritdoc />
        public override void OnUpdated(CheatContext context, UpdateTickedEventArgs e)
        {
            if (Game1.player?.CurrentTool is FishingRod { isTimingCast: true } rod)
                rod.castingPower = 1.01f;
        }
    }
}
