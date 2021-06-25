using System.Collections.Generic;
using CJBCheatsMenu.Framework.Components;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.Cheats.Time
{
    /// <summary>A cheat which sets the current day.</summary>
    internal class SetDayCheat : BaseCheat
    {
        /*********
        ** Public methods
        *********/
        /// <summary>Get the config UI fields to show in the cheats menu.</summary>
        /// <param name="context">The cheat context.</param>
        public override IEnumerable<OptionsElement> GetFields(CheatContext context)
        {
            yield return new CheatsOptionsSlider(
                label: I18n.Date_Day(),
                value: Game1.dayOfMonth,
                minValue: 1,
                maxValue: 28,
                setValue: this.SafelySetDay,
                width: 100
            );
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Safely transition to the given day.</summary>
        /// <param name="day">The day.</param>
        private void SafelySetDay(int day)
        {
            Game1.dayOfMonth = day;
            Game1.stats.DaysPlayed = (uint)SDate.Now().DaysSinceStart;
            if (Game1.IsMasterGame)
                Game1.netWorldState.Value.UpdateFromGame1();
        }
    }
}
