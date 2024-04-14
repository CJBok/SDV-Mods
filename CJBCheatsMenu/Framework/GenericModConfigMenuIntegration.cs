using System;
using CJB.Common.Integrations;
using CJBCheatsMenu.Framework.Models;
using StardewModdingAPI;

namespace CJBCheatsMenu.Framework
{
    /// <summary>Registers the mod configuration with Generic Mod Config Menu.</summary>
    internal class GenericModConfigMenuIntegration
    {
        /*********
        ** Fields
        *********/
        /// <summary>The CJB Cheats Menu manifest.</summary>
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
        /// <param name="manifest">The CJB Cheats Menu manifest.</param>
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
            menu.AddSectionTitle(this.Manifest, I18n.Controls_Title);
            menu.AddKeybindList(
                mod: this.Manifest,
                name: I18n.Controls_OpenMenu,
                tooltip: I18n.Config_OpenMenu_Desc,
                getValue: () => this.Config.OpenMenuKey,
                setValue: value => this.Config.OpenMenuKey = value
            );
            menu.AddKeybindList(
                mod: this.Manifest,
                name: I18n.Controls_FreezeTime,
                tooltip: I18n.Config_FreezeTime_Desc,
                getValue: () => this.Config.FreezeTimeKey,
                setValue: value => this.Config.FreezeTimeKey = value
            );
            menu.AddKeybindList(
                mod: this.Manifest,
                name: I18n.Controls_GrowTree,
                tooltip: I18n.Config_GrowTree_Desc,
                getValue: () => this.Config.GrowTreeKey,
                setValue: value => this.Config.GrowTreeKey = value
            );
            menu.AddKeybindList(
                mod: this.Manifest,
                name: I18n.Controls_GrowCrops,
                tooltip: I18n.Config_GrowCrops_Desc,
                getValue: () => this.Config.GrowCropsKey,
                setValue: value => this.Config.GrowCropsKey = value
            );

            // other options
            menu.AddSectionTitle(this.Manifest, I18n.Config_Title_OtherOptions);
            menu.AddTextOption(
                mod: this.Manifest,
                name: I18n.Config_DefaultTab_Name,
                tooltip: I18n.Config_DefaultTab_Desc,
                getValue: () => this.Config.DefaultTab.ToString(),
                setValue: value =>
                {
                    if (Enum.TryParse(value, out MenuTab tab))
                        this.Config.DefaultTab = tab;
                },
                allowedValues: ["PlayerAndTools", "FarmAndFishing", "Skills", "Weather", "Relationships", "WarpLocations", "Time", "Advanced", "Controls"]
            );
            menu.AddParagraph(this.Manifest, I18n.Config_OtherOptions);
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Reset the mod's config to its default values.</summary>
        private void Reset()
        {
            ModConfig config = this.Config;
            ModConfig defaults = new();

            config.OpenMenuKey = defaults.OpenMenuKey;
            config.FreezeTimeKey = defaults.FreezeTimeKey;
            config.GrowTreeKey = defaults.GrowTreeKey;
            config.GrowCropsKey = defaults.GrowCropsKey;

            config.DefaultTab = defaults.DefaultTab;
        }
    }
}
