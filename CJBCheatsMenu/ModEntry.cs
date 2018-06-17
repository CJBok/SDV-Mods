﻿using System;
using System.Collections.Generic;
using System.Linq;
using CJBCheatsMenu.Framework;
using CJBCheatsMenu.Framework.Menu;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu
{
    internal class TestMenu : Framework.Menu.IMenu
    {
        public TestMenu(string id, string title)
        {
            this.Id = id;
            this.Title = title;
        }
        public string Id { get; }
        public string Title { get; }

        public List<IOptionGroup> OptionGroups => new List<IOptionGroup>();
    }

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

        private MenuManager MenuManager = new MenuManager();


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            this.Config = helper.ReadConfig<ModConfig>();
            this.Cheats = new Cheats(this.Config);
            this.RegisterCheatMenus();

            SaveEvents.AfterLoad += this.SaveEvents_AfterLoad;
            LocationEvents.LocationsChanged += this.LocationEvents_LocationsChanged;

            GameEvents.UpdateTick += this.Events_UpdateTick;
            GameEvents.OneSecondTick += this.GameEvents_OneSecondTick;
            TimeEvents.TimeOfDayChanged += this.TimeEvents_TimeOfDayChanged;

            ControlEvents.KeyPressed += this.Events_KeyPressed;
            ControlEvents.ControllerButtonPressed += this.ControlEvents_ControllerButtonPressed;

            GraphicsEvents.OnPostRenderEvent += this.GraphicsEvents_DrawTick;

            MenuEvents.MenuClosed += this.MenuEvents_MenuChanged;
        }

        private void RegisterCheatMenus()
        {
            this.MenuManager.RegisterMenu(new Framework.CheatMenus.PlayersAndToolsCheatMenu(this.Config, this.Cheats, this.Helper.Translation));
            this.MenuManager.RegisterMenu(new Framework.CheatMenus.FarmAndFishingCheatMenu(this.Config, this.Cheats, this.Helper.Translation));
            this.MenuManager.RegisterMenu(new Framework.CheatMenus.SkillsCheatMenu(this.Config, this.Cheats, this.Helper.Translation));
            this.MenuManager.RegisterMenu(new Framework.CheatMenus.WeatherCheatMenu(this.Config, this.Cheats, this.Helper.Translation));
            this.MenuManager.RegisterMenu(new Framework.CheatMenus.RelationshipsCheatMenu(this.Config, this.Cheats, this.Helper.Translation));
            this.MenuManager.RegisterMenu(new Framework.CheatMenus.WarpLocationsCheatMenu(this.Config, this.Cheats, this.Helper.Translation));
            this.MenuManager.RegisterMenu(new Framework.CheatMenus.TimeCheatMenu(this.Config, this.Cheats, this.Helper.Translation));
            this.MenuManager.RegisterMenu(new Framework.CheatMenus.ControlsCheatsMenu(this.Config, this.Cheats, this.Helper.Translation));
        }

        public override object GetApi()
        {
            return this.MenuManager;
        }


        /*********
        ** Private methods
        *********/
        private void SaveEvents_AfterLoad(object sender, EventArgs eventArgs)
        {
            this.Locations = CJB.GetAllLocations().ToArray();
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

        private void ControlEvents_ControllerButtonPressed(object sender, EventArgsControllerButtonPressed e)
        {
            if (!this.IsLoaded)
                return;

            if (Context.IsPlayerFree)
            {
                if (e.ButtonPressed.ToString() == this.Config.OpenMenuKey)
                    this.OpenMenu();
                else if (e.ButtonPressed.ToString() == this.Config.FreezeTimeKey)
                    this.Config.FreezeTime = !this.Config.FreezeTime;
            }
            else if (Game1.activeClickableMenu is GameMenu menu)
            {
                IClickableMenu page = this.Helper.Reflection.GetField<List<IClickableMenu>>(menu, "pages").GetValue()[menu.currentTab];
                if (page is CheatsMenu)
                    page.receiveGamePadButton(e.ButtonPressed);
            }
        }

        private void Events_KeyPressed(object sender, EventArgsKeyPressed e)
        {
            if (!this.IsLoaded || !Context.IsPlayerFree)
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

            this.Cheats.OnDrawTick(this.Helper.Translation);
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

        private void MenuEvents_MenuChanged(object sender, EventArgsClickableMenuClosed e)
        {
            if (e.PriorMenu is CheatsMenu)
                this.SaveConfig();
        }

        /// <summary>Open the cheats menu.</summary>
        private void OpenMenu()
        {
            if (Game1.activeClickableMenu != null)
                Game1.exitActiveMenu();
            Game1.activeClickableMenu = new CheatsMenu(this.Config.DefaultTab, 0, this.MenuManager, this.Config, this.Helper.Translation);
        }

        /// <summary>Update the mod's config.json file from the current <see cref="Config"/>.</summary>
        private void SaveConfig()
        {
            this.Helper.WriteConfig(this.Config);
            this.Cheats.OnOptionsChanged();
        }
    }
}
