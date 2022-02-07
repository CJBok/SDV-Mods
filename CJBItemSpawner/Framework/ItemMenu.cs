using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using CJB.Common;
using CJB.Common.UI;
using CJBItemSpawner.Framework.Constants;
using CJBItemSpawner.Framework.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Objects;
using SConstants = StardewModdingAPI.Constants;
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

        /// <summary>The available category names.</summary>
        private readonly string[] Categories;

        /// <summary>Whether the menu is being displayed on Android.</summary>
        private bool IsAndroid => SConstants.TargetPlatform == GamePlatform.Android;

        /// <summary>Indented spaces to add to sort labels to make room for the sort icon.</summary>
        private readonly string SortLabelIndent;

        /// <summary>Predefined data about items.</summary>
        private readonly ModItemData Data;

        /****
        ** Config
        ****/
        /// <summary>Whether the trash can upgrade which reclaims part of the price of the destroyed items is applied in the item spawner menu too.</summary>
        private readonly bool ReclaimPriceInTrashCan;

        /****
        ** State
        ****/
        /// <summary>Handles writing to the SMAPI console and log.</summary>
        private readonly IMonitor Monitor;

        /// <summary>Manages the gamepad text entry UI.</summary>
        private readonly TextEntryManager TextEntryManager;

        /// <summary>The base draw method.</summary>
        /// <remarks>This circumvents an issue where <see cref="ItemGrabMenu.draw(SpriteBatch)"/> can't be called directly due to a conflicting overload.</remarks>
        private readonly Action<SpriteBatch> BaseDraw;

        /// <summary>The icon representing the default quality.</summary>
        private readonly Texture2D StarOutlineTexture;

        /// <summary>The icon for the sort button.</summary>
        private readonly Texture2D SortTexture;

        /// <summary>The current item quality.</summary>
        private ItemQuality Quality = ItemQuality.Normal;

        /// <summary>The search text for which to filter items.</summary>
        private string SearchText = "";

        /// <summary>The field by which to sort items.</summary>
        private ItemSort SortBy = ItemSort.DisplayName;

        /// <summary>All items that can be spawned.</summary>
        private readonly SpawnableItem[] AllItems;

        /// <summary>The items matching the current search filters, without scrolling.</summary>
        private readonly List<SpawnableItem> FilteredItems = new();

        /// <summary>The items currently visible in the UI.</summary>
        private readonly IList<Item> ItemsInView;

        /// <summary>The index of the top row shown in the UI view, used to scroll through the results.</summary>
        private int TopRowIndex;

        /// <summary>The maximum <see cref="TopRowIndex"/> value for the current number of items.</summary>
        private int MaxTopRowIndex;

        /// <summary>Whether the player can scroll up in the list.</summary>
        private bool CanScrollUp => this.TopRowIndex > 0;

        /// <summary>Whether the player can scroll down in the list.</summary>
        private bool CanScrollDown => this.TopRowIndex < this.MaxTopRowIndex;

        /// <summary>Whether the user explicitly selected the textbox by clicking on it, so the selection should be maintained.</summary>
        private bool IsSearchBoxSelectedExplicitly;

        /****
        ** UI components
        ****/
        /// <summary>An icon shown next to the sort button.</summary>
        private ClickableTextureComponent SortIcon;

        /// <summary>A button which toggles between sort criteria.</summary>
        private ClickableComponent SortButton;

        /// <summary>The bounds for the quality button background.</summary>
        private ClickableComponent QualityButton;

        /// <summary>A dropdown list to choose a category filter.</summary>
        private Dropdown<string> CategoryDropdown;

        /// <summary>The up arrow to scroll results.</summary>
        private ClickableTextureComponent UpArrow;

        /// <summary>The down arrow to scroll results.</summary>
        private ClickableTextureComponent DownArrow;

        /// <summary>The search textbox.</summary>
        private TextBox SearchBox;

        /// <summary>A clickable component corresponding to the <see cref="SearchBox"/> area. This only exists for controller movement support.</summary>
        private ClickableComponent SearchBoxArea;

        /// <summary>The search textbox area.</summary>
        private Rectangle SearchBoxBounds;

        /// <summary>The search icon for the search box.</summary>
        private ClickableTextureComponent SearchIcon;

        /// <summary>The current opacity of the <see cref="SearchIcon"/>, which fades out when selected and back in when deselected.</summary>
        private float SearchIconOpacity = 1f;

        /// <summary>Whether the search icon is transitioning between the selected/unselected states.</summary>
        private bool IsSearchBoxSelectionChanging => this.SearchIconOpacity is > 0 and < 1;


        /*********
        ** Accessors
        *********/
        /// <summary>The child components for controller snapping.</summary>
        /// <remarks>This must be public and match a type supported by <see cref="IClickableMenu.populateClickableComponentList"/>.</remarks>
        public readonly List<ClickableComponent> ChildComponents = new();


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="spawnableItems">The items available to spawn.</param>
        /// <param name="textEntryManager">Manages the gamepad text entry UI.</param>
        /// <param name="data">Predefined data about items.</param>
        /// <param name="content">The content helper for loading assets.</param>
        /// <param name="monitor">Handles writing to the SMAPI console and log.</param>
        /// <param name="reclaimPriceInTrashCan">Whether the trash can upgrade which reclaims part of the price of the destroyed items is applied in the item spawner menu too.</param>
        public ItemMenu(SpawnableItem[] spawnableItems, TextEntryManager textEntryManager, ModItemData data, IModContentHelper content, IMonitor monitor, bool reclaimPriceInTrashCan)
            : base(
                inventory: new List<Item>(),
                reverseGrab: false,
                showReceivingMenu: true,
                highlightFunction: _ => true,
                behaviorOnItemGrab: (_, _) => { },
                behaviorOnItemSelectFunction: (_, _) => { },
                message: null,
                canBeExitedWithKey: true,
                showOrganizeButton: false,
                source: SConstants.TargetPlatform == GamePlatform.Android ? ItemGrabMenu.source_chest : ItemGrabMenu.source_none // needed on Android to avoid a malformed UI
            )
        {
            // init settings
            this.TextEntryManager = textEntryManager;
            this.Data = data;
            this.Monitor = monitor;
            this.BaseDraw = this.GetBaseDraw();
            this.ItemsInView = this.ItemsToGrabMenu.actualInventory;
            this.AllItems = spawnableItems;
            this.Categories = this.GetDisplayCategories(spawnableItems).ToArray();
            this.ReclaimPriceInTrashCan = reclaimPriceInTrashCan;

            // init assets
            this.StarOutlineTexture = content.Load<Texture2D>("assets/empty-quality.png");
            this.SortTexture = content.Load<Texture2D>("assets/sort.png");
            this.SortLabelIndent = this.GetSpaceIndent(Game1.smallFont, this.SortTexture.Width) + " ";

            // init base UI
            this.drawBG = false; // handled manually to draw arrows between background and menu on Android, and fix UI scaling issues on all platforms
            this.behaviorOnItemGrab = this.OnItemGrab;

            // init custom UI
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
                this.TrashHeldItem();

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
            else if (this.CategoryDropdown.TryClick(x, y, out bool itemClicked, out bool dropdownToggled))
            {
                if (dropdownToggled)
                    this.SetDropdown(this.CategoryDropdown.IsExpanded);
                if (itemClicked)
                    this.SetCategory(this.CategoryDropdown.Selected);
            }

            // textbox
            else if (this.SearchBoxBounds.Contains(x, y))
            {
                if (!this.SearchBox.Selected || !this.IsSearchBoxSelectedExplicitly)
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
            if (this.SearchBoxBounds.Contains(x, y))
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
            bool inDropdown = this.CategoryDropdown.IsExpanded;
            bool isEscape = key == Keys.Escape;
            bool isExitButton =
                isEscape
                || Game1.options.doesInputListContain(Game1.options.menuButton, key)
                || Game1.options.doesInputListContain(Game1.options.cancelButton, key);

            // clear textbox
            if (isEscape && (this.IsSearchBoxSelectedExplicitly || (this.SearchBox.Selected && !string.IsNullOrEmpty(this.SearchBox.Text))))
            {
                this.SearchBox.Text = "";
                this.DeselectSearchBox();
            }

            // close dropdown
            else if (inDropdown && isExitButton)
                this.SetDropdown(false);

            // allow trashing any item
            else if (key == Keys.Delete && this.heldItem != null)
                this.TrashHeldItem();

            // navigate
            else if (key is Keys.Left or Keys.Right)
            {
                int direction = key == Keys.Left ? -1 : 1;
                this.NextCategory(direction);
            }

            // scroll
            else if (key is Keys.Up or Keys.Down)
            {
                int direction = key == Keys.Up ? -1 : 1;

                if (inDropdown)
                    this.CategoryDropdown.ReceiveScrollWheelAction(direction);
                else
                    this.ScrollView(direction);
            }

            // default behavior (unless we're already handling a search box selection change)
            else
            {
                bool isIgnoredExitKey = this.SearchBox.Selected && isExitButton && !isEscape;
                if (!isIgnoredExitKey && !this.IsSearchBoxSelectionChanging)
                    base.receiveKeyPress(key);
            }
        }

        /// <summary>Handle a controller button press by the player.</summary>
        /// <param name="button">The button that was pressed.</param>
        public override void receiveGamePadButton(Buttons button)
        {
            bool isExitKey = button is Buttons.B or Buttons.Y or Buttons.Start;
            bool inDropdown = this.CategoryDropdown.IsExpanded;

            // handle search box
            if (this.SearchBox.Selected)
            {
                // open on-screen keyboard
                if (button == Buttons.A)
                    Game1.showTextEntry(this.SearchBox);

                // cancel search box
                else if (isExitKey || button is Buttons.LeftShoulder or Buttons.RightShoulder)
                    this.DeselectSearchBox();
            }

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
                this.ScrollView(-direction); // higher scroll value = scroll down
        }

        /// <summary>Handle the player hovering the cursor over the menu.</summary>
        /// <param name="x">The cursor's X pixel position.</param>
        /// <param name="y">The cursor's Y pixel position.</param>
        public override void performHoverAction(int x, int y)
        {
            // handle search box selected
            if (!this.IsSearchBoxSelectedExplicitly && !Game1.options.gamepadControls && Game1.lastCursorMotionWasMouse)
            {
                bool overSearchBox = this.SearchBoxBounds.Contains(x, y);
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
            // deselect textbox when text entry closes
            if (this.SearchBox.Selected && this.TextEntryManager.JustClosed())
                this.DeselectSearchBox(snapToItems: true);

            // update state when search box is deselected
            if (this.IsSearchBoxSelectedExplicitly && !this.SearchBox.Selected)
                this.DeselectSearchBox();

            // update search text
            if (this.SearchText != this.SearchBox.Text)
            {
                this.SearchText = this.SearchBox.Text;
                this.TopRowIndex = 0;
                this.ResetItemView(rebuild: true);
            }

            // fade out search icon on search box selected, fade back in on deselected
            {
                float delta = 1.5f / time.ElapsedGameTime.Milliseconds;
                if (!this.SearchBox.Selected && this.SearchIconOpacity < 1f)
                    this.SearchIconOpacity = Math.Min(1f, this.SearchIconOpacity + delta);
                else if (this.SearchBox.Selected && this.SearchIconOpacity > 0f)
                    this.SearchIconOpacity = Math.Max(0f, this.SearchIconOpacity - delta);
            }

            // fix empty spots on Android
            if (this.IsAndroid && this.ItemsInView.Any(p => p == null!)) // deliberate null-check to fix edge case
                this.ResetItemView();

            base.update(time);
        }

        /// <summary>Draw the menu to the screen.</summary>
        /// <param name="spriteBatch">The sprite batch being drawn.</param>
        public override void draw(SpriteBatch spriteBatch)
        {
            // draw background overlay
            spriteBatch.Draw(Game1.fadeToBlackRect, new Rectangle(0, 0, Game1.uiViewport.Width, Game1.uiViewport.Height), Color.Black * 0.5f);

            // draw arrows under base UI, so tooltips are drawn over them
            if (this.CanScrollUp)
                this.UpArrow.draw(spriteBatch);
            if (this.CanScrollDown)
                this.DownArrow.draw(spriteBatch);
            this.BaseDraw(spriteBatch);

            // draw search box
            CommonHelper.DrawTab(this.SearchBoxBounds.X, this.SearchBoxBounds.Y - CommonHelper.ButtonBorderWidth / 2, this.SearchBoxBounds.Width - CommonHelper.ButtonBorderWidth * 3 / 2, this.SearchBoxBounds.Height - CommonHelper.ButtonBorderWidth, out _, drawShadow: this.IsAndroid);
            this.SearchBox.Draw(spriteBatch);
            spriteBatch.Draw(this.SearchIcon.texture, this.SearchIcon.bounds, this.SearchIcon.sourceRect, Color.White * this.SearchIconOpacity);

            // draw buttons
            CommonHelper.DrawTab(this.QualityButton.bounds.X, this.QualityButton.bounds.Y, this.QualityButton.bounds.Width - CommonHelper.ButtonBorderWidth, this.QualityButton.bounds.Height - CommonHelper.ButtonBorderWidth, out Vector2 qualityIconPos, drawShadow: this.IsAndroid);
            CommonHelper.DrawTab(this.SortButton.bounds.X, this.SortButton.bounds.Y, Game1.smallFont, this.SortButton.name, drawShadow: this.IsAndroid);
            this.SortIcon.draw(spriteBatch);

            // draw quality icon
            {
                this.GetQualityIcon(out Texture2D texture, out Rectangle sourceRect, out Color color);
                spriteBatch.Draw(texture, new Vector2(qualityIconPos.X, qualityIconPos.Y - 1 * Game1.pixelZoom), sourceRect, color, 0, Vector2.Zero, Game1.pixelZoom, SpriteEffects.None, 1f);
            }

            // draw category dropdown
            this.CategoryDropdown.Draw(spriteBatch);

            // redraw cursor over new UI
            this.drawMouse(spriteBatch);
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Get the sell price for an item.</summary>
        /// <param name="item">The item to check.</param>
        /// <returns>Returns the sell price, or <c>null</c> if it can't be sold.</returns>
        private int? GetSellPrice(Item item)
        {
            return CommonHelper.GetSellPrice(item, this.Data.ForceSellable);
        }

        /// <summary>Get the icon to draw on the quality button.</summary>
        /// <param name="texture">The texture containing the icon.</param>
        /// <param name="sourceRect">The icon's pixel area within the texture.</param>
        /// <param name="color">The icon color and transparency.</param>
        private void GetQualityIcon(out Texture2D texture, out Rectangle sourceRect, out Color color)
        {
            texture = Game1.mouseCursors;
            color = Color.White;

            switch (this.Quality)
            {
                case ItemQuality.Normal:
                    texture = this.StarOutlineTexture;
                    sourceRect = new Rectangle(0, 0, texture.Width, texture.Height);
                    color = color * 0.65f;
                    break;

                case ItemQuality.Silver:
                    sourceRect = new Rectangle(338, 400, 8, 8); // silver
                    break;

                case ItemQuality.Gold:
                    sourceRect = new Rectangle(346, 400, 8, 8); // gold
                    break;

                default:
                    sourceRect = new Rectangle(346, 392, 8, 8); // iridium
                    break;
            }
        }

        /// <summary>Get the categories to show in the UI.</summary>
        /// <param name="items">The items that can be spawned.</param>
        private IEnumerable<string> GetDisplayCategories(SpawnableItem[] items)
        {
            string all = I18n.Filter_All();
            string misc = I18n.Filter_Miscellaneous();

            HashSet<string> categories = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (SpawnableItem item in items)
            {
                if (this.EqualsCaseInsensitive(item.Category, all) || this.EqualsCaseInsensitive(item.Category, misc))
                    continue;
                categories.Add(item.Category);
            }

            yield return all;
            foreach (string category in categories.OrderBy(p => p, StringComparer.OrdinalIgnoreCase))
                yield return category;
            yield return misc;
        }

        /// <summary>Initialize the custom UI components.</summary>
        [MemberNotNull(
            nameof(ItemMenu.CategoryDropdown),
            nameof(ItemMenu.DownArrow),
            nameof(ItemMenu.QualityButton),
            nameof(ItemMenu.SearchBox),
            nameof(ItemMenu.SearchBoxArea),
            nameof(ItemMenu.SearchIcon),
            nameof(ItemMenu.SortButton),
            nameof(ItemMenu.SortIcon),
            nameof(ItemMenu.UpArrow)
        )]
        private void InitializeComponents()
        {
            // get basic positions
            int x = this.xPositionOnScreen;
            int y = this.yPositionOnScreen;
            int right = x + this.width;
            int top = this.IsAndroid
                ? y - (CommonSprites.Tab.Top.Height * Game1.pixelZoom) // at top of screen, moved up slightly to reduce overlap over items
                : y - Game1.tileSize * 2 + 10; // above menu

            // get tab sizes
            const int qualityButtonWidth = 9 * Game1.pixelZoom + CommonHelper.ButtonBorderWidth;
            int maxTabWidth = (this.width - qualityButtonWidth) / 3;
            int minSearchWidth = Math.Min((int)Game1.smallFont.MeasureString("ABCDEF").X, maxTabWidth);
            int maxSearchWidth = Math.Min((int)Game1.smallFont.MeasureString("ABCDEFGHIJKLMNOPQRSTUVWXYZ").X, maxTabWidth);
            int maxDropdownWidth = Math.Min((int)Game1.smallFont.MeasureString("ABCDEFGHIJKLMNOPQRSTUVW").X, maxTabWidth);

            // basic UI
            this.QualityButton = new ClickableComponent(new Rectangle(x - 2 * Game1.pixelZoom, top, qualityButtonWidth, qualityButtonWidth - 2), ""); // manually tweak height to align with sort button
            this.SortButton = new ClickableComponent(new Rectangle(this.QualityButton.bounds.Right + 20, top, this.GetMaxSortLabelWidth(Game1.smallFont) + CommonHelper.ButtonBorderWidth, Game1.tileSize), this.GetSortLabel(this.SortBy));
            this.SortIcon = new ClickableTextureComponent(new Rectangle(this.SortButton.bounds.X + CommonHelper.ButtonBorderWidth, top + CommonHelper.ButtonBorderWidth, this.SortTexture.Width, Game1.tileSize), this.SortTexture, new Rectangle(0, 0, this.SortTexture.Width, this.SortTexture.Height), 1f);
            this.CategoryDropdown = new Dropdown<string>(this.SortButton.bounds.Right + 20, this.SortButton.bounds.Y, Game1.smallFont, this.CategoryDropdown?.Selected ?? I18n.Filter_All(), this.Categories, p => p, maxTabWidth: maxDropdownWidth);
            this.UpArrow = new ClickableTextureComponent(new Rectangle(right - 32, y - 64, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(421, 459, 11, 12), Game1.pixelZoom);
            this.DownArrow = new ClickableTextureComponent(new Rectangle(this.UpArrow.bounds.X, this.UpArrow.bounds.Y + this.height / 2 - 64, this.UpArrow.bounds.Width, this.UpArrow.bounds.Height), Game1.mouseCursors, new Rectangle(421, 472, 11, 12), Game1.pixelZoom);

            // search box
            // aligned to the right, stretched to fit space between category dropdown and right margin up to a max width
            {
                int leftSpacing = 10 * Game1.pixelZoom; // min space between category dropdown and search box
                int searchRight = this.IsAndroid
                    ? this.upperRightCloseButton.bounds.X - 5 * Game1.pixelZoom
                    : right - 13 * Game1.pixelZoom;

                int searchWidth = Math.Clamp(searchRight - this.CategoryDropdown.bounds.Right - leftSpacing + 10, min: minSearchWidth, max: maxSearchWidth);
                int searchX = searchRight - searchWidth;

                this.SearchBox = new TextBox(Game1.content.Load<Texture2D>("LooseSprites\\textBox"), null, Game1.smallFont, Game1.textColor)
                {
                    X = searchX,
                    Y = top + 6,
                    Height = 0,
                    Width = searchWidth,
                    Text = this.SearchText
                };
                this.SearchBoxBounds = new Rectangle(this.SearchBox.X, this.SearchBox.Y + 4, this.SearchBox.Width, 12 * Game1.pixelZoom);
                this.SearchBoxArea = new ClickableComponent(new Rectangle(this.SearchBoxBounds.X, this.SearchBoxBounds.Y, this.SearchBoxBounds.Width, this.SearchBoxBounds.Height), "");
            }

            // search icon
            {
                var iconRect = new Rectangle(80, 0, 13, 13);
                float scale = 2.5f;
                var iconBounds = new Rectangle(
                    x: (int)(this.SearchBoxBounds.Right - (iconRect.Width * scale)),
                    y: (int)(this.SearchBoxBounds.Center.Y - (iconRect.Height / 2f * scale)),
                    width: (int)(iconRect.Width * scale),
                    height: (int)(iconRect.Height * scale)
                );
                this.SearchIcon = new ClickableTextureComponent(iconBounds, Game1.mouseCursors, iconRect, scale);
            }

            // move layout for Android
            if (this.IsAndroid)
            {
                this.UpArrow.bounds.X = this.upperRightCloseButton.bounds.Center.X - this.SortButton.bounds.Width / 2;
                this.UpArrow.bounds.Y = this.upperRightCloseButton.bounds.Bottom;
                this.DownArrow.bounds.X = this.UpArrow.bounds.X;
            }

            // controller flow
            this.InitializeControllerFlow();
        }

        /// <summary>Set the fields to support controller snapping.</summary>
        private void InitializeControllerFlow()
        {
            // implementation notes:
            //   - search box is deliberately excluded from controller flow since you can't enter values.
            //   - CategoryDropdown down neighbor ID is auto-managed depending on whether it's expanded.

            if (this.IsAndroid)
                return; // no controller on Android, and crashes due to Android-specific changes to the menu

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
            this.CategoryDropdown.rightNeighborID = this.SearchBoxArea.myID;
            this.SearchBoxArea.rightNeighborID = this.UpArrow.myID;
            this.UpArrow.downNeighborID = this.DownArrow.myID;

            // leftward flow across custom UI
            this.UpArrow.upNeighborID = this.SearchBoxArea.myID;
            this.UpArrow.leftNeighborID = slots[10].myID;
            this.SearchBoxArea.leftNeighborID = this.CategoryDropdown.myID;
            this.CategoryDropdown.leftNeighborID = this.SortButton.myID;
            this.SortButton.leftNeighborID = this.QualityButton.myID;

            // downward flow into inventory
            this.QualityButton.downNeighborID = slots[0].myID;
            this.SortButton.downNeighborID = slots[1].myID;
            this.CategoryDropdown.DefaultDownNeighborId = slots[5].myID;
            this.SearchBoxArea.downNeighborID = slots[10].myID;
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
            this.inventory.highlightMethod = _ => !expanded;
            this.ItemsToGrabMenu.highlightMethod = _ => !expanded;
            if (!expanded && !Game1.lastCursorMotionWasMouse)
            {
                this.setCurrentlySnappedComponentTo(this.CategoryDropdown.myID);
                this.snapCursorToCurrentSnappedComponent();
            }
        }

        /// <summary>Switch to the next category.</summary>
        /// <param name="direction">The direction to move in the category list.</param>
        protected void NextCategory(int direction)
        {
            direction = direction < 0 ? -1 : 1;
            int last = this.Categories.Length - 1;

            int index = Array.IndexOf(this.Categories, this.CategoryDropdown.Selected) + direction;
            if (index < 0)
                index = last;
            if (index > last)
                index = 0;

            this.SetCategory(this.Categories[index]);
        }

        /// <summary>Set the current category.</summary>
        /// <param name="category">The new category value.</param>
        protected void SetCategory(string category)
        {
            if (!this.CategoryDropdown.TrySelect(category))
            {
                this.Monitor.Log($"Failed selecting category filter category '{category}'.", LogLevel.Warn);
                if (category != I18n.Filter_All())
                    this.SetCategory(I18n.Filter_All());
                return;
            }

            this.ResetItemView(rebuild: true);
        }

        /// <summary>Scroll the item view.</summary>
        /// <param name="direction">The scroll direction.</param>
        /// <param name="resetItemView">Whether to update the item view.</param>
        public void ScrollView(int direction, bool resetItemView = true)
        {
            // apply scroll
            if (direction < 0)
                this.TopRowIndex--;
            else if (direction > 0)
                this.TopRowIndex++;

            // normalize scroll
            this.TopRowIndex = MathHelper.Clamp(this.TopRowIndex, 0, this.MaxTopRowIndex);

            // update view
            if (resetItemView)
                this.ResetItemView();
        }

        /// <summary>Set the search textbox selected.</summary>
        /// <param name="explicitly">Whether the textbox was selected explicitly by the user (rather than automatically by hovering), so the selection should be maintained.</param>
        private void SelectSearchBox(bool explicitly)
        {
            this.SearchBox.Selected = true;
            this.IsSearchBoxSelectedExplicitly = explicitly;
            this.SearchBox.Width = this.SearchBoxBounds.Width;
        }

        /// <summary>Set the search textbox non-selected.</summary>
        /// <param name="snapToItems">Whether to snap the gamepad cursor to the item grid, if applicable.</param>
        private void DeselectSearchBox(bool snapToItems = false)
        {
            Game1.closeTextEntry();

            this.SearchBox.Selected = false;
            this.IsSearchBoxSelectedExplicitly = false;
            this.SearchBox.Width = this.SearchIcon.bounds.X - this.SearchBox.X + this.SearchIcon.bounds.Width + 10 - this.SearchIcon.bounds.Width - 6 * Game1.pixelZoom;

            if (snapToItems && Game1.options.gamepadControls && !Game1.lastCursorMotionWasMouse)
            {
                this.setCurrentlySnappedComponentTo(this.ItemsToGrabMenu.inventory.First().myID);
                this.snapCursorToCurrentSnappedComponent();
            }
        }

        /// <summary>Reset the items shown in the view.</summary>
        /// <param name="rebuild">Whether to rebuild the search results.</param>
        private void ResetItemView(bool rebuild = false)
        {
            // rebuild underlying list
            if (rebuild)
            {
                this.FilteredItems.Clear();
                this.FilteredItems.AddRange(this.SearchItems());
                this.TopRowIndex = 0;
            }

            // fix scroll if needed
            int totalRows = (int)Math.Ceiling(this.FilteredItems.Count / (this.ItemsPerRow * 1m));
            this.MaxTopRowIndex = Math.Max(0, totalRows - 3);
            this.ScrollView(0, resetItemView: false);

            // update items in view
            this.ItemsInView.Clear();
            foreach (SpawnableItem match in this.FilteredItems.Skip(this.TopRowIndex * this.ItemsPerRow).Take(this.ItemsPerView))
            {
                Item item = match.CreateItem();
                item.Stack = item.maximumStackSize();
                if (item is SObject obj && !this.ItemsWithoutQuality.Contains(obj.ParentSheetIndex))
                    obj.Quality = (int)this.Quality;

                this.ItemsInView.Add(item);
            }
        }

        /// <summary>Get all items matching the search criteria, ignoring pagination.</summary>
        private IEnumerable<SpawnableItem> SearchItems()
        {
            // get base query
            IEnumerable<SpawnableItem> items = this.AllItems;
            items = this.SortBy switch
            {
                ItemSort.Price => items.OrderByDescending(p => this.GetSellPrice(p.Item) ?? -1).ThenBy(p => p.Item.DisplayName),
                ItemSort.Type => items.OrderBy(p => p.Item.Category),
                ItemSort.ID => items.OrderBy(p => p.Item.ParentSheetIndex),
                _ => items.OrderBy(p => p.Item.DisplayName)
            };

            // apply menu tab
            if (!this.EqualsCaseInsensitive(this.CategoryDropdown.Selected, I18n.Filter_All()))
                items = items.Where(item => this.EqualsCaseInsensitive(item.Category, this.CategoryDropdown.Selected));

            // apply search
            string search = this.SearchBox.Text.Trim();
            if (search != "")
            {
                items = items.Where(item =>
                    item.Name.Contains(search, StringComparison.InvariantCultureIgnoreCase)
                    || item.DisplayName.Contains(search, StringComparison.InvariantCultureIgnoreCase)
                );
            }

            return items;
        }

        /// <summary>Get a string space-indented to the given width.</summary>
        /// <param name="font">The font with which to measure the indent size.</param>
        /// <param name="width">The minimum indent width in pixels.</param>
        private string GetSpaceIndent(SpriteFont font, int width)
        {
            if (width <= 0)
                return "";

            string indent = " ";
            while (font.MeasureString(indent).X < width)
                indent += " ";

            return indent;
        }

        /// <summary>Get the translated label for a sort type.</summary>
        /// <param name="sort">The sort type.</param>
        private string GetSortLabel(ItemSort sort)
        {
            return this.SortLabelIndent + sort switch
            {
                ItemSort.DisplayName => I18n.Sort_ByName(),
                ItemSort.Price => I18n.Sort_ByPrice(),
                ItemSort.Type => I18n.Sort_ByType(),
                ItemSort.ID => I18n.Sort_ById(),
                _ => throw new NotSupportedException($"Invalid sort type {sort}.")
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

        /// <summary>Get an action wrapper which invokes <see cref="ItemGrabMenu.draw(SpriteBatch)"/>.</summary>
        /// <remarks>See remarks on <see cref="BaseDraw"/>.</remarks>
        private Action<SpriteBatch> GetBaseDraw()
        {
            MethodInfo method = typeof(ItemGrabMenu).GetMethod("draw", BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(SpriteBatch) }, null) ?? throw new InvalidOperationException($"Can't find {nameof(ItemGrabMenu)}.{nameof(ItemGrabMenu.draw)} method.");
            IntPtr pointer = method.MethodHandle.GetFunctionPointer();
            return (Action<SpriteBatch>)Activator.CreateInstance(typeof(Action<SpriteBatch>), this, pointer)!;
        }

        /// <summary>Destroy the held item through the in-game trash can.</summary>
        /// <remarks>Derived from <see cref="Utility.trashItem"/>, with the option to avoid reclaiming the price.</remarks>
        private void TrashHeldItem()
        {
            Item item = this.heldItem;
            if (item is null)
                return;

            if (item is SObject obj && Game1.player.specialItems.Contains(obj.ItemId))
                Game1.player.specialItems.Remove(obj.ItemId);
            if (this.ReclaimPriceInTrashCan)
            {
                int price = Utility.getTrashReclamationPrice(item, Game1.player);
                if (price > 0)
                    Game1.player.Money += price;
            }
            Game1.playSound("trashcan");

            this.heldItem = null;
        }

        /// <summary>Get whether two strings are equal, ignoring case differences.</summary>
        /// <param name="a">The first string to compare.</param>
        /// <param name="b">The second string to compare.</param>
        private bool EqualsCaseInsensitive(string a, string b)
        {
            return string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
        }
    }
}
