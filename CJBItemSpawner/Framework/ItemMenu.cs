using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CJB.Common;
using CJB.Common.UI;
using CJBItemSpawner.Framework.Constants;
using CJBItemSpawner.Framework.ItemData;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Objects;
using SObject = StardewValley.Object;

namespace CJBItemSpawner.Framework
{
    /// <summary>The item spawner menu which lets players add any item to their inventory and trash any item.</summary>
    internal class ItemMenu : ItemGrabMenu
    {
        /*********
        ** Fields
        *********/
        /****
        ** Constants
        ****/
        /// <summary>The max number of items that can shown at once in the UI view.</summary>
        private readonly int ItemsPerView = Chest.capacity;

        /// <summary>The max number of items that can be shown per row.</summary>
        private readonly int ItemsPerRow = Chest.capacity / 3;

        /// <summary>The IDs for objects which can't have a quality value.</summary>
        private readonly ISet<int> ItemsWithoutQuality = new HashSet<int>
        {
            447, // aged roe
            812 // roe
        };

        /// <summary>The tabs to show in their display order.</summary>
        private readonly Category[] CategoryOrder = {
            Constants.Category.All,
            Constants.Category.ToolsAndEquipment,
            Constants.Category.SeedsAndCrops,
            Constants.Category.FishAndBaitAndTrash,
            Constants.Category.ForageAndFruits,
            Constants.Category.ArtifactsAndMinerals,
            Constants.Category.ResourcesAndCrafting,
            Constants.Category.ArtisanAndCooking,
            Constants.Category.AnimalAndMonster,
            Constants.Category.Decorating,
            Constants.Category.Misc
        };

        /****
        ** State
        ****/
        /// <summary>The base draw method.</summary>
        /// <remarks>This circumvents an issue where <see cref="ItemGrabMenu.draw(SpriteBatch)"/> can't be called directly due to a conflicting overload.</remarks>
        private readonly Action<SpriteBatch> BaseDraw;

        /// <summary>The current filter tab.</summary>
        private Category Category = Category.All;

        /// <summary>The current item quality.</summary>
        private ItemQuality Quality = ItemQuality.Normal;

        /// <summary>The search text for which to filter items.</summary>
        private string SearchText = "";

        /// <summary>The field by which to sort items.</summary>
        private ItemSort SortBy = ItemSort.DisplayName;

        /// <summary>All items that can be spawned.</summary>
        private readonly Item[] AllItems;

        /// <summary>The items matching the current search filters, without scrolling.</summary>
        private readonly List<Item> FilteredItems = new List<Item>();

        /// <summary>The items currently visible in the UI.</summary>
        private readonly IList<Item> ItemsInView;

        /// <summary>The index of the top row shown in the UI view, used to scroll through the results.</summary>
        private int TopRowIndex;

        /// <summary>Whether the user explicitly selected the textbox by clicking on it, so the selection should be maintained.</summary>
        private bool TextboxExplicitlySelected;

        /****
        ** UI components
        ****/
        /// <summary>A button which toggles between sort criteria.</summary>
        private ClickableComponent SortButton;

        /// <summary>The bounds for the quality button background.</summary>
        private ClickableComponent QualityButton;

        /// <summary>A dropdown list to choose a category filter.</summary>
        private Dropdown<Category> CategoryDropdown;

        /// <summary>The up arrow to scroll results.</summary>
        private ClickableTextureComponent UpArrow;

        /// <summary>The down arrow to scroll results.</summary>
        private ClickableTextureComponent DownArrow;

        /// <summary>The search icon for the search box.</summary>
        private ClickableTextureComponent SearchIcon;

        /// <summary>The search textbox.</summary>
        private TextBox SearchBox;

        /// <summary>A clickable component corresponding to the <see cref="SearchBox"/> area. This only exists for controller movement support.</summary>
        private ClickableComponent SearchBoxArea;

        /// <summary>The textbox area.</summary>
        private Rectangle TextboxBounds;


