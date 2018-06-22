﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Monsters;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using SFarmer = StardewValley.Farmer;
using SObject = StardewValley.Object;

namespace CJBCheatsMenu.Framework
{
    internal class Cheats
    {
        /*********
        ** Properties
        *********/
        /// <summary>The mod settings.</summary>
        private readonly ModConfig Config;
        /// <summary>The minimum friendship points to maintain for each NPC.</summary>
        private readonly Dictionary<string, int> PreviousFriendships = new Dictionary<string, int>();


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="config">The mod settings.</param>
        public Cheats(ModConfig config)
        {
            this.Config = config;
        }

        /// <summary>Reset all tracked data.</summary>
        public void Reset()
        {
            this.PreviousFriendships.Clear();
        }

        /// <summary>Update the tracked friendship points for an NPC.</summary>
        /// <param name="npc">The NPC whose friendship to update.</param>
        /// <param name="points">The new friendship points value.</param>
        public void UpdateFriendship(NPC npc, int points)
        {
            this.PreviousFriendships[npc.Name] = points;
        }

        public void SetWeatherForNextDay(int weatherID)
        {
            Game1.weatherForTomorrow = weatherID;
            Game1.soundBank.PlayCue("thunder");
        }

        public void WaterAllFields(GameLocation[] locations)
        {
            foreach (GameLocation location in locations)
            {
                if (!location.IsFarm && !location.Name.Contains("Greenhouse"))
                    continue;

                foreach (TerrainFeature terrainFeature in location.terrainFeatures.Values)
                {
                    if (terrainFeature is HoeDirt dirt)
                        dirt.state.Value = HoeDirt.watered;
                }

                foreach (IndoorPot pot in location.objects.Values.OfType<IndoorPot>())
                {
                    if (pot.hoeDirt.Value is HoeDirt dirt)
                    {
                        dirt.state.Value = HoeDirt.watered;
                        pot.showNextIndex.Value = true;
                    }
                }
            }
        }

        public void GrowTree()
        {
            SFarmer player = Game1.player;
            int x = (int)player.GetToolLocation().X / Game1.tileSize;
            int y = (int)player.GetToolLocation().Y / Game1.tileSize;
            Vector2 index = new Vector2(x, y);

            if (player.currentLocation.terrainFeatures.ContainsKey(index))
            {
                TerrainFeature terrainFeature = player.currentLocation.terrainFeatures[index];
                if (terrainFeature is Tree tree)
                {
                    if (!tree.stump.Value)
                        tree.growthStage.Value = Tree.treeStage;
                }
                else if (terrainFeature is FruitTree fruitTree)
                {
                    if (!fruitTree.stump.Value)
                    {
                        fruitTree.growthStage.Value = FruitTree.treeStage;
                        fruitTree.daysUntilMature.Value = 0;
                    }
                }
            }
        }

        public void GrowCrops()
        {
            SFarmer player = Game1.player;
            List<Vector2> tiles = new List<Vector2>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                    tiles.Add(new Vector2(player.getTileX() + x, player.getTileY() + y));
            }

            foreach (Vector2 tile in tiles)
            {
                if (player.currentLocation.terrainFeatures.TryGetValue(tile, out TerrainFeature terrainFeature))
                {
                    if (terrainFeature is HoeDirt dirt)
                        dirt.crop?.growCompletely();
                }
                else if (player.currentLocation.objects.TryGetValue(tile, out SObject obj))
                {
                    if (obj is IndoorPot pot && pot.hoeDirt.Value is HoeDirt dirt)
                        dirt.crop?.growCompletely();
                }
            }
        }

        /// <summary>Perform any action needed after the cheat options change.</summary>
        public void OnOptionsChanged()
        {
            // disable harvest with sickle
            if (!this.Config.HarvestSickle)
            {
                IDictionary<int, int> cropHarvestMethods = this.GetCropHarvestMethods();
                foreach (GameLocation location in Game1.locations)
                {
                    if (!location.IsFarm && !location.Name.Contains("Greenhouse"))
                        continue;
                    foreach (TerrainFeature terrainFeature in location.terrainFeatures.Values)
                    {
                        if (terrainFeature is HoeDirt dirt && dirt.crop != null && cropHarvestMethods.TryGetValue(dirt.crop.indexOfHarvest.Value, out int harvestMethod))
                            dirt.crop.harvestMethod.Value = harvestMethod;
                    }
                }
            }
        }

