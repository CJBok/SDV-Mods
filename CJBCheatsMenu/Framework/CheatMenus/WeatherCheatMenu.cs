using System;
using System.Collections.Generic;

namespace CJBCheatsMenu.Framework.CheatMenus
{
    internal class WeatherCheatMenu : CheatMenu
    {
        /// <summary>
        /// Unique id for the cheat menu.
        /// </summary>
        public override string Id => "CBJCheatsMenu_WeatherCheatMenu";

        /// <summary>
        /// The title of the cheat menu (used for tab name).
        /// </summary>
        public override string Title => I18n.Get("tabs.weather");

        /// <summary>
        /// Constructs a cheat menu for controlling tomorrows weather.
        /// </summary>
        /// <param name="config">The user defined preferences.</param>
        /// <param name="cheats">Helper module that has various cheat utilities.</param>
        /// <param name="i18n">Helper module for internationalization.</param>
        public WeatherCheatMenu(ModConfig config, Cheats cheats, StardewModdingAPI.ITranslationHelper i18n)
            : base(config, cheats, i18n)
        {

        }

        /// <summary>
        /// The options rendered within this cheat menu.
        /// </summary>
        public override List<Menu.IOptionGroup> OptionGroups
        {
            get
            {
                List<Menu.IOptionGroup> optionGroups = new List<Menu.IOptionGroup>();

                Menu.IOptionGroup weatherOptionGroup = new TomorrowsWeatherGroup(this.I18n);
                
                Action<int> setTomorrowsWeatherAction = new Action<int>(weatherId =>
                {
                    this.Cheats.SetWeatherForNextDay(weatherId);
                });

                weatherOptionGroup.Options.Add(new Menu.OptionSetButton<int>(I18n.Get("weather.sunny"), StardewValley.Game1.weather_sunny, setTomorrowsWeatherAction));
                weatherOptionGroup.Options.Add(new Menu.OptionSetButton<int>(I18n.Get("weather.raining"), StardewValley.Game1.weather_rain, setTomorrowsWeatherAction));
                weatherOptionGroup.Options.Add(new Menu.OptionSetButton<int>(I18n.Get("weather.lightning"), StardewValley.Game1.weather_lightning, setTomorrowsWeatherAction));
                weatherOptionGroup.Options.Add(new Menu.OptionSetButton<int>(I18n.Get("weather.snowing"), StardewValley.Game1.weather_snow, setTomorrowsWeatherAction));
                optionGroups.Add(weatherOptionGroup);
                return optionGroups;
            }
        }

        /// <summary>
        /// A group with a label that updates based on the weather for tomorrow.
        /// </summary>
        private class TomorrowsWeatherGroup : Menu.OptionGroup
        {
            public TomorrowsWeatherGroup(StardewModdingAPI.ITranslationHelper i18n)
                : base(null)
            {
                this.I18n = i18n;
            }

            private StardewModdingAPI.ITranslationHelper I18n { get; set; }

            public override string Title => $"{this.I18n.Get("weather.title")}: {CJB.GetWeatherNexDay(I18n)}";
        }
    }
}
