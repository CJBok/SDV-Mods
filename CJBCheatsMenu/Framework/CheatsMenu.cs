using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CJB.Common;
using CJBCheatsMenu.Framework.Cheats;
using CJBCheatsMenu.Framework.Components;
using CJBCheatsMenu.Framework.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework;

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

    /// <summary>Reopen the cheats menu with the selected tab.</summary>
    private readonly Action<MenuTab> ReopenMenu;

    private readonly List<ClickableComponent> OptionSlots = [];
    private readonly List<OptionsElement> Options = [];
    private ClickableTextureComponent UpArrow;
    private ClickableTextureComponent DownArrow;
    private ClickableTextureComponent Scrollbar;
    private readonly List<ClickableComponent> Tabs = [];
    private ClickableComponent Title;
    private const int ItemsPerPage = 10;

    /// <summary>Whether the mod is running on Android.</summary>
    private readonly bool IsAndroid = Constants.TargetPlatform == GamePlatform.Android;

    private string HoverText = "";
    private int OptionsSlotHeld = -1;
    private int CurrentItemIndex;
    private bool IsScrolling;
    private Rectangle ScrollbarRunner;

    /// <summary>Whether the menu was opened in the current tick.</summary>
    private bool JustOpened = true;


    /*********
    ** Accessors
    *********/
    /// <summary>The currently open tab.</summary>
    public MenuTab CurrentTab { get; }


    /*********
    ** Public methods
    *********/
    /// <summary>Construct an instance.</summary>
    /// <param name="initialTab">The tab to display by default.</param>
    /// <param name="cheats">The cheats helper.</param>
    /// <param name="monitor">Encapsulates monitoring and logging.</param>
    /// <param name="isNewMenu">Whether to play the open-menu sound.</param>
    /// <param name="reopenMenu">Reopen the cheats menu with the selected tab.</param>
    public CheatsMenu(MenuTab initialTab, CheatManager cheats, IMonitor monitor, bool isNewMenu, Action<MenuTab> reopenMenu)
    {
        this.Cheats = cheats;
        this.Monitor = monitor;
        this.ReopenMenu = reopenMenu;
        this.CurrentTab = initialTab;
        this.ResetComponents();
        this.SetOptions();

        Game1.playSound(isNewMenu
            ? "bigSelect"   // menu open
            : "smallSelect" // tab select
        );

        this.ResetComponents();
    }

    /// <summary>Exit the menu if that's allowed for the current state.</summary>
    public void ExitIfValid()
    {
        if (this.readyToClose() && !GameMenu.forcePreventClose)
        {
            Game1.exitActiveMenu();
            Game1.playSound("bigDeSelect");
        }
    }

    /// <inheritdoc />
    public override bool overrideSnappyMenuCursorMovementBan()
    {
        return true;
    }

    /// <inheritdoc />
    public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
    {
        this.ResetComponents();
    }

    /// <inheritdoc />
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
            Game1.playSound("shiny4");
        }
        else
        {
            if (this.OptionsSlotHeld == -1 || this.OptionsSlotHeld + this.CurrentItemIndex >= this.Options.Count)
                return;
            this.Options[this.CurrentItemIndex + this.OptionsSlotHeld].leftClickHeld(x - this.OptionSlots[this.OptionsSlotHeld].bounds.X, y - this.OptionSlots[this.OptionsSlotHeld].bounds.Y);
        }
    }

    /// <inheritdoc />
    public override void receiveKeyPress(Keys key)
    {
        // exit menu
        if (Game1.options.menuButton.Contains(new InputButton(key)) && !this.IsPressNewKeyActive())
            this.ExitIfValid();

        // send key to active option
        else
            this.GetActiveOption()?.receiveKeyPress(key);
    }

    /// <inheritdoc />
    public override void receiveGamePadButton(Buttons button)
    {
        // navigate tabs
        if (button is (Buttons.LeftShoulder or Buttons.RightShoulder) && !this.IsPressNewKeyActive())
        {
            // rotate tab index
            int index = this.Tabs.FindIndex(p => p.name == this.CurrentTab.ToString());
            if (button == Buttons.LeftShoulder)
                index--;
            if (button == Buttons.RightShoulder)
                index++;

            if (index >= this.Tabs.Count)
                index = 0;
            if (index < 0)
                index = this.Tabs.Count - 1;

            // open menu with new index
            MenuTab tabId = this.GetTabID(this.Tabs[index]);
            this.ReopenMenu(tabId);
        }

        // send to active menu
        (this.GetActiveOption() as BaseOptionsElement)?.ReceiveButtonPress(button.ToSButton());
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
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

    /// <inheritdoc />
    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
        if (GameMenu.forcePreventClose)
            return;
        base.receiveLeftClick(x, y, playSound);

        if (this.DownArrow.containsPoint(x, y) && this.CurrentItemIndex < Math.Max(0, this.Options.Count - CheatsMenu.ItemsPerPage))
        {
            this.DownArrowPressed();
            Game1.playSound("shwip");
        }
        else if (this.UpArrow.containsPoint(x, y) && this.CurrentItemIndex > 0)
        {
            this.UpArrowPressed();
            Game1.playSound("shwip");
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

        foreach (ClickableComponent tab in this.Tabs)
        {
            if (tab.bounds.Contains(x, y))
            {
                MenuTab tabId = this.GetTabID(tab);
                this.ReopenMenu(tabId);
                break;
            }
        }
    }

    /// <inheritdoc />
    public override void performHoverAction(int x, int y)
    {
        if (GameMenu.forcePreventClose)
            return;
        this.HoverText = "";
        this.UpArrow.tryHover(x, y);
        this.DownArrow.tryHover(x, y);
        this.Scrollbar.tryHover(x, y);
    }

    /// <inheritdoc />
    public override bool showWithoutTransparencyIfOptionIsSet()
    {
        return true;
    }

    /// <inheritdoc />
    public override void draw(SpriteBatch b)
    {
        if (!Game1.options.showMenuBackground && !Game1.options.showClearBackgrounds)
            b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * .75f);

        base.draw(b);

        Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true);
        CommonHelper.DrawTab(this.Title.bounds.X, this.Title.bounds.Y, Game1.dialogueFont, this.Title.name, 1);
        b.End();
        b.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp);
        for (int index = 0; index < this.OptionSlots.Count; ++index)
        {
            if (this.CurrentItemIndex >= 0 && this.CurrentItemIndex + index < this.Options.Count)
                this.Options[this.CurrentItemIndex + index].draw(b, this.OptionSlots[index].bounds.X, this.OptionSlots[index].bounds.Y + 5);
        }
        b.End();
        b.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
        if (!GameMenu.forcePreventClose)
        {
            foreach (ClickableComponent tab in this.Tabs)
            {
                MenuTab tabID = this.GetTabID(tab);
                CommonHelper.DrawTab(tab.bounds.X + tab.bounds.Width, tab.bounds.Y, Game1.smallFont, tab.label, 2, this.CurrentTab == tabID ? 1F : 0.7F);
            }

            this.UpArrow.draw(b);
            this.DownArrow.draw(b);
            if (this.Options.Count > CheatsMenu.ItemsPerPage)
            {
                IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(403, 383, 6, 6), this.ScrollbarRunner.X, this.ScrollbarRunner.Y, this.ScrollbarRunner.Width, this.ScrollbarRunner.Height, Color.White, Game1.pixelZoom, false);
                this.Scrollbar.draw(b);
            }
        }
        if (this.HoverText != "")
            IClickableMenu.drawHoverText(b, this.HoverText, Game1.smallFont);

        if (!Game1.options.hardwareCursor && !this.IsAndroid)
            b.Draw(Game1.mouseCursors, new Vector2(Game1.getOldMouseX(), Game1.getOldMouseY()), Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, Game1.options.gamepadControls ? 44 : 0, 16, 16), Color.White, 0f, Vector2.Zero, Game1.pixelZoom + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 1f);

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
    [MemberNotNull(nameof(CheatsMenu.DownArrow), nameof(CheatsMenu.Scrollbar), nameof(CheatsMenu.ScrollbarRunner), nameof(CheatsMenu.Title), nameof(CheatsMenu.UpArrow))]
    private void ResetComponents()
    {
        // set dimensions
        this.width = (this.IsAndroid ? 750 : 800) + IClickableMenu.borderWidth * 2;
        this.height = (this.IsAndroid ? 550 : 600) + IClickableMenu.borderWidth * 2;
        this.xPositionOnScreen = Game1.uiViewport.Width / 2 - (this.width - (int)(Game1.tileSize * 2.4f)) / 2;
        this.yPositionOnScreen = Game1.uiViewport.Height / 2 - this.height / 2;

        // show close button on Android
        if (this.IsAndroid)
            this.initializeUpperRightCloseButton();

        // add title
        this.Title = new ClickableComponent(new Rectangle(this.xPositionOnScreen + this.width / 2, this.yPositionOnScreen, Game1.tileSize * 4, Game1.tileSize), I18n.ModName());

        // add tabs
        {
            int i = 0;
            int labelX = (int)(this.xPositionOnScreen - Game1.tileSize * 4.8f);
            int labelY = (int)(this.yPositionOnScreen + Game1.tileSize * (this.IsAndroid ? 1.25f : 1.5f));
            int labelHeight = (int)(Game1.tileSize * 0.9F);

            this.Tabs.Clear();
            this.Tabs.AddRange([
                new ClickableComponent(new Rectangle(labelX, labelY + labelHeight * i++, Game1.tileSize * 5, Game1.tileSize), MenuTab.PlayerAndTools.ToString(), I18n.Tabs_PlayerAndTools()),
                new ClickableComponent(new Rectangle(labelX, labelY + labelHeight * i++, Game1.tileSize * 5, Game1.tileSize), MenuTab.FarmAndFishing.ToString(), I18n.Tabs_FarmAndFishing()),
                new ClickableComponent(new Rectangle(labelX, labelY + labelHeight * i++, Game1.tileSize * 5, Game1.tileSize), MenuTab.Skills.ToString(), I18n.Tabs_Skills()),
                new ClickableComponent(new Rectangle(labelX, labelY + labelHeight * i++, Game1.tileSize * 5, Game1.tileSize), MenuTab.Weather.ToString(), I18n.Tabs_Weather()),
                new ClickableComponent(new Rectangle(labelX, labelY + labelHeight * i++, Game1.tileSize * 5, Game1.tileSize), MenuTab.Relationships.ToString(), I18n.Tabs_Relationships()),
                new ClickableComponent(new Rectangle(labelX, labelY + labelHeight * i++, Game1.tileSize * 5, Game1.tileSize), MenuTab.WarpLocations.ToString(), I18n.Tabs_Warp()),
                new ClickableComponent(new Rectangle(labelX, labelY + labelHeight * i++, Game1.tileSize * 5, Game1.tileSize), MenuTab.Time.ToString(), I18n.Tabs_Time()),
                new ClickableComponent(new Rectangle(labelX, labelY + labelHeight * i++, Game1.tileSize * 5, Game1.tileSize), MenuTab.Advanced.ToString(), I18n.Tabs_Advanced()),
                new ClickableComponent(new Rectangle(labelX, labelY + labelHeight * i++, Game1.tileSize * 5, Game1.tileSize), MenuTab.Controls.ToString(), I18n.Tabs_Controls())
            ]);
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
        ModConfig config = context.Config;

        cheats.Context.SlotWidth = this.OptionSlots[0].bounds.Width;

        this.Options.Clear();
        switch (this.CurrentTab)
        {
            case MenuTab.PlayerAndTools:
                // player
                this.AddOptions(
                    $"{I18n.Player_Title()}:",
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
                    $"{I18n.Tools_Title()}:",
                    cheats.InfiniteWater,
                    cheats.OneHitBreak,
                    cheats.HarvestWithScythe
                );

                // enchantments
                this.AddOptions(
                    $"{I18n.ToolEnchantments_Title()}:",
                    cheats.ToolEnchantments
                );

                // money
                this.AddOptions(
                    $"{I18n.Add_Money()}:",
                    cheats.AddMoney
                );

                // casino coins
                this.AddOptions(
                    $"{I18n.Add_CasinoCoins()}:",
                    cheats.AddCasinoCoins
                );

                // golden walnuts
                this.AddOptions(
                    $"{I18n.Add_GoldenWalnuts()}:",
                    cheats.AddGoldenWalnuts
                );

                // Qi gems
                this.AddOptions(
                    $"{I18n.Add_QiGems()}:",
                    cheats.AddQiGems
                );
                break;

            case MenuTab.FarmAndFishing:
                // farming
                this.AddOptions(
                    $"{I18n.Farm_Title()}:",
                    cheats.AutoWater,
                    cheats.DurableFences,
                    cheats.InstantBuild,
                    cheats.AutoFeedAnimals,
                    cheats.AutoPetAnimals,
                    cheats.AutoPetPets,
                    cheats.InfiniteHay
                );

                // fishing
                this.AddOptions(
                    $"{I18n.Fishing_Title()}:",
                    cheats.InstantFishCatch,
                    cheats.InstantFishBite,
                    cheats.AlwaysCastMaxDistance,
                    cheats.AlwaysFishTreasure,
                    cheats.DurableFishTackles
                );

                // fast machines
                this.AddOptions(
                    $"{I18n.FastMachines_Title()}:",
                    cheats.FastMachines
                );
                break;

            case MenuTab.Skills:
                // skills
                this.AddOptions(
                    $"{I18n.Skills_Title()}:",
                    cheats.Skills
                );

                // professions
                this.AddOptions(
                    $"{I18n.Professions_Title()}:",
                    cheats.Professions
                );
                break;

            case MenuTab.Weather:
                this.AddTitle($"{I18n.Weather_Title()}:");
                this.AddDescription(I18n.Weather_Explanation());
                this.AddOptions(cheats.SetWeatherForTomorrow);
                break;

            case MenuTab.Relationships:
                // relationship options
                this.AddOptions(
                    $"{I18n.Relationships_Title()}:",
                    cheats.AlwaysGiveGifts,
                    cheats.NoFriendshipDecay
                );

                // heart levels
                this.AddOptions(
                    $"{I18n.Relationships_Friends()}:",
                    cheats.Hearts
                );
                break;

            case MenuTab.WarpLocations:
                this.AddOptions(
                    cheats.Warps // grouped into sections, no need for a tab title
                );
                break;

            case MenuTab.Time:
                // time of day
                this.AddOptions(
                    $"{I18n.Time_Title()}:",
                    cheats.FreezeTime,
                    cheats.SetTime
                );

                // date
                this.AddTitle($"{I18n.Date_Title()}:");
                this.AddDescription(I18n.Date_Warning());
                this.AddOptions(
                    cheats.SetDay,
                    cheats.SetSeason,
                    cheats.SetYear
                );
                break;

            case MenuTab.Advanced:
                {
                    this.AddDescription(I18n.Flags_Warning());

                    // quests
                    this.AddOptions(
                        $"{I18n.Flags_Quests()}:",
                        cheats.Quests
                    );

                    // wallet items
                    this.AddOptions(
                        $"{I18n.Flags_Wallet()}:",
                        cheats.WalletItems
                    );

                    // locked doors
                    this.AddOptions(
                        $"{I18n.Flags_Unlocked()}:",
                        cheats.UnlockDoor
                    );

                    // locked content
                    this.AddOptions(
                        $"{I18n.Flags_UnlockedContent()}:",
                        cheats.UnlockContent
                    );

                    // community center
                    this.AddOptions(
                        $"{I18n.Flags_CommunityCenter()}:",
                        cheats.Bundles
                    );
                }
                break;

            case MenuTab.Controls:
                this.AddTitle($"{I18n.Controls_Title()}:");

                if (this.IsAndroid)
                    this.AddDescription(I18n.Controls_AndroidConfigNote());

                this.AddOptions(
                    new CheatsOptionsKeyListener(
                        label: I18n.Controls_OpenMenu(),
                        value: this.GetSingleButton(config.OpenMenuKey),
                        setValue: key => config.OpenMenuKey = new(key),
                        slotWidth: context.SlotWidth,
                        clearToButton: this.GetSingleButton(ModConfig.Defaults.OpenMenuKey)
                    ),
                    new CheatsOptionsKeyListener(
                        label: I18n.Controls_FreezeTime(),
                        value: this.GetSingleButton(config.FreezeTimeKey),
                        setValue: key => config.FreezeTimeKey = new(key),
                        slotWidth: context.SlotWidth
                    ),
                    new CheatsOptionsKeyListener(
                        label: I18n.Controls_GrowTree(),
                        value: this.GetSingleButton(config.GrowTreeKey),
                        setValue: key => config.GrowTreeKey = new(key),
                        slotWidth: context.SlotWidth
                    ),
                    new CheatsOptionsKeyListener(
                        label: I18n.Controls_GrowCrops(),
                        value: this.GetSingleButton(config.GrowCropsKey),
                        setValue: key => config.GrowCropsKey = new(key),
                        slotWidth: context.SlotWidth
                    ),
                    new CheatsOptionsSlider(
                        label: I18n.Controls_GrowRadius(),
                        value: config.GrowRadius,
                        minValue: 1,
                        maxValue: 10,
                        setValue: value => config.GrowRadius = value,
                        disabled: () => !config.GrowTreeKey.IsBound && !config.GrowCropsKey.IsBound
                    ),
                    new OptionsElement(string.Empty), // blank line
                    new CheatsOptionsButton(
                        label: I18n.Controls_ResetControls(),
                        toggle: this.ResetControls,
                        slotWidth: context.SlotWidth
                    )
                );
                break;
        }
        this.SetScrollBarToCurrentIndex();
    }

    /// <summary>Whether any button bind control is active and listening for input.</summary>
    private bool IsPressNewKeyActive()
    {
        return this.Options.Any(p => p is CheatsOptionsKeyListener { IsListening: true });
    }

    /// <summary>Get the currently active option, if any.</summary>
    private OptionsElement? GetActiveOption()
    {
        if (this.OptionsSlotHeld == -1)
            return null;

        int index = this.CurrentItemIndex + this.OptionsSlotHeld;
        return index < this.Options.Count
            ? this.Options[index]
            : null;
    }

    /// <summary>Get the first button in a key binding, if any.</summary>
    /// <param name="keybindList">The key binding list.</param>
    private SButton GetSingleButton(KeybindList keybindList)
    {
        foreach (Keybind keybind in keybindList.Keybinds)
        {
            if (keybind.IsBound)
                return keybind.Buttons.First();
        }

        return SButton.None;
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
                    this.Options.Add(new DescriptionElement(line, splitLinesIfNeeded: false));
                    line = word;
                }
            }
            if (line != "")
                this.Options.Add(new DescriptionElement(line, splitLinesIfNeeded: false));
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
        foreach (OptionsElement field in cheats.SelectMany(p => p.GetFields(this.Cheats.Context)))
        {
            if (field is DescriptionElement { SplitLinesIfNeeded: true } description)
                this.AddDescription(description.label);
            else
                this.Options.Add(field);
        }
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
        ModConfig config = this.Cheats.Context.Config;

        config.FreezeTimeKey = ModConfig.Defaults.FreezeTimeKey;
        config.GrowCropsKey = ModConfig.Defaults.GrowCropsKey;
        config.GrowTreeKey = ModConfig.Defaults.GrowTreeKey;
        config.OpenMenuKey = ModConfig.Defaults.OpenMenuKey;
        config.GrowRadius = ModConfig.Defaults.GrowRadius;

        Game1.playSound("bigDeSelect");

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
