using System;
using System.Collections.Generic;
using System.Linq;
using CJBCheatsMenu.Framework.Components;
using CJBCheatsMenu.Framework.ContentModels;
using Microsoft.Xna.Framework;
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

        /// <inheritdoc />
        public override IEnumerable<OptionsElement> GetFields(CheatContext context)
        {
            bool isJojaMember = this.HasFlag("JojaMember");

            WarpSectionContentModel[] rawSections = this.WarpContentLoader.LoadWarpSections();
            WarpContentModel[] rawWarps = this.WarpContentLoader.LoadWarps();

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
                    if (!GameStateQuery.CheckConditions(warp.Condition))
                        continue;

                    // get warp button
                    switch (warp.Location)
                    {
                        case "Farm" when warp.Tile == Vector2.Zero:
                            yield return new CheatsOptionsButton(warp.DisplayName, context.SlotWidth, this.WarpToFarm);
                            break;

                        case "Mine":
                        case "SkullCave":
                            yield return this.CreateMinesButton(warp, context.SlotWidth);
                            break;

                        default:
                            yield return new CheatsOptionsButton(warp.DisplayName, context.SlotWidth, toggle: () => this.Warp(warp.Location, (int)warp.Tile.X, (int)warp.Tile.Y));
                            break;
                    }
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

        /// <summary>Exit the menu and warp the player to the given location.</summary>
        /// <param name="locationName">The location name.</param>
        /// <param name="tileX">The tile X position.</param>
        /// <param name="tileY">The tile Y position.</param>
        private void Warp(string locationName, int tileX, int tileY)
        {
            this.Warp(() => Game1.warpFarmer(locationName, tileX, tileY, false));
        }

        /// <summary>Exit the menu and warp the player to the given location.</summary>
        /// <param name="warp">Perform the actual warp.</param>
        private void Warp(Action warp)
        {
            // reset state
            Game1.exitActiveMenu();
            Game1.player.swimming.Value = false;
            Game1.player.changeOutOfSwimSuit();

            // warp
            warp();
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

        /// <summary>Create a warp option with a mine level selector.</summary>
        /// <param name="warp">The warp data.</param>
        /// <param name="slotWidth">The width of the component to create.</param>
        private CheatsOptionsNumberWheel CreateMinesButton(WarpContentModel warp, int slotWidth)
        {
            const int bottomOfMine = MineShaft.bottomOfMineLevel;
            bool isSkullCavern = warp.Location == "SkullCave";
            bool inQuarryMine = (Game1.currentLocation as MineShaft)?.getMineArea() == MineShaft.quarryMineShaft;

            Func<int, string> formatValue = isSkullCavern
                ? value => (value - bottomOfMine).ToString()
                : value => value.ToString();

            return new CheatsOptionsNumberWheel(
                label: warp.DisplayName,
                slotWidth: slotWidth,
                action: field =>
                {
                    int floor = field.Value;
                    switch (floor)
                    {
                        case 0:
                            this.Warp(warp.Location ?? "Mine", (int)warp.Tile.X, (int)warp.Tile.Y);
                            break;

                        case bottomOfMine when isSkullCavern:
                            this.Warp(warp.Location!, (int)warp.Tile.X, (int)warp.Tile.Y);
                            break;

                        case MineShaft.quarryMineShaft:
                            this.Warp(() => Game1.enterMine(floor + 1)); // skip quarry mine (player can still get there by descending from the previous level though)
                            break;

                        default:
                            this.Warp(() => Game1.enterMine(floor));
                            break;
                    }
                },
                initialValue: inQuarryMine ? 0 : Game1.CurrentMineLevel,
                minValue: isSkullCavern ? bottomOfMine : 0,
                maxValue: isSkullCavern ? 999_999 : bottomOfMine, // the game behaves weirdly with high numbers and we have limited space, so set a semi-reasonable limit
                formatValue: formatValue
            );
        }
    }
}
