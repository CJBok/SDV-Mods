// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.InventoryMenu
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

namespace CJBItemSpawner
{
    internal class ItemInventoryMenu : IClickableMenu
    {
        /*********
        ** Properties
        *********/
        private bool PlayerInventory;
        private List<ClickableComponent> Inventory = new List<ClickableComponent>();
        private int Capacity;
        private int Rows;
        private int HorizontalGap;
        private int VerticalGap;


        /*********
        ** Accessors
        *********/
        public string HoverText = "";
        public string DescriptionTitle = "";
        public string DescriptionText = "";
        public List<Item> ActualInventory;
        public InventoryMenu.highlightThisItem HighlightMethod;
        public bool ShowGrayedOutSlots;
        public static int ScrollIndex;


        /*********
        ** Public methods
        *********/
        public ItemInventoryMenu(int xPosition, int yPosition, bool playerInventory, List<Item> actualInventory = null, InventoryMenu.highlightThisItem highlightMethod = null, int capacity = -1, int rows = 3, int horizontalGap = 0, int verticalGap = 0)
          : base(xPosition, yPosition, Game1.tileSize * ((capacity == -1 ? 36 : capacity) / rows), Game1.tileSize * rows + Game1.tileSize / 4)
        {
            this.HorizontalGap = horizontalGap;
            this.VerticalGap = verticalGap;
            this.Rows = rows;
            this.Capacity = capacity == -1 ? 36 : capacity;
            this.PlayerInventory = playerInventory;
            this.ActualInventory = actualInventory;

            if (actualInventory == null)
                this.ActualInventory = Game1.player.items;

            for (int index = 0; index < Game1.player.maxItems; ++index)
            {
                if (Game1.player.items.Count <= index)
                    Game1.player.items.Add(null);
            }
            this.PlayerInventory = this.ActualInventory == Game1.player.items;

            for (int index = 0; index < this.ActualInventory.Count && index < this.Capacity; index++)
                this.Inventory.Add(new ClickableComponent(new Rectangle(xPosition + index % (this.Capacity / rows) * Game1.tileSize + horizontalGap * (index % (this.Capacity / rows)), this.yPositionOnScreen + index / (this.Capacity / rows) * (Game1.tileSize + verticalGap) + (index / (this.Capacity / rows) - 1) * Game1.pixelZoom, Game1.tileSize, Game1.tileSize), index.ToString()));

            this.HighlightMethod = highlightMethod;
            if (highlightMethod != null)
                return;
            this.HighlightMethod = InventoryMenu.highlightAllItems;
        }

        public static bool HighlightAllItems(Item i)
        {
            return true;
        }

        public void MovePosition(int x, int y)
        {
            this.xPositionOnScreen += x;
            this.yPositionOnScreen += y;
            foreach (ClickableComponent clickableComponent in this.Inventory)
            {
                clickableComponent.bounds.X += x;
                clickableComponent.bounds.Y += y;
            }
        }

        public int GetInventoryPositionOfClick(int x, int y)
        {
            foreach (ClickableComponent component in this.Inventory)
            {
                if (component != null && component.bounds.Contains(x, y))
                    return Convert.ToInt32(component.name);
            }
            return -1;
        }

        public Item LeftClick(int x, int y, Item toPlace, bool playSound = true)
        {
            foreach (ClickableComponent clickableComponent in this.Inventory)
            {
                if (clickableComponent.containsPoint(x, y))
                {
                    int index = Convert.ToInt32(clickableComponent.name);
                    if (!this.PlayerInventory)
                        index = this.Capacity / this.Rows * ItemInventoryMenu.ScrollIndex + index;
                    if (index < this.ActualInventory.Count && (this.ActualInventory[index] == null || this.HighlightMethod(this.ActualInventory[index]) || this.ActualInventory[index].canStackWith(toPlace)))
                    {
                        if (this.ActualInventory[index] != null)
                        {
                            if (toPlace != null)
                            {
                                if (playSound)
                                    Game1.playSound("stoneStep");
                                return ItemInventoryMenu.AddItemToInventory(toPlace, index, this.ActualInventory);
                            }
                            if (playSound)
                                Game1.playSound("dwop");
                            return ItemInventoryMenu.RemoveItemFromInventory(index, this.ActualInventory);
                        }
                        if (toPlace != null)
                        {
                            if (playSound)
                                Game1.playSound("stoneStep");
                            return ItemInventoryMenu.AddItemToInventory(toPlace, index, this.ActualInventory);
                        }
                    }
                }
            }
            return toPlace;
        }

