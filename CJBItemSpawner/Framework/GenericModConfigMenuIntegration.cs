using System;
using System.Linq;
using CJB.Common.Integrations;
using CJBItemSpawner.Framework.Models;
using StardewModdingAPI;

namespace CJBItemSpawner.Framework
{
    /// <summary>Registers the mod configuration with Generic Mod Config Menu.</summary>
    internal class GenericModConfigMenuIntegration
    {
        /*********
        ** Fields
        *********/
        /// <summary>The CJB Item Spawner manifest.</summary>
        private readonly IManifest Manifest;

        /// <summary>The Generic Mod Config Menu integration.</summary>
        private readonly IGenericModConfigMenuApi? ConfigMenu;

        /// <summary>The current mod settings.</summary>
        private readonly ModConfig Config;

        /// <summary>Save the mod's current config to the <c>config.json</c> file.</summary>
        private readonly Action Save;


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="manifest">The CJB Item Spawner manifest.</param>
        /// <param name="modRegistry">An API for fetching metadata about loaded mods.</param>
        /// <param name="config">Get the current mod config.</param>
        /// <param name="save">Save the mod's current config to the <c>config.json</c> file.</param>
        public GenericModConfigMenuIntegration(IManifest manifest, IModRegistry modRegistry, ModConfig config, Action save)
        {
            this.Manifest = manifest;
            this.ConfigMenu = modRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            this.Config = config;
            this.Save = save;
        }