        /*********
        ** Accessors
        *********/
        /// <summary>The child components for controller snapping.</summary>
        /// <remarks>This must be public and match a type supported by <see cref="IClickableMenu.populateClickableComponentList"/>.</remarks>
        public readonly List<ClickableComponent> ChildComponents = new List<ClickableComponent>();


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="itemRepository">Provides methods for searching and constructing items.</param>
        public ItemMenu(ItemRepository itemRepository)
            : base(
                inventory: new List<Item>(),
                reverseGrab: false,
                showReceivingMenu: true,
                highlightFunction: item => true,
                behaviorOnItemGrab: (item, player) => { },
                behaviorOnItemSelectFunction: (item, player) => { },
                message: null,
                canBeExitedWithKey: true,
                showOrganizeButton: false
            )
        {
            base.drawBG = false; // handled manually to draw arrows between background and menu

            this.BaseDraw = this.GetBaseDraw();
            this.behaviorOnItemGrab = this.OnItemGrab;
            this.ItemsInView = this.ItemsToGrabMenu.actualInventory;
            this.AllItems = itemRepository.GetAll().Select(p => p.Item).ToArray();

            this.InitializeComponents();
            this.ResetItemView(rebuild: true);
        }

        /// <summary>Handle a left-click by the player.</summary>
        /// <param name="x">The X-position of the cursor.</param>
        /// <param name="y">The Y-position of the cursor.</param>
        /// <param name="playSound">Whether to play interaction sounds.</param>
        /// <returns>Whether the event has been handled and shouldn't be propagated further.</returns>
        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            // allow trashing any item
            if (this.trashCan.containsPoint(x, y) && this.heldItem != null)
            {
                Utility.trashItem(this.heldItem);
                this.heldItem = null;
            }

            // sort button
            else if (this.SortButton.bounds.Contains(x, y))
            {
                this.SortBy = this.SortBy.GetNext();
                this.SortButton.label = this.SortButton.name = this.GetSortLabel(this.SortBy);
                this.ResetItemView(rebuild: true);
            }

            // quality button
            else if (this.QualityButton.bounds.Contains(x, y))
            {
                this.Quality = this.Quality.GetNext();
                this.ResetItemView();
            }

            // scroll buttons
            else if (this.UpArrow.bounds.Contains(x, y))
                this.receiveScrollWheelAction(1);
            else if (this.DownArrow.bounds.Contains(x, y))
                this.receiveScrollWheelAction(-1);

            // category dropdown
            else if (this.CategoryDropdown.containsPoint(x, y))
            {
                if (this.CategoryDropdown.TrySelect(x, y, out Category newTab))
                    this.SetCategory(newTab);
                this.SetDropdown(!this.CategoryDropdown.IsExpanded);
            }

            // textbox
            else if (this.TextboxBounds.Contains(x, y))
            {
                if (!this.SearchBox.Selected || !this.TextboxExplicitlySelected)
                    this.SelectSearchBox(explicitly: true);
            }

            // fallback
            else
            {
                // deselect textbox
                bool justDeselectedSearch = this.SearchBox.Selected;
                if (justDeselectedSearch)
                    this.DeselectSearchBox();

                // default behavior
                base.receiveLeftClick(x, y, playSound);
            }

        }

        /// <summary>Handle a right-click by the player.</summary>
        /// <param name="x">The X-position of the cursor.</param>
        /// <param name="y">The Y-position of the cursor.</param>
        /// <param name="playSound">Whether to play interaction sounds.</param>
        public override void receiveRightClick(int x, int y, bool playSound = true)
        {
            // clear search box
            if (this.TextboxBounds.Contains(x, y))
                this.SearchBox.Text = "";

            // close dropdown
            else if (this.CategoryDropdown.IsExpanded)
                this.SetDropdown(false);

            // default behavior
            else
                base.receiveRightClick(x, y, playSound);
        }

        /// <summary>Handle a button press by the player.</summary>
        /// <param name="key">The button that was pressed.</param>
        public override void receiveKeyPress(Keys key)
        {
            // deselect textbox
            if (this.SearchBox.Selected && key == Keys.Escape)
                this.DeselectSearchBox();

            // close dropdown
            else if (this.CategoryDropdown.IsExpanded && key == Keys.Escape)
                this.SetDropdown(false);

            // allow trashing any item
            else if (key == Keys.Delete && this.heldItem != null)
            {
                Utility.trashItem(this.heldItem);
                this.heldItem = null;
            }

            // default behavior
            else if (!this.SearchBox.Selected)
                base.receiveKeyPress(key);
        }

        /// <summary>Handle a controller button press by the player.</summary>
        /// <param name="button">The button that was pressed.</param>
        public override void receiveGamePadButton(Buttons button)
        {
            bool isExitKey = button == Buttons.B || button == Buttons.Y || button == Buttons.Start;
            bool inDropdown = this.CategoryDropdown.IsExpanded;

            // cancel dropdown
            if (isExitKey && inDropdown)
                this.CategoryDropdown.IsExpanded = false;

            // cancel search box
            else if (isExitKey && this.SearchBox.Selected)
                this.DeselectSearchBox();

            // navigate category dropdown
            else if (button == Buttons.LeftTrigger && !inDropdown)
                this.NextCategory(-1);
            else if (button == Buttons.RightTrigger && !inDropdown)
                this.NextCategory(1);

            else
                base.receiveGamePadButton(button);
        }

