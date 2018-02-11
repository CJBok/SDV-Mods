using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardewValley.Menus;
using SObject = StardewValley.Object;

namespace CJBEndlessInventory.Framework
{
    internal class ItemInventoryMenu : IClickableMenu
    {
        /*********
        ** Properties
        *********/
        private readonly bool PlayerInventory;
        private readonly List<ClickableComponent> Inventory = new List<ClickableComponent>();
        private readonly int Capacity;
        private readonly int Rows;
        private readonly int HorizontalGap;
        private readonly int VerticalGap;
        private int ScrollIndex;


        /*********
        ** Accessors
        *********/
        public string HoverText = "";
        public string DescriptionTitle = "";
        public string DescriptionText = "";
        public List<Item> ActualInventory;
        public InventoryMenu.highlightThisItem HighlightMethod;
        public bool ShowGrayedOutSlots;


        /*********
        ** Public methods
        *********/
        public ItemInventoryMenu(int xPosition, int yPosition, bool playerInventory, List<Item> actualInventory = null, InventoryMenu.highlightThisItem highlightMethod = null, int capacity = -1, int rows = 3, int horizontalGap = 0, int verticalGap = 0)
          : base(xPosition, yPosition, Game1.tileSize * 12, Game1.tileSize * 3 + Game1.tileSize / 4)
        {
            this.HorizontalGap = horizontalGap;
            this.VerticalGap = verticalGap;
            this.Rows = rows;
            this.Capacity = capacity == -1 ? 36 : capacity;
            this.PlayerInventory = playerInventory;
            this.ActualInventory = actualInventory;
            if (actualInventory == null)
                this.ActualInventory = Game1.player.items;

            for (int i = 0; i < Game1.player.maxItems; ++i)
            {
                if (Game1.player.items.Count <= i)
                    Game1.player.items.Add(null);
            }
            this.PlayerInventory = this.ActualInventory == Game1.player.items;

            for (int i = 0; i < this.ActualInventory.Count && i < this.Capacity; i++)
                this.Inventory.Add(new ClickableComponent(new Rectangle(xPosition + i % (this.Capacity / rows) * Game1.tileSize + horizontalGap * (i % (this.Capacity / rows)), this.yPositionOnScreen + i / (this.Capacity / rows) * (Game1.tileSize + verticalGap) + (i / (this.Capacity / rows) - 1) * Game1.pixelZoom, Game1.tileSize, Game1.tileSize), string.Concat(i)));


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
            foreach (ClickableComponent slot in this.Inventory)
            {
                if (slot != null && slot.bounds.Contains(x, y))
                    return Convert.ToInt32(slot.name);
            }
            return -1;
        }

        public Item LeftClick(int x, int y, Item toPlace, bool playSound = true)
        {
            foreach (ClickableComponent component in this.Inventory)
            {
                if (component.containsPoint(x, y))
                {
                    int index = Convert.ToInt32(component.name);
                    if (!this.PlayerInventory)
                        index = this.Capacity / this.Rows * this.ScrollIndex + index;
                    if (index < this.ActualInventory.Count && (this.ActualInventory[index] == null || this.HighlightMethod(this.ActualInventory[index]) || this.ActualInventory[index].canStackWith(toPlace)))
                    {
                        if (this.ActualInventory[index] != null)
                        {
                            Item item;
                            if (toPlace != null)
                            {
                                if (playSound)
                                    Game1.playSound("stoneStep");
                                item = Utility.addItemToInventory(toPlace, index, this.ActualInventory);
                                return item;
                            }
                            if (playSound)
                                Game1.playSound("dwop");
                            item = Utility.removeItemFromInventory(index, this.ActualInventory);
                            return item;
                        }
                        else if (toPlace != null)
                        {
                            if (playSound)
                                Game1.playSound("stoneStep");
                            return Utility.addItemToInventory(toPlace, index, this.ActualInventory);
                        }
                    }
                }
            }
            return toPlace;
        }

