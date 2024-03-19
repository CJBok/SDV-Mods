using System;
using System.Collections.Generic;
using System.Linq;
using CJB.Common;
using CJBCheatsMenu.Framework.Components;
using CJBCheatsMenu.Framework.Models;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.Cheats.Warps
{
    /// <summary>A cheat which warps the player to selected locations.</summary>
    internal class WarpCheat : BaseCheat
    {
        /*********
        ** Fields
        *********/
        /// <summary>Get the warp data.</summary>
        private readonly Func<ModData> GetWarpData;


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="getWarps">Get the warp data.</param>
        public WarpCheat(Func<ModData> getWarps)
        {
            this.GetWarpData = getWarps;
        }

        /// <summary>Get the config UI fields to show in the cheats menu.</summary>
        /// <param name="context">The cheat context.</param>
        public override IEnumerable<OptionsElement> GetFields(CheatContext context)
        {
            bool isJojaMember = this.HasFlag("JojaMember");

            ModData warpData = this.GetWarpData();
            IDictionary<string, int> sectionOrder = this.GetSectionOrder(warpData);
            IDictionary<string, ModDataWarp[]> warpsBySection = this.GetWarpsBySection(warpData);

            foreach ((string sectionKey, string sectionLabel) in this.GetSections(sectionOrder, warpsBySection))
            {
                // section title
                yield return new OptionsElement($"{sectionLabel}:");

                // warps
                foreach ((ModDataWarp warp, string warpLabel) in this.GetWarps(warpsBySection, sectionKey))
                {
                    // skip warps that don't apply
                    switch (warp.SpecialBehavior)
                    {
                        case WarpBehavior.Casino when !Game1.player.hasClubCard:
                        case WarpBehavior.CommunityCenter when isJojaMember:
                        case WarpBehavior.JojaMart when !isJojaMember && CommonHelper.GetIsCommunityCenterComplete():
                        case WarpBehavior.MovieTheaterCommunity when isJojaMember || !this.HasFlag("ccMovieTheater"):
                        case WarpBehavior.MovieTheaterJoja when !isJojaMember || !this.HasFlag("ccMovieTheater"):
                            continue;
                    }

                    // get warp button
                    yield return new CheatsOptionsButton(
                        label: warpLabel,
                        slotWidth: context.SlotWidth,
                        toggle: warp.SpecialBehavior switch
                        {
                            WarpBehavior.Farm => this.WarpToFarm,
                            _ => () => this.Warp(warp.Location ?? "Farm", (int)warp.Tile.X, (int)warp.Tile.Y)
                        }
                    );
                }
            }
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Get the order in which sections should be rendered based on the <see cref="ModData.SectionOrder"/> data.</summary>
        /// <param name="warpData">The underlying warp data.</param>
        private IDictionary<string, int> GetSectionOrder(ModData warpData)
        {
            return warpData.SectionOrder
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Select((section, index) => new { section, index })
                .ToDictionary(p => p.section, p => p.index, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>Get the available warps indexed by section.</summary>
        /// <param name="warpData">The underlying warp data.</param>
        private IDictionary<string, ModDataWarp[]> GetWarpsBySection(ModData warpData)
        {
            return warpData.Warps
                .GroupBy(p => p.Key, p => p.Value, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(p => p.Key, p => p.SelectMany(x => x).ToArray(), StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>Get the section IDs and display names in sorted order.</summary>
        /// <param name="sectionOrder">The order in which sections should be rendered based on the <see cref="ModData.SectionOrder"/> data.</param>
        /// <param name="warpsBySection">The available warps indexed by section.</param>
        private IEnumerable<KeyValuePair<string, string>> GetSections(IDictionary<string, int> sectionOrder, IDictionary<string, ModDataWarp[]> warpsBySection)
        {
            return
                (
                    from key in warpsBySection.Keys
                    let label = I18n.GetByKey(key).Default(key)
                    let order = sectionOrder.TryGetValue(key, out int order) ? order : int.MaxValue
                    orderby order, label
                    select new KeyValuePair<string, string>(key, label)
                );
        }

        /// <summary>Get the warps and display names in sorted order.</summary>
        /// <param name="warpsBySection">The available warps indexed by section.</param>
        /// <param name="section">The section whose warps to get.</param>
        private IEnumerable<(ModDataWarp Warp, string Label)> GetWarps(IDictionary<string, ModDataWarp[]> warpsBySection, string section)
        {
            if (!warpsBySection.TryGetValue(section, out ModDataWarp[]? warps))
                return Enumerable.Empty<(ModDataWarp, string)>();

            return
                (
                    from warp in warps
                    let label = I18n.GetByKey(warp.DisplayText).Default(warp.DisplayText ?? "???").ToString()
                    orderby warp.Order, label
                    select (warp, label)
                );
        }

        /// <summary>Warp the player to the given location.</summary>
        /// <param name="locationName">The location name.</param>
        /// <param name="tileX">The tile X position.</param>
        /// <param name="tileY">The tile Y position.</param>
        private void Warp(string locationName, int tileX, int tileY)
        {
            // reset state
            Game1.exitActiveMenu();
            Game1.player.swimming.Value = false;
            Game1.player.changeOutOfSwimSuit();

            // warp
            Game1.warpFarmer(locationName, tileX, tileY, false);
        }

        /// <summary>Warp the player to the farm.</summary>
        private void WarpToFarm()
        {
            // try to drop farmhand in front of their cabin
            string cabinName = Game1.player.homeLocation.Value;
            if (!Context.IsMainPlayer && cabinName != null)
            {
                foreach (GameLocation location in Game1.locations)
                {
                    foreach (Building building in location.buildings)
                    {
                        if (building.indoors.Value?.uniqueName.Value == cabinName)
                        {
                            int tileX = building.tileX.Value + building.humanDoor.X;
                            int tileY = building.tileY.Value + building.humanDoor.Y + 1;
                            this.Warp(location.Name, tileX, tileY);
                            return;
                        }
                    }
                }
            }

            // else farmhouse
            Point farmhousePos = Game1.getFarm().GetMainFarmHouseEntry();
            this.Warp("Farm", farmhousePos.X, farmhousePos.Y);
        }
    }
}