        /// <summary>Handle the player scrolling the mouse wheel.</summary>
        /// <param name="direction">The scroll direction.</param>
        public override void receiveScrollWheelAction(int direction)
        {
            base.receiveScrollWheelAction(direction);

            // scroll dropdown
            if (this.CategoryDropdown.IsExpanded)
                this.CategoryDropdown.ReceiveScrollWheelAction(direction);

            // scroll item view
            else
            {
                // apply scroll
                if (direction > 0)
                    this.TopRowIndex--;
                if (direction < 0)
                    this.TopRowIndex++;

                // normalize
                int maxRows = (int)Math.Ceiling(this.FilteredItems.Count / (this.ItemsPerRow * 1m));
                this.TopRowIndex = (int)MathHelper.Clamp(this.TopRowIndex, 0, maxRows);

                // update list
                this.ResetItemView(rebuild: false);
            }
        }

        /// <summary>Handle the player hovering the cursor over the menu.</summary>
        /// <param name="x">The cursor's X pixel position.</param>
        /// <param name="y">The cursor's Y pixel position.</param>
        public override void performHoverAction(int x, int y)
        {
            // handle search box selected
            if (!this.TextboxExplicitlySelected)
            {
                bool overSearchBox = this.TextboxBounds.Contains(x, y);
                if (this.SearchBox.Selected != overSearchBox)
                {
                    if (overSearchBox)
                        this.SelectSearchBox(explicitly: false);
                    else
                        this.DeselectSearchBox();
                }
            }

            // base logic
            base.performHoverAction(x, y);
        }

        /// <summary>Update the menu if needed.</summary>
        /// <param name="time">The current game time.</param>
        public override void update(GameTime time)
        {
            // deselect textbox
            if (this.TextboxExplicitlySelected && !this.SearchBox.Selected)
                this.DeselectSearchBox();

            // update search text
            if (this.SearchText != this.SearchBox.Text)
            {
                this.SearchText = this.SearchBox.Text;
                this.TopRowIndex = 0;
                this.ResetItemView(rebuild: true);
            }

            base.update(time);
        }

        /// <summary>Draw the menu to the screen.</summary>
        /// <param name="spriteBatch">The sprite batch being drawn.</param>
        public override void draw(SpriteBatch spriteBatch)
        {
            // draw arrows under base UI, so tooltips are drawn over them
            spriteBatch.Draw(Game1.fadeToBlackRect, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), Color.Black * 0.5f); // replicate base.drawBG so arrows are above it
            this.UpArrow.draw(spriteBatch);
            this.DownArrow.draw(spriteBatch);

            // draw base UI
            this.BaseDraw(spriteBatch);

            // add main UI
            this.SearchBox.Draw(spriteBatch);
            this.SearchIcon.draw(spriteBatch);
            this.CategoryDropdown.Draw(spriteBatch);
            CommonHelper.DrawTab(this.QualityButton.bounds.X, this.QualityButton.bounds.Y, this.QualityButton.bounds.Width - CommonHelper.ButtonBorderWidth, this.QualityButton.bounds.Height - CommonHelper.ButtonBorderWidth, out Vector2 qualityIconPos, forIcon: true);
            spriteBatch.Draw(Game1.mouseCursors, qualityIconPos, new Rectangle(345, 391, 10, 9), Color.White, 0, Vector2.Zero, Game1.pixelZoom, SpriteEffects.None, 1f);
            CommonHelper.DrawTab(this.SortButton.bounds.X, this.SortButton.bounds.Y, Game1.smallFont, this.SortButton.name);

