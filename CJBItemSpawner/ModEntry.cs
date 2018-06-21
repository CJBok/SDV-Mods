using CJBItemSpawner.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace CJBItemSpawner
{
    /// <summary>The mod entry point.</summary>
    internal class ModEntry : Mod
    {
        /*********
        ** Public methods
        *********/
        /// <summary>The mod settings.</summary>
        private ModConfig Config;


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

            // hook events
            ControlEvents.KeyPressed += this.Events_KeyPressed;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>The method invoked when the player presses a keyboard button.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void Events_KeyPressed(object sender, EventArgsKeyPressed e)
        {
            if (!Context.IsPlayerFree)
                return;

            if (e.KeyPressed == this.Config.ShowMenuKey)
                Game1.activeClickableMenu = new ItemMenu(this.Helper.Translation);
        }
    }
}
