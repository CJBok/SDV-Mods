using System;
using System.Collections.Generic;
using System.Linq;
using CJBItemSpawner.Framework.ItemData;
using StardewValley;
using SObject = StardewValley.Object;

namespace CJBItemSpawner.Framework.Models;

/// <summary>The rules for matching a category.</summary>
internal record ModDataCategoryRule
{
    /*********
    ** Accessors
    *********/
    /// <summary>The full name of the item's C# instance type.</summary>
    public HashSet<string> Class { get; }

    /// <summary>The object's type (i.e. <see cref="SObject.Type"/>).</summary>
    public HashSet<string> ObjType { get; }

    /// <summary>The object's category (i.e. <see cref="Item.Category"/>).</summary>
    public HashSet<int> ObjCategory { get; }

    /// <summary>The item's unique ID (i.e. <see cref="Item.ParentSheetIndex"/>).</summary>
    public HashSet<string> ItemId { get; }


    /*********
    ** Public methods
    *********/
    /// <summary>Construct an instance.</summary>
    /// <param name="class">The full name of the item's C# instance type.</param>
    /// <param name="objType">The object's type (i.e. <see cref="SObject.Type"/>).</param>
    /// <param name="objCategory">The object's category (i.e. <see cref="Item.Category"/>).</param>
    /// <param name="itemId">The item's unique ID (i.e. <see cref="Item.ParentSheetIndex"/>).</param>
    public ModDataCategoryRule(HashSet<string>? @class, HashSet<string>? objType, HashSet<int>? objCategory, HashSet<string>? itemId)
    {
        IEnumerable<string> empty = [];

        this.Class = new HashSet<string>(@class ?? empty, StringComparer.OrdinalIgnoreCase);
        this.ObjType = new HashSet<string>(objType ?? empty, StringComparer.OrdinalIgnoreCase);
        this.ObjCategory = new HashSet<int>(objCategory ?? Enumerable.Empty<int>());
        this.ItemId = new HashSet<string>(itemId ?? empty, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>Get whether a given item matches the rules for this category.</summary>
    /// <param name="entry">The searchable item to check.</param>
    public bool IsMatch(SearchableItem entry)
    {
        Item item = entry.Item;
        SObject? obj = item as SObject;

        // match criteria
        if (this.Class.Any() && this.GetClassFullNames(item).Any(className => this.Class.Contains(className)))
            return true;
        if (this.ObjCategory.Any() && this.ObjCategory.Contains(item.Category))
            return true;
        if (this.ObjType.Any() && obj != null && this.ObjType.Contains(obj.Type))
            return true;
        if (this.ItemId.Any() && this.ItemId.Contains($"{entry.Type}:{item.ParentSheetIndex}"))
            return true;

        return false;
    }


    /*********
    ** Private methods
    *********/
    /// <summary>Get the full names for each class in an item's class hierarchy.</summary>
    /// <param name="item">The item instance.</param>
    private IEnumerable<string> GetClassFullNames(Item item)
    {
        for (Type? type = item.GetType(); type != null; type = type.BaseType)
            yield return type.FullName!;
    }
}
