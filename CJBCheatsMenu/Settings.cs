using Microsoft.Xna.Framework.Input;

namespace CJBCheatsMenu
{
    public class Settings
    {
        /*********
        ** Accessors
        *********/
        public string OpenMenuKey { get; set; } = Keys.P.ToString();
        public string FreezeTimeKey { get; set; } = Keys.T.ToString();
        public string GrowTreeKey { get; set; } = Keys.NumPad1.ToString();
        public string GrowCropsKey { get; set; } = Keys.NumPad2.ToString();

        public bool IncreasedMovement { get; set; }
        public bool InfiniteHealth { get; set; }
        public bool InfiniteStamina { get; set; }
        public bool InstantCatch { get; set; }
        public bool OneHitKill { get; set; }
        public bool InfiniteWateringCan { get; set; }
        public bool ThrowBobberMax { get; set; }
        public bool MaxDailyLuck { get; set; }
        public bool AlwaysGiveGift { get; set; }
        public bool NoFriendshipDecay { get; set; }
        public bool FreezeTime { get; set; }
        public bool FreezeTimeInside { get; set; }
        public bool FreezeTimeCaves { get; set; }
        public bool AlwaysTreasure { get; set; }
        public bool DurableFences { get; set; }
        public bool OneHitBreak { get; set; }
        public bool InstantBite { get; set; }
        public bool InstantBuild { get; set; }
        public bool AutoFeed { get; set; }
        public bool InfiniteHay { get; set; }
        public bool DurableTackles { get; set; }
        public bool HarvestSickle { get; set; }

        public bool FastCask { get; set; }
        public bool FastFurnace { get; set; }
        public bool FastRecyclingMachine { get; set; }
        public bool FastCrystalarium { get; set; }
        public bool FastIncubator { get; set; }
        public bool FastSlimeIncubator { get; set; }
        public bool FastKeg { get; set; }
        public bool FastPreservesJar { get; set; }
        public bool FastCheesePress { get; set; }
        public bool FastMayonnaiseMachine { get; set; }
        public bool FastLoom { get; set; }
        public bool FastOilMaker { get; set; }
        public bool FastSeedMaker { get; set; }
        public bool FastCharcoalKiln { get; set; }
        public bool FastSlimeEggPress { get; set; }
        public bool FastBeeHouse { get; set; }
        public bool FastMushroomBox { get; set; }
        public bool FastTapper { get; set; }
        public bool FastLightningRod { get; set; }
        public bool FastWormBin { get; set; }

        public int MoveSpeed { get; set; } = 1;
    }
}
