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
using StardewValley.Objects;
using StardewValley.Tools;
using Object = StardewValley.Object;

namespace CJBItemSpawner
{
    public class ItemMenu : ItemMenuWithInventory
    {
        public bool showReceivingMenu = true;
        public bool drawBG = false;
        private ItemInventoryMenu ItemsToGrabMenu;
        private TemporaryAnimatedSprite poof;
        public bool reverseGrab = false;
        public bool destroyItemOnClick;
        public bool canExitOnKey = true;
        public bool playRightClickSound;
        public bool allowRightClick;
        public Item hoverItem;
        private bool snappedtoBottom = true;
        public static List<Item> itemList;
        private static bool itemsLoaded = false;

        private ClickableComponent title;
        private ClickableComponent sortButton;
        private ClickableComponent qualityButton;
        private List<ClickableComponent> tabs = new List<ClickableComponent>();
        private static int tab = 0;
        private static int sortID = 0;
        private static ItemQuality quality = ItemQuality.Normal;

        private TextBox textBox;
        private Rectangle textBoxBounds;
        private TextBoxEvent e;
        public static string tempText = "";

        private List<Item> inventoryItems;
        private ClickableTextureComponent upArrow;
        private ClickableTextureComponent downArrow;

        public ItemMenu(List<Item> inventory)
          : base(null, true, true, 0, -50)
        {
            this.inventoryItems = inventory;
            base.movePosition(110, Game1.viewport.Height / 2 - (650 + IClickableMenu.borderWidth * 2) / 2);

            this.textBox = new TextBox(null, null, Game1.dialogueFont, Game1.textColor);
            this.textBox.X = Game1.viewport.Width / 2 - Game1.tileSize * 3;
            this.textBox.Y = Game1.viewport.Height / 2;
            this.textBox.Width = Game1.tileSize * 8;
            this.textBox.Height = Game1.tileSize * 3;
            this.textBox.X = this.xPositionOnScreen + (width / 2) - (this.textBox.Width / 2) - Game1.tileSize + 32;
            this.textBox.Y = this.yPositionOnScreen + (height / 2) + Game1.tileSize * 2 + 40;
            this.textBox.Selected = false;
            this.textBox.OnEnterPressed += this.e;
            this.textBox.Text = tempText;
            Game1.keyboardDispatcher.Subscriber = this.textBox;
            this.textBoxBounds = new Rectangle(this.textBox.X, this.textBox.Y, this.textBox.Width, this.textBox.Height / 3);

            title = new ClickableComponent(new Rectangle(this.xPositionOnScreen + width - Game1.tileSize, this.yPositionOnScreen - Game1.tileSize * 2, Game1.tileSize * 4, Game1.tileSize), "CJB Item Spawner");
            sortButton = new ClickableComponent(new Rectangle(this.xPositionOnScreen, this.yPositionOnScreen - Game1.tileSize * 2 + 10, Game1.tileSize * 4, Game1.tileSize), "Sort By: Name");
            qualityButton = new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize * 4, this.yPositionOnScreen - Game1.tileSize * 2 + 10, Game1.tileSize * 4, Game1.tileSize), "Quality");
            this.upArrow = new ClickableTextureComponent("up-arrow", new Rectangle(this.xPositionOnScreen + width - Game1.tileSize / 2, this.yPositionOnScreen - Game1.tileSize, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), "", "", Game1.mouseCursors, new Rectangle(421, 459, 11, 12), (float)Game1.pixelZoom);
            this.downArrow = new ClickableTextureComponent("down-arrow", new Rectangle(this.xPositionOnScreen + width - Game1.tileSize / 2, this.yPositionOnScreen + height / 2 - Game1.tileSize * 2, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), "", "", Game1.mouseCursors, new Rectangle(421, 472, 11, 12), (float)Game1.pixelZoom);

            int i = -1;

            int x = (int)(this.xPositionOnScreen - Game1.tileSize * 5.3f);
            int y = (int)(this.yPositionOnScreen + 10);
            int lblHeight = (int)(Game1.tileSize * 0.9F);

