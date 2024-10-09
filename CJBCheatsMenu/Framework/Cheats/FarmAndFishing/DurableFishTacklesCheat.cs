using System.Collections.Generic;
using CJBCheatsMenu.Framework.Components;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Tools;

namespace CJBCheatsMenu.Framework.Cheats.FarmAndFishing
{
    /// <summary>A cheat which gives fishing tackles infinite endurance.</summary>
    internal class DurableFishTacklesCheat : BaseCheat
    {
        /*********
        ** Public methods
        *********/
        /// <inheritdoc />
        public override IEnumerable<OptionsElement> GetFields(CheatContext context)
        {
            yield return new CheatsOptionsCheckbox(
                label: I18n.Fishing_DurableTackles(),
                value: context.Config.DurableTackles,
                setValue: value => context.Config.DurableTackles = value
            );
        }

        /// <inheritdoc />
        public override void OnConfig(CheatContext context, out bool needsInput, out bool needsUpdate, out bool needsRendering)
        {
            needsInput = false;
            needsUpdate = context.Config.DurableTackles;
            needsRendering = false;
        }

        /// <inheritdoc />
        public override void OnUpdated(CheatContext context, UpdateTickedEventArgs e)
        {
            if (Game1.player?.CurrentTool is FishingRod rod)
            {
                for (int i = FishingRod.TackleIndex; i < rod.attachments.Count; i++)
                {
                    var tackle = rod.attachments[i];
                    if (tackle != null)
                        tackle.uses.Value = 0;
                }
            }
        }
    }
}
