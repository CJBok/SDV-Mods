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
        private string storageFilePath => $"data/{Constants.SaveFolderName}/inventory.json";


        /*********
        ** Accessors
        *********/
        public static StorageItems storageItems { get; set; }
        public static ModSettings settings { get; set; }
        public static bool newDay = false;


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

            settings = helper.ReadConfig<ModSettings>();
        }


        /*********
        ** Private methods
        *********/
        private void GameEvents_UpdateTick(object sender, EventArgs e)
        {
            if (Game1.newDay != newDay)
            {
                newDay = Game1.newDay;
                if (newDay == false)
                    this.Helper.WriteJsonFile(this.storageFilePath, CJBEndlessInventory.storageItems);
            }
        }

        private void ControlEvents_KeyPressed(object sender, EventArgsKeyPressed e)
        {
            if (e.KeyPressed.ToString().Equals(settings.menuButton.ToString()))
            {
                if (Game1.hasLoadedGame && Game1.activeClickableMenu == null && Game1.player.CanMove && !Game1.dialogueUp && !Game1.eventUp)
                {
                    ItemMenu.Open();
                }
            }
        }

        private void PlayerEvents_LoadedGame(object sender, EventArgsLoadedGameChanged e)
        {
            storageItems = new StorageItems();
            if (File.Exists(this.storageFilePath))
                storageItems = this.Helper.ReadJsonFile<StorageItems>(this.storageFilePath);
        }
    }
}
