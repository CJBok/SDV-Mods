﻿using System;
using System.Collections.Generic;
using System.Linq;
using CJBItemSpawner.Framework.Constants;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Objects;
using StardewValley.Tools;
using SObject = StardewValley.Object;

namespace CJBItemSpawner.Framework
{
    internal class ItemMenu : ItemMenuWithInventory
    {
        /*********
        ** Properties
        *********/
        private readonly ITranslationHelper TranslationHelper;
        private readonly Item[] SpawnableItems;
        private readonly ClickableComponent Title;
        private readonly ClickableComponent SortButton;
        private readonly ClickableComponent QualityButton;
        private readonly ClickableTextureComponent UpArrow;
        private readonly ClickableTextureComponent DownArrow;
        private readonly List<ClickableComponent> Tabs = new List<ClickableComponent>();
        private readonly TextBox Textbox;
        private readonly bool AllowRightClick;
        private readonly MenuTab CurrentTab;
        private readonly int SortID;
        private readonly ItemQuality Quality;
        private readonly bool ShowReceivingMenu = true;
        private readonly bool CanExitOnKey = true;

        private ItemInventoryMenu ItemsToGrabMenu;
        private TemporaryAnimatedSprite Poof;
        private Rectangle TextboxBounds;
        private string TempText = "";


        /*********
        ** Public methods
        *********/
        public ItemMenu(MenuTab currentTab, int sortID, ItemQuality quality, ITranslationHelper i18n)
          : base(null, true, true, 0, -50)
        {
            // initialise
            this.TranslationHelper = i18n;
            this.MovePosition(110, Game1.viewport.Height / 2 - (650 + IClickableMenu.borderWidth * 2) / 2);
            this.CurrentTab = currentTab;
            this.SortID = sortID;
            this.Quality = quality;
            this.SpawnableItems = this.GetSpawnableItems().ToArray();
            this.AllowRightClick = true;

            // create search box
            int textWidth = Game1.tileSize * 8;
            this.Textbox = new TextBox(null, null, Game1.dialogueFont, Game1.textColor)
            {
                X = this.xPositionOnScreen + (this.width / 2) - (textWidth / 2) - Game1.tileSize + 32,
                Y = this.yPositionOnScreen + (this.height / 2) + Game1.tileSize * 2 + 40,
                Width = textWidth,
                Height = Game1.tileSize * 3,
                Selected = false,
                Text = this.TempText
            };
            Game1.keyboardDispatcher.Subscriber = this.Textbox;
            this.TextboxBounds = new Rectangle(this.Textbox.X, this.Textbox.Y, this.Textbox.Width, this.Textbox.Height / 3);

            // create buttons
            this.Title = new ClickableComponent(new Rectangle(this.xPositionOnScreen + this.width - Game1.tileSize, this.yPositionOnScreen - Game1.tileSize * 2, Game1.tileSize * 4, Game1.tileSize), i18n.Get("title"));
            this.SortButton = new ClickableComponent(new Rectangle(this.xPositionOnScreen, this.yPositionOnScreen - Game1.tileSize * 2 + 10, Game1.tileSize * 4, Game1.tileSize), i18n.Get("labels.sort-by-name"));
            this.QualityButton = new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize * 4, this.yPositionOnScreen - Game1.tileSize * 2 + 10, Game1.tileSize * 4, Game1.tileSize), i18n.Get("labels.quality"));
            this.UpArrow = new ClickableTextureComponent("up-arrow", new Rectangle(this.xPositionOnScreen + this.width - Game1.tileSize / 2, this.yPositionOnScreen - Game1.tileSize, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), "", "", Game1.mouseCursors, new Rectangle(421, 459, 11, 12), Game1.pixelZoom);
            this.DownArrow = new ClickableTextureComponent("down-arrow", new Rectangle(this.xPositionOnScreen + this.width - Game1.tileSize / 2, this.yPositionOnScreen + this.height / 2 - Game1.tileSize * 2, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), "", "", Game1.mouseCursors, new Rectangle(421, 472, 11, 12), Game1.pixelZoom);

