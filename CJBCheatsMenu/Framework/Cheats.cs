using System;
using System.Collections.Generic;
using System.Linq;
using CJBCheatsMenu.Framework.Models;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Monsters;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using SObject = StardewValley.Object;

namespace CJBCheatsMenu.Framework
{
    internal class Cheats
    {
        /*********
        ** Fields
        *********/
        /// <summary>The mod settings.</summary>
        private readonly ModConfig Config;

        /// <summary>Provides translations stored in the mod folder.</summary>
        private readonly ITranslationHelper Translations;

        /// <summary>The minimum friendship points to maintain for each NPC.</summary>
        private readonly Dictionary<string, int> PreviousFriendships = new Dictionary<string, int>();

        /// <summary>Whether to grow crops under the cursor.</summary>
        private bool ShouldGrowCrops;

        /// <summary>Whether to grow trees under the cursor.</summary>
        private bool ShouldGrowTrees;

        /// <summary>The unique buff ID for the player speed.</summary>
        private int BuffUniqueID => 58012398 + this.Config.MoveSpeed;


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="config">The mod settings.</param>
        /// <param name="translations">Provides translations stored in the mod folder.</param>
        public Cheats(ModConfig config, ITranslationHelper translations)
        {
            this.Config = config;
            this.Translations = translations;
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

        public void WaterAllFields()
        {
            foreach (GameLocation location in CJB.GetAllLocations())
            {
                if (!location.IsFarm && !location.IsGreenhouse)
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

        public void GrowTree(Vector2 origin)
        {
            var player = Game1.player;
            if (player == null)
                return;

            if (player.currentLocation.terrainFeatures.ContainsKey(origin))
            {
                TerrainFeature terrainFeature = player.currentLocation.terrainFeatures[origin];
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

        public void GrowCrops(Vector2 origin)
        {
            var player = Game1.player;
            if (player == null)
                return;

            const int radius = 1;
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    Vector2 tile = new Vector2(origin.X + x, origin.Y + y);

                    // get target
                    object target = null;
                    {
                        if (player.currentLocation.terrainFeatures.TryGetValue(tile, out TerrainFeature terrainFeature))
                        {
                            if (terrainFeature is HoeDirt dirt)
                                target = dirt.crop;
                            if (terrainFeature is Bush bush)
                                target = bush;
                        }
                        if (target == null && player.currentLocation.objects.TryGetValue(tile, out SObject obj) && obj is IndoorPot pot)
                        {
                            // crop
                            if (pot.hoeDirt.Value is HoeDirt dirt)
                                target = dirt.crop;

                            // planted bush
                            if (pot.bush.Value is Bush bush)
                                target = bush;
                        }
                    }

                    // grow target
                    switch (target)
                    {
                        case Crop crop:
                            crop.growCompletely();
                            break;

                        case Bush bush when bush.size.Value == Bush.greenTeaBush && bush.getAge() < Bush.daysToMatureGreenTeaBush:
                            bush.datePlanted.Value = (int)(Game1.stats.DaysPlayed - Bush.daysToMatureGreenTeaBush);
                            bush.dayUpdate(player.currentLocation, tile); // update source rect, grow tea leaves, etc
                            break;
                    }
                }
            }
        }

        /// <summary>Apply the player speed buff to the current player.</summary>
        private void UpdateBuff()
        {
            // ignore if disabled
            if (!this.Config.IncreasedMovement)
                return;

            // ignore in cutscenes
            if (Game1.eventUp)
                return;

            // ignore if walking
            bool running = Game1.player.running;
            bool runEnabled = running || Game1.options.autoRun != Game1.isOneOfTheseKeysDown(Game1.GetKeyboardState(), Game1.options.runButton); // auto-run enabled and not holding walk button, or disabled and holding run button
            if (!runEnabled)
                return;

            // add or update buff
            Buff buff = Game1.buffsDisplay.otherBuffs.FirstOrDefault(p => p.which == this.BuffUniqueID);
            if (buff == null)
            {
                buff = new Buff(0, 0, 0, 0, 0, 0, 0, 0, 0, speed: this.Config.MoveSpeed, 0, 0, minutesDuration: 1, source: "CJB Cheats Menu", displaySource: this.Translations.Get("mod-name")) { which = this.BuffUniqueID };
                Game1.buffsDisplay.addOtherBuff(buff);
            }
            buff.millisecondsDuration = 50;
        }

        /// <summary>Perform any action needed after the cheat options change.</summary>
        public void OnOptionsChanged()
        {
            // disable harvest with scythe
            if (!this.Config.HarvestScythe)
            {
                IDictionary<int, int> cropHarvestMethods = this.GetCropHarvestMethods();
                foreach (GameLocation location in Game1.locations)
                {
                    if (!location.IsFarm && !location.IsGreenhouse)
                        continue;
                    foreach (TerrainFeature terrainFeature in location.terrainFeatures.Values)
                    {
                        if (terrainFeature is HoeDirt dirt && dirt.crop != null && cropHarvestMethods.TryGetValue(dirt.crop.indexOfHarvest.Value, out int harvestMethod))
                            dirt.crop.harvestMethod.Value = harvestMethod;
                    }
                }
            }
        }

        /// <summary>Raised after the game draws to the sprite patch in a draw tick, just before the final sprite batch is rendered to the screen.</summary>
        /// <param name="i18n">The translation helper.</param>
        public void OnRendered(ITranslationHelper i18n)
        {
            if (this.ShouldFreezeTime(Game1.currentLocation, out bool isCave))
                CJB.DrawTextBox(5, isCave ? 100 : 5, Game1.smallFont, i18n.Get("messages.time-frozen"));
        }

        /// <summary>Raised once per second.</summary>
        /// <param name="locations">The known in-game locations.</param>
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
            foreach (GameLocation location in locations)
            {
                // instant buildings
                if (this.Config.InstantBuild && location is BuildableGameLocation buildableLocation)
                {
                    foreach (Building building in buildableLocation.buildings)
                    {
                        if (building.daysOfConstructionLeft.Value > 0)
                            building.dayUpdate(0);
                        if (building.daysUntilUpgrade.Value > 0)
                            building.dayUpdate(0);
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
                    else if (this.IsFastMachine(obj))
                        this.CompleteMachine(location, obj);
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

                // auto-feed animals
                if (this.Config.AutoFeed && farm != null && location is AnimalHouse animalHouse)
                {
                    int animalCount = animalHouse.animals.Values.Count();
                    int hayObjects = animalHouse.numberOfObjectsWithName("Hay");
                    int hayUsed = Math.Min(animalCount - hayObjects, farm.piecesOfHay.Value);
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

                // harvest with scythe
                if (this.Config.HarvestScythe && (location.IsFarm || location.IsGreenhouse))
                {
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

        /// <summary>Raised after the game state is updated (≈60 times per second).</summary>
        /// <param name="e">The event arguments.</param>
        /// <param name="reflection">Simplifies access to private game code.</param>
        /// <param name="input">An API for checking input state.</param>
        public void OnUpdateTicked(UpdateTickedEventArgs e, IReflectionHelper reflection, IInputHelper input)
        {
            if (Game1.player?.currentLocation != null)
            {
                Farmer player = Game1.player;

                // movement speed
                this.UpdateBuff();

                // infinite health/stamina
                if (this.Config.InfiniteHealth)
                    player.health = player.maxHealth;
                if (this.Config.InfiniteStamina)
                    player.stamina = player.MaxStamina;

                // fishing cheats
                if (player.CurrentTool is FishingRod rod)
                {
                    BobberBar bobberMenu = Game1.activeClickableMenu as BobberBar;

                    // max cast power
                    if (this.Config.ThrowBobberMax && rod.isTimingCast)
                        rod.castingPower = 1.01f;

                    // durable tackles
                    if (this.Config.DurableTackles && rod.attachments[1] != null)
                        rod.attachments[1].uses.Value = 0;

                    // instant bite
                    if (this.Config.InstantBite && rod.isFishing && !rod.hit && rod.timeUntilFishingBite > 0)
                        rod.timeUntilFishingBite = 0;

                    // always treasure
                    if (this.Config.AlwaysTreasure && bobberMenu != null)
                        reflection.GetField<bool>(bobberMenu, "treasure").SetValue(true);

                    // instant catch
                    if (this.Config.InstantCatch && bobberMenu != null)
                    {
                        reflection.GetField<float>(bobberMenu, "distanceFromCatching").SetValue(1);
                        if (reflection.GetField<bool>(bobberMenu, "treasure").GetValue())
                            reflection.GetField<bool>(bobberMenu, "treasureCaught").SetValue(true);
                    }
                }

                // one-hit break
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

                // infinite watering can
                if (this.Config.InfiniteWateringCan && player.CurrentTool is WateringCan can)
                    can.WaterLeft = can.waterCanMax;

                // unlimited gifts
                if (this.Config.AlwaysGiveGift)
                {
                    foreach (Friendship friendship in player.friendshipData.Values)
                    {
                        friendship.GiftsThisWeek = 0;
                        friendship.GiftsToday = 0;
                    }
                }

                // one-hit kill
                if (this.Config.OneHitKill)
                {
                    foreach (Monster monster in player.currentLocation.characters.OfType<Monster>())
                    {
                        if (monster.Health > 1)
                            monster.Health = 1;
                    }
                }

                // grow crops/trees
                if (this.ShouldGrowCrops && e.IsMultipleOf(15))
                    this.GrowCrops(input.GetCursorPosition().Tile);
                if (this.ShouldGrowTrees && e.IsMultipleOf(15))
                    this.GrowTree(input.GetCursorPosition().Tile);
            }

            if (this.Config.MaxDailyLuck)
                Game1.player.team.sharedDailyLuck.Value = 0.115d;

            if (this.Config.InfiniteHay)
            {
                Farm farm = Game1.getFarm();
                if (farm != null)
                    farm.piecesOfHay.Value = Utility.numSilos() * 240;
            }

            if (this.ShouldFreezeTime(Game1.currentLocation, out bool _))
                Game1.gameTimeInterval = 0;
        }

        /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
        /// <param name="input">The event arguments.</param>
        public void OnButtonPressed(ButtonPressedEventArgs input)
        {
            if (!Context.IsPlayerFree)
                return;

            if (input.Button == this.Config.FreezeTimeKey)
                this.Config.FreezeTime = !this.Config.FreezeTime;
            else if (input.Button == this.Config.GrowTreeKey)
                this.ShouldGrowTrees = true;
            else if (input.Button == this.Config.GrowCropsKey)
                this.ShouldGrowCrops = true;
        }

        /// <summary>Raised after the player releases a button on the keyboard, controller, or mouse.</summary>
        /// <param name="input">The event arguments.</param>
        public void OnButtonReleased(ButtonReleasedEventArgs input)
        {
            if (input.Button == this.Config.GrowTreeKey)
                this.ShouldGrowTrees = false;
            else if (input.Button == this.Config.GrowCropsKey)
                this.ShouldGrowCrops = false;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Get whether an object is a machine with 'fast processing' enabled.</summary>
        /// <param name="obj">The machine to check.</param>
        private bool IsFastMachine(SObject obj)
        {
            return
                (this.Config.FastBeeHouse && obj.name == "Bee House")
                || (this.Config.FastCask && obj is Cask)
                || (this.Config.FastCharcoalKiln && obj.name == "Charcoal Kiln")
                || (this.Config.FastCheesePress && obj.name == "Cheese Press")
                || (this.Config.FastCrystalarium && obj.name == "Crystalarium")
                || (this.Config.FastFurnace && obj.name == "Furnace")
                || (this.Config.FastIncubator && obj.name == "Incubator")
                || (this.Config.FastKeg && obj.name == "Keg")
                || (this.Config.FastLightningRod && obj.name == "Lightning Rod")
                || (this.Config.FastLoom && obj.name == "Loom")
                || (this.Config.FastMayonnaiseMachine && obj.name == "Mayonnaise Machine")
                || (this.Config.FastMushroomBox && obj.name == "Mushroom Box")
                || (this.Config.FastOilMaker && obj.name == "Oil Maker")
                || (this.Config.FastPreservesJar && obj.name == "Preserves Jar")
                || (this.Config.FastRecyclingMachine && obj.name == "Recycling Machine")
                || (this.Config.FastSeedMaker && obj.name == "Seed Maker")
                || (this.Config.FastSlimeEggPress && obj.name == "Slime Egg-Press")
                || (this.Config.FastSlimeIncubator && obj.name == "Slime Incubator")
                || (this.Config.FastTapper && obj.name == "Tapper")
                || (this.Config.FastWoodChipper && obj is WoodChipper)
                || (this.Config.FastWormBin && obj.name == "Worm Bin");
        }

        /// <summary>Finish a machine's processing.</summary>
        /// <param name="location">The machine's location.</param>
        /// <param name="machine">The machine to complete.</param>
        private void CompleteMachine(GameLocation location, SObject machine)
        {
            if (machine.heldObject.Value == null)
                return;

            // casks
            if (machine is Cask cask)
            {
                cask.daysToMature.Value = 0;
                cask.checkForMaturity();
            }

            // other machines
            if (machine.MinutesUntilReady > 0)
                machine.minutesElapsed(machine.MinutesUntilReady, location);
        }

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

        /// <summary>Get whether time should be frozen in the given location.</summary>
        /// <param name="location">The location to check.</param>
        /// <param name="isCave">Indicates whether the given location is a cave.</param>
        private bool ShouldFreezeTime(GameLocation location, out bool isCave)
        {
            isCave = location is MineShaft || location is FarmCave;

            if (this.Config.FreezeTime)
                return true;
            if (this.Config.FreezeTimeCaves && isCave)
                return true;
            if (this.Config.FreezeTimeInside && location != null && !location.IsOutdoors && !isCave)
                return true;

            return false;
        }
    }
}
