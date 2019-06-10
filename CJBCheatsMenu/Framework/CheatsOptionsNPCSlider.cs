using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Quests;

namespace CJBCheatsMenu.Framework
{
    /// <summary>A UI slider for setting a villager's friendship level.</summary>
    internal class CheatsOptionsNpcSlider : OptionsElement
    {
        /*********
        ** Fields
        *********/
        /// <summary>The maximum number of hearts allowed.</summary>
        private readonly int SliderMaxValue;

        /// <summary>The villager NPC represented the slider.</summary>
        private readonly NPC Npc;

        /// <summary>The portrait to display next to the slider.</summary>
        private readonly ClickableTextureComponent Mugshot;

        /// <summary>The current value.</summary>
        private int Value;

        /// <summary>A callback to invoke when the value changes.</summary>
        private readonly Action<int> OnValueChanged;


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="npc">The villager NPC represented the slider.</param>
        /// <param name="onValueChanged">A callback to invoke when the value changes.</param>
        public CheatsOptionsNpcSlider(NPC npc, Action<int> onValueChanged)
            : base(npc.displayName, x: 96, y: -1, width: (CheatsOptionsNpcSlider.IsSpouse(npc) ? 112 : 80) * Game1.pixelZoom, height: 6 * Game1.pixelZoom, whichOption: 0)
        {
            bool isKnown = Game1.player.friendshipData.TryGetValue(npc.Name, out Friendship friendship);

            this.Npc = npc;
            this.OnValueChanged = onValueChanged;
            this.Mugshot = new ClickableTextureComponent("Mugshot", this.bounds, "", "", npc.Sprite.Texture, npc.getMugShotSourceRect(), 0.7f * Game1.pixelZoom);
            this.greyedOut = !isKnown;
            this.label = npc.getName();
            this.SliderMaxValue = CheatsOptionsNpcSlider.IsSpouse(npc) ? 14 : NPC.maxFriendshipPoints / NPC.friendshipPointsPerHeartLevel;
            this.Value = isKnown
                ? Math.Max(0, Math.Min(this.SliderMaxValue, friendship.Points / NPC.friendshipPointsPerHeartLevel))
                : 0;
        }

        /// <summary>The method invoked each update tick while the player is holding the left mouse button.</summary>
        /// <param name="x">The X-position of the cursor.</param>
        /// <param name="y">The Y-position of the cursor.</param>
        /// <returns>Whether the event has been handled and shouldn't be propagated further.</returns>
        public override void leftClickHeld(int x, int y)
        {
            // calculate new value
            base.leftClickHeld(x, y);
            this.Value = x >= this.bounds.X ? (x <= this.bounds.Right - 10 * Game1.pixelZoom ? (int)((x - this.bounds.X) / (this.bounds.Width - 10d * Game1.pixelZoom) * this.SliderMaxValue) : this.SliderMaxValue) : 0;

            // add friendship if needed
            if (!Game1.player.friendshipData.TryGetValue(this.Npc.Name, out Friendship friendship))
            {
                friendship = new Friendship();
                Game1.player.friendshipData.Add(this.Npc.Name, friendship);
                SocializeQuest socialQuest = Game1.player.questLog.OfType<SocializeQuest>().FirstOrDefault();
                if (socialQuest != null && !socialQuest.completed.Value)
                    socialQuest.checkIfComplete(this.Npc);
                this.greyedOut = false;
            }

            // update friendship points
            friendship.Points = this.Value * NPC.friendshipPointsPerHeartLevel;
            this.OnValueChanged(friendship.Points);
        }

        /// <summary>The method invoked when the player clicks the left mouse button.</summary>
        /// <param name="x">The X-position of the cursor.</param>
        /// <param name="y">The Y-position of the cursor.</param>
        /// <returns>Whether the event has been handled and shouldn't be propagated further.</returns>
        public override void receiveLeftClick(int x, int y)
        {
            if (this.greyedOut)
                return;
            base.receiveLeftClick(x, y);
            this.leftClickHeld(x, y);
        }

        /// <summary>Draw the UI element to the screen.</summary>
        /// <param name="spriteBatch">The sprite batch being drawn.</param>
        /// <param name="slotX">The X position at which to draw the element.</param>
        /// <param name="slotY">The Y position at which to draw the element.</param>
        public override void draw(SpriteBatch spriteBatch, int slotX, int slotY)
        {
            base.draw(spriteBatch, slotX, slotY);

            Color tint = this.greyedOut ? (Color.White * 0.5f) : Color.White;

            if (this.Mugshot != null)
            {
                this.Mugshot.bounds = new Rectangle(slotX + 32, slotY, Game1.tileSize, Game1.tileSize);
                this.Mugshot.draw(spriteBatch, tint, 0.88f);
            }

            for (int i = 0; i < this.SliderMaxValue; i++)
            {
                Rectangle sourceRectangle = i < this.Value
                    ? new Rectangle(211, 428, 7, 6)
                    : new Rectangle(218, 428, 7, 6);
                spriteBatch.Draw(Game1.mouseCursors, new Vector2(slotX + this.bounds.X + i * (8 * Game1.pixelZoom), slotY + this.bounds.Y), sourceRectangle, tint, 0f, Vector2.Zero, Game1.pixelZoom, SpriteEffects.None, 0.88f);
            }
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Get whether the given NPC is the player's spouse.</summary>
        /// <param name="npc">The NPC to check.</param>
        private static bool IsSpouse(NPC npc)
        {
            return npc.Name == Game1.player.spouse;
        }
    }
}
