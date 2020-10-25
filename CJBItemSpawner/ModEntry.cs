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

            // read data
            this.ItemData = helper.Data.ReadJsonFile<ModItemData>("assets/item-data.json");
            if (this.ItemData?.ProblematicItems == null)
                this.Monitor.Log("One of the mod files (assets/item-data.json) is missing or invalid. Some features may not work correctly; consider reinstalling the mod.", LogLevel.Warn);

            // init translations
            I18n.Init(helper.Translation);

            // hook events
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
                Game1.activeClickableMenu = new ItemMenu(this.GetSpawnableItems());
        }

        /// <summary>Get the items which can be spawned.</summary>
        private SearchableItem[] GetSpawnableItems()
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

            return items.ToArray();
        }
    }
}
