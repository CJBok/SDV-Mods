using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using System.Linq;

namespace CJBCheatsMenu.Framework
{
    internal class CheatsMenu : IClickableMenu, View.IOverlayController
    {
        /*********
        ** Properties
        *********/
        /// <summary>The mod settings.</summary>
        private readonly ModConfig Config;

        /// <summary>Provides translations for the mod.</summary>
        private readonly ITranslationHelper TranslationHelper;

        /// <summary>
        /// The manager that maintains a list of all the currently registered menus.
        /// </summary>
        private readonly MenuManager MenuManager;

        /// <summary>
        /// The scrollbar for the menu tabs.
        /// </summary>
        private View.ViewScrollbar TabsScrollbar { get; set; }

        /// <summary>
        /// The scroll area for the menu tabs.
        /// </summary>
        private View.ViewScrollableContents<View.ViewMenuTab> TabsScrollArea { get; set; }

        /// <summary>
        /// The views that are subviews of this menu.
        /// </summary>
        private List<View.IView> Children { get; set; } = new List<View.IView>();

        /// <summary>
        /// The child views that are scrollable.
        /// </summary>
        private List<View.IViewScrollable> Scrollables { get; set; } = new List<View.IViewScrollable>();

        /// <summary>
        /// The title of the menu.
        /// </summary>
        private readonly ClickableComponent Title;

        /// <summary>
        /// Ensures the first 'P' button press that opened this menu doesn't immediately close it.
        /// </summary>
        private bool CanClose = false;

        /// <summary>
        /// The number of items in a scroll area page.
        /// </summary>
        private const int ItemsPerPage = 10;

        /// <summary>
        /// The current view that the left click is being held down on.
        /// </summary>
        private View.IView ViewHeld  { get; set; } = null;

        /// <summary>
        /// The menus (for which there are tabs rendered for).
        /// </summary>
        private IReadOnlyList<Menu.IMenu> Menus => MenuManager.Menus;

        /// <summary>
        /// A label that represents what is displayed when the dark overlay is displayed.
        /// </summary>
        /// <remarks>
        /// This is currently used for displaying some text when the key picker is listening for input.
        /// </remarks>
        private string OverlayLabel { get; set; }

        /// <summary>
        /// The width of a row where options are rendered.
        /// </summary>
        private int OptionRowWidth => this.width - Game1.tileSize / 2;

        /// <summary>
        /// The height of a row where the options are being rendered.
        /// </summary>
        private int OptionRowHeight => 9 * StardewValley.Game1.pixelZoom;

        /// <summary>
        /// The id of the menu that is currently selected.
        /// </summary>
        /// <remarks>
        /// Returns an empty string if there are no currently selected menus.
        /// </remarks>
        private string CurrentMenuId
        {
            get
            {
                if (this.CurrentMenu != null)
                {
                    return this.CurrentMenu.Id;
                }
                else
                {
                    return "";
                }
            }

            set
            {
                if (this.MenuManager.HasRegisteredMenu(value))
                {
                    this.CurrentMenu = this.MenuManager.GetRegisteredMenu(value);
                }
                else if (this.Menus.Count > 0)
                {
                    this.CurrentMenu = this.Menus.First();
                }
                else
                {
                    this.CurrentMenu = null;
                }
            }
        }

        /// <summary>
        /// The current menu that is selected.
        /// </summary>
        /// <remarks>
        /// null if there is currently no selected menu.
        /// </remarks>
        private Menu.IMenu CurrentMenu { get; set; }

