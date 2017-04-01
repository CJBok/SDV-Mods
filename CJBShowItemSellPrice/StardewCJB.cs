using System;
using System.Collections.Generic;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;
using Microsoft.Xna.Framework.Input;

namespace CJBShowItemSellPrice
{
    public class StardewCJB : Mod
    {
        public static RenderTarget2D RTarg { get; set; }

        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper) {
            //GraphicsEvents.DrawTick += GraphicsEvents_OnPostRenderGuiEvent;
            GraphicsEvents.OnPostRenderGuiEvent += GraphicsEvents_OnPostRenderGuiEvent;
        }

        private void GraphicsEvents_OnPostRenderGuiEvent(object sender, EventArgs e) {
            if (Game1.activeClickableMenu != null) {
                Item item = null;

                if (Game1.activeClickableMenu is GameMenu) {
                    GameMenu menu = (GameMenu)Game1.activeClickableMenu;

                    if (menu.currentTab == 0) {
                        List<IClickableMenu> pages = (List<IClickableMenu>)typeof(GameMenu).GetField("pages", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(menu);
                        InventoryPage inv = (InventoryPage)pages[0];

                        item = (Item)typeof(InventoryPage).GetField("hoveredItem", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(inv);
                    }
                    if (menu.currentTab == 4) {
                        List<IClickableMenu> pages = (List<IClickableMenu>)typeof(GameMenu).GetField("pages", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(menu);
                        CraftingPage inv = (CraftingPage)pages[4];

                        item = (Item)typeof(CraftingPage).GetField("hoverItem", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(inv);
                    }
                }

                if (Game1.activeClickableMenu is MenuWithInventory) {
                    MenuWithInventory menu = (MenuWithInventory)Game1.activeClickableMenu;
                    InventoryMenu inv = menu.inventory;
                    item = menu.hoveredItem;
                }

                if (item == null)
                    return;

                if (item is StardewValley.Object) {
                    StardewValley.Object o = item as StardewValley.Object;
                    if (o.stack > 1)
                        drawHoverTextBox(Game1.smallFont, o.sellToStorePrice(), o.stack);
                    else
                        drawHoverTextBox(Game1.smallFont, o.sellToStorePrice());
                } else if (item != null) {
                    if (item.Stack > 1)
                        drawHoverTextBox(Game1.smallFont, (item.salePrice() / 2), item.Stack);
                    else
                        drawHoverTextBox(Game1.smallFont, item.salePrice());
                }
            }
        }

        private void drawHoverTextBox(SpriteFont font, int price, int stack = -1) {

            if (price < 1)
                return;

            string p = price.ToString();
            string ps = Environment.NewLine + (price * stack).ToString();

            string s1 = "Single: " + price;
            string s2 = "Stack: " + price * stack;

            string message = "" + s1;

            string message1 = "Single: ";

            if (stack > 1) {
                message += Environment.NewLine + s2;
                message1 += Environment.NewLine + "Stack: ";
            }

            
            Vector2 bounds = font.MeasureString(message);
            int width = (int)bounds.X + Game1.tileSize / 2 + 40;
            int height = (int)font.MeasureString(message).Y + Game1.tileSize / 3 + 5;

            int x = (int)(Mouse.GetState().X / Game1.options.zoomLevel) - Game1.tileSize / 2 - width;
            int y = (int)(Mouse.GetState().Y / Game1.options.zoomLevel) + Game1.tileSize / 2;

            if (x < 0) {
                x = 0;
            }
            if (y + height > Game1.graphics.GraphicsDevice.Viewport.Height) {
                y = Game1.graphics.GraphicsDevice.Viewport.Height - height;
            }

            /*if (!Game1.options.zoomLevel.Equals(1.0f)) {
                if (TheGame.Screen.RenderTargetUsage == RenderTargetUsage.DiscardContents) {
                    TheGame.Screen = new RenderTarget2D(Game1.graphics.GraphicsDevice, Math.Min(4096, (int)((double)TheGame.Window.ClientBounds.Width * (1.0 / (double)Game1.options.zoomLevel))),
                     Math.Min(4096, (int)((double)TheGame.Window.ClientBounds.Height * (1.0 / (double)Game1.options.zoomLevel))),
                        false, SurfaceFormat.Color, DepthFormat.Depth16, 1, RenderTargetUsage.PreserveContents);
                }
                TheGame.GraphicsDevice.SetRenderTarget(TheGame.Screen);
            }
            Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
            */
            IClickableMenu.drawTextureBox(Game1.spriteBatch, Game1.menuTexture, new Rectangle(0, 256, 60, 60), x, y, width, height, Color.White, 1f, true);
            Game1.spriteBatch.Draw(Game1.debrisSpriteSheet, new Vector2((float)(x + Game1.tileSize / 4) + font.MeasureString(message + "   ").X, (float)(y + Game1.tileSize / 4 + 10)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.debrisSpriteSheet, 8, 16, 16)), Color.White, 0.0f, new Vector2(8f, 8f), (float)Game1.pixelZoom, SpriteEffects.None, 1f);
            if (stack > 1)
                Game1.spriteBatch.Draw(Game1.debrisSpriteSheet, new Vector2((float)(x + Game1.tileSize / 4) + font.MeasureString(message + "   ").X, (float)(y + Game1.tileSize / 4 + 38)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.debrisSpriteSheet, 8, 16, 16)), Color.White, 0.0f, new Vector2(8f, 8f), (float)Game1.pixelZoom, SpriteEffects.None, 0.95f);
            Utility.drawTextWithShadow(Game1.spriteBatch, message1, font, new Vector2((float)(x + Game1.tileSize / 4), (float)(y + Game1.tileSize / 4)), Game1.textColor, 1f, -1f, -1, -1, 1F, 3);

            Utility.drawTextWithShadow(Game1.spriteBatch, p, font, new Vector2((float)(x + width + Game1.tileSize / 4 - 60 - font.MeasureString(p).X), (float)(y + Game1.tileSize / 4)), Game1.textColor, 1f, -1f, -1, -1, 1F, 3);
            if (stack > 1)
                Utility.drawTextWithShadow(Game1.spriteBatch, ps, font, new Vector2((float)(x + width + Game1.tileSize / 4 - 60 - font.MeasureString(ps).X), (float)(y + Game1.tileSize / 4)), Game1.textColor, 1f, -1f, -1, -1, 1F, 3);

            /*Game1.spriteBatch.End();

            if (!Game1.options.zoomLevel.Equals(1.0f)) {
                TheGame.GraphicsDevice.SetRenderTarget((RenderTarget2D)null);
                Game1.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone);
                Game1.spriteBatch.Draw((Texture2D)TheGame.Screen, Vector2.Zero, new Microsoft.Xna.Framework.Rectangle?(TheGame.Screen.Bounds), Color.White, 0.0f, Vector2.Zero, Game1.options.zoomLevel, SpriteEffects.None, 1f);
                Game1.spriteBatch.End();
            }*/
        }
    }
}