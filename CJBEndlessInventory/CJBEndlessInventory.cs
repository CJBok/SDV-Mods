using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Input;

namespace CJBEndlessInventory
{
    public class CJBEndlessInventory : Mod
    {
        public static StorageItems storageItems { get; set; }
        public static ModSettings settings { get; set; }

        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper) {
            PlayerEvents.LoadedGame += PlayerEvents_LoadedGame;
            ControlEvents.KeyPressed += ControlEvents_KeyPressed;
            GameEvents.UpdateTick += GameEvents_UpdateTick;

            settings = helper.ReadConfig<ModSettings>();
        }

        public static bool newDay = false;
        private void GameEvents_UpdateTick(object sender, EventArgs e) {
            if (Game1.newDay != newDay) {
                newDay = Game1.newDay;
                if (newDay == false) {
                    XmlSerializer SerializerObj = new XmlSerializer(typeof(StorageItems));
                    TextWriter writeFileStream = new StreamWriter(PerSaveConfigPath);
                    SerializerObj.Serialize(writeFileStream, CJBEndlessInventory.storageItems);
                    writeFileStream.Close();
                }
            }
        }

        private void ControlEvents_KeyPressed(object sender, EventArgsKeyPressed e) {
            if (e.KeyPressed.ToString().Equals(settings.menuButton.ToString())) {
                if (Game1.hasLoadedGame && Game1.activeClickableMenu == null && Game1.player.CanMove && !Game1.dialogueUp && !Game1.eventUp) {
                    ItemMenu.Open();
                }
            }
        }

        private void PlayerEvents_LoadedGame(object sender, EventArgsLoadedGameChanged e) {
            storageItems = new StorageItems();
            if (File.Exists(PerSaveConfigPath)) {
                XmlSerializer SerializerObj = new XmlSerializer(typeof(StorageItems));
                FileStream readFileStream = new FileStream(PerSaveConfigPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                storageItems = (StorageItems)SerializerObj.Deserialize(readFileStream);
                readFileStream.Close();
            }
        }
    }

    [Serializable()]
    public class StorageItems {
        public List<Item> playerItems { get; set; }

        public StorageItems() {
            playerItems = new List<Item>();
        }
    }

    public class ModSettings {
        public string menuButton { get; set; } = Keys.Q.ToString();
    }
}