            this.tabs.Add(new ClickableComponent(new Rectangle(x, y + lblHeight * i++, Game1.tileSize * 5, Game1.tileSize), "All"));
            this.tabs.Add(new ClickableComponent(new Rectangle(x, y + lblHeight * i++, Game1.tileSize * 5, Game1.tileSize), "Tools & Equipment"));
            this.tabs.Add(new ClickableComponent(new Rectangle(x, y + lblHeight * i++, Game1.tileSize * 5, Game1.tileSize), "Seeds & Crops"));
            this.tabs.Add(new ClickableComponent(new Rectangle(x, y + lblHeight * i++, Game1.tileSize * 5, Game1.tileSize), "Fish & Bait & Trash"));
            this.tabs.Add(new ClickableComponent(new Rectangle(x, y + lblHeight * i++, Game1.tileSize * 5, Game1.tileSize), "Forage & Fruits"));
            this.tabs.Add(new ClickableComponent(new Rectangle(x, y + lblHeight * i++, Game1.tileSize * 5, Game1.tileSize), "Artifacts & Minerals"));
            this.tabs.Add(new ClickableComponent(new Rectangle(x, y + lblHeight * i++, Game1.tileSize * 5, Game1.tileSize), "Resources & Crafting"));
            this.tabs.Add(new ClickableComponent(new Rectangle(x, y + lblHeight * i++, Game1.tileSize * 5, Game1.tileSize), "Artisan & Cooking"));
            this.tabs.Add(new ClickableComponent(new Rectangle(x, y + lblHeight * i++, Game1.tileSize * 5, Game1.tileSize), "Animal & Monster"));
            this.tabs.Add(new ClickableComponent(new Rectangle(x, y + lblHeight * i++, Game1.tileSize * 5, Game1.tileSize), "Decorating"));
            this.tabs.Add(new ClickableComponent(new Rectangle(x, y + lblHeight * i++, Game1.tileSize * 5, Game1.tileSize), "Misc"));

            if (!itemsLoaded)
                loadItems();

            this.allowRightClick = true;
            this.inventory.showGrayedOutSlots = true;

            switch (sortID)
            {
                case 0:
                    sortButton.name = "Sort By: Name";
                    break;
                case 1:
                    sortButton.name = "Sort By: Category";
                    break;
                case 2:
                    sortButton.name = "Sort By: ID";
                    break;
            }

