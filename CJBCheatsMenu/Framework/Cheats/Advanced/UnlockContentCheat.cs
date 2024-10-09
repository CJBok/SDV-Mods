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
        /// <inheritdoc />
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
                setValue: this.SetPerfection
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
            if (!achieved)
                this.SetFlag(false, "Summit_event"); // forget event was seen when disabling perfection
        }
    }
}
