using System.Collections.Generic;
using CJBCheatsMenu.Framework.Components;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
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
        /// <inheritdoc />
        public override IEnumerable<OptionsElement> GetFields(CheatContext context)
        {
            yield return new CheatsOptionsCheckbox(
                label: I18n.Tools_OneHitBreak(),
                value: context.Config.OneHitBreak,
                setValue: value => context.Config.OneHitBreak = value
            );
        }

        /// <inheritdoc />
        public override void OnConfig(CheatContext context, out bool needsInput, out bool needsUpdate, out bool needsRendering)
        {
            needsInput = false;
            needsUpdate = context.Config.OneHitBreak;
            needsRendering = false;
        }

        /// <inheritdoc />
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
            Vector2 tile = new((int)(player.GetToolLocation().X / Game1.tileSize), (int)(player.GetToolLocation().Y / Game1.tileSize));

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
            foreach (ResourceClump? clump in location.resourceClumps)
            {
                if (clump != null && clump.getBoundingBox().Contains((int)player.GetToolLocation().X, (int)player.GetToolLocation().Y) && clump.health.Value > 0)
                    clump.health.Value = 0;
            }
        }
    }
}
