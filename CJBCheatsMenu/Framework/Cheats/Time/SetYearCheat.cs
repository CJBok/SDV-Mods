using System.Collections.Generic;
using CJBCheatsMenu.Framework.Components;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.Cheats.Time
{
    /// <summary>A cheat which sets the current year.</summary>
    internal class SetYearCheat : BaseCheat
    {
        /*********
        ** Public methods
        *********/
        /// <summary>Get the config UI fields to show in the cheats menu.</summary>
        /// <param name="context">The cheat context.</param>
        public override IEnumerable<OptionsElement> GetFields(CheatContext context)
        {
            yield return new CheatsOptionsSlider(
                label: I18n.Date_Year(),
                value: Game1.year,
                minValue: 1,
                maxValue: 30,
                setValue: this.SafelySetYear,
                width: 100
            );
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Safely transition to the given year.</summary>
        /// <param name="year">The year.</param>
        private void SafelySetYear(int year)
        {
            Game1.year = year;
            Game1.stats.DaysPlayed = (uint)SDate.Now().DaysSinceStart;
            if (Game1.IsMasterGame)
                Game1.netWorldState.Value.UpdateFromGame1();
        }
    }
}
