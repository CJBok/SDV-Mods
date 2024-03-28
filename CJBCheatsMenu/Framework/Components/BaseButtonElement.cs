using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.Components
{
    /// <summary>A parameterless version of BaseButtonElement&lt;TAction&gt; for use with classes that don't need a value</summary>
    internal abstract class BaseButtonElement : BaseButtonElement<bool>
    {
        /// <summary>Construct an instance.</summary>
        /// <param name="label">The field label.</param>
        /// <param name="slotWidth">The field width.</param>
        /// <param name="toggle">The action to perform when the button is toggled.</param>
        /// <param name="disabled">Whether the button should be disabled.</param>
        protected BaseButtonElement(string label, int slotWidth, Action toggle, bool disabled = false)
            : base(label, slotWidth, _ => toggle(), disabled)
        {
        }

        /// <summary>Get the current value of the component.</summary>
        protected override bool GetValue()
        {
            return true;
        }
    }

    /// <summary>A button which invokes a callback when clicked.</summary>
    internal abstract class BaseButtonElement<TAction> : BaseOptionsElement
    {
        /*********
        ** Fields
        *********/
        /// <summary>The action to perform when the button is toggled (or <c>null</c> to handle it manually).</summary>
        private readonly Action<TAction> Toggle;

        /// <summary>The source rectangle for the 'set' button sprite.</summary>
        private readonly Rectangle SetButtonSprite = new(294, 428, 21, 11);

        /// <summary>The button area in screen pixels.</summary>
        private Rectangle SetButtonBounds;

        /// <summary>Construct an instance.</summary>
        /// <param name="label">The field label.</param>
        /// <param name="slotWidth">The field width.</param>
        /// <param name="toggle">The action to perform when the button is toggled.</param>
        /// <param name="disabled">Whether the button should be disabled.</param>
        protected BaseButtonElement(string label, int slotWidth, Action<TAction> toggle, bool disabled = false)
            : base(label, -1, -1, slotWidth + 1, 11 * Game1.pixelZoom)
        {
            this.SetButtonBounds = new Rectangle(slotWidth - 28 * Game1.pixelZoom, -1 + Game1.pixelZoom * 3, 21 * Game1.pixelZoom, 11 * Game1.pixelZoom);
            this.Toggle = toggle;
            this.greyedOut = disabled;
        }

        /// <summary>Handle the player clicking the left mouse button.</summary>
        /// <param name="x">The cursor's X pixel position.</param>
        /// <param name="y">The cursor's Y pixel position.</param>
        public override void receiveLeftClick(int x, int y)
        {
            if (this.greyedOut || !this.SetButtonBounds.Contains(x, y))
                return;

            // callback handler
            this.Toggle(this.GetValue());
        }

        /// <summary>Draw the component to the screen.</summary>
        /// <param name="spriteBatch">The sprite batch being drawn.</param>
        /// <param name="slotX">The X position at which to draw, relative to the bounds.</param>
        /// <param name="slotY">The Y position at which to draw, relative to the bounds.</param>
        /// <param name="context">The menu drawing the component.</param>
        public sealed override void draw(SpriteBatch spriteBatch, int slotX, int slotY, IClickableMenu? context = null)
        {
            this.DrawElement(spriteBatch, slotX, slotY, context);
            Utility.drawWithShadow(spriteBatch, Game1.mouseCursors, new Vector2(this.SetButtonBounds.X + slotX, this.SetButtonBounds.Y + slotY), this.SetButtonSprite, Color.White, 0.0f, Vector2.Zero, Game1.pixelZoom, false, 0.15f);
        }

        /// <summary>Draw the component to the screen.</summary>
        /// <param name="spriteBatch">The sprite batch being drawn.</param>
        /// <param name="slotX">The X position at which to draw, relative to the bounds.</param>
        /// <param name="slotY">The Y position at which to draw, relative to the bounds.</param>
        /// <param name="context">The menu drawing the component.</param>
        protected abstract void DrawElement(SpriteBatch spriteBatch, int slotX, int slotY, IClickableMenu? context = null);

        /// <summary>Get the current value of the component.</summary>
        protected abstract TAction GetValue();
    }
}
