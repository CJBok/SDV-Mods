using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Locations;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework
{
    internal static class CJB
    {
        /*********
        ** Public methods
        *********/
        public static void DrawTextBox(int x, int y, SpriteFont font, string message, int align = 0, float colorIntensity = 1F)
        {
            SpriteBatch spriteBatch = Game1.spriteBatch;

            Vector2 bounds = font.MeasureString(message);
            int width = (int)bounds.X + Game1.tileSize / 2;
            int height = (int)font.MeasureString(message).Y + Game1.tileSize / 3;
            switch (align)
            {
                case 0:
                    IClickableMenu.drawTextureBox(spriteBatch, Game1.menuTexture, new Rectangle(0, 256, 60, 60), x, y, width, height + Game1.tileSize / 16, Color.White * colorIntensity);
                    Utility.drawTextWithShadow(spriteBatch, message, font, new Vector2(x + Game1.tileSize / 4, y + Game1.tileSize / 4), Game1.textColor);
                    break;
                case 1:
                    IClickableMenu.drawTextureBox(spriteBatch, Game1.menuTexture, new Rectangle(0, 256, 60, 60), x - width / 2, y, width, height + Game1.tileSize / 16, Color.White * colorIntensity);
                    Utility.drawTextWithShadow(spriteBatch, message, font, new Vector2(x + Game1.tileSize / 4 - width / 2, y + Game1.tileSize / 4), Game1.textColor);
                    break;
                case 2:
                    IClickableMenu.drawTextureBox(spriteBatch, Game1.menuTexture, new Rectangle(0, 256, 60, 60), x - width, y, width, height + Game1.tileSize / 16, Color.White * colorIntensity);
                    Utility.drawTextWithShadow(spriteBatch, message, font, new Vector2(x + Game1.tileSize / 4 - width, y + Game1.tileSize / 4), Game1.textColor);
                    break;
            }
        }

        public static string GetWeatherNexDay(ITranslationHelper i18n)
        {
            switch (Game1.weatherForTomorrow)
            {
                case Game1.weather_sunny:
                case Game1.weather_debris:
                case Game1.weather_festival:
                case Game1.weather_wedding:
                    return i18n.Get("weather.sunny");
                case Game1.weather_rain:
                    return i18n.Get("weather.raining");
                case Game1.weather_lightning:
                    return i18n.Get("weather.lightning");
                case Game1.weather_snow:
                    return i18n.Get("weather.snowing");
                default:
                    return "";
            }
        }

        public static int GetExperiencePoints(int level)
        {

            if (level < 0 || level > 9)
                return 0;

            int[] exp = { 100, 280, 390, 530, 850, 1150, 1500, 2100, 3100, 5000 };

            return exp[level];
        }

        /// <summary>Get all game locations.</summary>
        public static IEnumerable<GameLocation> GetAllLocations()
        {
            foreach (GameLocation location in Game1.locations)
            {
                // current location
                yield return location;

                // buildings
                if (location is BuildableGameLocation buildableLocation)
                {
                    foreach (Building building in buildableLocation.buildings)
                    {
                        GameLocation indoors = building.indoors.Value;
                        if (indoors != null)
                            yield return indoors;
                    }
                }
            }
        }
    }
}
