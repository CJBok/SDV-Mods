using System;
using System.Collections.Generic;
using System.Linq;
using CJBItemSpawner.Framework.Constants;
using CJBItemSpawner.Framework.ItemData;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Objects;
using SObject = StardewValley.Object;

namespace CJBItemSpawner.Framework
{
    internal class ItemMenu : ItemMenuWithInventory
    {
        /*********
        ** Fields
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
        private readonly ItemSort SortBy;
        private readonly ItemQuality Quality;
        private readonly bool ShowReceivingMenu = true;
        private readonly bool CanExitOnKey = true;

        /// <summary>Provides methods for searching and constructing items.</summary>
        private readonly ItemRepository ItemRepository;

        private ItemInventoryMenu ItemsToGrabMenu;
        private TemporaryAnimatedSprite Poof;
        private Rectangle TextboxBounds;
        private string PreviousText = "";


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="currentTab">The selected tab.</param>
        /// <param name="sortBy">How to sort items.</param>
        /// <param name="quality">The item quality to display.</param>
        /// <param name="search">The search term to prepopulate.</param>
        /// <param name="i18n">Provides translations for the mod.</param>
        /// <param name="itemRepository">Provides methods for searching and constructing items.</param>
        public ItemMenu(MenuTab currentTab, ItemSort sortBy, ItemQuality quality, string search, ITranslationHelper i18n, ItemRepository itemRepository)
          : base(null, true, true, 0, -50)
        {
            // initialise
            this.TranslationHelper = i18n;
            this.ItemRepository = itemRepository;
            this.MovePosition(110, Game1.viewport.Height / 2 - (650 + IClickableMenu.borderWidth * 2) / 2);
            this.CurrentTab = currentTab;
            this.SortBy = sortBy;
            this.Quality = quality;
            this.SpawnableItems = this.ItemRepository.GetFiltered().Select(p => p.Item).ToArray();
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
                Text = search
            };
            Game1.keyboardDispatcher.Subscriber = this.Textbox;
            this.TextboxBounds = new Rectangle(this.Textbox.X, this.Textbox.Y, this.Textbox.Width, this.Textbox.Height / 3);

            // create buttons
            this.Title = new ClickableComponent(new Rectangle(this.xPositionOnScreen + this.width - Game1.tileSize, this.yPositionOnScreen - Game1.tileSize * 2, Game1.tileSize * 4, Game1.tileSize), i18n.Get("title"));
            this.QualityButton = new ClickableComponent(new Rectangle(this.xPositionOnScreen, this.yPositionOnScreen - Game1.tileSize * 2 + 10, (int)Game1.smallFont.MeasureString(i18n.Get("labels.quality")).X, Game1.tileSize), i18n.Get("labels.quality"));
            this.SortButton = new ClickableComponent(new Rectangle(this.xPositionOnScreen + this.QualityButton.bounds.Width + 40, this.yPositionOnScreen - Game1.tileSize * 2 + 10, Game1.tileSize * 4, Game1.tileSize), this.GetSortLabel(sortBy));
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
                this.Tabs.Add(new ClickableComponent(new Rectangle(x, y + lblHeight * i++, Game1.tileSize * 5, Game1.tileSize), MenuTab.ArtisanAndCooking.ToString(), i18n.Get("tabs.artisan-and-cooking")));
                this.Tabs.Add(new ClickableComponent(new Rectangle(x, y + lblHeight * i++, Game1.tileSize * 5, Game1.tileSize), MenuTab.AnimalAndMonster.ToString(), i18n.Get("tabs.animal-and-monster")));
                this.Tabs.Add(new ClickableComponent(new Rectangle(x, y + lblHeight * i++, Game1.tileSize * 5, Game1.tileSize), MenuTab.Decorating.ToString(), i18n.Get("tabs.decorating")));
                this.Tabs.Add(new ClickableComponent(new Rectangle(x, y + lblHeight * i, Game1.tileSize * 5, Game1.tileSize), MenuTab.Misc.ToString(), i18n.Get("tabs.miscellaneous")));
            }

            // load items
            this.LoadInventory(this.SpawnableItems);
        }

        /// <summary>Construct an instance.</summary>
        /// <param name="i18n">Provides translations for the mod.</param>
        /// <param name="itemRepository">Provides methods for searching and constructing items.</param>
        public ItemMenu(ITranslationHelper i18n, ItemRepository itemRepository)
            : this(0, 0, ItemQuality.Normal, "", i18n, itemRepository) { }

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
                if (this.HeldItem is SObject obj && obj.ParentSheetIndex == 326)
                {
                    this.HeldItem = null;
                    Game1.player.canUnderstandDwarves = true;
                    this.Poof = new TemporaryAnimatedSprite("TileSheets\\animations", new Rectangle(0, 320, 64, 64), 50f, 8, 0, new Vector2(x - x % 64 + 16, y - y % 64 + 16), false, false);
                    Game1.playSound("fireball");
                }
                else if (this.HeldItem is SObject recipe && recipe.IsRecipe)
                {
                    string key = this.HeldItem.Name.Substring(0, recipe.Name.IndexOf("Recipe", StringComparison.InvariantCultureIgnoreCase) - 1);
                    try
                    {
                        if (recipe.Category == -7)
                            Game1.player.cookingRecipes.Add(key, 0);
                        else
                            Game1.player.craftingRecipes.Add(key, 0);
                        this.Poof = new TemporaryAnimatedSprite("TileSheets\\animations", new Rectangle(0, 320, 64, 64), 50f, 8, 0, new Vector2(x - x % 64 + 16, y - y % 64 + 16), false, false);
                        Game1.playSound("newRecipe");
                    }
                    catch
                    {
                        // deliberately ignore errors
                    }
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
                    this.Reopen(sortBy: this.SortBy.GetNext());
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
                SObject obj = this.HeldItem as SObject;
                if (obj != null && obj.ParentSheetIndex == 326)
                {
                    this.HeldItem = null;
                    Game1.player.canUnderstandDwarves = true;
                    this.Poof = new TemporaryAnimatedSprite("TileSheets\\animations", new Rectangle(0, 320, 64, 64), 50f, 8, 0, new Vector2(x - x % Game1.tileSize + Game1.tileSize / 4, y - y % Game1.tileSize + Game1.tileSize / 4), false, false);
                    Game1.playSound("fireball");
                }
                else if (obj != null && obj.ParentSheetIndex == 102)
                {
                    this.HeldItem = null;
                    Game1.player.foundArtifact(102, 1);
                    this.Poof = new TemporaryAnimatedSprite("TileSheets\\animations", new Rectangle(0, 320, 64, 64), 50f, 8, 0, new Vector2(x - x % Game1.tileSize + Game1.tileSize / 4, y - y % Game1.tileSize + Game1.tileSize / 4), false, false);
                    Game1.playSound("fireball");
                }
                else if (obj != null && obj.IsRecipe)
                {
                    string key = obj.Name.Substring(0, obj.Name.IndexOf("Recipe", StringComparison.InvariantCultureIgnoreCase) - 1);
                    try
                    {
                        if (obj.Category == -7)
                            Game1.player.cookingRecipes.Add(key, 0);
                        else
                            Game1.player.craftingRecipes.Add(key, 0);
                        this.Poof = new TemporaryAnimatedSprite("TileSheets\\animations", new Rectangle(0, 320, 64, 64), 50f, 8, 0, new Vector2(x - x % Game1.tileSize + Game1.tileSize / 4, y - y % Game1.tileSize + Game1.tileSize / 4), false, false);
                        Game1.playSound("newRecipe");
                    }
                    catch
                    {
                        // deliberately ignore errors
                    }
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
            if (this.HeldItem is SObject obj && Game1.player.specialItems.Contains(obj.ParentSheetIndex))
                Game1.player.specialItems.Remove(obj.ParentSheetIndex);
            this.HeldItem = null;
            Game1.playSound("trashcan");
        }

        public override void update(GameTime time)
        {
            if (this.PreviousText != this.Textbox.Text)
            {
                this.PreviousText = this.Textbox.Text;
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
                    obj.Quality = direction > 0
                        ? (int)((ItemQuality)obj.Quality).GetNext()
                        : (int)((ItemQuality)obj.Quality).GetPrevious();
                }
                catch
                {
                    // deliberately ignore errors
                }
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
            {
                if (this.HoverAmount > 0)
                    IClickableMenu.drawToolTip(spriteBatch, this.HoverText, "", null, true, -1, 0, -1, -1, null, this.HoverAmount);
                else
                    IClickableMenu.drawHoverText(spriteBatch, this.HoverText, Game1.smallFont);
            }

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
        private void Reopen(MenuTab? tabIndex = null, ItemSort? sortBy = null, ItemQuality? quality = null, string search = null)
        {
            Game1.activeClickableMenu = new ItemMenu(tabIndex ?? this.CurrentTab, sortBy ?? this.SortBy, quality ?? this.Quality, search ?? this.PreviousText, this.TranslationHelper, this.ItemRepository);
        }

        /// <summary>Get the translated label for a sort type.</summary>
        /// <param name="sort">The sort type.</param>
        private string GetSortLabel(ItemSort sort)
        {
            switch (sort)
            {
                case ItemSort.DisplayName:
                    return this.TranslationHelper.Get("labels.sort-by-name");
                case ItemSort.Category:
                    return this.TranslationHelper.Get("labels.sort-by-category");
                case ItemSort.ID:
                    return this.TranslationHelper.Get("labels.sort-by-id");
                default:
                    throw new NotSupportedException($"Invalid sort type {sort}.");
            }
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
            // sort items
            switch (this.SortBy)
            {
                case ItemSort.Category:
                    spawnableItems = spawnableItems.OrderBy(o => o.Category).ToArray();
                    break;

                case ItemSort.ID:
                    spawnableItems = spawnableItems.OrderBy(o => o.ParentSheetIndex).ToArray();
                    break;

                default:
                    spawnableItems = spawnableItems.OrderBy(o => o.DisplayName).ToArray();
                    break;
            }

            // load inventory
            List<Item> inventoryItems = new List<Item>();
            string search = this.Textbox.Text.Trim();
            foreach (Item item in spawnableItems)
            {
                // get item
                item.Stack = item.maximumStackSize();
                if (item is SObject obj)
                    obj.Quality = (int)this.Quality;

                // skip if not applicable
                if (this.CurrentTab != MenuTab.All && this.GetRelevantTab(item) != this.CurrentTab)
                    continue;
                if (search != "" && item.Name.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) < 0 && item.DisplayName.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) < 0)
                    continue;

                // add to inventory
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
                case Clothing _:
                    return MenuTab.ToolsAndEquipment;

                case Furniture _:
                    return MenuTab.Decorating;
            }

            // by category
            switch (item.Category)
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
            if ((item as SObject)?.Type == "Arch")
                return MenuTab.ArtifactsAndMinerals;

            // anything else
            return MenuTab.Misc;
        }
    }
}
