using System;
using System.Collections.Generic;
using System.Linq;
using CJB.Common;
using CJB.Common.Integrations;
using CJBItemSpawner.Framework;
using CJBItemSpawner.Framework.ItemData;
using CJBItemSpawner.Framework.Models;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace CJBItemSpawner
{
    /// <summary>The mod entry point.</summary>
    internal class ModEntry : Mod
    {
        /*********
        ** Fields
        *********/
        /// <summary>The mod settings.</summary>
        private ModConfig Config = null!; // set in Entry

        /// <summary>The internal mod data about items.</summary>
        private ModItemData ItemData = null!; // set in Entry

        /// <summary>The item category filters available in the item spawner menu.</summary>
        private ModDataCategory[] Categories = null!; // set in Entry

        /// <summary>Manages the gamepad text entry UI.</summary>
        private readonly TextEntryManager TextEntryManager = new();


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            CommonHelper.RemoveObsoleteFiles(this, "CJBItemSpawner.pdb");

            // read config
            this.Config = helper.ReadConfig<ModConfig>();
            this.LogCustomConfig();

            // read item data
            {
                ModItemData? itemData = helper.Data.ReadJsonFile<ModItemData>("assets/item-data.json");
                if (itemData?.ForceSellable is null)
                    this.Monitor.Log("One of the mod files (assets/item-data.json) is missing or invalid. Some features may not work correctly; consider reinstalling the mod.", LogLevel.Warn);
                this.ItemData = itemData ?? new ModItemData(null);
            }

            // read categories
            {
                ModDataCategory[]? categories = helper.Data.ReadJsonFile<ModDataCategory[]>("assets/categories.json");
                if (categories == null)
                    this.Monitor.LogOnce("One of the mod files (assets/categories.json) is missing or invalid. Some features may not work correctly; consider reinstalling the mod.", LogLevel.Warn);
                this.Categories = categories ?? Array.Empty<ModDataCategory>();
            }

            // init mod
            I18n.Init(helper.Translation);
            helper.Events.GameLoop.GameLaunched += this.OnGameLaunched;
            helper.Events.Input.ButtonsChanged += this.OnButtonsChanged;
            helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Raised after the game is launched, right before the first update tick. This happens once per game session (unrelated to loading saves). All mods are loaded and initialised at this point, so this is a good time to set up mod integrations.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
        {
            // get Generic Mod Config Menu's API (if it's installed)
            var configMenu = this.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (configMenu is null)
                return;

            // register mod
            configMenu.Register(
                mod: this.ModManifest,
                reset: () => this.Config = new ModConfig(),
                save: () => this.Helper.WriteConfig(this.Config),
                titleScreenOnly: false
            );

            // ---------------------------- //
            // Main options
            // ---------------------------- //
            configMenu.AddSectionTitle(this.ModManifest, I18n.Config_Title_MainOptions);

            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: I18n.Config_ReclaimPriceInMenuTrashCan_Name,
                tooltip: I18n.Config_ReclaimPriceInMenuTrashCan_Desc,
                getValue: () => this.Config.ReclaimPriceInMenuTrashCan,
                setValue: value => this.Config.ReclaimPriceInMenuTrashCan = value
            );

            // ---------------------------- //
            // Hide categories
            // ---------------------------- //
            configMenu.AddSectionTitle(this.ModManifest, I18n.Config_Title_HideCategories);

            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: I18n.Filter_ArtisanAndCooking,
                getValue: () => this.Config.HideCategories.Contains("filter.artisan-and-cooking"),
                setValue: value => {
                    if (value)
                        this.Config.HideCategories = this.Config.HideCategories.Append("filter.artisan-and-cooking").Distinct().ToArray();
                    else
                        this.Config.HideCategories = this.Config.HideCategories.Where(p => p != "filter.artisan-and-cooking").ToArray();
                }
            );
            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: I18n.Filter_Crafting_Products,
                getValue: () => this.Config.HideCategories.Contains("filter.crafting.products"),
                setValue: value => {
                    if (value)
                        this.Config.HideCategories = this.Config.HideCategories.Append("filter.crafting.products").Distinct().ToArray();
                    else
                        this.Config.HideCategories = this.Config.HideCategories.Where(p => p != "filter.crafting.products").ToArray();
                }
            );
            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: I18n.Filter_Crafting_Resources,
                getValue: () => this.Config.HideCategories.Contains("filter.crafting.resources"),
                setValue: value => {
                    if (value)
                        this.Config.HideCategories = this.Config.HideCategories.Append("filter.crafting.resources").Distinct().ToArray();
                    else
                        this.Config.HideCategories = this.Config.HideCategories.Where(p => p != "filter.crafting.resources").ToArray();
                }
            );
            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: I18n.Filter_Decor_Furniture,
                getValue: () => this.Config.HideCategories.Contains("filter.decor.furniture"),
                setValue: value => {
                    if (value)
                        this.Config.HideCategories = this.Config.HideCategories.Append("filter.decor.furniture").Distinct().ToArray();
                    else
                        this.Config.HideCategories = this.Config.HideCategories.Where(p => p != "filter.decor.furniture").ToArray();
                }
            );
            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: I18n.Filter_Decor_Other,
                getValue: () => this.Config.HideCategories.Contains("filter.decor.other"),
                setValue: value => {
                    if (value)
                        this.Config.HideCategories = this.Config.HideCategories.Append("filter.decor.other").Distinct().ToArray();
                    else
                        this.Config.HideCategories = this.Config.HideCategories.Where(p => p != "filter.decor.other").ToArray();
                }
            );
            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: I18n.Filter_EquipmentBoots,
                getValue: () => this.Config.HideCategories.Contains("filter.equipment-boots"),
                setValue: value => {
                    if (value)
                        this.Config.HideCategories = this.Config.HideCategories.Append("filter.equipment-boots").Distinct().ToArray();
                    else
                        this.Config.HideCategories = this.Config.HideCategories.Where(p => p != "filter.equipment-boots").ToArray();
                }
            );
            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: I18n.Filter_EquipmentClothes,
                getValue: () => this.Config.HideCategories.Contains("filter.equipment-clothes"),
                setValue: value => {
                    if (value)
                        this.Config.HideCategories = this.Config.HideCategories.Append("filter.equipment-clothes").Distinct().ToArray();
                    else
                        this.Config.HideCategories = this.Config.HideCategories.Where(p => p != "filter.equipment-clothes").ToArray();
                }
            );
            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: I18n.Filter_EquipmentHats,
                getValue: () => this.Config.HideCategories.Contains("filter.equipment-hats"),
                setValue: value => {
                    if (value)
                        this.Config.HideCategories = this.Config.HideCategories.Append("filter.equipment-hats").Distinct().ToArray();
                    else
                        this.Config.HideCategories = this.Config.HideCategories.Where(p => p != "filter.equipment-hats").ToArray();
                }
            );
            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: I18n.Filter_EquipmentRings,
                getValue: () => this.Config.HideCategories.Contains("filter.equipment-rings"),
                setValue: value => {
                    if (value)
                        this.Config.HideCategories = this.Config.HideCategories.Append("filter.equipment-rings").Distinct().ToArray();
                    else
                        this.Config.HideCategories = this.Config.HideCategories.Where(p => p != "filter.equipment-rings").ToArray();
                }
            );
            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: I18n.Filter_EquipmentTools,
                getValue: () => this.Config.HideCategories.Contains("filter.equipment-tools"),
                setValue: value => {
                    if (value)
                        this.Config.HideCategories = this.Config.HideCategories.Append("filter.equipment-tools").Distinct().ToArray();
                    else
                        this.Config.HideCategories = this.Config.HideCategories.Where(p => p != "filter.equipment-tools").ToArray();
                }
            );
            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: I18n.Filter_EquipmentWeapons,
                getValue: () => this.Config.HideCategories.Contains("filter.equipment-weapons"),
                setValue: value => {
                    if (value)
                        this.Config.HideCategories = this.Config.HideCategories.Append("filter.equipment-weapons").Distinct().ToArray();
                    else
                        this.Config.HideCategories = this.Config.HideCategories.Where(p => p != "filter.equipment-weapons").ToArray();
                }
            );
            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: I18n.Filter_FarmAnimalDrops,
                getValue: () => this.Config.HideCategories.Contains("filter.farm-animal-drops"),
                setValue: value => {
                    if (value)
                        this.Config.HideCategories = this.Config.HideCategories.Append("filter.farm-animal-drops").Distinct().ToArray();
                    else
                        this.Config.HideCategories = this.Config.HideCategories.Where(p => p != "filter.farm-animal-drops").ToArray();
                }
            );
            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: I18n.Filter_FarmCrops,
                getValue: () => this.Config.HideCategories.Contains("filter.farm-crops"),
                setValue: value => {
                    if (value)
                        this.Config.HideCategories = this.Config.HideCategories.Append("filter.farm-crops").Distinct().ToArray();
                    else
                        this.Config.HideCategories = this.Config.HideCategories.Where(p => p != "filter.farm-crops").ToArray();
                }
            );
            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: I18n.Filter_FarmSeeds,
                getValue: () => this.Config.HideCategories.Contains("filter.farm-seeds"),
                setValue: value => {
                    if (value)
                        this.Config.HideCategories = this.Config.HideCategories.Append("filter.farm-seeds").Distinct().ToArray();
                    else
                        this.Config.HideCategories = this.Config.HideCategories.Where(p => p != "filter.farm-seeds").ToArray();
                }
            );
            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: I18n.Filter_Fish,
                getValue: () => this.Config.HideCategories.Contains("filter.fish"),
                setValue: value => {
                    if (value)
                        this.Config.HideCategories = this.Config.HideCategories.Append("filter.fish").Distinct().ToArray();
                    else
                        this.Config.HideCategories = this.Config.HideCategories.Where(p => p != "filter.fish").ToArray();
                }
            );
            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: I18n.Filter_MineralsAndArtifacts,
                getValue: () => this.Config.HideCategories.Contains("filter.minerals-and-artifacts"),
                setValue: value => {
                    if (value)
                        this.Config.HideCategories = this.Config.HideCategories.Append("filter.minerals-and-artifacts").Distinct().ToArray();
                    else
                        this.Config.HideCategories = this.Config.HideCategories.Where(p => p != "filter.minerals-and-artifacts").ToArray();
                }
            );

            // ---------------------------- //
            // Controls
            // ---------------------------- //
            configMenu.AddSectionTitle(this.ModManifest, I18n.Config_Title_Controls);

            configMenu.AddKeybindList(
                mod: this.ModManifest,
                name: I18n.Config_ShowMenuKey_Name,
                tooltip: I18n.Config_ShowMenuKey_Desc,
                getValue: () => this.Config.ShowMenuKey,
                setValue: value => this.Config.ShowMenuKey = value
            );
        }

        /// <summary>Raised after the player presses or releases any buttons on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnButtonsChanged(object? sender, ButtonsChangedEventArgs e)
        {
            if (this.Config.ShowMenuKey.JustPressed())
            {
                if (!Context.IsPlayerFree)
                {
                    // Players often ask for help due to the menu not opening when expected. To
                    // simplify troubleshooting, log when the key is ignored.
                    if (Game1.activeClickableMenu != null)
                        this.Monitor.Log($"Received menu open key, but a '{Game1.activeClickableMenu.GetType().Name}' menu is already open.");
                    else if (Game1.eventUp)
                        this.Monitor.Log("Received menu open key, but an event is active.");
                    else
                        this.Monitor.Log("Received menu open key, but the player isn't free.");

                    return;
                }

                this.Monitor.Log("Received menu open key.");
                Game1.activeClickableMenu = this.BuildMenu();
            }
        }

        /// <summary>Raised after the game state is updated (≈60 times per second).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnUpdateTicked(object? sender, UpdateTickedEventArgs e)
        {
            this.TextEntryManager.Update();
        }

        /// <summary>Build an item spawner menu.</summary>
        private ItemMenu BuildMenu()
        {
            SpawnableItem[] items = this.GetSpawnableItems().ToArray();
            return new ItemMenu(items, this.TextEntryManager, this.ItemData, this.Helper.ModContent, this.Monitor, this.Config.ReclaimPriceInMenuTrashCan);
        }

        /// <summary>Get the items which can be spawned.</summary>
        private IEnumerable<SpawnableItem> GetSpawnableItems()
        {
            foreach (SearchableItem entry in new ItemRepository().GetAll())
            {
                ModDataCategory? category = this.Categories.FirstOrDefault(rule => rule.IsMatch(entry));

                if (category?.Label != null && this.Config.HideCategories.Contains(category.Label))
                    continue;

                string categoryLabel = category != null
                    ? I18n.GetByKey(category.Label).Default(category.Label)
                    : I18n.Filter_Miscellaneous();

                yield return new SpawnableItem(entry, categoryLabel);
            }
        }

        /// <summary>Log a trace message which summarizes the user's current config.</summary>
        private void LogCustomConfig()
        {
            List<string> phrases = new() { $"menu key {this.Config.ShowMenuKey}" };

            if (!this.Config.ReclaimPriceInMenuTrashCan)
                phrases.Add("reclaim trash can price disabled");

            if (this.Config.HideCategories.Any())
                phrases.Add($"hidden categories {string.Join(", ", this.Config.HideCategories)}");

            this.Monitor.Log($"Started with {string.Join(", ", phrases)}.");
        }
    }
}
