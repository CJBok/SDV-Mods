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
            int intervals = this.GetMinutesBetween(time, Game1.timeOfDay) / 10;
            if (intervals > 0)
            {
                for (int i = 0; i < intervals; i++)
                    Game1.performTenMinuteClockUpdate();
            }
            else if (intervals < 0)
            {
                for (int i = 0; i > intervals; i--)
                {
                    Game1.timeOfDay = this.OffsetTime(Game1.timeOfDay, -20); // offset 20 mins so game updates to next interval
                    Game1.performTenMinuteClockUpdate();
                }
            }
        }

        /// <summary>Get the number of minutes between two times.</summary>
        /// <param name="newTime">The new time.</param>
        /// <param name="oldTime">The old time.</param>
        private int GetMinutesBetween(int newTime, int oldTime)
        {
            return (int)(this.ToTimeSpan(newTime) - this.ToTimeSpan(oldTime)).TotalMinutes;
        }

        /// <summary>Get a new game time with the given offset.</summary>
        /// <param name="time">The game time.</param>
        /// <param name="minutes">The number of minutes to offset.</param>
        private int OffsetTime(int time, int minutes)
        {
            TimeSpan span = this.ToTimeSpan(time);
            span = span.Add(TimeSpan.FromMinutes(minutes));
            return this.ToGameTime(span);
        }

        /// <summary>Convert a time game time like '2300' into a time span.</summary>
        /// <param name="time">The time span to convert.</param>
        private int ToGameTime(TimeSpan time)
        {
            return
                (time.Days * 2400)
                + (time.Hours * 100)
                + time.Minutes;
        }

        /// <summary>Convert a game time like '2300' into a time span.</summary>
        /// <param name="time">The game time to convert.</param>
        private TimeSpan ToTimeSpan(int time)
        {
            return new TimeSpan(
                days: 0,
                hours: time / 100,
                minutes: time % 100,
                seconds: 0
            );
        }
    }
}
