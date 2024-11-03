using System;
using System.Linq;
using CJB.Common.Integrations;
using CJBItemSpawner.Framework.Models;
using StardewModdingAPI;

namespace CJBItemSpawner.Framework;

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

        // controls
        menu.AddSectionTitle(this.Manifest, I18n.Config_Title_Controls);
        menu.AddKeybindList(
            mod: this.Manifest,
            name: I18n.Config_ShowMenuKey_Name,
            tooltip: I18n.Config_ShowMenuKey_Desc,
            getValue: () => this.Config.ShowMenuKey,
            setValue: value => this.Config.ShowMenuKey = value
        );

        // main options
        menu.AddSectionTitle(this.Manifest, I18n.Config_Title_Options);
        menu.AddBoolOption(
            mod: this.Manifest,
            name: I18n.Config_ApplyTrashCanUpgrade_Name,
            tooltip: I18n.Config_ApplyTrashCanUpgrade_Desc,
            getValue: () => this.Config.ReclaimPriceInMenuTrashCan,
            setValue: value => this.Config.ReclaimPriceInMenuTrashCan = value
        );

        // hide categories
        menu.AddSectionTitle(this.Manifest, I18n.Config_Title_HideCategories);
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
    private void Reset()
    {
        ModConfig config = this.Config;
        ModConfig defaults = new();

        config.ReclaimPriceInMenuTrashCan = defaults.ReclaimPriceInMenuTrashCan;
        config.HideCategories = defaults.HideCategories.ToArray();
        config.ShowMenuKey = defaults.ShowMenuKey;
    }

    /// <summary>Add a checkbox to set whether a category is hidden.</summary>
    /// <param name="menu">The config menu to update.</param>
    /// <param name="id">The category ID.</param>
    /// <param name="displayName">Get the category's translated display name.</param>
    private void AddHideCategoryOption(IGenericModConfigMenuApi menu, string id, Func<string> displayName)
    {
        menu.AddBoolOption(
            mod: this.Manifest,
            name: displayName,
            tooltip: () => I18n.Config_HideCategory_Desc(displayName()),
            getValue: () => this.Config.HideCategories.Contains(id),
            setValue: hide =>
            {
                bool wasHidden = Array.IndexOf(this.Config.HideCategories, id) != -1;
                if (hide != wasHidden)
                {
                    this.Config.HideCategories = hide
                        ? this.Config.HideCategories.Append(id).Distinct().ToArray()
                        : this.Config.HideCategories.Where(p => p != id).ToArray();
                }
            }
        );
    }
}
