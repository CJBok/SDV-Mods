#nullable enable

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace CJB.Common.UI
{
    /**


    This code is copied from Pathoschild.Stardew.Common.UI in https://github.com/Pathoschild/StardewMods,
    available under the MIT License. See that repository for the latest version.


    **/
    /// <summary>Simplifies access to the game's sprite sheets.</summary>
    /// <remarks>Each sprite is represented by a rectangle, which specifies the coordinates and dimensions of the image in the sprite sheet.</remarks>
    internal static class CommonSprites
    {
        /// <summary>Sprites used to draw a dropdown list.</summary>
        public static class DropDown
        {
            /// <summary>The sprite sheet containing the menu sprites.</summary>
            public static readonly Texture2D Sheet = Game1.mouseCursors;

            /// <summary>The background for the selected item.</summary>
            public static readonly Rectangle ActiveBackground = new(258, 258, 4, 4);

            /// <summary>The background for a non-selected, non-hovered item.</summary>
            public static readonly Rectangle InactiveBackground = new(269, 258, 4, 4);

            /// <summary>The background for an item under the cursor.</summary>
            public static readonly Rectangle HoverBackground = new(161, 340, 4, 4);
        }

        /// <summary>Sprites used to draw icons.</summary>
        public static class Icons
        {
            /// <summary>The sprite sheet containing the icon sprites.</summary>
            public static Texture2D Sheet => Game1.mouseCursors;

            /// <summary>A down arrow for scrolling content.</summary>
            public static readonly Rectangle DownArrow = new(12, 76, 40, 44);

            /// <summary>An up arrow for scrolling content.</summary>
            public static readonly Rectangle UpArrow = new(76, 72, 40, 44);
        }

        /// <summary>Sprites used to draw a tab.</summary>
        public static class Tab
        {
            /// <summary>The sprite sheet containing the tab sprites.</summary>
            public static readonly Texture2D Sheet = Game1.mouseCursors;

            /// <summary>The top-left corner.</summary>
            public static readonly Rectangle TopLeft = new(0, 384, 5, 5);

            /// <summary>The top-right corner.</summary>
            public static readonly Rectangle TopRight = new(11, 384, 5, 5);

            /// <summary>The bottom-left corner.</summary>
            public static readonly Rectangle BottomLeft = new(0, 395, 5, 5);

            /// <summary>The bottom-right corner.</summary>
            public static readonly Rectangle BottomRight = new(11, 395, 5, 5);

            /// <summary>The top edge.</summary>
            public static readonly Rectangle Top = new(4, 384, 1, 3);

            /// <summary>The left edge.</summary>
            public static readonly Rectangle Left = new(0, 388, 3, 1);

            /// <summary>The right edge.</summary>
            public static readonly Rectangle Right = new(13, 388, 3, 1);

            /// <summary>The bottom edge.</summary>
            public static readonly Rectangle Bottom = new(4, 397, 1, 3);

            /// <summary>The tab background.</summary>
            public static readonly Rectangle Background = new(5, 387, 1, 1);
        }
    }
}
