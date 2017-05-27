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

namespace CJBCheatsMenu
{
    internal class Cheats
    {
        /*********
        ** Properties
        *********/
        private static int PreviousTime = -1;
        private static Dictionary<string, int> PreviousFriendships;


        /*********
        ** Public methods
        *********/
        public static void SetWeatherForNextDay(int weatherID)
        {
            Game1.weatherForTomorrow = weatherID;
            Game1.soundBank.PlayCue("thunder");
        }

        public static void WaterAllFields(GameLocation[] locations)
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

        public static void GrowTree()
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

        public static void GrowCrops()
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
        public static void OnOptionsChanged()
        {
            // disable harvest with sickle
            if (!CJBCheatsMenu.Config.HarvestSickle)
            {
                IDictionary<int, int> cropHarvestMethods = Cheats.GetCropHarvestMethods();
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

        public static void OnTimeOfDayChanged()
        {
            GameLocation location = Game1.currentLocation;
            if (Cheats.PreviousTime == -1 || Game1.timeOfDay == 600)
                Cheats.PreviousTime = Game1.timeOfDay;
            else
            {
                bool inCave = location is MineShaft || location is FarmCave;
                bool frozen = (CJBCheatsMenu.Config.FreezeTimeInside && !location.IsOutdoors && !inCave) || (CJBCheatsMenu.Config.FreezeTimeCaves && inCave);
                frozen = frozen || CJBCheatsMenu.Config.FreezeTime;

                if (frozen)
                    Game1.timeOfDay = Cheats.PreviousTime;
                else
                    Cheats.PreviousTime = Game1.timeOfDay;
            }
        }

        public static void OnDrawTick()
        {
            GameLocation location = Game1.currentLocation;

            bool inCave = location is MineShaft || location is FarmCave;
            bool frozen = (CJBCheatsMenu.Config.FreezeTimeInside && !location.IsOutdoors && !inCave) || (CJBCheatsMenu.Config.FreezeTimeCaves && inCave);
            frozen = frozen || CJBCheatsMenu.Config.FreezeTime;
            if (frozen)
                CJB.DrawTextBox(5, inCave ? 100 : 5, Game1.smallFont, "Time Frozen");
        }

        public static void OneSecondUpdate(GameLocation[] locations)
        {
            // disable friendship decay
            if (CJBCheatsMenu.Config.NoFriendshipDecay)
            {
                if (Cheats.PreviousFriendships == null)
                    Cheats.PreviousFriendships = Game1.player.friendships.ToDictionary(p => p.Key.ToString(), p => p.Value[0]);
                foreach (KeyValuePair<string, int[]> n in Game1.player.friendships)
                {
                    foreach (KeyValuePair<string, int> o in Cheats.PreviousFriendships)
                    {
                        if (n.Key == o.Key && n.Value[0] < o.Value)
                            n.Value[0] = o.Value;
                    }
                }
                Cheats.PreviousFriendships = Game1.player.friendships.ToDictionary(p => p.Key.ToString(), p => p.Value[0]);
            }

            // instant buildings
            if (CJBCheatsMenu.Config.InstantBuild)
            {
                foreach (BuildableGameLocation location in locations.OfType<BuildableGameLocation>())
                {
                    foreach (Building building in location.buildings)
                    {
                        if (building.daysOfConstructionLeft > 0 || building.daysUntilUpgrade > 0)
                            building.dayUpdate(1);
                    }
                }
            }

            // fast machines
            foreach (GameLocation location in locations)
            {
                foreach (KeyValuePair<Vector2, SObject> pair in location.objects)
                {
                    if (pair.Value == null)
                        continue;

                    if (CJBCheatsMenu.Config.DurableFences && pair.Value is Fence fence)
                        fence.repair();
                    else if (CJBCheatsMenu.Config.FastCask && pair.Value is Cask cask && cask.heldObject != null)
                    {
                        cask.daysToMature = 0;
                        cask.minutesUntilReady = 0;
                        cask.heldObject.quality = 4;
                    }
                    else if (CJBCheatsMenu.Config.FastFurnace && pair.Value.name == "Furnace")
                        pair.Value.minutesUntilReady = 0;
                    else if (CJBCheatsMenu.Config.FastRecyclingMachine && pair.Value.name == "Recycling Machine")
                        pair.Value.minutesUntilReady = 0;
                    else if (CJBCheatsMenu.Config.FastCrystalarium && pair.Value.name == "Crystalarium")
                        pair.Value.minutesUntilReady = 0;
                    else if (CJBCheatsMenu.Config.FastIncubator && pair.Value.name == "Incubator")
                        pair.Value.minutesUntilReady = 0;
                    else if (CJBCheatsMenu.Config.FastSlimeIncubator && pair.Value.name == "Slime Incubator")
                        pair.Value.minutesUntilReady = 0;
                    else if (CJBCheatsMenu.Config.FastKeg && pair.Value.name == "Keg")
                        pair.Value.minutesUntilReady = 0;
                    else if (CJBCheatsMenu.Config.FastPreservesJar && pair.Value.name == "Preserves Jar")
                        pair.Value.minutesUntilReady = 0;
                    else if (CJBCheatsMenu.Config.FastCheesePress && pair.Value.name == "Cheese Press")
                        pair.Value.minutesUntilReady = 0;
                    else if (CJBCheatsMenu.Config.FastMayonnaiseMachine && pair.Value.name == "Mayonnaise Machine")
                        pair.Value.minutesUntilReady = 0;
                    else if (CJBCheatsMenu.Config.FastLoom && pair.Value.name == "Loom")
                        pair.Value.minutesUntilReady = 0;
                    else if (CJBCheatsMenu.Config.FastOilMaker && pair.Value.name == "Oil Maker")
                        pair.Value.minutesUntilReady = 0;
                    else if (CJBCheatsMenu.Config.FastSeedMaker && pair.Value.name == "Seed Maker")
                        pair.Value.minutesUntilReady = 0;
                    else if (CJBCheatsMenu.Config.FastCharcoalKiln && pair.Value.name == "Charcoal Kiln")
                        pair.Value.minutesUntilReady = 0;
                    else if (CJBCheatsMenu.Config.FastSlimeEggPress && pair.Value.name == "Slime Egg-Press")
                        pair.Value.minutesUntilReady = 0;
                    else if (CJBCheatsMenu.Config.FastBeeHouse && pair.Value.name == "Bee House")
                        pair.Value.minutesUntilReady = 0;
                    else if (CJBCheatsMenu.Config.FastMushroomBox && pair.Value.name == "Mushroom Box")
                        pair.Value.minutesUntilReady = 0;
                    else if (CJBCheatsMenu.Config.FastTapper && pair.Value.name == "Tapper")
                        pair.Value.minutesUntilReady = 0;
                    else if (CJBCheatsMenu.Config.FastLightningRod && pair.Value.name == "Lightning Rod")
                        pair.Value.minutesUntilReady = 0;
                    else if (CJBCheatsMenu.Config.FastWormBin && pair.Value.name == "Worm Bin")
                        pair.Value.minutesUntilReady = 0;
                }
            }

            // autofeed animals
            if (CJBCheatsMenu.Config.AutoFeed)
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
            if (CJBCheatsMenu.Config.HarvestSickle)
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

        public static void OnUpdate(IModHelper helper)
        {
            if (Game1.player != null)
            {
                SFarmer player = Game1.player;

                if (CJBCheatsMenu.Config.IncreasedMovement && player.running)
                    player.addedSpeed = CJBCheatsMenu.Config.MoveSpeed;
                else if (!CJBCheatsMenu.Config.IncreasedMovement && player.addedSpeed == CJBCheatsMenu.Config.MoveSpeed)
                    player.addedSpeed = 0;

                if (player.controller != null)
                    player.addedSpeed = 0;

                if (Game1.CurrentEvent == null)
                    player.movementDirections.Clear();

                if (CJBCheatsMenu.Config.InfiniteHealth)
                    player.health = player.maxHealth;

                if (CJBCheatsMenu.Config.InfiniteStamina)
                    player.stamina = player.maxStamina;

                if (Game1.activeClickableMenu == null && player.CurrentTool is FishingRod rod)
                {
                    if (CJBCheatsMenu.Config.ThrowBobberMax)
                        rod.castingPower = 1.01F;
                    if (CJBCheatsMenu.Config.InstantBite && rod.isFishing)
                    {
                        if (rod.timeUntilFishingBite > 0)
                            rod.timeUntilFishingBite = 0;
                    }
                    if (CJBCheatsMenu.Config.DurableTackles && rod.attachments[1] != null)
                        rod.attachments[1].scale.Y = 1;
                }

                if (CJBCheatsMenu.Config.OneHitBreak && player.usingTool && (player.CurrentTool is Axe || player.CurrentTool is Pickaxe))
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

                if (CJBCheatsMenu.Config.InfiniteWateringCan && player.CurrentTool is WateringCan can)
                    helper.Reflection.GetPrivateField<int>(can, "waterLeft").SetValue(can.waterCanMax);

                if (CJBCheatsMenu.Config.AlwaysGiveGift)
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

            if (CJBCheatsMenu.Config.MaxDailyLuck) Game1.dailyLuck = 0.115d;

            if (CJBCheatsMenu.Config.OneHitKill && Game1.currentLocation != null)
            {
                if (Game1.currentLocation.characters != null)
                {
                    foreach (Monster monster in Game1.currentLocation.characters.OfType<Monster>())
                        monster.health = 1;
                }
            }

            if ((CJBCheatsMenu.Config.InstantCatch || CJBCheatsMenu.Config.AlwaysTreasure) && Game1.activeClickableMenu is BobberBar bobberMenu)
            {
                if (CJBCheatsMenu.Config.AlwaysTreasure)
                    helper.Reflection.GetPrivateField<bool>(bobberMenu, "treasure").SetValue(true);

                if (CJBCheatsMenu.Config.InstantCatch)
                    helper.Reflection.GetPrivateField<float>(bobberMenu, "distanceFromCatching").SetValue(1);

                if (helper.Reflection.GetPrivateValue<bool>(bobberMenu, "treasure"))
                    helper.Reflection.GetPrivateField<bool>(bobberMenu, "treasureCaught").SetValue(true);
            }

            if (CJBCheatsMenu.Config.InfiniteHay)
            {
                Farm farm = Game1.getFarm();
                if (farm != null)
                    farm.piecesOfHay = Utility.numSilos() * 240;
            }
        }

        public static void OnKeyPress(Keys key)
        {
            if (key.ToString() == CJBCheatsMenu.Config.OpenMenuKey)
            {
                CheatsMenu.Open(CJBCheatsMenu.Config.DefaultTab);
            }

            else if (key.ToString() == CJBCheatsMenu.Config.FreezeTimeKey)
            {
                if (Game1.hasLoadedGame && Game1.activeClickableMenu == null)
                    CJBCheatsMenu.Config.FreezeTime = !CJBCheatsMenu.Config.FreezeTime;
            }

            else if (key.ToString() == CJBCheatsMenu.Config.GrowTreeKey)
            {
                if (Game1.hasLoadedGame && Game1.activeClickableMenu == null)
                    Cheats.GrowTree();
            }

            else if (key.ToString() == CJBCheatsMenu.Config.GrowCropsKey)
            {
                if (Game1.hasLoadedGame && Game1.activeClickableMenu == null)
                    Cheats.GrowCrops();
            }
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Get a crop ID => harvest method lookup.</summary>
        private static IDictionary<int, int> GetCropHarvestMethods()
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
