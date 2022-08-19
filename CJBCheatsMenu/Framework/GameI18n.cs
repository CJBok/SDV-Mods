using System.Collections.Generic;
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

        /// <summary>Get the translated name for a building.</summary>
        /// <param name="id">The building ID.</param>
        public static string GetBuildingName(string id)
        {
            var data = Game1.content.Load<Dictionary<string, string>>("Data\\Blueprints");

            if (!data.TryGetValue(id, out string? rawData))
                return $"(missing translation: no building with ID '{id}')";

            string[] fields = rawData.Split('/');
            if (fields.Length <= 8)
                return $"(missing translation: building with ID '{id}' has invalid data)";

            return fields[8];
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
