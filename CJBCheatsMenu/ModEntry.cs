using System.Linq;
using CJBCheatsMenu.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace CJBCheatsMenu
{
    /// <summary>The mod entry point.</summary>
    internal class ModEntry : Mod
    {
        /*********
        ** Fields
        *********/
        /// <summary>The known in-game location.</summary>
        private GameLocation[] Locations;

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

            helper.Events.Display.Rendered += this.OnRendered;
            helper.Events.Display.MenuChanged += this.OnMenuChanged;

            helper.Events.GameLoop.SaveLoaded += this.OnSaveLoaded;
            helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;

            helper.Events.Input.ButtonPressed += this.OnButtonPressed;

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
            this.Locations = CJB.GetAllLocations().ToArray();
            this.Cheats.Reset();
        }

        /// <summary>Raised after a game location is added or removed.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnLocationListChanged(object sender, LocationListChangedEventArgs e)
        {
            this.Locations = CJB.GetAllLocations().ToArray();
        }

        /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (!Context.IsPlayerFree || Game1.currentMinigame != null)
                return;

            // open menu
            if (e.Button == this.Config.OpenMenuKey)
                Game1.activeClickableMenu = new CheatsMenu(this.Config.DefaultTab, this.Config, this.Cheats, this.Helper.Translation, this.Monitor);

            // handle button if applicable
            else
                this.Cheats.OnButtonPressed(e);
        }

        /// <summary>Raised after the game draws to the sprite patch in a draw tick, just before the final sprite batch is rendered to the screen.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnRendered(object sender, RenderedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            this.Cheats.OnRendered(this.Helper.Translation);
        }

        /// <summary>Raised after the game state is updated (≈60 times per second).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            this.Cheats.OnUpdateTicked(e, this.Helper.Reflection);
            if (e.IsOneSecond)
                this.Cheats.OneSecondUpdate(this.Locations);
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
                this.Cheats.OnOptionsChanged();
            }
        }
    }
}
