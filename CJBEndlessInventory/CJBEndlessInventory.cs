using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace CJBEndlessInventory
{
    public class CJBEndlessInventory : Mod
    {
        public static StorageItems storageItems { get; set; }
        public static ModSettings settings { get; set; }
        

        public override void Entry(params object[] objects) {
            PlayerEvents.LoadedGame += PlayerEvents_LoadedGame;
            ControlEvents.KeyPressed += ControlEvents_KeyPressed;
            GameEvents.UpdateTick += GameEvents_UpdateTick;

            settings = new ModSettings().InitializeConfig(BaseConfigPath);
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

    public class ModSettings : Config {
        public string menuButton { get; set; }

        public override T GenerateDefaultConfig<T>() {
            menuButton = Microsoft.Xna.Framework.Input.Keys.Q.ToString();
            return this as T;
        }
    }
}
