using System;

namespace CJBItemSpawner.Framework.Models
{
    /// <summary>Predefined mod data.</summary>
    internal record ModItemData
    {
        /*********
        ** Accessors
        *********/
        /// <summary>Items which should be hidden by default because they cause in-game bugs or crashes.</summary>
        public string[] ProblematicItems { get; }


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="problematicItems">Items which should be hidden by default because they cause in-game bugs or crashes.</param>
        public ModItemData(string[]? problematicItems)
        {
            this.ProblematicItems = problematicItems ?? Array.Empty<string>();
        }
    }
}
