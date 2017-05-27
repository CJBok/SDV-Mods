using System;
using System.Collections.Generic;
using System.Linq;
using CJBCheatsMenu.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Locations;
using StardewValley.Menus;

namespace CJBCheatsMenu
{
    public class CJBCheatsMenu : Mod
    {
        /*********
        ** Properties
        *********/
        private static IModHelper Helper;
        private static GameLocation[] Locations;
        private static bool IsLoaded => Game1.hasLoadedGame && CJBCheatsMenu.Locations != null;


        /*********
        ** Accessors
        *********/
        internal static ModConfig Config;


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            CJBCheatsMenu.Helper = helper;
            CJBCheatsMenu.Config = helper.ReadConfig<ModConfig>();

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
        internal static void SaveConfig()
        {
            CJBCheatsMenu.Helper.WriteConfig(CJBCheatsMenu.Config);
            Cheats.OnOptionsChanged();
        }

        /// <summary>Get all game locations.</summary>
        internal static IEnumerable<GameLocation> GetAllLocations()
        {
            foreach (GameLocation location in Game1.locations)
            {
                // current location
                yield return location;

                // buildings
                if (location is BuildableGameLocation buildableLocation)
                {
                    foreach (Building building in buildableLocation.buildings)
                    {
                        if (building.indoors != null)
                            yield return building.indoors;
                    }
                }
            }
        }

        /*********
        ** Private methods
        *********/
        private void SaveEvents_AfterLoad(object sender, EventArgs eventArgs)
        {
            CJBCheatsMenu.Locations = CJBCheatsMenu.GetAllLocations().ToArray();
        }

        private void LocationEvents_LocationsChanged(object sender, EventArgsGameLocationsChanged e)
        {
            CJBCheatsMenu.Locations = CJBCheatsMenu.GetAllLocations().ToArray();
        }

        private void GameEvents_OneSecondTick(object sender, EventArgs e)
        {
            if (!CJBCheatsMenu.IsLoaded)
                return;

            Cheats.OneSecondUpdate(CJBCheatsMenu.Locations);
        }

        private void ControlEvents_ControllerButtonPressed(object sender, EventArgsControllerButtonPressed e)
        {
            if (!CJBCheatsMenu.IsLoaded)
                return;

            if (e.ButtonPressed.ToString() == CJBCheatsMenu.Config.OpenMenuKey)
            {
                if (Game1.activeClickableMenu == null && Game1.player.CanMove && !Game1.dialogueUp && !Game1.eventUp)
                    CheatsMenu.Open(CJBCheatsMenu.Config.DefaultTab);
                return;
            }

            if (e.ButtonPressed.ToString() == CJBCheatsMenu.Config.FreezeTimeKey)
            {
                if (Game1.activeClickableMenu == null)
                    CJBCheatsMenu.Config.FreezeTime = !CJBCheatsMenu.Config.FreezeTime;
                return;
            }

            if (Game1.activeClickableMenu is GameMenu menu)
            {
                IClickableMenu page = CJBCheatsMenu.Helper.Reflection.GetPrivateValue<List<IClickableMenu>>(menu, "pages")[menu.currentTab];
                if (page is CheatsMenu)
                    page.receiveGamePadButton(e.ButtonPressed);
            }
        }

        private void Events_KeyPressed(object sender, EventArgsKeyPressed e)
        {
            if (!CJBCheatsMenu.IsLoaded)
                return;

            Cheats.OnKeyPress(e.KeyPressed);
        }

        private void GraphicsEvents_DrawTick(object sender, EventArgs e)
        {
            if (!CJBCheatsMenu.IsLoaded)
                return;

            Cheats.OnDrawTick();
        }

        private void TimeEvents_TimeOfDayChanged(object sender, EventArgsIntChanged e)
        {
            if (!CJBCheatsMenu.IsLoaded)
                return;

            Cheats.OnTimeOfDayChanged();
        }

        private void Events_UpdateTick(object sender, EventArgs e)
        {
            if (!CJBCheatsMenu.IsLoaded)
                return;

            Cheats.OnUpdate(CJBCheatsMenu.Helper);
        }
    }
}
