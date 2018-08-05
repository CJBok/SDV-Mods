using System;

namespace CJBItemSpawner.Framework.Constants
{
    /// <summary>Specifies how to sort items in the item menu.</summary>
    internal enum ItemSort
    {
        /// <summary>Sort by the item's display name.</summary>
        DisplayName,

        /// <summary>Sort by the item's category name.</summary>
        Category,

        /// <summary>Sort by the item ID.</summary>
        ID
    }

    /// <summary>Extension methods for <see cref="ItemSort"/>.</summary>
    internal static class ItemSortExtensions
    {
        /*********
        ** Public methods
        *********/
        /// <summary>Get the next sort option.</summary>
        /// <param name="current">The current sort value.</param>
        public static ItemSort GetNext(this ItemSort current)
        {
            switch (current)
            {
                case ItemSort.DisplayName:
                    return ItemSort.Category;
                case ItemSort.Category:
                    return ItemSort.ID;
                case ItemSort.ID:
                    return ItemSort.DisplayName;
                default:
                    throw new NotSupportedException($"Unknown sort '{current}'.");
            }
        }
    }
}
