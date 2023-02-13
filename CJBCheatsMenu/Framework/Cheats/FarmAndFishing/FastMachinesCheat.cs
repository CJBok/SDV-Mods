using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CJBCheatsMenu.Framework.Components;
using CJBCheatsMenu.Framework.Models;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Menus;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using SObject = StardewValley.Object;

namespace CJBCheatsMenu.Framework.Cheats.FarmAndFishing
{
    /// <summary>A cheat which makes machines complete their output instantly.</summary>
    internal class FastMachinesCheat : BaseCheat
    {
        /*********
        ** Public methods
        *********/
        /// <summary>Get the config UI fields to show in the cheats menu.</summary>
        /// <param name="context">The cheat context.</param>
        public override IEnumerable<OptionsElement> GetFields(CheatContext context)
        {
            return this.SortFields(
                new CheatsOptionsCheckbox(
                    label: GameI18n.GetBigCraftableName("10"),
                    value: context.Config.FastBeeHouse,
                    setValue: value => context.Config.FastBeeHouse = value
                ),
                new CheatsOptionsCheckbox(
                    label: GameI18n.GetBigCraftableName("90"),
                    value: context.Config.FastBoneMill,
                    setValue: value => context.Config.FastBoneMill = value
                ),
                new CheatsOptionsCheckbox(
                    label: GameI18n.GetBigCraftableName("163"),
                    value: context.Config.FastCask,
                    setValue: value => context.Config.FastCask = value
                ),
                new CheatsOptionsCheckbox(
                    label: GameI18n.GetBigCraftableName("114"),
                    value: context.Config.FastCharcoalKiln,
                    setValue: value => context.Config.FastCharcoalKiln = value
                ),
                new CheatsOptionsCheckbox(
                    label: GameI18n.GetBigCraftableName("16"),
                    value: context.Config.FastCheesePress,
                    setValue: value => context.Config.FastCheesePress = value
                ),
                new CheatsOptionsCheckbox(
                    label: GameI18n.GetBigCraftableName("246"),
                    value: context.Config.FastCoffeeMaker,
                    setValue: value => context.Config.FastCoffeeMaker = value
                ),
                new CheatsOptionsCheckbox(
                    label: GameI18n.GetObjectName("710"),
                    value: context.Config.FastCrabPot,
                    setValue: value => context.Config.FastCrabPot = value
                ),
                new CheatsOptionsCheckbox(
                    label: GameI18n.GetBigCraftableName("21"),
                    value: context.Config.FastCrystalarium,
                    setValue: value => context.Config.FastCrystalarium = value
                ),
                new CheatsOptionsCheckbox(
                    label: GameI18n.GetBigCraftableName("265"),
                    value: context.Config.FastDeconstructor,
                    setValue: value => context.Config.FastDeconstructor = value
                ),
                new CheatsOptionsCheckbox(
                    label: I18n.FastMachines_FruitTrees(),
                    value: context.Config.FastFruitTree,
                    setValue: value => context.Config.FastFruitTree = value
                ),
                new CheatsOptionsCheckbox(
                    label: GameI18n.GetBigCraftableName("13"),
                    value: context.Config.FastFurnace,
                    setValue: value => context.Config.FastFurnace = value
                ),
                new CheatsOptionsCheckbox(
                    label: GameI18n.GetBigCraftableName("182"),
                    value: context.Config.FastGeodeCrusher,
                    setValue: value => context.Config.FastGeodeCrusher = value
                ),
                new CheatsOptionsCheckbox(
                    label: I18n.FastMachines_ReadyInTheMorning(machineName: GameI18n.GetBigCraftableName("101")),
                    value: context.Config.FastIncubator,
                    setValue: value => context.Config.FastIncubator = value
                ),
                new CheatsOptionsCheckbox(
                    label: GameI18n.GetBigCraftableName("12"),
                    value: context.Config.FastKeg,
                    setValue: value => context.Config.FastKeg = value
                ),
                new CheatsOptionsCheckbox(
                    label: GameI18n.GetBigCraftableName("9"),
                    value: context.Config.FastLightningRod,
                    setValue: value => context.Config.FastLightningRod = value
                ),
                new CheatsOptionsCheckbox(
                    label: GameI18n.GetBigCraftableName("17"),
                    value: context.Config.FastLoom,
                    setValue: value => context.Config.FastLoom = value
                ),
                new CheatsOptionsCheckbox(
                    label: GameI18n.GetBigCraftableName("24"),
                    value: context.Config.FastMayonnaiseMachine,
                    setValue: value => context.Config.FastMayonnaiseMachine = value
                ),
                new CheatsOptionsCheckbox(
                    label: Game1.content.LoadString("Strings\\Buildings:Mill_Name"),
                    value: context.Config.FastMillMachine,
                    setValue: value => context.Config.FastMillMachine = value
                ),
                new CheatsOptionsCheckbox(
                    label: GameI18n.GetBigCraftableName("128"),
                    value: context.Config.FastMushroomBox,
                    setValue: value => context.Config.FastMushroomBox = value
                ),
                new CheatsOptionsCheckbox(
                    label: GameI18n.GetBigCraftableName("19"),
                    value: context.Config.FastOilMaker,
                    setValue: value => context.Config.FastOilMaker = value
                ),
                new CheatsOptionsCheckbox(
                    label: I18n.FastMachines_ReadyInTheMorning(GameI18n.GetBigCraftableName("254")),
                    value: context.Config.FastOstrichIncubator,
                    setValue: value => context.Config.FastOstrichIncubator = value
                ),
                new CheatsOptionsCheckbox(
                    label: GameI18n.GetBigCraftableName("15"),
                    value: context.Config.FastPreservesJar,
                    setValue: value => context.Config.FastPreservesJar = value
                ),
                new CheatsOptionsCheckbox(
                    label: GameI18n.GetBigCraftableName("20"),
                    value: context.Config.FastRecyclingMachine,
                    setValue: value => context.Config.FastRecyclingMachine = value
                ),
                new CheatsOptionsCheckbox(
                    label: GameI18n.GetBigCraftableName("25"),
                    value: context.Config.FastSeedMaker,
                    setValue: value => context.Config.FastSeedMaker = value
                ),
                new CheatsOptionsCheckbox(
                    label: GameI18n.GetBigCraftableName("158"),
                    value: context.Config.FastSlimeEggPress,
                    setValue: value => context.Config.FastSlimeEggPress = value
                ),
                new CheatsOptionsCheckbox(
                    label: GameI18n.GetBigCraftableName("156"),
                    value: context.Config.FastSlimeIncubator,
                    setValue: value => context.Config.FastSlimeIncubator = value
                ),
                new CheatsOptionsCheckbox(
                    label: GameI18n.GetBigCraftableName("117"),
                    value: context.Config.FastSodaMachine,
                    setValue: value => context.Config.FastSodaMachine = value
                ),

                new CheatsOptionsCheckbox(
                    label: GameI18n.GetBigCraftableName("231"),
                    value: context.Config.FastSolarPanel,
                    setValue: value => context.Config.FastSolarPanel = value
                ),
                new CheatsOptionsCheckbox(
                    label: GameI18n.GetBigCraftableName("127"),
                    value: context.Config.FastStatueOfEndlessFortune,
                    setValue: value => context.Config.FastStatueOfEndlessFortune = value
                ),
                new CheatsOptionsCheckbox(
                    label: GameI18n.GetBigCraftableName("160"),
                    value: context.Config.FastStatueOfPerfection,
                    setValue: value => context.Config.FastStatueOfPerfection = value
                ),
                new CheatsOptionsCheckbox(
                    label: GameI18n.GetBigCraftableName("280"),
                    value: context.Config.FastStatueOfTruePerfection,
                    setValue: value => context.Config.FastStatueOfTruePerfection = value
                ),
                new CheatsOptionsCheckbox(
                    label: GameI18n.GetBigCraftableName("105"),
                    value: context.Config.FastTapper,
                    setValue: value => context.Config.FastTapper = value
                ),
                new CheatsOptionsCheckbox(
                    label: GameI18n.GetBigCraftableName("211"),
                    value: context.Config.FastWoodChipper,
                    setValue: value => context.Config.FastWoodChipper = value
                ),
                new CheatsOptionsCheckbox(
                    label: GameI18n.GetBigCraftableName("154"),
                    value: context.Config.FastWormBin,
                    setValue: value => context.Config.FastWormBin = value
                )
            );
        }

