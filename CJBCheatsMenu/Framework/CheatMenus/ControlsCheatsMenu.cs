using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace CJBCheatsMenu.Framework.CheatMenus
{
    /// <summary>
    /// Cheat menu for configuring various cheat hotkeys.
    /// </summary>
    class ControlsCheatsMenu : CheatMenu
    {
        /// <summary>
        /// Unique id for the cheat menu.
        /// </summary>
        public override string Id => "CBJCheatsMenu_ControlsCheatsMenu";

        /// <summary>
        /// The title of the cheat menu (used for tab name).
        /// </summary>
        public override string Title => this.I18n.Get("tabs.controls");

        /// <summary>
        /// Constructs a cheat menu for setting up key bindings for cheats.
        /// </summary>
        /// <param name="config">The user defined preferences.</param>
        /// <param name="cheats">Helper module that has various cheat utilities.</param>
        /// <param name="i18n">Helper module for internationalization.</param>
        public ControlsCheatsMenu(ModConfig config, Cheats cheats, StardewModdingAPI.ITranslationHelper i18n) 
            : base(config, cheats, i18n)
        {

        }

        /// <summary>
        /// The options rendered within this cheat menu.
        /// </summary>
        public override List<Menu.IOptionGroup> OptionGroups
        {
            get
            {
                List<Menu.IOptionGroup> optionGroups = new List<Menu.IOptionGroup>();
                Menu.IOptionGroup controlsOptionGroup = new Menu.OptionGroup($"{this.I18n.Get("controls.title")}:");
                controlsOptionGroup.Options.Add(new Menu.OptionKeyPicker(this.I18n.Get("controls.open-menu"), this.GetKeyFromString(this.Config.OpenMenuKey, ModConfig.DefaultKeyOpenMenu), key => this.Config.OpenMenuKey = key.ToString()));
                controlsOptionGroup.Options.Add(new Menu.OptionKeyPicker(this.I18n.Get("controls.freeze-time"), this.GetKeyFromString(this.Config.FreezeTimeKey, ModConfig.DefaultKeyFreezeTime), key => this.Config.FreezeTimeKey = key.ToString()));
                controlsOptionGroup.Options.Add(new Menu.OptionKeyPicker(this.I18n.Get("controls.grow-tree"), this.GetKeyFromString(this.Config.GrowTreeKey, ModConfig.DefaultKeyGrowTree), key => this.Config.GrowTreeKey = key.ToString()));
                controlsOptionGroup.Options.Add(new Menu.OptionKeyPicker(this.I18n.Get("controls.grow-crops"), this.GetKeyFromString(this.Config.GrowCropsKey, ModConfig.DefaultKeyGrowCrop), key => this.Config.GrowCropsKey = key.ToString()));
                optionGroups.Add(controlsOptionGroup);
                return optionGroups;
            }
        }

        /// <summary>
        /// Parse a string and return the key (e.g "B" -> Keys.B)
        /// </summary>
        /// <param name="keyString">The string that holds the key.</param>
        /// <param name="defaultKey">The Key to return if parsing the string fails.</param>
        /// <returns>The Key parsed from the string, or the defaultKey if the parsing failed.</returns>
        public Keys GetKeyFromString(string keyString, Keys defaultKey)
        {
            Keys key;
            bool success = Enum.TryParse(keyString, out key);
            if (success)
            {
                return key;
            }
            else
            {
                return defaultKey;
            }
        }
    }
}
