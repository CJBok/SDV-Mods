using System;
using System.Collections.Generic;

namespace CJBCheatsMenu.Framework.Models;

/// <summary>The model for the data file.</summary>
internal record ModData
{
    /*********
    ** Accessors
    *********/
    /// <summary>The order in which to list warp sections in the menu. Any other sections will appear alphabetically after these.</summary>
    public string[] SectionOrder { get; }

    /// <summary>The warps to show in the cheats menu.</summary>
    public Dictionary<string, ModDataWarp[]> Warps { get; }


    /*********
    ** Public methods
    *********/
    /// <summary>Construct an instance.</summary>
    /// <param name="sectionOrder">The order in which to list warp sections in the menu. Any other sections will appear alphabetically after these.</param>
    /// <param name="warps">The warps to show in the cheats menu.</param>
    public ModData(string[]? sectionOrder, Dictionary<string, ModDataWarp[]>? warps)
    {
        this.SectionOrder = sectionOrder ?? Array.Empty<string>();
        this.Warps = warps ?? new();
    }
}
