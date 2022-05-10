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
        private ModConfig Config = null!; // set in Entry

        /// <summary>The internal mod data about items.</summary>
        private ModItemData ItemData = null!; // set in Entry

        /// <summary>The item category filters available in the item spawner menu.</summary>
        private ModDataCategory[] Categories = null!; // set in Entry

        /// <summary>Manages the gamepad text entry UI.</summary>
        private readonly TextEntryManager TextEntryManager = new();


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
            {
                ModItemData? itemData = helper.Data.ReadJsonFile<ModItemData>("assets/item-data.json");
                if (itemData?.ProblematicItems == null)
                    this.Monitor.Log("One of the mod files (assets/item-data.json) is missing or invalid. Some features may not work correctly; consider reinstalling the mod.", LogLevel.Warn);
                this.ItemData = itemData ?? new ModItemData(null);
            }

            // read categories
            {
                ModDataCategory[]? categories = helper.Data.ReadJsonFile<ModDataCategory[]>("assets/categories.json");
                if (categories == null)
                    this.Monitor.LogOnce("One of the mod files (assets/categories.json) is missing or invalid. Some features may not work correctly; consider reinstalling the mod.", LogLevel.Warn);
                this.Categories = categories ?? Array.Empty<ModDataCategory>();
            }

            // init mod
            I18n.Init(helper.Translation);
            helper.Events.Input.ButtonsChanged += this.OnButtonsChanged;
            helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Raised after the player presses or releases any buttons on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnButtonsChanged(object? sender, ButtonsChangedEventArgs e)
        {
            if (!Context.IsPlayerFree)
                return;

            if (this.Config.ShowMenuKey.JustPressed())
                Game1.activeClickableMenu = this.BuildMenu();
        }

        /// <summary>Raised after the game state is updated (≈60 times per second).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnUpdateTicked(object? sender, UpdateTickedEventArgs e)
        {
            this.TextEntryManager.Update();
        }

        /// <summary>Build an item spawner menu.</summary>
        private ItemMenu BuildMenu()
        {
            SpawnableItem[] items = this.GetSpawnableItems().ToArray();
            return new ItemMenu(items, this.TextEntryManager, this.Helper.ModContent, this.Monitor);
        }

        /// <summary>Get the items which can be spawned.</summary>
        private IEnumerable<SpawnableItem> GetSpawnableItems()
        {
            IEnumerable<SearchableItem> items = new ItemRepository().GetAll();

            // apply 'problematic items' filter
            if (!this.Config.AllowProblematicItems && this.ItemData.ProblematicItems.Any())
            {
                var problematicItems = new HashSet<string>(this.ItemData.ProblematicItems, StringComparer.OrdinalIgnoreCase);
                items = items.Where(item => !problematicItems.Contains($"{item.Type}:{item.ID}"));
            }

            // yield models
            foreach (SearchableItem entry in items)
            {
                ModDataCategory? category = this.Categories.FirstOrDefault(rule => rule.IsMatch(entry));
                string categoryLabel = category != null
                    ? I18n.GetByKey(category.Label).Default(category.Label)
                    : I18n.Filter_Miscellaneous();

                yield return new SpawnableItem(entry, categoryLabel);
            }
        }
    }
}
