using System;
using System.Collections.Generic;
using System.Linq;
using CJBCheatsMenu.Framework.Constants;
using CJBCheatsMenu.Framework.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Quests;

namespace CJBCheatsMenu.Framework
{
    internal class CheatsMenu : IClickableMenu
    {
        /*********
        ** Fields
        *********/
        /// <summary>The mod settings.</summary>
        private readonly ModConfig Config;

        /// <summary>The warps to show in the menu.</summary>
        private readonly ModDataWarp[] Warps;

        /// <summary>The cheats helper.</summary>
        private readonly Cheats Cheats;

        /// <summary>Provides translations for the mod.</summary>
        private readonly ITranslationHelper TranslationHelper;

        /// <summary>Encapsulates monitoring and logging.</summary>
        private readonly IMonitor Monitor;

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
        private readonly Rectangle ScrollbarRunner;
        private bool CanClose;
        private readonly MenuTab CurrentTab;

        /// <summary>Maps JojaMart completion flags to their Community Center equivalent.</summary>
        private readonly IDictionary<string, string> JojaMartCompletionFlags = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
        {
            ["jojaBoilerRoom"] = "ccBoilerRoom",
            ["jojaCraftsRoom"] = "ccCraftsRoom",
            ["jojaFishTank"] = "ccFishTank",
            ["jojaPantry"] = "ccPantry",
            ["jojaVault"] = "ccVault"
        };

        /// <summary>Maps Community Center completion flags to their area ID.</summary>
        private readonly Dictionary<string, int> CommunityCenterCompletionFlags = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase)
        {
            ["ccBoilerRoom"] = CommunityCenter.AREA_BoilerRoom,
            ["ccBulletin"] = CommunityCenter.AREA_Bulletin,
            ["ccCraftsRoom"] = CommunityCenter.AREA_CraftsRoom,
            ["ccFishTank"] = CommunityCenter.AREA_FishTank,
            ["ccPantry"] = CommunityCenter.AREA_Pantry,
            ["ccVault"] = CommunityCenter.AREA_Vault
        };


