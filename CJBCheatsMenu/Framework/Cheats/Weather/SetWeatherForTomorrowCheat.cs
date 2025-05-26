using System.Collections.Generic;
using CJBCheatsMenu.Framework.Components;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Objects;

namespace CJBCheatsMenu.Framework.Cheats.Weather;

/// <summary>A cheat which sets the weather for tomorrow.</summary>
internal class SetWeatherForTomorrowCheat : BaseCheat
{
    /*********
    ** Public methods
    *********/
    /// <inheritdoc />
    public override IEnumerable<OptionsElement> GetFields(CheatContext context)
    {
        return
        [
            new DescriptionElement(() => I18n.Weather_CurrentValue(this.GetWeatherForNextDay())),
            this.GetWeatherField(context, I18n.Weather_Sunny(), Game1.weather_sunny),
            this.GetWeatherField(context, I18n.Weather_Raining(), Game1.weather_rain),
            this.GetWeatherField(context, I18n.Weather_Lightning(), Game1.weather_lightning),
            this.GetWeatherField(context, I18n.Weather_Snowing(), Game1.weather_snow),
            this.GetWeatherField(context, I18n.Weather_GreenRain(), Game1.weather_green_rain)
        ];
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
    /// <remarks>Forecast logic derived from <see cref="TV.getWeatherForecast()"/>.</remarks>
    private string GetWeatherForNextDay()
    {
        WorldDate tomorrow = WorldDate.ForDaysPlayed(WorldDate.Now().TotalDays + 1);
        string forecast = Game1.IsMasterGame
            ? Game1.getWeatherModificationsForDate(tomorrow, Game1.weatherForTomorrow)
            : Game1.getWeatherModificationsForDate(tomorrow, Game1.netWorldState.Value.WeatherForTomorrow);

        switch (forecast)
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

            case Game1.weather_green_rain:
                return I18n.Weather_GreenRain();

            default:
                return forecast;
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
