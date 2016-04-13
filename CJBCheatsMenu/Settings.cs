using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CJBCheatsMenu {
    public class Settings : Config {

        public string openMenuKey { get; set; }
        public string freezeTimeKey { get; set; }
        public string growTreeKey { get; set; }
        public string growCropsKey { get; set; }

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

        public int moveSpeed { get; set; }

        public override T GenerateDefaultConfig<T>() {

            openMenuKey = "P";
            freezeTimeKey = "T";
            growTreeKey = "NumPad1";
            growCropsKey = "NumPad2";

            increasedMovement = false;
            infiniteHealth = false;
            infiniteStamina = false;
            instantCatch = false;
            oneHitKill = false;
            infiniteWateringCan = false;
            throwBobberMax = false;
            maxDailyLuck = false;
            alwaysGiveGift = false;
            noFriendshipDecay = false;
            freezeTime = false;
            freezeTimeInside = false;
            freezeTimeCaves = false;
            alwaysTreasure = false;
            durableFences = false;
            oneHitBreak = false;
            instantBite = false;
            instantBuild = false;
            autoFeed = false;
            infiniteHay = false;
            durableTackles = false;
            harvestSickle = false;

            fastFurnace = false;
            fastRecyclingMachine = false;
            fastCrystalarium = false;
            fastIncubator = false;
            fastSlimeIncubator = false;
            fastKeg = false;
            fastPreservesJar = false;
            fastCheesePress = false;
            fastMayonnaiseMachine = false;
            fastLoom = false;
            fastOilMaker = false;
            fastSeedMaker = false;
            fastCharcoalKiln = false;
            fastSlimeEggPress = false;
            fastBeeHouse = false;
            fastMushroomBox = false;
            fastTapper = false;
            fastLightningRod = false;

            moveSpeed = 1;

            return this as T;
        }

    }
}
