using System;
using System.Collections.Generic;
using System.Linq;
using CJBCheatsMenu.Framework.Constants;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardewValley.Characters;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework
{
    internal class CheatsMenu : IClickableMenu
    {
        /*********
        ** Properties
        *********/
        /// <summary>The mod settings.</summary>
        private readonly ModConfig Config;

        /// <summary>The cheats helper.</summary>
        private readonly Cheats Cheats;

        private readonly List<ClickableComponent> OptionSlots = new List<ClickableComponent>();
        private readonly List<OptionsElement> Options = new List<OptionsElement>();
        private readonly ClickableTextureComponent UpArrow;
        private readonly ClickableTextureComponent DownArrow;
        private readonly ClickableTextureComponent Scrollbar;
        private readonly List<ClickableComponent> Tabs = new List<ClickableComponent>();
        private readonly ClickableComponent Title;
        private const int ItemsPerPage = 10;

        private string HoverText = "";
        private int OptionsSlotHeld = -1;
        private int CurrentItemIndex;
        private bool IsScrolling;
        private Rectangle ScrollbarRunner;
        private bool CanClose;
        private readonly MenuTab CurrentTab;


        /*********
        ** Public methods
        *********/
        public CheatsMenu(MenuTab tabIndex, ModConfig config, Cheats cheats)
          : base(Game1.viewport.Width / 2 - (600 + IClickableMenu.borderWidth * 2) / 2, Game1.viewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2, 800 + IClickableMenu.borderWidth * 2, 600 + IClickableMenu.borderWidth * 2)
        {
            this.Config = config;
            this.Cheats = cheats;

            this.Title = new ClickableComponent(new Rectangle(this.xPositionOnScreen + this.width / 2, this.yPositionOnScreen, Game1.tileSize * 4, Game1.tileSize), "CJB Cheats Menu");
            this.CurrentTab = tabIndex;

            {
                int i = 0;
                int labelX = (int)(this.xPositionOnScreen - Game1.tileSize * 4.8f);
                int labelY = (int)(this.yPositionOnScreen + Game1.tileSize * 1.5f);
                int labelHeight = (int)(Game1.tileSize * 0.9F);

                this.Tabs.Add(new ClickableComponent(new Rectangle(labelX, labelY + labelHeight * i++, Game1.tileSize * 5, Game1.tileSize), MenuTab.PlayerAndTools.ToString(), "Player & Tools"));
                this.Tabs.Add(new ClickableComponent(new Rectangle(labelX, labelY + labelHeight * i++, Game1.tileSize * 5, Game1.tileSize), MenuTab.FarmAndFishing.ToString(), "Farm & Fishing"));
                this.Tabs.Add(new ClickableComponent(new Rectangle(labelX, labelY + labelHeight * i++, Game1.tileSize * 5, Game1.tileSize), MenuTab.Skills.ToString(), "Skills"));
                this.Tabs.Add(new ClickableComponent(new Rectangle(labelX, labelY + labelHeight * i++, Game1.tileSize * 5, Game1.tileSize), MenuTab.Weather.ToString(), "Weather"));
                this.Tabs.Add(new ClickableComponent(new Rectangle(labelX, labelY + labelHeight * i++, Game1.tileSize * 5, Game1.tileSize), MenuTab.Relationships.ToString(), "Relationships"));
                this.Tabs.Add(new ClickableComponent(new Rectangle(labelX, labelY + labelHeight * i++, Game1.tileSize * 5, Game1.tileSize), MenuTab.WarpLocations.ToString(), "Warp Locations"));
                this.Tabs.Add(new ClickableComponent(new Rectangle(labelX, labelY + labelHeight * i++, Game1.tileSize * 5, Game1.tileSize), MenuTab.Time.ToString(), "Time"));
                this.Tabs.Add(new ClickableComponent(new Rectangle(labelX, labelY + labelHeight * i, Game1.tileSize * 5, Game1.tileSize), MenuTab.Controls.ToString(), "Controls"));
            }

            this.UpArrow = new ClickableTextureComponent("up-arrow", new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), "", "", Game1.mouseCursors, new Rectangle(421, 459, 11, 12), Game1.pixelZoom);
            this.DownArrow = new ClickableTextureComponent("down-arrow", new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize / 4, this.yPositionOnScreen + this.height - Game1.tileSize, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), "", "", Game1.mouseCursors, new Rectangle(421, 472, 11, 12), Game1.pixelZoom);
            this.Scrollbar = new ClickableTextureComponent("scrollbar", new Rectangle(this.UpArrow.bounds.X + Game1.pixelZoom * 3, this.UpArrow.bounds.Y + this.UpArrow.bounds.Height + Game1.pixelZoom, 6 * Game1.pixelZoom, 10 * Game1.pixelZoom), "", "", Game1.mouseCursors, new Rectangle(435, 463, 6, 10), Game1.pixelZoom);
            this.ScrollbarRunner = new Rectangle(this.Scrollbar.bounds.X, this.UpArrow.bounds.Y + this.UpArrow.bounds.Height + Game1.pixelZoom, this.Scrollbar.bounds.Width, this.height - Game1.tileSize * 2 - this.UpArrow.bounds.Height - Game1.pixelZoom * 2);
            for (int i = 0; i < CheatsMenu.ItemsPerPage; i++)
                this.OptionSlots.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize * 5 / 4 + Game1.pixelZoom + i * ((this.height - Game1.tileSize * 2) / CheatsMenu.ItemsPerPage), this.width - Game1.tileSize / 2, (this.height - Game1.tileSize * 2) / CheatsMenu.ItemsPerPage + Game1.pixelZoom), string.Concat(i)));

            switch (this.CurrentTab)
            {
                case MenuTab.PlayerAndTools:
                    this.Options.Add(new OptionsElement("Player:"));
                    this.Options.Add(new CheatsOptionsCheckbox("Infinite Stamina", config.InfiniteStamina, value => config.InfiniteStamina = value));
                    this.Options.Add(new CheatsOptionsCheckbox("Infinite Health", config.InfiniteHealth, value => config.InfiniteHealth = value));
                    this.Options.Add(new CheatsOptionsCheckbox("Increased Movement Speed", config.IncreasedMovement, value => config.IncreasedMovement = value));
                    this.Options.Add(new CheatsOptionsSlider("Move Speed", 0, 10, this.Config));
                    this.Options.Add(new CheatsOptionsCheckbox("One Hit Kill", config.OneHitKill, value => config.OneHitKill = value));
                    this.Options.Add(new CheatsOptionsCheckbox("Max Daily Luck", config.MaxDailyLuck, value => config.MaxDailyLuck = value));

                    this.Options.Add(new OptionsElement("Tools:"));
                    this.Options.Add(new CheatsOptionsCheckbox("Infinite Water in Can", config.InfiniteWateringCan, value => config.InfiniteWateringCan = value));
                    this.Options.Add(new CheatsOptionsCheckbox("One Hit Break", config.OneHitBreak, value => config.OneHitBreak = value));
                    this.Options.Add(new CheatsOptionsCheckbox("Harvest With Sickle (no XP gain)", config.HarvestSickle, value => config.HarvestSickle = value));

                    this.Options.Add(new OptionsElement("Money:"));
                    this.Options.Add(new CheatsOptionsInputListener("Add 100g", 2, this.OptionSlots[0].bounds.Width, config, cheats));
                    this.Options.Add(new CheatsOptionsInputListener("Add 1000g", 3, this.OptionSlots[0].bounds.Width, config, cheats));
                    this.Options.Add(new CheatsOptionsInputListener("Add 10000g", 4, this.OptionSlots[0].bounds.Width, config, cheats));
                    this.Options.Add(new CheatsOptionsInputListener("Add 100000g", 5, this.OptionSlots[0].bounds.Width, config, cheats));

                    this.Options.Add(new OptionsElement("Casino Coins:"));
                    this.Options.Add(new CheatsOptionsInputListener("Add 100", 6, this.OptionSlots[0].bounds.Width, config, cheats));
                    this.Options.Add(new CheatsOptionsInputListener("Add 1000", 7, this.OptionSlots[0].bounds.Width, config, cheats));
                    this.Options.Add(new CheatsOptionsInputListener("Add 10000", 8, this.OptionSlots[0].bounds.Width, config, cheats));
                    break;

                case MenuTab.FarmAndFishing:
                    this.Options.Add(new OptionsElement("Farm:"));
                    this.Options.Add(new CheatsOptionsInputListener("Water All Fields", 9, this.OptionSlots[0].bounds.Width, config, cheats));
                    this.Options.Add(new CheatsOptionsCheckbox("Durable Fences", config.DurableFences, value => config.DurableFences = value));
                    this.Options.Add(new CheatsOptionsCheckbox("Instant Build", config.InstantBuild, value => config.InstantBuild = value));
                    this.Options.Add(new CheatsOptionsCheckbox("Always Auto-Feed", config.AutoFeed, value => config.AutoFeed = value));
                    this.Options.Add(new CheatsOptionsCheckbox("Infinite Hay", config.InfiniteHay, value => config.InfiniteHay = value));

                    this.Options.Add(new OptionsElement("Fishing:"));
                    this.Options.Add(new CheatsOptionsCheckbox("Instant Catch", config.InstantCatch, value => config.InstantCatch = value));
                    this.Options.Add(new CheatsOptionsCheckbox("Instant Bite", config.InstantBite, value => config.InstantBite = value));
                    this.Options.Add(new CheatsOptionsCheckbox("Always Throw Max Distance", config.ThrowBobberMax, value => config.ThrowBobberMax = value));
                    this.Options.Add(new CheatsOptionsCheckbox("Always Treasure", config.AlwaysTreasure, value => config.AlwaysTreasure = value));
                    this.Options.Add(new CheatsOptionsCheckbox("Durable Tackles", config.DurableTackles, value => config.DurableTackles = value));

                    this.Options.Add(new OptionsElement("Fast Machine Processing:"));
                    this.Options.Add(new CheatsOptionsCheckbox("Cask", config.FastCask, value => config.FastCask = value));
                    this.Options.Add(new CheatsOptionsCheckbox("Furnace", config.FastFurnace, value => config.FastFurnace = value));
                    this.Options.Add(new CheatsOptionsCheckbox("Recycling Machine", config.FastRecyclingMachine, value => config.FastRecyclingMachine = value));
                    this.Options.Add(new CheatsOptionsCheckbox("Crystalarium", config.FastCrystalarium, value => config.FastCrystalarium = value));
                    this.Options.Add(new CheatsOptionsCheckbox("Incubator", config.FastIncubator, value => config.FastIncubator = value));
                    this.Options.Add(new CheatsOptionsCheckbox("Slime Incubator", config.FastSlimeIncubator, value => config.FastSlimeIncubator = value));
                    this.Options.Add(new CheatsOptionsCheckbox("Keg", config.FastKeg, value => config.FastKeg = value));
                    this.Options.Add(new CheatsOptionsCheckbox("Preserves Jar", config.FastPreservesJar, value => config.FastPreservesJar = value));
                    this.Options.Add(new CheatsOptionsCheckbox("Cheese Press", config.FastCheesePress, value => config.FastCheesePress = value));
                    this.Options.Add(new CheatsOptionsCheckbox("Mayonnaise Machine", config.FastMayonnaiseMachine, value => config.FastMayonnaiseMachine = value));
                    this.Options.Add(new CheatsOptionsCheckbox("Loom", config.FastLoom, value => config.FastLoom = value));
                    this.Options.Add(new CheatsOptionsCheckbox("Oil Maker", config.FastOilMaker, value => config.FastOilMaker = value));
                    this.Options.Add(new CheatsOptionsCheckbox("Seed Maker", config.FastSeedMaker, value => config.FastSeedMaker = value));
                    this.Options.Add(new CheatsOptionsCheckbox("Charcoal Kiln", config.FastCharcoalKiln, value => config.FastCharcoalKiln = value));
                    this.Options.Add(new CheatsOptionsCheckbox("Slime Egg-Press", config.FastSlimeEggPress, value => config.FastSlimeEggPress = value));
                    this.Options.Add(new CheatsOptionsCheckbox("Tapper", config.FastTapper, value => config.FastTapper = value));
                    this.Options.Add(new CheatsOptionsCheckbox("Lightning Rod", config.FastLightningRod, value => config.FastLightningRod = value));
                    this.Options.Add(new CheatsOptionsCheckbox("Bee House", config.FastBeeHouse, value => config.FastBeeHouse = value));
                    this.Options.Add(new CheatsOptionsCheckbox("Mushroom Box", config.FastMushroomBox, value => config.FastMushroomBox = value));
                    this.Options.Add(new CheatsOptionsCheckbox("Worm Bin", config.FastWormBin, value => config.FastWormBin = value));
                    break;

                case MenuTab.Skills:
                    this.Options.Add(new OptionsElement("Skills:"));
                    this.Options.Add(new CheatsOptionsInputListener("Incr. Farming Lvl", 200, this.OptionSlots[0].bounds.Width, config, cheats));
                    this.Options.Add(new CheatsOptionsInputListener("Incr. Mining Lvl", 201, this.OptionSlots[0].bounds.Width, config, cheats));
                    this.Options.Add(new CheatsOptionsInputListener("Incr. Foraging Lvl", 202, this.OptionSlots[0].bounds.Width, config, cheats));
                    this.Options.Add(new CheatsOptionsInputListener("Incr. Fishing Lvl", 203, this.OptionSlots[0].bounds.Width, config, cheats));
                    this.Options.Add(new CheatsOptionsInputListener("Incr. Combat Lvl", 204, this.OptionSlots[0].bounds.Width, config, cheats));
                    this.Options.Add(new CheatsOptionsInputListener("RESET SKILLS!", 205, this.OptionSlots[0].bounds.Width, config, cheats));
                    break;

                case MenuTab.Weather:
                    this.Options.Add(new OptionsElement("Weather Next Day:"));
                    this.Options.Add(new CheatsOptionsElement("Current: ", 1));
                    this.Options.Add(new CheatsOptionsInputListener("Sunny", 10, this.OptionSlots[0].bounds.Width, config, cheats));
                    this.Options.Add(new CheatsOptionsInputListener("Raining", 11, this.OptionSlots[0].bounds.Width, config, cheats));
                    this.Options.Add(new CheatsOptionsInputListener("Lightning", 12, this.OptionSlots[0].bounds.Width, config, cheats));
                    this.Options.Add(new CheatsOptionsInputListener("Snowing", 13, this.OptionSlots[0].bounds.Width, config, cheats));
                    break;

                case MenuTab.Relationships:
                    this.Options.Add(new OptionsElement("Relationships:"));
                    this.Options.Add(new CheatsOptionsCheckbox("Give Gift Anytime", config.AlwaysGiveGift, value => config.AlwaysGiveGift = value));
                    this.Options.Add(new CheatsOptionsCheckbox("No Friendship Decay", config.NoFriendshipDecay, value => config.NoFriendshipDecay = value));
                    this.Options.Add(new OptionsElement("Friends:"));

                    foreach (NPC npc in Utility.getAllCharacters().OrderBy(p => p.name))
                    {
                        if ((npc.name != "Sandy" || Game1.player.mailReceived.Contains("ccVault")) && npc.name != "???" && npc.name != "Bouncer" && npc.name != "Marlon" && npc.name != "Gil" && npc.name != "Gunther" && !npc.IsMonster && !(npc is Horse) && !(npc is Pet))
                        {
                            if (Game1.player.friendships.ContainsKey(npc.name))
                                this.Options.Add(new CheatsOptionsNPCSlider(npc, 9999));
                        }
                    }
                    break;

                case MenuTab.WarpLocations:
                    this.Options.Add(new OptionsElement("Warp to:"));
                    this.Options.Add(new CheatsOptionsInputListener("Farm", 100, this.OptionSlots[0].bounds.Width, config, cheats));
                    this.Options.Add(new CheatsOptionsInputListener("Pierre's", 101, this.OptionSlots[0].bounds.Width, config, cheats));
                    this.Options.Add(new CheatsOptionsInputListener("Blacksmith", 102, this.OptionSlots[0].bounds.Width, config, cheats));
                    this.Options.Add(new CheatsOptionsInputListener("Archaeology Office", 103, this.OptionSlots[0].bounds.Width, config, cheats));
                    this.Options.Add(new CheatsOptionsInputListener("Saloon", 104, this.OptionSlots[0].bounds.Width, config, cheats));
                    this.Options.Add(new CheatsOptionsInputListener("Community Center", 105, this.OptionSlots[0].bounds.Width, config, cheats));
                    this.Options.Add(new CheatsOptionsInputListener("Carpenter", 106, this.OptionSlots[0].bounds.Width, config, cheats));
                    this.Options.Add(new CheatsOptionsInputListener("Guild", 107, this.OptionSlots[0].bounds.Width, config, cheats));
                    this.Options.Add(new CheatsOptionsInputListener("Ranch", 113, this.OptionSlots[0].bounds.Width, config, cheats));
                    this.Options.Add(new CheatsOptionsInputListener("Mines", 109, this.OptionSlots[0].bounds.Width, config, cheats));
                    this.Options.Add(new CheatsOptionsInputListener("Willy", 110, this.OptionSlots[0].bounds.Width, config, cheats));
                    this.Options.Add(new CheatsOptionsInputListener("Wizard", 114, this.OptionSlots[0].bounds.Width, config, cheats));
                    this.Options.Add(new CheatsOptionsInputListener("Hats", 115, this.OptionSlots[0].bounds.Width, config, cheats));
                    this.Options.Add(new CheatsOptionsInputListener("Desert", 112, this.OptionSlots[0].bounds.Width, config, cheats));
                    this.Options.Add(new CheatsOptionsInputListener("Sandy", 119, this.OptionSlots[0].bounds.Width, config, cheats));
                    this.Options.Add(new CheatsOptionsInputListener("Casino", 120, this.OptionSlots[0].bounds.Width, config, cheats));
                    this.Options.Add(new CheatsOptionsInputListener("Quarry", 108, this.OptionSlots[0].bounds.Width, config, cheats));
                    this.Options.Add(new CheatsOptionsInputListener("New Beach", 111, this.OptionSlots[0].bounds.Width, config, cheats));
                    this.Options.Add(new CheatsOptionsInputListener("Secret Forest", 116, this.OptionSlots[0].bounds.Width, config, cheats));
                    this.Options.Add(new CheatsOptionsInputListener("Sewer", 117, this.OptionSlots[0].bounds.Width, config, cheats));
                    this.Options.Add(new CheatsOptionsInputListener("Bathhouse", 118, this.OptionSlots[0].bounds.Width, config, cheats));
                    break;

                case MenuTab.Time:
                    this.Options.Add(new OptionsElement("Time:"));
                    this.Options.Add(new CheatsOptionsCheckbox("Freeze Time Inside", config.FreezeTimeInside, value => config.FreezeTimeInside = value));
                    this.Options.Add(new CheatsOptionsCheckbox("Freeze Time Caves", config.FreezeTimeCaves, value => config.FreezeTimeCaves = value));
                    this.Options.Add(new CheatsOptionsCheckbox("Freeze Time Everywhere", config.FreezeTime, value => config.FreezeTime = value));
                    this.Options.Add(new CheatsOptionsSlider("Time", 10, 20, this.Config, width: 100));
                    break;

                case MenuTab.Controls:
                    this.Options.Add(new OptionsElement("Controls:"));
                    this.Options.Add(new CheatsOptionsInputListener("Open Menu", 1000, this.OptionSlots[0].bounds.Width, config, cheats));
                    this.Options.Add(new CheatsOptionsInputListener("Freeze Time", 1001, this.OptionSlots[0].bounds.Width, config, cheats));
                    this.Options.Add(new CheatsOptionsInputListener("Grow Tree", 1002, this.OptionSlots[0].bounds.Width, config, cheats));
                    this.Options.Add(new CheatsOptionsInputListener("Grow Crops", 1003, this.OptionSlots[0].bounds.Width, config, cheats));
                    break;
            }
            this.SetScrollBarToCurrentIndex();
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

        public override void leftClickHeld(int x, int y)
        {
            if (GameMenu.forcePreventClose)
                return;
            base.leftClickHeld(x, y);
            if (this.IsScrolling)
            {
                int num = this.Scrollbar.bounds.Y;
                this.Scrollbar.bounds.Y = Math.Min(this.yPositionOnScreen + this.height - Game1.tileSize - Game1.pixelZoom * 3 - this.Scrollbar.bounds.Height, Math.Max(y, this.yPositionOnScreen + this.UpArrow.bounds.Height + Game1.pixelZoom * 5));
                this.CurrentItemIndex = Math.Min(this.Options.Count - CheatsMenu.ItemsPerPage, Math.Max(0, (int)(this.Options.Count * (double)((y - this.ScrollbarRunner.Y) / (float)this.ScrollbarRunner.Height))));
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

        public override void receiveKeyPress(Keys key)
        {
            if ((Game1.options.menuButton.Contains(new InputButton(key)) || key.ToString() == this.Config.OpenMenuKey) && this.readyToClose() && this.CanClose)
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
                Game1.activeClickableMenu = new CheatsMenu(tabID, this.Config, this.Cheats);
            }
        }

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

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            if (GameMenu.forcePreventClose)
                return;
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
                    Game1.activeClickableMenu = new CheatsMenu(tabID, this.Config, this.Cheats);
                    break;
                }
            }
        }

        public override void receiveRightClick(int x, int y, bool playSound = true) { }

        public override void performHoverAction(int x, int y)
        {
            if (GameMenu.forcePreventClose)
                return;
            this.HoverText = "";
            this.UpArrow.tryHover(x, y);
            this.DownArrow.tryHover(x, y);
            this.Scrollbar.tryHover(x, y);
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            if (!Game1.options.showMenuBackground)
                spriteBatch.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.4f);

            Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true);
            CJB.DrawTextBox(this.Title.bounds.X, this.Title.bounds.Y, Game1.dialogueFont, this.Title.name, 1);
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
                    CJB.DrawTextBox(tab.bounds.X + tab.bounds.Width, tab.bounds.Y, Game1.smallFont, tab.label, 2, this.CurrentTab == tabID ? 1F : 0.7F);
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

            if (!Game1.options.hardwareCursor)
                spriteBatch.Draw(Game1.mouseCursors, new Vector2(Game1.getOldMouseX(), Game1.getOldMouseY()), Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, Game1.options.gamepadControls ? 44 : 0, 16, 16), Color.White, 0f, Vector2.Zero, Game1.pixelZoom + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 1f);
        }


        /*********
        ** Private methods
        *********/
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
