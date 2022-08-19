using System;
using System.Collections.Generic;

namespace CJBItemSpawner.Framework.Models
{
    /// <summary>Predefined data about items.</summary>
    internal record ModItemData
    {
        /*********
        ** Accessors
        *********/
        /// <summary>Items which should be hidden by default because they cause in-game bugs or crashes.</summary>
        public string[] ProblematicItems { get; }

        /// <summary>Item categories that can be sold in shops, regardless of what <see cref="StardewValley.Object.canBeShipped"/> returns.</summary>
        public HashSet<int> ForceSellable { get; }


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="problematicItems">Items which should be hidden by default because they cause in-game bugs or crashes.</param>
        /// <param name="forceSellable">Item categories that can be sold in shops, regardless of what <see cref="StardewValley.Object.canBeShipped"/> returns.</param>
        public ModItemData(string[]? problematicItems, HashSet<int>? forceSellable)
        {
            this.ProblematicItems = problematicItems ?? Array.Empty<string>();
            this.ForceSellable = forceSellable ?? new HashSet<int>();
        }
    }
}