            loadInventory();
        }

        private void loadInventory()
        {
            List<Item> tempItems = itemList.OrderBy(o => o.Name).ToList();

            switch (sortID)
            {
                case 1:
                    tempItems = itemList.OrderBy(o => o.category).ToList();
                    break;
                case 2:
                    tempItems = itemList.OrderBy(o => o.parentSheetIndex).ToList();
                    break;
            }

            this.inventoryItems.Clear();
            foreach (Item item in tempItems)
            {
                item.Stack = item.maximumStackSize();

                if (item is Object)
                    ((Object)item).quality = (int)quality;

                if (isCategoryAllowed(item) && item.Name.ToLower().Contains(this.textBox.Text.ToLower()))
                    this.inventoryItems.Add(item);
            }

            this.ItemsToGrabMenu = new ItemInventoryMenu(this.xPositionOnScreen + Game1.tileSize / 2, this.yPositionOnScreen, false, this.inventoryItems, null, -1, 3, 0, 0, true);
        }
        private bool isCategoryAllowed(Item i)
        {
            if (tab == 0)
            {
                return true;
            }
            if (tab == 1)
            {
                return (i is Tool || i is MeleeWeapon || i.getCategoryName().Equals("Ring") || i is Hat || i is Boots);
            }
            if (tab == 2)
            {
                return (i.getCategoryName().Equals("Seed") || i.getCategoryName().Equals("Vegetable") || i.getCategoryName().Equals("Fertilizer") || i.getCategoryName().Equals("Flower"));
            }
            if (tab == 3)
            {
                return (i.getCategoryName().Equals("Fish") || i.getCategoryName().Equals("Bait") || i.getCategoryName().Equals("Trash") || i.getCategoryName().Equals("Fishing Tackle"));
            }
            if (tab == 4)
            {
                return (i.getCategoryName().Equals("Forage") || i.getCategoryName().Equals("Fruit"));
            }
            if (tab == 5)
            {
                return (i.getCategoryName().Equals("Artifact") || i.getCategoryName().Equals("Mineral"));
            }
            if (tab == 6)
            {
                return (i.getCategoryName().Equals("Resource") || i.getCategoryName().Equals("Crafting") || i.category == -8 || i.category == -9);
            }
            if (tab == 7)
            {
                return (i.getCategoryName().Equals("Artisan Goods") || i.getCategoryName().Equals("Cooking"));
            }
            if (tab == 8)
            {
                return (i.getCategoryName().Equals("Animal Product") || i.getCategoryName().Equals("Monster Loot"));
            }
            if (tab == 9)
            {
                return (i.getCategoryName().Equals("Furniture") || i.getCategoryName().Equals("Decor"));
            }
            if (tab == 10)
            {
                return (i.getCategoryName().Trim().Equals(""));
            }
            return false;
        }

        private static void loadItems()
        {
            itemsLoaded = true;
            itemList = new List<Item>();

            itemList.Add(ToolFactory.getToolFromDescription(0, 0));
            itemList.Add(ToolFactory.getToolFromDescription(0, 1));
            itemList.Add(ToolFactory.getToolFromDescription(0, 2));
            itemList.Add(ToolFactory.getToolFromDescription(0, 3));
            itemList.Add(ToolFactory.getToolFromDescription(0, 4));
            itemList.Add(ToolFactory.getToolFromDescription(1, 0));
            itemList.Add(ToolFactory.getToolFromDescription(1, 1));
            itemList.Add(ToolFactory.getToolFromDescription(1, 2));
            itemList.Add(ToolFactory.getToolFromDescription(1, 3));
            itemList.Add(ToolFactory.getToolFromDescription(1, 4));
            itemList.Add(ToolFactory.getToolFromDescription(2, 0));
            itemList.Add(ToolFactory.getToolFromDescription(2, 1));
            itemList.Add(ToolFactory.getToolFromDescription(2, 2));
            itemList.Add(ToolFactory.getToolFromDescription(2, 3));
            itemList.Add(ToolFactory.getToolFromDescription(3, 0));
            itemList.Add(ToolFactory.getToolFromDescription(3, 1));
            itemList.Add(ToolFactory.getToolFromDescription(3, 2));
            itemList.Add(ToolFactory.getToolFromDescription(3, 3));
            itemList.Add(ToolFactory.getToolFromDescription(3, 4));
            itemList.Add(ToolFactory.getToolFromDescription(4, 0));
            itemList.Add(ToolFactory.getToolFromDescription(4, 1));
            itemList.Add(ToolFactory.getToolFromDescription(4, 2));
            itemList.Add(ToolFactory.getToolFromDescription(4, 3));
            itemList.Add(ToolFactory.getToolFromDescription(4, 4));
            itemList.Add(new MilkPail());
            itemList.Add(new Shears());
            itemList.Add(new Pan());

            foreach (KeyValuePair<string, string> o in CraftingRecipe.craftingRecipes)
            {
                CraftingRecipe rec = new CraftingRecipe(o.Key, false);
                Item item = rec.createItem();
                if (item != null)
                {
                    itemList.Add(item);
                }
            }

            for (int i = 0; i < 112; i++)
            {
                itemList.Add(new Wallpaper(i, false) { category = -24 });
            }

            for (int i = 0; i < 40; i++)
            {
                itemList.Add(new Wallpaper(i, true) { category = -24 });
            }

            foreach (KeyValuePair<int, string> o in Game1.content.Load<Dictionary<int, string>>("Data\\Boots"))
            {
                Item item = (Item)new Boots(o.Key);
                itemList.Add(item);
            }

            foreach (KeyValuePair<int, string> o in Game1.content.Load<Dictionary<int, string>>("Data\\hats"))
            {
                Item item = (Item)new Hat(o.Key);
                itemList.Add(item);
            }

            foreach (KeyValuePair<int, string> o in Game1.content.Load<Dictionary<int, string>>("Data\\Furniture"))
            {
                Item item = (Item)new Furniture(o.Key, Vector2.Zero);

                if (o.Key == 1466 || o.Key == 1468)
                    item = new TV(o.Key, Vector2.Zero);
                itemList.Add(item);
            }

            foreach (KeyValuePair<int, string> o in Game1.content.Load<Dictionary<int, string>>("Data\\weapons"))
            {
                Item item = (Item)new MeleeWeapon(o.Key);

                if (o.Key >= 32 && o.Key <= 34)
                {
                    item = (Item)new Slingshot(o.Key);
                }

                itemList.Add(item);
            }

            foreach (KeyValuePair<int, string> o in Game1.content.Load<Dictionary<int, string>>("Data\\Fish"))
            {
                Item item = (Item)new StardewValley.Object(o.Key, 999);
                item.category = -4;
                itemList.Add(item);
            }

            foreach (KeyValuePair<int, string> o in Game1.bigCraftablesInformation)
            {
                if (hasId(o.Key, o.Value.Split('/')[0]))
                    continue;

                itemList.Add((Item)new StardewValley.Object(Vector2.Zero, o.Key));
            }

            foreach (KeyValuePair<int, string> o in Game1.objectInformation)
            {
                if (hasId(o.Key, o.Value.Split('/')[0]))
                    continue;

                string[] info = o.Value.Split('/');
                if (info.Length >= 3)
                {
                    if (info[3].StartsWith("Ring"))
                    {
                        itemList.Add(new Ring(o.Key));
                        continue;
                    }
                    StardewValley.Object item = new StardewValley.Object(o.Key, 1);
                    itemList.Add(item);

                    if (item.category == -79)
                    {
                        StardewValley.Object obj = new StardewValley.Object(Vector2.Zero, 348, item.Name + " Wine", false, true, false, false);
                        obj.name = item.Name + " Wine";
                        obj.price = item.price * 3;
                        itemList.Add(obj);
                    }
                    if (item.category == -75)
                    {
                        StardewValley.Object obj = new StardewValley.Object(Vector2.Zero, 350, item.Name + " Juice", false, true, false, false);
                        obj.name = item.Name + " Juice";
                        obj.price = (int)(item.price * 2.25d);
                        itemList.Add(obj);
                    }

                    if (item.category == -79)
                    {
                        StardewValley.Object obj = new StardewValley.Object(Vector2.Zero, 344, item.Name + " Jelly", false, true, false, false);
                        obj.name = item.Name + " Jelly";
                        obj.price = 50 + item.Price * 2;
                        itemList.Add(obj);
                    }
                    if (item.category == -75)
                    {
                        StardewValley.Object obj = new StardewValley.Object(Vector2.Zero, 342, "Pickled " + item.Name, false, true, false, false);
                        obj.name = "Pickled " + item.Name;
                        obj.price = 50 + item.Price * 2;
                        itemList.Add(obj);
                    }
                }
            }
        }

        private static bool hasId(int i, string name)
        {
            foreach (Item item in itemList)
            {
                if (item.parentSheetIndex == i && item.Name.Equals(name))
                    return true;
            }
            return false;
        }

        public override void receiveRightClick(int x, int y, bool playSound = true)
        {
            if (this.textBoxBounds.Contains(x, y))
            {
                this.textBox.Text = "";
                return;
            }
            if (!this.allowRightClick)
                return;
            base.receiveRightClick(x, y, playSound && this.playRightClickSound);
            if (this.heldItem == null && this.showReceivingMenu)
            {
                this.heldItem = this.ItemsToGrabMenu.rightClick(x, y, this.heldItem, false);
                if (this.heldItem is StardewValley.Object && (this.heldItem as StardewValley.Object).parentSheetIndex == 326)
                {
                    this.heldItem = (Item)null;
                    Game1.player.canUnderstandDwarves = true;
                    this.poof = new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 320, 64, 64), 50f, 8, 0, new Vector2((float)(x - x % Game1.tileSize + Game1.tileSize / 4), (float)(y - y % Game1.tileSize + Game1.tileSize / 4)), false, false);
                    Game1.playSound("fireball");
                }
                else if (this.heldItem is StardewValley.Object && (this.heldItem as StardewValley.Object).isRecipe)
                {
                    string key = this.heldItem.Name.Substring(0, this.heldItem.Name.IndexOf("Recipe") - 1);
                    try
                    {
                        if ((this.heldItem as StardewValley.Object).category == -7)
                            Game1.player.cookingRecipes.Add(key, 0);
                        else
                            Game1.player.craftingRecipes.Add(key, 0);
                        this.poof = new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 320, 64, 64), 50f, 8, 0, new Vector2((float)(x - x % Game1.tileSize + Game1.tileSize / 4), (float)(y - y % Game1.tileSize + Game1.tileSize / 4)), false, false);
                        Game1.playSound("newRecipe");
                    }
                    catch (Exception ex)
                    {
                    }
                    this.heldItem = (Item)null;
                }
                else
                {
                    if (!Game1.player.addItemToInventoryBool(this.heldItem, false))
                        return;
                    this.heldItem = (Item)null;
                    Game1.playSound("coin");
                }
            }
            else
            {
                if (!this.destroyItemOnClick)
                    return;
                this.heldItem = (Item)null;
            }
        }

        public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
        {
            Open();
            /*if (this.snappedtoBottom)
                this.movePosition((newBounds.Width - oldBounds.Width) / 2, Game1.viewport.Height - (this.yPositionOnScreen + this.height - IClickableMenu.spaceToClearTopBorder));
            if (this.ItemsToGrabMenu != null)
                this.ItemsToGrabMenu.gameWindowSizeChanged(oldBounds, newBounds);*/
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            base.receiveLeftClick(x, y, !this.destroyItemOnClick);

            if (this.heldItem == null)
            {
                for (int i = 0; i < tabs.Count(); i++)
                {
                    ClickableComponent t = this.tabs[i];
                    int w = (int)Game1.smallFont.MeasureString(t.name).Y;
                    if (t.bounds.Contains(x, y))
                    {
                        tab = i;
                        Game1.exitActiveMenu();
                        ItemInventoryMenu.scrollIndex = 0;
                        Open();
                        break;
                    }
                }

                if (sortButton.bounds.Contains(x, y))
                {
                    sortID++;
                    if (sortID > 2)
                        sortID = 0;
                    Open();
                }

                if (qualityButton.bounds.Contains(x, y))
                {
                    quality = quality != quality.GetNext()
                        ? quality.GetNext()
                        : ItemQuality.Normal;
                    Open();
                }

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
                if (this.heldItem is StardewValley.Object && (this.heldItem as StardewValley.Object).parentSheetIndex == 326)
                {
                    this.heldItem = (Item)null;
                    Game1.player.canUnderstandDwarves = true;
                    this.poof = new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 320, 64, 64), 50f, 8, 0, new Vector2((float)(x - x % Game1.tileSize + Game1.tileSize / 4), (float)(y - y % Game1.tileSize + Game1.tileSize / 4)), false, false);
                    Game1.playSound("fireball");
                }
                else if (this.heldItem is StardewValley.Object && (this.heldItem as StardewValley.Object).parentSheetIndex == 102)
                {
                    this.heldItem = (Item)null;
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
                            Game1.player.cookingRecipes.Add(key, 0);
                        else
                            Game1.player.craftingRecipes.Add(key, 0);
                        this.poof = new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 320, 64, 64), 50f, 8, 0, new Vector2((float)(x - x % Game1.tileSize + Game1.tileSize / 4), (float)(y - y % Game1.tileSize + Game1.tileSize / 4)), false, false);
                        Game1.playSound("newRecipe");
                    }
                    catch (Exception ex)
                    {
                    }
                    this.heldItem = (Item)null;
                }
                else if (Game1.player.addItemToInventoryBool(this.heldItem, false))
                {
                    this.heldItem = (Item)null;
                    Game1.playSound("coin");
                }
            }
            else if (this.reverseGrab)
            {
                if (this.destroyItemOnClick)
                {
                    this.heldItem = (Item)null;
                    return;
                }
            }
            if (this.heldItem == null || this.isWithinBounds(x, y) || !this.heldItem.canBeTrashed())
                return;
            Game1.playSound("throwDownITem");
            Game1.createItemDebris(this.heldItem, Game1.player.getStandingPosition(), Game1.player.FacingDirection);
            this.heldItem = (Item)null;

        }

        public static void organizeItemsInList(List<Item> items)
        {
            items.Sort();
            items.Reverse();
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
            if (this.textBox.Selected)
            {
                return;
            }

            if ((this.canExitOnKey || this.areAllItemsTaken()) && (Game1.options.doesInputListContain(Game1.options.menuButton, key) && this.readyToClose()))
            {
                this.exitThisMenu(true);
                if (Game1.currentLocation.currentEvent != null)
                    ++Game1.currentLocation.currentEvent.CurrentCommand;
            }
            else if (Game1.options.doesInputListContain(Game1.options.menuButton, key) && this.heldItem != null)
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
            if (!tempText.Equals(this.textBox.Text))
            {
                tempText = this.textBox.Text;
                ItemInventoryMenu.scrollIndex = 0;
                loadInventory();
            }

            //this.textBox.Update();
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

            if (this.textBoxBounds.Contains(x, y))
            {
                this.textBox.Selected = true;
            }
            else
            {
                this.textBox.Selected = false;
            }

        }

        public override void receiveScrollWheelAction(int direction)
        {
            if (GameMenu.forcePreventClose)
                return;

            if (this.heldItem == null && this.hoveredItem != null && Game1.oldKBState.IsKeyDown(Keys.LeftShift))
            {
                try
                {
                    Object obj = (Object)hoveredItem;
                    obj.quality = direction > 0
                        ? (int)((ItemQuality)obj.quality).GetNext()
                        : (int)((ItemQuality)obj.quality).GetPrevious();
                }
                catch (Exception) { }

                return;
            }

            if (this.ItemsToGrabMenu != null)
                this.ItemsToGrabMenu.receiveScrollWheelAction(direction);
        }

        public override void draw(SpriteBatch b)
        {
            if (!Game1.options.showMenuBackground)
            {
                b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.8f);
            }
            textBox.Draw(b);
            this.draw(b, false, false);
            if (this.showReceivingMenu)
            {
                CJB.drawTextBox(title.bounds.X, title.bounds.Y, Game1.borderFont, title.name, true, 2, 1.0f);
                Game1.drawDialogueBox(this.ItemsToGrabMenu.xPositionOnScreen - IClickableMenu.borderWidth - IClickableMenu.spaceToClearSideBorder, this.ItemsToGrabMenu.yPositionOnScreen - IClickableMenu.borderWidth - IClickableMenu.spaceToClearTopBorder, this.ItemsToGrabMenu.width + IClickableMenu.borderWidth * 2 + IClickableMenu.spaceToClearSideBorder * 2, this.ItemsToGrabMenu.height + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth * 2, false, true, (string)null, false);
                this.ItemsToGrabMenu.draw(b);
                for (int i = 0; i < tabs.Count(); i++)
                {
                    ClickableComponent current = tabs[i];
                    CJB.drawTextBox(current.bounds.X + current.bounds.Width, current.bounds.Y, Game1.smallFont, current.name, true, 2, tab == i ? 1F : 0.7F);
                }

                CJB.drawTextBox(sortButton.bounds.X, sortButton.bounds.Y, Game1.smallFont, sortButton.name, true, 0, 1F);
                CJB.drawTextBox(qualityButton.bounds.X, qualityButton.bounds.Y, Game1.smallFont, qualityButton.name, true, 0, 1F);

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

        public delegate void behaviorOnItemSelect(Item item, Farmer who);

        public static void Open()
        {
            Game1.activeClickableMenu = new ItemMenu(new List<Item>());
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
