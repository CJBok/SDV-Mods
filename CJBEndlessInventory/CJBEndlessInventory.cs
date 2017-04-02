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
        private static bool NewDay;

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
            PlayerEvents.LoadedGame += PlayerEvents_LoadedGame;
            ControlEvents.KeyPressed += ControlEvents_KeyPressed;
            GameEvents.UpdateTick += GameEvents_UpdateTick;

            CJBEndlessInventory.Settings = helper.ReadConfig<ModSettings>();
        }


        /*********
        ** Private methods
        *********/
        private void GameEvents_UpdateTick(object sender, EventArgs e)
        {
            if (Game1.newDay != CJBEndlessInventory.NewDay)
            {
                CJBEndlessInventory.NewDay = Game1.newDay;
                if (CJBEndlessInventory.NewDay == false)
                    this.Helper.WriteJsonFile(this.StorageFilePath, CJBEndlessInventory.StorageItems);
            }
        }

        private void ControlEvents_KeyPressed(object sender, EventArgsKeyPressed e)
        {
            if (e.KeyPressed.ToString() == CJBEndlessInventory.Settings.MenuButton)
            {
                if (Game1.hasLoadedGame && Game1.activeClickableMenu == null && Game1.player.CanMove && !Game1.dialogueUp && !Game1.eventUp)
                {
                    ItemMenu.Open();
                }
            }
        }

        private void PlayerEvents_LoadedGame(object sender, EventArgsLoadedGameChanged e)
        {
            CJBEndlessInventory.StorageItems = new StorageItems();
            if (File.Exists(this.StorageFilePath))
                CJBEndlessInventory.StorageItems = this.Helper.ReadJsonFile<StorageItems>(this.StorageFilePath);
        }
    }
}