        /*********
        ** Public methods
        *********/
        public CheatsMenu(MenuTab tabIndex, ModConfig config, ModDataWarp[] warps, Cheats cheats, ITranslationHelper i18n, IMonitor monitor)
          : base(Game1.viewport.Width / 2 - (600 + IClickableMenu.borderWidth * 2) / 2, Game1.viewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2, 800 + IClickableMenu.borderWidth * 2, 600 + IClickableMenu.borderWidth * 2)
        {
            this.Config = config;
            this.Warps = warps;
            this.Cheats = cheats;
            this.TranslationHelper = i18n;
            this.Monitor = monitor;

            this.Title = new ClickableComponent(new Rectangle(this.xPositionOnScreen + this.width / 2, this.yPositionOnScreen, Game1.tileSize * 4, Game1.tileSize), i18n.Get("mod-name"));
            this.CurrentTab = tabIndex;

            {
                int i = 0;
                int labelX = (int)(this.xPositionOnScreen - Game1.tileSize * 4.8f);
                int labelY = (int)(this.yPositionOnScreen + Game1.tileSize * 1.5f);
                int labelHeight = (int)(Game1.tileSize * 0.9F);

                this.Tabs.Add(new ClickableComponent(new Rectangle(labelX, labelY + labelHeight * i++, Game1.tileSize * 5, Game1.tileSize), MenuTab.PlayerAndTools.ToString(), i18n.Get("tabs.player-and-tools")));
                this.Tabs.Add(new ClickableComponent(new Rectangle(labelX, labelY + labelHeight * i++, Game1.tileSize * 5, Game1.tileSize), MenuTab.FarmAndFishing.ToString(), i18n.Get("tabs.farm-and-fishing")));
                this.Tabs.Add(new ClickableComponent(new Rectangle(labelX, labelY + labelHeight * i++, Game1.tileSize * 5, Game1.tileSize), MenuTab.Skills.ToString(), i18n.Get("tabs.skills")));
                this.Tabs.Add(new ClickableComponent(new Rectangle(labelX, labelY + labelHeight * i++, Game1.tileSize * 5, Game1.tileSize), MenuTab.Weather.ToString(), i18n.Get("tabs.weather")));
                this.Tabs.Add(new ClickableComponent(new Rectangle(labelX, labelY + labelHeight * i++, Game1.tileSize * 5, Game1.tileSize), MenuTab.Relationships.ToString(), i18n.Get("tabs.relationships")));
                this.Tabs.Add(new ClickableComponent(new Rectangle(labelX, labelY + labelHeight * i++, Game1.tileSize * 5, Game1.tileSize), MenuTab.WarpLocations.ToString(), i18n.Get("tabs.warp")));
                this.Tabs.Add(new ClickableComponent(new Rectangle(labelX, labelY + labelHeight * i++, Game1.tileSize * 5, Game1.tileSize), MenuTab.Time.ToString(), i18n.Get("tabs.time")));
                this.Tabs.Add(new ClickableComponent(new Rectangle(labelX, labelY + labelHeight * i++, Game1.tileSize * 5, Game1.tileSize), MenuTab.Advanced.ToString(), i18n.Get("tabs.advanced")));
                this.Tabs.Add(new ClickableComponent(new Rectangle(labelX, labelY + labelHeight * i, Game1.tileSize * 5, Game1.tileSize), MenuTab.Controls.ToString(), i18n.Get("tabs.controls")));
            }

            this.UpArrow = new ClickableTextureComponent("up-arrow", new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), "", "", Game1.mouseCursors, new Rectangle(421, 459, 11, 12), Game1.pixelZoom);
            this.DownArrow = new ClickableTextureComponent("down-arrow", new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize / 4, this.yPositionOnScreen + this.height - Game1.tileSize, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), "", "", Game1.mouseCursors, new Rectangle(421, 472, 11, 12), Game1.pixelZoom);
            this.Scrollbar = new ClickableTextureComponent("scrollbar", new Rectangle(this.UpArrow.bounds.X + Game1.pixelZoom * 3, this.UpArrow.bounds.Y + this.UpArrow.bounds.Height + Game1.pixelZoom, 6 * Game1.pixelZoom, 10 * Game1.pixelZoom), "", "", Game1.mouseCursors, new Rectangle(435, 463, 6, 10), Game1.pixelZoom);
            this.ScrollbarRunner = new Rectangle(this.Scrollbar.bounds.X, this.UpArrow.bounds.Y + this.UpArrow.bounds.Height + Game1.pixelZoom, this.Scrollbar.bounds.Width, this.height - Game1.tileSize * 2 - this.UpArrow.bounds.Height - Game1.pixelZoom * 2);
            for (int i = 0; i < CheatsMenu.ItemsPerPage; i++)
                this.OptionSlots.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize * 5 / 4 + Game1.pixelZoom + i * ((this.height - Game1.tileSize * 2) / CheatsMenu.ItemsPerPage), this.width - Game1.tileSize / 2, (this.height - Game1.tileSize * 2) / CheatsMenu.ItemsPerPage + Game1.pixelZoom), string.Concat(i)));

            this.SetOptions();
        }

        /// <summary>Set the options to display.</summary>
        private void SetOptions()
        {
            ITranslationHelper i18n = this.TranslationHelper;
            ModConfig config = this.Config;
            Cheats cheats = this.Cheats;

            int slotWidth = this.OptionSlots[0].bounds.Width;
            this.Options.Clear();
            switch (this.CurrentTab)
            {
                case MenuTab.PlayerAndTools:
                    this.Options.Add(new OptionsElement($"{i18n.Get("player.title")}:"));
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("player.infinite-stamina"), config.InfiniteStamina, value => config.InfiniteStamina = value));
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("player.infinite-health"), config.InfiniteHealth, value => config.InfiniteHealth = value));
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("player.increased-movement-speed"), config.IncreasedMovement, value => config.IncreasedMovement = value));
                    this.Options.Add(new CheatsOptionsSlider(i18n.Get("player.movement-speed"), this.Config.MoveSpeed, 10, value => this.Config.MoveSpeed = value, disabled: () => !this.Config.IncreasedMovement));
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("player.one-hit-kill"), config.OneHitKill, value => config.OneHitKill = value));
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("player.max-daily-luck"), config.MaxDailyLuck, value => config.MaxDailyLuck = value));

                    this.Options.Add(new OptionsElement($"{i18n.Get("tools.title")}:"));
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("tools.infinite-water"), config.InfiniteWateringCan, value => config.InfiniteWateringCan = value));
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("tools.one-hit-break"), config.OneHitBreak, value => config.OneHitBreak = value));
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("tools.harvest-with-scythe"), config.HarvestScythe, value => config.HarvestScythe = value));

                    this.Options.Add(new OptionsElement($"{i18n.Get("money.title")}:"));
                    this.Options.Add(new CheatsOptionsInputListener(i18n.Get("money.add-amount", new { amount = 100 }), 2, slotWidth, config, cheats, i18n));
                    this.Options.Add(new CheatsOptionsInputListener(i18n.Get("money.add-amount", new { amount = 1000 }), 3, slotWidth, config, cheats, i18n));
                    this.Options.Add(new CheatsOptionsInputListener(i18n.Get("money.add-amount", new { amount = 10000 }), 4, slotWidth, config, cheats, i18n));
                    this.Options.Add(new CheatsOptionsInputListener(i18n.Get("money.add-amount", new { amount = 100000 }), 5, slotWidth, config, cheats, i18n));

                    this.Options.Add(new OptionsElement($"{i18n.Get("casino-coins.title")}:"));
                    this.Options.Add(new CheatsOptionsInputListener(i18n.Get("casino-coins.add-amount", new { amount = 100 }), 6, slotWidth, config, cheats, i18n));
                    this.Options.Add(new CheatsOptionsInputListener(i18n.Get("casino-coins.add-amount", new { amount = 1000 }), 7, slotWidth, config, cheats, i18n));
                    this.Options.Add(new CheatsOptionsInputListener(i18n.Get("casino-coins.add-amount", new { amount = 10000 }), 8, slotWidth, config, cheats, i18n));
                    break;

                case MenuTab.FarmAndFishing:
                    this.Options.Add(new OptionsElement($"{i18n.Get("farm.title")}:"));
                    this.Options.Add(new CheatsOptionsInputListener(i18n.Get("farm.water-all-fields"), 9, slotWidth, config, cheats, i18n));
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("farm.durable-fences"), config.DurableFences, value => config.DurableFences = value));
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("farm.instant-build"), config.InstantBuild, value => config.InstantBuild = value));
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("farm.always-auto-feed"), config.AutoFeed, value => config.AutoFeed = value));
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("farm.infinite-hay"), config.InfiniteHay, value => config.InfiniteHay = value));

                    this.Options.Add(new OptionsElement($"{i18n.Get("fishing.title")}:"));
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("fishing.instant-catch"), config.InstantCatch, value => config.InstantCatch = value));
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("fishing.instant-bite"), config.InstantBite, value => config.InstantBite = value));
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("fishing.always-throw-max-distance"), config.ThrowBobberMax, value => config.ThrowBobberMax = value));
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("fishing.always-treasure"), config.AlwaysTreasure, value => config.AlwaysTreasure = value));
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("fishing.durable-tackles"), config.DurableTackles, value => config.DurableTackles = value));

                    this.Options.Add(new OptionsElement($"{i18n.Get("fast-machines.title")}:"));
                    this.AddSortedOptions(this.Options,
                        new CheatsOptionsCheckbox(i18n.Get("fast-machines.bee-house"), config.FastBeeHouse, value => config.FastBeeHouse = value),
                        new CheatsOptionsCheckbox(i18n.Get("fast-machines.cask"), config.FastCask, value => config.FastCask = value),
                        new CheatsOptionsCheckbox(i18n.Get("fast-machines.charcoal-kiln"), config.FastCharcoalKiln, value => config.FastCharcoalKiln = value),
                        new CheatsOptionsCheckbox(i18n.Get("fast-machines.cheese-press"), config.FastCheesePress, value => config.FastCheesePress = value),
                        new CheatsOptionsCheckbox(i18n.Get("fast-machines.crystalarium"), config.FastCrystalarium, value => config.FastCrystalarium = value),
                        new CheatsOptionsCheckbox(i18n.Get("fast-machines.fruit-trees"), config.FastFruitTree, value => config.FastFruitTree = value),
                        new CheatsOptionsCheckbox(i18n.Get("fast-machines.furnace"), config.FastFurnace, value => config.FastFurnace = value),
                        new CheatsOptionsCheckbox(i18n.Get("fast-machines.incubator"), config.FastIncubator, value => config.FastIncubator = value),
                        new CheatsOptionsCheckbox(i18n.Get("fast-machines.keg"), config.FastKeg, value => config.FastKeg = value),
                        new CheatsOptionsCheckbox(i18n.Get("fast-machines.lightning-rod"), config.FastLightningRod, value => config.FastLightningRod = value),
                        new CheatsOptionsCheckbox(i18n.Get("fast-machines.loom"), config.FastLoom, value => config.FastLoom = value),
                        new CheatsOptionsCheckbox(i18n.Get("fast-machines.mayonnaise-machine"), config.FastMayonnaiseMachine, value => config.FastMayonnaiseMachine = value),
                        new CheatsOptionsCheckbox(i18n.Get("fast-machines.mushroom-box"), config.FastMushroomBox, value => config.FastMushroomBox = value),
                        new CheatsOptionsCheckbox(i18n.Get("fast-machines.oil-maker"), config.FastOilMaker, value => config.FastOilMaker = value),
                        new CheatsOptionsCheckbox(i18n.Get("fast-machines.preserves-jar"), config.FastPreservesJar, value => config.FastPreservesJar = value),
                        new CheatsOptionsCheckbox(i18n.Get("fast-machines.recycling-machine"), config.FastRecyclingMachine, value => config.FastRecyclingMachine = value),
                        new CheatsOptionsCheckbox(i18n.Get("fast-machines.seed-maker"), config.FastSeedMaker, value => config.FastSeedMaker = value),
                        new CheatsOptionsCheckbox(i18n.Get("fast-machines.slime-egg-press"), config.FastSlimeEggPress, value => config.FastSlimeEggPress = value),
                        new CheatsOptionsCheckbox(i18n.Get("fast-machines.slime-incubator"), config.FastSlimeIncubator, value => config.FastSlimeIncubator = value),
                        new CheatsOptionsCheckbox(i18n.Get("fast-machines.tapper"), config.FastTapper, value => config.FastTapper = value),
                        new CheatsOptionsCheckbox(i18n.Get("fast-machines.wood-chipper"), config.FastWoodChipper, value => config.FastWoodChipper = value),
                        new CheatsOptionsCheckbox(i18n.Get("fast-machines.worm-bin"), config.FastWormBin, value => config.FastWormBin = value)
                    );
                    break;

                case MenuTab.Skills:
                    this.Options.Add(new OptionsElement($"{i18n.Get("skills.title")}:"));
                    this.Options.Add(new CheatsOptionsInputListener(i18n.Get("skills.increase-farming"), 200, slotWidth, config, cheats, i18n));
                    this.Options.Add(new CheatsOptionsInputListener(i18n.Get("skills.increase-mining"), 201, slotWidth, config, cheats, i18n));
                    this.Options.Add(new CheatsOptionsInputListener(i18n.Get("skills.increase-foraging"), 202, slotWidth, config, cheats, i18n));
                    this.Options.Add(new CheatsOptionsInputListener(i18n.Get("skills.increase-fishing"), 203, slotWidth, config, cheats, i18n));
                    this.Options.Add(new CheatsOptionsInputListener(i18n.Get("skills.increase-combat"), 204, slotWidth, config, cheats, i18n));
                    this.Options.Add(new CheatsOptionsInputListener(i18n.Get("skills.reset"), 205, slotWidth, config, cheats, i18n));
                    this.Options.Add(new OptionsElement($"{i18n.Get("professions.title")}:"));
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("professions.combat.fighter"), this.GetProfession(Farmer.fighter), value => this.SetProfession(Farmer.fighter, value)));
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("professions.combat.scout"), this.GetProfession(Farmer.scout), value => this.SetProfession(Farmer.scout, value)));
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("professions.combat.acrobat"), this.GetProfession(Farmer.acrobat), value => this.SetProfession(Farmer.acrobat, value)));
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("professions.combat.brute"), this.GetProfession(Farmer.brute), value => this.SetProfession(Farmer.brute, value)));
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("professions.combat.defender"), this.GetProfession(Farmer.defender), value => this.SetProfession(Farmer.defender, value)));
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("professions.combat.desperado"), this.GetProfession(Farmer.desperado), value => this.SetProfession(Farmer.desperado, value)));
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("professions.farming.rancher"), this.GetProfession(Farmer.rancher), value => this.SetProfession(Farmer.rancher, value)));
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("professions.farming.tiller"), this.GetProfession(Farmer.tiller), value => this.SetProfession(Farmer.tiller, value)));
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("professions.farming.agriculturist"), this.GetProfession(Farmer.agriculturist), value => this.SetProfession(Farmer.agriculturist, value)));
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("professions.farming.artisan"), this.GetProfession(Farmer.artisan), value => this.SetProfession(Farmer.artisan, value)));
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("professions.farming.coopmaster"), this.GetProfession(Farmer.butcher), value => this.SetProfession(Farmer.butcher, value))); // butcher = coopmaster
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("professions.farming.shepherd"), this.GetProfession(Farmer.shepherd), value => this.SetProfession(Farmer.shepherd, value)));
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("professions.fishing.fisher"), this.GetProfession(Farmer.fisher), value => this.SetProfession(Farmer.fisher, value)));
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("professions.fishing.trapper"), this.GetProfession(Farmer.trapper), value => this.SetProfession(Farmer.trapper, value)));
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("professions.fishing.angler"), this.GetProfession(Farmer.angler), value => this.SetProfession(Farmer.angler, value)));
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("professions.fishing.luremaster"), this.GetProfession(Farmer.mariner), value => this.SetProfession(Farmer.mariner, value))); // mariner = luremaster (???)
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("professions.fishing.mariner"), this.GetProfession(Farmer.baitmaster), value => this.SetProfession(Farmer.baitmaster, value))); // baitmaster = mariner (???)
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("professions.fishing.pirate"), this.GetProfession(Farmer.pirate), value => this.SetProfession(Farmer.pirate, value)));
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("professions.foraging.forester"), this.GetProfession(Farmer.forester), value => this.SetProfession(Farmer.forester, value)));
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("professions.foraging.gatherer"), this.GetProfession(Farmer.gatherer), value => this.SetProfession(Farmer.gatherer, value)));
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("professions.foraging.botanist"), this.GetProfession(Farmer.botanist), value => this.SetProfession(Farmer.botanist, value)));
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("professions.foraging.lumberjack"), this.GetProfession(Farmer.lumberjack), value => this.SetProfession(Farmer.lumberjack, value)));
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("professions.foraging.tapper"), this.GetProfession(Farmer.tapper), value => this.SetProfession(Farmer.tapper, value)));
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("professions.foraging.tracker"), this.GetProfession(Farmer.tracker), value => this.SetProfession(Farmer.tracker, value)));
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("professions.mining.geologist"), this.GetProfession(Farmer.geologist), value => this.SetProfession(Farmer.geologist, value)));
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("professions.mining.miner"), this.GetProfession(Farmer.miner), value => this.SetProfession(Farmer.miner, value)));
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("professions.mining.blacksmith"), this.GetProfession(Farmer.blacksmith), value => this.SetProfession(Farmer.blacksmith, value)));
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("professions.mining.excavator"), this.GetProfession(Farmer.excavator), value => this.SetProfession(Farmer.excavator, value)));
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("professions.mining.gemologist"), this.GetProfession(Farmer.gemologist), value => this.SetProfession(Farmer.gemologist, value)));
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("professions.mining.prospector"), this.GetProfession(Farmer.burrower), value => this.SetProfession(Farmer.burrower, value))); // burrower = prospector
                    break;

                case MenuTab.Weather:
                    this.Options.Add(new OptionsElement($"{i18n.Get("weather.title")}:"));
                    this.Options.Add(new CheatsOptionsWeatherElement($"{i18n.Get("weather.current")}", () => CJB.GetWeatherNexDay(i18n)));
                    this.Options.Add(new CheatsOptionsInputListener(i18n.Get("weather.sunny"), 10, slotWidth, config, cheats, i18n));
                    this.Options.Add(new CheatsOptionsInputListener(i18n.Get("weather.raining"), 11, slotWidth, config, cheats, i18n));
                    this.Options.Add(new CheatsOptionsInputListener(i18n.Get("weather.lightning"), 12, slotWidth, config, cheats, i18n));
                    this.Options.Add(new CheatsOptionsInputListener(i18n.Get("weather.snowing"), 13, slotWidth, config, cheats, i18n));
                    break;

                case MenuTab.Relationships:
                    {
                        this.Options.Add(new OptionsElement($"{i18n.Get("relationships.title")}:"));
                        this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("relationships.give-gifts-anytime"), config.AlwaysGiveGift, value => config.AlwaysGiveGift = value));
                        this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("relationships.no-decay"), config.NoFriendshipDecay, value => config.NoFriendshipDecay = value));
                        this.Options.Add(new OptionsElement($"{i18n.Get("relationships.friends")}:"));
                        this.AddSortedOptions(this.Options,
                            this.GetSocialCharacters()
                                .Distinct()
                                .Select(npc => (OptionsElement)new CheatsOptionsNpcSlider(npc, onValueChanged: points => this.Cheats.UpdateFriendship(npc, points)))
                                .ToArray()
                        );
                    }
                    break;

                case MenuTab.WarpLocations:
                    this.Options.Add(new OptionsElement($"{i18n.Get("warp.title")}:"));
                    this.Options.Add(new CheatsOptionsInputListener(i18n.Get("warp.farm"), slotWidth, this.WarpToFarm));

                    var sortedWarps = new List<OptionsElement>();
                    if (Game1.player.hasClubCard)
                        sortedWarps.Add(new CheatsOptionsInputListener(i18n.Get("warp.casino"), slotWidth, () => this.Warp("Club", 8, 11)));
                    foreach (ModDataWarp warp in this.Warps)
                    {
                        string displayText = i18n.Get(warp.DisplayText).Default(warp.DisplayText);
                        sortedWarps.Add(new CheatsOptionsInputListener(displayText, slotWidth, () => this.Warp(warp.Location, (int)warp.Tile.X, (int)warp.Tile.Y)));
                    }

                    this.AddSortedOptions(this.Options, sortedWarps.ToArray());
                    break;

                case MenuTab.Time:
                    this.Options.Add(new OptionsElement($"{i18n.Get("time.title")}:"));
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("time.freeze-inside"), config.FreezeTimeInside, value => config.FreezeTimeInside = value));
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("time.freeze-caves"), config.FreezeTimeCaves, value => config.FreezeTimeCaves = value));
                    this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("time.freeze-everywhere"), config.FreezeTime, value => config.FreezeTime = value));
                    this.Options.Add(new CheatsOptionsSlider(i18n.Get("time.time"), (Game1.timeOfDay - 600) / 100, 19, value => this.SafelySetTime((value * 100) + 600), width: 100, format: value => Game1.getTimeOfDayString((value * 100) + 600)));
                    break;

                case MenuTab.Advanced:
                    {
                        this.Options.AddRange(this.GetDescriptionElements(i18n.Get("flags.warning")));

                        // quests
                        this.Options.Add(new OptionsElement($"{i18n.Get("flags.quests")}:"));
                        {
                            int i = 0;
                            foreach (Quest quest in Game1.player.questLog)
                            {
                                if (!quest.completed.Value)
                                    this.Options.Add(new CheatsOptionsInputListener(quest.questTitle, 300 + i++, slotWidth, config, cheats, i18n));
                            }
                        }

                        // wallet items
                        this.Options.Add(new OptionsElement($"{i18n.Get("flags.wallet")}:"));
                        this.AddSortedOptions(this.Options,
                            new CheatsOptionsCheckbox(Game1.content.LoadString(@"Strings\StringsFromCSFiles:SkillsPage.cs.11587"), Game1.player.canUnderstandDwarves, value => Game1.player.canUnderstandDwarves = value),
                            new CheatsOptionsCheckbox(Game1.content.LoadString(@"Strings\StringsFromCSFiles:SkillsPage.cs.11588"), Game1.player.hasRustyKey, value => Game1.player.hasRustyKey = value),
                            new CheatsOptionsCheckbox(Game1.content.LoadString(@"Strings\StringsFromCSFiles:SkillsPage.cs.11589"), Game1.player.hasClubCard, value => Game1.player.hasClubCard = value),
                            new CheatsOptionsCheckbox(Game1.content.LoadString(@"Strings\StringsFromCSFiles:SkillsPage.cs.11590"), Game1.player.hasSpecialCharm, value => Game1.player.hasSpecialCharm = value),
                            new CheatsOptionsCheckbox(Game1.content.LoadString(@"Strings\StringsFromCSFiles:SkillsPage.cs.11591"), Game1.player.hasSkullKey, value => Game1.player.hasSkullKey = value),
                            new CheatsOptionsCheckbox(Game1.content.LoadString(@"Strings\Objects:MagnifyingGlass"), Game1.player.hasMagnifyingGlass, value => Game1.player.hasMagnifyingGlass = value),
                            new CheatsOptionsCheckbox(Game1.content.LoadString(@"Strings\Objects:DarkTalisman"), Game1.player.hasDarkTalisman, value => Game1.player.hasDarkTalisman = value),
                            new CheatsOptionsCheckbox(Game1.content.LoadString(@"Strings\Objects:MagicInk"), Game1.player.hasMagicInk, value => Game1.player.hasMagicInk = value),
                            new CheatsOptionsCheckbox(Game1.content.LoadString(@"Strings\Objects:BearPaw"), this.HasEvent(2120303), value => this.SetEvent(2120303, value)),
                            new CheatsOptionsCheckbox(Game1.content.LoadString(@"Strings\Objects:SpringOnionBugs"), this.HasEvent(3910979), value => this.SetEvent(3910979, value))
                        );

                        // unlocked areas
                        this.Options.Add(new OptionsElement($"{i18n.Get("flags.unlocked")}:"));
                        this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("flags.unlocked.guild"), this.HasFlag("guildMember"), value => this.SetFlag(value, "guildMember")));
                        this.AddSortedOptions(this.Options,
                            this.GetSocialCharacters()
                                .Select(npc => (OptionsElement)new CheatsOptionsCheckbox(i18n.Get("flags.unlocked.room", new { name = npc.displayName }), this.HasFlag($"doorUnlock{npc.Name}"), value => this.SetFlag(value, $"doorUnlock{npc.Name}")))
                                .ToArray()
                        );

                        // unlocked content
                        this.Options.Add(new OptionsElement($"{i18n.Get("flags.unlocked-content")}:"));
                        this.AddSortedOptions(this.Options,
                            new CheatsOptionsCheckbox(i18n.Get("flags.unlocked-content.dyes-and-tailoring"), this.HasEvent(992559), value => this.SetEvent(992559, value))
                        );

                        // community center
                        this.Options.Add(new OptionsElement($"{i18n.Get("flags.community-center")}:"));
                        this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("flags.community-center.door-unlocked"), this.HasFlag("ccDoorUnlock"), value => this.SetFlag(value, "ccDoorUnlock")));
                        this.Options.Add(new CheatsOptionsCheckbox(i18n.Get("flags.jojamart.membership"), this.HasFlag("JojaMember"), value => this.SetCommunityCenterFlags(value, "JojaMember")));
                        this.AddSortedOptions(this.Options,
                            new CheatsOptionsCheckbox(this.GetJunimoRewardText("BoilerRoom", "Boiler"), this.HasFlag("ccBoilerRoom"), value => this.SetCommunityCenterFlags(value, "ccBoilerRoom")),
                            new CheatsOptionsCheckbox(this.GetJunimoRewardText("BulletinBoard", "Bulletin"), this.HasFlag("ccBulletin"), value => this.SetCommunityCenterFlags(value, "ccBulletin")),
                            new CheatsOptionsCheckbox(this.GetJunimoRewardText("CraftsRoom", "Crafts"), this.HasFlag("ccCraftsRoom"), value => this.SetCommunityCenterFlags(value, "ccCraftsRoom")),
                            new CheatsOptionsCheckbox(this.GetJunimoRewardText("FishTank", "FishTank"), this.HasFlag("ccFishTank"), value => this.SetCommunityCenterFlags(value, "ccFishTank")),
                            new CheatsOptionsCheckbox(this.GetJunimoRewardText("Pantry", "Pantry"), this.HasFlag("ccPantry"), value => this.SetCommunityCenterFlags(value, "ccPantry")),
                            new CheatsOptionsCheckbox(this.GetJunimoRewardText("Vault", "Vault"), this.HasFlag("ccVault"), value => this.SetCommunityCenterFlags(value, "ccVault"))
                        );
                        this.Options.Add(new CheatsOptionsCheckbox(this.GetJunimoRewardText("AbandonedJojaMart"), this.HasFlag("ccMovieTheater"), value => this.SetCommunityCenterFlags(value, "ccMovieTheater")));
                    }
                    break;

                case MenuTab.Controls:
                    this.Options.Add(new OptionsElement($"{i18n.Get("controls.title")}:"));
                    this.Options.Add(new CheatsOptionsInputListener(i18n.Get("controls.open-menu"), 1000, slotWidth, config, cheats, i18n));
                    this.Options.Add(new CheatsOptionsInputListener(i18n.Get("controls.freeze-time"), 1001, slotWidth, config, cheats, i18n));
                    this.Options.Add(new CheatsOptionsInputListener(i18n.Get("controls.grow-tree"), 1002, slotWidth, config, cheats, i18n));
                    this.Options.Add(new CheatsOptionsInputListener(i18n.Get("controls.grow-crops"), 1003, slotWidth, config, cheats, i18n));
                    break;
            }
            this.SetScrollBarToCurrentIndex();
        }

        /// <summary>Get option elements to display a paragraph of explanatory text.</summary>
        /// <param name="text">The text to display.</param>
        private IEnumerable<DescriptionElement> GetDescriptionElements(string text)
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
                        yield return new DescriptionElement(line);
                        line = word;
                    }
                }
                if (line != "")
                    yield return new DescriptionElement(line);
            }
        }

        /// <summary>Add options to the list in alphabetical label order.</summary>
        /// <param name="list">The option list to populate.</param>
        /// <param name="elements">The options to add.</param>
        private void AddSortedOptions(List<OptionsElement> list, params OptionsElement[] elements)
        {
            list.AddRange(elements.OrderBy(p => p.label));
        }

        /// <summary>Get the display text for a toggle to mark a Community Center or JojaMart bundle complete.</summary>
        /// <param name="areaName">The name used in the translation key for the bundle name.</param>
        /// <param name="rewardName">The name used in the translation key for the reward name (or <c>null</c> to show '???').</param>
        private string GetJunimoRewardText(string areaName, string rewardName = null)
        {
            return $"{Game1.content.LoadString($@"Strings\Locations:CommunityCenter_AreaName_{areaName}")} ({(rewardName != null ? Game1.content.LoadString($@"Strings\UI:JunimoNote_Reward{rewardName}") : "???")})";
        }

        /// <summary>Get all NPCs which have relationship data.</summary>
        /// <remarks>Derived from the <see cref="SocialPage"/> constructor.</remarks>
        private IEnumerable<NPC> GetSocialCharacters()
        {
            foreach (NPC npc in Utility.getAllCharacters())
            {
                if (npc.CanSocialize || Game1.player.friendshipData.ContainsKey(npc.Name))
                    yield return npc;
            }
        }

        /// <summary>Get whether the player has the given mail flag.</summary>
        /// <param name="flag">The mail flag to check.</param>
        public bool HasFlag(string flag)
        {
            return Game1.player.mailReceived.Contains(flag);
        }

        /// <summary>Set whether the player has the given mail flag.</summary>
        /// <param name="enable">Whether to add the flag, as opposed to removing it.</param>
        /// <param name="flag">The mail flag to set.</param>
        /// <param name="log">Whether to log the flag change.</param>
        /// <returns>Returns whether the flag changed.</returns>
        public bool SetFlag(bool enable, string flag, bool log = true)
        {
            bool changed = false;
            if (enable)
            {
                if (!this.HasFlag(flag))
                {
                    Game1.player.mailReceived.Add(flag);
                    changed = true;
                }
            }
            else
                changed = Game1.player.mailReceived.Remove(flag);

            if (log && changed)
                this.Monitor.Log($"{(enable ? "Set" : "Unset")} flag: {flag}", LogLevel.Trace);

            return changed;
        }

        /// <summary>Set whether the player has the given mail flag, and automatically fix issues related to community center flag changes.</summary>
        /// <param name="enable">Whether to add the flag, as opposed to removing it.</param>
        /// <param name="flags">The mail flags to set.</param>
        public void SetCommunityCenterFlags(bool enable, params string[] flags)
        {
            // track changes
            IDictionary<bool, HashSet<string>> logFlags = new Dictionary<bool, HashSet<string>>
            {
                [true] = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase),
                [false] = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase)
            };
            void SetAndTrackFlag(bool tryAdd, string flag)
            {
                if (this.SetFlag(tryAdd, flag, log: false))
                {
                    logFlags[tryAdd].Add(flag);
                    logFlags[!tryAdd].Remove(flag);
                }
            }

            // set initial flags
            foreach (string flag in flags)
                SetAndTrackFlag(enable, flag);

            // adjust game to reflect changes
            {
                bool allAreasDone = this.CommunityCenterCompletionFlags.Keys.All(this.HasFlag);
                bool isJoja = this.HasFlag("JojaMember");

                // fix completion flags
                SetAndTrackFlag(allAreasDone, "ccComplete");
                foreach (var pair in this.JojaMartCompletionFlags)
                {
                    bool areaDone = isJoja && this.HasFlag(pair.Value);
                    SetAndTrackFlag(areaDone, pair.Key);
                }

                // mark areas complete
                if (Game1.getLocationFromName("CommunityCenter") is CommunityCenter communityCenter)
                {
                    foreach (var pair in this.CommunityCenterCompletionFlags)
                    {
                        if (communityCenter.areasComplete.Length > pair.Value)
                            communityCenter.areasComplete[pair.Value] = this.HasFlag(pair.Key);
                    }
                }
                if (this.HasFlag("ccMovieTheater") && Game1.getLocationFromName(nameof(AbandonedJojaMart)) is AbandonedJojaMart mart)
                    mart.restoreAreaCutscene();

                // log flag changes
                bool added = logFlags[true].Any();
                bool removed = logFlags[false].Any();
                if (added || removed)
                {
                    string summary = "";
                    if (added)
                        summary += $"Set flags: {string.Join(", ", logFlags[true])}";
                    if (removed)
                    {
                        if (added)
                            summary += "; unset ";
                        else
                            summary += "Unset ";
                        summary += $"flags: {string.Join(", ", logFlags[false])}";
                    }
                    this.Monitor.Log(summary, LogLevel.Trace);
                }
            }
        }

        /// <summary>Get whether the player has seen the given event.</summary>
        /// <param name="id">The event ID to check.</param>
        public bool HasEvent(int id)
        {
            return Game1.player.eventsSeen.Contains(id);
        }

        /// <summary>Set whether the player has seen the given event.</summary>
        /// <param name="id">The event to set.</param>
        /// <param name="enable">Whether to add the event, as opposed to removing it.</param>
        public void SetEvent(int id, bool enable)
        {
            if (enable)
            {
                if (!this.HasEvent(id))
                    Game1.player.eventsSeen.Add(id);
            }
            else
                Game1.player.eventsSeen.Remove(id);
        }

        /// <summary>Safely transition to the given time, allowing NPCs to update their schedule.</summary>
        /// <param name="time">The time of day.</param>
        private void SafelySetTime(int time)
        {
            // define conversion between game time and TimeSpan
            TimeSpan ToTimeSpan(int value) => new TimeSpan(0, value / 100, value % 100, 0);
            int FromTimeSpan(TimeSpan span) => (span.Hours * 100) + span.Minutes;

            // transition to new time
            int intervals = (int)((ToTimeSpan(time) - ToTimeSpan(Game1.timeOfDay)).TotalMinutes / 10);
            if (intervals > 0)
            {
                for (int i = 0; i < intervals; i++)
                    Game1.performTenMinuteClockUpdate();
            }
            else if (intervals < 0)
            {
                for (int i = 0; i > intervals; i--)
                {
                    Game1.timeOfDay = FromTimeSpan(ToTimeSpan(Game1.timeOfDay).Subtract(TimeSpan.FromMinutes(20))); // offset 20 mins so game updates to next interval
                    Game1.performTenMinuteClockUpdate();
                }
            }
        }

        /// <summary>Get whether the player has the given profession.</summary>
        /// <param name="id">The profession ID.</param>
        private bool GetProfession(int id)
        {
            return Game1.player.professions.Contains(id);
        }

        /// <summary>Toggle a player profession.</summary>
        /// <param name="id">The profession ID.</param>
        /// <param name="enable">Whether to enable the profession (else disable).</param>
        private void SetProfession(int id, bool enable)
        {
            if (enable)
            {
                if (!this.GetProfession(id))
                    Game1.player.professions.Add(id);
            }
            else
                Game1.player.professions.Remove(id);
        }

        /// <summary>Warp the player to the farm.</summary>
        private void WarpToFarm()
        {
            // try to drop farmhand in front of their cabin
            string cabinName = Game1.player.homeLocation.Value;
            if (!Context.IsMainPlayer && cabinName != null)
            {
                foreach (BuildableGameLocation location in Game1.locations.OfType<BuildableGameLocation>())
                {
                    foreach (Building building in location.buildings)
                    {
                        if (building.indoors.Value?.uniqueName.Value == cabinName)
                        {
                            int tileX = building.tileX.Value + building.humanDoor.X;
                            int tileY = building.tileY.Value + building.humanDoor.Y + 1;
                            this.Warp(location.Name, tileX, tileY);
                            return;
                        }
                    }
                }
            }

            // else farmhouse
            this.Warp("Farm", 64, 15);
        }

        /// <summary>Warp the player to the given location.</summary>
        /// <param name="locationName">The location name.</param>
        /// <param name="tileX">The tile X position.</param>
        /// <param name="tileY">The tile Y position.</param>
        private void Warp(string locationName, int tileX, int tileY)
        {
            // reset state
            Game1.exitActiveMenu();
            Game1.player.swimming.Value = false;
            Game1.player.changeOutOfSwimSuit();

            // warp
            Game1.warpFarmer(locationName, tileX, tileY, false);
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
            bool isExitKey = Game1.options.menuButton.Contains(new InputButton(key)) || (this.Config.OpenMenuKey.TryGetKeyboard(out Keys exitKey) && key == exitKey);
            if (isExitKey && this.readyToClose() && this.CanClose)
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
                Game1.activeClickableMenu = new CheatsMenu(tabID, this.Config, this.Warps, this.Cheats, this.TranslationHelper, this.Monitor);
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
                    Game1.activeClickableMenu = new CheatsMenu(tabID, this.Config, this.Warps, this.Cheats, this.TranslationHelper, this.Monitor);
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