        public void OnTimeOfDayChanged()
        {
        //    GameLocation location = Game1.currentLocation;
        //    if (this.PreviousTime == -1 || Game1.timeOfDay == 600)
        //        this.PreviousTime = Game1.timeOfDay;
        //    else
        //    {
        //        bool inCave = location is MineShaft || location is FarmCave;
        //        bool frozen = (this.Config.FreezeTimeInside && !location.IsOutdoors && !inCave) || (this.Config.FreezeTimeCaves && inCave);
        //        frozen = frozen || this.Config.FreezeTime;
        //
        //        if (frozen)
        //            Game1.timeOfDay = this.PreviousTime;
        //        else
        //            this.PreviousTime = Game1.timeOfDay;
        //    }
        }

        public void OnDrawTick(ITranslationHelper i18n)
        {
            GameLocation location = Game1.currentLocation;

            bool inCave = location is MineShaft || location is FarmCave;
            bool frozen = (this.Config.FreezeTimeInside && !location.IsOutdoors && !inCave) || (this.Config.FreezeTimeCaves && inCave);
            frozen = frozen || this.Config.FreezeTime;
            if (frozen)
                CJB.DrawTextBox(5, inCave ? 100 : 5, Game1.smallFont, i18n.Get("messages.time-frozen"));
        }

        public void OneSecondUpdate(GameLocation[] locations)
        {
            // disable friendship decay
            if (this.Config.NoFriendshipDecay)
            {
                if (this.PreviousFriendships.Any())
                {
                    foreach (string key in Game1.player.friendshipData.Keys)
                    {
                        Friendship friendship = Game1.player.friendshipData[key];
                        if (this.PreviousFriendships.TryGetValue(key, out int oldPoints) && oldPoints > friendship.Points)
                            friendship.Points = oldPoints;
                    }
                }

                this.PreviousFriendships.Clear();
                foreach (var pair in Game1.player.friendshipData.FieldDict)
                    this.PreviousFriendships[pair.Key] = pair.Value.Value.Points;
            }

            // apply location changes
            Farm farm = Game1.getFarm();
            foreach (GameLocation location in CJB.GetAllLocations())
            {
                // instant buildings
                if (this.Config.InstantBuild && location is BuildableGameLocation buildableLocation)
                {
                    foreach (Building building in buildableLocation.buildings)
                    {
                        if (building.daysOfConstructionLeft.Value > 0 || building.daysUntilUpgrade.Value > 0)
                            building.dayUpdate(1);
                    }
                }

                // durable fences + fast processing
                foreach (SObject obj in location.objects.Values)
                {
                    if (obj == null)
                        continue;

                    // durable fences
                    if (this.Config.DurableFences && obj is Fence fence)
                        fence.repair();

                    // fast machines
                    else if (this.Config.FastCask && obj is Cask cask && cask.heldObject.Value != null)
                    {
                        cask.daysToMature.Value = 0;
                        cask.MinutesUntilReady = 0;
                        cask.heldObject.Value.Quality = 4;
                    }
                    else if (this.Config.FastFurnace && obj.name == "Furnace")
                        obj.MinutesUntilReady = 0;
                    else if (this.Config.FastRecyclingMachine && obj.name == "Recycling Machine")
                        obj.MinutesUntilReady = 0;
                    else if (this.Config.FastCrystalarium && obj.name == "Crystalarium")
                        obj.MinutesUntilReady = 0;
                    else if (this.Config.FastIncubator && obj.name == "Incubator")
                        obj.MinutesUntilReady = 0;
                    else if (this.Config.FastSlimeIncubator && obj.name == "Slime Incubator")
                        obj.MinutesUntilReady = 0;
                    else if (this.Config.FastKeg && obj.name == "Keg")
                        obj.MinutesUntilReady = 0;
                    else if (this.Config.FastPreservesJar && obj.name == "Preserves Jar")
                        obj.MinutesUntilReady = 0;
                    else if (this.Config.FastCheesePress && obj.name == "Cheese Press")
                        obj.MinutesUntilReady = 0;
                    else if (this.Config.FastMayonnaiseMachine && obj.name == "Mayonnaise Machine")
                        obj.MinutesUntilReady = 0;
                    else if (this.Config.FastLoom && obj.name == "Loom")
                        obj.MinutesUntilReady = 0;
                    else if (this.Config.FastOilMaker && obj.name == "Oil Maker")
                        obj.MinutesUntilReady = 0;
                    else if (this.Config.FastSeedMaker && obj.name == "Seed Maker")
                        obj.MinutesUntilReady = 0;
                    else if (this.Config.FastCharcoalKiln && obj.name == "Charcoal Kiln")
                        obj.MinutesUntilReady = 0;
                    else if (this.Config.FastSlimeEggPress && obj.name == "Slime Egg-Press")
                        obj.MinutesUntilReady = 0;
                    else if (this.Config.FastBeeHouse && obj.name == "Bee House")
                        obj.MinutesUntilReady = 0;
                    else if (this.Config.FastMushroomBox && obj.name == "Mushroom Box")
                        obj.MinutesUntilReady = 0;
                    else if (this.Config.FastTapper && obj.name == "Tapper")
                        obj.MinutesUntilReady = 0;
                    else if (this.Config.FastLightningRod && obj.name == "Lightning Rod")
                        obj.MinutesUntilReady = 0;
                    else if (this.Config.FastWormBin && obj.name == "Worm Bin")
                        obj.MinutesUntilReady = 0;
                }

                // fast fruit trees
                if (this.Config.FastFruitTree)
                {
                    foreach (TerrainFeature terrainFeature in location.terrainFeatures.Values)
                    {
                        if (terrainFeature is FruitTree tree && tree.growthStage.Value >= FruitTree.treeStage && tree.fruitsOnTree.Value < FruitTree.maxFruitsOnTrees)
                            tree.fruitsOnTree.Value = FruitTree.maxFruitsOnTrees;
                    }
                }

                // autofeed animals
                if (this.Config.AutoFeed && location is AnimalHouse animalHouse)
                {
                    int animalcount = animalHouse.animals.Values.Count();
                    int hayobjects = animalHouse.numberOfObjectsWithName("Hay");
                    int hayUsed = Math.Min(animalcount - hayobjects, farm.piecesOfHay.Value);
                    farm.piecesOfHay.Value -= hayUsed;

                    int tileX = 6;
                    if (animalHouse.Name.Contains("Barn"))
                        tileX = 8;

                    for (int i = 0; i < animalHouse.animalLimit.Value; i++)
                    {
                        if (hayUsed <= 0)
                            break;

                        Vector2 tile = new Vector2(tileX + i, 3f);
                        if (!animalHouse.objects.ContainsKey(tile))
                            animalHouse.objects.Add(tile, new SObject(178, 1));
                        hayUsed--;
                    }
                }

                // harvest with sickle
                if (this.Config.HarvestSickle)
                {
                    if (!location.IsFarm && !location.Name.Contains("Greenhouse"))
                        continue;

                    foreach (TerrainFeature terrainFeature in location.terrainFeatures.Values)
                    {
                        if (terrainFeature is HoeDirt dirt)
                        {
                            if (dirt.crop != null)
                                dirt.crop.harvestMethod.Value = 1;
                        }
                    }
                }
            }
        }

