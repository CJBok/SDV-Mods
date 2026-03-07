using System;
using CJB.Common.Integrations.GenericModConfigMenu;
using StardewModdingAPI;

namespace CJBShowItemSellPrice.Framework;

/// <summary>Registers the mod configuration with Generic Mod Config Menu.</summary>
internal class GenericModConfigMenuIntegrationForShowItemSellPrice
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
    /// <param name="manifest">The CJB Show Item Sell Price manifest.</param>
    /// <param name="modRegistry">An API for fetching metadata about loaded mods.</param>
    /// <param name="monitor">Encapsulates monitoring and logging.</param>
    /// <param name="config">Get the current mod config.</param>
    /// <param name="save">Save the mod's current config to the <c>config.json</c> file.</param>
    public GenericModConfigMenuIntegrationForShowItemSellPrice(IManifest manifest, IModRegistry modRegistry, IMonitor monitor, ModConfig config, Action save)
    {
        this.ConfigMenu = new GenericModConfigMenuIntegration<ModConfig>(modRegistry, monitor, manifest, () => config, () => this.Reset(config), save);
    }

    /// <summary>Register the config menu if available.</summary>
    public void Register()
    {
        var menu = this.ConfigMenu;
        if (menu is null)
            return;

        menu
            .Register()
            .AddDropdown(
                name: I18n.Config_ShowWhen_Name,
                tooltip: I18n.Config_ShowWhen_Desc,
                get: config => config.ShowWhen.ToString(),
                set: (config, value) => config.ShowWhen = Enum.Parse<ActivateCondition>(value),
                allowedValues: Enum.GetNames<ActivateCondition>(),
                formatAllowedValue: value => I18n.GetByKey($"config.show-when.{value}")
            );
    }


    /*********
    ** Private methods
    *********/
    /// <summary>Reset the mod's config to its default values.</summary>
    /// <param name="config">The mod config to reset.</param>
    private void Reset(ModConfig config)
    {
        ModConfig defaults = new();

        config.ShowWhen = defaults.ShowWhen;
    }
}
