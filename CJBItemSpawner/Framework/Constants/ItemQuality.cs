using System;
using SObject = StardewValley.Object;

namespace CJBItemSpawner.Framework.Constants
{
    /// <summary>Indicates an item quality. (Higher-quality items are sold at a higher price.)</summary>
    internal enum ItemQuality
    {
        Normal = SObject.lowQuality,
        Silver = SObject.medQuality,
        Gold = SObject.highQuality,
        Iridium = SObject.bestQuality
    }

    /// <summary>Extension methods for <see cref="ItemQuality"/>.</summary>
    internal static class ItemQualityExtensions
    {
        /*********
        ** Public methods
        *********/
        /// <summary>Get the previous quality.</summary>
        /// <param name="current">The current quality.</param>
        public static ItemQuality GetPrevious(this ItemQuality current)
        {
            return current switch
            {
                ItemQuality.Normal => ItemQuality.Normal,
                ItemQuality.Silver => ItemQuality.Normal,
                ItemQuality.Gold => ItemQuality.Silver,
                ItemQuality.Iridium => ItemQuality.Gold,
                _ => throw new NotSupportedException($"Unknown quality '{current}'.")
            };
        }

        /// <summary>Get the next better quality.</summary>
        /// <param name="current">The current quality.</param>
        public static ItemQuality GetNext(this ItemQuality current)
        {
            return current switch
            {
                ItemQuality.Normal => ItemQuality.Silver,
                ItemQuality.Silver => ItemQuality.Gold,
                ItemQuality.Gold => ItemQuality.Iridium,
                ItemQuality.Iridium => ItemQuality.Iridium,
                _ => throw new NotSupportedException($"Unknown quality '{current}'.")
            };
        }
    }
}
