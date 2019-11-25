using CJBCheatsMenu.Framework.Constants;
using StardewModdingAPI;

namespace CJBCheatsMenu.Framework.Models
{
    /// <summary>The mod configuration model.</summary>
    internal class ModConfig
    {
        /*********
        ** Accessors
        *********/
        /****
        ** Keyboard buttons
        ****/
        /// <summary>The button which opens the menu.</summary>
        public SButton OpenMenuKey { get; set; } = SButton.P;

        /// <summary>The button which freezes the game clock.</summary>
        public SButton FreezeTimeKey { get; set; } = SButton.T;

        /// <summary>The button held to grow trees under the cursor.</summary>
        public SButton GrowTreeKey { get; set; } = SButton.NumPad1;

        /// <summary>The button held to grow crops under the cursor.</summary>
        public SButton GrowCropsKey { get; set; } = SButton.NumPad2;

        /****
        ** Menu settings
        ****/
        /// <summary>The tab shown by default when you open the menu.</summary>
        public MenuTab DefaultTab { get; set; } = MenuTab.PlayerAndTools;

        /****
        ** Player cheats
        ****/
        /// <summary>Whether your character moves faster. The added speed is set by the <see cref="MoveSpeed"/> field.</summary>
        public bool IncreasedMovement { get; set; }

        /// <summary>The player speed to add if <see cref="IncreasedMovement"/> is <c>true</c>. This is an added multiplier (e.g. 1 doubles the default speed).</summary>
        public int MoveSpeed { get; set; } = 1;

        /// <summary>The player's health never decreases.</summary>
        public bool InfiniteHealth { get; set; }

        /// <summary>The player's stamina never decreases.</summary>
        public bool InfiniteStamina { get; set; }

        /// <summary>The player's daily luck is always at the maximum value.</summary>
        public bool MaxDailyLuck { get; set; }

        /// <summary>The player's attacks kill any monster in one hit.</summary>
        public bool OneHitKill { get; set; }

        /// <summary>The player's tools break things instantly.</summary>
        public bool OneHitBreak { get; set; }

        /// <summary>The player's watering can never runs dry.</summary>
        public bool InfiniteWateringCan { get; set; }

        /// <summary>The player can harvest any crop with the scythe.</summary>
        public bool HarvestScythe { get; set; }

        /****
        ** Fishing cheats
        ****/
        /// <summary>After casting the fishing line, the fishing minigame appears immediately.</summary>
        public bool InstantBite { get; set; }

        /// <summary>When the fishing minigame appears, the fish is caught immediately.</summary>
        public bool InstantCatch { get; set; }

        /// <summary>When casting the fishing line, it always reaches the maximum distance.</summary>
        public bool ThrowBobberMax { get; set; }

        /// <summary>Fishing tackles never break.</summary>
        public bool DurableTackles { get; set; }

        /// <summary>Every fishing minigame has a treasure.</summary>
        public bool AlwaysTreasure { get; set; }

        /****
        ** Time cheats
        ****/
        /// <summary>The game clock never changes.</summary>
        public bool FreezeTime { get; set; }

        /// <summary>The game clock doesn't change when you're inside a building.</summary>
        public bool FreezeTimeInside { get; set; }

        /// <summary>The game clock doesn't change when you're inside the mines, Skull Cavern, or farm cave.</summary>
        public bool FreezeTimeCaves { get; set; }

        /// <summary>Beehouses finish instantly.</summary>
        public bool FastBeeHouse { get; set; }

        /// <summary>Casks finish instantly.</summary>
        public bool FastCask { get; set; }

        /// <summary>Charcoal kilns finish instantly.</summary>
        public bool FastCharcoalKiln { get; set; }

        /// <summary>Cheese presses finish instantly.</summary>
        public bool FastCheesePress { get; set; }

        /// <summary>Crystalariums finish instantly.</summary>
        public bool FastCrystalarium { get; set; }

        /// <summary>Fruit trees bear fruit instantly.</summary>
        public bool FastFruitTree { get; set; }

        /// <summary>Furnaces finish instantly.</summary>
        public bool FastFurnace { get; set; }

        /// <summary>Incubators finish instantly.</summary>
        public bool FastIncubator { get; set; }

        /// <summary>Kegs finish instantly.</summary>
        public bool FastKeg { get; set; }

        /// <summary>Lightning rods finish instantly.</summary>
        public bool FastLightningRod { get; set; }

        /// <summary>Looms finish instantly.</summary>
        public bool FastLoom { get; set; }

        /// <summary>Mayonnaise machines finish instantly.</summary>
        public bool FastMayonnaiseMachine { get; set; }

        /// <summary>Mushroom boxs finish instantly.</summary>
        public bool FastMushroomBox { get; set; }

        /// <summary>Oil makers finish instantly.</summary>
        public bool FastOilMaker { get; set; }

        /// <summary>Preserves jars finish instantly.</summary>
        public bool FastPreservesJar { get; set; }

        /// <summary>Recycling machines finish instantly.</summary>
        public bool FastRecyclingMachine { get; set; }

        /// <summary>Seed makers finish instantly.</summary>
        public bool FastSeedMaker { get; set; }

        /// <summary>Slime egg presss finish instantly.</summary>
        public bool FastSlimeEggPress { get; set; }

        /// <summary>Slime incubators finish instantly.</summary>
        public bool FastSlimeIncubator { get; set; }

        /// <summary>Tappers finish instantly.</summary>
        public bool FastTapper { get; set; }

        /// <summary>Wood Chippers finish instantly.</summary>
        public bool FastWoodChipper { get; set; }

        /// <summary>Worm bins finish instantly.</summary>
        public bool FastWormBin { get; set; }

        /****
        ** Other cheats
        ****/
        /// <summary>The player can always give gifts to villagers, regardless of the daily and weekly limits.</summary>
        public bool AlwaysGiveGift { get; set; }

        /// <summary>A villager's friendship value no longer slowly decays if it isn't maxed out.</summary>
        public bool NoFriendshipDecay { get; set; }

        /// <summary>Fences never break.</summary>
        public bool DurableFences { get; set; }

        /// <summary>Building new structures on the farm completes instantly.</summary>
        public bool InstantBuild { get; set; }

        /// <summary>Feed troughs in your barns and coops are refilled automatically.</summary>
        public bool AutoFeed { get; set; }

        /// <summary>Hay silos are always full.</summary>
        public bool InfiniteHay { get; set; }
    }
}
