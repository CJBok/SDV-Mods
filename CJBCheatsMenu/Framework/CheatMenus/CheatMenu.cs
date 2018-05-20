using System.Collections.Generic;
using CJBCheatsMenu.Framework.Menu;

namespace CJBCheatsMenu.Framework.CheatMenus
{
    /// <summary>
    /// A cheat menu that has a tab in the main cheat dialog, and holds a bunch of related cheat options.
    /// </summary>
    abstract internal class CheatMenu : Menu.IMenu
    {
        /// <summary>
        /// The user defined preferences.
        /// </summary>
        protected ModConfig Config { get; }

        /// <summary>
        /// Helper module that has various cheat utilities.
        /// </summary>
        protected Cheats Cheats { get; }

        /// <summary>
        /// Helper module for internationalization.
        /// </summary>
        protected StardewModdingAPI.ITranslationHelper I18n { get; }

        /// <summary>
        /// Contructs a cheats menu.
        /// </summary>
        /// <param name="config">The user defined preferences.</param>
        /// <param name="cheats">Helper module that has various cheat utilities.</param>
        /// <param name="i18n">Helper module for internationalization.</param>
        public CheatMenu(ModConfig config, Cheats cheats, StardewModdingAPI.ITranslationHelper i18n)
        {
            Config = config;
            Cheats = cheats;
            I18n = i18n;
        }

        /// <summary>
        /// Unique id for the cheat menu.
        /// </summary>
        abstract public string Id { get; }

        /// <summary>
        /// The title of the cheat menu (used for tab name).
        /// </summary>
        abstract public string Title { get; }

        /// <summary>
        /// The options rendered within this cheat menu.
        /// </summary>
        abstract public List<IOptionGroup> OptionGroups { get; }
    }
}
