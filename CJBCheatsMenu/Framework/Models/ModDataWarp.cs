using Microsoft.Xna.Framework;

namespace CJBCheatsMenu.Framework.Models
{
    /// <summary>The model for a configurable warp.</summary>
    internal class ModDataWarp
    {
        /*********
        ** Accessors
        *********/
        /// <summary>The display text to show in the menu. This can be a translation ID, or the raw text to display.</summary>
        public string DisplayText { get; set; }

        /// <summary>The warp's target location name.</summary>
        public string Location { get; set; }

        /// <summary>The warp's target tile coordinate.</summary>
        public Vector2 Tile { get; set; }
    }
}
