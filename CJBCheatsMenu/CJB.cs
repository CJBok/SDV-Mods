using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu
{
    public class CJB
    {
        /*********
        ** Public methods
        *********/
        public static string GetSaveLocation()
        {
            /*string s = Game1.player.name;
            foreach (char c in s) {
                if (!char.IsLetterOrDigit(c)) {
                    s = s.Replace(c.ToString(), "");
                }
            }
            string saveDir = s + "_" + Game1.uniqueIDForThisGame;*/
            FileInfo f = new FileInfo(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StardewValley"), "Saves"));
            return f.FullName;
        }

        public static void drawTextBox(int x, int y, SpriteFont font, string message, int align = 0, float colorIntensity = 1F)
        {
            SpriteBatch b = Game1.spriteBatch;

            Vector2 bounds = font.MeasureString(message);
            int width = (int)bounds.X + Game1.tileSize / 2;
            int height = (int)font.MeasureString(message).Y + Game1.tileSize / 3;
            if (align == 0)
            {
                IClickableMenu.drawTextureBox(b, Game1.menuTexture, new Rectangle(0, 256, 60, 60), x, y, width, height + Game1.tileSize / 16, Color.White * colorIntensity, 1f, true);
                Utility.drawTextWithShadow(b, message, font, new Vector2((float)(x + Game1.tileSize / 4), (float)(y + Game1.tileSize / 4)), Game1.textColor);
            }
            if (align == 1)
            {
                IClickableMenu.drawTextureBox(b, Game1.menuTexture, new Rectangle(0, 256, 60, 60), x - width / 2, y, width, height + Game1.tileSize / 16, Color.White * colorIntensity, 1f, true);
                Utility.drawTextWithShadow(b, message, font, new Vector2((float)(x + Game1.tileSize / 4 - width / 2), (float)(y + Game1.tileSize / 4)), Game1.textColor);
            }
            if (align == 2)
            {
                IClickableMenu.drawTextureBox(b, Game1.menuTexture, new Rectangle(0, 256, 60, 60), x - width, y, width, height + Game1.tileSize / 16, Color.White * colorIntensity, 1f, true);
                Utility.drawTextWithShadow(b, message, font, new Vector2((float)(x + Game1.tileSize / 4 - width), (float)(y + Game1.tileSize / 4)), Game1.textColor);
            }
        }

        public static string getWeatherNexDay()
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

        public static int getExperiencePoints(int level)
        {

            if (level < 0 || level > 9)
                return 0;

            int[] exp = new int[] { 100, 280, 390, 530, 850, 1150, 1500, 2100, 3100, 5000 };

            return exp[level];
        }
    }
}
