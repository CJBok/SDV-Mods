using System;
using System.CodeDom.Compiler;
using System.Diagnostics.CodeAnalysis;
using StardewModdingAPI;

namespace CJBCheatsMenu.Framework
{
    /// <summary>Get translations from the mod's <c>i18n</c> folder.</summary>
    /// <remarks>This is auto-generated from the <c>i18n/default.json</c> file when the T4 template is saved.</remarks>
    [GeneratedCode("TextTemplatingFileGenerator", "1.0.0")]
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Deliberately named for consistency and to match translation conventions.")]
    internal static class I18n
    {
        /*********
        ** Fields
        *********/
        /// <summary>The mod's translation helper.</summary>
        private static ITranslationHelper Translations;


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="translations">The mod's translation helper.</param>
        public static void Init(ITranslationHelper translations)
        {
            I18n.Translations = translations;
        }

        /// <summary>Get a translation equivalent to "CJB Cheats Menu".</summary>
        public static string ModName()
        {
            return I18n.GetByKey("mod-name");
        }

        /// <summary>Get a translation equivalent to "Player & Tools".</summary>
        public static string Tabs_PlayerAndTools()
        {
            return I18n.GetByKey("tabs.player-and-tools");
        }

        /// <summary>Get a translation equivalent to "Farm & Fishing".</summary>
        public static string Tabs_FarmAndFishing()
        {
            return I18n.GetByKey("tabs.farm-and-fishing");
        }

        /// <summary>Get a translation equivalent to "Skills".</summary>
        public static string Tabs_Skills()
        {
            return I18n.GetByKey("tabs.skills");
        }

        /// <summary>Get a translation equivalent to "Weather".</summary>
        public static string Tabs_Weather()
        {
            return I18n.GetByKey("tabs.weather");
        }

        /// <summary>Get a translation equivalent to "Relationships".</summary>
        public static string Tabs_Relationships()
        {
            return I18n.GetByKey("tabs.relationships");
        }

        /// <summary>Get a translation equivalent to "Warp Locations".</summary>
        public static string Tabs_Warp()
        {
            return I18n.GetByKey("tabs.warp");
        }

        /// <summary>Get a translation equivalent to "Time".</summary>
        public static string Tabs_Time()
        {
            return I18n.GetByKey("tabs.time");
        }

        /// <summary>Get a translation equivalent to "Advanced".</summary>
        public static string Tabs_Advanced()
        {
            return I18n.GetByKey("tabs.advanced");
        }

        /// <summary>Get a translation equivalent to "Controls".</summary>
        public static string Tabs_Controls()
        {
            return I18n.GetByKey("tabs.controls");
        }

        /// <summary>Get a translation equivalent to "Player".</summary>
        public static string Player_Title()
        {
            return I18n.GetByKey("player.title");
        }

        /// <summary>Get a translation equivalent to "Infinite Stamina".</summary>
        public static string Player_InfiniteStamina()
        {
            return I18n.GetByKey("player.infinite-stamina");
        }

        /// <summary>Get a translation equivalent to "Infinite Health".</summary>
        public static string Player_InfiniteHealth()
        {
            return I18n.GetByKey("player.infinite-health");
        }

        /// <summary>Get a translation equivalent to "Instant Weapon Cooldowns".</summary>
        public static string Player_InstantCooldowns()
        {
            return I18n.GetByKey("player.instant-cooldowns");
        }

        /// <summary>Get a translation equivalent to "Inventory Size".</summary>
        public static string Player_InventorySize()
        {
            return I18n.GetByKey("player.inventory-size");
        }

        /// <summary>Get a translation equivalent to "Move Speed".</summary>
        public static string Player_MovementSpeed()
        {
            return I18n.GetByKey("player.movement-speed");
        }

        /// <summary>Get a translation equivalent to "normal".</summary>
        public static string Player_MovementSpeed_Default()
        {
            return I18n.GetByKey("player.movement-speed.default");
        }

        /// <summary>Get a translation equivalent to "One Hit Kill".</summary>
        public static string Player_OneHitKill()
        {
            return I18n.GetByKey("player.one-hit-kill");
        }

