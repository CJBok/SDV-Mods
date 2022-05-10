using System.Collections.Generic;
using System.Linq;
using CJBCheatsMenu.Framework.Components;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using SObject = StardewValley.Object;

namespace CJBCheatsMenu.Framework.Cheats.PlayerAndTools
{
    /// <summary>A cheat which enables one-hit break.</summary>
    internal class OneHitBreakCheat : BaseCheat
    {
        /*********
        ** Public methods
        *********/
        /// <summary>Get the config UI fields to show in the cheats menu.</summary>
        /// <param name="context">The cheat context.</param>
        public override IEnumerable<OptionsElement> GetFields(CheatContext context)
        {
            yield return new CheatsOptionsCheckbox(
                label: I18n.Tools_OneHitBreak(),
                value: context.Config.OneHitBreak,
                setValue: value => context.Config.OneHitBreak = value
            );
        }

        /// <summary>Handle the cheat options being loaded or changed.</summary>
        /// <param name="context">The cheat context.</param>
        /// <param name="needsUpdate">Whether the cheat should be notified of game updates.</param>
        /// <param name="needsInput">Whether the cheat should be notified of button presses.</param>
        /// <param name="needsRendering">Whether the cheat should be notified of render ticks.</param>
        public override void OnConfig(CheatContext context, out bool needsInput, out bool needsUpdate, out bool needsRendering)
        {
            needsInput = false;
            needsUpdate = context.Config.OneHitBreak;
            needsRendering = false;
        }

        /// <summary>Handle a game update if <see cref="ICheat.OnSaveLoaded"/> indicated updates were needed.</summary>
        /// <param name="context">The cheat context.</param>
        /// <param name="e">The update event arguments.</param>
        public override void OnUpdated(CheatContext context, UpdateTickedEventArgs e)
        {
            // skip if not using a tool
            if (!Context.IsPlayerFree || !Game1.player.UsingTool || Game1.player.CurrentTool is not (Axe or Pickaxe))
                return;

            Farmer player = Game1.player;
            var tool = player.CurrentTool;
            var location = player.currentLocation;
            if (location == null)
                return;

            // get affected tile
            Vector2 tile = new((int)player.GetToolLocation().X / Game1.tileSize, (int)player.GetToolLocation().Y / Game1.tileSize);

            // break stones
            if (tool is Pickaxe && location.objects.TryGetValue(tile, out SObject obj) && obj?.name == "Stone")
                obj.MinutesUntilReady = 0;

            // break trees
            if (tool is Axe && location.terrainFeatures.TryGetValue(tile, out TerrainFeature feature))
            {
                if (feature is Tree tree && tree.health.Value > 1)
                    tree.health.Value = 1;
                else if (feature is FruitTree fruitTree && fruitTree.health.Value > 1)
                    fruitTree.health.Value = 1;
            }

            // break resource clumps
            foreach (ResourceClump? clump in this.GetResourceClumps(location))
            {
                if (clump != null && clump.getBoundingBox(clump.tile.Value).Contains((int)player.GetToolLocation().X, (int)player.GetToolLocation().Y) && clump.health.Value > 0)
                    clump.health.Value = 0;
            }
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Get the resource clumps in a location.</summary>
        /// <param name="location">The location to check.</param>
        private IEnumerable<ResourceClump?> GetResourceClumps(GameLocation location)
        {
            IEnumerable<ResourceClump> clumps = location.resourceClumps;

            switch (location)
            {
                case Forest forest when forest.log != null:
                    clumps = clumps.Concat(new[] { forest.log });
                    break;

                case Woods woods when woods.stumps.Any():
                    clumps = clumps.Concat(woods.stumps);
                    break;
            }

            return clumps;
        }
    }
}
