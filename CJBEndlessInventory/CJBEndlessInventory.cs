using System;
using System.IO;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace CJBEndlessInventory
{
    public class CJBEndlessInventory : Mod
    {
        /*********
        ** Properties
        *********/
        private string StorageFilePath => $"data/{Constants.SaveFolderName}/inventory.json";


        /*********
        ** Accessors
        *********/
        internal static StorageItems StorageItems { get; set; }
        internal static ModSettings Settings { get; set; }


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

            CJBEndlessInventory.Settings = helper.ReadConfig<ModSettings>();
        }

        /*********
        ** Private methods
        *********/
        private void SaveEvents_AfterSave(object sender, EventArgs eventArgs)
        {
            this.Helper.WriteJsonFile(this.StorageFilePath, CJBEndlessInventory.StorageItems);
        }

        private void ControlEvents_KeyPressed(object sender, EventArgsKeyPressed e)
        {
            if (e.KeyPressed.ToString() == CJBEndlessInventory.Settings.MenuButton)
            {
                if (Context.IsWorldReady && Game1.activeClickableMenu == null && Game1.player.CanMove && !Game1.dialogueUp && !Game1.eventUp)
                    ItemMenu.Open();
            }
        }

        private void SaveEvents_AfterLoad(object sender, EventArgs e)
        {
            CJBEndlessInventory.StorageItems = new StorageItems();
            if (File.Exists(this.StorageFilePath))
                CJBEndlessInventory.StorageItems = this.Helper.ReadJsonFile<StorageItems>(this.StorageFilePath);
        }
    }
}
