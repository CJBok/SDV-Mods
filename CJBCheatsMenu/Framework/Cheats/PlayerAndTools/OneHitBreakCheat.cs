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
                label: context.Text.Get("tools.one-hit-break"),
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
            if (!Context.IsPlayerFree || !Game1.player.UsingTool || !(Game1.player.CurrentTool is Axe || Game1.player.CurrentTool is Pickaxe))
                return;

            Farmer player = Game1.player;
            var tool = player.CurrentTool;

            // get affected tile
            Vector2 tile = new Vector2((int)player.GetToolLocation().X / Game1.tileSize, (int)player.GetToolLocation().Y / Game1.tileSize);

            // break stones
            if (tool is Pickaxe && player.currentLocation.objects.ContainsKey(tile))
            {
                Object obj = player.currentLocation.Objects[tile];
                if (obj != null && obj.name == "Stone")
                    obj.MinutesUntilReady = 0;
            }

            // break trees
            if (tool is Axe && player.currentLocation.terrainFeatures.ContainsKey(tile))
            {
                TerrainFeature obj = player.currentLocation.terrainFeatures[tile];
                if (obj is Tree tree && tree.health.Value > 1)
                    tree.health.Value = 1;
                else if (obj is FruitTree fruitTree && fruitTree.health.Value > 1)
                    fruitTree.health.Value = 1;
            }

            // break resource clumps
            foreach (ResourceClump r in this.GetResourceClumps(player.currentLocation))
            {
                if (r != null && r.getBoundingBox(r.tile.Value).Contains((int)player.GetToolLocation().X, (int)player.GetToolLocation().Y) && r.health.Value > 0)
                    r.health.Value = 0;
            }
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Get the resource clumps in a location.</summary>
        /// <param name="location">The location to check.</param>
        private IEnumerable<ResourceClump> GetResourceClumps(GameLocation location)
        {
            return location switch
            {
                MineShaft mineShaft => mineShaft.resourceClumps,
                Farm farm => farm.resourceClumps,
                Forest forest => new[] { forest.log },
                Woods woods => woods.stumps,
                _ => Enumerable.Empty<ResourceClump>()
            };
        }
    }
}
