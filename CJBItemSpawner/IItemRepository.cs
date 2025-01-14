using System.Collections.Generic;
using StardewValley.ItemTypeDefinitions;

namespace CJBItemSpawner;

/// <summary>Provides methods for searching and constructing items.</summary>
/// <remarks>This is copied from the SMAPI source code and should be kept in sync with it.</remarks>
public interface IItemRepository
{
    /*********
     ** Public methods
     *********/
    /// <summary>Get all spawnable items.</summary>
    /// <param name="onlyType">Only include items for the given <see cref="IItemDataDefinition.Identifier"/>.</param>
    /// <param name="includeVariants">Whether to include flavored variants like "Sunflower Honey".</param>
    public IEnumerable<ISearchableItem> GetAll(string? onlyType = null, bool includeVariants = true);
}
