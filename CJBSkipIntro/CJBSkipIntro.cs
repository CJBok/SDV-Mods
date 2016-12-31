using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using System;

namespace CJBSkipIntro
{
    public class CJBSkipIntro : Mod {
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper) {
            GameEvents.UpdateTick += Events_UpdateTick;
        }

        public bool loadMenu = false;
        private void Events_UpdateTick(object sender, EventArgs e) {
            if (Game1.activeClickableMenu is TitleMenu && !loadMenu) {
                Game1.activeClickableMenu.receiveKeyPress(Microsoft.Xna.Framework.Input.Keys.Escape);
                (Game1.activeClickableMenu as TitleMenu).performButtonAction("Load");
                loadMenu = true;
            }
        }
    }
}
