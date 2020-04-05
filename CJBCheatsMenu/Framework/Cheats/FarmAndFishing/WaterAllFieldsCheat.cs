using System.Collections.Generic;
using System.Linq;
using CJBCheatsMenu.Framework.Components;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;

namespace CJBCheatsMenu.Framework.Cheats.FarmAndFishing
{
    /// <summary>A cheat which waters all current fields.</summary>
    internal class WaterAllFieldsCheat : BaseCheat
    {
        /*********
        ** Public methods
        *********/
        /// <summary>Get the config UI fields to show in the cheats menu.</summary>
        /// <param name="context">The cheat context.</param>
        public override IEnumerable<OptionsElement> GetFields(CheatContext context)
        {
            yield return new CheatsOptionsButton(
                label: context.Text.Get("farm.water-all-fields"),
                slotWidth: context.SlotWidth,
                toggle: () => this.WaterAllFields(context.GetAllLocations())
            );
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Water all current fields.</summary>
        /// <param name="locations">The locations to update.</param>
        public void WaterAllFields(IEnumerable<GameLocation> locations)
        {
            Game1.soundBank.PlayCue("glug");

            foreach (GameLocation location in locations)
            {
                if (location.IsFarm || location.IsGreenhouse)
                {
                    foreach (TerrainFeature terrainFeature in location.terrainFeatures.Values)
                    {
                        if (terrainFeature is HoeDirt dirt)
                            dirt.state.Value = HoeDirt.watered;
                    }
                }

                foreach (IndoorPot pot in location.objects.Values.OfType<IndoorPot>())
                {
                    HoeDirt dirt = pot.hoeDirt.Value;
                    if (dirt != null)
                    {
                        dirt.state.Value = HoeDirt.watered;
                        pot.showNextIndex.Value = true;
                    }
                }
            }
        }
    }
}
