// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.ItemGrabMenu
// Assembly: Stardew Valley, Version=1.0.5912.41135, Culture=neutral, PublicKeyToken=null
// MVID: B585F4A7-F5D4-496B-8930-4705FA185302
// Assembly location: K:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardewValley.Menus;

namespace CJBEndlessInventory
{
    internal class ItemMenu : ItemMenuWithInventory
    {
        /*********
        ** Properties
        *********/
        private ItemInventoryMenu ItemsToGrabMenu;
        private TemporaryAnimatedSprite Poof;

        private ClickableComponent Title;
        private ClickableTextureComponent UpArrow;
        private ClickableTextureComponent DownArrow;


        /*********
        ** Accessors
        *********/
        public delegate void BehaviorOnItemSelect(Item item, Farmer who);

        public bool ShowReceivingMenu = true;
        public bool DrawBackground = false;
        public bool ReverseGrab = true;
        public bool DestroyItemOnClick = true;
        public bool CanExitOnKey = true;
        public bool PlayRightClickSound;
        public bool AllowRightClick;
        public bool CanClose = false;
        public Item HoverItem;
        public static List<Item> ItemList;

        public ItemMenu.BehaviorOnItemSelect BehaviorFunction;
        public ItemMenu.BehaviorOnItemSelect BehaviorOnItemGrab;


        /*********
        ** Public methods
        *********/
        public ItemMenu(List<Item> inventory, ItemMenu.BehaviorOnItemSelect behaviorOnItemSelectFunction, ItemMenu.BehaviorOnItemSelect behaviorOnItemGrab = null)
          : base(null, true, true, 0, 0)
        {
            //this.inventoryItems = inventory;
            //base.MovePosition(0, Game1.viewport.Height - (this.yPositionOnScreen + this.height + IClickableMenu.spaceToClearTopBorder));

            this.BehaviorFunction = new BehaviorOnItemSelect(behaviorOnItemSelectFunction);
            this.BehaviorOnItemGrab = new BehaviorOnItemSelect(behaviorOnItemGrab);

            this.Title = new ClickableComponent(new Rectangle(this.xPositionOnScreen + (width / 2), this.yPositionOnScreen - Game1.tileSize * 2, Game1.tileSize * 4, Game1.tileSize), "CJB Endless Inventory");
            this.UpArrow = new ClickableTextureComponent("up-arrow", new Rectangle(this.xPositionOnScreen + width - Game1.tileSize / 2, this.yPositionOnScreen - Game1.tileSize, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), "", "", Game1.mouseCursors, new Rectangle(421, 459, 11, 12), (float)Game1.pixelZoom);
            this.DownArrow = new ClickableTextureComponent("down-arrow", new Rectangle(this.xPositionOnScreen + width - Game1.tileSize / 2, this.yPositionOnScreen + height / 2 - Game1.tileSize * 2, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), "", "", Game1.mouseCursors, new Rectangle(421, 472, 11, 12), (float)Game1.pixelZoom);


            this.AllowRightClick = true;
            this.Inventory.ShowGrayedOutSlots = false;

            this.ItemsToGrabMenu = new ItemInventoryMenu(this.xPositionOnScreen + Game1.tileSize / 2, this.yPositionOnScreen, false, inventory, null, 99999, 99999 / 12, 0, 0, true);
        }

