using System;
using System.Collections.Generic;
using System.Linq;
using CJBShowItemSellPrice.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Menus;
using SObject = StardewValley.Object;

namespace CJBShowItemSellPrice
{
    /// <summary>The mod entry point.</summary>
    internal class ModEntry : Mod
    {
        /*********
        ** Fields
        *********/
        /// <summary>The spritesheet source rectangle for the coin icon.</summary>
        private readonly Rectangle CoinSourceRect = new Rectangle(5, 69, 6, 6);

        /// <summary>The spritesheet source rectangle for the tooltip box.</summary>
        private readonly Rectangle TooltipSourceRect = new Rectangle(0, 256, 60, 60);

        /// <summary>The pixel size of the tooltip box's border (i.e. the number of pixels to offset for text to appear inside the box).</summary>
        private const int TooltipBorderSize = 12;

        /// <summary>The padding between elements in the tooltip box.</summary>
        private const int Padding = 5;

        /// <summary>The pixel offset to apply to the tooltip box relative to the cursor position.</summary>
        private readonly Vector2 TooltipOffset = new Vector2(Game1.tileSize / 2);

        /// <summary>Metadata that isn't available from the game data directly.</summary>
        private DataModel Data;

        /// <summary>The cached toolbar instance.</summary>
        private readonly PerScreen<Toolbar> Toolbar = new PerScreen<Toolbar>();

        /// <summary>The cached toolbar slots.</summary>
        private readonly PerScreen<IList<ClickableComponent>> ToolbarSlots = new PerScreen<IList<ClickableComponent>>();


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            // init text
            I18n.Init(helper.Translation);

            // load data
            this.Data = helper.Data.ReadJsonFile<DataModel>("assets/data.json") ?? new DataModel();
            this.Data.ForceSellable ??= new HashSet<int>();

            // hook events
            helper.Events.Display.RenderedActiveMenu += this.OnRenderedActiveMenu;
            helper.Events.Display.RenderedHud += this.OnRenderedHud;
            helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Raised after the game state is updated (≈60 times per second).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            // cache the toolbar & slots
            if (e.IsOneSecond)
            {
                if (Context.IsPlayerFree)
                {
                    this.Toolbar.Value = Game1.onScreenMenus.OfType<Toolbar>().FirstOrDefault();
                    this.ToolbarSlots.Value = this.Toolbar.Value != null
                        ? this.Helper.Reflection.GetField<List<ClickableComponent>>(this.Toolbar.Value, "buttons").GetValue()
                        : null;
                }
                else
                {
                    this.Toolbar.Value = null;
                    this.ToolbarSlots.Value = null;
                }
            }
        }

        /// <summary>When a menu is open (<see cref="Game1.activeClickableMenu"/> isn't null), raised after that menu is drawn to the sprite batch but before it's rendered to the screen.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnRenderedActiveMenu(object sender, RenderedActiveMenuEventArgs e)
        {
            // get item
            Item item = this.GetItemFromMenu(Game1.activeClickableMenu);
            if (item == null)
                return;

            // draw tooltip
            this.DrawPriceTooltip(Game1.spriteBatch, Game1.smallFont, item);
        }

        /// <summary>Raised after drawing the HUD (item toolbar, clock, etc) to the sprite batch, but before it's rendered to the screen. The vanilla HUD may be hidden at this point (e.g. because a menu is open).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnRenderedHud(object sender, EventArgs e)
        {
            if (!Context.IsPlayerFree)
                return;

            // get item
            Item item = this.GetItemFromToolbar();
            if (item == null)
                return;

            // draw tooltip
            this.DrawPriceTooltip(Game1.spriteBatch, Game1.smallFont, item);
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

            return null;
        }

        /// <summary>Get the hovered item from the on-screen toolbar.</summary>
        private Item GetItemFromToolbar()
        {
            if (!Context.IsPlayerFree || this.Toolbar?.Value == null || this.ToolbarSlots == null || !Game1.displayHUD)
                return null;

            // find hovered slot
            int x = Game1.getMouseX();
            int y = Game1.getMouseY();
            ClickableComponent hoveredSlot = this.ToolbarSlots.Value.FirstOrDefault(slot => slot.containsPoint(x, y));
            if (hoveredSlot == null)
                return null;

            // get inventory index
            int index = this.ToolbarSlots.Value.IndexOf(hoveredSlot);
            if (index < 0 || index > Game1.player.Items.Count - 1)
                return null;

            // get hovered item
            return Game1.player.Items[index];
        }

