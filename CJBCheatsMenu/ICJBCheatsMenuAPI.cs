using CJBCheatsMenu.Framework;
using StardewValley;

namespace CJBCheatsMenu;

/// <summary>The API which lets other mods interact with CJB Cheats Menu.</summary>
public interface ICJBCheatsMenuAPI
{
    /// <summary>
    /// Open the cheats menu.
    /// </summary>
    /// <param name="tab">The initial tab to display, or opens the default.</param>
    void OpenCheatsMenu(MenuTab? tab);
}
