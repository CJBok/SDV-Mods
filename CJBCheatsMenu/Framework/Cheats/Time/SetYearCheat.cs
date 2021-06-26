using System;
using System.Collections.Generic;
using CJBCheatsMenu.Framework.Components;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.Cheats.Time
{
    /// <summary>A cheat which sets the current year.</summary>
    internal class SetYearCheat : BaseDateCheat
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
                maxValue: Math.Max(30, Game1.year + 15),
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
            this.SafelySetDate(Game1.dayOfMonth, Game1.currentSeason, year);
        }
    }
}
