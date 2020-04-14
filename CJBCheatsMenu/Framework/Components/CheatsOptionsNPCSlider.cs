using System;
using CJB.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.Components
{
    /// <summary>A UI slider for setting a villager's friendship level.</summary>
    internal class CheatsOptionsNpcSlider : BaseOptionsElement
    {
        /*********
        ** Fields
        *********/
        /// <summary>The current number of hearts.</summary>
        private int Value;

        /// <summary>The maximum number of hearts allowed.</summary>
        private readonly int MaxValue;

        /// <summary>The portrait to display next to the slider.</summary>
        private readonly ClickableTextureComponent Mugshot;

        /// <summary>The callback to invoke when the value changes.</summary>
        private readonly Action<int> SetValue;

        /// <summary>The spritesheet position for a filled heart.</summary>
        private readonly Rectangle FilledHeart = new Rectangle(211, 428, 7, 6);

        /// <summary>The spritesheet position for an empty heart.</summary>
        private readonly Rectangle EmptyHeart = new Rectangle(218, 428, 7, 6);

        /// <summary>The size of one rendered heart, accounting for zoom.</summary>
        private const int HeartSize = 8 * Game1.pixelZoom;


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="npc">The villager NPC represented the slider.</param>
        /// <param name="value">The current number of hearts.</param>
        /// <param name="maxValue">The maximum number of hearts.</param>
        /// <param name="isMet">Whether the player has met the NPC.</param>
        /// <param name="setValue">The callback to invoke when the value changes.</param>
        public CheatsOptionsNpcSlider(NPC npc, int value, int maxValue, bool isMet, Action<int> setValue)
            : base(label: npc.displayName, x: 96, y: -1, width: maxValue * HeartSize, height: 6 * Game1.pixelZoom, whichOption: 0)
        {
            this.SetValue = setValue;
            this.Mugshot = new ClickableTextureComponent("Mugshot", this.bounds, "", "", npc.Sprite.Texture, npc.getMugShotSourceRect(), 0.7f * Game1.pixelZoom);
            this.greyedOut = !isMet;
            this.label = npc.getName();
            this.Value = value;
            this.MaxValue = maxValue;
        }

        /// <summary>Handle the player holding the left mouse button.</summary>
        /// <param name="x">The cursor's X pixel position.</param>
        /// <param name="y">The cursor's Y pixel position.</param>
        public override void leftClickHeld(int x, int y)
        {
            base.leftClickHeld(x, y);

            int width = this.bounds.Width - 5;
            float valuePosition = CommonHelper.GetRangePosition(x + (HeartSize / 2), this.bounds.X, this.bounds.X + width); // offset by half a heart, so clicking the middle of a heart selects it

            this.greyedOut = false;
            this.Value = CommonHelper.GetValueAtPosition(valuePosition, 0, this.MaxValue);
            this.SetValue(this.Value);
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

        /// <summary>Draw the component to the screen.</summary>
        /// <param name="spriteBatch">The sprite batch being drawn.</param>
        /// <param name="slotX">The X position at which to draw, relative to the bounds.</param>
        /// <param name="slotY">The Y position at which to draw, relative to the bounds.</param>
        public override void draw(SpriteBatch spriteBatch, int slotX, int slotY)
        {
            base.draw(spriteBatch, slotX + this.GetOffsetX(), slotY);

            Color tint = this.greyedOut ? (Color.White * 0.5f) : Color.White;

            // draw mugshot
            if (this.Mugshot != null)
            {
                this.Mugshot.bounds = new Rectangle(slotX + 32, slotY, Game1.tileSize, Game1.tileSize);
                this.Mugshot.draw(spriteBatch, tint, 0.88f);
            }


            // draw hearts
            for (int i = 0; i < this.MaxValue; i++)
            {
                Rectangle heartSourceRect = i < this.Value
                    ? this.FilledHeart
                    : this.EmptyHeart;

                spriteBatch.Draw(Game1.mouseCursors, new Vector2(slotX + this.bounds.X + (i * HeartSize), slotY + this.bounds.Y), heartSourceRect, tint, 0f, Vector2.Zero, Game1.pixelZoom, SpriteEffects.None, 0.88f);
            }
        }
    }
}
