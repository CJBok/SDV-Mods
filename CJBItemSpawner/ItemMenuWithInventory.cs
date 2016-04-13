// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.MenuWithInventory
// Assembly: Stardew Valley, Version=1.0.5912.41135, Culture=neutral, PublicKeyToken=null
// MVID: B585F4A7-F5D4-496B-8930-4705FA185302
// Assembly location: K:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;

namespace CJBItemSpawner {
    public class ItemMenuWithInventory : IClickableMenu {
        public string descriptionText = "";
        public string hoverText = "";
        public string descriptionTitle = "";
        public ItemInventoryMenu inventory;
        public Item heldItem;
        public Item hoveredItem;
        public int wiggleWordsTimer;
        public ClickableTextureComponent okButton;
        public ClickableTextureComponent trashCan;
        public float trashCanLidRotation;
        public int inventoryYOffset;

        public ItemMenuWithInventory(InventoryMenu.highlightThisItem highlighterMethod = null, bool okButton = false, bool trashCan = false, int inventoryXOffset = 0, int inventoryYOffset = 0)
          : base(Game1.viewport.Width / 2 - (800 + IClickableMenu.borderWidth * 2) / 2, Game1.viewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2, 800 + IClickableMenu.borderWidth * 2, 600 + IClickableMenu.borderWidth * 2, false) {
            if (this.yPositionOnScreen < IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder)
                this.yPositionOnScreen = IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder;
            if (this.xPositionOnScreen < 0)
                this.xPositionOnScreen = 0;
            this.inventoryYOffset = inventoryYOffset;
            int yPosition = this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth + Game1.tileSize * 3 - Game1.tileSize / 4 + inventoryYOffset;
            this.inventory = new ItemInventoryMenu(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth / 2 + inventoryXOffset, yPosition, false, (List<Item>)null, highlighterMethod, -1, 3, 0, 0, true);
            if (okButton)
                this.okButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + 4, this.yPositionOnScreen + this.height - Game1.tileSize * 3 - IClickableMenu.borderWidth, Game1.tileSize, Game1.tileSize), "", "", Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46, -1, -1), 1f);
            if (!trashCan)
                return;
            this.trashCan = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + 4, this.yPositionOnScreen + this.height - Game1.tileSize * 3 - Game1.tileSize / 2 - IClickableMenu.borderWidth - 104, Game1.tileSize, 104), "", "", Game1.mouseCursors, new Rectangle(669, 261, 16, 26), (float)Game1.pixelZoom);
        }

        public void movePosition(int dx, int dy) {
            this.xPositionOnScreen += dx;
            this.yPositionOnScreen += dy;
            this.inventory.movePosition(dx, dy);
            if (this.okButton != null) {
                this.okButton.bounds.X += dx;
                this.okButton.bounds.Y += dy;
            }
            if (this.trashCan == null)
                return;
            this.trashCan.bounds.X += dx;
            this.trashCan.bounds.Y += dy;
        }

        public override bool readyToClose() {
            return this.heldItem == null;
        }

        public override bool isWithinBounds(int x, int y) {
            return base.isWithinBounds(x, y);
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true) {
            this.heldItem = this.inventory.leftClick(x, y, this.heldItem, playSound);
            if (!this.isWithinBounds(x, y) && this.readyToClose() && this.trashCan != null)
                this.trashCan.containsPoint(x, y);
            if (this.okButton != null && this.okButton.containsPoint(x, y) && this.readyToClose()) {
                this.exitThisMenu(true);
                if (Game1.currentLocation.currentEvent != null)
                    ++Game1.currentLocation.currentEvent.CurrentCommand;
                Game1.playSound("bigDeSelect");
            }
            if (this.trashCan == null || !this.trashCan.containsPoint(x, y) || (this.heldItem == null))
                return;
            if (this.heldItem is StardewValley.Object && Game1.player.specialItems.Contains((this.heldItem as StardewValley.Object).parentSheetIndex))
                Game1.player.specialItems.Remove((this.heldItem as StardewValley.Object).parentSheetIndex);
            this.heldItem = (Item)null;
            Game1.playSound("trashcan");
        }

        public override void receiveRightClick(int x, int y, bool playSound = true) {
            this.heldItem = this.inventory.rightClick(x, y, this.heldItem, playSound);
        }

        public override void performHoverAction(int x, int y) {
            this.descriptionText = "";
            this.descriptionTitle = "";
            this.hoveredItem = this.inventory.hover(x, y, this.heldItem);
            this.hoverText = this.inventory.hoverText;
            if (this.okButton != null) {
                if (this.okButton.containsPoint(x, y))
                    this.okButton.scale = Math.Min(1.1f, this.okButton.scale + 0.05f);
                else
                    this.okButton.scale = Math.Max(1f, this.okButton.scale - 0.05f);
            }
            if (this.trashCan == null)
                return;
            if (this.trashCan.containsPoint(x, y)) {
                if ((double)this.trashCanLidRotation <= 0.0)
                    Game1.playSound("trashcanlid");
                this.trashCanLidRotation = Math.Min(this.trashCanLidRotation + 0.06544985f, 1.570796f);
            } else
                this.trashCanLidRotation = Math.Max(this.trashCanLidRotation - 0.06544985f, 0.0f);
        }

        public override void receiveScrollWheelAction(int direction) {
            if (GameMenu.forcePreventClose)
                return;
            base.receiveScrollWheelAction(direction);
            if (this.inventory != null)
                this.inventory.receiveScrollWheelAction(direction);
        }

        public override void update(GameTime time) {
            if (this.wiggleWordsTimer <= 0)
                return;
            this.wiggleWordsTimer -= time.ElapsedGameTime.Milliseconds;
        }

        public virtual void draw(SpriteBatch b, bool drawUpperPortion = true, bool drawDescriptionArea = true) {
            if (this.trashCan != null) {
                this.trashCan.draw(b);
                b.Draw(Game1.mouseCursors, new Vector2((float)(this.trashCan.bounds.X + 60), (float)(this.trashCan.bounds.Y + 40)), new Rectangle?(new Rectangle(686, 256, 18, 10)), Color.White, this.trashCanLidRotation, new Vector2(16f, 10f), (float)Game1.pixelZoom, SpriteEffects.None, 0.86f);
            }
            if (drawUpperPortion) {
                Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true, (string)null, false);
                this.drawHorizontalPartition(b, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + 4 * Game1.tileSize, false);
                if (drawDescriptionArea) {
                    this.drawVerticalUpperIntersectingPartition(b, this.xPositionOnScreen + Game1.tileSize * 9, 5 * Game1.tileSize + Game1.tileSize / 8);
                    if (!this.descriptionText.Equals("")) {
                        int num1 = this.xPositionOnScreen + Game1.tileSize * 9 + Game1.tileSize * 2 / 3 + (this.wiggleWordsTimer > 0 ? Game1.random.Next(-2, 3) : 0);
                        int num2 = this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 2 + (this.wiggleWordsTimer > 0 ? Game1.random.Next(-2, 3) : 0);
                        b.DrawString(Game1.smallFont, Game1.parseText(this.descriptionText, Game1.smallFont, Game1.tileSize * 3 + Game1.tileSize / 2), new Vector2((float)num1, (float)num2), Game1.textColor * 0.75f);
                    }
                }
            } else
                Game1.drawDialogueBox(this.xPositionOnScreen - IClickableMenu.borderWidth / 2, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize + this.inventoryYOffset, this.width, this.height - (IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + 3 * Game1.tileSize), false, true, (string)null, false);
            if (this.okButton != null)
                this.okButton.draw(b);
            this.inventory.draw(b);
        }

        public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds) {
            base.gameWindowSizeChanged(oldBounds, newBounds);
            if (this.yPositionOnScreen < IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder)
                this.yPositionOnScreen = IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder;
            if (this.xPositionOnScreen < 0)
                this.xPositionOnScreen = 0;
            int yPosition = this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth + Game1.tileSize * 3 - Game1.tileSize / 4;
            this.inventory = new ItemInventoryMenu(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth / 2, yPosition, false, (List<Item>)null, this.inventory.highlightMethod, -1, 3, 0, 0, true);
            if (this.okButton != null)
                this.okButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + 4, this.yPositionOnScreen + this.height - Game1.tileSize * 3 - IClickableMenu.borderWidth, Game1.tileSize, Game1.tileSize), "", "", Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46, -1, -1), 1f);
            if (this.trashCan == null)
                return;
            this.trashCan = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + 4, this.yPositionOnScreen + this.height - Game1.tileSize * 3 - Game1.tileSize / 2 - IClickableMenu.borderWidth - 104, Game1.tileSize, 104), "", "", Game1.mouseCursors, new Rectangle(669, 261, 16, 26), (float)Game1.pixelZoom);
        }

        public override void draw(SpriteBatch b) {
            throw new NotImplementedException();
        }
    }
}