        /// <summary>Handle the cheat options being loaded or changed.</summary>
        /// <param name="context">The cheat context.</param>
        /// <param name="needsUpdate">Whether the cheat should be notified of game updates.</param>
        /// <param name="needsInput">Whether the cheat should be notified of button presses.</param>
        /// <param name="needsRendering">Whether the cheat should be notified of render ticks.</param>
        public override void OnConfig(CheatContext context, out bool needsInput, out bool needsUpdate, out bool needsRendering)
        {
            needsInput = false;
            needsUpdate = this
                .GetFields(context)
                .Cast<CheatsOptionsCheckbox>()
                .Any(p => p.IsChecked);
            needsRendering = false;
        }

        /// <summary>Handle a game update if <see cref="ICheat.OnSaveLoaded"/> indicated updates were needed.</summary>
        /// <param name="context">The cheat context.</param>
        /// <param name="e">The update event arguments.</param>
        public override void OnUpdated(CheatContext context, UpdateTickedEventArgs e)
        {
            if (!e.IsOneSecond || !Context.IsWorldReady)
                return;

            foreach (GameLocation location in context.GetAllLocations())
            {
                foreach (Building building in location.buildings)
                {
                    if (this.IsFastMachine(context, building))
                        this.CompleteMachine(location, building);
                }

                foreach (SObject obj in location.objects.Values)
                {
                    if (this.IsFastMachine(context, obj))
                        this.CompleteMachine(location, obj);
                }

                if (context.Config.FastFruitTree)
                {
                    foreach (TerrainFeature terrainFeature in location.terrainFeatures.Values)
                    {
                        if (terrainFeature is FruitTree tree && tree.growthStage.Value >= FruitTree.treeStage)
                        {
                            int addFruit = FruitTree.maxFruitsOnTrees - tree.fruit.Count;
                            for (int i = 0; i < addFruit; i++)
                                tree.TryAddFruit();
                        }
                    }
                }
            }
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Get whether a building is a machine with 'fast processing' enabled.</summary>
        /// <param name="context">The cheat context.</param>
        /// <param name="building">The machine to check.</param>
        private bool IsFastMachine(CheatContext context, [NotNullWhen(true)] Building? building)
        {
            ModConfig config = context.Config;
            return building?.buildingType.Value switch
            {
                "Mill" => config.FastMillMachine,
                _ => false
            };
        }

        /// <summary>Get whether an object is a machine with 'fast processing' enabled.</summary>
        /// <param name="context">The cheat context.</param>
        /// <param name="obj">The machine to check.</param>
        private bool IsFastMachine(CheatContext context, [NotNullWhen(true)] SObject? obj)
        {
            // quick initial check
            if (obj is not (CrabPot or { bigCraftable.Value: true }))
                return false;

            // specific check
            ModConfig config = context.Config;
            return obj switch
            {
                Cask => config.FastCask,
                CrabPot => config.FastCrabPot,
                WoodChipper => config.FastWoodChipper,
                _ => obj.name switch
                {
                    "Bee House" => config.FastBeeHouse,
                    "Bone Mill" => config.FastBoneMill,
                    "Charcoal Kiln" => config.FastCharcoalKiln,
                    "Cheese Press" => config.FastCheesePress,
                    "Coffee Maker" => config.FastCoffeeMaker,
                    "Crystalarium" => config.FastCrystalarium,
                    "Deconstructor" => config.FastDeconstructor,
                    "Furnace" => config.FastFurnace,
                    "Geode Crusher" => config.FastGeodeCrusher,
                    "Heavy Tapper" => config.FastTapper,
                    "Incubator" => config.FastIncubator,
                    "Keg" => config.FastKeg,
                    "Lightning Rod" => config.FastLightningRod,
                    "Loom" => config.FastLoom,
                    "Mayonnaise Machine" => config.FastMayonnaiseMachine,
                    "Mushroom Box" => config.FastMushroomBox,
                    "Oil Maker" => config.FastOilMaker,
                    "Ostrich Incubator" => config.FastOstrichIncubator,
                    "Preserves Jar" => config.FastPreservesJar,
                    "Recycling Machine" => config.FastRecyclingMachine,
                    "Seed Maker" => config.FastSeedMaker,
                    "Slime Egg-Press" => config.FastSlimeEggPress,
                    "Slime Incubator" => config.FastSlimeIncubator,
                    "Soda Machine" => config.FastSodaMachine,
                    "Solar Panel" => config.FastSolarPanel,
                    "Statue Of Endless Fortune" => config.FastStatueOfEndlessFortune,
                    "Statue Of Perfection" => config.FastStatueOfPerfection,
                    "Statue Of True Perfection" => config.FastStatueOfTruePerfection,
                    "Tapper" => config.FastTapper,
                    "Worm Bin" => config.FastWormBin,
                    _ => false
                }
            };
        }

        /// <summary>Finish a machine's processing.</summary>
        /// <param name="location">The machine's location.</param>
        /// <param name="machine">The machine to complete.</param>
        private void CompleteMachine(GameLocation location, Building machine)
        {
            if (machine.isUnderConstruction())
                return;

            machine.dayUpdate(Game1.dayOfMonth);
        }

        /// <summary>Finish a machine's processing.</summary>
        /// <param name="location">The machine's location.</param>
        /// <param name="machine">The machine to complete.</param>
        private void CompleteMachine(GameLocation location, SObject machine)
        {
            bool hasItem = machine.heldObject.Value != null;
            bool processing = machine.MinutesUntilReady > 0;

            // mark complete
            switch (machine)
            {
                case Cask cask:
                    if (hasItem && processing)
                    {
                        cask.heldObject.Value.Quality = SObject.bestQuality;
                        cask.daysToMature.Value = 0;
                        cask.MinutesUntilReady = 1;
                        machine.minutesElapsed(machine.MinutesUntilReady, location);
                    }
                    break;

                case CrabPot pot:
                    if (!hasItem)
                        pot.DayUpdate(location);
                    break;

                // by name
                default:
                    switch (machine.Name)
                    {
                        // daily machines
                        case "Coffee Maker":
                        case "Soda Machine":
                        case "Statue Of Endless Fortune":
                        case "Statue Of Perfection":
                        case "Statue Of True Perfection":
                            if (!hasItem)
                            {
                                machine.DayUpdate(location);
                                machine.minutesElapsed(1, location);
                            }
                            break;

                        // daily solar panels
                        case "Solar Panel":
                            if (machine.MinutesUntilReady > 1)
                                machine.MinutesUntilReady = 1;
                            machine.DayUpdate(location); // complete machine if conditions are correct (e.g. outdoors and sunny)
                            break;

                        // input processing machines
                        default:
                            if (hasItem && processing)
                                machine.minutesElapsed(machine.MinutesUntilReady, location);
                            break;
                    }
                    break;
            }

            // run post-completion logic
            switch (machine.Name)
            {
                case "Mushroom Box" when !hasItem:
                case "Slime Incubator" when hasItem:
                    machine.DayUpdate(location);
                    break;
            }
        }
    }
}
