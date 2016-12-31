using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Inheritance;
using StardewValley;
using StardewValley.Minigames;
using StardewValley.Buildings;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Monsters;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CJBCheatsMenu {
    public class Cheats {

        public static void setWeatherForNextDay(int i) {
            Game1.weatherForTomorrow = i;
            Game1.soundBank.PlayCue("thunder");
        }

        public static void waterAllFields() {
            foreach (GameLocation loc in Game1.locations) {
                if (!loc.isFarm && !loc.name.Contains("Greenhouse")) continue;
                foreach (KeyValuePair<Vector2, TerrainFeature> kp in loc.terrainFeatures) {
                    if (kp.Value == null) continue;
                    if (kp.Value is HoeDirt) {
                        ((HoeDirt)kp.Value).state = 1;
                    }
                }
            }
        }

        public static void GrowTree() {
            Farmer plr = Game1.player;
            int x = (int)plr.GetToolLocation().X / Game1.tileSize;
            int y = (int)plr.GetToolLocation().Y / Game1.tileSize;
            Vector2 index = new Vector2(x, y);

            if (plr.currentLocation.terrainFeatures.ContainsKey(index)) {
                TerrainFeature ter = plr.currentLocation.terrainFeatures[index];
                if (ter is Tree) {
                    Tree tree = (Tree)ter;
                    if (!tree.stump)
                        tree.growthStage = 5;
                }
                if (ter is FruitTree) {
                    FruitTree tree = (FruitTree)ter;
                    if (!tree.stump)
                        tree.growthStage = 5;
                }
            }
        }

        public static void GrowCrops() {
            Farmer plr = Game1.player;
            List<Vector2> tiles = new List<Vector2>();

            for (int x = -1; x <= 1; x++) {
                for (int y = -1; y <= 1; y++) {
                    tiles.Add(new Vector2(plr.getTileX() + x, plr.getTileY() + y));
                }
            }

            foreach (Vector2 index in tiles) {
                if (plr.currentLocation.terrainFeatures.ContainsKey(index)) {
                    if (plr.currentLocation.terrainFeatures[index] is HoeDirt) {
                        HoeDirt dirt = (HoeDirt)plr.currentLocation.terrainFeatures[index];
                        dirt.crop?.growCompletely();
                    }
                }
            }
        }

        public static int lasttime = -1;
        public static void onTimeOfDayChanged() {
            GameLocation loc = Game1.currentLocation;
            if (lasttime == -1 || Game1.timeOfDay == 600) {
                lasttime = Game1.timeOfDay;
            } else {
                bool inCave = (loc is MineShaft || loc is FarmCave);
                bool frozen = (CJBCheatsMenu.config.freezeTimeInside && !loc.IsOutdoors && !inCave) || (CJBCheatsMenu.config.freezeTimeCaves && inCave);
                frozen = (frozen || CJBCheatsMenu.config.freezeTime);

                if (frozen) {
                    Game1.timeOfDay = lasttime;
                } else {
                    lasttime = Game1.timeOfDay;
                }
            }
        }

        public static void onDayOfMonthChanged() {

        }

        public static void onDrawTick() {
            GameLocation loc = Game1.currentLocation;

            bool inCave = (loc is MineShaft || loc is FarmCave);
            bool frozen = (CJBCheatsMenu.config.freezeTimeInside && !loc.IsOutdoors && !inCave) || (CJBCheatsMenu.config.freezeTimeCaves && inCave);
            frozen = (frozen || CJBCheatsMenu.config.freezeTime);
            if (frozen) { 
                CJB.drawTextBox(5, inCave ? 100 : 5, Game1.smallFont, "Time Frozen");
            }

            /*
            int xTile = (Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize;
            int yTile = (Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize;

            Vector2 tile = new Vector2(xTile, yTile);

            if (Game1.currentLocation.objects.ContainsKey(tile)) {
                int time = Game1.currentLocation.objects[tile].minutesUntilReady;
                if (time > 0)
                    CJB.drawTextBox(5, inCave ? 100 : 5, Game1.smallFont, "Ready in: " + time);
            }*/
        }

        public static Dictionary<string, int> prevFriends;
        public static void OneSecondUpdate() {

            if (CJBCheatsMenu.config.noFriendshipDecay) {
                if (prevFriends == null) {
                    prevFriends = Game1.player.friendships.ToDictionary(p => p.Key.ToString(), p => p.Value[0]);
                }
                foreach (KeyValuePair<string, int[]> n in Game1.player.friendships) {
                    foreach (KeyValuePair<string, int> o in prevFriends) {
                        if (n.Key.Equals(o.Key) && n.Value[0] < o.Value) {
                            n.Value[0] = o.Value;
                        }
                    }
                }
                prevFriends = Game1.player.friendships.ToDictionary(p => p.Key.ToString(), p => p.Value[0]);
            }
                

            if (CJBCheatsMenu.config.instantBuild) {
                foreach (GameLocation loc in Game1.locations) {
                    if (loc is BuildableGameLocation) {
                        BuildableGameLocation bloc = (BuildableGameLocation)loc;
                        foreach (Building bu in bloc.buildings) {
                            if (bu.daysOfConstructionLeft > 0 || bu.daysUntilUpgrade > 0) {
                                bu.dayUpdate(1);
                            }
                        }
                    }
                }
            }

            List<GameLocation> locations = new List<GameLocation>();
            foreach (GameLocation loc in Game1.locations) {
                if (!loc.isFarm && !loc.name.Contains("Greenhouse"))
                    continue;
                locations.Add(loc);
                if (loc is BuildableGameLocation) {
                    BuildableGameLocation bLoc = (BuildableGameLocation)loc;
                    foreach (Building build in bLoc.buildings) {
                        if (build.indoors != null)
                            locations.Add(build.indoors);
                    }
                }
            }

            foreach (GameLocation loc in locations) {
                foreach (KeyValuePair<Vector2, StardewValley.Object> kp in loc.objects) {
                    if (kp.Value == null)
                        continue;

                    if (CJBCheatsMenu.config.durableFences && kp.Value is Fence) {
                        ((Fence)kp.Value).repair();
                        continue;
                    }

                    if (CJBCheatsMenu.config.fastFurnace && kp.Value.name.Equals("Furnace")) {
                        kp.Value.minutesUntilReady = 0;
                        continue;
                    }
                    if (CJBCheatsMenu.config.fastRecyclingMachine && kp.Value.name.Equals("Recycling Machine")) {
                        kp.Value.minutesUntilReady = 0;
                        continue;
                    }
                    if (CJBCheatsMenu.config.fastCrystalarium && kp.Value.name.Equals("Crystalarium")) {
                        kp.Value.minutesUntilReady = 0;
                        continue;
                    }
                    if (CJBCheatsMenu.config.fastIncubator && kp.Value.name.Equals("Incubator")) {
                        kp.Value.minutesUntilReady = 0;
                        continue;
                    }
                    if (CJBCheatsMenu.config.fastSlimeIncubator && kp.Value.name.Equals("Slime Incubator")) {
                        kp.Value.minutesUntilReady = 0;
                        continue;
                    }
                    if (CJBCheatsMenu.config.fastKeg && kp.Value.name.Equals("Keg")) {
                        kp.Value.minutesUntilReady = 0;
                        continue;
                    }
                    if (CJBCheatsMenu.config.fastPreservesJar && kp.Value.name.Equals("Preserves Jar")) {
                        kp.Value.minutesUntilReady = 0;
                        continue;
                    }
                    if (CJBCheatsMenu.config.fastCheesePress && kp.Value.name.Equals("Cheese Press")) {
                        kp.Value.minutesUntilReady = 0;
                        continue;
                    }
                    if (CJBCheatsMenu.config.fastMayonnaiseMachine && kp.Value.name.Equals("Mayonnaise Machine")) {
                        kp.Value.minutesUntilReady = 0;
                        continue;
                    }
                    if (CJBCheatsMenu.config.fastLoom && kp.Value.name.Equals("Loom")) {
                        kp.Value.minutesUntilReady = 0;
                        continue;
                    }
                    if (CJBCheatsMenu.config.fastOilMaker && kp.Value.name.Equals("Oil Maker")) {
                        kp.Value.minutesUntilReady = 0;
                        continue;
                    }
                    if (CJBCheatsMenu.config.fastSeedMaker && kp.Value.name.Equals("Seed Maker")) {
                        kp.Value.minutesUntilReady = 0;
                        continue;
                    }
                    if (CJBCheatsMenu.config.fastCharcoalKiln && kp.Value.name.Equals("Charcoal Kiln")) {
                        kp.Value.minutesUntilReady = 0;
                        continue;
                    }
                    if (CJBCheatsMenu.config.fastSlimeEggPress && kp.Value.name.Equals("Slime Egg-Press")) {
                        kp.Value.minutesUntilReady = 0;
                        continue;
                    }
                    if (CJBCheatsMenu.config.fastBeeHouse && kp.Value.name.Equals("Bee House")) {
                        kp.Value.minutesUntilReady = 0;
                        continue;
                    }
                    if (CJBCheatsMenu.config.fastMushroomBox && kp.Value.name.Equals("Mushroom Box")) {
                        kp.Value.minutesUntilReady = 0;
                        continue;
                    }
                    if (CJBCheatsMenu.config.fastTapper && kp.Value.name.Equals("Tapper")) {
                        kp.Value.minutesUntilReady = 0;
                        continue;
                    }
                    if (CJBCheatsMenu.config.fastLightningRod && kp.Value.name.Equals("Lightning Rod")) {
                        kp.Value.minutesUntilReady = 0;
                        continue;
                    }
                }
            }
            locations.Clear();

            if (CJBCheatsMenu.config.autoFeed) {
                foreach (GameLocation loc in Game1.locations) {
                    if (!loc.isFarm && !loc.name.Contains("Greenhouse"))
                        continue;
                    if (loc is BuildableGameLocation) {
                        BuildableGameLocation bLoc = (BuildableGameLocation)loc;
                        foreach (Building build in bLoc.buildings) {
                            if (build.indoors is AnimalHouse) {
                                AnimalHouse indoor = (AnimalHouse)build.indoors;

                                int animalcount = indoor.animals.Count();
                                build.currentOccupants = animalcount;
                                int hayobjects = indoor.numberOfObjectsWithName("Hay");
                                int hayUsed = Math.Min(animalcount - hayobjects, (Game1.getLocationFromName("Farm") as Farm).piecesOfHay);
                                (Game1.getLocationFromName("Farm") as Farm).piecesOfHay -= hayUsed;

                                int pos = 6;
                                if (indoor.name.Contains("Barn"))
                                    pos = 8;

                                for (int i = 0; i < indoor.animalLimit; i++) {
                                    if (hayUsed <= 0)
                                        break;

                                    Vector2 key = new Vector2((float)(pos + i), 3f);
                                    if (!indoor.objects.ContainsKey(key)) {
                                        indoor.objects.Add(key, new StardewValley.Object(178, 1, false, -1, 0));
                                    }
                                    hayUsed--;
                                }
                            }
                        }
                    }
                }
            }

            if (CJBCheatsMenu.config.harvestSickle) {
                foreach (GameLocation loc in Game1.locations) {
                    if (!loc.isFarm && !loc.name.Contains("Greenhouse")) continue;
                    foreach (KeyValuePair<Vector2, TerrainFeature> kp in loc.terrainFeatures) {
                        if (kp.Value == null) continue;
                        if (kp.Value is HoeDirt) {
                            if (((HoeDirt)kp.Value).crop != null) {
                                ((HoeDirt)kp.Value).crop.harvestMethod = 1;
                            }
                        }
                    }
                }
            }
        }

        public static void onUpdate() {

            if (Game1.player != null) {
                Farmer plr = Game1.player;

                

                if (CJBCheatsMenu.config.increasedMovement && plr.running)
                    plr.addedSpeed = CJBCheatsMenu.config.moveSpeed;
                else if (!CJBCheatsMenu.config.increasedMovement && plr.addedSpeed == CJBCheatsMenu.config.moveSpeed)
                    plr.addedSpeed = 0;

                if (plr.controller != null) {
                    plr.addedSpeed = 0;
                }

                if (Game1.CurrentEvent == null)
                    plr.movementDirections.Clear();

                if (CJBCheatsMenu.config.infiniteHealth)
                    plr.health = plr.maxHealth;

                if (CJBCheatsMenu.config.infiniteStamina)
                    plr.stamina = plr.maxStamina;

                if (Game1.activeClickableMenu == null && plr.CurrentTool is FishingRod) {
                    FishingRod tool = (FishingRod)plr.CurrentTool;

                    if (CJBCheatsMenu.config.throwBobberMax) {
                        tool.castingPower = 1.01F;
                    }
                    if (CJBCheatsMenu.config.instantBite && tool.isFishing) {
                        if (tool.timeUntilFishingBite > 0) {
                            tool.timeUntilFishingBite = 0;
                        }
                    }
                    if (CJBCheatsMenu.config.durableTackles && tool.attachments[1] != null) {
                        tool.attachments[1].scale.Y = 1;
                    }
                }

                if (CJBCheatsMenu.config.oneHitBreak && plr.usingTool && (plr.CurrentTool is Axe || plr.CurrentTool is Pickaxe)) {
                    int x = (int)plr.GetToolLocation().X / Game1.tileSize;
                    int y = (int)plr.GetToolLocation().Y / Game1.tileSize;

                    Vector2 index = new Vector2(x, y);

                    if (plr.CurrentTool is Pickaxe && plr.currentLocation.objects.ContainsKey(index)) {
                        StardewValley.Object o = plr.currentLocation.Objects[index];
                        if (o != null && o.name.Equals("Stone")) {
                            o.minutesUntilReady = 0;
                        }
                    }

                    if (plr.CurrentTool is Axe && plr.currentLocation.terrainFeatures.ContainsKey(index)) {
                        TerrainFeature o = plr.currentLocation.terrainFeatures[index];
                        if (o != null && o is Tree) {
                            Tree t = (Tree)o;
                            if (t.health > 1)
                                t.health = 1;
                        }
                    }

                    List<ResourceClump> rl = new List<ResourceClump>();
                    if (plr.currentLocation is MineShaft)
                        rl.AddRange(((MineShaft)plr.currentLocation).resourceClumps);

                    if (plr.currentLocation is Farm)
                        rl.AddRange(((Farm)plr.currentLocation).resourceClumps);

                    if (plr.currentLocation is Forest)
                        rl.Add(((Forest)plr.currentLocation).log);

                    if (plr.currentLocation is Woods)
                        rl.AddRange(((Woods)plr.currentLocation).stumps);

                    foreach (ResourceClump r in rl) {
                        if (r == null) continue;
                        if (r.getBoundingBox(r.tile).Contains((int)plr.GetToolLocation().X, (int)plr.GetToolLocation().Y) && r.health > 0)
                            r.health = 0;
                    }
                }

                if (CJBCheatsMenu.config.infiniteWateringCan && plr.CurrentTool is WateringCan) {
                    WateringCan tool = (WateringCan)plr.CurrentTool;
                    typeof(WateringCan).GetField("waterLeft", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(tool, tool.waterCanMax);
                }

                if (CJBCheatsMenu.config.alwaysGiveGift) {
                    foreach (KeyValuePair<string, int[]> fr in plr.friendships) {
                        if (fr.Value != null) {
                            fr.Value[1] = 0;
                            fr.Value[3] = 0;
                            /*if (fr.Value[0] < 500)
                                fr.Value[0] = 500;*/
                        }
                    }
                }
            }

            if (CJBCheatsMenu.config.maxDailyLuck) Game1.dailyLuck = 0.115d;

            if (CJBCheatsMenu.config.oneHitKill && Game1.currentLocation != null) {
                if (Game1.currentLocation.characters != null) {
                    foreach (NPC npc in Game1.currentLocation.characters) {
                        if (npc is Monster) {
                            ((Monster)npc).health = 1;
                        }
                    }
                }
            }

            if ((CJBCheatsMenu.config.instantCatch || CJBCheatsMenu.config.alwaysTreasure) && Game1.activeClickableMenu is BobberBar) {
                BobberBar menu = (BobberBar)Game1.activeClickableMenu;

                if (CJBCheatsMenu.config.alwaysTreasure) {
                    typeof(BobberBar).GetField("treasure", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(menu, true);
                }

                if (CJBCheatsMenu.config.instantCatch) {
                    typeof(BobberBar).GetField("distanceFromCatching", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(menu, 1F);
                }

                if ((bool)typeof(BobberBar).GetField("treasure", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(menu)) {
                    typeof(BobberBar).GetField("treasureCaught", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(menu, true);
                }
            }

            if (CJBCheatsMenu.config.infiniteHay) {
                Farm farm = (Farm)Game1.getLocationFromName("Farm");
                if (farm != null) {
                    farm.piecesOfHay = Utility.numSilos() * 240;
                }
            }
        }

        public static void onKeyPress(Microsoft.Xna.Framework.Input.Keys key) {
            if (key.ToString().Equals(CJBCheatsMenu.config.openMenuKey)) {
                if (Game1.hasLoadedGame && Game1.activeClickableMenu == null) {
                    CheatsMenu.Open();
                }
                return;
            }

            if (key.ToString().Equals(CJBCheatsMenu.config.freezeTimeKey)) {
                if (Game1.hasLoadedGame && Game1.activeClickableMenu == null) {
                    CJBCheatsMenu.config.freezeTime = !CJBCheatsMenu.config.freezeTime;
                }
                return;
            }

            if (key.ToString().Equals(CJBCheatsMenu.config.growTreeKey)) {
                if (Game1.hasLoadedGame && Game1.activeClickableMenu == null) {
                    GrowTree();
                }
                return;
            }

            if (key.ToString().Equals(CJBCheatsMenu.config.growCropsKey)) {
                if (Game1.hasLoadedGame && Game1.activeClickableMenu == null) {
                    GrowCrops();
                }
                return;
            }
        }
    }
}
