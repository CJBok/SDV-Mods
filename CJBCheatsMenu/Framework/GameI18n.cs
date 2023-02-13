using System.Diagnostics.CodeAnalysis;
using StardewValley;
using StardewValley.ItemTypeDefinitions;

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
        /// <param name="id">The unqualified big craftable ID.</param>
        public static string GetBigCraftableName(string id)
        {
            if (Game1.bigCraftableData is null)
                return "(missing translation: game hasn't loaded big craftable data yet)";

            ParsedItemData? data = ItemRegistry.GetData(ItemRegistry.type_bigCraftable + id);
            if (data is null)
                return $"(missing translation: no big craftable #{id})";

            return data.DisplayName;
        }

        /// <summary>Get the translated name for an object.</summary>
        /// <param name="id">The unqualified object ID.</param>
        public static string GetObjectName(string id)
        {
            if (Game1.objectData is null)
                return "(missing translation: game hasn't loaded object data yet)";

            ParsedItemData? data = ItemRegistry.GetData(ItemRegistry.type_object + id);
            if (data is null)
                return $"(missing translation: no object #{id})";

            return data.DisplayName;
        }
    }
}
