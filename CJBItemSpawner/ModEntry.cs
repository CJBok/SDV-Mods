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

        /// <summary>Provides methods for searching and constructing items.</summary>
        private ItemRepository ItemRepository;


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            // read config
            this.Config = helper.ReadConfig<ModConfig>();
            this.Monitor.Log($"Started with menu key {this.Config.ShowMenuKey}.", LogLevel.Trace);

            // init item repository
            ModData data = helper.Data.ReadJsonFile<ModData>("data.json");
            this.ItemRepository = this.GetItemRepository(data, this.Config.AllowProblematicItems);

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
                Game1.activeClickableMenu = new ItemMenu(this.Helper.Translation, this.ItemRepository);
        }

        /// <summary>Get an item repository.</summary>
        /// <param name="data">The predefined mod data.</param>
        /// <param name="allowProblematicItems">Whether to show items which may cause bugs or crashes when spawned.</param>
        private ItemRepository GetItemRepository(ModData data, bool allowProblematicItems)
        {
            // no filter needed
            if (data?.ProblematicItems == null && !allowProblematicItems)
            {
                this.Monitor.Log("One of the mod files (data.json) is missing or invalid. Some features may not work correctly; consider reinstalling the mod.", LogLevel.Warn);
                allowProblematicItems = true;
            }
            if (allowProblematicItems)
                return new ItemRepository(filter: item => true);

            // create with filter
            var problematicItems = new HashSet<Tuple<ItemType, int>>(
                data.ProblematicItems.Select(item => Tuple.Create(item.Type, item.ID))
            );
            return new ItemRepository(
                filter: item => !problematicItems.Contains(Tuple.Create(item.Type, item.ID))
            );
        }
    }
}
