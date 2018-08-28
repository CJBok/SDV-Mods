using System;
using System.Linq;
using CJBCheatsMenu.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace CJBCheatsMenu
{
    internal class ModEntry : Mod
    {
        /*********
        ** Properties
        *********/
        private GameLocation[] Locations;
        private bool IsLoaded => Context.IsWorldReady && this.Locations != null;

        /// <summary>The mod settings.</summary>
        private ModConfig Config;

        /// <summary>The cheats helper.</summary>
        private Cheats Cheats;


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            this.Config = helper.ReadConfig<ModConfig>();
            this.Monitor.Log($"Started with menu key {this.Config.OpenMenuKey}.", LogLevel.Trace);

            this.Cheats = new Cheats(this.Config);

            SaveEvents.AfterLoad += this.SaveEvents_AfterLoad;

            LocationEvents.LocationsChanged += this.LocationEvents_LocationsChanged;

            GameEvents.UpdateTick += this.Events_UpdateTick;
            GameEvents.OneSecondTick += this.GameEvents_OneSecondTick;

            InputEvents.ButtonPressed += this.Events_ButtonPressed;

            GraphicsEvents.OnPostRenderEvent += this.GraphicsEvents_DrawTick;

            MenuEvents.MenuClosed += this.MenuEvents_MenuClosed;
        }


        /*********
        ** Private methods
        *********/
        private void SaveEvents_AfterLoad(object sender, EventArgs eventArgs)
        {
            this.Locations = CJB.GetAllLocations().ToArray();
            this.Cheats.Reset();
        }

        private void LocationEvents_LocationsChanged(object sender, EventArgsLocationsChanged e)
        {
            this.Locations = CJB.GetAllLocations().ToArray();
        }

        private void GameEvents_OneSecondTick(object sender, EventArgs e)
        {
            if (!this.IsLoaded)
                return;

            this.Cheats.OneSecondUpdate(this.Locations);
        }

        private void Events_ButtonPressed(object sender, EventArgsInput e)
        {
            if (!this.IsLoaded || !Context.IsPlayerFree)
                return;

            if (e.Button == this.Config.OpenMenuKey)
                this.OpenMenu();
            else
                this.Cheats.OnButtonPress(e);
        }

        private void GraphicsEvents_DrawTick(object sender, EventArgs e)
        {
            if (!this.IsLoaded)
                return;

            this.Cheats.OnDrawTick(this.Helper.Translation);
        }

        private void Events_UpdateTick(object sender, EventArgs e)
        {
            if (!this.IsLoaded)
                return;

            this.Cheats.OnUpdate(this.Helper);
        }

        private void MenuEvents_MenuClosed(object sender, EventArgsClickableMenuClosed e)
        {
            if (e.PriorMenu is CheatsMenu)
                this.SaveConfig();
        }

        /// <summary>Open the cheats menu.</summary>
        private void OpenMenu()
        {
            if (Game1.activeClickableMenu != null)
                Game1.exitActiveMenu();
            Game1.activeClickableMenu = new CheatsMenu(this.Config.DefaultTab, this.Config, this.Cheats, this.Helper.Translation);
        }

        /// <summary>Update the mod's config.json file from the current <see cref="Config"/>.</summary>
        private void SaveConfig()
        {
            this.Helper.WriteConfig(this.Config);
            this.Cheats.OnOptionsChanged();
        }
    }
}