        /// <summary>Get a translation equivalent to "Max Daily Luck".</summary>
        public static string Player_MaxDailyLuck()
        {
            return I18n.GetByKey("player.max-daily-luck");
        }

        /// <summary>Get a translation equivalent to "Tools".</summary>
        public static string Tools_Title()
        {
            return I18n.GetByKey("tools.title");
        }

        /// <summary>Get a translation equivalent to "Infinite Water in Can".</summary>
        public static string Tools_InfiniteWater()
        {
            return I18n.GetByKey("tools.infinite-water");
        }

        /// <summary>Get a translation equivalent to "One Hit Break".</summary>
        public static string Tools_OneHitBreak()
        {
            return I18n.GetByKey("tools.one-hit-break");
        }

        /// <summary>Get a translation equivalent to "Harvest With Scythe".</summary>
        public static string Tools_HarvestWithScythe()
        {
            return I18n.GetByKey("tools.harvest-with-scythe");
        }

        /// <summary>Get a translation equivalent to "Money".</summary>
        public static string Add_Money()
        {
            return I18n.GetByKey("add.money");
        }

        /// <summary>Get a translation equivalent to "Casino Coins".</summary>
        public static string Add_CasinoCoins()
        {
            return I18n.GetByKey("add.casino-coins");
        }

        /// <summary>Get a translation equivalent to "Golden Walnuts".</summary>
        public static string Add_GoldenWalnuts()
        {
            return I18n.GetByKey("add.golden-walnuts");
        }

        /// <summary>Get a translation equivalent to "Qi Gems".</summary>
        public static string Add_QiGems()
        {
            return I18n.GetByKey("add.qi-gems");
        }

        /// <summary>Get a translation equivalent to "Add {{amount}}g".</summary>
        /// <param name="amount">The value to inject for the <c>{{amount}}</c> token.</param>
        public static string Add_AmountGold(object amount)
        {
            return I18n.GetByKey("add.amount-gold", new { amount });
        }

        /// <summary>Get a translation equivalent to "Add {{amount}}".</summary>
        /// <param name="amount">The value to inject for the <c>{{amount}}</c> token.</param>
        public static string Add_AmountOther(object amount)
        {
            return I18n.GetByKey("add.amount-other", new { amount });
        }

        /// <summary>Get a translation equivalent to "Farm".</summary>
        public static string Farm_Title()
        {
            return I18n.GetByKey("farm.title");
        }

        /// <summary>Get a translation equivalent to "Auto-Water Crops".</summary>
        public static string Farm_AutoWater()
        {
            return I18n.GetByKey("farm.auto-water");
        }

        /// <summary>Get a translation equivalent to "Durable Fences".</summary>
        public static string Farm_DurableFences()
        {
            return I18n.GetByKey("farm.durable-fences");
        }

        /// <summary>Get a translation equivalent to "Instant Build".</summary>
        public static string Farm_InstantBuild()
        {
            return I18n.GetByKey("farm.instant-build");
        }

        /// <summary>Get a translation equivalent to "Auto-Feed Animals".</summary>
        public static string Farm_AutoFeedAnimals()
        {
            return I18n.GetByKey("farm.auto-feed-animals");
        }

        /// <summary>Get a translation equivalent to "Auto-Pet Animals".</summary>
        public static string Farm_AutoPetAnimals()
        {
            return I18n.GetByKey("farm.auto-pet-animals");
        }

        /// <summary>Get a translation equivalent to "Infinite Hay".</summary>
        public static string Farm_InfiniteHay()
        {
            return I18n.GetByKey("farm.infinite-hay");
        }

        /// <summary>Get a translation equivalent to "Fishing".</summary>
        public static string Fishing_Title()
        {
            return I18n.GetByKey("fishing.title");
        }

        /// <summary>Get a translation equivalent to "Instant Catch".</summary>
        public static string Fishing_InstantCatch()
        {
            return I18n.GetByKey("fishing.instant-catch");
        }

        /// <summary>Get a translation equivalent to "Instant Bite".</summary>
        public static string Fishing_InstantBite()
        {
            return I18n.GetByKey("fishing.instant-bite");
        }