        /// <summary>
        /// Reterns a view for the passed in option.
        /// </summary>
        /// <remarks>
        /// I wish I didn't have to upcast here, see stack overflow question for reasoning: https://stackoverflow.com/q/50472035/3154314
        /// </remarks>
        /// <param name="option">The option to get the view for.</param>
        /// <returns>A view to render for this option.</returns>
        private View.IViewGroupItem GetViewForOption(Menu.IOption option)
        {
            Menu.IOptionCheckbox checkboxOption = option as Menu.IOptionCheckbox;
            if (checkboxOption != null)
            {
                return new View.ViewOptionCheckbox(checkboxOption);
            }

            Menu.IOptionSetButton setButtonOption = option as Menu.IOptionSetButton;
            if (setButtonOption != null)
            {
                return new View.ViewOptionSetButton(setButtonOption, this.OptionRowWidth);
            }

            Menu.IOptionSlider sliderOption = option as Menu.IOptionSlider;
            if (sliderOption != null)
            {
                return new View.ViewOptionSlider(sliderOption);
            }

            Menu.IOptionHeartPicker heartPickerOption = option as Menu.IOptionHeartPicker;
            if (heartPickerOption != null)
            {
                return new View.ViewOptionHeartPicker(heartPickerOption);
            }

            Menu.IOptionKeyPicker keyPickerOption = option as Menu.IOptionKeyPicker;
            if (keyPickerOption != null)
            {
                return new View.ViewOptionKeyPicker(keyPickerOption, this.OptionRowWidth, this, this.TranslationHelper);
            }

            return new View.ViewOption<Menu.IOption>(option, this.OptionRowWidth, this.OptionRowHeight);
        }

        /// <summary>
        /// Adds a child to the list of child views and adds it to the list of scrollables if it is scrollable.
        /// </summary>
        /// <param name="child">The child to add.</param>
        private void AddChild(View.IView child)
        {
            this.Children.Add(child);
            View.IViewScrollable scrollable = child as View.IViewScrollable;
            if (scrollable != null)
            {
                this.Scrollables.Add(scrollable);
            }
        }

        /// <summary>
        /// Creates the tabs scroll area and scrollbar.
        /// </summary>
        /// <param name="currentScrollerTabPosition">The current position of the scrollbar.</param>
        private void CreateTabsScrollArea(int currentScrollerTabPosition)
        {
            int tabWidth = Game1.tileSize * 4;

            List<View.ViewMenuTab> menuTabs = new List<View.ViewMenuTab>();
            int selectedTabIndex = 0;
            for (int i = 0; i < this.Menus.Count; i++)
            {
                Menu.IMenu menu = this.Menus[i];
                View.ViewMenuTab menuTab = new View.ViewMenuTab(menu, tabWidth, this.OnMenuTabPressed);
                if (menu.Id == this.CurrentMenuId)
                {
                    menuTab.Intensify = true;
                    selectedTabIndex = i;
                }
                menuTabs.Add(menuTab);
            }

            this.TabsScrollbar = new View.ViewScrollbar(currentScrollerTabPosition, CheatsMenu.ItemsPerPage, this.xPositionOnScreen - tabWidth - 11 * Game1.pixelZoom, this.yPositionOnScreen + Game1.tileSize, this.height - Game1.tileSize * 2 + Game1.pixelZoom * CheatsMenu.ItemsPerPage, menuTabs.Count);
            if (selectedTabIndex < this.TabsScrollbar.CurrentVisibleStartIndex ||
                selectedTabIndex > this.TabsScrollbar.CurrentVisibleEndIndex)
            {
                this.TabsScrollbar.ScrollTo(selectedTabIndex);
            }
            Rectangle scrollableTabsBounds = new Rectangle(this.TabsScrollbar.Bounds.X + this.TabsScrollbar.Bounds.Width, this.TabsScrollbar.Bounds.Y, tabWidth, this.TabsScrollbar.Bounds.Height);
            this.AddChild(this.TabsScrollbar);
            this.TabsScrollArea = new View.ViewScrollableContents<View.ViewMenuTab>(this.TabsScrollbar, menuTabs, scrollableTabsBounds);
            this.AddChild(this.TabsScrollArea);
        }

        /// <summary>
        /// Create the scroll area and scrollbar for the options for the currently selected menu.
        /// </summary>
        private void CreateOptionsScrollArea()
        {
            if (this.CurrentMenu != null)
            {
                List<View.IViewGroupItem> options = new List<View.IViewGroupItem>();
                foreach (Menu.IOptionGroup group in this.CurrentMenu.OptionGroups)
                {
                    options.Add(new View.ViewOptionGroupHeader(group));
                    foreach (Menu.IOption option in group.Options)
                    {
                        View.IViewGroupItem optionView = this.GetViewForOption(option);
                        options.Add(optionView);
                    }
                }

                View.ViewScrollbar scrollbar = new View.ViewScrollbar(0, CheatsMenu.ItemsPerPage, this.xPositionOnScreen + this.width + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize, this.height - Game1.tileSize * 2);
                this.AddChild(scrollbar);
                Rectangle optionsScrollAreaBounds = new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize * 5 / 4 + Game1.pixelZoom, this.OptionRowWidth, (this.height - Game1.tileSize * 2) + Game1.pixelZoom);
                this.AddChild(new View.ViewScrollableContents<View.IViewGroupItem>(scrollbar, options, optionsScrollAreaBounds));
            }
        }