        public Vector2 SnapToClickableComponent(int x, int y)
        {
            foreach (ClickableComponent clickableComponent in this.Inventory)
            {
                if (clickableComponent.containsPoint(x, y))
                    return new Vector2(clickableComponent.bounds.X, clickableComponent.bounds.Y);
            }
            return new Vector2(x, y);
        }

        public Item GetItemAt(int x, int y)
        {
            foreach (ClickableComponent c in this.Inventory)
            {
                if (c.containsPoint(x, y))
                    return this.GetItemFromClickableComponent(c);
            }
            return null;
        }

        public Item GetItemFromClickableComponent(ClickableComponent c)
        {
            if (c != null)
            {
                int index = Convert.ToInt32(c.name);
                if (index < this.ActualInventory.Count)
                    return this.ActualInventory[index];
            }
            return null;
        }

        public Item RightClick(int x, int y, Item toAddTo, bool playSound = true)
        {
            foreach (ClickableComponent clickableComponent in this.Inventory)
            {
                int index = Convert.ToInt32(clickableComponent.name);
                if (!this.PlayerInventory)
                    index = this.Capacity / this.Rows * ItemInventoryMenu.ScrollIndex + index;
                if (clickableComponent.containsPoint(x, y) && index < this.ActualInventory.Count && this.ActualInventory[index] != null)
                {
                    if (this.ActualInventory[index] is Tool tool && (toAddTo == null || toAddTo is StardewValley.Object) && tool.canThisBeAttached((StardewValley.Object)toAddTo))
                        return tool.attach((StardewValley.Object)toAddTo);
                    if (toAddTo == null)
                    {
                        if (this.ActualInventory[index].maximumStackSize() != -1)
                        {
                            if (index == Game1.player.CurrentToolIndex && this.ActualInventory[index] != null && this.ActualInventory[index].Stack == 1)
                                this.ActualInventory[index].actionWhenStopBeingHeld(Game1.player);
                            Item one = this.ActualInventory[index].getOne();
                            if (this.ActualInventory[index].Stack > 1)
                            {
                                if (Game1.isOneOfTheseKeysDown(Game1.oldKBState, new[] { new InputButton(Keys.LeftShift) }))
                                {
                                    one.Stack = (int)Math.Ceiling(this.ActualInventory[index].Stack / 2.0);
                                    if (this.PlayerInventory)
                                        this.ActualInventory[index].Stack = this.ActualInventory[index].Stack / 2;
                                    goto label_15;
                                }
                            }
                            if (this.PlayerInventory)
                            {
                                if (this.ActualInventory[index].Stack == 1)
                                    this.ActualInventory[index] = null;
                                else
                                    --this.ActualInventory[index].Stack;
                            }

                            label_15:
                            if (this.PlayerInventory && this.ActualInventory[index] != null && this.ActualInventory[index].Stack <= 0)
                                this.ActualInventory[index] = null;
                            if (playSound)
                                Game1.playSound("dwop");
                            return one;
                        }
                    }
                    else if (this.ActualInventory[index].canStackWith(toAddTo) && toAddTo.Stack < toAddTo.maximumStackSize())
                    {
                        if (Game1.isOneOfTheseKeysDown(Game1.oldKBState, new[] { new InputButton(Keys.LeftShift) }))
                        {
                            toAddTo.Stack += (int)Math.Ceiling(this.ActualInventory[index].Stack / 2.0);
                            this.ActualInventory[index].Stack = this.ActualInventory[index].Stack / 2;
                        }
                        else
                        {
                            ++toAddTo.Stack;
                            --this.ActualInventory[index].Stack;
                        }
                        if (playSound)
                            Game1.playSound("dwop");
                        if (this.ActualInventory[index].Stack <= 0)
                        {
                            if (index == Game1.player.CurrentToolIndex)
                                this.ActualInventory[index].actionWhenStopBeingHeld(Game1.player);
                            this.ActualInventory[index] = null;
                        }
                        return toAddTo;
                    }
                }
            }
            return toAddTo;
        }