        public override void receiveRightClick(int x, int y, bool playSound = true)
        {
            if (!this.AllowRightClick)
            {
                return;
            }
            base.receiveRightClick(x, y, playSound && this.PlayRightClickSound);
            if (this.HeldItem == null && this.ShowReceivingMenu)
            {
                this.HeldItem = this.ItemsToGrabMenu.RightClick(x, y, this.HeldItem, false);
                if (this.HeldItem != null && this.BehaviorOnItemGrab != null)
                {
                    this.BehaviorOnItemGrab(this.HeldItem, Game1.player);
                }
                if (this.HeldItem is StardewValley.Object && (this.HeldItem as StardewValley.Object).parentSheetIndex == 326)
                {
                    this.HeldItem = null;
                    Game1.player.canUnderstandDwarves = true;
                    this.Poof = new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 320, 64, 64), 50f, 8, 0, new Vector2((float)(x - x % Game1.tileSize + Game1.tileSize / 4), (float)(y - y % Game1.tileSize + Game1.tileSize / 4)), false, false);
                    Game1.playSound("fireball");
                    return;
                }
                if (this.HeldItem is StardewValley.Object recipe && recipe.isRecipe)
                {
                    string key = recipe.Name.Substring(0, recipe.Name.IndexOf("Recipe") - 1);
                    try
                    {
                        if (recipe.category == -7)
                            Game1.player.cookingRecipes.Add(key, 0);
                        else
                            Game1.player.craftingRecipes.Add(key, 0);
                        this.Poof = new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 320, 64, 64), 50f, 8, 0, new Vector2((float)(x - x % Game1.tileSize + Game1.tileSize / 4), (float)(y - y % Game1.tileSize + Game1.tileSize / 4)), false, false);
                        Game1.playSound("newRecipe");
                    }
                    catch (Exception)
                    {
                    }
                    this.HeldItem = null;
                    return;
                }
                if (Game1.player.addItemToInventoryBool(this.HeldItem, false))
                {
                    CJB.RemoveLastHudMessage();
                    this.HeldItem = null;
                    Game1.playSound("coin");
                    return;
                }
            }
            else if (this.ReverseGrab)
            {
                this.BehaviorFunction(this.HeldItem, Game1.player);
                if (this.DestroyItemOnClick)
                {
                    this.HeldItem = null;
                }
            }
        }

        public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
        {
            ItemMenu.Open();
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            base.receiveLeftClick(x, y, !this.DestroyItemOnClick);

            if (this.HeldItem == null)
            {
                if (this.UpArrow.bounds.Contains(x, y))
                {
                    if (this.ItemsToGrabMenu != null)
                        this.ItemsToGrabMenu.receiveScrollWheelAction(1);
                }

                if (this.DownArrow.bounds.Contains(x, y))
                {
                    if (this.ItemsToGrabMenu != null)
                        this.ItemsToGrabMenu.receiveScrollWheelAction(-1);
                }
            }
            if (this.HeldItem == null && this.ShowReceivingMenu)
            {
                this.HeldItem = this.ItemsToGrabMenu.LeftClick(x, y, this.HeldItem, false);
                if (this.HeldItem != null && this.BehaviorOnItemGrab != null)
                {
                    this.BehaviorOnItemGrab(this.HeldItem, Game1.player);
                }
                if (this.HeldItem is StardewValley.Object obj && obj.parentSheetIndex == 326)
                {
                    this.HeldItem = null;
                    Game1.player.canUnderstandDwarves = true;
                    this.Poof = new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 320, 64, 64), 50f, 8, 0, new Vector2((float)(x - x % Game1.tileSize + Game1.tileSize / 4), (float)(y - y % Game1.tileSize + Game1.tileSize / 4)), false, false);
                    Game1.playSound("fireball");
                }
                else if (this.HeldItem is StardewValley.Object && (this.HeldItem as StardewValley.Object).parentSheetIndex == 102)
                {
                    this.HeldItem = null;
                    Game1.player.foundArtifact(102, 1);
                    this.Poof = new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 320, 64, 64), 50f, 8, 0, new Vector2((float)(x - x % Game1.tileSize + Game1.tileSize / 4), (float)(y - y % Game1.tileSize + Game1.tileSize / 4)), false, false);
                    Game1.playSound("fireball");
                }
                else if (this.HeldItem is StardewValley.Object recipe && recipe.isRecipe)
                {
                    string key = recipe.Name.Substring(0, recipe.Name.IndexOf("Recipe") - 1);
                    try
                    {
                        if (recipe.category == -7)
                            Game1.player.cookingRecipes.Add(key, 0);
                        else
                            Game1.player.craftingRecipes.Add(key, 0);
                        this.Poof = new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 320, 64, 64), 50f, 8, 0, new Vector2((float)(x - x % Game1.tileSize + Game1.tileSize / 4), (float)(y - y % Game1.tileSize + Game1.tileSize / 4)), false, false);
                        Game1.playSound("newRecipe");
                    }
                    catch (Exception)
                    {
                    }
                    this.HeldItem = null;
                }
                else if (Game1.player.addItemToInventoryBool(this.HeldItem, false))
                {
                    CJB.RemoveLastHudMessage();
                    this.HeldItem = null;
                    Game1.playSound("coin");
                }
            }
            else if ((this.ReverseGrab || this.BehaviorFunction != null) && this.isWithinBounds(x, y))
            {
                this.BehaviorFunction(this.HeldItem, Game1.player);
                if (this.DestroyItemOnClick)
                {
                    this.HeldItem = null;
                    return;
                }
            }
            if (this.HeldItem != null && !this.isWithinBounds(x, y) && this.HeldItem.canBeTrashed())
            {
                Game1.playSound("throwDownITem");
                Game1.createItemDebris(this.HeldItem, Game1.player.getStandingPosition(), Game1.player.FacingDirection);
                this.HeldItem = null;
            }
        }

        public bool AreAllItemsTaken()
        {
            for (int index = 0; index < Enumerable.Count<Item>((IEnumerable<Item>)this.ItemsToGrabMenu.ActualInventory); ++index)
            {
                if (this.ItemsToGrabMenu.ActualInventory[index] != null)
                    return false;
            }
            return true;
        }

        public override void receiveKeyPress(Keys key)
        {
            if ((this.CanExitOnKey || this.AreAllItemsTaken()) && ((Game1.options.doesInputListContain(Game1.options.menuButton, key) || key.ToString() == CJBEndlessInventory.Settings.MenuButton) && this.readyToClose() && this.CanClose))
            {
                this.exitThisMenu(true);
                if (Game1.currentLocation.currentEvent != null)
                    ++Game1.currentLocation.currentEvent.CurrentCommand;

                return;
            }

            if (Game1.options.doesInputListContain(Game1.options.menuButton, key) && this.HeldItem != null)
                Game1.setMousePosition(this.TrashCan.bounds.Center);
            if (key != Keys.Delete || this.HeldItem == null || !this.HeldItem.canBeTrashed())
                return;
            if (this.HeldItem is StardewValley.Object obj && Game1.player.specialItems.Contains(obj.parentSheetIndex))
                Game1.player.specialItems.Remove(obj.parentSheetIndex);
            this.HeldItem = null;
            Game1.playSound("trashcan");
        }

        public override void update(GameTime time)
        {
            if (Keyboard.GetState().GetPressedKeys().Length == 0)
            {
                this.CanClose = true;
            }
            base.update(time);
            if (this.Poof == null || !this.Poof.update(time))
                return;
            this.Poof = (TemporaryAnimatedSprite)null;
        }

        public override void performHoverAction(int x, int y)
        {
            if (this.ItemsToGrabMenu.isWithinBounds(x, y) && this.ShowReceivingMenu)
            {
                this.HoveredItem = this.ItemsToGrabMenu.Hover(x, y, this.HeldItem);
            }
            else
                base.performHoverAction(x, y);
        }

        public override void receiveScrollWheelAction(int direction)
        {
            if (GameMenu.forcePreventClose)
                return;

            if (this.ItemsToGrabMenu != null)
                this.ItemsToGrabMenu.receiveScrollWheelAction(direction);
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            if (!Game1.options.showMenuBackground)
            {
                spriteBatch.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.4f);
            }
            this.Draw(spriteBatch, false, false);
            if (this.ShowReceivingMenu)
            {
                CJB.DrawTextBox(this.Title.bounds.X, this.Title.bounds.Y, Game1.borderFont, this.Title.name, true, 1, 1.0f);
                Game1.drawDialogueBox(this.ItemsToGrabMenu.xPositionOnScreen - IClickableMenu.borderWidth - IClickableMenu.spaceToClearSideBorder, this.ItemsToGrabMenu.yPositionOnScreen - IClickableMenu.borderWidth - IClickableMenu.spaceToClearTopBorder, this.ItemsToGrabMenu.width + IClickableMenu.borderWidth * 2 + IClickableMenu.spaceToClearSideBorder * 2, this.ItemsToGrabMenu.height + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth * 2, false, true, (string)null, false);
                this.ItemsToGrabMenu.draw(spriteBatch);
                this.UpArrow.draw(spriteBatch);
                this.DownArrow.draw(spriteBatch);
            }

            if (this.Poof != null)
                this.Poof.draw(spriteBatch, true, 0, 0);
            if (this.HoverText != null && (this.HoveredItem == null || this.HoveredItem == null || this.ItemsToGrabMenu == null))
                IClickableMenu.drawHoverText(spriteBatch, this.HoverText, Game1.smallFont, 0, 0, -1, (string)null, -1, (string[])null, (Item)null, 0, -1, -1, -1, -1, 1f, (CraftingRecipe)null);
            if (this.HoveredItem != null)
                IClickableMenu.drawToolTip(spriteBatch, this.HoveredItem.getDescription(), this.HoveredItem.Name, this.HoveredItem, this.HeldItem != null, -1, 0, -1, -1, (CraftingRecipe)null, -1);
            else if (this.HoveredItem != null && this.ItemsToGrabMenu != null)
                IClickableMenu.drawToolTip(spriteBatch, this.ItemsToGrabMenu.DescriptionText, this.ItemsToGrabMenu.DescriptionTitle, this.HoveredItem, this.HeldItem != null, -1, 0, -1, -1, (CraftingRecipe)null, -1);
            if (this.HeldItem != null)
                this.HeldItem.drawInMenu(spriteBatch, new Vector2((float)(Game1.getOldMouseX() + 8), (float)(Game1.getOldMouseY() + 8)), 1f);

            if (this.HoveredItem is StardewValley.Object obj)
            {
                if (obj.stack > 1)
                    this.DrawHoverTextBox(spriteBatch, Game1.smallFont, obj.sellToStorePrice(), obj.stack);
                else
                    this.DrawHoverTextBox(spriteBatch, Game1.smallFont, obj.sellToStorePrice());
            }
            else if (this.HoveredItem != null)
            {
                if (this.HoveredItem.Stack > 1)
                    this.DrawHoverTextBox(spriteBatch, Game1.smallFont, (this.HoveredItem.salePrice() / 2), this.HoveredItem.Stack);
                else
                    this.DrawHoverTextBox(spriteBatch, Game1.smallFont, this.HoveredItem.salePrice());
            }
            if (!Game1.options.hardwareCursor)
                spriteBatch.Draw(Game1.mouseCursors, new Vector2((float)Game1.getOldMouseX(), (float)Game1.getOldMouseY()), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 0, 16, 16)), Color.White, 0.0f, Vector2.Zero, (float)Game1.pixelZoom + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 1f);
        }

        public static void Open()
        {
            Storage storage = new Storage(0, CJBEndlessInventory.StorageItems.PlayerItems, Vector2.Zero, false);
            Game1.activeClickableMenu = new ItemMenu(CJBEndlessInventory.StorageItems.PlayerItems, storage.GrabItemFromInventory, storage.GrabItemFromChest);
        }


        /*********
        ** Private methods
        *********/
        private void DrawHoverTextBox(SpriteBatch spriteBatch, SpriteFont font, int price, int stack = -1)
        {
            if (price < 1)
                return;

            string priceString = price.ToString();
            string stackPriceString = Environment.NewLine + (price * stack).ToString();

            string message = "Single: " + price;
            string message1 = "Single: ";

            if (stack > 1)
            {
                message += Environment.NewLine + "Stack: " + price * stack;
                message1 += Environment.NewLine + "Stack: ";
            }

            //spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
            Vector2 bounds = font.MeasureString(message);
            int width = (int)bounds.X + Game1.tileSize / 2 + 40;
            int height = (int)font.MeasureString(message).Y + Game1.tileSize / 3 + 5;
            int x = Game1.getOldMouseX() - Game1.tileSize / 2 - width;
            int y = Game1.getOldMouseY() + Game1.tileSize / 2;

            if (x < 0)
            {
                x = 0;
            }
            if (y + height > Game1.graphics.GraphicsDevice.Viewport.Height)
            {
                y = Game1.graphics.GraphicsDevice.Viewport.Height - height;
            }
            IClickableMenu.drawTextureBox(spriteBatch, Game1.menuTexture, new Rectangle(0, 256, 60, 60), x, y, width, height, Color.White, 1f, true);
            spriteBatch.Draw(Game1.debrisSpriteSheet, new Vector2((float)(x + Game1.tileSize / 4) + font.MeasureString(message + "   ").X, (float)(y + Game1.tileSize / 4 + 10)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.debrisSpriteSheet, 8, 16, 16)), Color.White, 0.0f, new Vector2(8f, 8f), (float)Game1.pixelZoom, SpriteEffects.None, 1f);
            if (stack > 1)
                spriteBatch.Draw(Game1.debrisSpriteSheet, new Vector2((float)(x + Game1.tileSize / 4) + font.MeasureString(message + "   ").X, (float)(y + Game1.tileSize / 4 + 38)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.debrisSpriteSheet, 8, 16, 16)), Color.White, 0.0f, new Vector2(8f, 8f), (float)Game1.pixelZoom, SpriteEffects.None, 0.95f);
            Utility.drawTextWithShadow(spriteBatch, message1, font, new Vector2((float)(x + Game1.tileSize / 4), (float)(y + Game1.tileSize / 4)), Game1.textColor, 1f, -1f, -1, -1, 1F, 3);

            Utility.drawTextWithShadow(spriteBatch, priceString, font, new Vector2((float)(x + width + Game1.tileSize / 4 - 60 - font.MeasureString(priceString).X), (float)(y + Game1.tileSize / 4)), Game1.textColor, 1f, -1f, -1, -1, 1F, 3);
            if (stack > 1)
                Utility.drawTextWithShadow(spriteBatch, stackPriceString, font, new Vector2((float)(x + width + Game1.tileSize / 4 - 60 - font.MeasureString(stackPriceString).X), (float)(y + Game1.tileSize / 4)), Game1.textColor, 1f, -1f, -1, -1, 1F, 3);
            //spriteBatch.End();
        }
    }
}
