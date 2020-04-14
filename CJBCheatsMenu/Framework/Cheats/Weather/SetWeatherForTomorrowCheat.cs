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
                new CheatsOptionsWeatherElement($"{context.Text.Get("weather.current")}", () => this.GetWeatherForNextDay(context)),
                this.GetWeatherField(context, "sunny", Game1.weather_sunny),
                this.GetWeatherField(context, "raining", Game1.weather_rain),
                this.GetWeatherField(context, "lightning", Game1.weather_lightning),
                this.GetWeatherField(context, "snowing", Game1.weather_snow)
            };
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Get the option field to set a weather for tomorrow.</summary>
        /// <param name="context">The cheat context.</param>
        /// <param name="translationKey">The unique portion of its display text translation key.</param>
        /// <param name="id">The game's weather ID.</param>
        private CheatsOptionsButton GetWeatherField(CheatContext context, string translationKey, int id)
        {
            return new CheatsOptionsButton(
                label: $"{context.Text.Get($"weather.{translationKey}")}",
                slotWidth: context.SlotWidth,
                toggle: () => this.SetWeatherForNextDay(id)
            );
        }

        /// <summary>Get the display text for the current weather for tomorrow.</summary>
        /// <param name="context">The cheat context.</param>
        public string GetWeatherForNextDay(CheatContext context)
        {
            var text = context.Text;

            switch (Game1.weatherForTomorrow)
            {
                case Game1.weather_sunny:
                case Game1.weather_debris:
                case Game1.weather_festival:
                case Game1.weather_wedding:
                    return text.Get("weather.sunny");

                case Game1.weather_rain:
                    return text.Get("weather.raining");

                case Game1.weather_lightning:
                    return text.Get("weather.lightning");

                case Game1.weather_snow:
                    return text.Get("weather.snowing");

                default:
                    return "";
            }
        }

        /// <summary>Set the weather for tomorrow.</summary>
        /// <param name="weatherID">The game's weather ID.</param>
        public void SetWeatherForNextDay(int weatherID)
        {
            Game1.weatherForTomorrow = weatherID;
            Game1.soundBank.PlayCue("thunder");
        }
    }
}