            // redraw cursor over new UI
            base.drawMouse(spriteBatch);
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Initialize the custom UI components.</summary>
        private void InitializeComponents()
        {
            // basic buttons
            this.QualityButton = new ClickableComponent(new Rectangle(this.xPositionOnScreen, this.yPositionOnScreen - Game1.tileSize * 2 + 10, 9 * Game1.pixelZoom + CommonHelper.ButtonBorderWidth, 9 * Game1.pixelZoom + CommonHelper.ButtonBorderWidth - 2), ""); // manually tweak height to align with sort button
            this.SortButton = new ClickableComponent(new Rectangle(this.QualityButton.bounds.Right + 20, this.QualityButton.bounds.Y, this.GetMaxSortLabelWidth(Game1.smallFont), Game1.tileSize), this.GetSortLabel(this.SortBy));
            this.UpArrow = new ClickableTextureComponent("up-arrow", new Rectangle(this.xPositionOnScreen + this.width - 32, this.yPositionOnScreen - 64, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), "", "", Game1.mouseCursors, new Rectangle(421, 459, 11, 12), Game1.pixelZoom);
            this.DownArrow = new ClickableTextureComponent("down-arrow", new Rectangle(this.UpArrow.bounds.X, this.UpArrow.bounds.Y + this.height / 2 - 64, this.UpArrow.bounds.Width, this.UpArrow.bounds.Height), "", "", Game1.mouseCursors, new Rectangle(421, 472, 11, 12), Game1.pixelZoom);
            this.SearchIcon = new ClickableTextureComponent("search", new Rectangle(this.xPositionOnScreen + this.width - 39 - 45, this.yPositionOnScreen - Game1.tileSize * 2 + 20, 39, 39), "", "", Game1.mouseCursors, new Rectangle(80, 0, 13, 13), 3);

            // search box
            {
                var searchBoxTexture = Game1.content.Load<Texture2D>("LooseSprites\\textBox");
                this.SearchBox = new TextBox(searchBoxTexture, null, Game1.smallFont, Game1.textColor)
                {
                    X = this.SearchIcon.bounds.X - searchBoxTexture.Width - 10,
                    Y = this.SearchIcon.bounds.Y,
                    Text = this.SearchText
                };
                this.TextboxBounds = new Rectangle(this.SearchBox.X, this.SearchBox.Y, this.SearchBox.Width, this.SearchBox.Height);
            }
            this.SearchBoxArea = new ClickableComponent(new Rectangle(this.SearchBox.X, this.SearchBox.Y, this.SearchBox.Width, this.SearchBox.Height), "");

            // category dropdown (centered between sort and search)
            this.CategoryDropdown = new Dropdown<Category>(0, this.SortButton.bounds.Y, Game1.smallFont, this.Category, this.CategoryOrder, this.GetTabLabel);
            this.CategoryDropdown.bounds.X = this.SortButton.bounds.Right + (this.SearchBox.X - this.SortButton.bounds.Right) / 2 - this.CategoryDropdown.bounds.Width / 2;
            this.CategoryDropdown.ReinitializeComponents();

            // controller flow
            this.InitializeControllerFlow();
        }

        /// <summary>Set the fields to support controller snapping.</summary>
        private void InitializeControllerFlow()
        {
            // implementation notes:
            //   - search box is deliberately excluded from controller flow since you can't enter values.
            //   - CategoryDropdown down neighbor ID is auto-managed depending on whether it's expanded.


            // get components
            List<ClickableComponent> slots = this.ItemsToGrabMenu.inventory;

            // update component list
            this.ChildComponents.Clear();
            this.ChildComponents.AddRange(new[] { this.QualityButton, this.SortButton, this.UpArrow, this.DownArrow, this.SearchBoxArea, this.CategoryDropdown });

            // set IDs
            {
                int curId = 1_000_000;
                foreach (ClickableComponent component in this.ChildComponents)
                    component.myID = curId++;
            }

            // rightward flow across custom UI
            this.QualityButton.rightNeighborID = this.SortButton.myID;
            this.SortButton.rightNeighborID = this.CategoryDropdown.myID;
            this.CategoryDropdown.rightNeighborID = this.UpArrow.myID;
            this.UpArrow.downNeighborID = this.DownArrow.myID;

            // leftward flow across custom UI
            this.UpArrow.upNeighborID = this.CategoryDropdown.myID;
            this.CategoryDropdown.leftNeighborID = this.SortButton.myID;
            this.SortButton.leftNeighborID = this.QualityButton.myID;

            // downward flow into inventory
            this.QualityButton.downNeighborID = slots[0].myID;
            this.SortButton.downNeighborID = slots[1].myID;
            this.CategoryDropdown.DefaultDownNeighborId = slots[5].myID;
            this.DownArrow.leftNeighborID = slots.Last().myID;
            this.DownArrow.downNeighborID = this.trashCan.myID;

            // upward flow into custom UI
            slots[0].upNeighborID = this.QualityButton.myID;
            foreach (int i in new[] { 1, 2 })
                slots[i].upNeighborID = this.SortButton.myID;
            foreach (int i in new[] { 3, 4, 5, 6, 8, 9, 10, 11 })
                slots[i].upNeighborID = this.QualityButton.myID;
            foreach (int i in new[] { 11, 23 })
                slots[i].rightNeighborID = this.UpArrow.myID;
            slots.Last().rightNeighborID = this.DownArrow.myID;
            this.trashCan.upNeighborID = this.DownArrow.myID;

            // dropdown entries
            this.CategoryDropdown.ReinitializeControllerFlow();
            this.ChildComponents.AddRange(this.CategoryDropdown.GetChildComponents());

            // update component list
            this.populateClickableComponentList();
        }

