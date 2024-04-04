using CJBItemSpawner.Framework.ItemData;

namespace CJBItemSpawner.Framework
{
    /// <summary>A game item with metadata for the spawn menu.</summary>
    internal class SpawnableItem : SearchableItem
    {
        /*********
        ** Accessors
        *********/
        /// <summary>The item's category filter labels for the spawn menu.</summary>
        public string[] Categories { get; }


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="item">The item metadata.</param>
        /// <param name="categories">The item's category filter labels for the spawn menu.</param>
        public SpawnableItem(SearchableItem item, string[] categories)
            : base(item)
        {
            this.Categories = categories;
        }
    }
}
