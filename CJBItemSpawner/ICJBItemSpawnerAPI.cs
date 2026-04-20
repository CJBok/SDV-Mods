using System;
using System.Collections.Generic;
using StardewValley;
using StardewValley.ItemTypeDefinitions;

namespace CJBItemSpawner;

/// <summary>The API which lets other mods interact with CJB Item Spawner.</summary>
public interface ICJBItemSpawnerAPI
{
    /// <summary>
    /// Open the item spawner menu.
    /// </summary>
    void OpenItemSpawnerMenu();

    /// <summary>Add an item repository.</summary>
    /// <param name="repository">An item repository which returns all spawnable items.</param>
    void AddRepository(Repository repository);

    /// <summary>Get all spawnable items.</summary>
    /// <param name="onlyType">Only include items for the given <see cref="IItemDataDefinition.Identifier"/>.</param>
    /// <param name="includeVariants">Whether to include flavored variants like "Sunflower Honey".</param>
    public delegate IEnumerable<(string Type, string Id, CreateItem CreateItem)> Repository(string? onlyType = null, bool includeVariants = true);

    /// <summary>Create an item instance.</summary>
    /// <param name="type">The item type.</param>
    /// <param name="id">The unqualified item ID.</param>
    public delegate Item CreateItem(string type, string id);
}
