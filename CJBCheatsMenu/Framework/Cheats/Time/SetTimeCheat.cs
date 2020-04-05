using System;
using System.Collections.Generic;
using CJBCheatsMenu.Framework.Components;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.Cheats.Time
{
    /// <summary>A cheat which sets the current time.</summary>
    internal class SetTimeCheat : BaseCheat
    {
        /*********
        ** Public methods
        *********/
        /// <summary>Get the config UI fields to show in the cheats menu.</summary>
        /// <param name="context">The cheat context.</param>
        public override IEnumerable<OptionsElement> GetFields(CheatContext context)
        {
            yield return new CheatsOptionsSlider(
                label: context.Text.Get("time.time"),
                value: Game1.timeOfDay / 100,
                minValue: 6,
                maxValue: 25,
                setValue: value => this.SafelySetTime(value * 100),
                width: 100,
                format: value => Game1.getTimeOfDayString(value * 100)
            );
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Safely transition to the given time, allowing NPCs to update their schedule.</summary>
        /// <param name="time">The time of day.</param>
        private void SafelySetTime(int time)
        {
            // define conversion between game time and TimeSpan
            TimeSpan ToTimeSpan(int value) => new TimeSpan(0, value / 100, value % 100, 0);
            int FromTimeSpan(TimeSpan span) => (span.Hours * 100) + span.Minutes;

            // transition to new time
            int intervals = (int)((ToTimeSpan(time) - ToTimeSpan(Game1.timeOfDay)).TotalMinutes / 10);
            if (intervals > 0)
            {
                for (int i = 0; i < intervals; i++)
                    Game1.performTenMinuteClockUpdate();
            }
            else if (intervals < 0)
            {
                for (int i = 0; i > intervals; i--)
                {
                    Game1.timeOfDay = FromTimeSpan(ToTimeSpan(Game1.timeOfDay).Subtract(TimeSpan.FromMinutes(20))); // offset 20 mins so game updates to next interval
                    Game1.performTenMinuteClockUpdate();
                }
            }
        }
    }
}
