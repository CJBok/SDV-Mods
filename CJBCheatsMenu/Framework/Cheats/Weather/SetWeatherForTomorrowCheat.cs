using System.Collections.Generic;
using CJBCheatsMenu.Framework.Components;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.Cheats.Weather
{
    /// <summary>A cheat which sets the weather for tomorrow.</summary>
    internal class SetWeatherForTomorrowCheat : BaseCheat
    {
        /*********
        ** Public methods
        *********/
        /// <summary>Get the config UI fields to show in the cheats menu.</summary>
        /// <param name="context">The cheat context.</param>
        public override IEnumerable<OptionsElement> GetFields(CheatContext context)
        {
            return new OptionsElement[]
            {
                new CheatsOptionsWeatherElement(I18n.Weather_Current(), this.GetWeatherForNextDay),
                this.GetWeatherField(context, I18n.Weather_Sunny(), Game1.weather_sunny),
                this.GetWeatherField(context, I18n.Weather_Raining(), Game1.weather_rain),
                this.GetWeatherField(context, I18n.Weather_Lightning(), Game1.weather_lightning),
                this.GetWeatherField(context, I18n.Weather_Snowing(), Game1.weather_snow)
            };
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Get the option field to set a weather for tomorrow.</summary>
        /// <param name="context">The cheat context.</param>
        /// <param name="label">The option label text.</param>
        /// <param name="id">The game's weather ID.</param>
        private CheatsOptionsButton GetWeatherField(CheatContext context, string label, string id)
        {
            return new CheatsOptionsButton(
                label: label,
                slotWidth: context.SlotWidth,
                toggle: () => this.SetWeatherForNextDay(id)
            );
        }

        /// <summary>Get the display text for the current weather for tomorrow.</summary>
        private string GetWeatherForNextDay()
        {
            switch (Game1.weatherForTomorrow)
            {
                case Game1.weather_sunny:
                case Game1.weather_debris:
                case Game1.weather_festival:
                case Game1.weather_wedding:
                    return I18n.Weather_Sunny();

                case Game1.weather_rain:
                    return I18n.Weather_Raining();

                case Game1.weather_lightning:
                    return I18n.Weather_Lightning();

                case Game1.weather_snow:
                    return I18n.Weather_Snowing();

                default:
                    return "";
            }
        }

        /// <summary>Set the weather for tomorrow.</summary>
        /// <param name="weatherID">The game's weather ID.</param>
        public void SetWeatherForNextDay(string weatherID)
        {
            Game1.weatherForTomorrow = weatherID;
            Game1.playSound("thunder");
        }
    }
}
