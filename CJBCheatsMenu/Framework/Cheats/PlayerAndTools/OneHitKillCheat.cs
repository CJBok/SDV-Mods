using System.Collections.Generic;
using System.Linq;
using CJBCheatsMenu.Framework.Components;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Monsters;

namespace CJBCheatsMenu.Framework.Cheats.PlayerAndTools
{
    /// <summary>A cheat which enables one-hit kill.</summary>
    internal class OneHitKillCheat : BaseCheat
    {
        /*********
        ** Public methods
        *********/
        /// <summary>Get the config UI fields to show in the cheats menu.</summary>
        /// <param name="context">The cheat context.</param>
        public override IEnumerable<OptionsElement> GetFields(CheatContext context)
        {
            yield return new CheatsOptionsCheckbox(
                label: context.Text.Get("player.one-hit-kill"),
                value: context.Config.OneHitKill,
                setValue: value => context.Config.OneHitKill = value
            );
        }

        /// <summary>Handle the cheat options being loaded or changed.</summary>
        /// <param name="context">The cheat context.</param>
        /// <param name="needsUpdate">Whether the cheat should be notified of game updates.</param>
        /// <param name="needsInput">Whether the cheat should be notified of button presses.</param>
        /// <param name="needsRendering">Whether the cheat should be notified of render ticks.</param>
        public override void OnConfig(CheatContext context, out bool needsInput, out bool needsUpdate, out bool needsRendering)
        {
            needsInput = false;
            needsUpdate = context.Config.OneHitKill;
            needsRendering = false;
        }

        /// <summary>Handle a game update if <see cref="ICheat.OnSaveLoaded"/> indicated updates were needed.</summary>
        /// <param name="context">The cheat context.</param>
        /// <param name="e">The update event arguments.</param>
        public override void OnUpdated(CheatContext context, UpdateTickedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            var monsters = Game1.currentLocation?.characters?.OfType<Monster>();
            if (monsters != null) // some custom locations can have null characters list
            {
                foreach (Monster monster in monsters)
                {
                    if (monster.Health > 1)
                        monster.Health = 1;
                }
            }
        }
    }
}
