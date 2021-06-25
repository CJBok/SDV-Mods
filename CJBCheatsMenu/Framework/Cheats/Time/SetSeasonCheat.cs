using System.Collections.Generic;
using CJBCheatsMenu.Framework.Components;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.Cheats.Time
{
    /// <summary>A cheat which sets the current season.</summary>
    internal class SetSeasonCheat : BaseCheat
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
                format: value => Utility.getSeasonNameFromNumber(value)
            );
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Safely transition to the given season.</summary>
        /// <param name="season">The season.</param>
        private void SafelySetSeason(int season)
        {
            Game1.currentSeason = season switch
            {
                0 => "spring",
                1 => "summer",
                2 => "fall",
                3 => "winter",
                _ => Game1.currentSeason,
            };
            Game1.setGraphicsForSeason();
            Game1.stats.DaysPlayed = (uint)SDate.Now().DaysSinceStart;
            if (Game1.IsMasterGame)
                Game1.netWorldState.Value.UpdateFromGame1();
        }
    }
}
