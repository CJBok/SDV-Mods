namespace CJBCheatsMenu.Framework.Menu
{
    /// <summary>
    /// Api used for registering a new menu.
    /// </summary>
    public interface IMenuManager
    {
        /// <summary>
        /// Registers a menu so that it appears in the tabs of the menu dialog.
        /// </summary>
        /// <param name="menu"></param>
        /// <returns>true if menu was successfully registered, false otherwise (e.g. menu with same id was already registered). </returns>
        bool RegisterMenu(IMenu menu);

        /// <summary>
        /// Deregisters a previously registered menu.
        /// </summary>
        /// <param name="menuId">The id of the menu to be deregistered.</param>
        /// <returns>true if the menu was successfully deregistered, false otherwise (e.g. no menu with that id was registered).</returns>
        bool DeregisterMenu(string menuId);
    }
}