        /// <summary>Handle the user selecting an item from the menu.</summary>
        /// <param name="item">The item grabbed from the menu.</param>
        /// <param name="player">The player who grabbed the item.</param>
        private void OnItemGrab(Item item, Farmer player)
        {
            this.ResetItemView();
        }

        /// <summary>Expand or collapse the category dropdown.</summary>
        /// <param name="expanded">Whether the dropdown should be expanded.</param>
        protected void SetDropdown(bool expanded)
        {
            this.CategoryDropdown.IsExpanded = expanded;
            this.inventory.highlightMethod = item => !expanded;
            this.ItemsToGrabMenu.highlightMethod = item => !expanded;
        }

        /// <summary>Switch to the next category.</summary>
        /// <param name="direction">The direction to move in the category list.</param>
        protected void NextCategory(int direction)
        {
            direction = direction < 0 ? -1 : 1;
            int last = this.CategoryOrder.Length - 1;

            int index = Array.IndexOf(this.CategoryOrder, this.Category) + direction;
            if (index < 0)
                index = last;
            if (index > last)
                index = 0;

            this.SetCategory(this.CategoryOrder[index]);
        }

        /// <summary>Set the current category.</summary>
        /// <param name="category">The new category value.</param>
        protected void SetCategory(Category category)
        {
            this.Category = category;
            this.CategoryDropdown.TrySelect(category);
            this.ResetItemView(rebuild: true);
        }

        /// <summary>Set the search texbox selected.</summary>
        /// <param name="explicitly">Whether the textbox was selected explicitly by the user (rather than automatically by hovering), so the selection should be maintained.</param>
        private void SelectSearchBox(bool explicitly)
        {
            this.SearchBox.Selected = true;
            this.TextboxExplicitlySelected = explicitly;
        }

        /// <summary>Set the search texbox non-selected.</summary>
        private void DeselectSearchBox()
        {
            this.SearchBox.Selected = false;
            this.TextboxExplicitlySelected = false;
        }

        /// <summary>Reset the items shown in the view.</summary>
        /// <param name="rebuild">Whether to rebuild the search results.</param>
        private void ResetItemView(bool rebuild = false)
        {
            if (rebuild)
            {
                this.FilteredItems.Clear();
                this.FilteredItems.AddRange(this.SearchItems());
            }

            this.ItemsInView.Clear();
            foreach (var item in this.FilteredItems.Skip(this.TopRowIndex * this.ItemsPerRow).Take(this.ItemsPerView))
            {
                item.Stack = item.maximumStackSize();
                if (item is SObject obj && !this.ItemsWithoutQuality.Contains(obj.ParentSheetIndex))
                    obj.Quality = (int)this.Quality;

                this.ItemsInView.Add(item);
            }
        }

