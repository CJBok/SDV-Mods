using System;
using System.Diagnostics.CodeAnalysis;
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
        /// <summary>The relative file to the warps data file.</summary>
        private readonly string WarpsPath = "assets/warps.json";

        /// <summary>The mod settings.</summary>
        private ModConfig Config = null!; // set in Entry

        /// <summary>Manages building and loading the warp data assets.</summary>
        private WarpContentLoader WarpContentLoader = null!; // set in Entry

        /// <summary>Manages the cheat implementations.</summary>
        private PerScreen<CheatManager> Cheats = null!; // set in Entry

        /// <summary>The known in-game location.</summary>
        private readonly PerScreen<Lazy<GameLocation[]>> Locations = new(ModEntry.GetLocationsForCache);


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            CommonHelper.RemoveObsoleteFiles(this, "CJBCheatsMenu.pdb");

            // load config
            this.Config = helper.ReadConfig<ModConfig>();
            this.Monitor.Log($"Started with menu key {this.Config.OpenMenuKey}.");

            // init translations
            I18n.Init(helper.Translation);

            // init warp content loader
            this.WarpContentLoader = new WarpContentLoader(this.ModManifest.UniqueID, () => this.Config, this.LoadModData(), this.Monitor, this.Helper.ModRegistry);

            // load cheats
            this.ResetLocationCache();
            this.Cheats = new PerScreen<CheatManager>(() => new CheatManager(this.Config, this.Helper.GameContent, this.Helper.Reflection, this.WarpContentLoader, () => this.Locations.Value.Value));

            // hook events
            helper.Events.GameLoop.GameLaunched += this.OnGameLaunched;
            helper.Events.Content.AssetRequested += this.OnAssetRequested;

            helper.Events.Display.Rendered += this.OnRendered;
            helper.Events.Display.MenuChanged += this.OnMenuChanged;

            helper.Events.GameLoop.ReturnedToTitle += this.OnReturnedToTitle;
            helper.Events.GameLoop.SaveLoaded += this.OnSaveLoaded;
            helper.Events.GameLoop.Saving += this.OnSaving;
            helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;

            helper.Events.Input.ButtonsChanged += this.OnButtonChanged;

            helper.Events.World.LocationListChanged += this.OnLocationListChanged;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Raised after the game is launched, right before the first update tick. This happens once per game session (unrelated to loading saves). All mods are loaded and initialised at this point, so this is a good time to set up mod integrations.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
        {
            var configMenu = new GenericModConfigMenuIntegration(
                manifest: this.ModManifest,
                modRegistry: this.Helper.ModRegistry,
                config: this.Config,
                save: () => this.Helper.WriteConfig(this.Config)
            );
            configMenu.Register();
        }

        /// <summary>Raised after the player loads a save slot.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnSaveLoaded(object? sender, SaveLoadedEventArgs e)
        {
            this.ResetLocationCache();
            this.Cheats.Value.OnSaveLoaded();
        }

        /// <summary>Raised after the player returns to the title screen.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnReturnedToTitle(object? sender, ReturnedToTitleEventArgs e)
        {
            this.ResetLocationCache();
        }

        /// <summary>Raised after a game location is added or removed.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnLocationListChanged(object? sender, LocationListChangedEventArgs e)
        {
            this.ResetLocationCache();
        }

        /// <summary>Raised after the player presses or releases any keys on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnButtonChanged(object? sender, ButtonsChangedEventArgs e)
        {
            // reload config
            if (this.Config.ReloadConfigKey.JustPressed())
            {
                this.Config = this.Helper.ReadConfig<ModConfig>();
                this.Monitor.Log($"Reloaded config; menu key is {this.Config.OpenMenuKey}.");
            }

            // open menu
            if (this.Config.OpenMenuKey.JustPressed())
            {
                if (Game1.activeClickableMenu is CheatsMenu menu)
                    menu.ExitIfValid();

                else if (!Context.IsPlayerFree || Game1.currentMinigame != null)
                {
                    // Players often ask for help due to the menu not opening when expected. To
                    // simplify troubleshooting, log when the key is ignored.
                    if (Game1.currentMinigame != null)
                        this.Monitor.Log($"Received menu open key, but a '{Game1.currentMinigame.GetType().Name}' minigame is active.");
                    else if (Game1.eventUp)
                        this.Monitor.Log("Received menu open key, but an event is active.");
                    else if (Game1.activeClickableMenu != null)
                        this.Monitor.Log($"Received menu open key, but a '{Game1.activeClickableMenu.GetType().Name}' menu is already open.");
                    else
                        this.Monitor.Log("Received menu open key, but the player isn't free.");
                }

                else
                {
                    this.Monitor.Log("Received menu open key.");
                    CommonHelper.WarnOnGameMenuKeyConflict(this.Helper.Input, this.Monitor, this.Config.OpenMenuKey, "cheats menu");
                    Game1.activeClickableMenu = new CheatsMenu(this.Config.DefaultTab, this.Cheats.Value, this.Monitor, isNewMenu: true);
                }
            }

            // handle button if applicable
            if (Game1.keyboardDispatcher?.Subscriber is null)
                this.Cheats.Value.OnButtonsChanged(e);
        }

        /// <inheritdoc cref="IContentEvents.AssetRequested"/>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnAssetRequested(object? sender, AssetRequestedEventArgs e)
        {
            // apply cheats
            if (this.Cheats.IsActiveForScreen())
                this.Cheats.Value.HarvestWithScythe.OnAssetRequested(this.Cheats.Value.Context, e);

            // load warp sections
            this.WarpContentLoader.OnAssetRequested(e);
        }

        /// <summary>Raised after the game draws to the sprite patch in a draw tick, just before the final sprite batch is rendered to the screen.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnRendered(object? sender, RenderedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            this.Cheats.Value.OnRendered();
        }

        /// <summary>Raised after the game state is updated (≈60 times per second).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnUpdateTicked(object? sender, UpdateTickedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            this.Cheats.Value.OnUpdateTicked(e);
        }

        /// <inheritdoc cref="IGameLoopEvents.Saving"/>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnSaving(object? sender, SavingEventArgs e)
        {
            this.Cheats.Value.OnSaving();
        }

        /// <summary>Raised after a game menu is opened, closed, or replaced.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnMenuChanged(object? sender, MenuChangedEventArgs e)
        {
            // save config
            if (e.OldMenu is CheatsMenu)
            {
                this.Helper.WriteConfig(this.Config);
                this.Cheats.Value.OnOptionsChanged();
            }
        }

        /// <summary>Load the default warps from the data file, if it's valid.</summary>
        [SuppressMessage("ReSharper", "ConstantNullCoalescingCondition", Justification = "The warps field is initialized in this method.")]
        private ModData LoadModData()
        {
            try
            {
                ModData? modData = this.Helper.Data.ReadJsonFile<ModData>(this.WarpsPath);
                if (modData != null)
                    return modData;

                this.Monitor.Log($"Some of the mod files are missing ({this.WarpsPath}); try reinstalling this mod.", LogLevel.Error);
            }
            catch (Exception ex)
            {
                this.Monitor.Log($"Some of the mod files are broken or corrupted ({this.WarpsPath}); try reinstalling this mod.\n{ex}", LogLevel.Error);
            }

            return new ModData(null, null);
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
                : Array.Empty<GameLocation>()
            );
        }
    }
}