            // create tabs
            {
                int i = -1;

                int x = (int)(this.xPositionOnScreen - Game1.tileSize * 5.3f);
                int y = this.yPositionOnScreen + 10;
                int lblHeight = (int)(Game1.tileSize * 0.9F);

                this.Tabs.Add(new ClickableComponent(new Rectangle(x, y + lblHeight * i++, Game1.tileSize * 5, Game1.tileSize), MenuTab.All.ToString(), i18n.Get("tabs.all")));
                this.Tabs.Add(new ClickableComponent(new Rectangle(x, y + lblHeight * i++, Game1.tileSize * 5, Game1.tileSize), MenuTab.ToolsAndEquipment.ToString(), i18n.Get("tabs.equipment")));
                this.Tabs.Add(new ClickableComponent(new Rectangle(x, y + lblHeight * i++, Game1.tileSize * 5, Game1.tileSize), MenuTab.SeedsAndCrops.ToString(), i18n.Get("tabs.crops")));
                this.Tabs.Add(new ClickableComponent(new Rectangle(x, y + lblHeight * i++, Game1.tileSize * 5, Game1.tileSize), MenuTab.FishAndBaitAndTrash.ToString(), i18n.Get("tabs.fishing")));
                this.Tabs.Add(new ClickableComponent(new Rectangle(x, y + lblHeight * i++, Game1.tileSize * 5, Game1.tileSize), MenuTab.ForageAndFruits.ToString(), i18n.Get("tabs.forage")));
                this.Tabs.Add(new ClickableComponent(new Rectangle(x, y + lblHeight * i++, Game1.tileSize * 5, Game1.tileSize), MenuTab.ArtifactsAndMinerals.ToString(), i18n.Get("tabs.artifacts-and-minerals")));
                this.Tabs.Add(new ClickableComponent(new Rectangle(x, y + lblHeight * i++, Game1.tileSize * 5, Game1.tileSize), MenuTab.ResourcesAndCrafting.ToString(), i18n.Get("tabs.resources-and-crafting")));
                this.Tabs.Add(new ClickableComponent(new Rectangle(x, y + lblHeight * i++, Game1.tileSize * 5, Game1.tileSize), MenuTab.ArtisanAndCooking.ToString(), i18n.Get("tabs.artisan-and-crafting")));
                this.Tabs.Add(new ClickableComponent(new Rectangle(x, y + lblHeight * i++, Game1.tileSize * 5, Game1.tileSize), MenuTab.AnimalAndMonster.ToString(), i18n.Get("tabs.animal-and-monster")));
                this.Tabs.Add(new ClickableComponent(new Rectangle(x, y + lblHeight * i++, Game1.tileSize * 5, Game1.tileSize), MenuTab.Decorating.ToString(), i18n.Get("tabs.decorating")));
                this.Tabs.Add(new ClickableComponent(new Rectangle(x, y + lblHeight * i, Game1.tileSize * 5, Game1.tileSize), MenuTab.Misc.ToString(), i18n.Get("tabs.miscellaneous")));
            }

            // initialise sort UI
            switch (this.SortID)
            {
                case 0:
                    this.SortButton.name = i18n.Get("labels.sort-by-name");
                    break;
                case 1:
                    this.SortButton.name = i18n.Get("labels.sort-by-category");
                    break;
                case 2:
                    this.SortButton.name = i18n.Get("labels.sort-by-id");
                    break;
            }

