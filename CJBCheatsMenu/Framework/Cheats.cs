using System;
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

        private int PreviousTime = -1;
        private Dictionary<string, int> PreviousFriendships;


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="config">The mod settings.</param>
        public Cheats(ModConfig config)
        {
            this.Config = config;
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
                if (!location.isFarm && !location.name.Contains("Greenhouse"))
                    continue;

                foreach (KeyValuePair<Vector2, TerrainFeature> pair in location.terrainFeatures)
                {
                    if (pair.Value is HoeDirt dirt)
                        dirt.state = 1;
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
                    if (!tree.stump)
                        tree.growthStage = 5;
                }
                else if (terrainFeature is FruitTree fruitTree)
                {
                    if (!fruitTree.stump)
                    {
                        fruitTree.growthStage = 5;
                        fruitTree.daysUntilMature = 0;
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
                if (player.currentLocation.terrainFeatures.ContainsKey(tile))
                {
                    if (player.currentLocation.terrainFeatures[tile] is HoeDirt dirt)
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
                    if (!location.isFarm && !location.name.Contains("Greenhouse"))
                        continue;

                    foreach (KeyValuePair<Vector2, TerrainFeature> pair in location.terrainFeatures)
                    {
                        if (pair.Value is HoeDirt dirt && dirt.crop != null && cropHarvestMethods.TryGetValue(dirt.crop.indexOfHarvest, out int harvestMethod))
                            dirt.crop.harvestMethod = harvestMethod;
                    }
                }
            }
        }

        public void OnTimeOfDayChanged()
        {
            GameLocation location = Game1.currentLocation;
            if (this.PreviousTime == -1 || Game1.timeOfDay == 600)
                this.PreviousTime = Game1.timeOfDay;
            else
            {
                bool inCave = location is MineShaft || location is FarmCave;
                bool frozen = (this.Config.FreezeTimeInside && !location.IsOutdoors && !inCave) || (this.Config.FreezeTimeCaves && inCave);
                frozen = frozen || this.Config.FreezeTime;

                if (frozen)
                    Game1.timeOfDay = this.PreviousTime;
                else
                    this.PreviousTime = Game1.timeOfDay;
            }
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
                if (this.PreviousFriendships == null)
                    this.PreviousFriendships = Game1.player.friendships.ToDictionary(p => p.Key.ToString(), p => p.Value[0]);
                foreach (KeyValuePair<string, int[]> n in Game1.player.friendships)
                {
                    foreach (KeyValuePair<string, int> o in this.PreviousFriendships)
                    {
                        if (n.Key == o.Key && n.Value[0] < o.Value)
                            n.Value[0] = o.Value;
                    }
                }
                this.PreviousFriendships = Game1.player.friendships.ToDictionary(p => p.Key.ToString(), p => p.Value[0]);
            }

            // apply location changes
            foreach (GameLocation location in locations)
            {
                // instant buildings
                if (this.Config.InstantBuild && location is BuildableGameLocation buildableLocation)
                {
                    foreach (Building building in buildableLocation.buildings)
                    {
                        if (building.daysOfConstructionLeft > 0 || building.daysUntilUpgrade > 0)
                            building.dayUpdate(1);
                    }
                }

                // durable fences + fast processing
                foreach (KeyValuePair<Vector2, SObject> pair in location.objects)
                {
                    if (pair.Value == null)
                        continue;

                    // durable fences
                    if (this.Config.DurableFences && pair.Value is Fence fence)
                        fence.repair();

                    // fast machines
                    else if (this.Config.FastCask && pair.Value is Cask cask && cask.heldObject != null)
                    {
                        cask.daysToMature = 0;
                        cask.minutesUntilReady = 0;
                        cask.heldObject.quality = 4;
                    }
                    else if (this.Config.FastFurnace && pair.Value.name == "Furnace")
                        pair.Value.minutesUntilReady = 0;
                    else if (this.Config.FastRecyclingMachine && pair.Value.name == "Recycling Machine")
                        pair.Value.minutesUntilReady = 0;
                    else if (this.Config.FastCrystalarium && pair.Value.name == "Crystalarium")
                        pair.Value.minutesUntilReady = 0;
                    else if (this.Config.FastIncubator && pair.Value.name == "Incubator")
                        pair.Value.minutesUntilReady = 0;
                    else if (this.Config.FastSlimeIncubator && pair.Value.name == "Slime Incubator")
                        pair.Value.minutesUntilReady = 0;
                    else if (this.Config.FastKeg && pair.Value.name == "Keg")
                        pair.Value.minutesUntilReady = 0;
                    else if (this.Config.FastPreservesJar && pair.Value.name == "Preserves Jar")
                        pair.Value.minutesUntilReady = 0;
                    else if (this.Config.FastCheesePress && pair.Value.name == "Cheese Press")
                        pair.Value.minutesUntilReady = 0;
                    else if (this.Config.FastMayonnaiseMachine && pair.Value.name == "Mayonnaise Machine")
                        pair.Value.minutesUntilReady = 0;
                    else if (this.Config.FastLoom && pair.Value.name == "Loom")
                        pair.Value.minutesUntilReady = 0;
                    else if (this.Config.FastOilMaker && pair.Value.name == "Oil Maker")
                        pair.Value.minutesUntilReady = 0;
                    else if (this.Config.FastSeedMaker && pair.Value.name == "Seed Maker")
                        pair.Value.minutesUntilReady = 0;
                    else if (this.Config.FastCharcoalKiln && pair.Value.name == "Charcoal Kiln")
                        pair.Value.minutesUntilReady = 0;
                    else if (this.Config.FastSlimeEggPress && pair.Value.name == "Slime Egg-Press")
                        pair.Value.minutesUntilReady = 0;
                    else if (this.Config.FastBeeHouse && pair.Value.name == "Bee House")
                        pair.Value.minutesUntilReady = 0;
                    else if (this.Config.FastMushroomBox && pair.Value.name == "Mushroom Box")
                        pair.Value.minutesUntilReady = 0;
                    else if (this.Config.FastTapper && pair.Value.name == "Tapper")
                        pair.Value.minutesUntilReady = 0;
                    else if (this.Config.FastLightningRod && pair.Value.name == "Lightning Rod")
                        pair.Value.minutesUntilReady = 0;
                    else if (this.Config.FastWormBin && pair.Value.name == "Worm Bin")
                        pair.Value.minutesUntilReady = 0;
                }

                // fast fruit trees
                if (this.Config.FastFruitTree)
                {
                    foreach (KeyValuePair<Vector2, TerrainFeature> pair in location.terrainFeatures)
                    {
                        if (pair.Value is FruitTree tree && tree.growthStage >= FruitTree.treeStage && tree.fruitsOnTree < FruitTree.maxFruitsOnTrees)
                            tree.fruitsOnTree = FruitTree.maxFruitsOnTrees;
                    }
                }
            }

            // autofeed animals
            if (this.Config.AutoFeed)
            {
                Farm farm = Game1.getFarm();
                foreach (GameLocation location in locations)
                {
                    if (!location.isFarm && !location.name.Contains("Greenhouse"))
                        continue;
                    if (location is BuildableGameLocation buildableLocation)
                    {
                        foreach (Building building in buildableLocation.buildings)
                        {
                            if (building.indoors is AnimalHouse indoors)
                            {
                                int animalcount = indoors.animals.Count;
                                building.currentOccupants = animalcount;
                                int hayobjects = indoors.numberOfObjectsWithName("Hay");
                                int hayUsed = Math.Min(animalcount - hayobjects, farm.piecesOfHay);
                                farm.piecesOfHay -= hayUsed;

                                int tileX = 6;
                                if (indoors.name.Contains("Barn"))
                                    tileX = 8;

                                for (int i = 0; i < indoors.animalLimit; i++)
                                {
                                    if (hayUsed <= 0)
                                        break;

                                    Vector2 tile = new Vector2(tileX + i, 3f);
                                    if (!indoors.objects.ContainsKey(tile))
                                        indoors.objects.Add(tile, new SObject(178, 1));
                                    hayUsed--;
                                }
                            }
                        }
                    }
                }
            }

            // harvest with sickle
            if (this.Config.HarvestSickle)
            {
                foreach (GameLocation location in locations)
                {
                    if (!location.isFarm && !location.name.Contains("Greenhouse"))
                        continue;

                    foreach (KeyValuePair<Vector2, TerrainFeature> pair in location.terrainFeatures)
                    {
                        if (pair.Value is HoeDirt dirt)
                        {
                            if (dirt.crop != null)
                                dirt.crop.harvestMethod = 1;
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
                    player.stamina = player.maxStamina;

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
                        rod.attachments[1].scale.Y = 1;
                }

                if (this.Config.OneHitBreak && player.usingTool && (player.CurrentTool is Axe || player.CurrentTool is Pickaxe))
                {
                    Vector2 tile = new Vector2((int)player.GetToolLocation().X / Game1.tileSize, (int)player.GetToolLocation().Y / Game1.tileSize);

                    if (player.CurrentTool is Pickaxe && player.currentLocation.objects.ContainsKey(tile))
                    {
                        SObject obj = player.currentLocation.Objects[tile];
                        if (obj != null && obj.name == "Stone")
                            obj.minutesUntilReady = 0;
                    }

                    if (player.CurrentTool is Axe && player.currentLocation.terrainFeatures.ContainsKey(tile))
                    {
                        TerrainFeature obj = player.currentLocation.terrainFeatures[tile];
                        if (obj is Tree tree && tree.health > 1)
                            tree.health = 1;
                        else if (obj is FruitTree fruitTree && fruitTree.health > 1)
                            fruitTree.health = 1;
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
                        if (r.getBoundingBox(r.tile).Contains((int)player.GetToolLocation().X, (int)player.GetToolLocation().Y) && r.health > 0)
                            r.health = 0;
                    }
                }

                if (this.Config.InfiniteWateringCan && player.CurrentTool is WateringCan can)
                    helper.Reflection.GetField<int>(can, "waterLeft").SetValue(can.waterCanMax);

                if (this.Config.AlwaysGiveGift)
                {
                    foreach (KeyValuePair<string, int[]> friendship in player.friendships)
                    {
                        if (friendship.Value != null)
                        {
                            friendship.Value[1] = 0;
                            friendship.Value[3] = 0;
                        }
                    }
                }
            }

            if (this.Config.MaxDailyLuck) Game1.dailyLuck = 0.115d;

            if (this.Config.OneHitKill && Game1.currentLocation != null)
            {
                if (Game1.currentLocation.characters != null)
                {
                    foreach (Monster monster in Game1.currentLocation.characters.OfType<Monster>())
                        monster.health = 1;
                }
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
                    farm.piecesOfHay = Utility.numSilos() * 240;
            }
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
    }
}