        /// <summary>Get a translation equivalent to "Always Throw Max Distance".</summary>
        public static string Fishing_AlwaysThrowMaxDistance()
        {
            return I18n.GetByKey("fishing.always-throw-max-distance");
        }

        /// <summary>Get a translation equivalent to "Always Treasure".</summary>
        public static string Fishing_AlwaysTreasure()
        {
            return I18n.GetByKey("fishing.always-treasure");
        }

        /// <summary>Get a translation equivalent to "Durable Tackles".</summary>
        public static string Fishing_DurableTackles()
        {
            return I18n.GetByKey("fishing.durable-tackles");
        }

        /// <summary>Get a translation equivalent to "Fast Machine Processing".</summary>
        public static string FastMachines_Title()
        {
            return I18n.GetByKey("fast-machines.title");
        }

        /// <summary>Get a translation equivalent to "Fruit Trees".</summary>
        public static string FastMachines_FruitTrees()
        {
            return I18n.GetByKey("fast-machines.fruit-trees");
        }

        /// <summary>Get a translation equivalent to "{{machineName}} (ready in the morning)".</summary>
        /// <param name="machineName">The value to inject for the <c>{{machineName}}</c> token.</param>
        public static string FastMachines_ReadyInTheMorning(object machineName)
        {
            return I18n.GetByKey("fast-machines.ready-in-the-morning", new { machineName });
        }

        /// <summary>Get a translation equivalent to "Skills".</summary>
        public static string Skills_Title()
        {
            return I18n.GetByKey("skills.title");
        }

        /// <summary>Get a translation equivalent to "Incr. Farming Lvl: {{currentLevel}}".</summary>
        /// <param name="currentLevel">The value to inject for the <c>{{currentLevel}}</c> token.</param>
        public static string Skills_IncreaseFarming(object currentLevel)
        {
            return I18n.GetByKey("skills.increase-farming", new { currentLevel });
        }

        /// <summary>Get a translation equivalent to "Incr. Mining Lvl: {{currentLevel}}".</summary>
        /// <param name="currentLevel">The value to inject for the <c>{{currentLevel}}</c> token.</param>
        public static string Skills_IncreaseMining(object currentLevel)
        {
            return I18n.GetByKey("skills.increase-mining", new { currentLevel });
        }

        /// <summary>Get a translation equivalent to "Incr. Foraging Lvl: {{currentLevel}}".</summary>
        /// <param name="currentLevel">The value to inject for the <c>{{currentLevel}}</c> token.</param>
        public static string Skills_IncreaseForaging(object currentLevel)
        {
            return I18n.GetByKey("skills.increase-foraging", new { currentLevel });
        }

        /// <summary>Get a translation equivalent to "Incr. Fishing Lvl: {{currentLevel}}".</summary>
        /// <param name="currentLevel">The value to inject for the <c>{{currentLevel}}</c> token.</param>
        public static string Skills_IncreaseFishing(object currentLevel)
        {
            return I18n.GetByKey("skills.increase-fishing", new { currentLevel });
        }

        /// <summary>Get a translation equivalent to "Incr. Combat Lvl: {{currentLevel}}".</summary>
        /// <param name="currentLevel">The value to inject for the <c>{{currentLevel}}</c> token.</param>
        public static string Skills_IncreaseCombat(object currentLevel)
        {
            return I18n.GetByKey("skills.increase-combat", new { currentLevel });
        }

        /// <summary>Get a translation equivalent to "RESET SKILLS!".</summary>
        public static string Skills_Reset()
        {
            return I18n.GetByKey("skills.reset");
        }

        /// <summary>Get a translation equivalent to "Professions".</summary>
        public static string Professions_Title()
        {
            return I18n.GetByKey("professions.title");
        }

        /// <summary>Get a translation equivalent to "Combat lvl 5 - Fighter".</summary>
        public static string Professions_Combat_Fighter()
        {
            return I18n.GetByKey("professions.combat.fighter");
        }

        /// <summary>Get a translation equivalent to "Combat lvl 5 - Scout".</summary>
        public static string Professions_Combat_Scout()
        {
            return I18n.GetByKey("professions.combat.scout");
        }

