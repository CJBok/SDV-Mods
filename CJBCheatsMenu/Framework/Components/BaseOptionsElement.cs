using StardewModdingAPI;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.Components
{
    /// <summary>The base implementation for a cheat option element.</summary>
    internal abstract class BaseOptionsElement : OptionsElement
    {
        /*********
        ** Protected methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="label">The element label.</param>
        protected BaseOptionsElement(string label)
            : base(label) { }

        /// <summary>Construct an instance.</summary>
        /// <param name="label">The display label.</param>
        /// <param name="x">The X pixel position at which to draw the element.</param>
        /// <param name="y">The Y pixel position at which to draw the element.</param>
        /// <param name="width">The pixel width.</param>
        /// <param name="height">The pixel height.</param>
        /// <param name="whichOption">The option ID.</param>
        protected BaseOptionsElement(string label, int x, int y, int width, int height, int whichOption = -1)
            : base(label, x, y, width, height, whichOption) { }

        /// <summary>Get the X offset at which to render the element.</summary>
        protected int GetOffsetX()
        {
            // Android port doesn't consider the element width, so we do so here
            return Constants.TargetPlatform == GamePlatform.Android
                ? this.bounds.Width + 8
                : 0;
        }
    }
}
