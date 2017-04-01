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
    public class ItemInventoryMenu : IClickableMenu
    {
        /*********
        ** Properties
        *********/
        private bool playerInventory;
        private bool drawSlots;


        /*********
        ** Accessors
        *********/
        public string hoverText = "";
        public string hoverTitle = "";
        public string descriptionTitle = "";
        public string descriptionText = "";
        public List<ClickableComponent> inventory = new List<ClickableComponent>();
        public List<Item> actualInventory;
        public InventoryMenu.highlightThisItem highlightMethod;
        public ItemGrabMenu.behaviorOnItemSelect onAddItem;
        public bool showGrayedOutSlots;
        public int capacity;
        public int rows;
        public int horizontalGap;
        public int verticalGap;
        public static int scrollIndex = 0;

        public delegate bool highlightThisItem(Item i);


        /*********
        ** Public methods
        *********/
        public ItemInventoryMenu(int xPosition, int yPosition, bool playerInventory, List<Item> actualInventory = null, InventoryMenu.highlightThisItem highlightMethod = null, int capacity = -1, int rows = 3, int horizontalGap = 0, int verticalGap = 0, bool drawSlots = true)
          : base(xPosition, yPosition, Game1.tileSize * 12, Game1.tileSize * 3 + Game1.tileSize / 4, false)
        {
            this.drawSlots = drawSlots;
            this.horizontalGap = horizontalGap;
            this.verticalGap = verticalGap;
            this.rows = rows;
            this.capacity = capacity == -1 ? 36 : capacity;
            this.playerInventory = playerInventory;
            this.actualInventory = actualInventory;

            if (actualInventory == null)
                this.actualInventory = Game1.player.items;

            for (int index = 0; index < Game1.player.maxItems; ++index)
            {
                if (Game1.player.items.Count() <= index)
                    Game1.player.items.Add((Item)null);
            }
            this.playerInventory = this.actualInventory == Game1.player.items;

            for (int index = 0; index < this.actualInventory.Count() && index < this.capacity; index++)
            {
                this.inventory.Add(new ClickableComponent(new Rectangle(xPosition + index % (this.capacity / rows) * Game1.tileSize + horizontalGap * (index % (this.capacity / rows)), this.yPositionOnScreen + index / (this.capacity / rows) * (Game1.tileSize + verticalGap) + (index / (this.capacity / rows) - 1) * Game1.pixelZoom, Game1.tileSize, Game1.tileSize), string.Concat((object)index)));
            }


            this.highlightMethod = highlightMethod;
            if (highlightMethod != null)
                return;
            this.highlightMethod = new InventoryMenu.highlightThisItem(InventoryMenu.highlightAllItems);
        }

        public static bool highlightAllItems(Item i)
        {
            return true;
        }

        public void movePosition(int x, int y)
        {
            this.xPositionOnScreen += x;
            this.yPositionOnScreen += y;
            foreach (ClickableComponent clickableComponent in this.inventory)
            {
                clickableComponent.bounds.X += x;
                clickableComponent.bounds.Y += y;
            }
        }

        public int getInventoryPositionOfClick(int x, int y)
        {
            for (int index = 0; index < Enumerable.Count<ClickableComponent>((IEnumerable<ClickableComponent>)this.inventory); ++index)
            {
                if (this.inventory[index] != null && this.inventory[index].bounds.Contains(x, y))
                    return Convert.ToInt32(this.inventory[index].name);
            }
            return -1;
        }

        public Item leftClick(int x, int y, Item toPlace, bool playSound = true)
        {
            foreach (ClickableComponent clickableComponent in this.inventory)
            {
                if (clickableComponent.containsPoint(x, y))
                {
                    int num = Convert.ToInt32(clickableComponent.name);
                    if (!this.playerInventory)
                        num = capacity / rows * scrollIndex + num;
                    if (num < this.actualInventory.Count && (this.actualInventory[num] == null || this.highlightMethod(this.actualInventory[num]) || this.actualInventory[num].canStackWith(toPlace)))
                    {
                        if (this.actualInventory[num] != null)
                        {
                            Item result;
                            if (toPlace != null)
                            {
                                if (playSound)
                                {
                                    Game1.playSound("stoneStep");
                                }
                                result = Utility.addItemToInventory(toPlace, num, this.actualInventory, this.onAddItem);
                                return result;
                            }
                            if (playSound)
                            {
                                Game1.playSound("dwop");
                            }
                            result = Utility.removeItemFromInventory(num, this.actualInventory);
                            return result;
                        }
                        else if (toPlace != null)
                        {
                            if (playSound)
                            {
                                Game1.playSound("stoneStep");
                            }
                            Item result = Utility.addItemToInventory(toPlace, num, this.actualInventory, this.onAddItem);
                            return result;
                        }
                    }
                }
            }
            return toPlace;
        }

        public Vector2 snapToClickableComponent(int x, int y)
        {
            foreach (ClickableComponent clickableComponent in this.inventory)
            {
                if (clickableComponent.containsPoint(x, y))
                    return new Vector2((float)clickableComponent.bounds.X, (float)clickableComponent.bounds.Y);
            }
            return new Vector2((float)x, (float)y);
        }

        public Item getItemAt(int x, int y)
        {
            foreach (ClickableComponent c in this.inventory)
            {
                if (c.containsPoint(x, y))
                    return this.getItemFromClickableComponent(c);
            }
            return (Item)null;
        }

        public Item getItemFromClickableComponent(ClickableComponent c)
        {
            if (c != null)
            {
                int index = Convert.ToInt32(c.name);
                if (index < this.actualInventory.Count)
                    return this.actualInventory[index];
            }
            return (Item)null;
        }

        public Item rightClick(int x, int y, Item toAddTo, bool playSound = true)
        {
            foreach (ClickableComponent clickableComponent in this.inventory)
            {
                int num = Convert.ToInt32(clickableComponent.name);
                if (!this.playerInventory)
                    num = capacity / rows * scrollIndex + num;
                if (clickableComponent.containsPoint(x, y) && (this.actualInventory[num] == null || this.highlightMethod(this.actualInventory[num])) && num < this.actualInventory.Count && this.actualInventory[num] != null)
                {
                    if (this.actualInventory[num] is Tool && (toAddTo == null || toAddTo is StardewValley.Object) && (this.actualInventory[num] as Tool).canThisBeAttached((StardewValley.Object)toAddTo))
                    {
                        Item result = (this.actualInventory[num] as Tool).attach((toAddTo == null) ? null : ((StardewValley.Object)toAddTo));
                        return result;
                    }
                    if (toAddTo == null)
                    {
                        if (this.actualInventory[num].maximumStackSize() != -1)
                        {
                            if (num == Game1.player.CurrentToolIndex && this.actualInventory[num] != null && this.actualInventory[num].Stack == 1)
                            {
                                this.actualInventory[num].actionWhenStopBeingHeld(Game1.player);
                            }
                            Item one = this.actualInventory[num].getOne();
                            if (this.actualInventory[num].Stack > 1 && Game1.isOneOfTheseKeysDown(Game1.oldKBState, new InputButton[]
                            {
                                new InputButton(Keys.LeftShift)
                            }))
                            {
                                one.Stack = (int)Math.Ceiling((double)this.actualInventory[num].Stack / 2.0);
                                this.actualInventory[num].Stack = this.actualInventory[num].Stack / 2;
                            }
                            else if (this.actualInventory[num].Stack == 1)
                            {
                                this.actualInventory[num] = null;
                            }
                            else
                            {
                                this.actualInventory[num].Stack--;
                            }
                            if (this.actualInventory[num] != null && this.actualInventory[num].Stack <= 0)
                            {
                                this.actualInventory[num] = null;
                            }
                            if (playSound)
                            {
                                Game1.playSound("dwop");
                            }
                            Item result = one;
                            return result;
                        }
                    }
                    else if (this.actualInventory[num].canStackWith(toAddTo) && toAddTo.Stack < toAddTo.maximumStackSize())
                    {
                        if (Game1.isOneOfTheseKeysDown(Game1.oldKBState, new InputButton[]
                        {
                            new InputButton(Keys.LeftShift)
                        }))
                        {
                            toAddTo.Stack += (int)Math.Ceiling((double)this.actualInventory[num].Stack / 2.0);
                            this.actualInventory[num].Stack = this.actualInventory[num].Stack / 2;
                        }
                        else
                        {
                            toAddTo.Stack++;
                            this.actualInventory[num].Stack--;
                        }
                        if (playSound)
                        {
                            Game1.playSound("dwop");
                        }
                        if (this.actualInventory[num].Stack <= 0)
                        {
                            if (num == Game1.player.CurrentToolIndex)
                            {
                                this.actualInventory[num].actionWhenStopBeingHeld(Game1.player);
                            }
                            this.actualInventory[num] = null;
                        }
                        Item result = toAddTo;
                        return result;
                    }
                }
            }
            return toAddTo;
        }

        public Item hover(int x, int y, Item heldItem)
        {
            this.descriptionText = "";
            this.descriptionTitle = "";
            this.hoverText = "";
            this.hoverTitle = "";
            Item obj = (Item)null;
            foreach (ClickableComponent clickableComponent in this.inventory)
            {
                int index = Convert.ToInt32(clickableComponent.name);
                if (!this.playerInventory)
                    index = capacity / rows * scrollIndex + index;

                clickableComponent.scale = Math.Max(1f, clickableComponent.scale - 0.025f);
                if (clickableComponent.containsPoint(x, y) && index < this.actualInventory.Count && this.actualInventory[index] != null)
                {
                    return obj = this.actualInventory[index];
                }
            }
            return obj;
        }

        public override void setUpForGamePadMode()
        {
            base.setUpForGamePadMode();
            if (this.inventory == null || Enumerable.Count<ClickableComponent>((IEnumerable<ClickableComponent>)this.inventory) <= 0)
                return;
            Game1.setMousePosition(this.inventory[0].bounds.Right - this.inventory[0].bounds.Width / 8, this.inventory[0].bounds.Bottom - this.inventory[0].bounds.Height / 8);
        }

        public override int moveCursorInDirection(int direction)
        {
            Rectangle rectangle = new Rectangle(this.inventory[0].bounds.X, this.inventory[0].bounds.Y, Enumerable.Last<ClickableComponent>((IEnumerable<ClickableComponent>)this.inventory).bounds.X + Enumerable.Last<ClickableComponent>((IEnumerable<ClickableComponent>)this.inventory).bounds.Width - this.inventory[0].bounds.X, Enumerable.Last<ClickableComponent>((IEnumerable<ClickableComponent>)this.inventory).bounds.Y + Enumerable.Last<ClickableComponent>((IEnumerable<ClickableComponent>)this.inventory).bounds.Height - this.inventory[0].bounds.Y);
            if (!rectangle.Contains(Game1.getMousePosition()))
                Game1.setMousePosition(this.inventory[0].bounds.Right - this.inventory[0].bounds.Width / 8, this.inventory[0].bounds.Bottom - this.inventory[0].bounds.Height / 8);
            Point mousePosition = Game1.getMousePosition();
            switch (direction)
            {
                case 0:
                    Game1.setMousePosition(mousePosition.X, mousePosition.Y - Game1.tileSize - this.verticalGap);
                    break;
                case 1:
                    Game1.setMousePosition(mousePosition.X + Game1.tileSize + this.horizontalGap, mousePosition.Y);
                    break;
                case 2:
                    Game1.setMousePosition(mousePosition.X, mousePosition.Y + Game1.tileSize + this.verticalGap);
                    break;
                case 3:
                    Game1.setMousePosition(mousePosition.X - Game1.tileSize - this.horizontalGap, mousePosition.Y);
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
            if (direction > 0) scrollIndex--;
            if (direction < 0) scrollIndex++;

            if (scrollIndex < 0) scrollIndex = 0;
            if (scrollIndex > ((this.actualInventory.Count() - 1) / (capacity / rows)))
                scrollIndex = ((this.actualInventory.Count() - 1) / (capacity / rows));
        }

        public override void draw(SpriteBatch b)
        {
            for (int index = 0; index < 36; ++index)
            {

                int indexOffset = index;
                if (!this.playerInventory)
                    indexOffset = 36 / 3 * scrollIndex + index;

                Vector2 vector2 = new Vector2((float)(this.xPositionOnScreen + index % (36 / 3) * Game1.tileSize + this.horizontalGap * (index % (36 / 3))), (float)(this.yPositionOnScreen + index / (36 / 3) * (Game1.tileSize + this.verticalGap) + (index / (36 / 3) - 1) * Game1.pixelZoom));
                b.Draw(Game1.menuTexture, vector2, new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 10, -1, -1)), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
                if (this.actualInventory.Count() > indexOffset && this.actualInventory[indexOffset] != null)
                    this.actualInventory[indexOffset].drawInMenu(b, vector2, this.inventory.Count() > index ? this.inventory[index].scale : 1f, !this.highlightMethod(this.actualInventory[indexOffset]) ? 0.2f : 1f, 0.865f);
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

        public static Item removeItemFromInventory(int whichItemIndex, List<Item> items)
        {
            if (whichItemIndex >= 0 && whichItemIndex < items.Count<Item>() && items[whichItemIndex] != null)
            {
                Item item = items[whichItemIndex].getOne();
                item.Stack = items[whichItemIndex].Stack;
                if (whichItemIndex == Game1.player.CurrentToolIndex && items.Equals(Game1.player.items) && item != null)
                {
                    item.actionWhenStopBeingHeld(Game1.player);
                }
                if (items.Equals(Game1.player.items))
                    items[whichItemIndex] = null;
                return item;
            }
            return null;
        }

        public static Item addItemToInventory(Item item, int position, List<Item> items, ItemGrabMenu.behaviorOnItemSelect onAddFunction = null)
        {
            if (items.Equals(Game1.player.items) && item is StardewValley.Object && (item as StardewValley.Object).specialItem)
            {
                if ((item as StardewValley.Object).bigCraftable)
                {
                    Game1.player.specialBigCraftables.Add((item as StardewValley.Object).isRecipe ? (-(item as StardewValley.Object).parentSheetIndex) : (item as StardewValley.Object).parentSheetIndex);
                }
                else
                {
                    Game1.player.specialItems.Add((item as StardewValley.Object).isRecipe ? (-(item as StardewValley.Object).parentSheetIndex) : (item as StardewValley.Object).parentSheetIndex);
                }
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
                Item item2 = items[position];
                if (position == Game1.player.CurrentToolIndex && items.Equals(Game1.player.items) && item2 != null)
                {
                    item2.actionWhenStopBeingHeld(Game1.player);
                    item.actionWhenBeingHeld(Game1.player);
                }
                items[position] = item;
                if (onAddFunction != null)
                {
                    onAddFunction(item, null);
                }
                return item2;
            }
            int num = items[position].addToStack(item.getStack());
            if (num <= 0)
            {
                return null;
            }

            item.Stack = num;
            if (onAddFunction != null)
            {
                onAddFunction(item, null);
            }
            return item;
        }
    }
}
