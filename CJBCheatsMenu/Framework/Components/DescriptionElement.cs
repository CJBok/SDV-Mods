using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.Components
{
    /// <summary>An options element which contains descriptive text.</summary>
    internal class DescriptionElement : BaseOptionsElement
    {
        /*********
        ** Fields
        *********/
        /// <summary>If set, get the label to display each tick.</summary>
        private readonly Func<string>? GetLabel;


        /*********
        ** Accessors
        *********/
        /// <summary>Whether to split the element into multiple lines if needed to fit the page.</summary>
        public bool SplitLinesIfNeeded { get; }


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="label">The checkbox label.</param>
        /// <param name="splitLinesIfNeeded">Whether to split the element into multiple lines if needed to fit the page.</param>
        public DescriptionElement(string label, bool splitLinesIfNeeded = true)
            : base(label, -1, -1, 0, 0, 0)
        {
            this.SplitLinesIfNeeded = splitLinesIfNeeded;
        }

        /// <summary>Construct an instance.</summary>
        /// <param name="label">The checkbox label.</param>
        public DescriptionElement(Func<string> label)
            : base(label(), -1, -1, 0, 0, 0)
        {
            this.GetLabel = label;
            this.SplitLinesIfNeeded = false; // not compatible with dynamic line splitting, since that happens ahead of time
        }

        /// <inheritdoc />
        public override void draw(SpriteBatch spriteBatch, int slotX, int slotY, IClickableMenu? context = null)
        {
            if (this.GetLabel != null)
                this.label = this.GetLabel();

            spriteBatch.DrawString(Game1.smallFont, this.label, new Vector2(slotX + this.bounds.X, slotY + this.bounds.Y), Color.Black);
        }
    }
}
