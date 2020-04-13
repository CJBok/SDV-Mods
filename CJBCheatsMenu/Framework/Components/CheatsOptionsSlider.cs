using System;
using CJB.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.Components
{
    /// <summary>A UI slider for selecting from a range of values.</summary>
    internal class CheatsOptionsSlider : BaseOptionsElement
    {
        /*********
        ** Fields
        *********/
        /// <summary>The field label.</summary>
        private readonly string Label;

        /// <summary>The callback to invoke when the value changes.</summary>
        private readonly Action<int> SetValue;

        /// <summary>The minimum value that can be selected using the field.</summary>
        private readonly int MinValue;

        /// <summary>The maximum value that can be selected using the field.</summary>
        private readonly int MaxValue;

        /// <summary>The current value.</summary>
        private int Value;

        /// <summary>The value's position in the range of values between <see cref="MinValue"/> and <see cref="MaxValue"/>.</summary>
        private float ValuePosition;

        /// <summary>Whether the slider should be disabled.</summary>
        private readonly Func<bool> IsDisabled;

        /// <summary>Format the display label.</summary>
        private readonly Func<int, string> Format;

        /// <summary>The pixel width of the slider area.</summary>
        private int PixelWidth => this.bounds.Width - 10 * Game1.pixelZoom;


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="label">The field label.</param>
        /// <param name="value">The initial value.</param>
        /// <param name="maxValue">The maximum value that can be selected using the field.</param>
        /// <param name="setValue">The callback to invoke when the value changes.</param>
        /// <param name="minValue">The minimum value that can be selected using the field.</param>
        /// <param name="disabled">Whether the slider should be disabled.</param>
        /// <param name="format">Format the display label.</param>
        /// <param name="width">The field width.</param>
        public CheatsOptionsSlider(string label, int value, int maxValue, Action<int> setValue, int minValue = 0, Func<bool> disabled = null, Func<int, string> format = null, int width = 48)
            : base(label, -1, -1, width * Game1.pixelZoom, 6 * Game1.pixelZoom, 0)
        {
            this.Label = label;
            this.Value = value;
            this.MinValue = minValue;
            this.MaxValue = maxValue;
            this.SetValue = setValue;
            this.IsDisabled = disabled ?? (() => false);
            this.Format = format ?? (v => v.ToString());

            this.ValuePosition = this.GetRangePosition();
        }

        /// <summary>Handle the player holding the left mouse button.</summary>
        /// <param name="x">The cursor's X pixel position.</param>
        /// <param name="y">The cursor's Y pixel position.</param>
        public override void leftClickHeld(int x, int y)
        {
            if (this.greyedOut)
                return;

            base.leftClickHeld(x, y);

            this.ValuePosition = CommonHelper.GetRangePosition(x, this.bounds.X, this.bounds.X + this.PixelWidth);
            this.Value = CommonHelper.GetValueAtPosition(this.ValuePosition, this.MinValue, this.MaxValue);
        }

        /// <summary>Handle the player clicking the left mouse button.</summary>
        /// <param name="x">The cursor's X pixel position.</param>
        /// <param name="y">The cursor's Y pixel position.</param>
        public override void receiveLeftClick(int x, int y)
        {
            if (this.greyedOut)
                return;
            base.receiveLeftClick(x, y);
            this.leftClickHeld(x, y);
        }

        /// <summary>Handle the player releasing the left mouse button.</summary>
        /// <param name="x">The cursor's X pixel position.</param>
        /// <param name="y">The cursor's Y pixel position.</param>
        public override void leftClickReleased(int x, int y)
        {
            this.ValuePosition = this.GetRangePosition(); // snap to value position
            this.SetValue(this.Value);
        }

        /// <summary>Draw the component to the screen.</summary>
        /// <param name="spriteBatch">The sprite batch being drawn.</param>
        /// <param name="slotX">The X position at which to draw, relative to the bounds.</param>
        /// <param name="slotY">The Y position at which to draw, relative to the bounds.</param>
        public override void draw(SpriteBatch spriteBatch, int slotX, int slotY)
        {
            this.label = $"{this.Label}: {this.Format(this.Value)}";
            this.greyedOut = this.IsDisabled();

            base.draw(spriteBatch, slotX + this.GetOffsetX(), slotY);

            int sliderOffsetX = CommonHelper.GetValueAtPosition(this.ValuePosition, 0, this.PixelWidth);
            IClickableMenu.drawTextureBox(spriteBatch, Game1.mouseCursors, OptionsSlider.sliderBGSource, slotX + this.bounds.X, slotY + this.bounds.Y, this.bounds.Width, this.bounds.Height, Color.White, Game1.pixelZoom, false);
            spriteBatch.Draw(Game1.mouseCursors, new Vector2(slotX + this.bounds.X + sliderOffsetX, slotY + this.bounds.Y), OptionsSlider.sliderButtonRect, Color.White, 0.0f, Vector2.Zero, Game1.pixelZoom, SpriteEffects.None, 0.9f);
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Get the <see cref="Value"/>'s fractional position, as a value between 0 (<see cref="MinValue"/>) and 1 (<see cref="MaxValue"/>).</summary>
        private float GetRangePosition()
        {
            return CommonHelper.GetRangePosition(this.Value, this.MinValue, this.MaxValue);
        }
    }
}
