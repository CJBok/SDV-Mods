using System.Collections.Generic;
using CJBCheatsMenu.Framework.Components;
using StardewValley;
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
                label: I18n.Flags_UnlockedContent_DyesAndTailoring(),
                value: this.HasEvent("992559"),
                setValue: value => this.SetEvent("992559", value)
            );
            yield return new CheatsOptionsCheckbox(
                label: I18n.Flags_UnlockedContent_JunimoText(),
                value: this.HasFlag("canReadJunimoText"),
                setValue: value => this.SetFlag(value, "canReadJunimoText")
            );
            yield return new CheatsOptionsCheckbox(
                label: I18n.Flags_UnlockedContent_Perfection(),
                value: Game1.player.team.farmPerfect.Value,
                setValue: value => this.SetPerfection(value)
            );
        }

        /*********
        ** Private methods
        *********/
        /// <summary>Sets the player's perfection status.</summary>
        /// <param name="achieved">Whether to enable or disable perfection.</param>
        private void SetPerfection(bool achieved)
        {
            Game1.player.team.farmPerfect.Value = achieved;
            this.SetFlag(achieved, "Farm_Eternal");
            this.SetFlag(achieved, "SummitBoulder");
            this.SetFlag(false, "Summit_event");
        }
    }
}
