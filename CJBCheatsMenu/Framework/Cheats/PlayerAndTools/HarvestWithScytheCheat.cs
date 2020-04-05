using System;
using System.Collections.Generic;
using System.Linq;
using CJBCheatsMenu.Framework.Components;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;

namespace CJBCheatsMenu.Framework.Cheats.PlayerAndTools
{
    /// <summary>A cheat which enables harvesting all crops with the scythe.</summary>
    internal class HarvestWithScytheCheat : BaseCheat
    {
        /*********
        ** Public methods
        *********/
        /// <summary>Get the config UI fields to show in the cheats menu.</summary>
        /// <param name="context">The cheat context.</param>
        public override IEnumerable<OptionsElement> GetFields(CheatContext context)
        {
            yield return new CheatsOptionsCheckbox(
                label: context.Text.Get("tools.harvest-with-scythe"),
                value: context.Config.HarvestScythe,
                setValue: value => context.Config.HarvestScythe = value
            );
        }

        /// <summary>Handle the cheat options being loaded or changed.</summary>
        /// <param name="context">The cheat context.</param>
        /// <param name="needsUpdate">Whether the cheat should be notified of game updates.</param>
        /// <param name="needsInput">Whether the cheat should be notified of button presses.</param>
        /// <param name="needsRendering">Whether the cheat should be notified of render ticks.</param>
        public override void OnConfig(CheatContext context, out bool needsInput, out bool needsUpdate, out bool needsRendering)
        {
            bool enabled = context.Config.HarvestScythe;
            needsInput = false;
            needsUpdate = enabled;
            needsRendering = false;

            // disable harvest with scythe
            if (!enabled)
            {
                IDictionary<int, int> cropHarvestMethods = this.GetCropHarvestMethods();
                foreach (Crop crop in context.GetAllLocations().SelectMany(this.GetCropsIn))
                {
                    if (cropHarvestMethods.TryGetValue(crop.indexOfHarvest.Value, out int harvestMethod))
                        crop.harvestMethod.Value = harvestMethod;
                }
            }
        }

        /// <summary>Handle a game update if <see cref="ICheat.OnSaveLoaded"/> indicated updates were needed.</summary>
        /// <param name="context">The cheat context.</param>
        /// <param name="e">The update event arguments.</param>
        public override void OnUpdated(CheatContext context, UpdateTickedEventArgs e)
        {
            if (!e.IsOneSecond || !Context.IsWorldReady)
                return;

            foreach (Crop crop in this.GetCropsIn(Game1.currentLocation))
                crop.harvestMethod.Value = Crop.sickleHarvest;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Get all crops in a location.</summary>
        /// <param name="location">The location to scan.</param>
        private IEnumerable<Crop> GetCropsIn(GameLocation location)
        {
            // planted crops
            if (location.IsFarm || location.IsGreenhouse)
            {
                foreach (HoeDirt dirt in location.terrainFeatures.Values.OfType<HoeDirt>())
                {
                    if (dirt.crop != null)
                        yield return dirt.crop;
                }
            }

            // garden pots
            foreach (IndoorPot pot in location.objects.Values.OfType<IndoorPot>())
            {
                var crop = pot.hoeDirt.Value?.crop;
                if (crop != null)
                    yield return crop;
            }
        }

        /// <summary>Get a crop ID => harvest method lookup.</summary>
        private IDictionary<int, int> GetCropHarvestMethods()
        {
            IDictionary<int, int> lookup = new Dictionary<int, int>();

            IDictionary<int, string> cropData = Game1.content.Load<Dictionary<int, string>>("Data\\Crops");
            foreach (KeyValuePair<int, string> entry in cropData)
            {
                string[] fields = entry.Value.Split('/');
                int cropID = Convert.ToInt32(fields[3]);
                int harvestMethod = Convert.ToInt32(fields[5]);

                if (!lookup.ContainsKey(cropID))
                    lookup.Add(cropID, harvestMethod);
            }

            return lookup;
        }
    }
}
