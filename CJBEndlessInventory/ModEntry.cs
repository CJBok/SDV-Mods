using System;
using System.IO;
using CJBEndlessInventory.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace CJBEndlessInventory
{
    internal class ModEntry : Mod
    {
        /*********
        ** Properties
        *********/
        private string StorageFilePath => $"data/{Constants.SaveFolderName}/inventory.json";


        /*********
        ** Accessors
        *********/
        internal static StorageItems StorageItems { get; set; }
        internal static ModConfig Settings { get; set; }


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            SaveEvents.AfterLoad += this.SaveEvents_AfterLoad;
            SaveEvents.AfterSave += this.SaveEvents_AfterSave;
            ControlEvents.KeyPressed += this.ControlEvents_KeyPressed;

            ModEntry.Settings = helper.ReadConfig<ModConfig>();
        }

        /*********
        ** Private methods
        *********/
        private void SaveEvents_AfterSave(object sender, EventArgs eventArgs)
        {
            this.Helper.WriteJsonFile(this.StorageFilePath, ModEntry.StorageItems);
        }

        private void ControlEvents_KeyPressed(object sender, EventArgsKeyPressed e)
        {
            if (e.KeyPressed.ToString() == ModEntry.Settings.MenuButton && Context.IsPlayerFree)
                ItemMenu.Open();
        }

        private void SaveEvents_AfterLoad(object sender, EventArgs e)
        {
            ModEntry.StorageItems = new StorageItems();
            if (File.Exists(this.StorageFilePath))
                ModEntry.StorageItems = this.Helper.ReadJsonFile<StorageItems>(this.StorageFilePath);
        }
    }
}
