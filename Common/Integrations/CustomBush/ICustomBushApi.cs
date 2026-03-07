using System.Diagnostics.CodeAnalysis;
using StardewValley.TerrainFeatures;

namespace CJB.Common.Integrations.CustomBush;

/// <summary>The API provided by the Custom Bush mod.</summary>
[SuppressMessage("ReSharper", "InconsistentNaming", Justification = "The naming convention is defined by the Custom Bush mod.")]
public interface ICustomBushApi
{
    /// <summary>Try to get the custom bush model associated with the given bush.</summary>
    /// <param name="bush">The bush.</param>
    /// <param name="customBush">When this method returns, contains the custom bush associated with the given bush, if found; otherwise, it contains null.</param>
    /// <returns>True if the custom bush associated with the given bush is found; otherwise, false.</returns>
    bool TryGetCustomBush(Bush bush, out ICustomBush? customBush);
}