        public void OnUpdate(IModHelper helper)
        {
            if (Game1.player != null)
            {
                SFarmer player = Game1.player;

                if (this.Config.IncreasedMovement && player.running)
                    player.addedSpeed = this.Config.MoveSpeed;
                else if (!this.Config.IncreasedMovement && player.addedSpeed == this.Config.MoveSpeed)
                    player.addedSpeed = 0;

                if (player.controller != null)
                    player.addedSpeed = 0;

                if (Game1.CurrentEvent == null)
                    player.movementDirections.Clear();

                if (this.Config.InfiniteHealth)
                    player.health = player.maxHealth;

                if (this.Config.InfiniteStamina)
                    player.stamina = player.MaxStamina;

                if (Game1.activeClickableMenu == null && player.CurrentTool is FishingRod rod)
                {
                    if (this.Config.ThrowBobberMax)
                        rod.castingPower = 1.01F;
                    if (this.Config.InstantBite && rod.isFishing)
                    {
                        if (rod.timeUntilFishingBite > 0)
                            rod.timeUntilFishingBite = 0;
                    }
                    if (this.Config.DurableTackles && rod.attachments[1] != null)
                        rod.attachments[1].uses.Value = 0;
                }

                if (this.Config.OneHitBreak && player.UsingTool && (player.CurrentTool is Axe || player.CurrentTool is Pickaxe))
                {
                    Vector2 tile = new Vector2((int)player.GetToolLocation().X / Game1.tileSize, (int)player.GetToolLocation().Y / Game1.tileSize);

                    if (player.CurrentTool is Pickaxe && player.currentLocation.objects.ContainsKey(tile))
                    {
                        SObject obj = player.currentLocation.Objects[tile];
                        if (obj != null && obj.name == "Stone")
                            obj.MinutesUntilReady = 0;
                    }

                    if (player.CurrentTool is Axe && player.currentLocation.terrainFeatures.ContainsKey(tile))
                    {
                        TerrainFeature obj = player.currentLocation.terrainFeatures[tile];
                        if (obj is Tree tree && tree.health.Value > 1)
                            tree.health.Value = 1;
                        else if (obj is FruitTree fruitTree && fruitTree.health.Value > 1)
                            fruitTree.health.Value = 1;
                    }

                    List<ResourceClump> resourceClumps = new List<ResourceClump>();
                    if (player.currentLocation is MineShaft mineShaft)
                        resourceClumps.AddRange(mineShaft.resourceClumps);

                    if (player.currentLocation is Farm farm)
                        resourceClumps.AddRange(farm.resourceClumps);

                    if (player.currentLocation is Forest forest)
                        resourceClumps.Add(forest.log);

                    if (player.currentLocation is Woods woods)
                        resourceClumps.AddRange(woods.stumps);

                    foreach (ResourceClump r in resourceClumps)
                    {
                        if (r == null)
                            continue;
                        if (r.getBoundingBox(r.tile.Value).Contains((int)player.GetToolLocation().X, (int)player.GetToolLocation().Y) && r.health.Value > 0)
                            r.health.Value = 0;
                    }
                }

                if (this.Config.InfiniteWateringCan && player.CurrentTool is WateringCan can)
                    helper.Reflection.GetField<int>(can, "waterLeft").SetValue(can.waterCanMax);

                if (this.Config.AlwaysGiveGift)
                {
                    foreach (Friendship friendship in player.friendshipData.Values)
                    {
                        friendship.GiftsThisWeek = 0;
                        friendship.GiftsToday = 0;
                    }
                }
            }

            if (this.Config.MaxDailyLuck)
                Game1.dailyLuck = 0.115d;

            if (this.Config.OneHitKill && Game1.currentLocation != null)
            {
                foreach (Monster monster in Game1.currentLocation.characters.OfType<Monster>())
                    monster.Health = 1;
            }

            if ((this.Config.InstantCatch || this.Config.AlwaysTreasure) && Game1.activeClickableMenu is BobberBar bobberMenu)
            {
                if (this.Config.AlwaysTreasure)
                    helper.Reflection.GetField<bool>(bobberMenu, "treasure").SetValue(true);

                if (this.Config.InstantCatch)
                    helper.Reflection.GetField<float>(bobberMenu, "distanceFromCatching").SetValue(1);

                if (helper.Reflection.GetField<bool>(bobberMenu, "treasure").GetValue())
                    helper.Reflection.GetField<bool>(bobberMenu, "treasureCaught").SetValue(true);
            }

            if (this.Config.InfiniteHay)
            {
                Farm farm = Game1.getFarm();
                if (farm != null)
                    farm.piecesOfHay.Value = Utility.numSilos() * 240;
            }

            SetTimeFreezeStatus();
        }

