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
    internal class ItemInventoryMenu : IClickableMenu
    {
        /*********
        ** Properties
        *********/
        private bool PlayerInventory;
        private bool DrawSlots;


        /*********
        ** Accessors
        *********/
        public string HoverText = "";
        public string HoverTitle = "";
        public string DescriptionTitle = "";
        public string DescriptionText = "";
        public List<ClickableComponent> Inventory = new List<ClickableComponent>();
        public List<Item> ActualInventory;
        public InventoryMenu.highlightThisItem HighlightMethod;
        public ItemGrabMenu.behaviorOnItemSelect OnAddItem;
        public bool ShowGrayedOutSlots;
        public int Capacity;
        public int Rows;
        public int HorizontalGap;
        public int VerticalGap;
        public static int ScrollIndex = 0;

        public delegate bool HighlightThisItem(Item i);


        /*********
        ** Public methods
        *********/
        public ItemInventoryMenu(int xPosition, int yPosition, bool playerInventory, List<Item> actualInventory = null, InventoryMenu.highlightThisItem highlightMethod = null, int capacity = -1, int rows = 3, int horizontalGap = 0, int verticalGap = 0, bool drawSlots = true)
          : base(xPosition, yPosition, Game1.tileSize * 12, Game1.tileSize * 3 + Game1.tileSize / 4, false)
        {
            this.DrawSlots = drawSlots;
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
                if (Game1.player.items.Count() <= i)
                    Game1.player.items.Add((Item)null);
            }
            this.PlayerInventory = this.ActualInventory == Game1.player.items;

            for (int i = 0; i < this.ActualInventory.Count() && i < this.Capacity; i++)
            {
                this.Inventory.Add(new ClickableComponent(new Rectangle(xPosition + i % (this.Capacity / rows) * Game1.tileSize + horizontalGap * (i % (this.Capacity / rows)), this.yPositionOnScreen + i / (this.Capacity / rows) * (Game1.tileSize + verticalGap) + (i / (this.Capacity / rows) - 1) * Game1.pixelZoom, Game1.tileSize, Game1.tileSize), string.Concat((object)i)));
            }


            this.HighlightMethod = highlightMethod;
            if (highlightMethod != null)
                return;
            this.HighlightMethod = new InventoryMenu.highlightThisItem(InventoryMenu.highlightAllItems);
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
            for (int i = 0; i < Enumerable.Count<ClickableComponent>((IEnumerable<ClickableComponent>)this.Inventory); ++i)
            {
                if (this.Inventory[i] != null && this.Inventory[i].bounds.Contains(x, y))
                    return Convert.ToInt32(this.Inventory[i].name);
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
                        index = this.Capacity / this.Rows * ItemInventoryMenu.ScrollIndex + index;
                    if (index < this.ActualInventory.Count && (this.ActualInventory[index] == null || this.HighlightMethod(this.ActualInventory[index]) || this.ActualInventory[index].canStackWith(toPlace)))
                    {
                        if (this.ActualInventory[index] != null)
                        {
                            Item item;
                            if (toPlace != null)
                            {
                                if (playSound)
                                {
                                    Game1.playSound("stoneStep");
                                }
                                item = Utility.addItemToInventory(toPlace, index, this.ActualInventory, this.OnAddItem);
                                return item;
                            }
                            if (playSound)
                            {
                                Game1.playSound("dwop");
                            }
                            item = Utility.removeItemFromInventory(index, this.ActualInventory);
                            return item;
                        }
                        else if (toPlace != null)
                        {
                            if (playSound)
                            {
                                Game1.playSound("stoneStep");
                            }
                            return Utility.addItemToInventory(toPlace, index, this.ActualInventory, this.OnAddItem);
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
                    return new Vector2((float)component.bounds.X, (float)component.bounds.Y);
            }
            return new Vector2((float)x, (float)y);
        }

        public Item GetItemAt(int x, int y)
        {
            foreach (ClickableComponent c in this.Inventory)
            {
                if (c.containsPoint(x, y))
                    return this.GetItemFromClickableComponent(c);
            }
            return (Item)null;
        }

        public Item GetItemFromClickableComponent(ClickableComponent c)
        {
            if (c != null)
            {
                int index = Convert.ToInt32(c.name);
                if (index < this.ActualInventory.Count)
                    return this.ActualInventory[index];
            }
            return (Item)null;
        }

        public Item RightClick(int x, int y, Item toAddTo, bool playSound = true)
        {
            foreach (ClickableComponent component in this.Inventory)
            {
                int index = Convert.ToInt32(component.name);
                if (!this.PlayerInventory)
                    index = this.Capacity / this.Rows * ItemInventoryMenu.ScrollIndex + index;
                if (component.containsPoint(x, y) && (this.ActualInventory[index] == null || this.HighlightMethod(this.ActualInventory[index])) && index < this.ActualInventory.Count && this.ActualInventory[index] != null)
                {
                    if (this.ActualInventory[index] is Tool && (toAddTo == null || toAddTo is StardewValley.Object) && (this.ActualInventory[index] as Tool).canThisBeAttached((StardewValley.Object)toAddTo))
                    {
                        return (this.ActualInventory[index] as Tool).attach((toAddTo == null) ? null : ((StardewValley.Object)toAddTo));
                    }
                    if (toAddTo == null)
                    {
                        if (this.ActualInventory[index].maximumStackSize() != -1)
                        {
                            if (index == Game1.player.CurrentToolIndex && this.ActualInventory[index] != null && this.ActualInventory[index].Stack == 1)
                            {
                                this.ActualInventory[index].actionWhenStopBeingHeld(Game1.player);
                            }
                            Item item = this.ActualInventory[index].getOne();
                            if (this.ActualInventory[index].Stack > 1 && Game1.isOneOfTheseKeysDown(Game1.oldKBState, new InputButton[]
                            {
                                new InputButton(Keys.LeftShift)
                            }))
                            {
                                item.Stack = (int)Math.Ceiling((double)this.ActualInventory[index].Stack / 2.0);
                                this.ActualInventory[index].Stack = this.ActualInventory[index].Stack / 2;
                            }
                            else if (this.ActualInventory[index].Stack == 1)
                            {
                                this.ActualInventory[index] = null;
                            }
                            else
                            {
                                this.ActualInventory[index].Stack--;
                            }
                            if (this.ActualInventory[index] != null && this.ActualInventory[index].Stack <= 0)
                            {
                                this.ActualInventory[index] = null;
                            }
                            if (playSound)
                            {
                                Game1.playSound("dwop");
                            }
                            return item;
                        }
                    }
                    else if (this.ActualInventory[index].canStackWith(toAddTo) && toAddTo.Stack < toAddTo.maximumStackSize())
                    {
                        if (Game1.isOneOfTheseKeysDown(Game1.oldKBState, new InputButton[]
                        {
                            new InputButton(Keys.LeftShift)
                        }))
                        {
                            toAddTo.Stack += (int)Math.Ceiling((double)this.ActualInventory[index].Stack / 2.0);
                            this.ActualInventory[index].Stack = this.ActualInventory[index].Stack / 2;
                        }
                        else
                        {
                            toAddTo.Stack++;
                            this.ActualInventory[index].Stack--;
                        }
                        if (playSound)
                        {
                            Game1.playSound("dwop");
                        }
                        if (this.ActualInventory[index].Stack <= 0)
                        {
                            if (index == Game1.player.CurrentToolIndex)
                            {
                                this.ActualInventory[index].actionWhenStopBeingHeld(Game1.player);
                            }
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
            this.HoverTitle = "";
            foreach (ClickableComponent clickableComponent in this.Inventory)
            {
                int index = Convert.ToInt32(clickableComponent.name);
                if (!this.PlayerInventory)
                    index = this.Capacity / this.Rows * ItemInventoryMenu.ScrollIndex + index;

                clickableComponent.scale = Math.Max(1f, clickableComponent.scale - 0.025f);
                if (clickableComponent.containsPoint(x, y) && index < this.ActualInventory.Count && this.ActualInventory[index] != null)
                {
                    return this.ActualInventory[index];
                }
            }
            return null;
        }

        public override void setUpForGamePadMode()
        {
            base.setUpForGamePadMode();
            if (this.Inventory == null || Enumerable.Count<ClickableComponent>((IEnumerable<ClickableComponent>)this.Inventory) <= 0)
                return;
            Game1.setMousePosition(this.Inventory[0].bounds.Right - this.Inventory[0].bounds.Width / 8, this.Inventory[0].bounds.Bottom - this.Inventory[0].bounds.Height / 8);
        }

        public override int moveCursorInDirection(int direction)
        {
            Rectangle rectangle = new Rectangle(this.Inventory[0].bounds.X, this.Inventory[0].bounds.Y, Enumerable.Last<ClickableComponent>((IEnumerable<ClickableComponent>)this.Inventory).bounds.X + Enumerable.Last<ClickableComponent>((IEnumerable<ClickableComponent>)this.Inventory).bounds.Width - this.Inventory[0].bounds.X, Enumerable.Last<ClickableComponent>((IEnumerable<ClickableComponent>)this.Inventory).bounds.Y + Enumerable.Last<ClickableComponent>((IEnumerable<ClickableComponent>)this.Inventory).bounds.Height - this.Inventory[0].bounds.Y);
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
            if (ItemInventoryMenu.ScrollIndex > ((this.ActualInventory.Count() - 1) / (this.Capacity / this.Rows)))
                ItemInventoryMenu.ScrollIndex = ((this.ActualInventory.Count() - 1) / (this.Capacity / this.Rows));
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            for (int index = 0; index < 36; ++index)
            {
                int indexOffset = index;
                if (!this.PlayerInventory)
                    indexOffset = 36 / 3 * ItemInventoryMenu.ScrollIndex + index;

                Vector2 slotPosition = new Vector2((float)(this.xPositionOnScreen + index % (36 / 3) * Game1.tileSize + this.HorizontalGap * (index % (36 / 3))), (float)(this.yPositionOnScreen + index / (36 / 3) * (Game1.tileSize + this.VerticalGap) + (index / (36 / 3) - 1) * Game1.pixelZoom));
                spriteBatch.Draw(Game1.menuTexture, slotPosition, new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 10, -1, -1)), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
                if (this.ActualInventory.Count() > indexOffset && this.ActualInventory[indexOffset] != null)
                    this.ActualInventory[indexOffset].drawInMenu(spriteBatch, slotPosition, this.Inventory.Count() > index ? this.Inventory[index].scale : 1f, !this.HighlightMethod(this.ActualInventory[indexOffset]) ? 0.2f : 1f, 0.865f);
            }
        }

        public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
        {
            base.gameWindowSizeChanged(oldBounds, newBounds);
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
        }

        public override void receiveRightClick(int x, int y, bool playSound = true)
        {
        }

        public override void performHoverAction(int x, int y)
        {
        }

        public static Item RemoveItemFromInventory(int index, List<Item> items)
        {
            if (index >= 0 && index < items.Count<Item>() && items[index] != null)
            {
                Item item = items[index].getOne();
                item.Stack = items[index].Stack;
                if (index == Game1.player.CurrentToolIndex && items.Equals(Game1.player.items) && item != null)
                {
                    item.actionWhenStopBeingHeld(Game1.player);
                }
                if (items.Equals(Game1.player.items))
                    items[index] = null;
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
            if (position < 0 || position >= items.Count<Item>())
            {
                return item;
            }
            if (items[position] == null)
            {
                items[position] = item;
                if (onAddFunction != null)
                {
                    onAddFunction(item, null);
                }
                return null;
            }
            if (items[position].maximumStackSize() == -1 || !items[position].Name.Equals(item.Name) || (item is StardewValley.Object && items[position] is StardewValley.Object && ((item as StardewValley.Object).quality != (items[position] as StardewValley.Object).quality || (item as StardewValley.Object).parentSheetIndex != (items[position] as StardewValley.Object).parentSheetIndex)) || !item.canStackWith(items[position]))
            {
                Item foundItem = items[position];
                if (position == Game1.player.CurrentToolIndex && items.Equals(Game1.player.items) && foundItem != null)
                {
                    foundItem.actionWhenStopBeingHeld(Game1.player);
                    item.actionWhenBeingHeld(Game1.player);
                }
                items[position] = item;
                if (onAddFunction != null)
                {
                    onAddFunction(item, null);
                }
                return foundItem;
            }
            int newStack = items[position].addToStack(item.getStack());
            if (newStack <= 0)
            {
                return null;
            }

            item.Stack = newStack;
            if (onAddFunction != null)
            {
                onAddFunction(item, null);
            }
            return item;
        }
    }
}
