using System;
using StardewValley;

namespace CJBItemSpawner.Framework;

/// <inheritdoc />
public sealed class CJBItemSpawnerAPI : ICJBItemSpawnerAPI
{
    /*********
     ** Fields
     *********/
    /// <summary>Build an item spawner menu.</summary>
    private readonly Func<ItemMenu> BuildMenu;

    /*********
     ** Public methods
     *********/
    /// <summary>Construct an instance.</summary>
    /// <param name="buildMenu">Method for building an item spawner menu.</param>
    internal CJBItemSpawnerAPI(Func<ItemMenu> buildMenu)
    {
        this.BuildMenu = buildMenu;
    }
    
    /// <inheritdoc />
    public void OpenItemSpawnerMenu()
    {
        Game1.activeClickableMenu = this.BuildMenu();
    }
}
