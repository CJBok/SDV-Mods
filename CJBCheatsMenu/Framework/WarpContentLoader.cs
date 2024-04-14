using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CJBCheatsMenu.Framework.ContentModels;
using CJBCheatsMenu.Framework.Models;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Extensions;

namespace CJBCheatsMenu.Framework
{
    /// <summary>Manages building and loading the warp data assets.</summary>
    internal class WarpContentLoader
    {
        /*********
        ** Fields
        *********/
        /// <summary>The asset name for the warp sections.</summary>
        private readonly string SectionsAssetName;

        /// <summary>The asset name for the warps.</summary>
        private readonly string WarpsAssetName;

        /// <summary>The mod data containing the default warps to show in the UI.</summary>
        private readonly ModData ModData;

        /// <summary>Encapsulates monitoring and logging.</summary>
        private readonly IMonitor Monitor;

        /// <summary>The mod registry with which to validate unique string IDs.</summary>
        private readonly IModRegistry ModRegistry;

        /// <summary>The default section IDs listed in the <see cref="ModData"/>.</summary>
        private readonly HashSet<string> DefaultSectionIds = new(StringComparer.OrdinalIgnoreCase);

        /// <summary>The default warp IDs listed in the <see cref="ModData"/>.</summary>
        private readonly HashSet<string> DefaultWarpIds = new(StringComparer.OrdinalIgnoreCase);


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="modId">The unique mod ID for CJB Cheats Menu.</param>
        /// <param name="modData">The mod data containing the default warps to show in the UI.</param>
        /// <param name="monitor">Encapsulates monitoring and logging.</param>
        /// <param name="modRegistry">The mod registry with which to validate unique string IDs.</param>
        public WarpContentLoader(string modId, ModData modData, IMonitor monitor, IModRegistry modRegistry)
        {
            this.SectionsAssetName = $"Mods/{modId}/WarpSections";
            this.WarpsAssetName = $"Mods/{modId}/Warps";
            this.ModData = modData;
            this.Monitor = monitor;
            this.ModRegistry = modRegistry;

            this.DefaultSectionIds.AddRange(modData.SectionOrder.Select(this.GetContentId));
            foreach (ModDataWarp[] section in modData.Warps.Values)
                this.DefaultWarpIds.AddRange(section.Select(warp => warp.Id ?? this.GetContentId(warp.DisplayText)));
        }

        /// <inheritdoc cref="IContentEvents.AssetRequested"/>
        /// <param name="e">The event arguments.</param>
        public void OnAssetRequested(AssetRequestedEventArgs e)
        {
            if (e.Name.IsEquivalentTo(this.SectionsAssetName))
                e.LoadFrom(this.BuildDefaultWarpSections, AssetLoadPriority.Exclusive);

            else if (e.Name.IsEquivalentTo(this.WarpsAssetName))
                e.LoadFrom(this.BuildDefaultWarps, AssetLoadPriority.Exclusive);
        }

