using System;
using System.Collections.Generic;
using System.Linq;
using CJB.Common;
using CJBCheatsMenu.Framework.Components;
using CJBCheatsMenu.Framework.ContentModels;
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
        /// <summary>Manages building and loading the warp data assets.</summary>
        private readonly WarpContentLoader WarpContentLoader;


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="warpContentLoader">Manages building and loading the warp data assets.</param>
        public WarpCheat(WarpContentLoader warpContentLoader)
        {
            this.WarpContentLoader = warpContentLoader;
        }

        /// <summary>Get the config UI fields to show in the cheats menu.</summary>
        /// <param name="context">The cheat context.</param>
        public override IEnumerable<OptionsElement> GetFields(CheatContext context)
        {
            bool isJojaMember = this.HasFlag("JojaMember");

            IReadOnlyList<WarpSectionContentModel> rawSections = this.WarpContentLoader.LoadWarpSections();
            IReadOnlyList<WarpContentModel> rawWarps = this.WarpContentLoader.LoadWarps();

            Dictionary<string, string> sectionNames = rawSections.ToDictionary(p => p.Id, p => p.DisplayName);
            Dictionary<string, List<WarpContentModel>> warps = this.GetWarpsBySection(rawSections, rawWarps);

            if (this.WarpContentLoader.IsCustomizedWarpList(rawWarps))
                yield return new DescriptionElement(I18n.Warp_CustomizedWarning());

            foreach ((string sectionKey, List<WarpContentModel> sectionWarps) in warps)
            {
                // section title
                yield return new OptionsElement($"{sectionNames.GetValueOrDefault(sectionKey, sectionKey)}:");

                // warps
                foreach (WarpContentModel warp in sectionWarps)
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
                        label: warp.DisplayName,
                        slotWidth: context.SlotWidth,
                        toggle: warp.SpecialBehavior switch
                        {
                            WarpBehavior.Farm => this.WarpToFarm,
                            _ => () => this.Warp(warp.Location, (int)warp.Tile.X, (int)warp.Tile.Y)
                        }
                    );
                }
            }
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Get the warps to show in the menu grouped by section.</summary>
        /// <param name="warpSections">The raw warp sections to show in the menu.</param>
        /// <param name="warps">The raw warps to show in the menu.</param>
        private Dictionary<string, List<WarpContentModel>> GetWarpsBySection(IReadOnlyList<WarpSectionContentModel> warpSections, IReadOnlyList<WarpContentModel> warps)
        {
            Dictionary<string, List<WarpContentModel>> sections = new(StringComparer.OrdinalIgnoreCase);

            // add sections
            foreach (WarpSectionContentModel section in warpSections)
                sections.TryAdd(section.Id, new());

            // add warps
            foreach (WarpContentModel warp in warps)
            {
                if (!sections.TryGetValue(warp.SectionId, out List<WarpContentModel>? list))
                    sections[warp.SectionId] = list = new();

                list.Add(warp);
            }

            return sections;
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
