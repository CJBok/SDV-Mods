using StardewModdingAPI.Utilities;
using StardewValley;

namespace CJBCheatsMenu.Framework.Cheats.Time
{
    /// <summary>The base implementation for classes which change the date.</summary>
    internal abstract class BaseDateCheat : BaseCheat
    {
        /*********
        ** Protected methods
        *********/
        /// <summary>Safely transition to the given day.</summary>
        /// <param name="day">The day of month.</param>
        /// <param name="season">The season.</param>
        /// <param name="year">The year.</param>
        protected void SafelySetDate(int day, string season, int year)
        {
            // update raw values
            bool seasonChanged = season != Game1.currentSeason;
            Game1.dayOfMonth = day;
            Game1.currentSeason = season;
            Game1.year = year;

            // update derived state
            Game1.stats.DaysPlayed = (uint)SDate.Now().DaysSinceStart;
            if (Game1.IsMasterGame)
                Game1.netWorldState.Value.UpdateFromGame1();

            // update seasonal textures
            if (seasonChanged)
                Game1.setGraphicsForSeason();
        }
    }
}