        /// <summary>
        /// Constructor for the cheats menu.
        /// </summary>
        /// <param name="currentMenuId">The id of the menu to open.</param>
        /// <param name="tabScrollerPosition">The position the tabs scroller is scrolled to when first opened.</param>
        /// <param name="menuManager">The manager that keeps track of the currently registered menus.</param>
        /// <param name="config">The cheats config that holds all the users current preferences.</param>
        /// <param name="i18n">Internaltionalization helper.</param>
        public CheatsMenu(string currentMenuId, int tabScrollerPosition, MenuManager menuManager, ModConfig config, ITranslationHelper i18n)
          : base(Game1.viewport.Width / 2 - (600 + IClickableMenu.borderWidth * 2) / 2, Game1.viewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2, 800 + IClickableMenu.borderWidth * 2, 600 + IClickableMenu.borderWidth * 2)
        {
            this.Config = config;
            this.TranslationHelper = i18n;
            this.MenuManager = menuManager;
            this.CurrentMenuId = currentMenuId;

            this.Title = new ClickableComponent(new Rectangle(this.xPositionOnScreen + this.width / 2, this.yPositionOnScreen, Game1.tileSize * 4, Game1.tileSize), i18n.Get("title"));

            int labelX = (int)(this.xPositionOnScreen - Game1.tileSize * 4.8f);
            int labelY = (int)(this.yPositionOnScreen + Game1.tileSize * 1.5f);

            this.CreateOptionsScrollArea();
            this.CreateTabsScrollArea(tabScrollerPosition);

        }

        /// <summary>
        /// Shows the dark overlay over this menu with a white message in the middle
        /// </summary>
        /// <param name="overlayLabel">The message to display in the overlay.</param>
        public void ShowOverlay(string overlayLabel)
        {
            GameMenu.forcePreventClose = true;
            this.OverlayLabel = overlayLabel;
        }

        /// <summary>
        /// Hides the overlay.
        /// </summary>
        public void HideOverlay()
        {
            GameMenu.forcePreventClose = false;
            this.OverlayLabel = null;
        }

        /// <summary>
        /// Callback when a tab is pressed.
        /// </summary>
        /// <param name="menu">The menu related to the tab that was pressed.</param>
        public void OnMenuTabPressed(Menu.IMenu menu)
        {
            Game1.activeClickableMenu = new CheatsMenu(menu.Id, this.TabsScrollbar.CurrentVisibleStartIndex, this.MenuManager, this.Config, this.TranslationHelper);
        }

        /// <summary>
        /// Handle the left click event.
        /// </summary>
        /// <param name="x">x position of mouse when left click occured.</param>
        /// <param name="y">y position of mouse when left click occured.</param>
        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            if (GameMenu.forcePreventClose)
                return;