        public void OnKeyPress(Keys key)
        {
            if (!Context.IsPlayerFree)
                return;

            if (key.ToString() == this.Config.FreezeTimeKey)
                this.Config.FreezeTime = !this.Config.FreezeTime;
            else if (key.ToString() == this.Config.GrowTreeKey)
                this.GrowTree();
            else if (key.ToString() == this.Config.GrowCropsKey)
                this.GrowCrops();
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Get a crop ID => harvest method lookup.</summary>
        private IDictionary<int, int> GetCropHarvestMethods()
        {
            IDictionary<int, int> lookup = new Dictionary<int, int>();

            IDictionary<int, string> cropData = Game1.content.Load<Dictionary<int, string>>("Data\\Crops");
            foreach (KeyValuePair<int, string> entry in cropData)
            {
                string[] fields = entry.Value.Split('/');
                int cropID = Convert.ToInt32(fields[3]);
                int harvestMethod = Convert.ToInt32(fields[5]);

                if (!lookup.ContainsKey(cropID))
                    lookup.Add(cropID, harvestMethod);
            }

            return lookup;
        }

        private void SetTimeFreezeStatus()
        {
            GameLocation location = Game1.currentLocation;

            bool frozen = this.Config.FreezeTime;

            if (location != null)
            {
	            bool inCave = location is MineShaft || location is FarmCave;
	            frozen = frozen || (this.Config.FreezeTimeInside && !location.IsOutdoors && !inCave) || (this.Config.FreezeTimeCaves && inCave);
	        }

            if (frozen)
                Game1.gameTimeInterval = 0;
        }
    }
}