        /// <summary>Get a translation equivalent to "Combat lvl 10 - Acrobat".</summary>
        public static string Professions_Combat_Acrobat()
        {
            return I18n.GetByKey("professions.combat.acrobat");
        }

        /// <summary>Get a translation equivalent to "Combat lvl 10 - Brute".</summary>
        public static string Professions_Combat_Brute()
        {
            return I18n.GetByKey("professions.combat.brute");
        }

        /// <summary>Get a translation equivalent to "Combat lvl 10 - Defender".</summary>
        public static string Professions_Combat_Defender()
        {
            return I18n.GetByKey("professions.combat.defender");
        }

        /// <summary>Get a translation equivalent to "Combat lvl 10 - Desperado".</summary>
        public static string Professions_Combat_Desperado()
        {
            return I18n.GetByKey("professions.combat.desperado");
        }

        /// <summary>Get a translation equivalent to "Farming lvl 5 - Rancher".</summary>
        public static string Professions_Farming_Rancher()
        {
            return I18n.GetByKey("professions.farming.rancher");
        }

        /// <summary>Get a translation equivalent to "Farming lvl 5 - Tiller".</summary>
        public static string Professions_Farming_Tiller()
        {
            return I18n.GetByKey("professions.farming.tiller");
        }

        /// <summary>Get a translation equivalent to "Farming lvl 10 - Agriculturist".</summary>
        public static string Professions_Farming_Agriculturist()
        {
            return I18n.GetByKey("professions.farming.agriculturist");
        }

        /// <summary>Get a translation equivalent to "Farming lvl 10 - Artisan".</summary>
        public static string Professions_Farming_Artisan()
        {
            return I18n.GetByKey("professions.farming.artisan");
        }

        /// <summary>Get a translation equivalent to "Farming lvl 10 - Coopmaster".</summary>
        public static string Professions_Farming_Coopmaster()
        {
            return I18n.GetByKey("professions.farming.coopmaster");
        }

        /// <summary>Get a translation equivalent to "Farming lvl 10 - Shepherd".</summary>
        public static string Professions_Farming_Shepherd()
        {
            return I18n.GetByKey("professions.farming.shepherd");
        }

        /// <summary>Get a translation equivalent to "Fishing lvl 5 - Fisher".</summary>
        public static string Professions_Fishing_Fisher()
        {
            return I18n.GetByKey("professions.fishing.fisher");
        }

        /// <summary>Get a translation equivalent to "Fishing lvl 5 - Trapper".</summary>
        public static string Professions_Fishing_Trapper()
        {
            return I18n.GetByKey("professions.fishing.trapper");
        }

        /// <summary>Get a translation equivalent to "Fishing lvl 10 - Angler".</summary>
        public static string Professions_Fishing_Angler()
        {
            return I18n.GetByKey("professions.fishing.angler");
        }

        /// <summary>Get a translation equivalent to "Fishing lvl 10 - Luremaster".</summary>
        public static string Professions_Fishing_Luremaster()
        {
            return I18n.GetByKey("professions.fishing.luremaster");
        }

        /// <summary>Get a translation equivalent to "Fishing lvl 10 - Mariner".</summary>
        public static string Professions_Fishing_Mariner()
        {
            return I18n.GetByKey("professions.fishing.mariner");
        }

        /// <summary>Get a translation equivalent to "Fishing lvl 10 - Pirate".</summary>
        public static string Professions_Fishing_Pirate()
        {
            return I18n.GetByKey("professions.fishing.pirate");
        }

        /// <summary>Get a translation equivalent to "Foraging lvl 5 - Forester".</summary>
        public static string Professions_Foraging_Forester()
        {
            return I18n.GetByKey("professions.foraging.forester");
        }

        /// <summary>Get a translation equivalent to "Foraging lvl 5 - Gatherer".</summary>
        public static string Professions_Foraging_Gatherer()
        {
            return I18n.GetByKey("professions.foraging.gatherer");
        }

        /// <summary>Get a translation equivalent to "Foraging lvl 10 - Botanist".</summary>
        public static string Professions_Foraging_Botanist()
        {
            return I18n.GetByKey("professions.foraging.botanist");
        }

