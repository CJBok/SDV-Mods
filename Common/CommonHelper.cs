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
        ** Accessors
        *********/
        /// <summary>The width of the borders drawn by <see cref="DrawTab(int,int,Microsoft.Xna.Framework.Graphics.SpriteFont,string,int,float)"/>.</summary>
        public const int ButtonBorderWidth = 4 * Game1.pixelZoom;


        /*********
        ** Public methods
        *********/
        /****
        ** UI
        ****/
        /// <summary>Draw a sprite to the screen.</summary>
        /// <param name="batch">The sprite batch.</param>
        /// <param name="sheet">The sprite sheet containing the sprite.</param>
        /// <param name="sprite">The sprite coordinates and dimensions in the sprite sheet.</param>
        /// <param name="x">The X-position at which to draw the sprite.</param>
        /// <param name="y">The X-position at which to draw the sprite.</param>
        /// <param name="width">The width to draw.</param>
        /// <param name="height">The height to draw.</param>
        /// <param name="color">The color to tint the sprite.</param>
        public static void Draw(this SpriteBatch batch, Texture2D sheet, Rectangle sprite, int x, int y, int width, int height, Color? color = null)
        {
            batch.Draw(sheet, new Rectangle(x, y, width, height), sprite, color ?? Color.White);
        }

        /// <summary>Draw a button texture fir the given text to the screen.</summary>
        /// <param name="x">The X position at which to draw.</param>
        /// <param name="y">The Y position at which to draw.</param>
        /// <param name="font">The text font.</param>
        /// <param name="text">The button text.</param>
        /// <param name="align">The button's horizontal alignment relative to <paramref name="x"/>. The possible values are 0 (left), 1 (center), or 2 (right).</param>
        /// <param name="alpha">The button opacity, as a value from 0 (transparent) to 1 (opaque).</param>
        public static void DrawTab(int x, int y, SpriteFont font, string text, int align = 0, float alpha = 1)
        {
            SpriteBatch spriteBatch = Game1.spriteBatch;
            Vector2 bounds = font.MeasureString(text);

            CommonHelper.DrawTab(x, y, (int)bounds.X, (int)bounds.Y, out Vector2 drawPos, align, alpha);
            Utility.drawTextWithShadow(spriteBatch, text, font, drawPos, Game1.textColor);
        }

        /// <summary>Draw a button texture to the screen.</summary>
        /// <param name="x">The X position at which to draw.</param>
        /// <param name="y">The Y position at which to draw.</param>
        /// <param name="innerWidth">The width of the button's inner content.</param>
        /// <param name="innerHeight">The height of the button's inner content.</param>
        /// <param name="innerDrawPosition">The position at which the content should be drawn.</param>
        /// <param name="align">The button's horizontal alignment relative to <paramref name="x"/>. The possible values are 0 (left), 1 (center), or 2 (right).</param>
        /// <param name="alpha">The button opacity, as a value from 0 (transparent) to 1 (opaque).</param>
        /// <param name="forIcon">Whether the button will contain an icon instead of text.</param>
        public static void DrawTab(int x, int y, int innerWidth, int innerHeight, out Vector2 innerDrawPosition, int align = 0, float alpha = 1, bool forIcon = false)
        {
            SpriteBatch spriteBatch = Game1.spriteBatch;

            // calculate outer coordinates
            int outerWidth = innerWidth + CommonHelper.ButtonBorderWidth * 2;
            int outerHeight = innerHeight + Game1.tileSize / 3;
            int offsetX = align switch
            {
                1 => -outerWidth / 2,
                2 => -outerWidth,
                _ => 0
            };

            // calculate inner coordinates
            {
                int iconOffsetX = forIcon ? -Game1.pixelZoom : 0;
                int iconOffsetY = forIcon ? 2 * -Game1.pixelZoom : 0;
                innerDrawPosition = new Vector2(x + CommonHelper.ButtonBorderWidth + offsetX + iconOffsetX, y + CommonHelper.ButtonBorderWidth + iconOffsetY);
            }

            // draw texture
            IClickableMenu.drawTextureBox(spriteBatch, Game1.menuTexture, new Rectangle(0, 256, 60, 60), x + offsetX, y, outerWidth, outerHeight + Game1.tileSize / 16, Color.White * alpha);
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
