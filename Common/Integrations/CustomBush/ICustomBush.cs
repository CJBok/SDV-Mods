using System.Collections.Generic;
using StardewValley;
using StardewValley.GameData;

namespace CJB.Common.Integrations.CustomBush;

/// <summary>Model used for custom bushes.</summary>
public interface ICustomBush
{
    /// <summary>Gets the age needed to produce.</summary>
    int AgeToProduce { get; }

    /// <summary>Gets the day of month to begin producing.</summary>
    int DayToBeginProducing { get; }

    /// <summary>Gets the description of the bush.</summary>
    string Description { get; }

    /// <summary>Gets the display name of the bush.</summary>
    string DisplayName { get; }

    /// <summary>Gets the default texture used when planted indoors.</summary>
    string IndoorTexture { get; }

    /// <summary>Gets the season in which this bush will produce its drops.</summary>
    List<Season> Seasons { get; }

    /// <summary>Gets the rules which override the locations that custom bushes can be planted in.</summary>
    List<PlantableRule> PlantableLocationRules { get; }

    /// <summary>Gets the texture of the tea bush.</summary>
    string Texture { get; }

    /// <summary>Gets the row index for the custom bush's sprites.</summary>
    int TextureSpriteRow { get; }
}
