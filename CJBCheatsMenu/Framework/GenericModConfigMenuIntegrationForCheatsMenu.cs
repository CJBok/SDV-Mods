using System;
using CJB.Common.Integrations.GenericModConfigMenu;
using CJBCheatsMenu.Framework.Models;
using StardewModdingAPI;

namespace CJBCheatsMenu.Framework;

/// <summary>Registers the mod configuration with Generic Mod Config Menu.</summary>
internal class GenericModConfigMenuIntegrationForCheatsMenu
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
    /// <param name="manifest">The CJB Cheats Menu manifest.</param>
    /// <param name="modRegistry">An API for fetching metadata about loaded mods.</param>
    /// <param name="monitor">Encapsulates monitoring and logging.</param>
    /// <param name="config">Get the current mod config.</param>
    /// <param name="save">Save the mod's current config to the <c>config.json</c> file.</param>
    public GenericModConfigMenuIntegrationForCheatsMenu(IManifest manifest, IModRegistry modRegistry, IMonitor monitor, ModConfig config, Action save)
    {
        this.ConfigMenu = new GenericModConfigMenuIntegration<ModConfig>(modRegistry, monitor, manifest, () => config, () => this.Reset(config), save);
    }

    /// <summary>Register the config menu if available.</summary>
    public void Register()
    {
        if (this.ConfigMenu is not { IsLoaded: true } menu)
            return;

        menu
            .Register()

            // controls
            .AddSectionTitle(I18n.Controls_Title)
            .AddKeyBinding(
                name: I18n.Controls_OpenMenu,
                tooltip: I18n.Config_OpenMenu_Desc,
                get: config => config.OpenMenuKey,
                set: (config, value) => config.OpenMenuKey = value
            )
            .AddKeyBinding(
                name: I18n.Controls_FreezeTime,
                tooltip: I18n.Config_FreezeTime_Desc,
                get: config => config.FreezeTimeKey,
                set: (config, value) => config.FreezeTimeKey = value
            )
            .AddKeyBinding(
                name: I18n.Controls_GrowTree,
                tooltip: I18n.Config_GrowTree_Desc,
                get: config => config.GrowTreeKey,
                set: (config, value) => config.GrowTreeKey = value
            )
            .AddKeyBinding(
                name: I18n.Controls_GrowCrops,
                tooltip: I18n.Config_GrowCrops_Desc,
                get: config => config.GrowCropsKey,
                set: (config, value) => config.GrowCropsKey = value
            )
            .AddKeyBinding(
                name: I18n.Controls_ReloadConfig,
                tooltip: I18n.Controls_ReloadConfig_Desc,
                get: config => config.ReloadConfigKey,
                set: (config, value) => config.ReloadConfigKey = value
            )

            // other options
            .AddSectionTitle(I18n.Config_Title_OtherOptions)
            .AddDropdown(
                name: I18n.Config_DefaultTab_Name,
                tooltip: I18n.Config_DefaultTab_Desc,
                get: config => config.DefaultTab.ToString(),
                set: (config, value) =>
                {
                    if (Enum.TryParse(value, out MenuTab tab))
                        config.DefaultTab = tab;
                },
                allowedValues: Enum.GetNames<MenuTab>(),
                formatAllowedValue: value =>
                {
                    if (Enum.TryParse(value, out MenuTab tab))
                    {
                        return tab switch
                        {
                            MenuTab.PlayerAndTools => I18n.Tabs_PlayerAndTools(),
                            MenuTab.FarmAndFishing => I18n.Tabs_FarmAndFishing(),
                            MenuTab.Skills => I18n.Tabs_Skills(),
                            MenuTab.Weather => I18n.Tabs_Weather(),
                            MenuTab.Relationships => I18n.Tabs_Relationships(),
                            MenuTab.WarpLocations => I18n.Tabs_Warp(),
                            MenuTab.Time => I18n.Tabs_Time(),
                            MenuTab.Advanced => I18n.Tabs_Advanced(),
                            MenuTab.Controls => I18n.Tabs_Controls(),
                            _ => value
                        };
                    }

                    return value;
                })
                .AddParagraph(I18n.Config_OtherOptions);
    }


    /*********
    ** Private methods
    *********/
    /// <summary>Reset the mod's config to its default values.</summary>
    /// <param name="config">The config to reset.</param>
    private void Reset(ModConfig config)
    {
        ModConfig defaults = new();

        config.OpenMenuKey = defaults.OpenMenuKey;
        config.FreezeTimeKey = defaults.FreezeTimeKey;
        config.GrowTreeKey = defaults.GrowTreeKey;
        config.GrowCropsKey = defaults.GrowCropsKey;

        config.DefaultTab = defaults.DefaultTab;
    }
}
