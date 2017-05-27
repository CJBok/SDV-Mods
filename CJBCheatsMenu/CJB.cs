using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Locations;
using StardewValley.Menus;

namespace CJBCheatsMenu
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

        public static string GetWeatherNexDay()
        {
            switch (Game1.weatherForTomorrow)
            {
                case Game1.weather_sunny:
                    return "Sunny";
                case Game1.weather_rain:
                    return "Rain";
                case Game1.weather_lightning:
                    return "Lightning";
                case Game1.weather_snow:
                    return "Snow";
                case Game1.weather_debris:
                    return "Windy";
                case Game1.weather_festival:
                    return "Festival";
                case Game1.weather_wedding:
                    return "Wedding";
            }
            return "";
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
                        if (building.indoors != null)
                            yield return building.indoors;
                    }
                }
            }
        }
    }
}
