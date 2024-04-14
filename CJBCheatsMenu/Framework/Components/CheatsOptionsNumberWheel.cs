using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.Components
{
    internal class CheatsOptionsNumberWheel : CheatsOptionsButton<CheatsOptionsNumberWheel>
    {
        /*********
        ** Fields
        *********/
        /// <summary>The minimum value that can be selected using the field.</summary>
        private readonly int MinValue;

        /// <summary>The maximum value that can be selected using the field.</summary>
        private readonly int MaxValue;

        /// <summary>The minus button area in screen pixels.</summary>
        private readonly Rectangle CurrentValueBounds;

        /// <summary>The minus button area in screen pixels.</summary>
        private Rectangle MinusButtonBounds;

        /// <summary>The plus button area in screen pixels.</summary>
        private Rectangle PlusButtonBounds;

        /// <summary>The source rectangle for the 'minus' button sprite.</summary>
        private readonly Rectangle MinusButtonSource = new Rectangle(177, 345, 7, 8);

        /// <summary>The source rectangle for the 'plus' button sprite.</summary>
        private readonly Rectangle PlusButtonSource = new Rectangle(184, 345, 7, 8);


        /*********
        ** Accessors
        *********/
        /// <summary>The current value.</summary>
        public int Value { get; private set; }


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="label">The field label.</param>
        /// <param name="slotWidth">The field width.</param>
        /// <param name="action">The action to perform when the set button is clicked.</param>
        /// <param name="value">The starting value of the number.</param>
        /// <param name="maxValue">The maximum value of the number.</param>
        /// <param name="minValue">The minimum value of the number.</param>
        public CheatsOptionsNumberWheel(string label, int slotWidth, Action<CheatsOptionsNumberWheel> action, int value, int maxValue,
            int minValue = 0)
            : base(label, slotWidth, action)
        {
            if (value < minValue || value > maxValue)
            {
                value = minValue;
            }
            this.Value = value;
            this.MaxValue = maxValue;
            this.MinValue = minValue;

            // calculate the correct height for the button to be in the middle of the line
            int buttonY =
                (int) ((double)this.bounds.Height / Game1.pixelZoom / 2d + this.MinusButtonSource.Height / 2d);

            // leave space for the label at the front
            int labelWidth = (int)Game1.dialogueFont.MeasureString(label).X / Game1.pixelZoom;
            int xOffset = labelWidth + 4;

            this.MinusButtonBounds = new Rectangle(xOffset * Game1.pixelZoom, buttonY, 7 * Game1.pixelZoom,
                8 * Game1.pixelZoom);
            xOffset += this.MinusButtonSource.Width + 2;

            // get the maximum width the value label can be
            string maxSizeValue = new('9', maxValue.ToString().Length);
            Vector2 maxValueSize = Game1.dialogueFont.MeasureString(maxSizeValue);
            int maxValueWidth = (int)(maxValueSize.X / Game1.pixelZoom) + 1;
            int maxValueHeight = (int)maxValueSize.Y;

            // leave enough space to draw the value without overlapping the plus button
            this.CurrentValueBounds = new Rectangle(xOffset * Game1.pixelZoom, Game1.pixelZoom,
                maxValueWidth * Game1.pixelZoom, maxValueHeight);
            xOffset += maxValueWidth + 2;
            this.PlusButtonBounds = new Rectangle(xOffset * Game1.pixelZoom, buttonY, 7 * Game1.pixelZoom,
                8 * Game1.pixelZoom);
        }

        /// <summary>Handle the player clicking the left mouse button.</summary>
        /// <param name="x">The cursor's X pixel position.</param>
        /// <param name="y">The cursor's Y pixel position.</param>
        public override void receiveLeftClick(int x, int y)
        {
            base.receiveLeftClick(x, y);

            if (this.greyedOut)
                return;

            // holding run (shift) will make the number go in steps of 10
            // holding control will do stops of 50, both will do steps of 500
            // todo: nicer way to do this on android?
            int multiplier = Game1.isOneOfTheseKeysDown(Game1.GetKeyboardState(), Game1.options.runButton) ? 10 : 1;
            multiplier *= Game1.GetKeyboardState().IsKeyDown(Keys.LeftControl) ? 50 : 1;

            // stupid hack to get the bounds to line up properly
            if (this.MinusButtonBounds.Contains(x - 8 * Game1.pixelZoom, y - 7 * Game1.pixelZoom))
                this.ChangeValue(-1 * multiplier);

            if (this.PlusButtonBounds.Contains(x - 8 * Game1.pixelZoom, y - 7 * Game1.pixelZoom))
                this.ChangeValue(1 * multiplier);
        }


        /*********
        ** Protected methods
        *********/
        /// <summary>Draw the component to the screen.</summary>
        /// <param name="spriteBatch">The sprite batch being drawn.</param>
        /// <param name="slotX">The X position at which to draw, relative to the bounds.</param>
        /// <param name="slotY">The Y position at which to draw, relative to the bounds.</param>
        /// <param name="context">The menu drawing the component.</param>
        protected override void DrawElement(SpriteBatch spriteBatch, int slotX, int slotY, IClickableMenu? context = null)
        {
            // draw label
            Utility.drawTextWithShadow(spriteBatch, this.label, Game1.dialogueFont,
                new Vector2(this.bounds.X + slotX, this.bounds.Y + slotY),
                this.greyedOut ? Game1.textColor * 0.33f : Game1.textColor, 1f, 0.15f);

            // draw minus button
            Utility.drawWithShadow(spriteBatch, Game1.mouseCursors,
                new Vector2(this.bounds.X + this.MinusButtonBounds.X + slotX,
                    this.bounds.Y + this.MinusButtonBounds.Y + slotY), this.MinusButtonSource,
                Color.White, 0.0f, Vector2.Zero, Game1.pixelZoom, false, 0.15f);

            // draw value
            Utility.drawTextWithShadow(spriteBatch, this.Value.ToString(), Game1.dialogueFont,
                new Vector2(this.bounds.X + this.CurrentValueBounds.X + slotX,
                    this.bounds.Y + this.CurrentValueBounds.Y + slotY), Game1.textColor,
                1f, 0.15f);

            // draw plus button
            Utility.drawWithShadow(spriteBatch, Game1.mouseCursors,
                new Vector2(this.bounds.X + this.PlusButtonBounds.X + slotX,
                    this.bounds.Y + this.PlusButtonBounds.Y + slotY), this.PlusButtonSource,
                Color.White, 0.0f, Vector2.Zero, Game1.pixelZoom, false, 0.15f);
        }

        /// <summary>Change the current value of the component, and play a sound to signal success.</summary>
        private void ChangeValue(int delta)
        {
            int newValue = Math.Clamp(this.Value + delta, this.MinValue, this.MaxValue);
            if (newValue == this.Value)
            {
                return;
            }

            this.Value = newValue;
            Game1.playSound("drumkit6");
        }
    }
}
