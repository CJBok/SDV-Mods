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
        /*********
        ** Properties
        *********/
        private ItemInventoryMenu ItemsToGrabMenu;
        private TemporaryAnimatedSprite Poof;
        private bool SnappedtoBottom = true;
        private static bool ItemsLoaded = false;

        private ClickableComponent Title;
        private ClickableComponent SortButton;
        private ClickableComponent QualityButton;
        private List<ClickableComponent> Tabs = new List<ClickableComponent>();
        private static int TabIndex = 0;
        private static int SortID = 0;
        private static ItemQuality Quality = ItemQuality.Normal;

        private TextBox Textbox;
        private Rectangle TextboxBounds;
        private TextBoxEvent TextboxEvent;
        private List<Item> InventoryItems;
        private ClickableTextureComponent UpArrow;
        private ClickableTextureComponent DownArrow;


        /*********
        ** Accessors
        *********/
        public bool ShowReceivingMenu = true;
        public bool DrawBackground = false;
        public bool ReverseGrab = false;
        public bool DestroyItemOnClick;
        public bool CanExitOnKey = true;
        public bool PlayRightClickSound;
        public bool AllowRightClick;
        public Item HoverItem;
        public static List<Item> ItemList;
        public static string TempText = "";

        public delegate void BehaviorOnItemSelect(Item item, Farmer who);


        /*********
        ** Public methods
        *********/
        public ItemMenu(List<Item> inventory)
          : base(null, true, true, 0, -50)
        {
            this.InventoryItems = inventory;
            base.MovePosition(110, Game1.viewport.Height / 2 - (650 + IClickableMenu.borderWidth * 2) / 2);

            this.Textbox = new TextBox(null, null, Game1.dialogueFont, Game1.textColor);
            this.Textbox.X = Game1.viewport.Width / 2 - Game1.tileSize * 3;
            this.Textbox.Y = Game1.viewport.Height / 2;
            this.Textbox.Width = Game1.tileSize * 8;
            this.Textbox.Height = Game1.tileSize * 3;
            this.Textbox.X = this.xPositionOnScreen + (width / 2) - (this.Textbox.Width / 2) - Game1.tileSize + 32;
            this.Textbox.Y = this.yPositionOnScreen + (height / 2) + Game1.tileSize * 2 + 40;
            this.Textbox.Selected = false;
            this.Textbox.OnEnterPressed += this.TextboxEvent;
            this.Textbox.Text = ItemMenu.TempText;
            Game1.keyboardDispatcher.Subscriber = this.Textbox;
            this.TextboxBounds = new Rectangle(this.Textbox.X, this.Textbox.Y, this.Textbox.Width, this.Textbox.Height / 3);

            this.Title = new ClickableComponent(new Rectangle(this.xPositionOnScreen + width - Game1.tileSize, this.yPositionOnScreen - Game1.tileSize * 2, Game1.tileSize * 4, Game1.tileSize), "CJB Item Spawner");
            this.SortButton = new ClickableComponent(new Rectangle(this.xPositionOnScreen, this.yPositionOnScreen - Game1.tileSize * 2 + 10, Game1.tileSize * 4, Game1.tileSize), "Sort By: Name");
            this.QualityButton = new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize * 4, this.yPositionOnScreen - Game1.tileSize * 2 + 10, Game1.tileSize * 4, Game1.tileSize), "Quality");
            this.UpArrow = new ClickableTextureComponent("up-arrow", new Rectangle(this.xPositionOnScreen + width - Game1.tileSize / 2, this.yPositionOnScreen - Game1.tileSize, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), "", "", Game1.mouseCursors, new Rectangle(421, 459, 11, 12), (float)Game1.pixelZoom);
            this.DownArrow = new ClickableTextureComponent("down-arrow", new Rectangle(this.xPositionOnScreen + width - Game1.tileSize / 2, this.yPositionOnScreen + height / 2 - Game1.tileSize * 2, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), "", "", Game1.mouseCursors, new Rectangle(421, 472, 11, 12), (float)Game1.pixelZoom);

            {
                int i = -1;

                int x = (int)(this.xPositionOnScreen - Game1.tileSize * 5.3f);
                int y = (int)(this.yPositionOnScreen + 10);
                int lblHeight = (int)(Game1.tileSize * 0.9F);

                this.Tabs.Add(new ClickableComponent(new Rectangle(x, y + lblHeight * i++, Game1.tileSize * 5, Game1.tileSize), "All"));
                this.Tabs.Add(new ClickableComponent(new Rectangle(x, y + lblHeight * i++, Game1.tileSize * 5, Game1.tileSize), "Tools & Equipment"));
                this.Tabs.Add(new ClickableComponent(new Rectangle(x, y + lblHeight * i++, Game1.tileSize * 5, Game1.tileSize), "Seeds & Crops"));
                this.Tabs.Add(new ClickableComponent(new Rectangle(x, y + lblHeight * i++, Game1.tileSize * 5, Game1.tileSize), "Fish & Bait & Trash"));
                this.Tabs.Add(new ClickableComponent(new Rectangle(x, y + lblHeight * i++, Game1.tileSize * 5, Game1.tileSize), "Forage & Fruits"));
                this.Tabs.Add(new ClickableComponent(new Rectangle(x, y + lblHeight * i++, Game1.tileSize * 5, Game1.tileSize), "Artifacts & Minerals"));
                this.Tabs.Add(new ClickableComponent(new Rectangle(x, y + lblHeight * i++, Game1.tileSize * 5, Game1.tileSize), "Resources & Crafting"));
                this.Tabs.Add(new ClickableComponent(new Rectangle(x, y + lblHeight * i++, Game1.tileSize * 5, Game1.tileSize), "Artisan & Cooking"));
                this.Tabs.Add(new ClickableComponent(new Rectangle(x, y + lblHeight * i++, Game1.tileSize * 5, Game1.tileSize), "Animal & Monster"));
                this.Tabs.Add(new ClickableComponent(new Rectangle(x, y + lblHeight * i++, Game1.tileSize * 5, Game1.tileSize), "Decorating"));
                this.Tabs.Add(new ClickableComponent(new Rectangle(x, y + lblHeight * i++, Game1.tileSize * 5, Game1.tileSize), "Misc"));
            }

            if (!ItemMenu.ItemsLoaded)
                ItemMenu.LoadItems();

            this.AllowRightClick = true;
            this.Inventory.ShowGrayedOutSlots = true;

            switch (ItemMenu.SortID)
            {
                case 0:
                    this.SortButton.name = "Sort By: Name";
                    break;
                case 1:
                    this.SortButton.name = "Sort By: Category";
                    break;
                case 2:
                    this.SortButton.name = "Sort By: ID";
                    break;
            }

            this.LoadInventory();
        }

        private void LoadInventory()
        {
            List<Item> items = ItemMenu.ItemList.OrderBy(o => o.Name).ToList();

            switch (ItemMenu.SortID)
            {
                case 1:
                    items = ItemMenu.ItemList.OrderBy(o => o.category).ToList();
                    break;
                case 2:
                    items = ItemMenu.ItemList.OrderBy(o => o.parentSheetIndex).ToList();
                    break;
            }

            this.InventoryItems.Clear();
            foreach (Item item in items)
            {
                item.Stack = item.maximumStackSize();

                if (item is Object obj)
                    obj.quality = (int)ItemMenu.Quality;

                if (this.IsCategoryAllowed(item) && item.Name.ToLower().Contains(this.Textbox.Text.ToLower()))
                    this.InventoryItems.Add(item);
            }

            this.ItemsToGrabMenu = new ItemInventoryMenu(this.xPositionOnScreen + Game1.tileSize / 2, this.yPositionOnScreen, false, this.InventoryItems, null, -1, 3, 0, 0, true);
        }
        private bool IsCategoryAllowed(Item item)
        {
            if (ItemMenu.TabIndex == 0)
            {
                return true;
            }
            if (ItemMenu.TabIndex == 1)
            {
                return (item is Tool || item is MeleeWeapon || item.getCategoryName().Equals("Ring") || item is Hat || item is Boots);
            }
            if (ItemMenu.TabIndex == 2)
            {
                return (item.getCategoryName().Equals("Seed") || item.getCategoryName().Equals("Vegetable") || item.getCategoryName().Equals("Fertilizer") || item.getCategoryName().Equals("Flower"));
            }
            if (ItemMenu.TabIndex == 3)
            {
                return (item.getCategoryName().Equals("Fish") || item.getCategoryName().Equals("Bait") || item.getCategoryName().Equals("Trash") || item.getCategoryName().Equals("Fishing Tackle"));
            }
            if (ItemMenu.TabIndex == 4)
            {
                return (item.getCategoryName().Equals("Forage") || item.getCategoryName().Equals("Fruit"));
            }
            if (ItemMenu.TabIndex == 5)
            {
                return (item.getCategoryName().Equals("Artifact") || item.getCategoryName().Equals("Mineral"));
            }
            if (ItemMenu.TabIndex == 6)
            {
                return (item.getCategoryName().Equals("Resource") || item.getCategoryName().Equals("Crafting") || item.category == -8 || item.category == -9);
            }
            if (ItemMenu.TabIndex == 7)
            {
                return (item.getCategoryName().Equals("Artisan Goods") || item.getCategoryName().Equals("Cooking"));
            }
            if (ItemMenu.TabIndex == 8)
            {
                return (item.getCategoryName().Equals("Animal Product") || item.getCategoryName().Equals("Monster Loot"));
            }
            if (ItemMenu.TabIndex == 9)
            {
                return (item.getCategoryName().Equals("Furniture") || item.getCategoryName().Equals("Decor"));
            }
            if (ItemMenu.TabIndex == 10)
            {
                return (item.getCategoryName().Trim().Equals(""));
            }
            return false;
        }

        private static void LoadItems()
        {
            ItemMenu.ItemsLoaded = true;
            ItemMenu.ItemList = new List<Item>();

            ItemMenu.ItemList.Add(ToolFactory.getToolFromDescription(0, 0));
            ItemMenu.ItemList.Add(ToolFactory.getToolFromDescription(0, 1));
            ItemMenu.ItemList.Add(ToolFactory.getToolFromDescription(0, 2));
            ItemMenu.ItemList.Add(ToolFactory.getToolFromDescription(0, 3));
            ItemMenu.ItemList.Add(ToolFactory.getToolFromDescription(0, 4));
            ItemMenu.ItemList.Add(ToolFactory.getToolFromDescription(1, 0));
            ItemMenu.ItemList.Add(ToolFactory.getToolFromDescription(1, 1));
            ItemMenu.ItemList.Add(ToolFactory.getToolFromDescription(1, 2));
            ItemMenu.ItemList.Add(ToolFactory.getToolFromDescription(1, 3));
            ItemMenu.ItemList.Add(ToolFactory.getToolFromDescription(1, 4));
            ItemMenu.ItemList.Add(ToolFactory.getToolFromDescription(2, 0));
            ItemMenu.ItemList.Add(ToolFactory.getToolFromDescription(2, 1));
            ItemMenu.ItemList.Add(ToolFactory.getToolFromDescription(2, 2));
            ItemMenu.ItemList.Add(ToolFactory.getToolFromDescription(2, 3));
            ItemMenu.ItemList.Add(ToolFactory.getToolFromDescription(3, 0));
            ItemMenu.ItemList.Add(ToolFactory.getToolFromDescription(3, 1));
            ItemMenu.ItemList.Add(ToolFactory.getToolFromDescription(3, 2));
            ItemMenu.ItemList.Add(ToolFactory.getToolFromDescription(3, 3));
            ItemMenu.ItemList.Add(ToolFactory.getToolFromDescription(3, 4));
            ItemMenu.ItemList.Add(ToolFactory.getToolFromDescription(4, 0));
            ItemMenu.ItemList.Add(ToolFactory.getToolFromDescription(4, 1));
            ItemMenu.ItemList.Add(ToolFactory.getToolFromDescription(4, 2));
            ItemMenu.ItemList.Add(ToolFactory.getToolFromDescription(4, 3));
            ItemMenu.ItemList.Add(ToolFactory.getToolFromDescription(4, 4));
            ItemMenu.ItemList.Add(new MilkPail());
            ItemMenu.ItemList.Add(new Shears());
            ItemMenu.ItemList.Add(new Pan());

            foreach (KeyValuePair<string, string> o in CraftingRecipe.craftingRecipes)
            {
                CraftingRecipe rec = new CraftingRecipe(o.Key, false);
                Item item = rec.createItem();
                if (item != null)
                {
                    ItemMenu.ItemList.Add(item);
                }
            }

            for (int i = 0; i < 112; i++)
            {
                ItemMenu.ItemList.Add(new Wallpaper(i, false) { category = -24 });
            }

            for (int i = 0; i < 40; i++)
            {
                ItemMenu.ItemList.Add(new Wallpaper(i, true) { category = -24 });
            }

            foreach (KeyValuePair<int, string> o in Game1.content.Load<Dictionary<int, string>>("Data\\Boots"))
            {
                Item item = (Item)new Boots(o.Key);
                ItemMenu.ItemList.Add(item);
            }

            foreach (KeyValuePair<int, string> o in Game1.content.Load<Dictionary<int, string>>("Data\\hats"))
            {
                Item item = (Item)new Hat(o.Key);
                ItemMenu.ItemList.Add(item);
            }

            foreach (KeyValuePair<int, string> o in Game1.content.Load<Dictionary<int, string>>("Data\\Furniture"))
            {
                Item item = (Item)new Furniture(o.Key, Vector2.Zero);

                if (o.Key == 1466 || o.Key == 1468)
                    item = new TV(o.Key, Vector2.Zero);
                ItemMenu.ItemList.Add(item);
            }

            foreach (KeyValuePair<int, string> o in Game1.content.Load<Dictionary<int, string>>("Data\\weapons"))
            {
                Item item = (Item)new MeleeWeapon(o.Key);

                if (o.Key >= 32 && o.Key <= 34)
                {
                    item = (Item)new Slingshot(o.Key);
                }

                ItemMenu.ItemList.Add(item);
            }

            foreach (KeyValuePair<int, string> o in Game1.content.Load<Dictionary<int, string>>("Data\\Fish"))
            {
                Item item = (Item)new StardewValley.Object(o.Key, 999);
                item.category = -4;
                ItemMenu.ItemList.Add(item);
            }

            foreach (KeyValuePair<int, string> o in Game1.bigCraftablesInformation)
            {
                if (ItemMenu.HasItem(o.Key, o.Value.Split('/')[0]))
                    continue;

                ItemMenu.ItemList.Add((Item)new StardewValley.Object(Vector2.Zero, o.Key));
            }

            foreach (KeyValuePair<int, string> o in Game1.objectInformation)
            {
                if (ItemMenu.HasItem(o.Key, o.Value.Split('/')[0]))
                    continue;

                string[] info = o.Value.Split('/');
                if (info.Length >= 3)
                {
                    if (info[3].StartsWith("Ring"))
                    {
                        ItemMenu.ItemList.Add(new Ring(o.Key));
                        continue;
                    }
                    StardewValley.Object item = new StardewValley.Object(o.Key, 1);
                    ItemMenu.ItemList.Add(item);

                    if (item.category == -79)
                    {
                        StardewValley.Object obj = new StardewValley.Object(Vector2.Zero, 348, item.Name + " Wine", false, true, false, false);
                        obj.name = item.Name + " Wine";
                        obj.price = item.price * 3;
                        ItemMenu.ItemList.Add(obj);
                    }
                    if (item.category == -75)
                    {
                        StardewValley.Object obj = new StardewValley.Object(Vector2.Zero, 350, item.Name + " Juice", false, true, false, false);
                        obj.name = item.Name + " Juice";
                        obj.price = (int)(item.price * 2.25d);
                        ItemMenu.ItemList.Add(obj);
                    }

                    if (item.category == -79)
                    {
                        StardewValley.Object obj = new StardewValley.Object(Vector2.Zero, 344, item.Name + " Jelly", false, true, false, false);
                        obj.name = item.Name + " Jelly";
                        obj.price = 50 + item.Price * 2;
                        ItemMenu.ItemList.Add(obj);
                    }
                    if (item.category == -75)
                    {
                        StardewValley.Object obj = new StardewValley.Object(Vector2.Zero, 342, "Pickled " + item.Name, false, true, false, false);
                        obj.name = "Pickled " + item.Name;
                        obj.price = 50 + item.Price * 2;
                        ItemMenu.ItemList.Add(obj);
                    }
                }
            }
        }

        private static bool HasItem(int itemID, string name)
        {
            foreach (Item item in ItemMenu.ItemList)
            {
                if (item.parentSheetIndex == itemID && item.Name.Equals(name))
                    return true;
            }
            return false;
        }

        public override void receiveRightClick(int x, int y, bool playSound = true)
        {
            if (this.TextboxBounds.Contains(x, y))
            {
                this.Textbox.Text = "";
                return;
            }
            if (!this.AllowRightClick)
                return;
            base.receiveRightClick(x, y, playSound && this.PlayRightClickSound);
            if (this.HeldItem == null && this.ShowReceivingMenu)
            {
                this.HeldItem = this.ItemsToGrabMenu.RightClick(x, y, this.HeldItem, false);
                if (this.HeldItem is Object obj && obj.parentSheetIndex == 326)
                {
                    this.HeldItem = null;
                    Game1.player.canUnderstandDwarves = true;
                    this.Poof = new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 320, 64, 64), 50f, 8, 0, new Vector2((float)(x - x % Game1.tileSize + Game1.tileSize / 4), (float)(y - y % Game1.tileSize + Game1.tileSize / 4)), false, false);
                    Game1.playSound("fireball");
                }
                else if (this.HeldItem is Object recipe && recipe.isRecipe)
                {
                    string key = this.HeldItem.Name.Substring(0, recipe.Name.IndexOf("Recipe") - 1);
                    try
                    {
                        if (recipe.category == -7)
                            Game1.player.cookingRecipes.Add(key, 0);
                        else
                            Game1.player.craftingRecipes.Add(key, 0);
                        this.Poof = new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 320, 64, 64), 50f, 8, 0, new Vector2((float)(x - x % Game1.tileSize + Game1.tileSize / 4), (float)(y - y % Game1.tileSize + Game1.tileSize / 4)), false, false);
                        Game1.playSound("newRecipe");
                    }
                    catch (Exception ex)
                    {
                    }
                    this.HeldItem = (Item)null;
                }
                else
                {
                    if (!Game1.player.addItemToInventoryBool(this.HeldItem, false))
                        return;
                    this.HeldItem = (Item)null;
                    Game1.playSound("coin");
                }
            }
            else
            {
                if (!this.DestroyItemOnClick)
                    return;
                this.HeldItem = (Item)null;
            }
        }

        public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
        {
            ItemMenu.Open();
            /*if (this.SnappedtoBottom)
                this.MovePosition((newBounds.Width - oldBounds.Width) / 2, Game1.viewport.Height - (this.yPositionOnScreen + this.height - IClickableMenu.spaceToClearTopBorder));
            if (this.ItemsToGrabMenu != null)
                this.ItemsToGrabMenu.gameWindowSizeChanged(oldBounds, newBounds);*/
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            base.receiveLeftClick(x, y, !this.DestroyItemOnClick);

            if (this.HeldItem == null)
            {
                for (int i = 0; i < this.Tabs.Count(); i++)
                {
                    ClickableComponent t = this.Tabs[i];
                    int w = (int)Game1.smallFont.MeasureString(t.name).Y;
                    if (t.bounds.Contains(x, y))
                    {
                        ItemMenu.TabIndex = i;
                        Game1.exitActiveMenu();
                        ItemInventoryMenu.ScrollIndex = 0;
                        ItemMenu.Open();
                        break;
                    }
                }

                if (this.SortButton.bounds.Contains(x, y))
                {
                    ItemMenu.SortID++;
                    if (ItemMenu.SortID > 2)
                        ItemMenu.SortID = 0;
                    ItemMenu.Open();
                }

                if (this.QualityButton.bounds.Contains(x, y))
                {
                    ItemMenu.Quality = ItemMenu.Quality != ItemMenu.Quality.GetNext()
                        ? ItemMenu.Quality.GetNext()
                        : ItemQuality.Normal;
                    ItemMenu.Open();
                }

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
                if (this.HeldItem is Object obj && obj.parentSheetIndex == 326)
                {
                    this.HeldItem = null;
                    Game1.player.canUnderstandDwarves = true;
                    this.Poof = new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 320, 64, 64), 50f, 8, 0, new Vector2((float)(x - x % Game1.tileSize + Game1.tileSize / 4), (float)(y - y % Game1.tileSize + Game1.tileSize / 4)), false, false);
                    Game1.playSound("fireball");
                }
                else if (this.HeldItem is Object && (this.HeldItem as StardewValley.Object).parentSheetIndex == 102)
                {
                    this.HeldItem = (Item)null;
                    Game1.player.foundArtifact(102, 1);
                    this.Poof = new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 320, 64, 64), 50f, 8, 0, new Vector2((float)(x - x % Game1.tileSize + Game1.tileSize / 4), (float)(y - y % Game1.tileSize + Game1.tileSize / 4)), false, false);
                    Game1.playSound("fireball");
                }
                else if (this.HeldItem is Object recipe && recipe.isRecipe)
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
                    catch (Exception ex)
                    {
                    }
                    this.HeldItem = (Item)null;
                }
                else if (Game1.player.addItemToInventoryBool(this.HeldItem, false))
                {
                    this.HeldItem = (Item)null;
                    Game1.playSound("coin");
                }
            }
            else if (this.ReverseGrab)
            {
                if (this.DestroyItemOnClick)
                {
                    this.HeldItem = (Item)null;
                    return;
                }
            }
            if (this.HeldItem == null || this.isWithinBounds(x, y) || !this.HeldItem.canBeTrashed())
                return;
            Game1.playSound("throwDownITem");
            Game1.createItemDebris(this.HeldItem, Game1.player.getStandingPosition(), Game1.player.FacingDirection);
            this.HeldItem = (Item)null;

        }

        public static void OrganizeItemsInList(List<Item> items)
        {
            items.Sort();
            items.Reverse();
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
            if (this.Textbox.Selected)
            {
                return;
            }

            if ((this.CanExitOnKey || this.AreAllItemsTaken()) && (Game1.options.doesInputListContain(Game1.options.menuButton, key) && this.readyToClose()))
            {
                this.exitThisMenu(true);
                if (Game1.currentLocation.currentEvent != null)
                    ++Game1.currentLocation.currentEvent.CurrentCommand;
            }
            else if (Game1.options.doesInputListContain(Game1.options.menuButton, key) && this.HeldItem != null)
                Game1.setMousePosition(this.TrashCan.bounds.Center);
            if (key != Keys.Delete || this.HeldItem == null || !this.HeldItem.canBeTrashed())
                return;
            if (this.HeldItem is Object obj && Game1.player.specialItems.Contains(obj.parentSheetIndex))
                Game1.player.specialItems.Remove(obj.parentSheetIndex);
            this.HeldItem = null;
            Game1.playSound("trashcan");
        }

        public override void update(GameTime time)
        {
            if (!ItemMenu.TempText.Equals(this.Textbox.Text))
            {
                ItemMenu.TempText = this.Textbox.Text;
                ItemInventoryMenu.ScrollIndex = 0;
                this.LoadInventory();
            }

            //this.Textbox.Update();
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

            if (this.TextboxBounds.Contains(x, y))
            {
                this.Textbox.Selected = true;
            }
            else
            {
                this.Textbox.Selected = false;
            }

        }

        public override void receiveScrollWheelAction(int direction)
        {
            if (GameMenu.forcePreventClose)
                return;

            if (this.HeldItem == null && this.HoveredItem != null && Game1.oldKBState.IsKeyDown(Keys.LeftShift))
            {
                try
                {
                    Object obj = (Object)this.HoveredItem;
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

        public override void draw(SpriteBatch spriteBatch)
        {
            if (!Game1.options.showMenuBackground)
            {
                spriteBatch.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.8f);
            }
            this.Textbox.Draw(spriteBatch);
            this.Draw(spriteBatch, false, false);
            if (this.ShowReceivingMenu)
            {
                CJB.DrawTextBox(this.Title.bounds.X, this.Title.bounds.Y, Game1.borderFont, this.Title.name, true, 2, 1.0f);
                Game1.drawDialogueBox(this.ItemsToGrabMenu.xPositionOnScreen - IClickableMenu.borderWidth - IClickableMenu.spaceToClearSideBorder, this.ItemsToGrabMenu.yPositionOnScreen - IClickableMenu.borderWidth - IClickableMenu.spaceToClearTopBorder, this.ItemsToGrabMenu.width + IClickableMenu.borderWidth * 2 + IClickableMenu.spaceToClearSideBorder * 2, this.ItemsToGrabMenu.height + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth * 2, false, true, (string)null, false);
                this.ItemsToGrabMenu.draw(spriteBatch);
                for (int i = 0; i < this.Tabs.Count(); i++)
                {
                    ClickableComponent current = this.Tabs[i];
                    CJB.DrawTextBox(current.bounds.X + current.bounds.Width, current.bounds.Y, Game1.smallFont, current.name, true, 2, ItemMenu.TabIndex == i ? 1F : 0.7F);
                }

                CJB.DrawTextBox(this.SortButton.bounds.X, this.SortButton.bounds.Y, Game1.smallFont, this.SortButton.name, true, 0, 1F);
                CJB.DrawTextBox(this.QualityButton.bounds.X, this.QualityButton.bounds.Y, Game1.smallFont, this.QualityButton.name, true, 0, 1F);

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

            if (this.HoveredItem is Object obj)
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
            Game1.activeClickableMenu = new ItemMenu(new List<Item>());
        }

        private void DrawHoverTextBox(SpriteBatch spriteBatch, SpriteFont font, int price, int stack = -1)
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

            Utility.drawTextWithShadow(spriteBatch, p, font, new Vector2((float)(x + width + Game1.tileSize / 4 - 60 - font.MeasureString(p).X), (float)(y + Game1.tileSize / 4)), Game1.textColor, 1f, -1f, -1, -1, 1F, 3);
            if (stack > 1)
                Utility.drawTextWithShadow(spriteBatch, ps, font, new Vector2((float)(x + width + Game1.tileSize / 4 - 60 - font.MeasureString(ps).X), (float)(y + Game1.tileSize / 4)), Game1.textColor, 1f, -1f, -1, -1, 1F, 3);
            //spriteBatch.End();
        }
    }
}
