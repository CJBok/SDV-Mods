using System;
using System.Linq;
using CJB.Common.Integrations.GenericModConfigMenu;
using CJBItemSpawner.Framework.Models;
using StardewModdingAPI;

namespace CJBItemSpawner.Framework;

/// <summary>Registers the mod configuration with Generic Mod Config Menu.</summary>
internal class GenericModConfigMenuIntegrationForItemSpawner
{
    /*********
    ** Fields
    *********/
    /// <summary>The Generic Mod Config Menu integration.</summary>
    private readonly GenericModConfigMenuIntegration<ModConfig>? ConfigMenu;


    /*********
    ** Public methods
    *********/
    /// <summary>Construct an instance.</summary>
    /// <param name="manifest">The CJB Item Spawner manifest.</param>
    /// <param name="modRegistry">An API for fetching metadata about loaded mods.</param>
    /// <param name="monitor">Encapsulates monitoring and logging.</param>
    /// <param name="config">Get the current mod config.</param>
    /// <param name="save">Save the mod's current config to the <c>config.json</c> file.</param>
    public GenericModConfigMenuIntegrationForItemSpawner(IManifest manifest, IModRegistry modRegistry, IMonitor monitor, ModConfig config, Action save)
    {
        this.ConfigMenu = new GenericModConfigMenuIntegration<ModConfig>(modRegistry, monitor, manifest, () => config, () => this.Reset(config), save);
    }

    /// <summary>Register the config menu if available.</summary>
    public void Register()
    {
        var menu = this.ConfigMenu;
        if (menu is null)
            return;

        menu.Register();

        // controls
        menu
            .AddSectionTitle(I18n.Config_Title_Controls)
            .AddKeyBinding(
                name: I18n.Config_ShowMenuKey_Name,
                tooltip: I18n.Config_ShowMenuKey_Desc,
                get: config => config.ShowMenuKey,
                set: (config, value) => config.ShowMenuKey = value
            );

        // main options
        menu
            .AddSectionTitle(I18n.Config_Title_Options)
            .AddCheckbox(
                name: I18n.Config_ApplyTrashCanUpgrade_Name,
                tooltip: I18n.Config_ApplyTrashCanUpgrade_Desc,
                get: config => config.ReclaimPriceInMenuTrashCan,
                set: (config, value) => config.ReclaimPriceInMenuTrashCan = value
            );

        // hide categories
        menu.AddSectionTitle(I18n.Config_Title_HideCategories);
        this.AddHideCategoryOption(menu, "filter.artisan-and-cooking", I18n.Filter_ArtisanAndCooking);
        this.AddHideCategoryOption(menu, "filter.crafting.products", I18n.Filter_Crafting_Products);
        this.AddHideCategoryOption(menu, "filter.crafting.resources", I18n.Filter_Crafting_Resources);
        this.AddHideCategoryOption(menu, "filter.decor.furniture", I18n.Filter_Decor_Furniture);
        this.AddHideCategoryOption(menu, "filter.decor.other", I18n.Filter_Decor_Other);
        this.AddHideCategoryOption(menu, "filter.equipment-boots", I18n.Filter_EquipmentBoots);
        this.AddHideCategoryOption(menu, "filter.equipment-clothes", I18n.Filter_EquipmentClothes);
        this.AddHideCategoryOption(menu, "filter.equipment-hats", I18n.Filter_EquipmentHats);
        this.AddHideCategoryOption(menu, "filter.equipment-rings", I18n.Filter_EquipmentRings);
        this.AddHideCategoryOption(menu, "filter.equipment-tools", I18n.Filter_EquipmentTools);
        this.AddHideCategoryOption(menu, "filter.equipment-weapons", I18n.Filter_EquipmentWeapons);
        this.AddHideCategoryOption(menu, "filter.farm-animal-drops", I18n.Filter_FarmAnimalDrops);
        this.AddHideCategoryOption(menu, "filter.farm-crops", I18n.Filter_FarmCrops);
        this.AddHideCategoryOption(menu, "filter.farm-seeds", I18n.Filter_FarmSeeds);
        this.AddHideCategoryOption(menu, "filter.fish", I18n.Filter_Fish);
        this.AddHideCategoryOption(menu, "filter.minerals-and-artifacts", I18n.Filter_MineralsAndArtifacts);
    }


    /*********
    ** Private methods
    *********/
    /// <summary>Reset the mod's config to its default values.</summary>
    /// <param name="config">The mod config to reset.</param>
    private void Reset(ModConfig config)
    {
        ModConfig defaults = new();

        config.ReclaimPriceInMenuTrashCan = defaults.ReclaimPriceInMenuTrashCan;
        config.HideCategories = defaults.HideCategories.ToArray();
        config.ShowMenuKey = defaults.ShowMenuKey;
    }

    /// <summary>Add a checkbox to set whether a category is hidden.</summary>
    /// <param name="configMenu">The config menu to extend.</param>
    /// <param name="id">The category ID.</param>
    /// <param name="displayName">Get the category's translated display name.</param>
    private void AddHideCategoryOption(GenericModConfigMenuIntegration<ModConfig> configMenu, string id, Func<string> displayName)
    {
        configMenu.AddCheckbox(
            name: displayName,
            tooltip: () => I18n.Config_HideCategory_Desc(displayName()),
            get: config => config.HideCategories.Contains(id),
            set: (config, hide) =>
            {
                bool wasHidden = Array.IndexOf(config.HideCategories, id) != -1;
                if (hide != wasHidden)
                {
                    config.HideCategories = hide
                        ? config.HideCategories.Append(id).Distinct().ToArray()
                        : config.HideCategories.Where(p => p != id).ToArray();
                }
            }
        );
    }
}