        /// <summary>Register the config menu if available.</summary>
        public void Register()
        {
            var menu = this.ConfigMenu;
            if (menu is null)
                return;

            menu.Register(this.Manifest, this.Reset, this.Save);

            // ---------------------------- //
            // Main options
            // ---------------------------- //
            menu.AddSectionTitle(this.Manifest, I18n.Config_Title_MainOptions);

            menu.AddBoolOption(
                mod: this.Manifest,
                name: I18n.Config_ReclaimPriceInMenuTrashCan_Name,
                tooltip: I18n.Config_ReclaimPriceInMenuTrashCan_Desc,
                getValue: () => this.Config.ReclaimPriceInMenuTrashCan,
                setValue: value => this.Config.ReclaimPriceInMenuTrashCan = value
            );

            // ---------------------------- //
            // Hide categories
            // ---------------------------- //
            menu.AddSectionTitle(this.Manifest, I18n.Config_Title_HideCategories);

            menu.AddBoolOption(
                mod: this.Manifest,
                name: I18n.Filter_ArtisanAndCooking,
                getValue: () => this.Config.HideCategories.Contains("filter.artisan-and-cooking"),
                setValue: value =>
                {
                    if (value)
                        this.Config.HideCategories = this.Config.HideCategories.Append("filter.artisan-and-cooking").Distinct().ToArray();
                    else
                        this.Config.HideCategories = this.Config.HideCategories.Where(p => p != "filter.artisan-and-cooking").ToArray();
                }
            );
            menu.AddBoolOption(
                mod: this.Manifest,
                name: I18n.Filter_Crafting_Products,
                getValue: () => this.Config.HideCategories.Contains("filter.crafting.products"),
                setValue: value =>
                {
                    if (value)
                        this.Config.HideCategories = this.Config.HideCategories.Append("filter.crafting.products").Distinct().ToArray();
                    else
                        this.Config.HideCategories = this.Config.HideCategories.Where(p => p != "filter.crafting.products").ToArray();
                }
            );
            menu.AddBoolOption(
                mod: this.Manifest,
                name: I18n.Filter_Crafting_Resources,
                getValue: () => this.Config.HideCategories.Contains("filter.crafting.resources"),
                setValue: value =>
                {
                    if (value)
                        this.Config.HideCategories = this.Config.HideCategories.Append("filter.crafting.resources").Distinct().ToArray();
                    else
                        this.Config.HideCategories = this.Config.HideCategories.Where(p => p != "filter.crafting.resources").ToArray();
                }
            );
            menu.AddBoolOption(
                mod: this.Manifest,
                name: I18n.Filter_Decor_Furniture,
                getValue: () => this.Config.HideCategories.Contains("filter.decor.furniture"),
                setValue: value =>
                {
                    if (value)
                        this.Config.HideCategories = this.Config.HideCategories.Append("filter.decor.furniture").Distinct().ToArray();
                    else
                        this.Config.HideCategories = this.Config.HideCategories.Where(p => p != "filter.decor.furniture").ToArray();
                }
            );
            menu.AddBoolOption(
                mod: this.Manifest,
                name: I18n.Filter_Decor_Other,
                getValue: () => this.Config.HideCategories.Contains("filter.decor.other"),
                setValue: value =>
                {
                    if (value)
                        this.Config.HideCategories = this.Config.HideCategories.Append("filter.decor.other").Distinct().ToArray();
                    else
                        this.Config.HideCategories = this.Config.HideCategories.Where(p => p != "filter.decor.other").ToArray();
                }
            );
            menu.AddBoolOption(
                mod: this.Manifest,
                name: I18n.Filter_EquipmentBoots,
                getValue: () => this.Config.HideCategories.Contains("filter.equipment-boots"),
                setValue: value =>
                {
                    if (value)
                        this.Config.HideCategories = this.Config.HideCategories.Append("filter.equipment-boots").Distinct().ToArray();
                    else
                        this.Config.HideCategories = this.Config.HideCategories.Where(p => p != "filter.equipment-boots").ToArray();
                }
            );
            menu.AddBoolOption(
                mod: this.Manifest,
                name: I18n.Filter_EquipmentClothes,
                getValue: () => this.Config.HideCategories.Contains("filter.equipment-clothes"),
                setValue: value =>
                {
                    if (value)
                        this.Config.HideCategories = this.Config.HideCategories.Append("filter.equipment-clothes").Distinct().ToArray();
                    else
                        this.Config.HideCategories = this.Config.HideCategories.Where(p => p != "filter.equipment-clothes").ToArray();
                }
            );
            menu.AddBoolOption(
                mod: this.Manifest,
                name: I18n.Filter_EquipmentHats,
                getValue: () => this.Config.HideCategories.Contains("filter.equipment-hats"),
                setValue: value =>
                {
                    if (value)
                        this.Config.HideCategories = this.Config.HideCategories.Append("filter.equipment-hats").Distinct().ToArray();
                    else
                        this.Config.HideCategories = this.Config.HideCategories.Where(p => p != "filter.equipment-hats").ToArray();
                }
            );
            menu.AddBoolOption(
                mod: this.Manifest,
                name: I18n.Filter_EquipmentRings,
                getValue: () => this.Config.HideCategories.Contains("filter.equipment-rings"),
                setValue: value =>
                {
                    if (value)
                        this.Config.HideCategories = this.Config.HideCategories.Append("filter.equipment-rings").Distinct().ToArray();
                    else
                        this.Config.HideCategories = this.Config.HideCategories.Where(p => p != "filter.equipment-rings").ToArray();
                }
            );
            menu.AddBoolOption(
                mod: this.Manifest,
                name: I18n.Filter_EquipmentTools,
                getValue: () => this.Config.HideCategories.Contains("filter.equipment-tools"),
                setValue: value =>
                {
                    if (value)
                        this.Config.HideCategories = this.Config.HideCategories.Append("filter.equipment-tools").Distinct().ToArray();
                    else
                        this.Config.HideCategories = this.Config.HideCategories.Where(p => p != "filter.equipment-tools").ToArray();
                }
            );
            menu.AddBoolOption(
                mod: this.Manifest,
                name: I18n.Filter_EquipmentWeapons,
                getValue: () => this.Config.HideCategories.Contains("filter.equipment-weapons"),
                setValue: value =>
                {
                    if (value)
                        this.Config.HideCategories = this.Config.HideCategories.Append("filter.equipment-weapons").Distinct().ToArray();
                    else
                        this.Config.HideCategories = this.Config.HideCategories.Where(p => p != "filter.equipment-weapons").ToArray();
                }
            );
            menu.AddBoolOption(
                mod: this.Manifest,
                name: I18n.Filter_FarmAnimalDrops,
                getValue: () => this.Config.HideCategories.Contains("filter.farm-animal-drops"),
                setValue: value =>
                {
                    if (value)
                        this.Config.HideCategories = this.Config.HideCategories.Append("filter.farm-animal-drops").Distinct().ToArray();
                    else
                        this.Config.HideCategories = this.Config.HideCategories.Where(p => p != "filter.farm-animal-drops").ToArray();
                }
            );
            menu.AddBoolOption(
                mod: this.Manifest,
                name: I18n.Filter_FarmCrops,
                getValue: () => this.Config.HideCategories.Contains("filter.farm-crops"),
                setValue: value =>
                {
                    if (value)
                        this.Config.HideCategories = this.Config.HideCategories.Append("filter.farm-crops").Distinct().ToArray();
                    else
                        this.Config.HideCategories = this.Config.HideCategories.Where(p => p != "filter.farm-crops").ToArray();
                }
            );
            menu.AddBoolOption(
                mod: this.Manifest,
                name: I18n.Filter_FarmSeeds,
                getValue: () => this.Config.HideCategories.Contains("filter.farm-seeds"),
                setValue: value =>
                {
                    if (value)
                        this.Config.HideCategories = this.Config.HideCategories.Append("filter.farm-seeds").Distinct().ToArray();
                    else
                        this.Config.HideCategories = this.Config.HideCategories.Where(p => p != "filter.farm-seeds").ToArray();
                }
            );
            menu.AddBoolOption(
                mod: this.Manifest,
                name: I18n.Filter_Fish,
                getValue: () => this.Config.HideCategories.Contains("filter.fish"),
                setValue: value =>
                {
                    if (value)
                        this.Config.HideCategories = this.Config.HideCategories.Append("filter.fish").Distinct().ToArray();
                    else
                        this.Config.HideCategories = this.Config.HideCategories.Where(p => p != "filter.fish").ToArray();
                }
            );
            menu.AddBoolOption(
                mod: this.Manifest,
                name: I18n.Filter_MineralsAndArtifacts,
                getValue: () => this.Config.HideCategories.Contains("filter.minerals-and-artifacts"),
                setValue: value =>
                {
                    if (value)
                        this.Config.HideCategories = this.Config.HideCategories.Append("filter.minerals-and-artifacts").Distinct().ToArray();
                    else
                        this.Config.HideCategories = this.Config.HideCategories.Where(p => p != "filter.minerals-and-artifacts").ToArray();
                }
            );

            // ---------------------------- //
            // Controls
            // ---------------------------- //
            menu.AddSectionTitle(this.Manifest, I18n.Config_Title_Controls);

            menu.AddKeybindList(
                mod: this.Manifest,
                name: I18n.Config_ShowMenuKey_Name,
                tooltip: I18n.Config_ShowMenuKey_Desc,
                getValue: () => this.Config.ShowMenuKey,
                setValue: value => this.Config.ShowMenuKey = value
            );
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Reset the mod's config to its default values.</summary>
        private void Reset()
        {
            ModConfig config = this.Config;
            ModConfig defaults = new();

            config.ReclaimPriceInMenuTrashCan = defaults.ReclaimPriceInMenuTrashCan;
            config.HideCategories = defaults.HideCategories.ToArray();
            config.ShowMenuKey = defaults.ShowMenuKey;
        }
    }
}
