using CJBItemSpawner.Framework.ItemData;

namespace CJBItemSpawner.Framework
{
    /// <summary>A game item with metadata for the spawn menu.</summary>
    internal class SpawnableItem : SearchableItem
    {
        /*********
        ** Accessors
        *********/
        /// <summary>The item's category filter label for the spawn menu.</summary>
        public string Category { get; }


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="item">The item metadata.</param>
        /// <param name="category">The item's category filter label for the spawn menu.</param>
        public SpawnableItem(SearchableItem item, string category)
            : base(item)
        {
            this.Category = category;
        }
    }
}
