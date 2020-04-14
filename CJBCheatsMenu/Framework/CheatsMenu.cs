using System;
using System.Collections.Generic;
using System.Linq;
using CJB.Common;
using CJBCheatsMenu.Framework.Cheats;
using CJBCheatsMenu.Framework.Components;
using CJBCheatsMenu.Framework.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework
{
    /// <summary>An interactive menu for configuring and toggling cheats.</summary>
    internal class CheatsMenu : IClickableMenu
    {
        /*********
        ** Fields
        *********/
        /// <summary>Manages the cheat implementations.</summary>
        private readonly CheatManager Cheats;

        /// <summary>Encapsulates monitoring and logging.</summary>
        private readonly IMonitor Monitor;

        private readonly List<ClickableComponent> OptionSlots = new List<ClickableComponent>();
        private readonly List<OptionsElement> Options = new List<OptionsElement>();
        private ClickableTextureComponent UpArrow;
        private ClickableTextureComponent DownArrow;
        private ClickableTextureComponent Scrollbar;
        private readonly List<ClickableComponent> Tabs = new List<ClickableComponent>();
        private ClickableComponent Title;
        private const int ItemsPerPage = 10;

        /// <summary>Whether the mod is running on Android.</summary>
        private readonly bool IsAndroid = Constants.TargetPlatform == GamePlatform.Android;

        private string HoverText = "";
        private int OptionsSlotHeld = -1;
        private int CurrentItemIndex;
        private bool IsScrolling;
        private Rectangle ScrollbarRunner;
        private bool CanClose;

        /// <summary>Whether the menu was opened in the current tick.</summary>
        private bool JustOpened = true;

        /// <summary>The currently open tab.</summary>
        private readonly MenuTab CurrentTab;


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="initialTab">The tab to display by default.</param>
        /// <param name="cheats">The cheats helper.</param>
        /// <param name="monitor">Encapsulates monitoring and logging.</param>
        public CheatsMenu(MenuTab initialTab, CheatManager cheats, IMonitor monitor)
        {
            this.Cheats = cheats;
            this.Monitor = monitor;
            this.CurrentTab = initialTab;
            this.ResetComponents();
            this.SetOptions();
        }

        /// <summary>Whether controller-style menus should be disabled for this menu.</summary>
        public override bool overrideSnappyMenuCursorMovementBan()
        {
            return true;
        }

        /// <summary>Handle the game window being resized.</summary>
        /// <param name="oldBounds">The previous window bounds.</param>
        /// <param name="newBounds">The new window bounds.</param>
        public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
        {
            this.ResetComponents();
        }

        /// <summary>Handle the player holding the left mouse button.</summary>
        /// <param name="x">The cursor's X pixel position.</param>
        /// <param name="y">The cursor's Y pixel position.</param>
        public override void leftClickHeld(int x, int y)
        {
            if (GameMenu.forcePreventClose)
                return;
            base.leftClickHeld(x, y);
            if (this.IsScrolling)
            {
                int num = this.Scrollbar.bounds.Y;
                this.Scrollbar.bounds.Y = Math.Min(this.yPositionOnScreen + this.height - Game1.tileSize - Game1.pixelZoom * 3 - this.Scrollbar.bounds.Height, Math.Max(y, this.yPositionOnScreen + this.UpArrow.bounds.Height + Game1.pixelZoom * 5));
                this.CurrentItemIndex = Math.Min(this.Options.Count - CheatsMenu.ItemsPerPage, Math.Max(0, (int)Math.Round((this.Options.Count - CheatsMenu.ItemsPerPage) * (double)((y - this.ScrollbarRunner.Y) / (float)this.ScrollbarRunner.Height))));
                this.SetScrollBarToCurrentIndex();
                if (num == this.Scrollbar.bounds.Y)
                    return;
                Game1.soundBank.PlayCue("shiny4");
            }
            else
            {
                if (this.OptionsSlotHeld == -1 || this.OptionsSlotHeld + this.CurrentItemIndex >= this.Options.Count)
                    return;
                this.Options[this.CurrentItemIndex + this.OptionsSlotHeld].leftClickHeld(x - this.OptionSlots[this.OptionsSlotHeld].bounds.X, y - this.OptionSlots[this.OptionsSlotHeld].bounds.Y);
            }
        }

        /// <summary>Handle the player pressing a keyboard button.</summary>
        /// <param name="key">The key that was pressed.</param>
        public override void receiveKeyPress(Keys key)
        {
            SButton openMenuKey = this.Cheats.Context.Config.OpenMenuKey;
            bool isExitKey = Game1.options.menuButton.Contains(new InputButton(key)) || (openMenuKey.TryGetKeyboard(out Keys exitKey) && key == exitKey);
            if (isExitKey && this.readyToClose() && this.CanClose && !GameMenu.forcePreventClose)
            {
                Game1.exitActiveMenu();
                Game1.soundBank.PlayCue("bigDeSelect");
                return;
            }

            this.CanClose = true;

            if (this.OptionsSlotHeld == -1 || this.OptionsSlotHeld + this.CurrentItemIndex >= this.Options.Count)
                return;
            this.Options[this.CurrentItemIndex + this.OptionsSlotHeld].receiveKeyPress(key);
        }

        /// <summary>Handle the player pressing a controller button.</summary>
        /// <param name="key">The key that was pressed.</param>
        public override void receiveGamePadButton(Buttons key)
        {
            if (key == Buttons.LeftShoulder || key == Buttons.RightShoulder)
            {
                // rotate tab index
                int index = this.Tabs.FindIndex(p => p.name == this.CurrentTab.ToString());
                if (key == Buttons.LeftShoulder)
                    index--;
                if (key == Buttons.RightShoulder)
                    index++;

                if (index >= this.Tabs.Count)
                    index = 0;
                if (index < 0)
                    index = this.Tabs.Count - 1;

                // open menu with new index
                MenuTab tabID = this.GetTabID(this.Tabs[index]);
                Game1.activeClickableMenu = new CheatsMenu(tabID, this.Cheats, this.Monitor);
            }
        }

        /// <summary>Handle the player scrolling the mouse wheel.</summary>
        /// <param name="direction">The scroll direction.</param>
        public override void receiveScrollWheelAction(int direction)
        {
            if (GameMenu.forcePreventClose)
                return;
            base.receiveScrollWheelAction(direction);
            if (direction > 0 && this.CurrentItemIndex > 0)
                this.UpArrowPressed();
            else
            {
                if (direction >= 0 || this.CurrentItemIndex >= Math.Max(0, this.Options.Count - CheatsMenu.ItemsPerPage))
                    return;
                this.DownArrowPressed();
            }
        }

        /// <summary>Handle the player releasing the left mouse button.</summary>
        /// <param name="x">The cursor's X pixel position.</param>
        /// <param name="y">The cursor's Y pixel position.</param>
        public override void releaseLeftClick(int x, int y)
        {
            if (GameMenu.forcePreventClose)
                return;
            base.releaseLeftClick(x, y);
            if (this.OptionsSlotHeld != -1 && this.OptionsSlotHeld + this.CurrentItemIndex < this.Options.Count)
                this.Options[this.CurrentItemIndex + this.OptionsSlotHeld].leftClickReleased(x - this.OptionSlots[this.OptionsSlotHeld].bounds.X, y - this.OptionSlots[this.OptionsSlotHeld].bounds.Y);
            this.OptionsSlotHeld = -1;
            this.IsScrolling = false;
        }

        /// <summary>Handle the player clicking the left mouse button.</summary>
        /// <param name="x">The cursor's X pixel position.</param>
        /// <param name="y">The cursor's Y pixel position.</param>
        /// <param name="playSound">Whether to play a sound if needed.</param>
        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            if (GameMenu.forcePreventClose)
                return;
            base.receiveLeftClick(x, y, playSound);

            if (this.DownArrow.containsPoint(x, y) && this.CurrentItemIndex < Math.Max(0, this.Options.Count - CheatsMenu.ItemsPerPage))
            {
                this.DownArrowPressed();
                Game1.soundBank.PlayCue("shwip");
            }
            else if (this.UpArrow.containsPoint(x, y) && this.CurrentItemIndex > 0)
            {
                this.UpArrowPressed();
                Game1.soundBank.PlayCue("shwip");
            }
            else if (this.Scrollbar.containsPoint(x, y))
                this.IsScrolling = true;
            else if (!this.DownArrow.containsPoint(x, y) && x > this.xPositionOnScreen + this.width && (x < this.xPositionOnScreen + this.width + Game1.tileSize * 2 && y > this.yPositionOnScreen) && y < this.yPositionOnScreen + this.height)
            {
                this.IsScrolling = true;
                this.leftClickHeld(x, y);
                this.releaseLeftClick(x, y);
            }
            this.CurrentItemIndex = Math.Max(0, Math.Min(this.Options.Count - CheatsMenu.ItemsPerPage, this.CurrentItemIndex));
            for (int index = 0; index < this.OptionSlots.Count; ++index)
            {
                if (this.OptionSlots[index].bounds.Contains(x, y) && this.CurrentItemIndex + index < this.Options.Count && this.Options[this.CurrentItemIndex + index].bounds.Contains(x - this.OptionSlots[index].bounds.X, y - this.OptionSlots[index].bounds.Y - 5))
                {
                    this.Options[this.CurrentItemIndex + index].receiveLeftClick(x - this.OptionSlots[index].bounds.X, y - this.OptionSlots[index].bounds.Y + 5);
                    this.OptionsSlotHeld = index;
                    break;
                }
            }

            foreach (var tab in this.Tabs)
            {
                if (tab.bounds.Contains(x, y))
                {
                    MenuTab tabID = this.GetTabID(tab);
                    Game1.activeClickableMenu = new CheatsMenu(tabID, this.Cheats, this.Monitor);
                    break;
                }
            }
        }

        /// <summary>Handle the player hovering the cursor over the menu.</summary>
        /// <param name="x">The cursor's X pixel position.</param>
        /// <param name="y">The cursor's Y pixel position.</param>
        public override void performHoverAction(int x, int y)
        {
            if (GameMenu.forcePreventClose)
                return;
            this.HoverText = "";
            this.UpArrow.tryHover(x, y);
            this.DownArrow.tryHover(x, y);
            this.Scrollbar.tryHover(x, y);
        }

        /// <summary>Draw the menu to the screen.</summary>
        /// <param name="spriteBatch">The sprite batch being drawn.</param>
        public override void draw(SpriteBatch spriteBatch)
        {
            if (!Game1.options.showMenuBackground)
                spriteBatch.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.4f);
            base.draw(spriteBatch);

            Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true);
            CommonHelper.DrawTextBox(this.Title.bounds.X, this.Title.bounds.Y, Game1.dialogueFont, this.Title.name, 1);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null);
            for (int index = 0; index < this.OptionSlots.Count; ++index)
            {
                if (this.CurrentItemIndex >= 0 && this.CurrentItemIndex + index < this.Options.Count)
                    this.Options[this.CurrentItemIndex + index].draw(spriteBatch, this.OptionSlots[index].bounds.X, this.OptionSlots[index].bounds.Y + 5);
            }
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
            if (!GameMenu.forcePreventClose)
            {

                foreach (ClickableComponent tab in this.Tabs)
                {
                    MenuTab tabID = this.GetTabID(tab);
                    CommonHelper.DrawTextBox(tab.bounds.X + tab.bounds.Width, tab.bounds.Y, Game1.smallFont, tab.label, 2, this.CurrentTab == tabID ? 1F : 0.7F);
                }

                this.UpArrow.draw(spriteBatch);
                this.DownArrow.draw(spriteBatch);
                if (this.Options.Count > CheatsMenu.ItemsPerPage)
                {
                    IClickableMenu.drawTextureBox(spriteBatch, Game1.mouseCursors, new Rectangle(403, 383, 6, 6), this.ScrollbarRunner.X, this.ScrollbarRunner.Y, this.ScrollbarRunner.Width, this.ScrollbarRunner.Height, Color.White, Game1.pixelZoom, false);
                    this.Scrollbar.draw(spriteBatch);
                }
            }
            if (this.HoverText != "")
                IClickableMenu.drawHoverText(spriteBatch, this.HoverText, Game1.smallFont);

            if (!Game1.options.hardwareCursor && !this.IsAndroid)
                spriteBatch.Draw(Game1.mouseCursors, new Vector2(Game1.getOldMouseX(), Game1.getOldMouseY()), Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, Game1.options.gamepadControls ? 44 : 0, 16, 16), Color.White, 0f, Vector2.Zero, Game1.pixelZoom + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 1f);

            // reinitialize the UI to fix Android pinch-zoom scaling issues
            if (this.JustOpened)
            {
                this.JustOpened = false;
                if (this.IsAndroid)
                    this.ResetComponents();
            }
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Initialize or reinitialize the UI components.</summary>
        private void ResetComponents()
        {
            var text = this.Cheats.Context.Text;

            // set dimensions
            this.width = (this.IsAndroid ? 750 : 800) + IClickableMenu.borderWidth * 2;
            this.height = (this.IsAndroid ? 550 : 600) + IClickableMenu.borderWidth * 2;
            this.xPositionOnScreen = Game1.viewport.Width / 2 - (this.width - (int)(Game1.tileSize * 2.4f)) / 2;
            this.yPositionOnScreen = Game1.viewport.Height / 2 - this.height / 2;

            // show close button on Android
            if (this.IsAndroid)
                this.initializeUpperRightCloseButton();

            // add title
            this.Title = new ClickableComponent(new Rectangle(this.xPositionOnScreen + this.width / 2, this.yPositionOnScreen, Game1.tileSize * 4, Game1.tileSize), text.Get("mod-name"));

            // add tabs
            {
                int i = 0;
                int labelX = (int)(this.xPositionOnScreen - Game1.tileSize * 4.8f);
                int labelY = (int)(this.yPositionOnScreen + Game1.tileSize * (this.IsAndroid ? 1.25f : 1.5f));
                int labelHeight = (int)(Game1.tileSize * 0.9F);

                this.Tabs.Clear();
                this.Tabs.AddRange(new[]
                {
                    new ClickableComponent(new Rectangle(labelX, labelY + labelHeight * i++, Game1.tileSize * 5, Game1.tileSize), MenuTab.PlayerAndTools.ToString(), text.Get("tabs.player-and-tools")),
                    new ClickableComponent(new Rectangle(labelX, labelY + labelHeight * i++, Game1.tileSize * 5, Game1.tileSize), MenuTab.FarmAndFishing.ToString(), text.Get("tabs.farm-and-fishing")),
                    new ClickableComponent(new Rectangle(labelX, labelY + labelHeight * i++, Game1.tileSize * 5, Game1.tileSize), MenuTab.Skills.ToString(), text.Get("tabs.skills")),
                    new ClickableComponent(new Rectangle(labelX, labelY + labelHeight * i++, Game1.tileSize * 5, Game1.tileSize), MenuTab.Weather.ToString(), text.Get("tabs.weather")),
                    new ClickableComponent(new Rectangle(labelX, labelY + labelHeight * i++, Game1.tileSize * 5, Game1.tileSize), MenuTab.Relationships.ToString(), text.Get("tabs.relationships")),
                    new ClickableComponent(new Rectangle(labelX, labelY + labelHeight * i++, Game1.tileSize * 5, Game1.tileSize), MenuTab.WarpLocations.ToString(), text.Get("tabs.warp")),
                    new ClickableComponent(new Rectangle(labelX, labelY + labelHeight * i++, Game1.tileSize * 5, Game1.tileSize), MenuTab.Time.ToString(), text.Get("tabs.time")),
                    new ClickableComponent(new Rectangle(labelX, labelY + labelHeight * i++, Game1.tileSize * 5, Game1.tileSize), MenuTab.Advanced.ToString(), text.Get("tabs.advanced")),
                    new ClickableComponent(new Rectangle(labelX, labelY + labelHeight * i++, Game1.tileSize * 5, Game1.tileSize), MenuTab.Controls.ToString(), text.Get("tabs.controls"))
                });
            }

            // add scroll UI
            int scrollbarOffset = Game1.tileSize * (this.IsAndroid ? 1 : 4) / 16;
            this.UpArrow = new ClickableTextureComponent("up-arrow", new Rectangle(this.xPositionOnScreen + this.width + scrollbarOffset, this.yPositionOnScreen + Game1.tileSize, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), "", "", Game1.mouseCursors, new Rectangle(421, 459, 11, 12), Game1.pixelZoom);
            this.DownArrow = new ClickableTextureComponent("down-arrow", new Rectangle(this.xPositionOnScreen + this.width + scrollbarOffset, this.yPositionOnScreen + this.height - Game1.tileSize, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), "", "", Game1.mouseCursors, new Rectangle(421, 472, 11, 12), Game1.pixelZoom);
            this.Scrollbar = new ClickableTextureComponent("scrollbar", new Rectangle(this.UpArrow.bounds.X + Game1.pixelZoom * 3, this.UpArrow.bounds.Y + this.UpArrow.bounds.Height + Game1.pixelZoom, 6 * Game1.pixelZoom, 10 * Game1.pixelZoom), "", "", Game1.mouseCursors, new Rectangle(435, 463, 6, 10), Game1.pixelZoom);
            this.ScrollbarRunner = new Rectangle(this.Scrollbar.bounds.X, this.UpArrow.bounds.Y + this.UpArrow.bounds.Height + Game1.pixelZoom, this.Scrollbar.bounds.Width, this.height - Game1.tileSize * 2 - this.UpArrow.bounds.Height - Game1.pixelZoom * 2);
            this.SetScrollBarToCurrentIndex();

            // add option slots
            this.OptionSlots.Clear();
            for (int i = 0; i < CheatsMenu.ItemsPerPage; i++)
                this.OptionSlots.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize * 5 / 4 + Game1.pixelZoom + i * ((this.height - Game1.tileSize * 2) / CheatsMenu.ItemsPerPage), this.width - Game1.tileSize / 2, (this.height - Game1.tileSize * 2) / CheatsMenu.ItemsPerPage + Game1.pixelZoom), string.Concat(i)));
        }

        /// <summary>Set the options to display.</summary>
        private void SetOptions()
        {
            CheatManager cheats = this.Cheats;
            CheatContext context = cheats.Context;
            ITranslationHelper text = context.Text;
            ModConfig config = context.Config;

            cheats.Context.SlotWidth = this.OptionSlots[0].bounds.Width;

            this.Options.Clear();
            switch (this.CurrentTab)
            {
                case MenuTab.PlayerAndTools:
                    // player
                    this.AddOptions(
                        $"{text.Get("player.title")}:",
                        cheats.InfiniteHealth,
                        cheats.InfiniteStamina,
                        cheats.InstantCooldowns,
                        cheats.OneHitKill,
                        cheats.MaxDailyLuck,
                        cheats.MoveSpeed,
                        cheats.InventorySize
                    );

                    // tools
                    this.AddOptions(
                        $"{text.Get("tools.title")}:",
                        cheats.InfiniteWater,
                        cheats.OneHitBreak,
                        cheats.HarvestWithScythe
                    );

                    // money
                    this.AddOptions(
                        $"{text.Get("money.title")}:",
                        cheats.AddMoney
                    );

                    // casino coins
                    this.AddOptions(
                        $"{text.Get("casino-coins.title")}:",
                        cheats.AddCasinoCoins
                    );
                    break;

                case MenuTab.FarmAndFishing:
                    // farming
                    this.AddOptions(
                        $"{text.Get("farm.title")}:",
                        cheats.AutoWater,
                        cheats.DurableFences,
                        cheats.InstantBuild,
                        cheats.AlwaysAutoFeed,
                        cheats.InfiniteHay
                    );

                    // fishing
                    this.AddOptions(
                        $"{text.Get("fishing.title")}:",
                        cheats.InstantFishCatch,
                        cheats.InstantFishBite,
                        cheats.AlwaysCastMaxDistance,
                        cheats.AlwaysFishTreasure,
                        cheats.DurableFishTackles
                    );

                    // fast machines
                    this.AddOptions(
                        $"{text.Get("fast-machines.title")}:",
                        cheats.FastMachines
                    );
                    break;

                case MenuTab.Skills:
                    // skills
                    this.AddOptions(
                        $"{text.Get("skills.title")}:",
                        cheats.Skills
                    );

                    // professions
                    this.AddOptions(
                        $"{text.Get("professions.title")}:",
                        cheats.Professions
                    );
                    break;

                case MenuTab.Weather:
                    this.AddOptions(
                        $"{text.Get("weather.title")}:",
                        cheats.SetWeatherForTomorrow
                    );
                    break;

                case MenuTab.Relationships:
                    // relationship options
                    this.AddOptions(
                        $"{text.Get("relationships.title")}:",
                        cheats.AlwaysGiveGifts,
                        cheats.NoFriendshipDecay
                    );

                    // heart levels
                    this.AddOptions(
                        $"{text.Get("relationships.friends")}:",
                        cheats.Hearts
                    );
                    break;

                case MenuTab.WarpLocations:
                    this.AddOptions(
                        cheats.Warps // grouped into sections, no need for a tab title
                    );
                    break;

                case MenuTab.Time:
                    this.AddOptions(
                        $"{text.Get("time.title")}:",
                        cheats.FreezeTime,
                        cheats.SetTime
                    );
                    break;

                case MenuTab.Advanced:
                    {
                        this.AddDescription(text.Get("flags.warning"));

                        // quests
                        this.AddOptions(
                            $"{text.Get("flags.quests")}:",
                            cheats.Quests
                        );

                        // wallet items
                        this.AddOptions(
                            $"{text.Get("flags.wallet")}:",
                            cheats.WalletItems
                        );

                        // locked doors
                        this.AddOptions(
                            $"{text.Get("flags.unlocked")}:",
                            cheats.UnlockDoor
                        );

                        // locked content
                        this.AddOptions(
                            $"{text.Get("flags.unlocked-content")}:",
                            cheats.UnlockContent
                        );

                        // community center
                        this.AddOptions(
                            $"{text.Get("flags.community-center")}:",
                            cheats.Bundles
                        );
                    }
                    break;

                case MenuTab.Controls:
                    this.AddTitle($"{text.Get("controls.title")}:");

                    if (this.IsAndroid)
                        this.AddDescription(text.Get("controls.android-config-note"));

                    this.AddOptions(
                        new CheatsOptionsKeyListener(
                            label: text.Get("controls.open-menu"),
                            value: config.OpenMenuKey,
                            setValue: key => config.OpenMenuKey = key,
                            slotWidth: context.SlotWidth,
                            i18n: text,
                            clearToButton: ModConfig.Defaults.OpenMenuKey
                        ),
                        new CheatsOptionsKeyListener(
                            label: text.Get("controls.freeze-time"),
                            value: config.FreezeTimeKey,
                            setValue: key => config.FreezeTimeKey = key,
                            slotWidth: context.SlotWidth,
                            i18n: text
                        ),
                        new CheatsOptionsKeyListener(
                            label: text.Get("controls.grow-tree"),
                            value: config.GrowTreeKey,
                            setValue: key => config.GrowTreeKey = key,
                            slotWidth: context.SlotWidth,
                            i18n: text
                        ),
                        new CheatsOptionsKeyListener(
                            label: text.Get("controls.grow-crops"),
                            value: config.GrowCropsKey,
                            setValue: key => config.GrowCropsKey = key,
                            slotWidth: context.SlotWidth,
                            i18n: text
                        ),
                        new CheatsOptionsSlider(
                            label: text.Get("controls.grow-radius"),
                            value: config.GrowRadius,
                            minValue: 1,
                            maxValue: 10,
                            setValue: value => config.GrowRadius = value,
                            disabled: () => config.GrowTreeKey == SButton.None && config.GrowCropsKey == SButton.None
                        ),
                        new OptionsElement(string.Empty), // blank line
                        new CheatsOptionsButton(
                            label: text.Get("controls.reset-controls"),
                            toggle: this.ResetControls,
                            slotWidth: context.SlotWidth
                        )
                    );
                    break;
            }
            this.SetScrollBarToCurrentIndex();
        }

        /// <summary>Add a section title to the options list.</summary>
        /// <param name="title">The section title.</param>
        private void AddTitle(string title)
        {
            this.Options.Add(new OptionsElement(title));
        }

        /// <summary>Add descriptive text that may extend onto multiple lines if it's too long.</summary>
        /// <param name="text">The text to render.</param>
        private void AddDescription(string text)
        {
            // get text lines
            int maxWidth = this.width - Game1.tileSize - 10;

            foreach (string originalLine in text.Replace("\r\n", "\n").Split('\n'))
            {
                string line = "";
                foreach (string word in originalLine.Split(' '))
                {
                    if (line == "")
                        line = word;
                    else if (Game1.smallFont.MeasureString(line + " " + word).X <= maxWidth)
                        line += " " + word;
                    else
                    {
                        this.Options.Add(new DescriptionElement(line));
                        line = word;
                    }
                }
                if (line != "")
                    this.Options.Add(new DescriptionElement(line));
            }
        }

        /// <summary>Add fields to the options list.</summary>
        /// <param name="fields">The fields to add.</param>
        private void AddOptions(params OptionsElement[] fields)
        {
            this.Options.AddRange(fields);
        }

        /// <summary>Add cheats to the options list.</summary>
        /// <param name="cheats">The cheats to add.</param>
        private void AddOptions(params ICheat[] cheats)
        {
            foreach (var field in cheats.SelectMany(p => p.GetFields(this.Cheats.Context)))
                this.Options.Add(field);
        }

        /// <summary>Add cheats to the options list.</summary>
        /// <param name="title">The section title.</param>
        /// <param name="cheats">The cheats to add.</param>
        private void AddOptions(string title, params ICheat[] cheats)
        {
            this.AddTitle(title);
            this.AddOptions(cheats);
        }

        /// <summary>Reset all controls to their default value.</summary>
        private void ResetControls()
        {
            var config = this.Cheats.Context.Config;

            config.FreezeTimeKey = ModConfig.Defaults.FreezeTimeKey;
            config.GrowCropsKey = ModConfig.Defaults.GrowCropsKey;
            config.GrowTreeKey = ModConfig.Defaults.GrowTreeKey;
            config.OpenMenuKey = ModConfig.Defaults.OpenMenuKey;
            config.GrowRadius = ModConfig.Defaults.GrowRadius;

            Game1.soundBank.PlayCue("bigDeSelect");

            this.SetOptions();
        }

        private void SetScrollBarToCurrentIndex()
        {
            if (!this.Options.Any())
                return;
            this.Scrollbar.bounds.Y = this.ScrollbarRunner.Height / Math.Max(1, this.Options.Count - CheatsMenu.ItemsPerPage + 1) * this.CurrentItemIndex + this.UpArrow.bounds.Bottom + Game1.pixelZoom;
            if (this.CurrentItemIndex != this.Options.Count - CheatsMenu.ItemsPerPage)
                return;
            this.Scrollbar.bounds.Y = this.DownArrow.bounds.Y - this.Scrollbar.bounds.Height - Game1.pixelZoom;
        }

        private void DownArrowPressed()
        {
            this.DownArrow.scale = this.DownArrow.baseScale;
            ++this.CurrentItemIndex;
            this.SetScrollBarToCurrentIndex();
        }

        private void UpArrowPressed()
        {
            this.UpArrow.scale = this.UpArrow.baseScale;
            --this.CurrentItemIndex;
            this.SetScrollBarToCurrentIndex();
        }

        /// <summary>Get the tab constant represented by a tab component.</summary>
        /// <param name="tab">The component to check.</param>
        private MenuTab GetTabID(ClickableComponent tab)
        {
            if (!Enum.TryParse(tab.name, out MenuTab tabID))
                throw new InvalidOperationException($"Couldn't parse tab name '{tab.name}'.");
            return tabID;
        }
    }
}