        public Item Hover(int x, int y, Item heldItem)
        {
            this.DescriptionText = "";
            this.DescriptionTitle = "";
            this.HoverText = "";
            foreach (ClickableComponent clickableComponent in this.Inventory)
            {
                int index = Convert.ToInt32(clickableComponent.name);
                if (!this.PlayerInventory)
                    index = this.Capacity / this.Rows * ItemInventoryMenu.ScrollIndex + index;

                clickableComponent.scale = Math.Max(1f, clickableComponent.scale - 0.025f);
                if (clickableComponent.containsPoint(x, y) && index < this.ActualInventory.Count && this.ActualInventory[index] != null)
                    return this.ActualInventory[index];
            }
            return null;
        }

        public override void setUpForGamePadMode()
        {
            base.setUpForGamePadMode();
            if (this.Inventory == null || this.Inventory.Count <= 0)
                return;
            Game1.setMousePosition(this.Inventory[0].bounds.Right - this.Inventory[0].bounds.Width / 8, this.Inventory[0].bounds.Bottom - this.Inventory[0].bounds.Height / 8);
        }

        public override int moveCursorInDirection(int direction)
        {
            Rectangle rectangle = new Rectangle(this.Inventory[0].bounds.X, this.Inventory[0].bounds.Y, this.Inventory.Last().bounds.X + this.Inventory.Last().bounds.Width - this.Inventory[0].bounds.X, this.Inventory.Last().bounds.Y + this.Inventory.Last().bounds.Height - this.Inventory[0].bounds.Y);
            if (!rectangle.Contains(Game1.getMousePosition()))
                Game1.setMousePosition(this.Inventory[0].bounds.Right - this.Inventory[0].bounds.Width / 8, this.Inventory[0].bounds.Bottom - this.Inventory[0].bounds.Height / 8);
            Point mousePosition = Game1.getMousePosition();
            switch (direction)
            {
                case 0:
                    Game1.setMousePosition(mousePosition.X, mousePosition.Y - Game1.tileSize - this.VerticalGap);
                    break;
                case 1:
                    Game1.setMousePosition(mousePosition.X + Game1.tileSize + this.HorizontalGap, mousePosition.Y);
                    break;
                case 2:
                    Game1.setMousePosition(mousePosition.X, mousePosition.Y + Game1.tileSize + this.VerticalGap);
                    break;
                case 3:
                    Game1.setMousePosition(mousePosition.X - Game1.tileSize - this.HorizontalGap, mousePosition.Y);
                    break;
            }
            if (rectangle.Contains(Game1.getMousePosition()))
                return -1;
            Game1.setMousePosition(mousePosition);
            return direction;
        }

