using Microsoft.Xna.Framework.Graphics;
using System;

namespace CJBCheatsMenu.Framework.View
{
    /// <summary>
    /// The view that rendors a tab for a menu within the menus dialog.
    /// </summary>
    internal class ViewMenuTab : ViewGroupItem
    {
        /// <summary>
        /// The menu that this tab is for.
        /// </summary>
        public Menu.IMenu Menu { get; private set; }

        /// <summary>
        /// The action that is called when this tab is pressed.
        /// </summary>
        private Action<Menu.IMenu> OnPressedAction;

        /// <summary>
        /// Whether or not this tab should be rendered with some intensity or not.
        /// </summary>
        /// <remarks>
        /// Used to make the currently selected tab stand out against the other tabs.
        /// </remarks>
        public bool Intensify { get; set; } = false;

        /// <summary>
        /// Constructs a menu tab view.
        /// </summary>
        /// <param name="menu">The menu this tab is for.</param>
        /// <param name="width">The width of the tab.</param>
        /// <param name="pressAction">The action that is called when this tab is clicked.</param>
        public ViewMenuTab(Menu.IMenu menu, int width, Action<Menu.IMenu> pressAction)
            : base(menu.Title, width, StardewValley.Game1.tileSize, -1)
        {
            this.Menu = menu;
            this.OnPressedAction = pressAction;
        }

        /// <summary>
        /// Calls the press action when left click received.
        /// </summary>
        /// <param name="x">x position of mouse when left click occured.</param>
        /// <param name="y">y position of mouse when left click occured.</param>
        public override void receiveLeftClick(int x, int y)
        {
            this.OnPressedAction(this.Menu);
        }

        /// <summary>
        /// Draws the tab at the given coordinates.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch used to render assets.</param>
        /// <param name="x">x position where to draw the tab.</param>
        /// <param name="y">y position where to draw the tab.</param>
        public override void draw(SpriteBatch spriteBatch, int x, int y)
        {
            // The tabs are somehow right aligned, so the x position is based on their right most position.
            CJB.DrawTextBox(x + this.bounds.Width, y, StardewValley.Game1.smallFont, this.Menu.Title, 2, this.Intensify ? 1F : 0.7F);
        }
    }
}
