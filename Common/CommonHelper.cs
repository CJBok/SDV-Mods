using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Locations;
using StardewValley.Menus;

namespace CJB.Common
{
    /// <summary>Provides common helpers for the mods.</summary>
    internal static class CommonHelper
    {
        /*********
        ** Public methods
        *********/
        /****
        ** UI
        ****/
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


        /****
        ** Game helpers
        ****/
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

        /// <summary>Get the tile coordinates in a map area.</summary>
        /// <param name="origin">The center tile coordinate.</param>
        /// <param name="radius">The number of tiles in each direction to include, not counting the origin.</param>
        public static IEnumerable<Vector2> GetTileArea(Vector2 origin, int radius)
        {
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                    yield return new Vector2(origin.X + x, origin.Y + y);
            }
        }

        /// <summary>Get whether the community center is complete.</summary>
        /// <remarks>See game logic in <see cref="Town.resetLocalState"/>.</remarks>
        public static bool GetIsCommunityCenterComplete()
        {
            return Game1.MasterPlayer.mailReceived.Contains("ccIsComplete") || Game1.MasterPlayer.hasCompletedCommunityCenter();
        }


        /****
        ** Math helpers
        ****/
        /// <summary>Get a value's fractional position within a range, as a value between 0 (<paramref name="minValue"/>) and 1 (<paramref name="maxValue"/>).</summary>
        /// <param name="value">The value within the range.</param>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        public static float GetRangePosition(int value, int minValue, int maxValue)
        {
            float position = (value - minValue) / (float)(maxValue - minValue);
            return MathHelper.Clamp(position, 0, 1);
        }

        /// <summary>Get the value from a position within a range.</summary>
        /// <param name="position">The position within the range, where 0 is the minimum value and 1 is the maximum value.</param>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        public static int GetValueAtPosition(float position, int minValue, int maxValue)
        {
            float value = position * (maxValue - minValue) + minValue;
            return (int)MathHelper.Clamp(value, minValue, maxValue);
        }
    }
}
