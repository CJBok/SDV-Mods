using System.Collections.Generic;

namespace CJBCheatsMenu.Framework.Menu
{
    /// <summary>
    /// A menu with options that will have a tab in the menu dialog.
    /// </summary>
    public interface IMenu
    {
        /// <summary>
        /// The unique id for this menu (ex: YourModName_CheatMenuName).
        /// </summary>
        string Id { get; }

        /// <summary>
        /// The string that will be displayed in the tabs section when selecting the menu.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// A list of option groups for this menu.
        /// </summary>
        List<IOptionGroup> OptionGroups { get; }
    }
}
