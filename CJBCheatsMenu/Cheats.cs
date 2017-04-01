using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Monsters;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;

namespace CJBCheatsMenu
{
    public class Cheats
    {
        /*********
        ** Properties
        *********/
        public static int PreviousTime = -1;
        public static Dictionary<string, int> PreviousFriendships;


        /*********
        ** Public methods
        *********/
        public static void SetWeatherForNextDay(int weatherID)
        {
            Game1.weatherForTomorrow = weatherID;
            Game1.soundBank.PlayCue("thunder");
        }

        public static void WaterAllFields()
        {
            foreach (GameLocation location in Game1.locations)
            {
                if (!location.isFarm && !location.name.Contains("Greenhouse")) continue;
                foreach (KeyValuePair<Vector2, TerrainFeature> pair in location.terrainFeatures)
                {
                    if (pair.Value == null) continue;
                    if (pair.Value is HoeDirt)
                    {
                        ((HoeDirt)pair.Value).state = 1;
                    }
                }
            }
        }

        public static void GrowTree()
        {
            Farmer player = Game1.player;
            int x = (int)player.GetToolLocation().X / Game1.tileSize;
            int y = (int)player.GetToolLocation().Y / Game1.tileSize;
            Vector2 index = new Vector2(x, y);

            if (player.currentLocation.terrainFeatures.ContainsKey(index))
            {
                TerrainFeature terrainFeature = player.currentLocation.terrainFeatures[index];
                if (terrainFeature is Tree)
                {
                    Tree tree = (Tree)terrainFeature;
                    if (!tree.stump)
                        tree.growthStage = 5;
                }
                if (terrainFeature is FruitTree)
                {
                    FruitTree tree = (FruitTree)terrainFeature;
                    if (!tree.stump)
                    {
                        tree.growthStage = 5;
                        tree.daysUntilMature = 0;
                    }
                }
            }
        }

        public static void GrowCrops()
        {
            Farmer player = Game1.player;
            List<Vector2> tiles = new List<Vector2>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    tiles.Add(new Vector2(player.getTileX() + x, player.getTileY() + y));
                }
            }

