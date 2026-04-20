using CJBCheatsMenu.Framework.Models;
using StardewModdingAPI;
using StardewModdingAPI.Utilities;
using StardewValley;

namespace CJBCheatsMenu.Framework;

/// <inheritdoc />
public sealed class CJBCheatsMenuAPI : ICJBCheatsMenuAPI
{
    /*********
     ** Fields
     *********/
    /// <summary>Manages the cheat implementations.</summary>
    private readonly CheatManager Cheats;

    /// <summary>The mod configuration.</summary>
    private readonly ModConfig Config;

    /// <summary>Encapsulates monitoring and logging.</summary>
    private readonly IMonitor Monitor;

    /*********
     ** Public methods
     *********/
    /// <summary>Construct an instance.</summary>
    /// <param name="cheats">The cheats helper.</param>
    /// <param name="config">The mod configuration.</param>
    /// <param name="monitor">Encapsulates monitoring and logging.</param>
    internal CJBCheatsMenuAPI(CheatManager cheats, ModConfig config, IMonitor monitor)
    {
        this.Cheats = cheats;
        this.Config = config;
        this.Monitor = monitor;
    }

    /// <inheritdoc />
    public void OpenCheatsMenu(MenuTab? tab)
    {
        Game1.activeClickableMenu = new CheatsMenu(tab ?? this.Config.DefaultTab, this.Cheats, this.Monitor, true);
    }
}
