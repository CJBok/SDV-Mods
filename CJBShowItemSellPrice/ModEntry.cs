using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using SObject = StardewValley.Object;

namespace CJBShowItemSellPrice
{
    internal class ModEntry : Mod
    {
        /*********
        ** Properties
        *********/
        /// <summary>The spritesheet source rectangle for the coin icon.</summary>
        private readonly Rectangle CoinSourceRect = Game1.getSourceRectForStandardTileSheet(Game1.debrisSpriteSheet, 8, 16, 16);

        /// <summary>The label text for the single-item price.</summary>
        private string SingleLabel;

        /// <summary>The label text for the stack price.</summary>
        private string StackLabel;


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            GraphicsEvents.OnPostRenderGuiEvent += GraphicsEvents_OnPostRenderGuiEvent;

            this.SingleLabel = this.Helper.Translation.Get("labels.single-price");
            this.StackLabel = this.Helper.Translation.Get("labels.stack-price");
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
            if (item is SObject obj)
                this.DrawHoverTextBox(Game1.smallFont, obj.sellToStorePrice(), obj.stack);
            else
            {
                if (item.Stack > 1)
                    this.DrawHoverTextBox(Game1.smallFont, item.salePrice() / 2, item.Stack);
                else
                    this.DrawHoverTextBox(Game1.smallFont, item.salePrice() / 2);
            }
        }

        /// <summary>Get the hovered item from an arbitrary menu.</summary>
        /// <param name="menu">The menu whose hovered item to find.</param>
        private Item GetItemFromMenu(IClickableMenu menu)
        {
            // game menu
            if (menu is GameMenu gameMenu)
            {
                IClickableMenu page = this.Helper.Reflection.GetField<List<IClickableMenu>>(gameMenu, "pages").GetValue()[gameMenu.currentTab];
                if (page is InventoryPage)
                    return this.Helper.Reflection.GetField<Item>(page, "hoveredItem").GetValue();
                else if (page is CraftingPage)
                    return this.Helper.Reflection.GetField<Item>(page, "hoverItem").GetValue();
            }

            // from inventory UI
            else if (menu is MenuWithInventory inventoryMenu)
                return inventoryMenu.hoveredItem;

            // CJB mods
            else if (menu.GetType().FullName == "CJBItemSpawner.ItemMenu")
                return this.Helper.Reflection.GetField<Item>(menu, "HoveredItem").GetValue();

            return null;
        }

        private void DrawHoverTextBox(SpriteFont font, int price, int stack = -1)
        {
            if (price < 1)
                return;

            string priceString = price.ToString();
            string stackPriceString = $"\n{price * stack}";

            string labels = $"{this.SingleLabel}:";
            string placeholderForSizing = $"{this.SingleLabel}: {price}";
            if (stack > 1)
            {
                labels += $"\n{this.StackLabel}:";
                placeholderForSizing += $"\n{this.StackLabel}: {price * stack}";
            }

            Vector2 size = font.MeasureString(placeholderForSizing) + new Vector2(Game1.tileSize / 2 + 40, Game1.tileSize / 3 + 5);
            int width = (int)size.X;
            int height = (int)size.Y;

            int x = (int)(Mouse.GetState().X / Game1.options.zoomLevel) - Game1.tileSize / 2 - width;
            int y = (int)(Mouse.GetState().Y / Game1.options.zoomLevel) + Game1.tileSize / 2;

            if (x < 0)
                x = 0;
            if (y + height > Game1.graphics.GraphicsDevice.Viewport.Height)
                y = Game1.graphics.GraphicsDevice.Viewport.Height - height;

            IClickableMenu.drawTextureBox(Game1.spriteBatch, Game1.menuTexture, new Rectangle(0, 256, 60, 60), x, y, width, height, Color.White);
            Game1.spriteBatch.Draw(Game1.debrisSpriteSheet, new Vector2(x + Game1.tileSize / 4 + font.MeasureString(placeholderForSizing + "   ").X, y + Game1.tileSize / 4 + 10), this.CoinSourceRect, Color.White, 0.0f, new Vector2(8, 8), Game1.pixelZoom, SpriteEffects.None, 1f);
            if (stack > 1)
                Game1.spriteBatch.Draw(Game1.debrisSpriteSheet, new Vector2(x + Game1.tileSize / 4 + font.MeasureString(placeholderForSizing + "   ").X, y + Game1.tileSize / 4 + 38), this.CoinSourceRect, Color.White, 0.0f, new Vector2(8, 8), Game1.pixelZoom, SpriteEffects.None, 0.95f);
            Utility.drawTextWithShadow(Game1.spriteBatch, labels, font, new Vector2(x + Game1.tileSize / 4, y + Game1.tileSize / 4), Game1.textColor);

            Utility.drawTextWithShadow(Game1.spriteBatch, priceString, font, new Vector2(x + width + Game1.tileSize / 4 - 60 - font.MeasureString(priceString).X, y + Game1.tileSize / 4), Game1.textColor);
            if (stack > 1)
                Utility.drawTextWithShadow(Game1.spriteBatch, stackPriceString, font, new Vector2(x + width + Game1.tileSize / 4 - 60 - font.MeasureString(stackPriceString).X, y + Game1.tileSize / 4), Game1.textColor);
        }
    }
}
