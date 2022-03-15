using System;
using System.Linq;
using CJB.Common;
using CJBCheatsMenu.Framework;
using CJBCheatsMenu.Framework.Models;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;

namespace CJBCheatsMenu
{
    /// <summary>The mod entry point.</summary>
    internal class ModEntry : Mod
    {
        /*********
        ** Fields
        *********/
        /// <summary>The mod settings.</summary>
        private ModConfig Config;

        /// <summary>The warps to show in the menu.</summary>
        private ModData Warps;

        /// <summary>Manages the cheat implementations.</summary>
        private PerScreen<CheatManager> Cheats;

        /// <summary>The known in-game location.</summary>
        private readonly PerScreen<Lazy<GameLocation[]>> Locations = new(ModEntry.GetLocationsForCache);


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            // load config
            this.Config = helper.ReadConfig<ModConfig>();
            this.Monitor.Log($"Started with menu key {this.Config.OpenMenuKey}.");

            // init translations
            I18n.Init(helper.Translation);

            // load warps
            try
            {
                this.Warps = helper.Data.ReadJsonFile<ModData>("assets/warps.json");
                if (this.Warps == null)
                {
                    this.Monitor.Log("Some of the mod files are missing (assets/warps.json); try reinstalling this mod.", LogLevel.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                this.Monitor.Log($"Some of the mod files are broken or corrupted (assets/warps.json); try reinstalling this mod.\n{ex}", LogLevel.Error);
            }

            // load cheats
            this.ResetLocationCache();
            this.Cheats = new PerScreen<CheatManager>(() => new CheatManager(this.Config, this.Helper.Reflection, () => this.Locations.Value.Value, this.Warps));

            // hook events
            helper.Events.Display.Rendered += this.OnRendered;
            helper.Events.Display.MenuChanged += this.OnMenuChanged;

            helper.Events.GameLoop.ReturnedToTitle += this.OnReturnedToTitle;
            helper.Events.GameLoop.SaveLoaded += this.OnSaveLoaded;
            helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;

            helper.Events.Input.ButtonsChanged += this.OnButtonChanged;

            helper.Events.World.LocationListChanged += this.OnLocationListChanged;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Raised after the player loads a save slot.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            this.ResetLocationCache();
            this.Cheats.Value.OnSaveLoaded();
        }

        /// <summary>Raised after the player returns to the title screen.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnReturnedToTitle(object sender, ReturnedToTitleEventArgs e)
        {
            this.ResetLocationCache();
        }

        /// <summary>Raised after a game location is added or removed.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnLocationListChanged(object sender, LocationListChangedEventArgs e)
        {
            this.ResetLocationCache();
        }

        /// <summary>Raised after the player presses or releases any keys on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnButtonChanged(object sender, ButtonsChangedEventArgs e)
        {
            // open menu
            if (this.Config.OpenMenuKey.JustPressed())
            {
                if (Context.IsPlayerFree && Game1.currentMinigame == null)
                    Game1.activeClickableMenu = new CheatsMenu(this.Config.DefaultTab, this.Cheats.Value, this.Monitor, isNewMenu: true);
                else if (Game1.activeClickableMenu is CheatsMenu menu)
                    menu.ExitIfValid();
            }

            // handle button if applicable
            this.Cheats.Value.OnButtonsChanged(e);
        }

        /// <summary>Raised after the game draws to the sprite patch in a draw tick, just before the final sprite batch is rendered to the screen.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnRendered(object sender, RenderedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            this.Cheats.Value.OnRendered();
        }

        /// <summary>Raised after the game state is updated (≈60 times per second).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            this.Cheats.Value.OnUpdateTicked(e);
        }

        /// <summary>Raised after a game menu is opened, closed, or replaced.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnMenuChanged(object sender, MenuChangedEventArgs e)
        {
            // save config
            if (e.OldMenu is CheatsMenu)
            {
                this.Helper.WriteConfig(this.Config);
                this.Cheats.Value.OnOptionsChanged();
            }
        }

        /// <summary>Reset the cached location list.</summary>
        private void ResetLocationCache()
        {
            if (this.Locations.Value.IsValueCreated)
                this.Locations.Value = ModEntry.GetLocationsForCache();
        }

        /// <summary>Get a cached lookup of available locations.</summary>
        private static Lazy<GameLocation[]> GetLocationsForCache()
        {
            return new(() => Context.IsWorldReady
                ? CommonHelper.GetAllLocations().ToArray()
                : new GameLocation[0]
            );
        }
    }
}