            foreach (Vector2 tile in tiles)
            {
                if (player.currentLocation.terrainFeatures.ContainsKey(tile))
                {
                    if (player.currentLocation.terrainFeatures[tile] is HoeDirt)
                    {
                        HoeDirt dirt = (HoeDirt)player.currentLocation.terrainFeatures[tile];
                        dirt.crop?.growCompletely();
                    }
                }
            }
        }

        public static void OnTimeOfDayChanged()
        {
            GameLocation location = Game1.currentLocation;
            if (Cheats.PreviousTime == -1 || Game1.timeOfDay == 600)
            {
                Cheats.PreviousTime = Game1.timeOfDay;
            }
            else
            {
                bool inCave = (location is MineShaft || location is FarmCave);
                bool frozen = (CJBCheatsMenu.Config.FreezeTimeInside && !location.IsOutdoors && !inCave) || (CJBCheatsMenu.Config.FreezeTimeCaves && inCave);
                frozen = (frozen || CJBCheatsMenu.Config.FreezeTime);

                if (frozen)
                {
                    Game1.timeOfDay = Cheats.PreviousTime;
                }
                else
                {
                    Cheats.PreviousTime = Game1.timeOfDay;
                }
            }
        }

        public static void OnDayOfMonthChanged()
        {

        }

        public static void OnDrawTick()
        {
            GameLocation location = Game1.currentLocation;

            bool inCave = (location is MineShaft || location is FarmCave);
            bool frozen = (CJBCheatsMenu.Config.FreezeTimeInside && !location.IsOutdoors && !inCave) || (CJBCheatsMenu.Config.FreezeTimeCaves && inCave);
            frozen = (frozen || CJBCheatsMenu.Config.FreezeTime);
            if (frozen)
            {
                CJB.DrawTextBox(5, inCave ? 100 : 5, Game1.smallFont, "Time Frozen");
            }

            /*
            int xTile = (Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize;
            int yTile = (Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize;

            Vector2 tile = new Vector2(xTile, yTile);

            if (Game1.currentLocation.objects.ContainsKey(tile)) {
                int time = Game1.currentLocation.objects[tile].minutesUntilReady;
                if (time > 0)
                    CJB.DrawTextBox(5, inCave ? 100 : 5, Game1.smallFont, "Ready in: " + time);
            }*/
        }

        public static void OneSecondUpdate()
        {

            if (CJBCheatsMenu.Config.NoFriendshipDecay)
            {
                if (Cheats.PreviousFriendships == null)
                {
                    Cheats.PreviousFriendships = Game1.player.friendships.ToDictionary(p => p.Key.ToString(), p => p.Value[0]);
                }
                foreach (KeyValuePair<string, int[]> n in Game1.player.friendships)
                {
                    foreach (KeyValuePair<string, int> o in Cheats.PreviousFriendships)
                    {
                        if (n.Key.Equals(o.Key) && n.Value[0] < o.Value)
                        {
                            n.Value[0] = o.Value;
                        }
                    }
                }
                Cheats.PreviousFriendships = Game1.player.friendships.ToDictionary(p => p.Key.ToString(), p => p.Value[0]);
            }


            if (CJBCheatsMenu.Config.InstantBuild)
            {
                foreach (GameLocation location in Game1.locations)
                {
                    if (location is BuildableGameLocation)
                    {
                        foreach (Building building in ((BuildableGameLocation)location).buildings)
                        {
                            if (building.daysOfConstructionLeft > 0 || building.daysUntilUpgrade > 0)
                            {
                                building.dayUpdate(1);
                            }
                        }
                    }
                }
            }

            List<GameLocation> locations = new List<GameLocation>();
            foreach (GameLocation location in Game1.locations)
            {
                if (!location.isFarm && !location.name.Contains("Greenhouse"))
                    continue;
                locations.Add(location);
                if (location is BuildableGameLocation)
                {
                    foreach (Building building in ((BuildableGameLocation)location).buildings)
                    {
                        if (building.indoors != null)
                            locations.Add(building.indoors);
                    }
                }
            }

            foreach (GameLocation location in locations)
            {
                foreach (KeyValuePair<Vector2, StardewValley.Object> pair in location.objects)
                {
                    if (pair.Value == null)
                        continue;

                    if (CJBCheatsMenu.Config.DurableFences && pair.Value is Fence)
                    {
                        ((Fence)pair.Value).repair();
                        continue;
                    }
                    if (CJBCheatsMenu.Config.FastCask && pair.Value is Cask)
                    {
                        if (pair.Value.heldObject != null)
                        {
                            pair.Value.minutesUntilReady = 0;
                            pair.Value.heldObject.quality = 4;
                        }
                    }
                    if (CJBCheatsMenu.Config.FastFurnace && pair.Value.name.Equals("Furnace"))
                    {
                        pair.Value.minutesUntilReady = 0;
                        continue;
                    }
                    if (CJBCheatsMenu.Config.FastRecyclingMachine && pair.Value.name.Equals("Recycling Machine"))
                    {
                        pair.Value.minutesUntilReady = 0;
                        continue;
                    }
                    if (CJBCheatsMenu.Config.FastCrystalarium && pair.Value.name.Equals("Crystalarium"))
                    {
                        pair.Value.minutesUntilReady = 0;
                        continue;
                    }
                    if (CJBCheatsMenu.Config.FastIncubator && pair.Value.name.Equals("Incubator"))
                    {
                        pair.Value.minutesUntilReady = 0;
                        continue;
                    }
                    if (CJBCheatsMenu.Config.FastSlimeIncubator && pair.Value.name.Equals("Slime Incubator"))
                    {
                        pair.Value.minutesUntilReady = 0;
                        continue;
                    }
                    if (CJBCheatsMenu.Config.FastKeg && pair.Value.name.Equals("Keg"))
                    {
                        pair.Value.minutesUntilReady = 0;
                        continue;
                    }
                    if (CJBCheatsMenu.Config.FastPreservesJar && pair.Value.name.Equals("Preserves Jar"))
                    {
                        pair.Value.minutesUntilReady = 0;
                        continue;
                    }
                    if (CJBCheatsMenu.Config.FastCheesePress && pair.Value.name.Equals("Cheese Press"))
                    {
                        pair.Value.minutesUntilReady = 0;
                        continue;
                    }
                    if (CJBCheatsMenu.Config.FastMayonnaiseMachine && pair.Value.name.Equals("Mayonnaise Machine"))
                    {
                        pair.Value.minutesUntilReady = 0;
                        continue;
                    }
                    if (CJBCheatsMenu.Config.FastLoom && pair.Value.name.Equals("Loom"))
                    {
                        pair.Value.minutesUntilReady = 0;
                        continue;
                    }
                    if (CJBCheatsMenu.Config.FastOilMaker && pair.Value.name.Equals("Oil Maker"))
                    {
                        pair.Value.minutesUntilReady = 0;
                        continue;
                    }
                    if (CJBCheatsMenu.Config.FastSeedMaker && pair.Value.name.Equals("Seed Maker"))
                    {
                        pair.Value.minutesUntilReady = 0;
                        continue;
                    }
                    if (CJBCheatsMenu.Config.FastCharcoalKiln && pair.Value.name.Equals("Charcoal Kiln"))
                    {
                        pair.Value.minutesUntilReady = 0;
                        continue;
                    }
                    if (CJBCheatsMenu.Config.FastSlimeEggPress && pair.Value.name.Equals("Slime Egg-Press"))
                    {
                        pair.Value.minutesUntilReady = 0;
                        continue;
                    }
                    if (CJBCheatsMenu.Config.FastBeeHouse && pair.Value.name.Equals("Bee House"))
                    {
                        pair.Value.minutesUntilReady = 0;
                        continue;
                    }
                    if (CJBCheatsMenu.Config.FastMushroomBox && pair.Value.name.Equals("Mushroom Box"))
                    {
                        pair.Value.minutesUntilReady = 0;
                        continue;
                    }
                    if (CJBCheatsMenu.Config.FastTapper && pair.Value.name.Equals("Tapper"))
                    {
                        pair.Value.minutesUntilReady = 0;
                        continue;
                    }
                    if (CJBCheatsMenu.Config.FastLightningRod && pair.Value.name.Equals("Lightning Rod"))
                    {
                        pair.Value.minutesUntilReady = 0;
                        continue;
                    }
                    if (CJBCheatsMenu.Config.FastWormBin && pair.Value.name.Equals("Worm Bin"))
                    {
                        pair.Value.minutesUntilReady = 0;
                        continue;
                    }
                }
            }
            locations.Clear();

            if (CJBCheatsMenu.Config.AutoFeed)
            {
                foreach (GameLocation location in Game1.locations)
                {
                    if (!location.isFarm && !location.name.Contains("Greenhouse"))
                        continue;
                    if (location is BuildableGameLocation)
                    {
                        foreach (Building building in ((BuildableGameLocation)location).buildings)
                        {
                            if (building.indoors is AnimalHouse)
                            {
                                AnimalHouse indoors = (AnimalHouse)building.indoors;

                                int animalcount = indoors.animals.Count();
                                building.currentOccupants = animalcount;
                                int hayobjects = indoors.numberOfObjectsWithName("Hay");
                                int hayUsed = Math.Min(animalcount - hayobjects, (Game1.getLocationFromName("Farm") as Farm).piecesOfHay);
                                (Game1.getLocationFromName("Farm") as Farm).piecesOfHay -= hayUsed;

                                int tileX = 6;
                                if (indoors.name.Contains("Barn"))
                                    tileX = 8;

                                for (int i = 0; i < indoors.animalLimit; i++)
                                {
                                    if (hayUsed <= 0)
                                        break;

                                    Vector2 tile = new Vector2((float)(tileX + i), 3f);
                                    if (!indoors.objects.ContainsKey(tile))
                                    {
                                        indoors.objects.Add(tile, new StardewValley.Object(178, 1, false, -1, 0));
                                    }
                                    hayUsed--;
                                }
                            }
                        }
                    }
                }
            }

            if (CJBCheatsMenu.Config.HarvestSickle)
            {
                foreach (GameLocation location in Game1.locations)
                {
                    if (!location.isFarm && !location.name.Contains("Greenhouse")) continue;
                    foreach (KeyValuePair<Vector2, TerrainFeature> kp in location.terrainFeatures)
                    {
                        if (kp.Value == null) continue;
                        if (kp.Value is HoeDirt)
                        {
                            if (((HoeDirt)kp.Value).crop != null)
                            {
                                ((HoeDirt)kp.Value).crop.harvestMethod = 1;
                            }
                        }
                    }
                }
            }
        }

        public static void OnUpdate()
        {
            if (Game1.player != null)
            {
                Farmer player = Game1.player;



                if (CJBCheatsMenu.Config.IncreasedMovement && player.running)
                    player.addedSpeed = CJBCheatsMenu.Config.MoveSpeed;
                else if (!CJBCheatsMenu.Config.IncreasedMovement && player.addedSpeed == CJBCheatsMenu.Config.MoveSpeed)
                    player.addedSpeed = 0;

                if (player.controller != null)
                {
                    player.addedSpeed = 0;
                }

                if (Game1.CurrentEvent == null)
                    player.movementDirections.Clear();

                if (CJBCheatsMenu.Config.InfiniteHealth)
                    player.health = player.maxHealth;

                if (CJBCheatsMenu.Config.InfiniteStamina)
                    player.stamina = player.maxStamina;

                if (Game1.activeClickableMenu == null && player.CurrentTool is FishingRod)
                {
                    FishingRod rod = (FishingRod)player.CurrentTool;

                    if (CJBCheatsMenu.Config.ThrowBobberMax)
                    {
                        rod.castingPower = 1.01F;
                    }
                    if (CJBCheatsMenu.Config.InstantBite && rod.isFishing)
                    {
                        if (rod.timeUntilFishingBite > 0)
                        {
                            rod.timeUntilFishingBite = 0;
                        }
                    }
                    if (CJBCheatsMenu.Config.DurableTackles && rod.attachments[1] != null)
                    {
                        rod.attachments[1].scale.Y = 1;
                    }
                }

                if (CJBCheatsMenu.Config.OneHitBreak && player.usingTool && (player.CurrentTool is Axe || player.CurrentTool is Pickaxe))
                {
                    Vector2 tile = player.GetToolLocation() / Game1.tileSize;

                    if (player.CurrentTool is Pickaxe && player.currentLocation.objects.ContainsKey(tile))
                    {
                        StardewValley.Object obj = player.currentLocation.Objects[tile];
                        if (obj != null && obj.name.Equals("Stone"))
                        {
                            obj.minutesUntilReady = 0;
                        }
                    }

                    if (player.CurrentTool is Axe && player.currentLocation.terrainFeatures.ContainsKey(tile))
                    {
                        TerrainFeature obj = player.currentLocation.terrainFeatures[tile];
                        if (obj != null && obj is Tree)
                        {
                            Tree tree = (Tree)obj;
                            if (tree.health > 1)
                                tree.health = 1;
                        }
                    }

                    List<ResourceClump> rl = new List<ResourceClump>();
                    if (player.currentLocation is MineShaft)
                        rl.AddRange(((MineShaft)player.currentLocation).resourceClumps);

                    if (player.currentLocation is Farm)
                        rl.AddRange(((Farm)player.currentLocation).resourceClumps);

                    if (player.currentLocation is Forest)
                        rl.Add(((Forest)player.currentLocation).log);

                    if (player.currentLocation is Woods)
                        rl.AddRange(((Woods)player.currentLocation).stumps);

                    foreach (ResourceClump r in rl)
                    {
                        if (r == null) continue;
                        if (r.getBoundingBox(r.tile).Contains((int)player.GetToolLocation().X, (int)player.GetToolLocation().Y) && r.health > 0)
                            r.health = 0;
                    }
                }

                if (CJBCheatsMenu.Config.InfiniteWateringCan && player.CurrentTool is WateringCan)
                {
                    WateringCan can = (WateringCan)player.CurrentTool;
                    typeof(WateringCan).GetField("waterLeft", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(can, can.waterCanMax);
                }

                if (CJBCheatsMenu.Config.AlwaysGiveGift)
                {
                    foreach (KeyValuePair<string, int[]> friendship in player.friendships)
                    {
                        if (friendship.Value != null)
                        {
                            friendship.Value[1] = 0;
                            friendship.Value[3] = 0;
                            /*if (fr.Value[0] < 500)
                                fr.Value[0] = 500;*/
                        }
                    }
                }
            }

            if (CJBCheatsMenu.Config.MaxDailyLuck) Game1.dailyLuck = 0.115d;

            if (CJBCheatsMenu.Config.OneHitKill && Game1.currentLocation != null)
            {
                if (Game1.currentLocation.characters != null)
                {
                    foreach (NPC npc in Game1.currentLocation.characters)
                    {
                        if (npc is Monster)
                        {
                            ((Monster)npc).health = 1;
                        }
                    }
                }
            }

            if ((CJBCheatsMenu.Config.InstantCatch || CJBCheatsMenu.Config.AlwaysTreasure) && Game1.activeClickableMenu is BobberBar)
            {
                BobberBar menu = (BobberBar)Game1.activeClickableMenu;

                if (CJBCheatsMenu.Config.AlwaysTreasure)
                {
                    typeof(BobberBar).GetField("treasure", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(menu, true);
                }

                if (CJBCheatsMenu.Config.InstantCatch)
                {
                    typeof(BobberBar).GetField("distanceFromCatching", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(menu, 1F);
                }

                if ((bool)typeof(BobberBar).GetField("treasure", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(menu))
                {
                    typeof(BobberBar).GetField("treasureCaught", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(menu, true);
                }
            }

            if (CJBCheatsMenu.Config.InfiniteHay)
            {
                Farm farm = (Farm)Game1.getLocationFromName("Farm");
                if (farm != null)
                {
                    farm.piecesOfHay = Utility.numSilos() * 240;
                }
            }
        }

        public static void OnKeyPress(Keys key)
        {
            if (key.ToString().Equals(CJBCheatsMenu.Config.OpenMenuKey))
            {
                if (Game1.hasLoadedGame && Game1.activeClickableMenu == null)
                {
                    CheatsMenu.Open();
                }
                return;
            }

            if (key.ToString().Equals(CJBCheatsMenu.Config.FreezeTimeKey))
            {
                if (Game1.hasLoadedGame && Game1.activeClickableMenu == null)
                {
                    CJBCheatsMenu.Config.FreezeTime = !CJBCheatsMenu.Config.FreezeTime;
                }
                return;
            }

            if (key.ToString().Equals(CJBCheatsMenu.Config.GrowTreeKey))
            {
                if (Game1.hasLoadedGame && Game1.activeClickableMenu == null)
                {
                    GrowTree();
                }
                return;
            }

            if (key.ToString().Equals(CJBCheatsMenu.Config.GrowCropsKey))
            {
                if (Game1.hasLoadedGame && Game1.activeClickableMenu == null)
                {
                    GrowCrops();
                }
                return;
            }
        }
    }
}
