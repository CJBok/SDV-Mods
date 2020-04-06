using System;
using System.Collections.Generic;
using System.Linq;
using CJB.Common;
using CJBCheatsMenu.Framework.Components;
using CJBCheatsMenu.Framework.Models;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Locations;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.Cheats.Warps
{
    /// <summary>A cheat which warps the player to selected locations.</summary>
    internal class WarpCheat : BaseCheat
    {
        /*********
        ** Fields
        *********/
        /// <summary>The defined section order.</summary>
        private readonly IDictionary<string, int> SectionOrder;

        /// <summary>Get the available warps indexed by section.</summary>
        private readonly IDictionary<string, ModDataWarp[]> WarpsBySection;


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="warpData">The available warps.</param>
        public WarpCheat(ModData warpData)
        {
            // get defined section order
            this.SectionOrder = warpData.SectionOrder
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Select((section, index) => new { section, index })
                .ToDictionary(p => p.section, p => p.index, StringComparer.OrdinalIgnoreCase);

            // get warps by section
            this.WarpsBySection = warpData.Warps
                .GroupBy(p => p.Key, p => p.Value, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(p => p.Key, p => p.SelectMany(x => x).ToArray(), StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>Get the config UI fields to show in the cheats menu.</summary>
        /// <param name="context">The cheat context.</param>
        public override IEnumerable<OptionsElement> GetFields(CheatContext context)
        {
            bool isJojaMember = this.HasFlag("JojaMember");

            foreach (var section in this.GetSections(context))
            {
                // section title
                yield return new OptionsElement(section.Value + ":");

                // warps
                foreach (var pair in this.GetWarps(context, section.Key))
                {
                    ModDataWarp warp = pair.Item1;
                    string label = pair.Item2;

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
                        label: label,
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
        /// <summary>Get the section IDs and display names in sorted order.</summary>
        /// <param name="context">The cheat context.</param>
        private IEnumerable<KeyValuePair<string, string>> GetSections(CheatContext context)
        {
            return
                (
                    from key in this.WarpsBySection.Keys
                    let label = context.Text.Get(key).Default(key)
                    let order = this.SectionOrder.TryGetValue(key, out int order) ? order : int.MaxValue
                    orderby order, label
                    select new KeyValuePair<string, string>(key, label)
                );
        }

        /// <summary>Get the warps and display names in sorted order.</summary>
        /// <param name="context">The cheat context.</param>
        /// <param name="section">The section whose warps to get.</param>
        private IEnumerable<Tuple<ModDataWarp, string>> GetWarps(CheatContext context, string section)
        {
            if (!this.WarpsBySection.TryGetValue(section, out ModDataWarp[] warps))
                return Enumerable.Empty<Tuple<ModDataWarp, string>>();

            return
                (
                    from warp in warps
                    let label = context.Text.Get(warp.DisplayText).Default(warp.DisplayText ?? "???").ToString()
                    orderby warp.Order, label
                    select Tuple.Create(warp, label)
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
                foreach (BuildableGameLocation location in Game1.locations.OfType<BuildableGameLocation>())
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
            this.Warp("Farm", 64, 15);
        }
    }
}