        /// <summary>Get all items matching the search criteria, ignoring pagination.</summary>
        private IEnumerable<Item> SearchItems()
        {
            // get base query
            IEnumerable<Item> items = this.AllItems;
            items = this.SortBy switch
            {
                ItemSort.Type => items.OrderBy(p => p.Category),
                ItemSort.ID => items.OrderBy(p => p.ParentSheetIndex),
                _ => items.OrderBy(p => p.DisplayName)
            };

            // apply menu tab
            if (this.Category != Category.All)
                items = items.Where(item => this.GetRelevantTab(item) == this.Category);

            // apply search
            string search = this.SearchBox.Text.Trim();
            if (search != "")
            {
                items = items.Where(item =>
                    item.Name.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) >= 0
                    || item.DisplayName.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) >= 0
                );
            }

            return items;
        }

        /// <summary>Get the translated label for a sort type.</summary>
        /// <param name="sort">The sort type.</param>
        private string GetSortLabel(ItemSort sort)
        {
            return sort switch
            {
                ItemSort.DisplayName => I18n.Labels_SortByName(),
                ItemSort.Type => I18n.Labels_SortByType(),
                ItemSort.ID => I18n.Labels_SortById(),
                _ => throw new NotSupportedException($"Invalid sort type {sort}.")
            };
        }

        /// <summary>Get the translated label for a type filter.</summary>
        /// <param name="type">The type.</param>
        private string GetTabLabel(Category type)
        {
            return type switch
            {
                Category.All => I18n.Tabs_All(),
                Category.ToolsAndEquipment => I18n.Tabs_Equipment(),
                Category.SeedsAndCrops => I18n.Tabs_Crops(),
                Category.FishAndBaitAndTrash => I18n.Tabs_Fishing(),
                Category.ForageAndFruits => I18n.Tabs_Forage(),
                Category.ArtifactsAndMinerals => I18n.Tabs_ArtifactsAndMinerals(),
                Category.ResourcesAndCrafting => I18n.Tabs_ResourcesAndCrafting(),
                Category.ArtisanAndCooking => I18n.Tabs_ArtisanAndCooking(),
                Category.AnimalAndMonster => I18n.Tabs_AnimalAndMonster(),
                Category.Decorating => I18n.Tabs_Decorating(),
                Category.Misc => I18n.Tabs_Miscellaneous(),
                _ => throw new NotSupportedException($"Invalid filter type {type}.")
            };
        }

        /// <summary>Get the maximum width of the sort label.</summary>
        /// <param name="font">The text font.</param>
        private int GetMaxSortLabelWidth(SpriteFont font)
        {
            return
                (
                    from ItemSort key in Enum.GetValues(typeof(ItemSort))
                    let text = this.GetSortLabel(key)
                    select (int)font.MeasureString(text).X
                )
                .Max();
        }

        /// <summary>Get the relevant tab for an item.</summary>
        /// <param name="item">The item whose tab to check.</param>
        private Category GetRelevantTab(Item item)
        {
            // by type
            switch (item)
            {
                case Tool _:
                case Ring _:
                case Hat _:
                case Boots _:
                case Clothing _:
                    return Category.ToolsAndEquipment;

                case Furniture _:
                    return Category.Decorating;
            }

            // by category
            switch (item.Category)
            {
                case SObject.SeedsCategory:
                case SObject.VegetableCategory:
                case SObject.fertilizerCategory:
                case SObject.flowersCategory:
                    return Category.SeedsAndCrops;

                case SObject.FishCategory:
                case SObject.baitCategory:
                case SObject.junkCategory:
                case SObject.tackleCategory:
                    return Category.FishAndBaitAndTrash;

                case SObject.GreensCategory:
                case SObject.FruitsCategory:
                    return Category.ForageAndFruits;

                case SObject.mineralsCategory:
                case SObject.GemCategory:
                    return Category.ArtifactsAndMinerals;

                case SObject.metalResources:
                case SObject.buildingResources:
                case SObject.CraftingCategory:
                case SObject.BigCraftableCategory:
                    return Category.ResourcesAndCrafting;

                case SObject.artisanGoodsCategory:
                case SObject.syrupCategory:
                case SObject.CookingCategory:
                case SObject.ingredientsCategory:
                    return Category.ArtisanAndCooking;

                case SObject.sellAtPierresAndMarnies:
                case SObject.meatCategory:
                case SObject.EggCategory:
                case SObject.MilkCategory:
                case SObject.monsterLootCategory:
                    return Category.AnimalAndMonster;

                case SObject.furnitureCategory:
                    return Category.Decorating;
            }

            // artifacts
            if ((item as SObject)?.Type == "Arch")
                return Category.ArtifactsAndMinerals;

            // anything else
            return Category.Misc;
        }

        /// <summary>Get an action wrapper which invokes <see cref="ItemGrabMenu.draw(SpriteBatch)"/>.</summary>
        /// <remarks>See remarks on <see cref="BaseDraw"/>.</remarks>
        private Action<SpriteBatch> GetBaseDraw()
        {
            MethodInfo method = typeof(ItemGrabMenu).GetMethod("draw", BindingFlags.Instance | BindingFlags.Public, null, new Type[] { typeof(SpriteBatch) }, null) ?? throw new InvalidOperationException($"Can't find {nameof(ItemGrabMenu)}.{nameof(ItemGrabMenu.draw)} method.");
            IntPtr pointer = method.MethodHandle.GetFunctionPointer();
            return (Action<SpriteBatch>)Activator.CreateInstance(typeof(Action<SpriteBatch>), this, pointer);
        }
    }
}
