using System.Collections.Generic;
using CJBCheatsMenu.Framework.Components;
using Microsoft.Xna.Framework;
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
            int ToValue(int time) => Utility.ConvertTimeToMinutes(time) / 10;
            int FromValue(int value) => Utility.ConvertMinutesToTime(value * 10);

            yield return new CheatsOptionsSlider(
                label: I18n.Time_Time(),
                value: ToValue(Game1.timeOfDay),
                minValue: ToValue(600),
                maxValue: ToValue(2550),
                setValue: value => this.SafelySetTime(FromValue(value)),
                width: 100,
                format: value => Game1.getTimeOfDayString(FromValue(value))
            );
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Safely transition to the given time, allowing NPCs to update their schedule.</summary>
        /// <param name="time">The time of day.</param>
        private void SafelySetTime(int time)
        {
            // move time back
            int intervals = Utility.CalculateMinutesBetweenTimes(Game1.timeOfDay, time) / 10;
            if (intervals > 0)
            {
                for (int i = 0; i < intervals; i++)
                    Game1.performTenMinuteClockUpdate();
            }
            else if (intervals < 0)
            {
                for (int i = 0; i > intervals; i--)
                {
                    Game1.timeOfDay = Utility.ModifyTime(Game1.timeOfDay, -20); // offset 20 minutes so game updates to next interval
                    Game1.performTenMinuteClockUpdate();
                }
            }

            // reset ambient light
            // White is the default non-raining color. If it's raining or dark out, UpdateGameClock
            // below will update it automatically.
            Game1.outdoorLight = Color.White;
            Game1.ambientLight = Color.White;

            // run clock update (to correct lighting, etc)
            Game1.gameTimeInterval = 0;
            Game1.UpdateGameClock(Game1.currentGameTime);
        }
    }
}
