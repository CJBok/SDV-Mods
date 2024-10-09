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
        /// <inheritdoc />
        public override IEnumerable<OptionsElement> GetFields(CheatContext context)
        {
            yield return new CheatsOptionsCheckbox(
                label: I18n.Player_OneHitKill(),
                value: context.Config.OneHitKill,
                setValue: value => context.Config.OneHitKill = value
            );
        }

        /// <inheritdoc />
        public override void OnConfig(CheatContext context, out bool needsInput, out bool needsUpdate, out bool needsRendering)
        {
            needsInput = false;
            needsUpdate = context.Config.OneHitKill;
            needsRendering = false;
        }

        /// <inheritdoc />
        public override void OnUpdated(CheatContext context, UpdateTickedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            IEnumerable<Monster>? monsters = Game1.currentLocation?.characters?.OfType<Monster>();
            if (monsters != null) // some custom locations can have null characters list
            {
                foreach (Monster monster in monsters)
                {
                    if (monster.Health > 1)
                    {
                        monster.Health = 1;
                        monster.MaxHealth = 1;
                    }
                }
            }
        }
    }
}