        /// <summary>Get a translation equivalent to "Foraging lvl 10 - Lumberjack".</summary>
        public static string Professions_Foraging_Lumberjack()
        {
            return I18n.GetByKey("professions.foraging.lumberjack");
        }

        /// <summary>Get a translation equivalent to "Foraging lvl 10 - Tapper".</summary>
        public static string Professions_Foraging_Tapper()
        {
            return I18n.GetByKey("professions.foraging.tapper");
        }

        /// <summary>Get a translation equivalent to "Foraging lvl 10 - Tracker".</summary>
        public static string Professions_Foraging_Tracker()
        {
            return I18n.GetByKey("professions.foraging.tracker");
        }

        /// <summary>Get a translation equivalent to "Mining lvl 5 - Geologist".</summary>
        public static string Professions_Mining_Geologist()
        {
            return I18n.GetByKey("professions.mining.geologist");
        }

        /// <summary>Get a translation equivalent to "Mining lvl 5 - Miner".</summary>
        public static string Professions_Mining_Miner()
        {
            return I18n.GetByKey("professions.mining.miner");
        }

        /// <summary>Get a translation equivalent to "Mining lvl 10 - Blacksmith".</summary>
        public static string Professions_Mining_Blacksmith()
        {
            return I18n.GetByKey("professions.mining.blacksmith");
        }

        /// <summary>Get a translation equivalent to "Mining lvl 10 - Excavator".</summary>
        public static string Professions_Mining_Excavator()
        {
            return I18n.GetByKey("professions.mining.excavator");
        }

        /// <summary>Get a translation equivalent to "Mining lvl 10 - Gemologist".</summary>
        public static string Professions_Mining_Gemologist()
        {
            return I18n.GetByKey("professions.mining.gemologist");
        }

        /// <summary>Get a translation equivalent to "Mining lvl 10 - Prospector".</summary>
        public static string Professions_Mining_Prospector()
        {
            return I18n.GetByKey("professions.mining.prospector");
        }

        /// <summary>Get a translation equivalent to "Weather Tomorrow".</summary>
        public static string Weather_Title()
        {
            return I18n.GetByKey("weather.title");
        }

        /// <summary>Get a translation equivalent to "Current".</summary>
        public static string Weather_Current()
        {
            return I18n.GetByKey("weather.current");
        }

        /// <summary>Get a translation equivalent to "Sunny".</summary>
        public static string Weather_Sunny()
        {
            return I18n.GetByKey("weather.sunny");
        }

        /// <summary>Get a translation equivalent to "Raining".</summary>
        public static string Weather_Raining()
        {
            return I18n.GetByKey("weather.raining");
        }

        /// <summary>Get a translation equivalent to "Lightning".</summary>
        public static string Weather_Lightning()
        {
            return I18n.GetByKey("weather.lightning");
        }

        /// <summary>Get a translation equivalent to "Snowing".</summary>
        public static string Weather_Snowing()
        {
            return I18n.GetByKey("weather.snowing");
        }

        /// <summary>Get a translation equivalent to "Relationships".</summary>
        public static string Relationships_Title()
        {
            return I18n.GetByKey("relationships.title");
        }

        /// <summary>Get a translation equivalent to "Give Gifts Anytime".</summary>
        public static string Relationships_GiveGiftsAnytime()
        {
            return I18n.GetByKey("relationships.give-gifts-anytime");
        }

        /// <summary>Get a translation equivalent to "No Friendship Decay".</summary>
        public static string Relationships_NoDecay()
        {
            return I18n.GetByKey("relationships.no-decay");
        }

        /// <summary>Get a translation equivalent to "Friends".</summary>
        public static string Relationships_Friends()
        {
            return I18n.GetByKey("relationships.friends");
        }

        /// <summary>Get a translation equivalent to "Main Areas".</summary>
        public static string WarpSection_Main()
        {
            return I18n.GetByKey("warp-section.main");
        }

        /// <summary>Get a translation equivalent to "Farm".</summary>
        public static string Warp_Farm()
        {
            return I18n.GetByKey("warp.farm");
        }

        /// <summary>Get a translation equivalent to "Carpenter".</summary>
        public static string Warp_Carpenter()
        {
            return I18n.GetByKey("warp.carpenter");
        }

