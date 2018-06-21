using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework
{
    internal class CheatsOptionsCheckbox : OptionsElement
    {
        /*********
        ** Properties
        *********/
        /// <summary>A callback to invoke when the value changes.</summary>
        private readonly Action<bool> SetValue;

        /// <summary>Whether the checkbox is currently checked.</summary>
        private bool IsChecked;


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="label">The checkbox label.</param>
        /// <param name="initialValue">The initial value to set.</param>
        /// <param name="setValue">A callback to invoke when the value changes.</param>
        public CheatsOptionsCheckbox(string label, bool initialValue, Action<bool> setValue)
          : base(label, -1, -1, 9 * Game1.pixelZoom, 9 * Game1.pixelZoom, 0)
        {
            this.IsChecked = initialValue;
            this.SetValue = setValue;
        }

        public override void receiveLeftClick(int x, int y)
        {
            if (this.greyedOut)
                return;
            Game1.soundBank.PlayCue("drumkit6");
            base.receiveLeftClick(x, y);
            this.IsChecked = !this.IsChecked;
            this.SetValue(this.IsChecked);
        }

        public override void draw(SpriteBatch spriteBatch, int slotX, int slotY)
        {
            spriteBatch.Draw(Game1.mouseCursors, new Vector2(slotX + this.bounds.X, slotY + this.bounds.Y), this.IsChecked ? OptionsCheckbox.sourceRectChecked : OptionsCheckbox.sourceRectUnchecked, Color.White * (this.greyedOut ? 0.33f : 1f), 0.0f, Vector2.Zero, Game1.pixelZoom, SpriteEffects.None, 0.4f);
            base.draw(spriteBatch, slotX, slotY);
        }
    }
}