            // load items
            this.LoadInventory(this.SpawnableItems);
        }

        public ItemMenu(ITranslationHelper i18n)
            : this(0, 0, ItemQuality.Normal, i18n) { }

        public override void receiveRightClick(int x, int y, bool playSound = true)
        {
            if (this.TextboxBounds.Contains(x, y))
            {
                this.Textbox.Text = "";
                return;
            }
            if (!this.AllowRightClick)
                return;
            base.receiveRightClick(x, y, false);
            if (this.HeldItem == null && this.ShowReceivingMenu)
            {
                this.HeldItem = this.ItemsToGrabMenu.RightClick(x, y, this.HeldItem, false);
                if (this.HeldItem is SObject obj && obj.parentSheetIndex == 326)
                {
                    this.HeldItem = null;
                    Game1.player.canUnderstandDwarves = true;
                    this.Poof = new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 320, 64, 64), 50f, 8, 0, new Vector2(x - x % Game1.tileSize + Game1.tileSize / 4, y - y % Game1.tileSize + Game1.tileSize / 4), false, false);
                    Game1.playSound("fireball");
                }
                else if (this.HeldItem is SObject recipe && recipe.isRecipe)
                {
                    string key = this.HeldItem.Name.Substring(0, recipe.Name.IndexOf("Recipe") - 1);
                    try
                    {
                        if (recipe.category == -7)
                            Game1.player.cookingRecipes.Add(key, 0);
                        else
                            Game1.player.craftingRecipes.Add(key, 0);
                        this.Poof = new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 320, 64, 64), 50f, 8, 0, new Vector2(x - x % Game1.tileSize + Game1.tileSize / 4, y - y % Game1.tileSize + Game1.tileSize / 4), false, false);
                        Game1.playSound("newRecipe");
                    }
                    catch { }
                    this.HeldItem = null;
                }
                else
                {
                    if (!Game1.player.addItemToInventoryBool(this.HeldItem))
                        return;
                    this.HeldItem = null;
                    Game1.playSound("coin");
                }
            }
        }

        public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
        {
            this.Reopen();
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            base.receiveLeftClick(x, y, playSound);

            if (this.HeldItem == null)
            {
                foreach (ClickableComponent tab in this.Tabs)
                {
                    if (tab.bounds.Contains(x, y))
                    {
                        Game1.exitActiveMenu();
                        ItemInventoryMenu.ScrollIndex = 0;
                        MenuTab tabID = this.GetTabID(tab);
                        this.Reopen(tabID);
                        break;
                    }
                }

                if (this.SortButton.bounds.Contains(x, y))
                {
                    int sortID = this.SortID + 1;
                    if (sortID > 2)
                        sortID = 0;
                    this.Reopen(sortID: sortID);
                }

                if (this.QualityButton.bounds.Contains(x, y))
                {
                    ItemQuality quality = this.Quality != this.Quality.GetNext()
                        ? this.Quality.GetNext()
                        : ItemQuality.Normal;
                    this.Reopen(quality: quality);
                }

                if (this.UpArrow.bounds.Contains(x, y))
                    this.ItemsToGrabMenu?.receiveScrollWheelAction(1);

                if (this.DownArrow.bounds.Contains(x, y))
                    this.ItemsToGrabMenu?.receiveScrollWheelAction(-1);
            }

            if (this.HeldItem == null && this.ShowReceivingMenu)
            {
                this.HeldItem = this.ItemsToGrabMenu?.LeftClick(x, y, this.HeldItem, false);
                if (this.HeldItem is SObject obj && obj.parentSheetIndex == 326)
                {
                    this.HeldItem = null;
                    Game1.player.canUnderstandDwarves = true;
                    this.Poof = new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 320, 64, 64), 50f, 8, 0, new Vector2(x - x % Game1.tileSize + Game1.tileSize / 4, y - y % Game1.tileSize + Game1.tileSize / 4), false, false);
                    Game1.playSound("fireball");
                }
                else if (this.HeldItem is SObject && (this.HeldItem as SObject).parentSheetIndex == 102)
                {
                    this.HeldItem = null;
                    Game1.player.foundArtifact(102, 1);
                    this.Poof = new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 320, 64, 64), 50f, 8, 0, new Vector2(x - x % Game1.tileSize + Game1.tileSize / 4, y - y % Game1.tileSize + Game1.tileSize / 4), false, false);
                    Game1.playSound("fireball");
                }
                else if (this.HeldItem is SObject recipe && recipe.isRecipe)
                {
                    string key = recipe.Name.Substring(0, recipe.Name.IndexOf("Recipe") - 1);
                    try
                    {
                        if (recipe.category == -7)
                            Game1.player.cookingRecipes.Add(key, 0);
                        else
                            Game1.player.craftingRecipes.Add(key, 0);
                        this.Poof = new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 320, 64, 64), 50f, 8, 0, new Vector2(x - x % Game1.tileSize + Game1.tileSize / 4, y - y % Game1.tileSize + Game1.tileSize / 4), false, false);
                        Game1.playSound("newRecipe");
                    }
                    catch { }
                    this.HeldItem = null;
                }
                else if (Game1.player.addItemToInventoryBool(this.HeldItem))
                {
                    this.HeldItem = null;
                    Game1.playSound("coin");
                }
            }
            if (this.HeldItem == null || this.isWithinBounds(x, y) || !this.HeldItem.canBeTrashed())
                return;
            Game1.playSound("throwDownITem");
            Game1.createItemDebris(this.HeldItem, Game1.player.getStandingPosition(), Game1.player.FacingDirection);
            this.HeldItem = null;

        }

        public bool AreAllItemsTaken()
        {
            return this.ItemsToGrabMenu.ActualInventory.All(t => t == null);
        }

        public override void receiveKeyPress(Keys key)
        {
            if (this.Textbox.Selected)
                return;

            if ((this.CanExitOnKey || this.AreAllItemsTaken()) && (Game1.options.doesInputListContain(Game1.options.menuButton, key) && this.readyToClose()))
            {
                this.exitThisMenu();
                if (Game1.currentLocation.currentEvent != null)
                    ++Game1.currentLocation.currentEvent.CurrentCommand;
            }
            else if (Game1.options.doesInputListContain(Game1.options.menuButton, key) && this.HeldItem != null)
                Game1.setMousePosition(this.TrashCan.bounds.Center);
            if (key != Keys.Delete || this.HeldItem == null || !this.HeldItem.canBeTrashed())
                return;
            if (this.HeldItem is SObject obj && Game1.player.specialItems.Contains(obj.parentSheetIndex))
                Game1.player.specialItems.Remove(obj.parentSheetIndex);
            this.HeldItem = null;
            Game1.playSound("trashcan");
        }

        public override void update(GameTime time)
        {
            if (this.TempText != this.Textbox.Text)
            {
                this.TempText = this.Textbox.Text;
                ItemInventoryMenu.ScrollIndex = 0;
                this.LoadInventory(this.SpawnableItems);
            }

            base.update(time);
            if (this.Poof == null || !this.Poof.update(time))
                return;
            this.Poof = null;
        }

        public override void performHoverAction(int x, int y)
        {
            if (this.ItemsToGrabMenu.isWithinBounds(x, y) && this.ShowReceivingMenu)
                this.HoveredItem = this.ItemsToGrabMenu.Hover(x, y, this.HeldItem);
            else
                base.performHoverAction(x, y);

            this.Textbox.Selected = this.TextboxBounds.Contains(x, y);
        }

        public override void receiveScrollWheelAction(int direction)
        {
            if (GameMenu.forcePreventClose)
                return;

            if (this.HeldItem == null && this.HoveredItem != null && Game1.oldKBState.IsKeyDown(Keys.LeftShift))
            {
                try
                {
                    SObject obj = (SObject)this.HoveredItem;
                    obj.quality = direction > 0
                        ? (int)((ItemQuality)obj.quality).GetNext()
                        : (int)((ItemQuality)obj.quality).GetPrevious();
                }
                catch { }
            }
            else
                this.ItemsToGrabMenu?.receiveScrollWheelAction(direction);
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            if (!Game1.options.showMenuBackground)
                spriteBatch.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.8f);
            this.Textbox.Draw(spriteBatch);
            this.Draw(spriteBatch, false, false);
            if (this.ShowReceivingMenu)
            {
                CJB.DrawTextBox(this.Title.bounds.X, this.Title.bounds.Y, Game1.dialogueFont, this.Title.name, true, 2);
                Game1.drawDialogueBox(this.ItemsToGrabMenu.xPositionOnScreen - IClickableMenu.borderWidth - IClickableMenu.spaceToClearSideBorder, this.ItemsToGrabMenu.yPositionOnScreen - IClickableMenu.borderWidth - IClickableMenu.spaceToClearTopBorder, this.ItemsToGrabMenu.width + IClickableMenu.borderWidth * 2 + IClickableMenu.spaceToClearSideBorder * 2, this.ItemsToGrabMenu.height + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth * 2, false, true);
                this.ItemsToGrabMenu.draw(spriteBatch);
                foreach (ClickableComponent tab in this.Tabs)
                {
                    MenuTab tabID = this.GetTabID(tab);
                    CJB.DrawTextBox(tab.bounds.X + tab.bounds.Width, tab.bounds.Y, Game1.smallFont, tab.label, true, 2, this.CurrentTab == tabID ? 1F : 0.7F);
                }

                CJB.DrawTextBox(this.SortButton.bounds.X, this.SortButton.bounds.Y, Game1.smallFont, this.SortButton.name, true);
                CJB.DrawTextBox(this.QualityButton.bounds.X, this.QualityButton.bounds.Y, Game1.smallFont, this.QualityButton.name, true);

                this.UpArrow.draw(spriteBatch);
                this.DownArrow.draw(spriteBatch);
            }

            this.Poof?.draw(spriteBatch, true);
            if (this.HoverText != null && (this.HoveredItem == null || this.ItemsToGrabMenu == null))
                IClickableMenu.drawHoverText(spriteBatch, this.HoverText, Game1.smallFont);
            if (this.HoveredItem != null)
                IClickableMenu.drawToolTip(spriteBatch, this.HoveredItem.getDescription(), this.HoveredItem.DisplayName, this.HoveredItem, this.HeldItem != null);
            else if (this.HoveredItem != null && this.ItemsToGrabMenu != null)
                IClickableMenu.drawToolTip(spriteBatch, this.ItemsToGrabMenu.DescriptionText, this.ItemsToGrabMenu.DescriptionTitle, this.HoveredItem, this.HeldItem != null);
            this.HeldItem?.drawInMenu(spriteBatch, new Vector2(Game1.getOldMouseX() + 8, Game1.getOldMouseY() + 8), 1f);

            if (!Game1.options.hardwareCursor)
                spriteBatch.Draw(Game1.mouseCursors, new Vector2(Game1.getOldMouseX(), Game1.getOldMouseY()), Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 0, 16, 16), Color.White, 0.0f, Vector2.Zero, Game1.pixelZoom + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 1f);
        }


        /*********
        ** Private methods
        *********/
        private void Reopen(MenuTab? tabIndex = null, int? sortID = null, ItemQuality? quality = null)
        {
            Game1.activeClickableMenu = new ItemMenu(tabIndex ?? this.CurrentTab, sortID ?? this.SortID, quality ?? this.Quality, this.TranslationHelper);
        }

        /// <summary>Get the tab constant represented by a tab component.</summary>
        /// <param name="tab">The component to check.</param>
        private MenuTab GetTabID(ClickableComponent tab)
        {
            if (!Enum.TryParse(tab.name, out MenuTab tabID))
                throw new InvalidOperationException($"Couldn't parse tab name '{tab.name}'.");
            return tabID;
        }

        private void LoadInventory(Item[] spawnableItems)
        {
            // get spawnable items in display order
            spawnableItems = spawnableItems.OrderBy(o => o.DisplayName).ToArray();
            switch (this.SortID)
            {
                case 1:
                    spawnableItems = spawnableItems.OrderBy(o => o.category).ToArray();
                    break;
                case 2:
                    spawnableItems = spawnableItems.OrderBy(o => o.parentSheetIndex).ToArray();
                    break;
            }

            // load inventory
            List<Item> inventoryItems = new List<Item>();
            foreach (Item item in spawnableItems)
            {
                item.Stack = item.maximumStackSize();
                if (item is SObject obj)
                    obj.quality = (int)this.Quality;

                if ((this.CurrentTab == MenuTab.All || this.GetRelevantTab(item) == this.CurrentTab) && item.Name.ToLower().Contains(this.Textbox.Text.ToLower()))
                    inventoryItems.Add(item);
            }

            // show menu
            this.ItemsToGrabMenu = new ItemInventoryMenu(this.xPositionOnScreen + Game1.tileSize / 2, this.yPositionOnScreen, false, inventoryItems);
        }

        /// <summary>Get the relevant tab for an item.</summary>
        /// <param name="item">The item whose tab to check.</param>
        private MenuTab GetRelevantTab(Item item)
        {
            // by type
            switch (item)
            {
                case Tool _:
                case Ring _:
                case Hat _:
                case Boots _:
                    return MenuTab.ToolsAndEquipment;

                case Furniture _:
                    return MenuTab.Decorating;
            }

            // by category
            switch (item.category)
            {
                case SObject.SeedsCategory:
                case SObject.VegetableCategory:
                case SObject.fertilizerCategory:
                case SObject.flowersCategory:
                    return MenuTab.SeedsAndCrops;

                case SObject.FishCategory:
                case SObject.baitCategory:
                case SObject.junkCategory:
                case SObject.tackleCategory:
                    return MenuTab.FishAndBaitAndTrash;

                case SObject.GreensCategory:
                case SObject.FruitsCategory:
                    return MenuTab.ForageAndFruits;

                case SObject.mineralsCategory:
                case SObject.GemCategory:
                    return MenuTab.ArtifactsAndMinerals;

                case SObject.metalResources:
                case SObject.buildingResources:
                case SObject.CraftingCategory:
                case SObject.BigCraftableCategory:
                    return MenuTab.ResourcesAndCrafting;

                case SObject.artisanGoodsCategory:
                case SObject.syrupCategory:
                case SObject.CookingCategory:
                case SObject.ingredientsCategory:
                    return MenuTab.ArtisanAndCooking;

                case SObject.sellAtPierresAndMarnies:
                case SObject.meatCategory:
                case SObject.EggCategory:
                case SObject.MilkCategory:
                case SObject.monsterLootCategory:
                    return MenuTab.AnimalAndMonster;

                case SObject.furnitureCategory:
                    return MenuTab.Decorating;
            }

            // artifacts
            if ((item as SObject)?.type == "Arch")
                return MenuTab.ArtifactsAndMinerals;

            // anything else
            return MenuTab.Misc;
        }

        private IEnumerable<Item> GetSpawnableItems()
        {
            // get tools
            for (int quality = Tool.stone; quality <= Tool.iridium; quality++)
            {
                yield return ToolFactory.getToolFromDescription(ToolFactory.axe, quality);
                yield return ToolFactory.getToolFromDescription(ToolFactory.hoe, quality);
                yield return ToolFactory.getToolFromDescription(ToolFactory.pickAxe, quality);
                yield return ToolFactory.getToolFromDescription(ToolFactory.wateringCan, quality);
                if (quality != Tool.iridium)
                    yield return ToolFactory.getToolFromDescription(ToolFactory.fishingRod, quality);
            }
            yield return new MilkPail();
            yield return new Shears();
            yield return new Pan();
            yield return new Wand(); // return scepter

            // wallpapers
            for (int id = 0; id < 112; id++)
                yield return new Wallpaper(id) { category = SObject.furnitureCategory };

            // flooring
            for (int id = 0; id < 40; id++)
                yield return new Wallpaper(id, true) { category = SObject.furnitureCategory };

            // equipment
            foreach (int id in Game1.content.Load<Dictionary<int, string>>("Data\\Boots").Keys)
                yield return new Boots(id);
            foreach (int id in Game1.content.Load<Dictionary<int, string>>("Data\\hats").Keys)
                yield return new Hat(id);

            // weapons
            foreach (int id in Game1.content.Load<Dictionary<int, string>>("Data\\weapons").Keys)
            {
                if (id >= 32 && id <= 34)
                    yield return new Slingshot(id);
                yield return new MeleeWeapon(id);
            }

            // furniture
            foreach (int id in Game1.content.Load<Dictionary<int, string>>("Data\\Furniture").Keys)
            {
                Item item = new Furniture(id, Vector2.Zero);
                if (id == 1466 || id == 1468)
                    item = new TV(id, Vector2.Zero);
                yield return item;
            }

            // craftables
            foreach (int id in Game1.bigCraftablesInformation.Keys)
            {
                SObject item = new SObject(Vector2.Zero, id);
                yield return item;
            }

            // objects
            foreach (int id in Game1.objectInformation.Keys)
            {
                // ring
                if (id >= Ring.ringLowerIndexRange && id <= Ring.ringUpperIndexRange)
                {
                    yield return new Ring(id);
                    continue;
                }

                // object
                SObject item = new SObject(id, 1);
                yield return item;

                // fruit products
                if (item.category == SObject.FruitsCategory)
                {
                    yield return new SObject(348, 1)
                    {
                        name = $"{item.Name} Wine",
                        price = item.price * 3,
                        preserve = SObject.PreserveType.Wine,
                        preservedParentSheetIndex = item.parentSheetIndex
                    };
                    yield return new SObject(344, 1)
                    {
                        name = $"{item.Name} Jelly",
                        price = 50 + item.Price * 2,
                        preserve = SObject.PreserveType.Jelly,
                        preservedParentSheetIndex = item.parentSheetIndex
                    };
                }

                // vegetable products
                else if (item.category == SObject.VegetableCategory)
                {
                    yield return new SObject(350, 1)
                    {
                        name = $"{item.Name} Juice",
                        price = (int)(item.price * 2.25d),
                        preserve = SObject.PreserveType.Juice,
                        preservedParentSheetIndex = item.parentSheetIndex
                    };
                    yield return new SObject(342, 1)
                    {
                        name = $"Pickled {item.Name}",
                        price = 50 + item.Price * 2,
                        preserve = SObject.PreserveType.Pickle,
                        preservedParentSheetIndex = item.parentSheetIndex
                    };
                }

                // flower products
                else if (item.category == SObject.flowersCategory)
                {
                    // get honey type
                    SObject.HoneyType? type = null;
                    switch (item.parentSheetIndex)
                    {
                        case 376:
                            type = SObject.HoneyType.Poppy;
                            break;
                        case 591:
                            type = SObject.HoneyType.Tulip;
                            break;
                        case 593:
                            type = SObject.HoneyType.SummerSpangle;
                            break;
                        case 595:
                            type = SObject.HoneyType.FairyRose;
                            break;
                        case 597:
                            type = SObject.HoneyType.BlueJazz;
                            break;
                        case 421: // sunflower standing in for all other flowers
                            type = SObject.HoneyType.Wild;
                            break;
                    }

                    // yield honey
                    if (type != null)
                    {
                        SObject honey = new SObject(Vector2.Zero, 340, item.Name + " Honey", false, true, false, false)
                        {
                            name = "Wild Honey",
                            honeyType = type
                        };
                        if (type != SObject.HoneyType.Wild)
                        {
                            honey.name = $"{item.Name} Honey";
                            honey.price += item.price * 2;
                        }
                        yield return honey;
                    }
                }
            }
        }
    }
}
