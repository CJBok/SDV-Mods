using StardewModdingAPI;
using StardewValley.TerrainFeatures;

namespace CJB.Common.Integrations.CustomBush;

/// <summary>Handles the logic for integrating with the Custom Bush mod.</summary>
internal class CustomBushIntegration : BaseIntegration<ICustomBushApi>
{
    /*********
    ** Public methods
    *********/
    /// <summary>Construct an instance.</summary>
    /// <param name="modRegistry">An API for fetching metadata about loaded mods.</param>
    /// <param name="monitor">Encapsulates monitoring and logging.</param>
    public CustomBushIntegration(IModRegistry modRegistry, IMonitor monitor)
        : base("CustomBush", "furyx639.CustomBush", "1.2.0", modRegistry, monitor) { }

    /// <inheritdoc cref="ICustomBushApi.TryGetCustomBush(Bush, out ICustomBush?)" />
    public bool TryGetCustomBush(Bush bush, out ICustomBush? customBush)
    {
        this.AssertLoaded();

        customBush = this.SafelyCallApi(
            api => api.TryGetCustomBush(bush, out ICustomBush? found) ? found : null,
            "Failed fetching bush from {0}."
        );
        return customBush != null;
    }
}
