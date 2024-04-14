using Microsoft.Xna.Framework;

namespace CJBCheatsMenu.Framework.ContentModels
{
    /// <summary>The data for a warp in the UI.</summary>
    internal class WarpContentModel
    {
        /*********
        ** Accessors
        *********/
        /// <summary>A unique string ID for this warp.</summary>
        public string Id { get; set; } = "";

        /// <summary>The section ID to add the warp to.</summary>
        public string SectionId { get; set; } = "";

        /// <summary>The translated display text to show in the menu.</summary>
        public string DisplayName { get; set; } = "";

        /// <summary>The warp's target location name.</summary>
        public string Location { get; set; } = "";

        /// <summary>The warp's target tile coordinate.</summary>
        public Vector2 Tile { get; set; }

        /// <summary>The relative order in which to list it in the warp menu (default 0).</summary>
        public int Order { get; set; }

        /// <summary>A game state query which indicates whether the warp should be visible, or <c>null</c> if it should always be visible.</summary>
        public string? Condition { get; set; }


        /*********
        ** Public methods
        *********/
        /// <summary>Get whether this model has the same fields as another instance.</summary>
        /// <param name="other">The other warp model to compare with.</param>
        public bool HasSameFieldsAs(WarpContentModel other)
        {
            return
                this.Id == other.Id
                && this.SectionId == other.SectionId
                && this.DisplayName == other.DisplayName
                && this.Location == other.Location
                && this.Tile == other.Tile
                && this.Order == other.Order
                && this.Condition == other.Condition;
        }
    }
}
