using System;
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

        /// <summary>The relative order in which to list it in the warp menu (default 0).</summary>
        public int Order { get; set; }

        /// <summary>The special behavior to apply.</summary>
        public WarpBehavior SpecialBehavior { get; set; }


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        public ModDataWarp() { }

        /// <summary>Construct an instance.</summary>
        /// <param name="displayText">The display text to show in the menu. This can be a translation ID, or the raw text to display.</param>
        /// <param name="location">The warp's target location name.</param>
        /// <param name="tile">The warp's target tile coordinate.</param>
        public ModDataWarp(string displayText, string location, Vector2 tile)
        {
            this.DisplayText = displayText;
            this.Location = location;
            this.Tile = tile;
        }

        /// <summary>Get whether the <see cref="DisplayText"/> has the given value, compared case-insensitively.</summary>
        /// <param name="id">The ID to check,</param>
        public bool HasId(string id)
        {
            return
                this.DisplayText != null
                && this.DisplayText.Trim().Equals(id?.Trim(), StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
