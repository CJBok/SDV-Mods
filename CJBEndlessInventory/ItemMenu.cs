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
    public class ItemMenu : ItemMenuWithInventory
    {

        public delegate void behaviorOnItemSelect(Item item, Farmer who);

        public bool showReceivingMenu = true;
        public bool drawBG = false;
        private ItemInventoryMenu ItemsToGrabMenu;
        private TemporaryAnimatedSprite poof;
        public bool reverseGrab = true;
        public bool destroyItemOnClick = true;
        public bool canExitOnKey = true;
        public bool playRightClickSound;
        public bool allowRightClick;
        public bool canClose = false;
        public Item hoverItem;
        public static List<Item> itemList;

        private ClickableComponent title;

        private ClickableTextureComponent upArrow;
        private ClickableTextureComponent downArrow;

        public ItemMenu.behaviorOnItemSelect behaviorFunction;
        public ItemMenu.behaviorOnItemSelect behaviorOnItemGrab;

        public ItemMenu(List<Item> inventory, ItemMenu.behaviorOnItemSelect behaviorOnItemSelectFunction, ItemMenu.behaviorOnItemSelect behaviorOnItemGrab = null)
          : base(null, true, true, 0, 0)
        {
            //this.inventoryItems = inventory;
            //base.movePosition(0, Game1.viewport.Height - (this.yPositionOnScreen + this.height + IClickableMenu.spaceToClearTopBorder));

            this.behaviorFunction = new behaviorOnItemSelect(behaviorOnItemSelectFunction);
            this.behaviorOnItemGrab = new behaviorOnItemSelect(behaviorOnItemGrab);

            title = new ClickableComponent(new Rectangle(this.xPositionOnScreen + (width / 2), this.yPositionOnScreen - Game1.tileSize * 2, Game1.tileSize * 4, Game1.tileSize), "CJB Endless Inventory");
            this.upArrow = new ClickableTextureComponent("up-arrow", new Rectangle(this.xPositionOnScreen + width - Game1.tileSize / 2, this.yPositionOnScreen - Game1.tileSize, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), "", "", Game1.mouseCursors, new Rectangle(421, 459, 11, 12), (float)Game1.pixelZoom);
            this.downArrow = new ClickableTextureComponent("down-arrow", new Rectangle(this.xPositionOnScreen + width - Game1.tileSize / 2, this.yPositionOnScreen + height / 2 - Game1.tileSize * 2, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), "", "", Game1.mouseCursors, new Rectangle(421, 472, 11, 12), (float)Game1.pixelZoom);


            this.allowRightClick = true;
            this.inventory.showGrayedOutSlots = false;

            this.ItemsToGrabMenu = new ItemInventoryMenu(this.xPositionOnScreen + Game1.tileSize / 2, this.yPositionOnScreen, false, inventory, null, 99999, 99999 / 12, 0, 0, true);
        }

        public override void receiveRightClick(int x, int y, bool playSound = true)
        {
            if (!this.allowRightClick)
            {
                return;
            }
            base.receiveRightClick(x, y, playSound && this.playRightClickSound);
            if (this.heldItem == null && this.showReceivingMenu)
            {
                this.heldItem = this.ItemsToGrabMenu.rightClick(x, y, this.heldItem, false);
                if (this.heldItem != null && this.behaviorOnItemGrab != null)
                {
                    this.behaviorOnItemGrab(this.heldItem, Game1.player);
                }
                if (this.heldItem is StardewValley.Object && (this.heldItem as StardewValley.Object).parentSheetIndex == 326)
                {
                    this.heldItem = null;
                    Game1.player.canUnderstandDwarves = true;
                    this.poof = new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 320, 64, 64), 50f, 8, 0, new Vector2((float)(x - x % Game1.tileSize + Game1.tileSize / 4), (float)(y - y % Game1.tileSize + Game1.tileSize / 4)), false, false);
                    Game1.playSound("fireball");
                    return;
                }
                if (this.heldItem is StardewValley.Object && (this.heldItem as StardewValley.Object).isRecipe)
                {
                    string key = this.heldItem.Name.Substring(0, this.heldItem.Name.IndexOf("Recipe") - 1);
                    try
                    {
                        if ((this.heldItem as StardewValley.Object).category == -7)
                        {
                            Game1.player.cookingRecipes.Add(key, 0);
                        }
                        else
                        {
                            Game1.player.craftingRecipes.Add(key, 0);
                        }
                        this.poof = new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 320, 64, 64), 50f, 8, 0, new Vector2((float)(x - x % Game1.tileSize + Game1.tileSize / 4), (float)(y - y % Game1.tileSize + Game1.tileSize / 4)), false, false);
                        Game1.playSound("newRecipe");
                    }
                    catch (Exception)
                    {
                    }
                    this.heldItem = null;
                    return;
                }
                if (Game1.player.addItemToInventoryBool(this.heldItem, false))
                {
                    CJB.removeLastHudMessage();
                    this.heldItem = null;
                    Game1.playSound("coin");
                    return;
                }
            }
            else if (this.reverseGrab)
            {
                this.behaviorFunction(this.heldItem, Game1.player);
                if (this.destroyItemOnClick)
                {
                    this.heldItem = null;
                }
            }
        }

        public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
        {
            Open();
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            base.receiveLeftClick(x, y, !this.destroyItemOnClick);

            if (this.heldItem == null)
            {
                if (this.upArrow.bounds.Contains(x, y))
                {
                    if (this.ItemsToGrabMenu != null)
                        this.ItemsToGrabMenu.receiveScrollWheelAction(1);
                }

                if (this.downArrow.bounds.Contains(x, y))
                {
                    if (this.ItemsToGrabMenu != null)
                        this.ItemsToGrabMenu.receiveScrollWheelAction(-1);
                }
            }
            if (this.heldItem == null && this.showReceivingMenu)
            {
                this.heldItem = this.ItemsToGrabMenu.leftClick(x, y, this.heldItem, false);
                if (this.heldItem != null && this.behaviorOnItemGrab != null)
                {
                    this.behaviorOnItemGrab(this.heldItem, Game1.player);
                }
                if (this.heldItem is StardewValley.Object && (this.heldItem as StardewValley.Object).parentSheetIndex == 326)
                {
                    this.heldItem = null;
                    Game1.player.canUnderstandDwarves = true;
                    this.poof = new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 320, 64, 64), 50f, 8, 0, new Vector2((float)(x - x % Game1.tileSize + Game1.tileSize / 4), (float)(y - y % Game1.tileSize + Game1.tileSize / 4)), false, false);
                    Game1.playSound("fireball");
                }
                else if (this.heldItem is StardewValley.Object && (this.heldItem as StardewValley.Object).parentSheetIndex == 102)
                {
                    this.heldItem = null;
                    Game1.player.foundArtifact(102, 1);
                    this.poof = new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 320, 64, 64), 50f, 8, 0, new Vector2((float)(x - x % Game1.tileSize + Game1.tileSize / 4), (float)(y - y % Game1.tileSize + Game1.tileSize / 4)), false, false);
                    Game1.playSound("fireball");
                }
                else if (this.heldItem is StardewValley.Object && (this.heldItem as StardewValley.Object).isRecipe)
                {
                    string key = this.heldItem.Name.Substring(0, this.heldItem.Name.IndexOf("Recipe") - 1);
                    try
                    {
                        if ((this.heldItem as StardewValley.Object).category == -7)
                        {
                            Game1.player.cookingRecipes.Add(key, 0);
                        }
                        else
                        {
                            Game1.player.craftingRecipes.Add(key, 0);
                        }
                        this.poof = new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 320, 64, 64), 50f, 8, 0, new Vector2((float)(x - x % Game1.tileSize + Game1.tileSize / 4), (float)(y - y % Game1.tileSize + Game1.tileSize / 4)), false, false);
                        Game1.playSound("newRecipe");
                    }
                    catch (Exception)
                    {
                    }
                    this.heldItem = null;
                }
                else if (Game1.player.addItemToInventoryBool(this.heldItem, false))
                {
                    CJB.removeLastHudMessage();
                    this.heldItem = null;
                    Game1.playSound("coin");
                }
            }
            else if ((this.reverseGrab || this.behaviorFunction != null) && this.isWithinBounds(x, y))
            {
                this.behaviorFunction(this.heldItem, Game1.player);
                if (this.destroyItemOnClick)
                {
                    this.heldItem = null;
                    return;
                }
            }
            if (this.heldItem != null && !this.isWithinBounds(x, y) && this.heldItem.canBeTrashed())
            {
                Game1.playSound("throwDownITem");
                Game1.createItemDebris(this.heldItem, Game1.player.getStandingPosition(), Game1.player.FacingDirection);
                this.heldItem = null;
            }
        }

        public bool areAllItemsTaken()
        {
            for (int index = 0; index < Enumerable.Count<Item>((IEnumerable<Item>)this.ItemsToGrabMenu.actualInventory); ++index)
            {
                if (this.ItemsToGrabMenu.actualInventory[index] != null)
                    return false;
            }
            return true;
        }

        public override void receiveKeyPress(Keys key)
        {
            if ((this.canExitOnKey || this.areAllItemsTaken()) && ((Game1.options.doesInputListContain(Game1.options.menuButton, key) || key.ToString() == CJBEndlessInventory.settings.menuButton) && this.readyToClose() && canClose))
            {
                this.exitThisMenu(true);
                if (Game1.currentLocation.currentEvent != null)
                    ++Game1.currentLocation.currentEvent.CurrentCommand;

                return;
            }

            if (Game1.options.doesInputListContain(Game1.options.menuButton, key) && this.heldItem != null)
                Game1.setMousePosition(this.trashCan.bounds.Center);
            if (key != Keys.Delete || this.heldItem == null || !this.heldItem.canBeTrashed())
                return;
            if (this.heldItem is StardewValley.Object && Game1.player.specialItems.Contains((this.heldItem as StardewValley.Object).parentSheetIndex))
                Game1.player.specialItems.Remove((this.heldItem as StardewValley.Object).parentSheetIndex);
            this.heldItem = (Item)null;
            Game1.playSound("trashcan");
        }

        public override void update(GameTime time)
        {
            if (Keyboard.GetState().GetPressedKeys().Length == 0)
            {
                canClose = true;
            }
            base.update(time);
            if (this.poof == null || !this.poof.update(time))
                return;
            this.poof = (TemporaryAnimatedSprite)null;
        }

        public override void performHoverAction(int x, int y)
        {
            if (this.ItemsToGrabMenu.isWithinBounds(x, y) && this.showReceivingMenu)
            {
                this.hoveredItem = this.ItemsToGrabMenu.hover(x, y, this.heldItem);
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

        public override void draw(SpriteBatch b)
        {
            if (!Game1.options.showMenuBackground)
            {
                b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.4f);
            }
            this.draw(b, false, false);
            if (this.showReceivingMenu)
            {
                CJB.drawTextBox(title.bounds.X, title.bounds.Y, Game1.borderFont, title.name, true, 1, 1.0f);
                Game1.drawDialogueBox(this.ItemsToGrabMenu.xPositionOnScreen - IClickableMenu.borderWidth - IClickableMenu.spaceToClearSideBorder, this.ItemsToGrabMenu.yPositionOnScreen - IClickableMenu.borderWidth - IClickableMenu.spaceToClearTopBorder, this.ItemsToGrabMenu.width + IClickableMenu.borderWidth * 2 + IClickableMenu.spaceToClearSideBorder * 2, this.ItemsToGrabMenu.height + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth * 2, false, true, (string)null, false);
                this.ItemsToGrabMenu.draw(b);
                this.upArrow.draw(b);
                this.downArrow.draw(b);
            }

            if (this.poof != null)
                this.poof.draw(b, true, 0, 0);
            if (this.hoverText != null && (this.hoveredItem == null || this.hoveredItem == null || this.ItemsToGrabMenu == null))
                IClickableMenu.drawHoverText(b, this.hoverText, Game1.smallFont, 0, 0, -1, (string)null, -1, (string[])null, (Item)null, 0, -1, -1, -1, -1, 1f, (CraftingRecipe)null);
            if (this.hoveredItem != null)
                IClickableMenu.drawToolTip(b, this.hoveredItem.getDescription(), this.hoveredItem.Name, this.hoveredItem, this.heldItem != null, -1, 0, -1, -1, (CraftingRecipe)null, -1);
            else if (this.hoveredItem != null && this.ItemsToGrabMenu != null)
                IClickableMenu.drawToolTip(b, this.ItemsToGrabMenu.descriptionText, this.ItemsToGrabMenu.descriptionTitle, this.hoveredItem, this.heldItem != null, -1, 0, -1, -1, (CraftingRecipe)null, -1);
            if (this.heldItem != null)
                this.heldItem.drawInMenu(b, new Vector2((float)(Game1.getOldMouseX() + 8), (float)(Game1.getOldMouseY() + 8)), 1f);

            if (hoveredItem is StardewValley.Object)
            {
                StardewValley.Object o = hoveredItem as StardewValley.Object;
                if (o.stack > 1)
                    drawHoverTextBox(b, Game1.smallFont, o.sellToStorePrice(), o.stack);
                else
                    drawHoverTextBox(b, Game1.smallFont, o.sellToStorePrice());
            }
            else if (hoveredItem != null)
            {
                if (hoveredItem.Stack > 1)
                    drawHoverTextBox(b, Game1.smallFont, (hoveredItem.salePrice() / 2), hoveredItem.Stack);
                else
                    drawHoverTextBox(b, Game1.smallFont, hoveredItem.salePrice());
            }
            if (!Game1.options.hardwareCursor)
                b.Draw(Game1.mouseCursors, new Vector2((float)Game1.getOldMouseX(), (float)Game1.getOldMouseY()), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 0, 16, 16)), Color.White, 0.0f, Vector2.Zero, (float)Game1.pixelZoom + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 1f);
        }

        public static void Open()
        {
            Storage storage = new Storage(0, CJBEndlessInventory.storageItems.playerItems, Vector2.Zero, false);
            Game1.activeClickableMenu = new ItemMenu(CJBEndlessInventory.storageItems.playerItems, storage.grabItemFromInventory, storage.grabItemFromChest);
        }

        private void drawHoverTextBox(SpriteBatch b, SpriteFont font, int price, int stack = -1)
        {

            if (price < 1)
                return;

            string p = price.ToString();
            string ps = Environment.NewLine + (price * stack).ToString();

            string s1 = "Single: " + price;
            string s2 = "Stack: " + price * stack;

            string message = "" + s1;

            string message1 = "Single: ";

            if (stack > 1)
            {
                message += Environment.NewLine + s2;
                message1 += Environment.NewLine + "Stack: ";
            }

            //b.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
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
            IClickableMenu.drawTextureBox(b, Game1.menuTexture, new Rectangle(0, 256, 60, 60), x, y, width, height, Color.White, 1f, true);
            b.Draw(Game1.debrisSpriteSheet, new Vector2((float)(x + Game1.tileSize / 4) + font.MeasureString(message + "   ").X, (float)(y + Game1.tileSize / 4 + 10)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.debrisSpriteSheet, 8, 16, 16)), Color.White, 0.0f, new Vector2(8f, 8f), (float)Game1.pixelZoom, SpriteEffects.None, 1f);
            if (stack > 1)
                b.Draw(Game1.debrisSpriteSheet, new Vector2((float)(x + Game1.tileSize / 4) + font.MeasureString(message + "   ").X, (float)(y + Game1.tileSize / 4 + 38)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.debrisSpriteSheet, 8, 16, 16)), Color.White, 0.0f, new Vector2(8f, 8f), (float)Game1.pixelZoom, SpriteEffects.None, 0.95f);
            Utility.drawTextWithShadow(b, message1, font, new Vector2((float)(x + Game1.tileSize / 4), (float)(y + Game1.tileSize / 4)), Game1.textColor, 1f, -1f, -1, -1, 1F, 3);

            Utility.drawTextWithShadow(b, p, font, new Vector2((float)(x + width + Game1.tileSize / 4 - 60 - font.MeasureString(p).X), (float)(y + Game1.tileSize / 4)), Game1.textColor, 1f, -1f, -1, -1, 1F, 3);
            if (stack > 1)
                Utility.drawTextWithShadow(b, ps, font, new Vector2((float)(x + width + Game1.tileSize / 4 - 60 - font.MeasureString(ps).X), (float)(y + Game1.tileSize / 4)), Game1.textColor, 1f, -1f, -1, -1, 1F, 3);
            //b.End();
        }
    }
}