        /// <summary>Draw a tooltip box which shows the unit and stack prices for an item.</summary>
        /// <param name="spriteBatch">The sprite batch to update.</param>
        /// <param name="font">The font with which to draw text.</param>
        /// <param name="item">The item whose price to display.</param>
        private void DrawPriceTooltip(SpriteBatch spriteBatch, SpriteFont font, Item item)
        {
            // get info
            int stack = item.Stack;
            int? price = this.GetSellPrice(item);
            if (price == null)
                return;

            // basic measurements
            const int borderSize = ModEntry.TooltipBorderSize;
            const int padding = ModEntry.Padding;
            int coinSize = this.CoinSourceRect.Width * Game1.pixelZoom;
            int lineHeight = (int)font.MeasureString("X").Y;
            Vector2 offsetFromCursor = this.TooltipOffset;
            bool showStack = stack > 1;

            // prepare text
            string unitLabel = I18n.Labels_SinglePrice() + ":";
            string unitPrice = price.ToString();
            string stackLabel = I18n.Labels_StackPrice() + ":";
            string stackPrice = (price * stack).ToString();

            // get dimensions
            Vector2 unitPriceSize = font.MeasureString(unitPrice);
            Vector2 stackPriceSize = font.MeasureString(stackPrice);
            Vector2 labelSize = font.MeasureString(unitLabel);
            if (showStack)
                labelSize = new Vector2(Math.Max(labelSize.X, font.MeasureString(stackLabel).X), labelSize.Y * 2);
            Vector2 innerSize = new Vector2(labelSize.X + padding + Math.Max(unitPriceSize.X, showStack ? stackPriceSize.X : 0) + padding + coinSize, labelSize.Y);
            Vector2 outerSize = innerSize + new Vector2((borderSize + padding) * 2);

            // get position
            float x = Game1.getMouseX() - offsetFromCursor.X - outerSize.X;
            float y = Game1.getMouseY() + offsetFromCursor.Y + borderSize;

            // adjust position to fit on screen
            Rectangle area = new Rectangle((int)x, (int)y, (int)outerSize.X, (int)outerSize.Y);
            if (area.Right > Game1.uiViewport.Width)
                x = Game1.uiViewport.Width - area.Width;
            if (area.Bottom > Game1.uiViewport.Height)
                y = Game1.uiViewport.Height - area.Height;

            // draw tooltip box
            IClickableMenu.drawTextureBox(spriteBatch, Game1.menuTexture, this.TooltipSourceRect, (int)x, (int)y, (int)outerSize.X, (int)outerSize.Y, Color.White);

            // draw coins
            spriteBatch.Draw(Game1.debrisSpriteSheet, new Vector2(x + outerSize.X - borderSize - padding - coinSize, y + borderSize + padding), this.CoinSourceRect, Color.White, 0.0f, Vector2.Zero, Game1.pixelZoom, SpriteEffects.None, 1f);
            if (showStack)
                spriteBatch.Draw(Game1.debrisSpriteSheet, new Vector2(x + outerSize.X - borderSize - padding - coinSize, y + borderSize + padding + lineHeight), this.CoinSourceRect, Color.White, 0.0f, Vector2.Zero, Game1.pixelZoom, SpriteEffects.None, 1f);

            // draw text
            Utility.drawTextWithShadow(spriteBatch, unitLabel, font, new Vector2(x + borderSize + padding, y + borderSize + padding), Game1.textColor);
            Utility.drawTextWithShadow(spriteBatch, unitPrice, font, new Vector2(x + outerSize.X - borderSize - padding - coinSize - padding - unitPriceSize.X, y + borderSize + padding), Game1.textColor);
            if (showStack)
            {
                Utility.drawTextWithShadow(spriteBatch, stackLabel, font, new Vector2(x + borderSize + padding, y + borderSize + padding + lineHeight), Game1.textColor);
                Utility.drawTextWithShadow(spriteBatch, stackPrice, font, new Vector2(x + outerSize.X - borderSize - padding - coinSize - padding - stackPriceSize.X, y + borderSize + padding + lineHeight), Game1.textColor);
            }
        }

        /// <summary>Get the sell price for an item.</summary>
        /// <param name="item">The item to check.</param>
        /// <returns>Returns the sell price, or <c>null</c> if it can't be sold.</returns>
        private int? GetSellPrice(Item item)
        {
            // skip unsellable item
            if (!this.CanBeSold(item))
                return null;

            // get price
            int price = Utility.getSellToStorePriceOfItem(item, countStack: false);
            return price >= 0
                ? price
                : null as int?;
        }

        /// <summary>Get whether an item can be sold.</summary>
        /// <param name="item">The item to check.</param>
        private bool CanBeSold(Item item)
        {
            return
                (item is SObject obj && obj.canBeShipped())
                || this.Data.ForceSellable.Contains(item.Category);
        }
    }
}
