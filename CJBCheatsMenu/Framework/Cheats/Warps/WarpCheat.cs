using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <summary>The available warps.</summary>
        private readonly ModDataWarp[] Warps;

        /// <summary>A lookup of mod data warps by ID.</summary>
        private readonly IDictionary<string, ModDataWarp> WarpsById;


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="warps">The available warps.</param>
        public WarpCheat(ModDataWarp[] warps)
        {
            this.Warps = warps;
            this.WarpsById = warps
                .Where(warp => !string.IsNullOrWhiteSpace(warp.DisplayText))
                .GroupBy(warp => warp.DisplayText?.Trim(), StringComparer.InvariantCultureIgnoreCase)
                .ToDictionary(
                    p => p.Key,
                    p => p.First(),
                    StringComparer.InvariantCultureIgnoreCase
                );
        }

        /// <summary>Get the config UI fields to show in the cheats menu.</summary>
        /// <param name="context">The cheat context.</param>
        public override IEnumerable<OptionsElement> GetFields(CheatContext context)
        {
            // farm
            yield return this.GetField(context, "warp.farm", this.WarpToFarm);

            // casino
            if (Game1.player.hasClubCard)
                yield return this.GetField(context, "warp.casino", this.WarpToCasino);

            // data warps
            var warpFields = this.Warps
                .Where(warp => !warp.HasId("warp.farm") && !warp.HasId("warp.casino"))
                .Select(warp => this.GetField(context, warp))
                .OrderBy(field => field.label);
            foreach (var field in warpFields)
                yield return field;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Get a warp field.</summary>
        /// <param name="context">The cheat context.</param>
        /// <param name="translationKey">The warp translation key or display text.</param>
        /// <param name="warp">The warp action.</param>
        private CheatsOptionsButton GetField(CheatContext context, string translationKey, Action warp)
        {
            return new CheatsOptionsButton(
                label: context.Text.Get(translationKey).Default(translationKey),
                slotWidth: context.SlotWidth,
                toggle: warp
            );
        }

        /// <summary>Get a warp field.</summary>
        /// <param name="context">The cheat context.</param>
        /// <param name="warp">The warp.</param>
        private CheatsOptionsButton GetField(CheatContext context, ModDataWarp warp)
        {
            return this.GetField(context, warp.DisplayText, () => this.Warp(warp));
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

        /// <summary>Warp the player to the given location.</summary>
        /// <param name="warp">The warp info.</param>
        private void Warp(ModDataWarp warp)
        {
            this.Warp(warp.Location, (int)warp.Tile.X, (int)warp.Tile.Y);
        }

        /// <summary>Warp the player to the casino.</summary>
        private void WarpToCasino()
        {
            if (this.WarpsById.TryGetValue("warp.casino", out ModDataWarp warp))
                this.Warp(warp);
            else
                this.Warp("Club", 8, 11);
        }

        /// <summary>Warp the player to the farm.</summary>
        private void WarpToFarm()
        {
            // apply override
            if (this.WarpsById.TryGetValue("warp.farm", out ModDataWarp warpOverride))
            {
                this.Warp(warpOverride);
                return;
            }

            // else try to drop farmhand in front of their cabin
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
