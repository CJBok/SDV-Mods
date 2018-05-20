using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.View
{
    /// <summary>
    /// Renders a slider option.
    /// </summary>
    internal class ViewOptionSlider : ViewOption<Menu.IOptionSlider>
    {
        /// <summary>
        /// The minimum width a slider can be.
        /// </summary>
        private const int MIN_WIDTH = 48;

        /// <summary>
        /// The maximum width a slider can be.
        /// </summary>
        private const int MAX_WIDTH = 96;

        /// <summary>
        /// The value the slider is currently set to (current even during drag operation).
        /// </summary>
        private int sliderValue;

        /// <summary>
        /// Constructor for a view option with slider.
        /// </summary>
        /// <param name="sliderOption">The underlying option to render.</param>
        public ViewOptionSlider(Menu.IOptionSlider sliderOption)
            : base(sliderOption, MIN_WIDTH * Game1.pixelZoom, 6 * Game1.pixelZoom, 0)
        {
            if (sliderOption.MinValue >= sliderOption.MaxValue)
            {
                throw new ArgumentException(String.Format("ISliderOption minValue cannot be greater than the maxValue. Got min({0}) max({1})", sliderOption.MinValue, sliderOption.MaxValue));
            }

            if (sliderOption.Step <= 0)
            {
                throw new ArgumentException(String.Format("ISliderOption step must be positive. Got {0})", sliderOption.Step));
            }

            this.SliderValue = sliderOption.Value;
            this.bounds.Width = Math.Min(MAX_WIDTH, Math.Max(MIN_WIDTH, this.SliderSegments * 5)) * Game1.pixelZoom;
        }

        /// <summary>
        /// The value the slider is currently set to (current even during drag operation).
        /// </summary>
        /// <remarks>
        /// This is opposed to Value, which is only updated when the slider is released.
        /// </remarks>
        private int SliderValue
        {
            get
            {
                return sliderValue;
            }
            set
            {
                if (value < this.Option.MinValue)
                {
                    this.sliderValue = this.Option.MinValue;
                }
                else if (value > this.Option.MaxValue)
                {
                    this.sliderValue = this.Option.MaxValue;
                }
                else
                {
                    this.sliderValue = value;
                }
            }
        }

        /// <summary>
        /// Then number of different values the slider can be set to.
        /// </summary>
        private int SliderSegments
        {
            get
            {
                return ((this.Option.MaxValue - this.Option.MinValue) / this.Option.Step) + 1;
            }
        }

        /// <summary>
        /// The index of the currently selected slider segment.
        /// </summary>
        private int CurrentSliderSegment
        {
            get
            {
                return (this.SliderValue - this.Option.MinValue) / (this.Option.Step);
            }
            set
            {
                this.SliderValue = this.Option.MinValue + value * this.Option.Step;
            }
        }

        /// <summary>
        /// Changes the SliderValue as the mouse is dragged.
        /// </summary>
        /// <param name="x">The x position of the mouse drag.</param>
        /// <param name="y">The y position of the mouse drag.</param>
        public override void leftClickHeld(int x, int y)
        {
            if (this.greyedOut)
                return;

            base.leftClickHeld(x, y);

            double xPositionInSlider = ((double)x - this.bounds.X) / this.bounds.Width;
            this.CurrentSliderSegment = (int) (xPositionInSlider * this.SliderSegments);
        }

        /// <summary>
        /// Called when a left click occurs, starts the draging of the slider.
        /// </summary>
        /// <param name="x">x position of the left click.</param>
        /// <param name="y">y position of the left click.</param>
        public override void receiveLeftClick(int x, int y)
        {
            if (this.greyedOut)
                return;
            base.receiveLeftClick(x, y);
            this.leftClickHeld(x, y);
        }

        /// <summary>
        /// Called when left click is released, updates the Value to the currently selected SliderValue.
        /// </summary>
        /// <param name="x">x position of the mouse when left click released.</param>
        /// <param name="y">y position of the mouse when left click released.</param>
        public override void leftClickReleased(int x, int y)
        {
            this.Option.Value = this.SliderValue;
        }

        public override string DrawnLabel => $"{this.Option.Label}: {this.Option.ConvertValueToString(this.SliderValue)}";

        /// <summary>
        /// Draws the option into the menu.
        /// </summary>
        /// <param name="spriteBatch">Passed to the base stardew valley renderer to perform rendering.</param>
        /// <param name="slotX">x position of the option to begin rendering.</param>
        /// <param name="slotY">y position of the option to begin rendering.</param>
        public override void draw(SpriteBatch spriteBatch, int slotX, int slotY)
        {
            base.draw(spriteBatch, slotX, slotY);
            IClickableMenu.drawTextureBox(spriteBatch, Game1.mouseCursors, OptionsSlider.sliderBGSource, slotX + this.bounds.X, slotY + this.bounds.Y, this.bounds.Width, this.bounds.Height, Color.White, Game1.pixelZoom, false);
            spriteBatch.Draw(Game1.mouseCursors, new Vector2(slotX + this.bounds.X + (this.bounds.Width - 10 * Game1.pixelZoom) * this.CurrentSliderSegment / ((float)this.SliderSegments - 1), slotY + this.bounds.Y), OptionsSlider.sliderButtonRect, Color.White, 0.0f, Vector2.Zero, Game1.pixelZoom, SpriteEffects.None, 0.9f);
        }
    }
}
