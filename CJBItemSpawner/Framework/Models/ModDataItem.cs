using CJBItemSpawner.Framework.ItemData;

namespace CJBItemSpawner.Framework.Models
{
    /// <summary>A unique item identifier.</summary>
    internal class ModDataItem
    {
        /********
        ** Accessors
        ********/
        /// <summary>The item type.</summary>
        public ItemType Type { get; set; }

        /// <summary>The unique item ID for the type.</summary>
        public int ID { get; set; }
    }
}