        public Vector2 SnapToClickableComponent(int x, int y)
        {
            foreach (ClickableComponent component in this.Inventory)
            {
                if (component.containsPoint(x, y))
                    return new Vector2(component.bounds.X, component.bounds.Y);
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
            foreach (ClickableComponent component in this.Inventory)
            {
                int index = Convert.ToInt32(component.name);
                if (!this.PlayerInventory)
                    index = this.Capacity / this.Rows * this.ScrollIndex + index;
                if (component.containsPoint(x, y) && (this.ActualInventory[index] == null || this.HighlightMethod(this.ActualInventory[index])) && index < this.ActualInventory.Count && this.ActualInventory[index] != null)
                {
                    if (this.ActualInventory[index] is Tool && (toAddTo == null || toAddTo is SObject) && (this.ActualInventory[index] as Tool).canThisBeAttached((SObject)toAddTo))
                        return (this.ActualInventory[index] as Tool).attach((SObject)toAddTo);
                    if (toAddTo == null)
                    {
                        if (this.ActualInventory[index].maximumStackSize() != -1)
                        {
                            if (index == Game1.player.CurrentToolIndex && this.ActualInventory[index] != null && this.ActualInventory[index].Stack == 1)
                                this.ActualInventory[index].actionWhenStopBeingHeld(Game1.player);
                            Item item = this.ActualInventory[index].getOne();
                            if (this.ActualInventory[index].Stack > 1 && Game1.isOneOfTheseKeysDown(Game1.oldKBState, new[] { new InputButton(Keys.LeftShift) }))
                            {
                                item.Stack = (int)Math.Ceiling(this.ActualInventory[index].Stack / 2.0);
                                this.ActualInventory[index].Stack = this.ActualInventory[index].Stack / 2;
                            }
                            else if (this.ActualInventory[index].Stack == 1)
                                this.ActualInventory[index] = null;
                            else
                                this.ActualInventory[index].Stack--;

                            if (this.ActualInventory[index] != null && this.ActualInventory[index].Stack <= 0)
                                this.ActualInventory[index] = null;
                            if (playSound)
                                Game1.playSound("dwop");
                            return item;
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
                            toAddTo.Stack++;
                            this.ActualInventory[index].Stack--;
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
                    index = this.Capacity / this.Rows * this.ScrollIndex + index;

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

        public override void receiveScrollWheelAction(int direction)
        {
            if (GameMenu.forcePreventClose)
                return;
            base.receiveScrollWheelAction(direction);
            if (direction > 0)
                this.ScrollIndex--;
            if (direction < 0)
                this.ScrollIndex++;

            if (this.ScrollIndex < 0)
                this.ScrollIndex = 0;
            if (this.ScrollIndex > ((this.ActualInventory.Count - 1) / (this.Capacity / this.Rows)))
                this.ScrollIndex = ((this.ActualInventory.Count - 1) / (this.Capacity / this.Rows));
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            for (int index = 0; index < 36; ++index)
            {
                int indexOffset = index;
                if (!this.PlayerInventory)
                    indexOffset = 36 / 3 * this.ScrollIndex + index;

                Vector2 slotPosition = new Vector2(this.xPositionOnScreen + index % (36 / 3) * Game1.tileSize + this.HorizontalGap * (index % (36 / 3)), this.yPositionOnScreen + index / (36 / 3) * (Game1.tileSize + this.VerticalGap) + (index / (36 / 3) - 1) * Game1.pixelZoom);
                spriteBatch.Draw(Game1.menuTexture, slotPosition, Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 10), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
                if (this.ActualInventory.Count > indexOffset && this.ActualInventory[indexOffset] != null)
                    this.ActualInventory[indexOffset].drawInMenu(spriteBatch, slotPosition, this.Inventory.Count > index ? this.Inventory[index].scale : 1f, !this.HighlightMethod(this.ActualInventory[indexOffset]) ? 0.2f : 1f, 0.865f);
            }
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true) { }

        public override void receiveRightClick(int x, int y, bool playSound = true) { }

        public override void performHoverAction(int x, int y) { }

        public static Item RemoveItemFromInventory(int index, List<Item> items)
        {
            if (index >= 0 && index < items.Count && items[index] != null)
            {
                Item item = items[index].getOne();
                item.Stack = items[index].Stack;
                if (index == Game1.player.CurrentToolIndex && items == Game1.player.items)
                    item.actionWhenStopBeingHeld(Game1.player);
                if (items == Game1.player.items)
                    items[index] = null;
                return item;
            }
            return null;
        }

        public static Item AddItemToInventory(Item item, int position, List<Item> items, ItemGrabMenu.behaviorOnItemSelect onAddFunction = null)
        {
            if (items == Game1.player.items && item is SObject obj && obj.specialItem)
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
            if (items[position].maximumStackSize() == -1 || items[position].Name != item.Name || (item is SObject addItem && items[position] is SObject slotItem && (addItem.quality != slotItem.quality || addItem.parentSheetIndex != slotItem.parentSheetIndex)) || !item.canStackWith(items[position]))
            {
                Item foundItem = items[position];
                if (position == Game1.player.CurrentToolIndex && items == Game1.player.items && foundItem != null)
                {
                    foundItem.actionWhenStopBeingHeld(Game1.player);
                    item.actionWhenBeingHeld(Game1.player);
                }
                items[position] = item;
                onAddFunction?.Invoke(item, null);
                return foundItem;
            }
            int newStack = items[position].addToStack(item.getStack());
            if (newStack <= 0)
                return null;

            item.Stack = newStack;
            onAddFunction?.Invoke(item, null);
            return item;
        }
    }
}