        /// <summary>Get a translation equivalent to "Desert".</summary>
        public static string Warp_Desert()
        {
            return I18n.GetByKey("warp.desert");
        }

        /// <summary>Get a translation equivalent to "Mines".</summary>
        public static string Warp_Mines()
        {
            return I18n.GetByKey("warp.mines");
        }

        /// <summary>Get a translation equivalent to "Pierre's Shop".</summary>
        public static string Warp_PierreShop()
        {
            return I18n.GetByKey("warp.pierre-shop");
        }

        /// <summary>Get a translation equivalent to "Town".</summary>
        public static string WarpSection_Town()
        {
            return I18n.GetByKey("warp-section.town");
        }

        /// <summary>Get a translation equivalent to "Blacksmith".</summary>
        public static string Warp_Blacksmith()
        {
            return I18n.GetByKey("warp.blacksmith");
        }

        /// <summary>Get a translation equivalent to "Community Center".</summary>
        public static string Warp_CommunityCenter()
        {
            return I18n.GetByKey("warp.community-center");
        }

        /// <summary>Get a translation equivalent to "JojaMart".</summary>
        public static string Warp_Jojamart()
        {
            return I18n.GetByKey("warp.jojamart");
        }

        /// <summary>Get a translation equivalent to "Movie Theater".</summary>
        public static string Warp_MovieTheater()
        {
            return I18n.GetByKey("warp.movie-theater");
        }

        /// <summary>Get a translation equivalent to "Museum".</summary>
        public static string Warp_Museum()
        {
            return I18n.GetByKey("warp.museum");
        }

        /// <summary>Get a translation equivalent to "Saloon".</summary>
        public static string Warp_Saloon()
        {
            return I18n.GetByKey("warp.saloon");
        }

        /// <summary>Get a translation equivalent to "Sewers".</summary>
        public static string Warp_Sewer()
        {
            return I18n.GetByKey("warp.sewer");
        }

        /// <summary>Get a translation equivalent to "Forest".</summary>
        public static string WarpSection_Forest()
        {
            return I18n.GetByKey("warp-section.forest");
        }

        /// <summary>Get a translation equivalent to "Hats".</summary>
        public static string Warp_Hats()
        {
            return I18n.GetByKey("warp.hats");
        }

        /// <summary>Get a translation equivalent to "Marnie's Ranch".</summary>
        public static string Warp_Ranch()
        {
            return I18n.GetByKey("warp.ranch");
        }

        /// <summary>Get a translation equivalent to "Secret Woods".</summary>
        public static string Warp_SecretWoods()
        {
            return I18n.GetByKey("warp.secret-woods");
        }

        /// <summary>Get a translation equivalent to "Wizard Tower".</summary>
        public static string Warp_WizardTower()
        {
            return I18n.GetByKey("warp.wizard-tower");
        }

        /// <summary>Get a translation equivalent to "Mountain".</summary>
        public static string WarpSection_Mountain()
        {
            return I18n.GetByKey("warp-section.mountain");
        }

        /// <summary>Get a translation equivalent to "Adventurer's Guild".</summary>
        public static string Warp_AdventurersGuild()
        {
            return I18n.GetByKey("warp.adventurers-guild");
        }

        /// <summary>Get a translation equivalent to "Bathhouse".</summary>
        public static string Warp_Bathhouse()
        {
            return I18n.GetByKey("warp.bathhouse");
        }

        /// <summary>Get a translation equivalent to "Quarry".</summary>
        public static string Warp_Quarry()
        {
            return I18n.GetByKey("warp.quarry");
        }

        /// <summary>Get a translation equivalent to "Beach".</summary>
        public static string WarpSection_Beach()
        {
            return I18n.GetByKey("warp-section.beach");
        }

        /// <summary>Get a translation equivalent to "Tide Pools".</summary>
        public static string Warp_TidePools()
        {
            return I18n.GetByKey("warp.tide-pools");
        }

        /// <summary>Get a translation equivalent to "Willy's Shop".</summary>
        public static string Warp_WillyShop()
        {
            return I18n.GetByKey("warp.willy-shop");
        }

