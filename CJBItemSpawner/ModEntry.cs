using System;
using System.Collections.Generic;
using System.Linq;
using CJBItemSpawner.Framework;
using CJBItemSpawner.Framework.ItemData;
using CJBItemSpawner.Framework.Models;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace CJBItemSpawner
{
    /// <summary>The mod entry point.</summary>
    internal class ModEntry : Mod
    {
        /*********
        ** Fields
        *********/
        /// <summary>The mod settings.</summary>
        private ModConfig Config;

        /// <summary>The internal mod data about items.</summary>
        private ModItemData ItemData;

        /// <summary>The item category filters available in the item spawner menu.</summary>
        public ModDataCategory[] Categories { get; set; }


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            // read config
            this.Config = helper.ReadConfig<ModConfig>();
            this.Monitor.Log($"Started with menu key {this.Config.ShowMenuKey}.");

            // read item data
            this.ItemData = helper.Data.ReadJsonFile<ModItemData>("assets/item-data.json");
            if (this.ItemData?.ProblematicItems == null)
                this.Monitor.Log("One of the mod files (assets/item-data.json) is missing or invalid. Some features may not work correctly; consider reinstalling the mod.", LogLevel.Warn);

            // read categories
            this.Categories = helper.Data.ReadJsonFile<ModDataCategory[]>("assets/categories.json");
            if (this.Categories == null)
                this.Monitor.LogOnce("One of the mod files (assets/categories.json) is missing or invalid. Some features may not work correctly; consider reinstalling the mod.", LogLevel.Warn);

            // init mod
            I18n.Init(helper.Translation);
            helper.Events.Input.ButtonPressed += this.OnButtonPressed;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (!Context.IsPlayerFree)
                return;

            if (e.Button == this.Config.ShowMenuKey)
                Game1.activeClickableMenu = this.BuildMenu();
        }

        /// <summary>Build an item spawner menu.</summary>
        private ItemMenu BuildMenu()
        {
            SpawnableItem[] items = this.GetSpawnableItems().ToArray();
            string[] categoryLabels = this.GetCategories().ToArray();

            return new ItemMenu(items, categoryLabels);
        }

        /// <summary>Get the available filter category labels.</summary>
        private IEnumerable<string> GetCategories()
        {
            return this.Categories?.Select(p => (string)I18n.GetByKey(p.Label).Default(p.Label))
                ?? new string[0];
        }

        /// <summary>Get the items which can be spawned.</summary>
        private IEnumerable<SpawnableItem> GetSpawnableItems()
        {
            IEnumerable<SearchableItem> items = new ItemRepository().GetAll();

            // apply 'problematic items' filter
            if (!this.Config.AllowProblematicItems && this.ItemData?.ProblematicItems?.Any() == true)
            {
                var problematicItems = new HashSet<Tuple<ItemType, int>>(
                    this.ItemData.ProblematicItems.Select(item => Tuple.Create(item.Type, item.ID))
                );
                items = items.Where(item => !problematicItems.Contains(Tuple.Create(item.Type, item.ID)));
            }

            // yield models
            foreach (SearchableItem entry in items)
            {
                ModDataCategory category = this.Categories?.FirstOrDefault(rule => rule.IsMatch(entry.Item));
                string categoryLabel = category != null
                    ? I18n.GetByKey(category.Label).Default(category.Label)
                    : I18n.Tabs_Miscellaneous();

                yield return new SpawnableItem(entry, categoryLabel);
            }
        }
    }
}
