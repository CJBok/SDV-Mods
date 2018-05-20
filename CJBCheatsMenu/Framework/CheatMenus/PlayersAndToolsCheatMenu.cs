using System;
using System.Collections.Generic;

namespace CJBCheatsMenu.Framework.CheatMenus
{
    /// <summary>
    /// A cheat menu for players and tools options.
    /// </summary>
    internal class PlayersAndToolsCheatMenu : CheatMenu
    {
        /// <summary>
        /// Constructs a cheat menu for players and tools options.
        /// </summary>
        /// <param name="config">The user defined preferences.</param>
        /// <param name="cheats">Helper module that has various cheat utilities.</param>
        /// <param name="i18n">Helper module for internationalization.</param>
        public PlayersAndToolsCheatMenu(ModConfig config, Cheats cheats, StardewModdingAPI.ITranslationHelper I18n)
            : base(config, cheats, I18n)
        {
        }

        /// <summary>
        /// Unique id for the cheat menu.
        /// </summary>
        public override string Id => "CBJCheatsMenu_PlayersAndToolsCheatMenu";

        /// <summary>
        /// The title of the cheat menu (used for tab name).
        /// </summary>
        public override string Title => I18n.Get("tabs.player-and-tools");

        /// <summary>
        /// The options rendered within this cheat menu.
        /// </summary>
        public override List<Menu.IOptionGroup> OptionGroups
        {
            get
            {
                List<Menu.IOptionGroup> optionGroups = new List<Menu.IOptionGroup>();
                Menu.OptionGroup playerOptionGroup = new Menu.OptionGroup($"{I18n.Get("player.title")}:");
                playerOptionGroup.Options.Add(new Menu.OptionCheckbox(I18n.Get("player.infinite-stamina"), Config.InfiniteStamina, value => Config.InfiniteStamina = value));
                playerOptionGroup.Options.Add(new Menu.OptionCheckbox(I18n.Get("player.infinite-health"), Config.InfiniteHealth, value => Config.InfiniteHealth = value));
                playerOptionGroup.Options.Add(new Menu.OptionCheckbox(I18n.Get("player.increased-movement-speed"), Config.IncreasedMovement, value => Config.IncreasedMovement = value));
                playerOptionGroup.Options.Add(new SpeedSlider(this.Config, this.I18n));
                playerOptionGroup.Options.Add(new Menu.OptionCheckbox(I18n.Get("player.one-hit-kill"), Config.OneHitKill, value => Config.OneHitKill = value));
                playerOptionGroup.Options.Add(new Menu.OptionCheckbox(I18n.Get("player.max-daily-luck"), Config.MaxDailyLuck, value => Config.MaxDailyLuck = value));
                optionGroups.Add(playerOptionGroup);

                Menu.OptionGroup toolsOptionGroup = new Menu.OptionGroup($"{I18n.Get("tools.title")}:");
                toolsOptionGroup.Options.Add(new Menu.OptionCheckbox(I18n.Get("tools.infinite-water"), Config.InfiniteWateringCan, value => Config.InfiniteWateringCan = value));
                toolsOptionGroup.Options.Add(new Menu.OptionCheckbox(I18n.Get("tools.one-hit-break"), Config.OneHitBreak, value => Config.OneHitBreak = value));
                toolsOptionGroup.Options.Add(new Menu.OptionCheckbox(I18n.Get("tools.harvest-with-sickle"), Config.HarvestSickle, value => Config.HarvestSickle = value));
                optionGroups.Add(toolsOptionGroup);

                Menu.OptionGroup moneyOptionGroup = new Menu.OptionGroup($"{I18n.Get("money.title")}:");
                moneyOptionGroup.Options.Add(CreateAddMoneyButton(100));
                moneyOptionGroup.Options.Add(CreateAddMoneyButton(1000));
                moneyOptionGroup.Options.Add(CreateAddMoneyButton(10000));
                moneyOptionGroup.Options.Add(CreateAddMoneyButton(100000));
                optionGroups.Add(moneyOptionGroup);

                Menu.OptionGroup casinoCoinsGroup = new Menu.OptionGroup($"{I18n.Get("casino-coins.title")}:");
                casinoCoinsGroup.Options.Add(CreateAddCasinoCoinsButton(100));
                casinoCoinsGroup.Options.Add(CreateAddCasinoCoinsButton(1000));
                casinoCoinsGroup.Options.Add(CreateAddCasinoCoinsButton(10000));
                optionGroups.Add(casinoCoinsGroup);

                return optionGroups;
            }
        }

        /// <summary>
        /// Creates a button that, when pressed, adds some amount of money to the players wallet.
        /// </summary>
        /// <param name="amount">The amound of money to add.</param>
        /// <returns>The button that when pressed, will add money to the players wallet.</returns>
        private Menu.OptionSetButton<int> CreateAddMoneyButton(int amount)
        {
            Action<int> addMoneyAction = new Action<int>(amountToAdd =>
            {
                StardewValley.Game1.player.Money += amountToAdd;
                StardewValley.Game1.soundBank.PlayCue("coin");
            });

            return new Menu.OptionSetButton<int>(I18n.Get("money.add-amount", new { amount }), amount, addMoneyAction);
        }

        /// <summary>
        /// Creates a button that adds casino coins to the players coin wallet.
        /// </summary>
        /// <param name="amount">The amount of coins to add.</param>
        /// <returns>The button that when pressed, will add casino coins to the wallet.</returns>
        private Menu.OptionSetButton<int> CreateAddCasinoCoinsButton(int amount)
        {
            Action<int> addCasinoCoinsAction = new Action<int>(amountToAdd =>
            {
                StardewValley.Game1.player.clubCoins += amountToAdd;
                StardewValley.Game1.soundBank.PlayCue("coin");
            });

            return new Menu.OptionSetButton<int>(I18n.Get("casino-coins.add-amount", new { amount }), amount, addCasinoCoinsAction);
        }

        /// <summary>
        /// A slider that controls the characters speed
        /// </summary>
        private class SpeedSlider : Menu.IOptionSlider
        {
            public SpeedSlider(ModConfig config, StardewModdingAPI.ITranslationHelper i18n)
            {
                this.Config = config;
                this.I18n = i18n;
            }

            private ModConfig Config { get; set; }

            private StardewModdingAPI.ITranslationHelper I18n { get; set; }

            public int MinValue => 0;

            public int MaxValue => 10;

            public int Step => 1;

            public int Value
            {
                get
                {
                    return Config.MoveSpeed;
                }
                set
                {
                    Config.MoveSpeed = value;
                }
            }

            public string Label => this.I18n.Get("player.movement-speed");

            public bool Disabled => !Config.IncreasedMovement;

            public string ConvertValueToString(int value)
            {
                return value.ToString();
            }
        }
    }
}
