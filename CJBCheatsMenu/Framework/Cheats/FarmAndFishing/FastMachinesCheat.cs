using System.Collections.Generic;
using System.Linq;
using CJBCheatsMenu.Framework.Components;
using CJBCheatsMenu.Framework.Models;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
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
                    label: I18n.FastMachines_BeeHouse(),
                    value: context.Config.FastBeeHouse,
                    setValue: value => context.Config.FastBeeHouse = value
                ),
                new CheatsOptionsCheckbox(
                    label: I18n.FastMachines_Cask(),
                    value: context.Config.FastCask,
                    setValue: value => context.Config.FastCask = value
                ),
                new CheatsOptionsCheckbox(
                    label: I18n.FastMachines_CharcoalKiln(),
                    value: context.Config.FastCharcoalKiln,
                    setValue: value => context.Config.FastCharcoalKiln = value
                ),
                new CheatsOptionsCheckbox(
                    label: I18n.FastMachines_CheesePress(),
                    value: context.Config.FastCheesePress,
                    setValue: value => context.Config.FastCheesePress = value
                ),
                new CheatsOptionsCheckbox(
                    label: I18n.FastMachines_CrabPot(),
                    value: context.Config.FastCrabPot,
                    setValue: value => context.Config.FastCrabPot = value
                ),
                new CheatsOptionsCheckbox(
                    label: I18n.FastMachines_Crystalarium(),
                    value: context.Config.FastCrystalarium,
                    setValue: value => context.Config.FastCrystalarium = value
                ),
                new CheatsOptionsCheckbox(
                    label: I18n.FastMachines_FruitTrees(),
                    value: context.Config.FastFruitTree,
                    setValue: value => context.Config.FastFruitTree = value
                ),
                new CheatsOptionsCheckbox(
                    label: I18n.FastMachines_Furnace(),
                    value: context.Config.FastFurnace,
                    setValue: value => context.Config.FastFurnace = value
                ),
                new CheatsOptionsCheckbox(
                    label: I18n.FastMachines_Incubator(),
                    value: context.Config.FastIncubator,
                    setValue: value => context.Config.FastIncubator = value
                ),
                new CheatsOptionsCheckbox(
                    label: I18n.FastMachines_Keg(),
                    value: context.Config.FastKeg,
                    setValue: value => context.Config.FastKeg = value
                ),
                new CheatsOptionsCheckbox(
                    label: I18n.FastMachines_LightningRod(),
                    value: context.Config.FastLightningRod,
                    setValue: value => context.Config.FastLightningRod = value
                ),
                new CheatsOptionsCheckbox(
                    label: I18n.FastMachines_Loom(),
                    value: context.Config.FastLoom,
                    setValue: value => context.Config.FastLoom = value
                ),
                new CheatsOptionsCheckbox(
                    label: I18n.FastMachines_MayonnaiseMachine(),
                    value: context.Config.FastMayonnaiseMachine,
                    setValue: value => context.Config.FastMayonnaiseMachine = value
                ),
                new CheatsOptionsCheckbox(
                    label: I18n.FastMachines_MushroomBox(),
                    value: context.Config.FastMushroomBox,
                    setValue: value => context.Config.FastMushroomBox = value
                ),
                new CheatsOptionsCheckbox(
                    label: I18n.FastMachines_OilMaker(),
                    value: context.Config.FastOilMaker,
                    setValue: value => context.Config.FastOilMaker = value
                ),
                new CheatsOptionsCheckbox(
                    label: I18n.FastMachines_PreservesJar(),
                    value: context.Config.FastPreservesJar,
                    setValue: value => context.Config.FastPreservesJar = value
                ),
                new CheatsOptionsCheckbox(
                    label: I18n.FastMachines_RecyclingMachine(),
                    value: context.Config.FastRecyclingMachine,
                    setValue: value => context.Config.FastRecyclingMachine = value
                ),
                new CheatsOptionsCheckbox(
                    label: I18n.FastMachines_SeedMaker(),
                    value: context.Config.FastSeedMaker,
                    setValue: value => context.Config.FastSeedMaker = value
                ),
                new CheatsOptionsCheckbox(
                    label: I18n.FastMachines_SlimeEggPress(),
                    value: context.Config.FastSlimeEggPress,
                    setValue: value => context.Config.FastSlimeEggPress = value
                ),
                new CheatsOptionsCheckbox(
                    label: I18n.FastMachines_SlimeIncubator(),
                    value: context.Config.FastSlimeIncubator,
                    setValue: value => context.Config.FastSlimeIncubator = value
                ),
                new CheatsOptionsCheckbox(
                    label: I18n.FastMachines_SodaMachine(),
                    value: context.Config.FastSodaMachine,
                    setValue: value => context.Config.FastSodaMachine = value
                ),
                new CheatsOptionsCheckbox(
                    label: I18n.FastMachines_StatueOfEndlessFortune(),
                    value: context.Config.FastStatueOfEndlessFortune,
                    setValue: value => context.Config.FastStatueOfEndlessFortune = value
                ),
                new CheatsOptionsCheckbox(
                    label: I18n.FastMachines_StatueOfPerfection(),
                    value: context.Config.FastStatueOfPerfection,
                    setValue: value => context.Config.FastStatueOfPerfection = value
                ),
                new CheatsOptionsCheckbox(
                    label: I18n.FastMachines_Tapper(),
                    value: context.Config.FastTapper,
                    setValue: value => context.Config.FastTapper = value
                ),
                new CheatsOptionsCheckbox(
                    label: I18n.FastMachines_WoodChipper(),
                    value: context.Config.FastWoodChipper,
                    setValue: value => context.Config.FastWoodChipper = value
                ),
                new CheatsOptionsCheckbox(
                    label: I18n.FastMachines_WormBin(),
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
                foreach (SObject obj in location.objects.Values)
                {
                    if (this.IsFastMachine(context, obj))
                        this.CompleteMachine(location, obj);
                }

                if (context.Config.FastFruitTree)
                {
                    foreach (TerrainFeature terrainFeature in location.terrainFeatures.Values)
                    {
                        if (terrainFeature is FruitTree tree && tree.growthStage.Value >= FruitTree.treeStage && tree.fruitsOnTree.Value < FruitTree.maxFruitsOnTrees)
                            tree.fruitsOnTree.Value = FruitTree.maxFruitsOnTrees;
                    }
                }
            }
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Get whether an object is a machine with 'fast processing' enabled.</summary>
        /// <param name="context">The cheat context.</param>
        /// <param name="obj">The machine to check.</param>
        private bool IsFastMachine(CheatContext context, SObject obj)
        {
            // quick initial check
            bool mayBeMachine = obj != null && (obj.bigCraftable.Value || obj is CrabPot);
            if (!mayBeMachine)
                return false;

            // specific check
            ModConfig config = context.Config;
            return obj switch
            {
                Cask _ => config.FastCask,
                CrabPot _ => config.FastCrabPot,
                WoodChipper _ => config.FastWoodChipper,
                _ => obj.name switch
                {
                    "Bee House" => config.FastBeeHouse,
                    "Charcoal Kiln" => config.FastCharcoalKiln,
                    "Cheese Press" => config.FastCheesePress,
                    "Crystalarium" => config.FastCrystalarium,
                    "Furnace" => config.FastFurnace,
                    "Incubator" => config.FastIncubator,
                    "Keg" => config.FastKeg,
                    "Lightning Rod" => config.FastLightningRod,
                    "Loom" => config.FastLoom,
                    "Mayonnaise Machine" => config.FastMayonnaiseMachine,
                    "Mushroom Box" => config.FastMushroomBox,
                    "Oil Maker" => config.FastOilMaker,
                    "Preserves Jar" => config.FastPreservesJar,
                    "Recycling Machine" => config.FastRecyclingMachine,
                    "Seed Maker" => config.FastSeedMaker,
                    "Slime Egg-Press" => config.FastSlimeEggPress,
                    "Slime Incubator" => config.FastSlimeIncubator,
                    "Soda Machine" => config.FastSodaMachine,
                    "Statue Of Endless Fortune" => config.FastStatueOfEndlessFortune,
                    "Statue Of Perfection" => config.FastStatueOfPerfection,
                    "Tapper" => config.FastTapper,
                    "Worm Bin" => config.FastWormBin,
                    _ => false
                }
            };
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
                        cask.daysToMature.Value = 0;
                        cask.checkForMaturity();
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
                        case "Soda Machine":
                        case "Statue Of Endless Fortune":
                        case "Statue Of Perfection":
                            if (!hasItem)
                            {
                                machine.DayUpdate(location);
                                machine.minutesElapsed(1, location);
                            }
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
