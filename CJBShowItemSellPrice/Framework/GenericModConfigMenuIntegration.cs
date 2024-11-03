using System;
using CJB.Common.Integrations;
using StardewModdingAPI;

namespace CJBShowItemSellPrice.Framework;

/// <summary>Registers the mod configuration with Generic Mod Config Menu.</summary>
internal class GenericModConfigMenuIntegration
{
    /*********
    ** Fields
    *********/
    /// <summary>The CJB Show Item Sell Price manifest.</summary>
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
    /// <param name="manifest">The CJB Show Item Sell Price manifest.</param>
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

        menu.AddTextOption(
            mod: this.Manifest,
            name: I18n.Config_ShowWhen_Name,
            tooltip: I18n.Config_ShowWhen_Desc,
            getValue: () => this.Config.ShowWhen.ToString(),
            setValue: value => this.Config.ShowWhen = Enum.Parse<ActivateCondition>(value),
            allowedValues: Enum.GetNames<ActivateCondition>(),
            formatAllowedValue: value => I18n.GetByKey($"config.show-when.{value}")
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

        config.ShowWhen = defaults.ShowWhen;
    }
}
