using System.Collections.Generic;
using System.Linq;
using CJBCheatsMenu.Framework.Components;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;

namespace CJBCheatsMenu.Framework.Cheats.FarmAndFishing
{
    /// <summary>A cheat which automatically waters crops.</summary>
    internal class AutoWaterCheat : BaseCheat
    {
        /*********
        ** Public methods
        *********/
        /// <inheritdoc />
        public override IEnumerable<OptionsElement> GetFields(CheatContext context)
        {
            yield return new CheatsOptionsCheckbox(
                label: I18n.Farm_AutoWater(),
                value: context.Config.AutoWater,
                setValue: value => context.Config.AutoWater = value
            );
        }

        /// <inheritdoc />
        public override void OnConfig(CheatContext context, out bool needsInput, out bool needsUpdate, out bool needsRendering)
        {
            needsInput = false;
            needsUpdate = context.Config.AutoWater;
            needsRendering = false;
        }

        /// <inheritdoc />
        public override void OnUpdated(CheatContext context, UpdateTickedEventArgs e)
        {
            if (!e.IsOneSecond || !Context.IsWorldReady)
                return;

            foreach (GameLocation location in context.GetAllLocations())
            {
                foreach (HoeDirt dirt in location.terrainFeatures.Values.OfType<HoeDirt>())
                    dirt.state.Value = HoeDirt.watered;

                foreach (IndoorPot pot in location.objects.Values.OfType<IndoorPot>())
                {
                    HoeDirt? dirt = pot.hoeDirt.Value;
                    if (dirt?.crop != null)
                    {
                        dirt.state.Value = HoeDirt.watered;
                        pot.showNextIndex.Value = true;
                    }
                }
            }
        }
    }
}
