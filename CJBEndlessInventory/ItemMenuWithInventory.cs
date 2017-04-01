// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.MenuWithInventory
// Assembly: Stardew Valley, Version=1.0.5912.41135, Culture=neutral, PublicKeyToken=null
// MVID: B585F4A7-F5D4-496B-8930-4705FA185302
// Assembly location: K:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace CJBEndlessInventory
{
    public class ItemMenuWithInventory : IClickableMenu
    {
        /*********
        ** Accessors
        *********/
        public string DescriptionText = "";
        public string HoverText = "";
        public string DescriptionTitle = "";
        public ItemInventoryMenu Inventory;
        public Item HeldItem;
        public Item HoveredItem;
        public int WiggleWordsTimer;
        public ClickableTextureComponent OkButton;
        public ClickableTextureComponent TrashCan;
        public float TrashCanLidRotation;


        /*********
        ** Public methods
        *********/
        public ItemMenuWithInventory(InventoryMenu.highlightThisItem highlighterMethod = null, bool okButton = false, bool trashCan = false, int inventoryXOffset = 0, int inventoryYOffset = 0)
          : base(Game1.viewport.Width / 2 - (800 + IClickableMenu.borderWidth * 2) / 2, Game1.viewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2, 800 + IClickableMenu.borderWidth * 2, 600 + IClickableMenu.borderWidth * 2, false)
        {
            if (this.yPositionOnScreen < IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder)
                this.yPositionOnScreen = IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder;
            if (this.xPositionOnScreen < 0)
                this.xPositionOnScreen = 0;
            int yPosition = this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth + Game1.tileSize * 3 - Game1.tileSize / 4 + inventoryYOffset;
            this.Inventory = new ItemInventoryMenu(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth / 2 + inventoryXOffset, yPosition, false, (List<Item>)null, highlighterMethod, -1, 3, 0, 0, true);
            if (okButton)
                this.OkButton = new ClickableTextureComponent("ok-button", new Rectangle(this.xPositionOnScreen + this.width + 4, this.yPositionOnScreen + this.height - Game1.tileSize * 3 - IClickableMenu.borderWidth, Game1.tileSize, Game1.tileSize), "", "", Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46, -1, -1), 1f);
            if (!trashCan)
                return;
            this.TrashCan = new ClickableTextureComponent("trashcan", new Rectangle(this.xPositionOnScreen + this.width + 4, this.yPositionOnScreen + this.height - Game1.tileSize * 3 - Game1.tileSize / 2 - IClickableMenu.borderWidth - 104, Game1.tileSize, 104), "", "", Game1.mouseCursors, new Rectangle(669, 261, 16, 26), (float)Game1.pixelZoom);
        }

        public void MovePosition(int dx, int dy)
        {
            this.xPositionOnScreen += dx;
            this.yPositionOnScreen += dy;
            this.Inventory.MovePosition(dx, dy);
            if (this.OkButton != null)
            {
                this.OkButton.bounds.X += dx;
                this.OkButton.bounds.Y += dy;
            }
            if (this.TrashCan == null)
                return;
            this.TrashCan.bounds.X += dx;
            this.TrashCan.bounds.Y += dy;
        }

        public override bool readyToClose()
        {
            return this.HeldItem == null;
        }

        public override bool isWithinBounds(int x, int y)
        {
            return base.isWithinBounds(x, y);
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            this.HeldItem = this.Inventory.LeftClick(x, y, this.HeldItem, playSound);
            if (!this.isWithinBounds(x, y) && this.readyToClose() && this.TrashCan != null)
                this.TrashCan.containsPoint(x, y);
            if (this.OkButton != null && this.OkButton.containsPoint(x, y) && this.readyToClose())
            {
                this.exitThisMenu(true);
                if (Game1.currentLocation.currentEvent != null)
                    ++Game1.currentLocation.currentEvent.CurrentCommand;
                Game1.playSound("bigDeSelect");
            }
            if (this.TrashCan == null || !this.TrashCan.containsPoint(x, y) || (this.HeldItem == null))
                return;
            if (this.HeldItem is StardewValley.Object && Game1.player.specialItems.Contains((this.HeldItem as StardewValley.Object).parentSheetIndex))
                Game1.player.specialItems.Remove((this.HeldItem as StardewValley.Object).parentSheetIndex);
            this.HeldItem = (Item)null;
            Game1.playSound("trashcan");
        }

        public override void receiveRightClick(int x, int y, bool playSound = true)
        {
            this.HeldItem = this.Inventory.RightClick(x, y, this.HeldItem, playSound);
        }

        public override void performHoverAction(int x, int y)
        {
            this.DescriptionText = "";
            this.DescriptionTitle = "";
            this.HoveredItem = this.Inventory.Hover(x, y, this.HeldItem);
            this.HoverText = this.Inventory.HoverText;
            if (this.OkButton != null)
            {
                if (this.OkButton.containsPoint(x, y))
                    this.OkButton.scale = Math.Min(1.1f, this.OkButton.scale + 0.05f);
                else
                    this.OkButton.scale = Math.Max(1f, this.OkButton.scale - 0.05f);
            }
            if (this.TrashCan == null)
                return;
            if (this.TrashCan.containsPoint(x, y))
            {
                if ((double)this.TrashCanLidRotation <= 0.0)
                    Game1.playSound("trashcanlid");
                this.TrashCanLidRotation = Math.Min(this.TrashCanLidRotation + 0.06544985f, 1.570796f);
            }
            else
                this.TrashCanLidRotation = Math.Max(this.TrashCanLidRotation - 0.06544985f, 0.0f);
        }

        public override void receiveScrollWheelAction(int direction)
        {
            if (GameMenu.forcePreventClose)
                return;
            base.receiveScrollWheelAction(direction);
            if (this.Inventory != null)
                this.Inventory.receiveScrollWheelAction(direction);
        }

        public override void update(GameTime time)
        {
            if (this.WiggleWordsTimer <= 0)
                return;
            this.WiggleWordsTimer -= time.ElapsedGameTime.Milliseconds;
        }

        public virtual void Draw(SpriteBatch spriteBatch, bool drawUpperPortion = true, bool drawDescriptionArea = true)
        {
            if (this.TrashCan != null)
            {
                this.TrashCan.draw(spriteBatch);
                spriteBatch.Draw(Game1.mouseCursors, new Vector2((float)(this.TrashCan.bounds.X + 60), (float)(this.TrashCan.bounds.Y + 40)), new Rectangle?(new Rectangle(686, 256, 18, 10)), Color.White, this.TrashCanLidRotation, new Vector2(16f, 10f), (float)Game1.pixelZoom, SpriteEffects.None, 0.86f);
            }
            if (drawUpperPortion)
            {
                Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true, (string)null, false);
                this.drawHorizontalPartition(spriteBatch, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + 4 * Game1.tileSize, false);
                if (drawDescriptionArea)
                {
                    this.drawVerticalUpperIntersectingPartition(spriteBatch, this.xPositionOnScreen + Game1.tileSize * 9, 5 * Game1.tileSize + Game1.tileSize / 8);
                    if (!this.DescriptionText.Equals(""))
                    {
                        int num1 = this.xPositionOnScreen + Game1.tileSize * 9 + Game1.tileSize * 2 / 3 + (this.WiggleWordsTimer > 0 ? Game1.random.Next(-2, 3) : 0);
                        int num2 = this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 2 + (this.WiggleWordsTimer > 0 ? Game1.random.Next(-2, 3) : 0);
                        spriteBatch.DrawString(Game1.smallFont, Game1.parseText(this.DescriptionText, Game1.smallFont, Game1.tileSize * 3 + Game1.tileSize / 2), new Vector2((float)num1, (float)num2), Game1.textColor * 0.75f);
                    }
                }
            }
            else
                Game1.drawDialogueBox(this.xPositionOnScreen - IClickableMenu.borderWidth / 2, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize, this.width, this.height - (IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + 3 * Game1.tileSize), false, true, (string)null, false);
            if (this.OkButton != null)
                this.OkButton.draw(spriteBatch);
            this.Inventory.draw(spriteBatch);
        }

        public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
        {
            base.gameWindowSizeChanged(oldBounds, newBounds);
            if (this.yPositionOnScreen < IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder)
                this.yPositionOnScreen = IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder;
            if (this.xPositionOnScreen < 0)
                this.xPositionOnScreen = 0;
            int yPosition = this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth + Game1.tileSize * 3 - Game1.tileSize / 4;
            this.Inventory = new ItemInventoryMenu(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth / 2, yPosition, false, (List<Item>)null, this.Inventory.HighlightMethod, -1, 3, 0, 0, true);
            if (this.OkButton != null)
                this.OkButton = new ClickableTextureComponent("ok-button", new Rectangle(this.xPositionOnScreen + this.width + 4, this.yPositionOnScreen + this.height - Game1.tileSize * 3 - IClickableMenu.borderWidth, Game1.tileSize, Game1.tileSize), "", "", Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46, -1, -1), 1f);
            if (this.TrashCan == null)
                return;
            this.TrashCan = new ClickableTextureComponent("trashcan", new Rectangle(this.xPositionOnScreen + this.width + 4, this.yPositionOnScreen + this.height - Game1.tileSize * 3 - Game1.tileSize / 2 - IClickableMenu.borderWidth - 104, Game1.tileSize, 104), "", "", Game1.mouseCursors, new Rectangle(669, 261, 16, 26), (float)Game1.pixelZoom);
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }
    }
}
