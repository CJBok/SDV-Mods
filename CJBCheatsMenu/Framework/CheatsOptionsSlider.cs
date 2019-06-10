using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework
{
    internal class CheatsOptionsSlider : OptionsElement
    {
        /*********
        ** Fields
        *********/
        /// <summary>The field label.</summary>
        private readonly string Label;

        /// <summary>The callback to invoke when the value changes.</summary>
        private readonly Action<int> SetValue;

        /// <summary>The maximum value that can be selected using the field.</summary>
        private readonly int MaxValue;

        /// <summary>The current value.</summary>
        private int Value;

        /// <summary>Whether the slider should be disabled.</summary>
        private readonly Func<bool> IsDisabled;

        /// <summary>Format the display label.</summary>
        private readonly Func<int, string> Format;


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="label">The field label.</param>
        /// <param name="initialValue">The initial value.</param>
        /// <param name="maxValue">The maximum value that can be selected using the field.</param>
        /// <param name="setValue">The callback to invoke when the value changes.</param>
        /// <param name="disabled">Whether the slider should be disabled.</param>
        /// <param name="format">Format the display label.</param>
        /// <param name="width">The field width.</param>
        public CheatsOptionsSlider(string label, int initialValue, int maxValue, Action<int> setValue, Func<bool> disabled = null, Func<int, string> format = null, int width = 48)
            : base(label, -1, -1, width * Game1.pixelZoom, 6 * Game1.pixelZoom, 0)
        {
            this.Label = label;
            this.Value = initialValue;
            this.MaxValue = maxValue;
            this.SetValue = setValue;
            this.IsDisabled = disabled ?? (() => false);
            this.Format = format ?? (value => value.ToString());
        }

        public override void leftClickHeld(int x, int y)
        {
            if (this.greyedOut)
                return;

            base.leftClickHeld(x, y);
            this.Value = x >= this.bounds.X
                ? (x <= this.bounds.Right - 10 * Game1.pixelZoom ? (int)((x - this.bounds.X) / (this.bounds.Width - 10d * Game1.pixelZoom) * this.MaxValue) : this.MaxValue)
                : 0;

        }

        public override void receiveLeftClick(int x, int y)
        {
            if (this.greyedOut)
                return;
            base.receiveLeftClick(x, y);
            this.leftClickHeld(x, y);
        }

        public override void leftClickReleased(int x, int y)
        {
            this.SetValue(this.Value);
        }

        public override void draw(SpriteBatch spriteBatch, int slotX, int slotY)
        {
            this.label = $"{this.Label}: {this.Format(this.Value)}";
            this.greyedOut = this.IsDisabled();

            base.draw(spriteBatch, slotX, slotY);
            IClickableMenu.drawTextureBox(spriteBatch, Game1.mouseCursors, OptionsSlider.sliderBGSource, slotX + this.bounds.X, slotY + this.bounds.Y, this.bounds.Width, this.bounds.Height, Color.White, Game1.pixelZoom, false);
            spriteBatch.Draw(Game1.mouseCursors, new Vector2(slotX + this.bounds.X + (this.bounds.Width - 10 * Game1.pixelZoom) * (this.Value / (float)this.MaxValue), slotY + this.bounds.Y), OptionsSlider.sliderButtonRect, Color.White, 0.0f, Vector2.Zero, Game1.pixelZoom, SpriteEffects.None, 0.9f);
        }
    }
}
