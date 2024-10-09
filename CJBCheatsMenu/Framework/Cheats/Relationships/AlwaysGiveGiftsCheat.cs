using System.Collections.Generic;
using CJBCheatsMenu.Framework.Components;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.Cheats.Relationships
{
    /// <summary>A cheat which always allows giving gifts to NPCs.</summary>
    internal class AlwaysGiveGiftsCheat : BaseCheat
    {
        /*********
        ** Public methods
        *********/
        /// <inheritdoc />
        public override IEnumerable<OptionsElement> GetFields(CheatContext context)
        {
            yield return new CheatsOptionsCheckbox(
                label: I18n.Relationships_GiveGiftsAnytime(),
                value: context.Config.AlwaysGiveGift,
                setValue: value => context.Config.AlwaysGiveGift = value
            );
        }

        /// <inheritdoc />
        public override void OnConfig(CheatContext context, out bool needsInput, out bool needsUpdate, out bool needsRendering)
        {
            needsInput = false;
            needsUpdate = context.Config.AlwaysGiveGift;
            needsRendering = false;
        }

        /// <inheritdoc />
        public override void OnUpdated(CheatContext context, UpdateTickedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            foreach (Friendship friendship in Game1.player.friendshipData.Values)
            {
                friendship.GiftsThisWeek = 0;
                friendship.GiftsToday = 0;
            }
        }
    }
}
