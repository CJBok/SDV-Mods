using System;
using System.Collections.Generic;
using System.Linq;
using CJBCheatsMenu.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu
{
    public class CJBCheatsMenu : Mod
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
            this.Cheats = new Cheats(this.Config);

            SaveEvents.AfterLoad += this.SaveEvents_AfterLoad;
            LocationEvents.LocationsChanged += this.LocationEvents_LocationsChanged;

            GameEvents.UpdateTick += this.Events_UpdateTick;
            GameEvents.OneSecondTick += this.GameEvents_OneSecondTick;
            TimeEvents.TimeOfDayChanged += this.TimeEvents_TimeOfDayChanged;

            ControlEvents.KeyPressed += this.Events_KeyPressed;
            ControlEvents.ControllerButtonPressed += this.ControlEvents_ControllerButtonPressed;

            GraphicsEvents.OnPostRenderEvent += this.GraphicsEvents_DrawTick;
        }

        /// <summary>Update the mod's config.json file from the current <see cref="Config"/>.</summary>
        internal void SaveConfig()
        {
            this.Helper.WriteConfig(this.Config);
            this.Cheats.OnOptionsChanged();
        }

        /*********
        ** Private methods
        *********/
        private void SaveEvents_AfterLoad(object sender, EventArgs eventArgs)
        {
            this.Locations = CJB.GetAllLocations().ToArray();
        }

        private void LocationEvents_LocationsChanged(object sender, EventArgsGameLocationsChanged e)
        {
            this.Locations = CJB.GetAllLocations().ToArray();
        }

        private void GameEvents_OneSecondTick(object sender, EventArgs e)
        {
            if (!this.IsLoaded)
                return;

            this.Cheats.OneSecondUpdate(this.Locations);
        }

        private void ControlEvents_ControllerButtonPressed(object sender, EventArgsControllerButtonPressed e)
        {
            if (!this.IsLoaded)
                return;

            if (e.ButtonPressed.ToString() == this.Config.OpenMenuKey)
            {
                if (Game1.activeClickableMenu == null && Game1.player.CanMove && !Game1.dialogueUp && !Game1.eventUp)
                    this.OpenMenu();
                return;
            }

            if (e.ButtonPressed.ToString() == this.Config.FreezeTimeKey)
            {
                if (Game1.activeClickableMenu == null)
                    this.Config.FreezeTime = !this.Config.FreezeTime;
                return;
            }

            if (Game1.activeClickableMenu is GameMenu menu)
            {
                IClickableMenu page = this.Helper.Reflection.GetPrivateValue<List<IClickableMenu>>(menu, "pages")[menu.currentTab];
                if (page is CheatsMenu)
                    page.receiveGamePadButton(e.ButtonPressed);
            }
        }

        private void Events_KeyPressed(object sender, EventArgsKeyPressed e)
        {
            if (!this.IsLoaded)
                return;

            if (e.KeyPressed.ToString() == this.Config.OpenMenuKey)
                this.OpenMenu();
            else
                this.Cheats.OnKeyPress(e.KeyPressed);
        }

        private void GraphicsEvents_DrawTick(object sender, EventArgs e)
        {
            if (!this.IsLoaded)
                return;

            this.Cheats.OnDrawTick();
        }

        private void TimeEvents_TimeOfDayChanged(object sender, EventArgsIntChanged e)
        {
            if (!this.IsLoaded)
                return;

            this.Cheats.OnTimeOfDayChanged();
        }

        private void Events_UpdateTick(object sender, EventArgs e)
        {
            if (!this.IsLoaded)
                return;

            this.Cheats.OnUpdate(this.Helper);
        }

        /// <summary>Open the cheats menu.</summary>
        private void OpenMenu()
        {
            if (Game1.activeClickableMenu != null)
                Game1.exitActiveMenu();
            Game1.activeClickableMenu = new CheatsMenu(this.Config.DefaultTab, this.Config, this.Cheats, this.SaveConfig);
        }
    }
}
