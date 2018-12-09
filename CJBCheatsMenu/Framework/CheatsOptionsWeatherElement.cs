using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework
{
    internal class CheatsOptionsWeatherElement : OptionsElement
    {
        /*********
        ** Fields
        *********/
        /// <summary>Get the current weather name.</summary>
        private readonly Func<string> CurrentWeather;


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="label">The field label.</param>
        /// <param name="currentWeather">Get the current weather name.</param>
        public CheatsOptionsWeatherElement(string label, Func<string> currentWeather)
          : base(label)
        {
            this.CurrentWeather = currentWeather;
            this.whichOption = 0;
        }

        public override void draw(SpriteBatch spriteBatch, int slotX, int slotY)
        {
            string info = this.CurrentWeather();
            Utility.drawTextWithShadow(spriteBatch, $"{this.label}: {info}", Game1.dialogueFont, new Vector2(this.bounds.X + slotX, this.bounds.Y + slotY), Game1.textColor, 1f, 0.15f);
        }
    }
}
