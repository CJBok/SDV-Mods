using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CJBItemSpawner
{
    public class CJBItemSpawner : Mod {
        public override void Entry(params object[] objects) {
            ControlEvents.KeyPressed += Events_KeyPressed;
        }

        private void Events_KeyPressed(object sender, EventArgsKeyPressed e) {
            if (e.KeyPressed == Microsoft.Xna.Framework.Input.Keys.I) {
                if (Game1.hasLoadedGame && Game1.activeClickableMenu == null && Game1.player.CanMove && !Game1.dialogueUp && !Game1.eventUp) {
                    ItemMenu.Open();
                }
            }
        }
    }
}
