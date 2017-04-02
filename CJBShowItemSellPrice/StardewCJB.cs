using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;

namespace CJBShowItemSellPrice
{
    public class StardewCJB : Mod
    {
        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            GraphicsEvents.OnPostRenderGuiEvent += GraphicsEvents_OnPostRenderGuiEvent;
        }


        /*********
        ** Private methods
        *********/
        private void GraphicsEvents_OnPostRenderGuiEvent(object sender, EventArgs e)
        {
            if (Game1.activeClickableMenu == null)
                return;

            // get item
            Item item = this.GetItemFromMenu(Game1.activeClickableMenu);
            if (item == null)
                return;

            // show hover info
            if (item is StardewValley.Object obj)
                this.DrawHoverTextBox(Game1.smallFont, obj.sellToStorePrice(), obj.stack);
            else
            {
                if (item.Stack > 1)
                    this.DrawHoverTextBox(Game1.smallFont, (item.salePrice() / 2), item.Stack);
                else
                    this.DrawHoverTextBox(Game1.smallFont, item.salePrice());
            }
        }

        /// <summary>Get the hovered item from an arbitrary menu.</summary>
        /// <param name="menu">The menu whose hovered item to find.</param>
        private Item GetItemFromMenu(IClickableMenu menu)
        {
            // game menu
            if (menu is GameMenu gameMenu)
            {
                if (gameMenu.currentTab == GameMenu.inventoryTab)
                {
                    List<IClickableMenu> pages = (List<IClickableMenu>)typeof(GameMenu).GetField("pages", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(gameMenu);
                    InventoryPage inv = (InventoryPage)pages[0];
                    return (Item)typeof(InventoryPage).GetField("hoveredItem", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(inv);
                }
                if (gameMenu.currentTab == GameMenu.craftingTab)
                {
                    List<IClickableMenu> pages = (List<IClickableMenu>)typeof(GameMenu).GetField("pages", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(gameMenu);
                    CraftingPage inv = (CraftingPage)pages[4];
                    return (Item)typeof(CraftingPage).GetField("hoverItem", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(inv);
                }
            }

            // from inventory UI
            else if (menu is MenuWithInventory inventoryMenu)
                return inventoryMenu.hoveredItem;

            return null;
        }

        private void DrawHoverTextBox(SpriteFont font, int price, int stack = -1)
        {
            if (price < 1)
                return;

            string priceString = price.ToString();
            string stackPriceString = Environment.NewLine + (price * stack);

            string message = "Single: " + price;
            string message1 = "Single: ";

            if (stack > 1)
            {
                message += Environment.NewLine + "Stack: " + price * stack;
                message1 += Environment.NewLine + "Stack: ";
            }

            Vector2 bounds = font.MeasureString(message);
            int width = (int)bounds.X + Game1.tileSize / 2 + 40;
            int height = (int)font.MeasureString(message).Y + Game1.tileSize / 3 + 5;

            int x = (int)(Mouse.GetState().X / Game1.options.zoomLevel) - Game1.tileSize / 2 - width;
            int y = (int)(Mouse.GetState().Y / Game1.options.zoomLevel) + Game1.tileSize / 2;

            if (x < 0)
                x = 0;
            if (y + height > Game1.graphics.GraphicsDevice.Viewport.Height)
                y = Game1.graphics.GraphicsDevice.Viewport.Height - height;

            IClickableMenu.drawTextureBox(Game1.spriteBatch, Game1.menuTexture, new Rectangle(0, 256, 60, 60), x, y, width, height, Color.White);
            Game1.spriteBatch.Draw(Game1.debrisSpriteSheet, new Vector2(x + Game1.tileSize / 4 + font.MeasureString(message + "   ").X, y + Game1.tileSize / 4 + 10), Game1.getSourceRectForStandardTileSheet(Game1.debrisSpriteSheet, 8, 16, 16), Color.White, 0.0f, new Vector2(8f, 8f), Game1.pixelZoom, SpriteEffects.None, 1f);
            if (stack > 1)
                Game1.spriteBatch.Draw(Game1.debrisSpriteSheet, new Vector2(x + Game1.tileSize / 4 + font.MeasureString(message + "   ").X, y + Game1.tileSize / 4 + 38), Game1.getSourceRectForStandardTileSheet(Game1.debrisSpriteSheet, 8, 16, 16), Color.White, 0.0f, new Vector2(8f, 8f), Game1.pixelZoom, SpriteEffects.None, 0.95f);
            Utility.drawTextWithShadow(Game1.spriteBatch, message1, font, new Vector2(x + Game1.tileSize / 4, y + Game1.tileSize / 4), Game1.textColor);

            Utility.drawTextWithShadow(Game1.spriteBatch, priceString, font, new Vector2(x + width + Game1.tileSize / 4 - 60 - font.MeasureString(priceString).X, y + Game1.tileSize / 4), Game1.textColor);
            if (stack > 1)
                Utility.drawTextWithShadow(Game1.spriteBatch, stackPriceString, font, new Vector2(x + width + Game1.tileSize / 4 - 60 - font.MeasureString(stackPriceString).X, y + Game1.tileSize / 4), Game1.textColor);
        }
    }
}
