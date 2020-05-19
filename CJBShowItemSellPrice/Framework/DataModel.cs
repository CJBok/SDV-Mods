using System.Collections.Generic;
using SObject = StardewValley.Object;

namespace CJBShowItemSellPrice.Framework
{
    /// <summary>Metadata that isn't available from the game data directly.</summary>
    internal class DataModel
    {
        /*********
        ** Accessors
        *********/
        /// <summary>Item categories that can be sold in shops, regardless of what <see cref="SObject.canBeShipped"/> returns.</summary>
        public HashSet<int> ForceSellable { get; set; } = new HashSet<int>();
    }
}
