using System;
using StardewValley;
using StardewValley.ItemTypeDefinitions;

namespace CJBItemSpawner;

/// <summary>A game item with metadata.</summary>
/// <remarks>This is copied from the SMAPI source code and should be kept in sync with it.</remarks>
public interface ISearchableItem
{
    /*********
     ** Accessors
     *********/
    /// <summary>The <see cref="IItemDataDefinition.Identifier"/> value for the item type.</summary>
    public string Type { get; }

    /// <summary>A sample item instance.</summary>
    public Item Item { get; }

    /// <summary>Create an item instance.</summary>
    public Func<Item> CreateItem { get; }

    /// <summary>The unqualified item ID.</summary>
    public string Id { get; }

    /// <summary>The qualified item ID.</summary>
    public string QualifiedItemId { get; }

    /// <summary>The item's default name.</summary>
    public string Name { get; }

    /// <summary>The item's display name for the current language.</summary>
    public string DisplayName { get; }

    /// <summary>Get whether the item name contains a case-insensitive substring.</summary>
    /// <param name="substring">The substring to find.</param>
    public bool NameContains(string substring);

    /// <summary>Get whether the item name is exactly equal to a case-insensitive string.</summary>
    /// <param name="name">The substring to find.</param>
    public bool NameEquivalentTo(string name);
}