        /// <summary>Get a translation equivalent to "Ginger Island".</summary>
        public static string WarpSection_Island()
        {
            return I18n.GetByKey("warp-section.island");
        }

        /// <summary>Get a translation equivalent to "Forge".</summary>
        public static string Warp_Forge()
        {
            return I18n.GetByKey("warp.forge");
        }

        /// <summary>Get a translation equivalent to "Island Farm".</summary>
        public static string Warp_IslandFarm()
        {
            return I18n.GetByKey("warp.island-farm");
        }

        /// <summary>Get a translation equivalent to "Volcano Dwarf Shop".</summary>
        public static string Warp_DwarfShop()
        {
            return I18n.GetByKey("warp.dwarf-shop");
        }

        /// <summary>Get a translation equivalent to "Field Office".</summary>
        public static string Warp_FieldOffice()
        {
            return I18n.GetByKey("warp.field-office");
        }

        /// <summary>Get a translation equivalent to "Leo's House".</summary>
        public static string Warp_LeoHouse()
        {
            return I18n.GetByKey("warp.leo-house");
        }

        /// <summary>Get a translation equivalent to "Desert".</summary>
        public static string WarpSection_Desert()
        {
            return I18n.GetByKey("warp-section.desert");
        }

        /// <summary>Get a translation equivalent to "Casino".</summary>
        public static string Warp_Casino()
        {
            return I18n.GetByKey("warp.casino");
        }

        /// <summary>Get a translation equivalent to "Sandy's Shop".</summary>
        public static string Warp_SandyShop()
        {
            return I18n.GetByKey("warp.sandy-shop");
        }

        /// <summary>Get a translation equivalent to "Skull Cavern".</summary>
        public static string Warp_SkullCavern()
        {
            return I18n.GetByKey("warp.skull-cavern");
        }

        /// <summary>Get a translation equivalent to "Time".</summary>
        public static string Time_Title()
        {
            return I18n.GetByKey("time.title");
        }

        /// <summary>Get a translation equivalent to "Freeze Time Inside".</summary>
        public static string Time_FreezeInside()
        {
            return I18n.GetByKey("time.freeze-inside");
        }

        /// <summary>Get a translation equivalent to "Freeze Time In Caves".</summary>
        public static string Time_FreezeCaves()
        {
            return I18n.GetByKey("time.freeze-caves");
        }

        /// <summary>Get a translation equivalent to "Freeze Time Everywhere".</summary>
        public static string Time_FreezeEverywhere()
        {
            return I18n.GetByKey("time.freeze-everywhere");
        }

        /// <summary>Get a translation equivalent to "Time".</summary>
        public static string Time_Time()
        {
            return I18n.GetByKey("time.time");
        }

        /// <summary>Get a translation equivalent to "Time Frozen".</summary>
        public static string Time_TimeFrozenMessage()
        {
            return I18n.GetByKey("time.time-frozen-message");
        }

        /// <summary>Get a translation equivalent to "Date".</summary>
        public static string Date_Title()
        {
            return I18n.GetByKey("date.title");
        }

        /// <summary>Get a translation equivalent to "Use this section at your own risk!\nMany updates will not occur until a night has passed.".</summary>
        public static string Date_Warning()
        {
            return I18n.GetByKey("date.warning");
        }

        /// <summary>Get a translation equivalent to "Year".</summary>
        public static string Date_Year()
        {
            return I18n.GetByKey("date.year");
        }

        /// <summary>Get a translation equivalent to "Season".</summary>
        public static string Date_Season()
        {
            return I18n.GetByKey("date.season");
        }

        /// <summary>Get a translation equivalent to "Day".</summary>
        public static string Date_Day()
        {
            return I18n.GetByKey("date.day");
        }

        /// <summary>Get a translation equivalent to "Use this section at your own risk!\nThis may cause issues like skipped mail, events, or quests.".</summary>
        public static string Flags_Warning()
        {
            return I18n.GetByKey("flags.warning");
        }

        /// <summary>Get a translation equivalent to "Complete Quests".</summary>
        public static string Flags_Quests()
        {
            return I18n.GetByKey("flags.quests");
        }

