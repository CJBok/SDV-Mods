using System.Collections.Generic;
using CJBCheatsMenu.Framework.Components;
using StardewModdingAPI.Events;
using StardewValley.Menus;
using StardewValley.Tools;

namespace CJBCheatsMenu.Framework.Cheats.PlayerAndTools
{
    /// <summary>A cheat which enables instant weapon cooldowns.</summary>
    internal class InstantCooldownCheat : BaseCheat
    {
        /*********
        ** Public methods
        *********/
        /// <inheritdoc />
        public override IEnumerable<OptionsElement> GetFields(CheatContext context)
        {
            yield return new CheatsOptionsCheckbox(
                label: I18n.Player_InstantCooldowns(),
                value: context.Config.InstantCooldowns,
                setValue: value => context.Config.InstantCooldowns = value
            );
        }

        /// <inheritdoc />
        public override void OnConfig(CheatContext context, out bool needsInput, out bool needsUpdate, out bool needsRendering)
        {
            needsInput = false;
            needsUpdate = context.Config.InstantCooldowns;
            needsRendering = false;
        }

        /// <inheritdoc />
        public override void OnUpdated(CheatContext context, UpdateTickedEventArgs e)
        {
            MeleeWeapon.attackSwordCooldown = 0;
            MeleeWeapon.clubCooldown = 0;
            MeleeWeapon.daggerCooldown = 0;
            MeleeWeapon.defenseCooldown = 0;
        }
    }
}
