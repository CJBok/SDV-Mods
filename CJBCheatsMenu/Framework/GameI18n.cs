using System.Diagnostics.CodeAnalysis;
using System.Linq;
using StardewValley;

namespace CJBCheatsMenu.Framework
{
    /// <summary>Provides access to the game's internal translations.</summary>
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Deliberately named to match convention.")]
    internal static class GameI18n
    {
        /*********
        ** Public methods
        *********/
        /// <summary>Get the translated name for a big craftable object.</summary>
        /// <param name="id">The big craftable ID.</param>
        public static string GetBigCraftableName(int id)
        {
            if (Game1.bigCraftablesInformation == null)
                return "(missing translation: game hasn't loaded big craftable data yet)";
            if (!Game1.bigCraftablesInformation.TryGetValue(id, out string? value))
                return $"(missing translation: no big craftable #{id})";

            return value.Split('/').Last();
        }

        /// <summary>Get the translated name for an object.</summary>
        /// <param name="id">The object ID.</param>
        public static string GetObjectName(int id)
        {
            if (Game1.objectInformation == null)
                return "(missing translation: game hasn't loaded object data yet)";
            if (!Game1.objectInformation.TryGetValue(id, out string? value))
                return $"(missing translation: no object #{id})";

            string[] parts = value.Split('/');
            if (parts.Length < 5)
                return $"(missing translation: object #{id} has an invalid format)";

            return parts[4];
        }
    }
}