        /// <summary>Get a translation equivalent to "Wallet Items".</summary>
        public static string Flags_Wallet()
        {
            return I18n.GetByKey("flags.wallet");
        }

        /// <summary>Get a translation equivalent to "Unlocked Areas".</summary>
        public static string Flags_Unlocked()
        {
            return I18n.GetByKey("flags.unlocked");
        }

        /// <summary>Get a translation equivalent to "Adventurer's Guild".</summary>
        public static string Flags_Unlocked_Guild()
        {
            return I18n.GetByKey("flags.unlocked.guild");
        }

        /// <summary>Get a translation equivalent to "Room: {{name}}".</summary>
        /// <param name="name">The value to inject for the <c>{{name}}</c> token.</param>
        public static string Flags_Unlocked_Room(object name)
        {
            return I18n.GetByKey("flags.unlocked.room", new { name });
        }

        /// <summary>Get a translation equivalent to "Unlocked content".</summary>
        public static string Flags_UnlockedContent()
        {
            return I18n.GetByKey("flags.unlocked-content");
        }

        /// <summary>Get a translation equivalent to "Dyes & tailoring".</summary>
        public static string Flags_UnlockedContent_DyesAndTailoring()
        {
            return I18n.GetByKey("flags.unlocked-content.dyes-and-tailoring");
        }

        /// <summary>Get a translation equivalent to "Community Center".</summary>
        public static string Flags_CommunityCenter()
        {
            return I18n.GetByKey("flags.community-center");
        }

        /// <summary>Get a translation equivalent to "Door Unlocked".</summary>
        public static string Flags_CommunityCenter_DoorUnlocked()
        {
            return I18n.GetByKey("flags.community-center.door-unlocked");
        }

        /// <summary>Get a translation equivalent to "JojaMart Membership".</summary>
        public static string Flags_Jojamart_Membership()
        {
            return I18n.GetByKey("flags.jojamart.membership");
        }

        /// <summary>Get a translation equivalent to "Controls".</summary>
        public static string Controls_Title()
        {
            return I18n.GetByKey("controls.title");
        }

        /// <summary>Get a translation equivalent to "You can change the on-screen buttons in the config files for the CJB Cheats Menu and Virtual Keyboard mods.".</summary>
        public static string Controls_AndroidConfigNote()
        {
            return I18n.GetByKey("controls.android-config-note");
        }

        /// <summary>Get a translation equivalent to "Open Menu".</summary>
        public static string Controls_OpenMenu()
        {
            return I18n.GetByKey("controls.open-menu");
        }

        /// <summary>Get a translation equivalent to "Freeze Time".</summary>
        public static string Controls_FreezeTime()
        {
            return I18n.GetByKey("controls.freeze-time");
        }

        /// <summary>Get a translation equivalent to "Grow Tree".</summary>
        public static string Controls_GrowTree()
        {
            return I18n.GetByKey("controls.grow-tree");
        }

        /// <summary>Get a translation equivalent to "Grow Crops".</summary>
        public static string Controls_GrowCrops()
        {
            return I18n.GetByKey("controls.grow-crops");
        }

        /// <summary>Get a translation equivalent to "Grow distance around player".</summary>
        public static string Controls_GrowRadius()
        {
            return I18n.GetByKey("controls.grow-radius");
        }

        /// <summary>Get a translation equivalent to "Reset".</summary>
        public static string Controls_ResetControls()
        {
            return I18n.GetByKey("controls.reset-controls");
        }

        /// <summary>Get a translation equivalent to "Press New Key...".</summary>
        public static string Controls_PressNewKey()
        {
            return I18n.GetByKey("controls.press-new-key");
        }

        /// <summary>Get a translation by its key.</summary>
        /// <param name="key">The translation key.</param>
        /// <param name="tokens">An object containing token key/value pairs. This can be an anonymous object (like <c>new { value = 42, name = "Cranberries" }</c>), a dictionary, or a class instance.</param>
        public static Translation GetByKey(string key, object tokens = null)
        {
            if (I18n.Translations == null)
                throw new InvalidOperationException($"You must call {nameof(I18n)}.{nameof(I18n.Init)} from the mod's entry method before reading translations.");
            return I18n.Translations.Get(key, tokens);
        }
    }
}

