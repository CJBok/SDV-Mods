using System;
using System.Collections.Generic;
using System.Linq;
using CJB.Common;
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
            CommonHelper.RemoveObsoleteFiles(this, "CJBItemSpawner.pdb");

            // read config
            this.Config = helper.ReadConfig<ModConfig>();
            this.LogCustomConfig();

            // read item data
            {
                ModItemData? itemData = helper.Data.ReadJsonFile<ModItemData>("assets/item-data.json");
                if (itemData?.ForceSellable is null)
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
            if (this.Config.ShowMenuKey.JustPressed())
            {
                if (!Context.IsPlayerFree)
                {
                    // Players often ask for help due to the menu not opening when expected. To
                    // simplify troubleshooting, log when the key is ignored.
                    if (Game1.activeClickableMenu != null)
                        this.Monitor.Log($"Received menu open key, but a '{Game1.activeClickableMenu.GetType().Name}' menu is already open.");
                    else if (Game1.eventUp)
                        this.Monitor.Log("Received menu open key, but an event is active.");
                    else
                        this.Monitor.Log("Received menu open key, but the player isn't free.");

                    return;
                }

                this.Monitor.Log("Received menu open key.");
                Game1.activeClickableMenu = this.BuildMenu();
            }
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
            return new ItemMenu(items, this.TextEntryManager, this.ItemData, this.Helper.ModContent, this.Monitor, this.Config.ReclaimPriceInMenuTrashCan);
        }

        /// <summary>Get the items which can be spawned.</summary>
        private IEnumerable<SpawnableItem> GetSpawnableItems()
        {
            foreach (SearchableItem entry in new ItemRepository().GetAll())
            {
                ModDataCategory[] categories = Array.FindAll(this.Categories, rule => rule.IsMatch(entry));

                // should this be categories.All(...) (hide only if all matching categories are hidden)
                // or categories.Any(...) (hide if any matching categories are hidden)
                if (categories.All(cat => this.Config.HideCategories.Contains(cat.Label)))
                    continue;

                string[] categoryLabels = categories.Length != 0 ?
                    categories.Select(cat => I18n.GetByKey(cat.Label).Default(cat.Label).ToString()).ToArray()
                    : new []{I18n.Filter_Miscellaneous()};

                yield return new SpawnableItem(entry, categoryLabels);
            }
        }

        /// <summary>Log a trace message which summarizes the user's current config.</summary>
        private void LogCustomConfig()
        {
            List<string> phrases = new() { $"menu key {this.Config.ShowMenuKey}" };

            if (!this.Config.ReclaimPriceInMenuTrashCan)
                phrases.Add("reclaim trash can price disabled");

            if (this.Config.HideCategories.Any())
                phrases.Add($"hidden categories {string.Join(", ", this.Config.HideCategories)}");

            this.Monitor.Log($"Started with {string.Join(", ", phrases)}.");
        }
    }
}
