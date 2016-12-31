using Microsoft.Xna.Framework.Input;

namespace CJBCheatsMenu {
    public class Settings {
        
        public string openMenuKey { get; set; } = Keys.P.ToString();
        public string freezeTimeKey { get; set; } = Keys.T.ToString();
        public string growTreeKey { get; set; } = Keys.NumPad1.ToString();
        public string growCropsKey { get; set; } = Keys.NumPad2.ToString();

        public bool increasedMovement { get; set; }
        public bool infiniteHealth { get; set; }
        public bool infiniteStamina { get; set; }
        public bool instantCatch { get; set; }
        public bool oneHitKill { get; set; }
        public bool infiniteWateringCan { get; set; }
        public bool throwBobberMax { get; set; }
        public bool maxDailyLuck { get; set; }
        public bool alwaysGiveGift { get; set; }
        public bool noFriendshipDecay { get; set; }
        public bool freezeTime { get; set; }
        public bool freezeTimeInside { get; set; }
        public bool freezeTimeCaves { get; set; }
        public bool alwaysTreasure { get; set; }
        public bool durableFences { get; set; }
        public bool oneHitBreak { get; set; }
        public bool instantBite { get; set; }
        public bool instantBuild { get; set; }
        public bool autoFeed { get; set; }
        public bool infiniteHay { get; set; }
        public bool durableTackles { get; set; }
        public bool harvestSickle { get; set; }

        public bool fastFurnace { get; set; }
        public bool fastRecyclingMachine { get; set; }
        public bool fastCrystalarium { get; set; }
        public bool fastIncubator { get; set; }
        public bool fastSlimeIncubator  { get; set; }
        public bool fastKeg { get; set; }
        public bool fastPreservesJar { get; set; }
        public bool fastCheesePress { get; set; }
        public bool fastMayonnaiseMachine  { get; set; }
        public bool fastLoom { get; set; }
        public bool fastOilMaker { get; set; }
        public bool fastSeedMaker { get; set; }
        public bool fastCharcoalKiln { get; set; }
        public bool fastSlimeEggPress { get; set; }
        public bool fastBeeHouse { get; set; }
        public bool fastMushroomBox { get; set; }
        public bool fastTapper { get; set; }
        public bool fastLightningRod { get; set; }

        public int moveSpeed { get; set; } = 1;
    }
}
