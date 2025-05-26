using System.Collections.Generic;

namespace CJBItemSpawner.Framework.Models;

/// <summary>Predefined data about items.</summary>
internal record ModItemData
{
    /*********
    ** Accessors
    *********/
    /// <summary>Item categories that can be sold in shops, regardless of what <see cref="StardewValley.Object.canBeShipped"/> returns.</summary>
    public HashSet<int> ForceSellable { get; }


    /*********
    ** Public methods
    *********/
    /// <summary>Construct an instance.</summary>
    /// <param name="forceSellable">Item categories that can be sold in shops, regardless of what <see cref="StardewValley.Object.canBeShipped"/> returns.</param>
    public ModItemData(HashSet<int>? forceSellable)
    {
        this.ForceSellable = forceSellable ?? [];
    }
}
