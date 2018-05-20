using System;
using System.Collections.Generic;
using System.Linq;

namespace CJBCheatsMenu.Framework.CheatMenus
{
    /// <summary>
    /// A cheat menu for farm and fishing settings.
    /// </summary>
    class FarmAndFishingCheatMenu : CheatMenu
    {
        /// <summary>
        /// Constructs a cheat menu for farm and fishing settings.
        /// </summary>
        /// <param name="config">The user defined preferences.</param>
        /// <param name="cheats">Helper module that has various cheat utilities.</param>
        /// <param name="i18n">Helper module for internationalization.</param>
        public FarmAndFishingCheatMenu(ModConfig config, Cheats cheats, StardewModdingAPI.ITranslationHelper i18n)
            : base(config, cheats, i18n)
        {
        }

        /// <summary>
        /// Unique id for the cheat menu.
        /// </summary>
        public override string Id => "CBJCheatsMenu_FarmAndFishingCheatMenu";

        /// <summary>
        /// The title of the cheat menu (used for tab name).
        /// </summary>
        public override string Title => I18n.Get("tabs.farm-and-fishing");

        /// <summary>
        /// The options rendered within this cheat menu.
        /// </summary>
        public override List<Menu.IOptionGroup> OptionGroups
        {
            get
            {
                List<Menu.IOptionGroup> optionGroups = new List<Menu.IOptionGroup>();
                Action<bool> waterAllFieldsAction = new Action<bool>(unused => 
                {
                    StardewValley.Game1.soundBank.PlayCue("glug");
                    Cheats.WaterAllFields(CJB.GetAllLocations().ToArray());
                });

                Menu.IOptionGroup farmGroup = new Menu.OptionGroup($"{I18n.Get("farm.title")}:");
                farmGroup.Options.Add(new Menu.OptionSetButton<bool>(I18n.Get("farm.water-all-fields"), true, waterAllFieldsAction));
                farmGroup.Options.Add(new Menu.OptionCheckbox(I18n.Get("farm.durable-fences"), Config.DurableFences, value => Config.DurableFences = value));
                farmGroup.Options.Add(new Menu.OptionCheckbox(I18n.Get("farm.instant-build"), Config.InstantBuild, value => Config.InstantBuild = value));
                farmGroup.Options.Add(new Menu.OptionCheckbox(I18n.Get("farm.always-auto-feed"), Config.AutoFeed, value => Config.AutoFeed = value));
                farmGroup.Options.Add(new Menu.OptionCheckbox(I18n.Get("farm.infinite-hay"), Config.InfiniteHay, value => Config.InfiniteHay = value));
                optionGroups.Add(farmGroup);

                Menu.IOptionGroup fishingGroup = new Menu.OptionGroup($"{I18n.Get("fishing.title")}:");
                fishingGroup.Options.Add(new Menu.OptionCheckbox(I18n.Get("fishing.instant-catch"), Config.InstantCatch, value => Config.InstantCatch = value));
                fishingGroup.Options.Add(new Menu.OptionCheckbox(I18n.Get("fishing.instant-bite"), Config.InstantBite, value => Config.InstantBite = value));
                fishingGroup.Options.Add(new Menu.OptionCheckbox(I18n.Get("fishing.always-throw-max-distance"), Config.ThrowBobberMax, value => Config.ThrowBobberMax = value));
                fishingGroup.Options.Add(new Menu.OptionCheckbox(I18n.Get("fishing.always-treasure"), Config.AlwaysTreasure, value => Config.AlwaysTreasure = value));
                fishingGroup.Options.Add(new Menu.OptionCheckbox(I18n.Get("fishing.durable-tackles"), Config.DurableTackles, value => Config.DurableTackles = value));
                optionGroups.Add(fishingGroup);

                Menu.OptionGroup fastMachinesGroup = new Menu.OptionGroup($"{I18n.Get("fast-machines.title")}:");
                fastMachinesGroup.Options.Add(new Menu.OptionCheckbox(I18n.Get("fast-machines.cask"), Config.FastCask, value => Config.FastCask = value));
                fastMachinesGroup.Options.Add(new Menu.OptionCheckbox(I18n.Get("fast-machines.furnace"), Config.FastFurnace, value => Config.FastFurnace = value));
                fastMachinesGroup.Options.Add(new Menu.OptionCheckbox(I18n.Get("fast-machines.recycling-machine"), Config.FastRecyclingMachine, value => Config.FastRecyclingMachine = value));
                fastMachinesGroup.Options.Add(new Menu.OptionCheckbox(I18n.Get("fast-machines.crystalarium"), Config.FastCrystalarium, value => Config.FastCrystalarium = value));
                fastMachinesGroup.Options.Add(new Menu.OptionCheckbox(I18n.Get("fast-machines.incubator"), Config.FastIncubator, value => Config.FastIncubator = value));
                fastMachinesGroup.Options.Add(new Menu.OptionCheckbox(I18n.Get("fast-machines.slime-incubator"), Config.FastSlimeIncubator, value => Config.FastSlimeIncubator = value));
                fastMachinesGroup.Options.Add(new Menu.OptionCheckbox(I18n.Get("fast-machines.keg"), Config.FastKeg, value => Config.FastKeg = value));
                fastMachinesGroup.Options.Add(new Menu.OptionCheckbox(I18n.Get("fast-machines.preserves-jar"), Config.FastPreservesJar, value => Config.FastPreservesJar = value));
                fastMachinesGroup.Options.Add(new Menu.OptionCheckbox(I18n.Get("fast-machines.cheese-press"), Config.FastCheesePress, value => Config.FastCheesePress = value));
                fastMachinesGroup.Options.Add(new Menu.OptionCheckbox(I18n.Get("fast-machines.mayonnaise-machine"), Config.FastMayonnaiseMachine, value => Config.FastMayonnaiseMachine = value));
                fastMachinesGroup.Options.Add(new Menu.OptionCheckbox(I18n.Get("fast-machines.loom"), Config.FastLoom, value => Config.FastLoom = value));
                fastMachinesGroup.Options.Add(new Menu.OptionCheckbox(I18n.Get("fast-machines.oil-maker"), Config.FastOilMaker, value => Config.FastOilMaker = value));
                fastMachinesGroup.Options.Add(new Menu.OptionCheckbox(I18n.Get("fast-machines.seed-maker"), Config.FastSeedMaker, value => Config.FastSeedMaker = value));
                fastMachinesGroup.Options.Add(new Menu.OptionCheckbox(I18n.Get("fast-machines.charcoal-kiln"), Config.FastCharcoalKiln, value => Config.FastCharcoalKiln = value));
                fastMachinesGroup.Options.Add(new Menu.OptionCheckbox(I18n.Get("fast-machines.slime-egg-press"), Config.FastSlimeEggPress, value => Config.FastSlimeEggPress = value));
                fastMachinesGroup.Options.Add(new Menu.OptionCheckbox(I18n.Get("fast-machines.tapper"), Config.FastTapper, value => Config.FastTapper = value));
                fastMachinesGroup.Options.Add(new Menu.OptionCheckbox(I18n.Get("fast-machines.lightning-rod"), Config.FastLightningRod, value => Config.FastLightningRod = value));
                fastMachinesGroup.Options.Add(new Menu.OptionCheckbox(I18n.Get("fast-machines.bee-house"), Config.FastBeeHouse, value => Config.FastBeeHouse = value));
                fastMachinesGroup.Options.Add(new Menu.OptionCheckbox(I18n.Get("fast-machines.mushroom-box"), Config.FastMushroomBox, value => Config.FastMushroomBox = value));
                fastMachinesGroup.Options.Add(new Menu.OptionCheckbox(I18n.Get("fast-machines.worm-bin"), Config.FastWormBin, value => Config.FastWormBin = value));
                fastMachinesGroup.Options.Add(new Menu.OptionCheckbox(I18n.Get("fast-machines.fruit-trees"), Config.FastFruitTree, value => Config.FastFruitTree = value));
                optionGroups.Add(fastMachinesGroup);

                return optionGroups;
            }
        }
    }
}