        public override void receiveScrollWheelAction(int direction)
        {
            if (GameMenu.forcePreventClose)
                return;
            base.receiveScrollWheelAction(direction);
            if (direction > 0) ItemInventoryMenu.ScrollIndex--;
            if (direction < 0) ItemInventoryMenu.ScrollIndex++;

            if (ItemInventoryMenu.ScrollIndex < 0) ItemInventoryMenu.ScrollIndex = 0;
            if (ItemInventoryMenu.ScrollIndex > (this.ActualInventory.Count / (this.Capacity / this.Rows)))
                ItemInventoryMenu.ScrollIndex = (this.ActualInventory.Count / (this.Capacity / this.Rows));
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            for (int index = 0; index < this.Capacity; ++index)
            {

                int indexOffset = index;
                if (!this.PlayerInventory)
                    indexOffset = this.Capacity / this.Rows * ItemInventoryMenu.ScrollIndex + index;

                Vector2 vector2 = new Vector2(this.xPositionOnScreen + index % (this.Capacity / this.Rows) * Game1.tileSize + this.HorizontalGap * (index % (this.Capacity / this.Rows)), this.yPositionOnScreen + index / (this.Capacity / this.Rows) * (Game1.tileSize + this.VerticalGap) + (index / (this.Capacity / this.Rows) - 1) * Game1.pixelZoom);
                spriteBatch.Draw(Game1.menuTexture, vector2, Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 10), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
                if (this.ActualInventory.Count > indexOffset && this.ActualInventory[indexOffset] != null)
                    this.ActualInventory[indexOffset].drawInMenu(spriteBatch, vector2, this.Inventory.Count > index ? this.Inventory[index].scale : 1f, !this.HighlightMethod(this.ActualInventory[indexOffset]) ? 0.2f : 1f, 0.865f);
            }
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true) { }
        public override void receiveRightClick(int x, int y, bool playSound = true) { }
        public override void performHoverAction(int x, int y) { }

        public static Item RemoveItemFromInventory(int whichItemIndex, List<Item> items)
        {
            if (whichItemIndex >= 0 && whichItemIndex < items.Count && items[whichItemIndex] != null)
            {
                Item item = items[whichItemIndex].getOne();
                item.Stack = items[whichItemIndex].Stack;
                if (whichItemIndex == Game1.player.CurrentToolIndex && items.Equals(Game1.player.items))
                    item.actionWhenStopBeingHeld(Game1.player);
                if (items.Equals(Game1.player.items))
                    items[whichItemIndex] = null;
                return item;
            }
            return null;
        }

        public static Item AddItemToInventory(Item item, int position, List<Item> items, ItemGrabMenu.behaviorOnItemSelect onAddFunction = null)
        {
            if (items.Equals(Game1.player.items) && item is StardewValley.Object obj && obj.specialItem)
            {
                if (obj.bigCraftable)
                    Game1.player.specialBigCraftables.Add(obj.isRecipe ? (-obj.parentSheetIndex) : obj.parentSheetIndex);
                else
                    Game1.player.specialItems.Add(obj.isRecipe ? (-obj.parentSheetIndex) : obj.parentSheetIndex);
            }
            if (position < 0 || position >= items.Count)
            {
                return item;
            }
            if (items[position] == null)
            {
                items[position] = item;
                onAddFunction?.Invoke(item, null);
                return null;
            }
            if (items[position].maximumStackSize() == -1 || !items[position].Name.Equals(item.Name) || (item is StardewValley.Object itemObj && items[position] is StardewValley.Object slotObj && (itemObj.quality != slotObj.quality || itemObj.parentSheetIndex != slotObj.parentSheetIndex)) || !item.canStackWith(items[position]))
            {
                Item item2 = items[position];
                if (position == Game1.player.CurrentToolIndex && items.Equals(Game1.player.items) && item2 != null)
                {
                    item2.actionWhenStopBeingHeld(Game1.player);
                    item.actionWhenBeingHeld(Game1.player);
                }
                items[position] = item;
                onAddFunction?.Invoke(item, null);
                return item2;
            }
            int num = items[position].addToStack(item.getStack());
            if (num <= 0)
                return null;

            item.Stack = num;
            onAddFunction?.Invoke(item, null);
            return item;
        }
    }
}
