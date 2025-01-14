using System;
using StardewValley;

namespace CJBItemSpawner.Framework.ItemData;

/// <inheritdoc />
internal class SearchableItem : ISearchableItem
{
    /*********
    ** Accessors
    *********/
    /// <inheritdoc />
    public string Type { get; }

    /// <inheritdoc />
    public Item Item { get; }

    /// <inheritdoc />
    public Func<Item> CreateItem { get; }

    /// <inheritdoc />
    public string Id { get; }

    /// <inheritdoc />
    public string QualifiedItemId { get; }

    /// <inheritdoc />
    public string Name => this.Item.Name;

    /// <inheritdoc />
    public string DisplayName => this.Item.DisplayName;


    /*********
    ** Public methods
    *********/
    /// <summary>Construct an instance.</summary>
    /// <param name="type">The item type.</param>
    /// <param name="id">The unqualified item ID.</param>
    /// <param name="createItem">Create an item instance.</param>
    public SearchableItem(string type, string id, Func<ISearchableItem, Item> createItem)
    {
        this.Type = type;
        this.Id = id;
        this.QualifiedItemId = this.Type + this.Id;
        this.CreateItem = () => createItem(this);
        this.Item = createItem(this);
    }

    /// <summary>Construct an instance.</summary>
    /// <param name="item">The item metadata to copy.</param>
    public SearchableItem(ISearchableItem item)
    {
        this.Type = item.Type;
        this.Id = item.Id;
        this.QualifiedItemId = item.QualifiedItemId;
        this.CreateItem = item.CreateItem;
        this.Item = item.Item;
    }

    /// <inheritdoc />
    public bool NameContains(string substring)
    {
        return
            this.Name.IndexOf(substring, StringComparison.OrdinalIgnoreCase) != -1
            || this.DisplayName.IndexOf(substring, StringComparison.OrdinalIgnoreCase) != -1;
    }

    /// <inheritdoc />
    public bool NameEquivalentTo(string name)
    {
        return
            this.Name.Equals(name, StringComparison.OrdinalIgnoreCase)
            || this.DisplayName.Equals(name, StringComparison.OrdinalIgnoreCase);
    }
}
