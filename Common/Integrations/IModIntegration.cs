/*

Copied from https://github.com/Pathoschild/StardewMods/blob/develop/Common/Integrations/IModIntegration.cs.
Copyright Jesse Plamondon-Willard (Pathoschild), released under MIT license: https://github.com/Pathoschild/StardewMods/blob/develop/LICENSE.

*/
namespace CJB.Common.Integrations;

/// <summary>Handles integration with a given mod.</summary>
internal interface IModIntegration
{
    /*********
    ** Accessors
    *********/
    /// <summary>A human-readable name for the mod.</summary>
    string Label { get; }

    /// <summary>Whether the mod is available.</summary>
    bool IsLoaded { get; }
}
