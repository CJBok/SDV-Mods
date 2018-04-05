using System;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;

namespace CJBSkipIntro
{
    internal class ModEntry : Mod
    {
        /*********
        ** Properties
        *********/
        private bool LoadMenu;


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            GameEvents.UpdateTick += Events_UpdateTick;
        }


        /*********
        ** Private methods
        *********/
        private void Events_UpdateTick(object sender, EventArgs e)
        {
            if (Game1.activeClickableMenu is TitleMenu menu && !this.LoadMenu)
            {
                menu.receiveKeyPress(Microsoft.Xna.Framework.Input.Keys.Escape);
                menu.performButtonAction("Load");
                this.LoadMenu = true;
            }
        }
    }
}