        /// <summary>Load the warp sections with any mod edits applied.</summary>
        [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract", Justification = "This is the method which enforces the contract.")]
        public List<WarpSectionContentModel> LoadWarpSections()
        {
            var sections = Game1.content.Load<List<WarpSectionContentModel>>(this.SectionsAssetName);

            HashSet<string> seenIds = new(StringComparer.OrdinalIgnoreCase);
            sections.RemoveAll(section =>
            {
                if (section is null)
                    return true;

                if (string.IsNullOrWhiteSpace(section.Id))
                {
                    this.Monitor.Log($"Ignored invalid warp section in {this.SectionsAssetName} which has no ID.", LogLevel.Warn);
                    return true;
                }

                if (!seenIds.Add(section.Id))
                {
                    this.Monitor.Log($"Ignored invalid warp section with duplicate ID '{section.Id}' in {this.SectionsAssetName}.", LogLevel.Warn);
                    return true;
                }

                if (!this.DefaultSectionIds.Contains(section.Id) && !this.IsValidUniqueStringId(section.Id))
                {
                    this.Monitor.Log($"Ignored invalid warp section with ID '{section.Id}' in {this.SectionsAssetName} because it either doesn't apply the unique string ID convention, or the prefix doesn't match an installed mod.", LogLevel.Warn);
                    return true;
                }

                if (string.IsNullOrWhiteSpace(section.DisplayName))
                {
                    this.Monitor.Log($"Ignored invalid warp section '{section.Id}' in {this.SectionsAssetName} which has no display name.", LogLevel.Warn);
                    return true;
                }

                return false;
            });

            return sections;
        }

        /// <summary>Load the warps with any mod edits applied.</summary>
        [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract", Justification = "This is the method which enforces the contract.")]
        public List<WarpContentModel> LoadWarps()
        {
            var warps = Game1.content.Load<List<WarpContentModel>>(this.WarpsAssetName);

            HashSet<string> seenIds = new(StringComparer.OrdinalIgnoreCase);
            warps.RemoveAll(warp =>
            {
                if (warp is null)
                    return true;

                if (string.IsNullOrWhiteSpace(warp.Id))
                {
                    this.Monitor.Log($"Ignored invalid warp in {this.WarpsAssetName} which has no ID.", LogLevel.Warn);
                    return true;
                }

                if (!seenIds.Add(warp.Id))
                {
                    this.Monitor.Log($"Ignored invalid warp with duplicate ID '{warp.Id}' in {this.WarpsAssetName}.", LogLevel.Warn);
                    return true;
                }

                if (!this.DefaultWarpIds.Contains(warp.Id) && !this.IsValidUniqueStringId(warp.Id))
                {
                    this.Monitor.Log($"Ignored invalid warp with ID '{warp.Id}' in {this.WarpsAssetName} because it either doesn't apply the unique string ID convention, or the prefix doesn't match an installed mod.", LogLevel.Warn);
                    return true;
                }

                if (string.IsNullOrWhiteSpace(warp.SectionId))
                {
                    this.Monitor.Log($"Ignored invalid warp '{warp.Id}' in {this.WarpsAssetName} which has no section ID.", LogLevel.Warn);
                    return true;
                }

                if (string.IsNullOrWhiteSpace(warp.DisplayName))
                {
                    this.Monitor.Log($"Ignored invalid warp '{warp.Id}' in {this.WarpsAssetName} which has no display text.", LogLevel.Warn);
                    return true;
                }

                if (string.IsNullOrWhiteSpace(warp.Location))
                {
                    this.Monitor.Log($"Ignored invalid warp '{warp.Id}' in {this.WarpsAssetName} which has no target location.", LogLevel.Warn);
                    return true;
                }

                return false;
            });

            return warps;
        }

        /// <summary>Get whether any warps were edited by other mods.</summary>
        /// <param name="warps">The loaded warps.</param>
        public bool IsCustomizedWarpList(IReadOnlyList<WarpContentModel> warps)
        {
            Dictionary<string, WarpContentModel> defaultWarps = this.BuildDefaultWarps().ToDictionary(p => p.Id, StringComparer.OrdinalIgnoreCase);

            // detect changes and new entries
            HashSet<string> seenIds = new HashSet<string>();
            foreach (WarpContentModel warp in warps)
            {
                if (!defaultWarps.TryGetValue(warp.Id, out WarpContentModel? defaultWarp))
                    return true;

                seenIds.Add(warp.Id);

                if (!warp.HasSameFieldsAs(defaultWarp))
                    return true;
            }

            // detect deleted entries
            foreach (string id in defaultWarps.Keys)
            {
                if (!seenIds.Contains(id))
                    return true;
            }

            return false;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Get the content model for the default warp sections.</summary>
        private List<WarpSectionContentModel> BuildDefaultWarpSections()
        {
            List<WarpSectionContentModel> sections = new();

            int order = 0;
            foreach (string translationKey in this.ModData.SectionOrder)
            {
                sections.Add(new WarpSectionContentModel
                {
                    Id = this.GetContentId(translationKey),
                    DisplayName = I18n.GetByKey(translationKey),
                    Order = ++order * 100
                });
            }

            return sections;
        }

        /// <summary>Get the content model for the default warps.</summary>
        private List<WarpContentModel> BuildDefaultWarps()
        {
            List<WarpContentModel> warps = new();

            HashSet<string> seenIds = new();

            foreach ((string sectionTranslationKey, ModDataWarp[] sectionWarps) in this.ModData.Warps)
            {
                string sectionId = this.GetContentId(sectionTranslationKey);

                foreach (ModDataWarp warp in sectionWarps.OrderBy(p => p.Order).ThenBy(p => p.DisplayText, StringComparer.OrdinalIgnoreCase))
                {
                    string warpId = warp.Id ?? this.GetContentId(warp.DisplayText);
                    if (!seenIds.Add(warpId))
                    {
                        this.Monitor.Log($"Ignored duplicate ID '{warpId}' in assets/warps.json; was this file edited manually?", LogLevel.Warn);
                        continue;
                    }

                    warps.Add(new WarpContentModel
                    {
                        Id = warpId,
                        SectionId = sectionId,
                        DisplayName = I18n.GetByKey(warp.DisplayText),
                        Location = warp.Location ?? "Farm",
                        Tile = warp.Tile,
                        Order = warp.Order,
                        Condition = warp.Condition
                    });
                }
            }

            return warps;
        }

        /// <summary>Get the content ID from a data translation key.</summary>
        /// <param name="translationKey">The translation key.</param>
        private string GetContentId(string translationKey)
        {
            // Before 1.36.0, data warps and sections used the translation key as the ID. The data format is unchanged
            // for backwards compatibility with older custom warp files, so convert the key into the content ID.
            const string warpPrefix = "warp.";
            const string sectionPrefix = "warp-section.";

            if (translationKey.StartsWith(warpPrefix))
                return translationKey[warpPrefix.Length..];

            if (translationKey.StartsWith(sectionPrefix))
                return translationKey[sectionPrefix.Length..];

            return translationKey;
        }

        /// <summary>Get whether a custom section or warp ID follows the <a href="https://stardewvalleywiki.com/Modding:Common_data_field_types#Unique_string_ID">unique string item ID convention</a>.</summary>
        /// <param name="id">The section or warp ID to validate.</param>
        private bool IsValidUniqueStringId(string id)
        {
            if (this.ModRegistry.IsLoaded(id))
                return true;

            // The unique string ID convention is `{mod id}_{item id}`, but both the mod ID and item ID can contain
            // underscores. So here we split by `_` and check every possible prefix before the final underscore to see
            // if it's a valid mod ID. We take the longest match since some mods use suffixes for grouped mods, like
            // `mainMod` and `mainMod_cp`.
            string[] parts = id.Split('_');
            {
                string modId = parts[0];
                int itemIdIndex = parts.Length - 1;
                for (int i = 0; i < itemIdIndex; i++)
                {
                    if (i != 0)
                        modId += '_' + parts[i];

                    if (this.ModRegistry.IsLoaded(modId))
                        return true;
                }
            }

            return false;
        }
    }
}
