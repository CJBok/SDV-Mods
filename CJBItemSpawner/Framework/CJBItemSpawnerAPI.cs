using System;
using System.Collections.Generic;
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

    /// <summary>The item repositories which returns all spawnable items.</summary>
    private readonly IList<IItemRepository> ItemRepositories;

    /*********
     ** Public methods
     *********/
    /// <summary>Construct an instance.</summary>
    /// <param name="buildMenu">Method for building an item spawner menu.</param>
    internal CJBItemSpawnerAPI(Func<ItemMenu> buildMenu, IList<IItemRepository> itemRepositories)
    {
        this.BuildMenu = buildMenu;
        this.ItemRepositories = itemRepositories;
    }
    
    /// <inheritdoc />
    public void OpenItemSpawnerMenu()
    {
        Game1.activeClickableMenu = this.BuildMenu();
    }

    /// <inheritdoc />
    public void AddRepository(IItemRepository repository)
    {
        this.ItemRepositories.Add(repository);
    }
}