            foreach (View.IView child in this.Children)
            {
                if (child.Bounds.Contains(x, y))
                {
                    child.ReceiveLeftClick(x, y);
                    this.ViewHeld = child;
                }
            }
        }

        /// <summary>
        /// Handle the left click release event.
        /// </summary>
        /// <param name="x">x position of mouse when left click release occured.</param>
        /// <param name="y">y position of mouse when left click release occured.</param>
        public override void releaseLeftClick(int x, int y)
        {
            if (GameMenu.forcePreventClose)
                return;

            base.releaseLeftClick(x, y);
            if (this.ViewHeld != null)
            {
                this.ViewHeld.LeftClickReleased(x, y);
            }
            this.ViewHeld = null;
        }

        /// <summary>
        /// Handle the left click held event.
        /// </summary>
        /// <param name="x">x position of mouse during the left click hold.</param>
        /// <param name="y">y position of mouse during the left click hold.</param>
        public override void leftClickHeld(int x, int y)
        {
            if (GameMenu.forcePreventClose)
                return;
            base.leftClickHeld(x, y);
            
            if (this.ViewHeld != null)
            {
                this.ViewHeld.LeftClickHeld(x, y);
            }
        }

        /// <summary>
        /// Handles a key press event.
        /// </summary>
        /// <param name="key">The key that was pressed.</param>
        public override void receiveKeyPress(Keys key)
        {
            if (!GameMenu.forcePreventClose && (Game1.options.menuButton.Contains(new InputButton(key)) || key.ToString() == this.Config.OpenMenuKey) && this.readyToClose() && this.CanClose)
            {
                Game1.exitActiveMenu();
                Game1.soundBank.PlayCue("bigDeSelect");
                return;
            }

            this.CanClose = true;

            foreach (View.IView child in this.Children)
            {
                child.ReceiveKeyPress(key);
            }
        }

        /// <summary>
        /// Handles game pad button press event.
        /// </summary>
        /// <param name="key">The gamepad button that was pressed.</param>
        public override void receiveGamePadButton(Buttons key)
        {
            if (key == Buttons.LeftShoulder || key == Buttons.RightShoulder)
            {
                if (key == Buttons.RightShoulder)
                {
                    this.TabsScrollArea.SelectNext();
                } else if (key == Buttons.LeftShoulder)
                {
                    this.TabsScrollArea.SelectPrevious();
                }
                
                // open menu with new index
                Game1.activeClickableMenu = new CheatsMenu(this.TabsScrollArea.CurrentlySelected.Menu.Id, this.TabsScrollbar.CurrentVisibleStartIndex, this.MenuManager, this.Config, this.TranslationHelper);
            }
        }

        /// <summary>
        /// Handles the scroll wheel action.
        /// </summary>
        /// <param name="direction">The direction of the scroll wheel action.</param>
        public override void receiveScrollWheelAction(int direction)
        {
            if (GameMenu.forcePreventClose)
                return;
            base.receiveScrollWheelAction(direction);

            Point mousePosition = Game1.getMousePosition();
            foreach (View.IViewScrollable scrollable in this.Scrollables)
            {
                if (scrollable.Bounds.Contains(mousePosition))
                {
                    scrollable.ReceiveScrollWheelAction(direction);
                    return;
                }
            }
        }

        public override void receiveRightClick(int x, int y, bool playSound = true) { }

        public override void performHoverAction(int x, int y)
        {
            if (GameMenu.forcePreventClose)
                return;
        }

        /// <summary>
        /// Draws the cheats menu.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch used to render assets.</param>
        public override void draw(SpriteBatch spriteBatch)
        {
            if (!Game1.options.showMenuBackground)
                spriteBatch.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.4f);

            Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true);
            CJB.DrawTextBox(this.Title.bounds.X, this.Title.bounds.Y, Game1.dialogueFont, this.Title.name, 1);
            foreach (View.IView child in this.Children)
            {
                child.Draw(spriteBatch);
            }

            if (this.OverlayLabel != null)
            {
                spriteBatch.Draw(StardewValley.Game1.staminaRect, new Rectangle(0, 0, StardewValley.Game1.graphics.GraphicsDevice.Viewport.Width, StardewValley.Game1.graphics.GraphicsDevice.Viewport.Height), new Rectangle(0, 0, 1, 1), Color.Black * 0.75f, 0.0f, Vector2.Zero, SpriteEffects.None, 0.009f);
                spriteBatch.DrawString(StardewValley.Game1.dialogueFont, this.OverlayLabel, StardewValley.Utility.getTopLeftPositionForCenteringOnScreen(StardewValley.Game1.tileSize * 3, StardewValley.Game1.tileSize), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0009f);
            }

            if (!Game1.options.hardwareCursor)
                spriteBatch.Draw(Game1.mouseCursors, new Vector2(Game1.getOldMouseX(), Game1.getOldMouseY()), Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, Game1.options.gamepadControls ? 44 : 0, 16, 16), Color.White, 0f, Vector2.Zero, Game1.pixelZoom + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 1f);
        }
    }
}
