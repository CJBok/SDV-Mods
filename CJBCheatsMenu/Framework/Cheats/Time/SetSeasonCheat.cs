using System.Collections.Generic;
using CJBCheatsMenu.Framework.Components;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.Cheats.Time
{
    /// <summary>A cheat which sets the current season.</summary>
    internal class SetSeasonCheat : BaseDateCheat
    {
        /*********
        ** Public methods
        *********/
        /// <summary>Get the config UI fields to show in the cheats menu.</summary>
        /// <param name="context">The cheat context.</param>
        public override IEnumerable<OptionsElement> GetFields(CheatContext context)
        {
            yield return new CheatsOptionsSlider(
                label: I18n.Date_Season(),
                value: Utility.getSeasonNumber(Game1.currentSeason),
                minValue: 0,
                maxValue: 3,
                setValue: this.SafelySetSeason,
                width: 100,
                format: Utility.getSeasonNameFromNumber
            );
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Safely transition to the given season.</summary>
        /// <param name="seasonNumber">The season number.</param>
        private void SafelySetSeason(int seasonNumber)
        {
            string season = seasonNumber switch
            {
                0 => "spring",
                1 => "summer",
                2 => "fall",
                3 => "winter",
                _ => Game1.currentSeason
            };

            this.SafelySetDate(Game1.dayOfMonth, season, Game1.year);
        }
    }
}
