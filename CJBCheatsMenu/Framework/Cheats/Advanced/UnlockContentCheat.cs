using System.Collections.Generic;
using CJBCheatsMenu.Framework.Components;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.Cheats.Advanced
{
    /// <summary>A cheat which unlocks locked game content.</summary>
    internal class UnlockContentCheat : BaseCheat
    {
        /*********
        ** Public methods
        *********/
        /// <summary>Get the config UI fields to show in the cheats menu.</summary>
        /// <param name="context">The cheat context.</param>
        public override IEnumerable<OptionsElement> GetFields(CheatContext context)
        {
            yield return new CheatsOptionsCheckbox(
                label: context.Text.Get("flags.unlocked-content.dyes-and-tailoring"),
                value: this.HasEvent(992559),
                setValue: value => this.SetEvent(992559, value)
            );
        }
    }
}
