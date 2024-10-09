using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.Components
{
    /// <summary>A button with a label which invokes a callback when clicked.</summary>
    internal class CheatsOptionsButton<TButton> : BaseOptionsElement
        where TButton : CheatsOptionsButton<TButton>
    {
        /*********
        ** Fields
        *********/
        /// <summary>The action to perform when the button is toggled (or <c>null</c> to handle it manually).</summary>
        private readonly Action<TButton> Toggle;

        /// <summary>The source rectangle for the 'set' button sprite.</summary>
        private readonly Rectangle SetButtonSprite = new(294, 428, 21, 11);

        /// <summary>The button area in screen pixels.</summary>
        private Rectangle SetButtonBounds;


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="label">The field label.</param>
        /// <param name="slotWidth">The field width.</param>
        /// <param name="toggle">The action to perform when the button is toggled.</param>
        /// <param name="disabled">Whether the button should be disabled.</param>
        public CheatsOptionsButton(string label, int slotWidth, Action<TButton> toggle, bool disabled = false)
          : base(label, -1, -1, slotWidth + 1, 11 * Game1.pixelZoom)
        {
            this.SetButtonBounds = new Rectangle(slotWidth - 28 * Game1.pixelZoom, -1 + Game1.pixelZoom * 3, 21 * Game1.pixelZoom, 11 * Game1.pixelZoom);
            this.Toggle = toggle;
            this.greyedOut = disabled;
        }

        /// <summary>Construct an instance.</summary>
        /// <param name="label">The field label.</param>
        /// <param name="slotWidth">The field width.</param>
        /// <param name="toggle">The action to perform when the button is toggled.</param>
        /// <param name="disabled">Whether the button should be disabled.</param>
        public CheatsOptionsButton(string label, int slotWidth, Action toggle, bool disabled = false)
            : this(label, slotWidth, _ => toggle(), disabled) { }

        /// <inheritdoc />
        public override void receiveLeftClick(int x, int y)
        {
            if (this.greyedOut || !this.SetButtonBounds.Contains(x, y))
                return;

            // callback handler
            this.Toggle((TButton)this);
        }

        /// <inheritdoc />
        public override void draw(SpriteBatch spriteBatch, int slotX, int slotY, IClickableMenu? context = null)
        {
            this.DrawElement(spriteBatch, slotX, slotY, context);
            Utility.drawWithShadow(spriteBatch, Game1.mouseCursors, new Vector2(this.SetButtonBounds.X + slotX, this.SetButtonBounds.Y + slotY), this.SetButtonSprite, Color.White, 0.0f, Vector2.Zero, Game1.pixelZoom, false, 0.15f);
        }

        /// <summary>Draw the component to the screen.</summary>
        /// <param name="spriteBatch">The sprite batch being drawn.</param>
        /// <param name="slotX">The X position at which to draw, relative to the bounds.</param>
        /// <param name="slotY">The Y position at which to draw, relative to the bounds.</param>
        /// <param name="context">The menu drawing the component.</param>
        protected virtual void DrawElement(SpriteBatch spriteBatch, int slotX, int slotY, IClickableMenu? context = null)
        {
            Utility.drawTextWithShadow(spriteBatch, this.label, Game1.dialogueFont, new Vector2(this.bounds.X + slotX, this.bounds.Y + slotY), this.greyedOut ? Game1.textColor * 0.33f : Game1.textColor, 1f, 0.15f);
        }
    }

    /// <inheritdoc cref="CheatsOptionsButton{T}" />
    internal class CheatsOptionsButton : CheatsOptionsButton<CheatsOptionsButton>
    {
        /*********
        ** Public methods
        *********/
        /// <inheritdoc />
        public CheatsOptionsButton(string label, int slotWidth, Action<CheatsOptionsButton> toggle, bool disabled = false)
            : base(label, slotWidth, toggle, disabled) { }

        /// <inheritdoc />
        public CheatsOptionsButton(string label, int slotWidth, Action toggle, bool disabled = false)
            : base(label, slotWidth, _ => toggle(), disabled) { }
    }
}
