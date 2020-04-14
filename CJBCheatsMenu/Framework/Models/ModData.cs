using System.Collections.Generic;

namespace CJBCheatsMenu.Framework.Models
{
    /// <summary>The model for the data file.</summary>
    internal class ModData
    {
        /*********
        ** Accessors
        *********/
        /// <summary>The order in which to list warp sections in the menu. Any other sections will appear alphabetically after these.</summary>
        public string[] SectionOrder { get; set; }

        /// <summary>The warps to show in the cheats menu.</summary>
        public IDictionary<string, ModDataWarp[]> Warps { get; set; }
    }
}
