using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Tools;
using SObject = StardewValley.Object;

namespace CJBItemSpawner.Framework
{
    internal class ItemInventoryMenu : IClickableMenu
    {
        /*********
        ** Fields
        *********/
        private readonly bool PlayerInventory;
        private readonly List<ClickableComponent> Inventory = new List<ClickableComponent>();
        private readonly int Capacity;
        private readonly int Rows;
        private readonly int HorizontalGap;
        private readonly int VerticalGap;


        /*********
        ** Accessors
        *********/
        public string HoverText = "";
        public string DescriptionTitle = "";
        public string DescriptionText = "";
        public IList<Item> ActualInventory;
        public InventoryMenu.highlightThisItem HighlightMethod;
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
                this.ActualInventory = Game1.player.Items;

            for (int index = 0; index < Game1.player.MaxItems; ++index)
            {
                if (Game1.player.Items.Count <= index)
                    Game1.player.Items.Add(null);
            }
            this.PlayerInventory = object.ReferenceEquals(this.ActualInventory, Game1.player.Items);

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
                                return this.AddItemToInventory(toPlace, index, this.ActualInventory);
                            }
                            if (playSound)
                                Game1.playSound("dwop");
                            return this.RemoveItemFromInventory(index, this.ActualInventory);
                        }
                        if (toPlace != null)
                        {
                            if (playSound)
                                Game1.playSound("stoneStep");
                            return this.AddItemToInventory(toPlace, index, this.ActualInventory);
                        }
                    }
                }
            }
            return toPlace;
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
                    if (this.ActualInventory[index] is Tool tool && (toAddTo == null || toAddTo is SObject) && tool.canThisBeAttached((SObject)toAddTo))
                        return tool.attach((SObject)toAddTo);
                    if (toAddTo == null)
                    {
                        if (this.ActualInventory[index].maximumStackSize() != -1)
                        {
                            if (index == Game1.player.CurrentToolIndex && this.ActualInventory[index] != null && this.ActualInventory[index].Stack == 1)
                                this.ActualInventory[index].actionWhenStopBeingHeld(Game1.player);
                            Item one = this.GetOne(this.ActualInventory[index]);
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

        public override void receiveRightClick(int x, int y, bool playSound = true) { }


        /*********
        ** Private methods
        *********/
        private Item AddItemToInventory(Item item, int position, IList<Item> items, ItemGrabMenu.behaviorOnItemSelect onAddFunction = null)
        {
            if (object.ReferenceEquals(items, Game1.player.Items) && item is SObject obj && obj.specialItem)
            {
                if (obj.bigCraftable.Value)
                    Game1.player.specialBigCraftables.Add(obj.IsRecipe ? -obj.ParentSheetIndex : obj.ParentSheetIndex);
                else
                    Game1.player.specialItems.Add(obj.IsRecipe ? -obj.ParentSheetIndex : obj.ParentSheetIndex);
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
            if (items[position].maximumStackSize() == -1 || items[position].Name != item.Name || (item is SObject itemObj && items[position] is SObject slotObj && (itemObj.Quality != slotObj.Quality || itemObj.ParentSheetIndex != slotObj.ParentSheetIndex)) || !item.canStackWith(items[position]))
            {
                Item item2 = items[position];
                if (position == Game1.player.CurrentToolIndex && object.ReferenceEquals(items, Game1.player.Items) && item2 != null)
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

        private Item RemoveItemFromInventory(int whichItemIndex, IList<Item> items)
        {
            if (whichItemIndex >= 0 && whichItemIndex < items.Count && items[whichItemIndex] != null)
            {
                Item item = this.PlayerInventory
                    ? items[whichItemIndex]
                    : this.GetOne(items[whichItemIndex]);
                item.Stack = items[whichItemIndex].Stack;
                if (whichItemIndex == Game1.player.CurrentToolIndex && object.ReferenceEquals(items, Game1.player.Items))
                    item.actionWhenStopBeingHeld(Game1.player);
                if (object.ReferenceEquals(items, Game1.player.Items))
                    items[whichItemIndex] = null;
                return item;
            }
            return null;
        }

        /// <summary>Get a new instance of an item.</summary>
        /// <param name="item">The item to clone.</param>
        private Item GetOne(Item item)
        {
            switch (item)
            {
                case Slingshot slingshot:
                    return new Slingshot(slingshot.InitialParentTileIndex); // slingshot.getOne() always returns basic slingshot

                default:
                    return item.getOne();
            }
        }
    }
}
