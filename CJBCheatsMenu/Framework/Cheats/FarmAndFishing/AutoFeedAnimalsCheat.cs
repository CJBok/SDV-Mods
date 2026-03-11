using System.Collections.Generic;
using CJBCheatsMenu.Framework.Components;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using xTile;
using xTile.Layers;
using xTile.Tiles;
using Object = StardewValley.Object;

namespace CJBCheatsMenu.Framework.Cheats.FarmAndFishing;

/// <summary>A cheat which automatically fills animal feed troughs.</summary>
internal class AutoFeedAnimalsCheat : BaseCheat
{
    /*********
    ** Fields
    *********/
    /// <summary>The number of ticks for which to cache the <see cref="TroughTilesByMapPath"/>.</summary>
    private const int CacheTicks = 60 * 30; // 30 seconds

    /// <summary>The next tick when the <see cref="TroughTilesByMapPath"/> should be reset.</summary>
    private int NextCacheResetTick;

    /// <summary>The tile positions at which hay can be placed, indexed by map path.</summary>
    private readonly Dictionary<string, IReadOnlyList<Vector2>> TroughTilesByMapPath = [];


    /*********
    ** Public methods
    *********/
    /// <inheritdoc />
    public override IEnumerable<OptionsElement> GetFields(CheatContext context)
    {
        yield return new CheatsOptionsCheckbox(
            label: I18n.Farm_AutoFeedAnimals(),
            value: context.Config.AutoFeed,
            setValue: value => context.Config.AutoFeed = value
        );
    }

    /// <inheritdoc />
    public override void OnConfig(CheatContext context, out bool needsInput, out bool needsUpdate, out bool needsRendering)
    {
        needsInput = false;
        needsUpdate = context.Config.AutoFeed;
        needsRendering = false;
    }

    /// <inheritdoc />
    public override void OnSaveLoaded(CheatContext context)
    {
        this.NextCacheResetTick = Game1.ticks;
    }

    /// <inheritdoc />
    public override void OnUpdated(CheatContext context, UpdateTickedEventArgs e)
    {
        if (!e.IsOneSecond || !Context.IsWorldReady)
            return;

        if (Game1.ticks >= this.NextCacheResetTick)
        {
            this.TroughTilesByMapPath.Clear();
            this.NextCacheResetTick = Game1.ticks + AutoFeedAnimalsCheat.CacheTicks;
        }

        Utility.ForEachLocation(
            location =>
            {
                switch (location)
                {
                    case AnimalHouse animalHouse:
                        {
                            int animalCount = animalHouse.animalsThatLiveHere.Count;
                            if (animalCount < 1)
                                break;

                            int missingHay = animalCount - animalHouse.numberOfObjectsWithName("Hay");
                            if (missingHay < 1)
                                break;

                            foreach (Vector2 tile in this.GetTroughTiles(location.map, location.mapPath.Value))
                            {
                                if (!animalHouse.objects.ContainsKey(tile))
                                {
                                    Object hay = GameLocation.GetHayFromAnySilo(animalHouse) ?? ItemRegistry.Create<Object>("(O)178"); // if no hay is available, this is a cheat mod so spawn some anyway
                                    animalHouse.objects.Add(tile, hay);

                                    missingHay--;
                                    if (missingHay < 1)
                                        break;
                                }
                            }
                        }
                        break;

                    case SlimeHutch slimeHutch:
                        for (int i = 0; i < slimeHutch.waterSpots.Length; i++)
                            slimeHutch.waterSpots[i] = true;
                        break;
                }

                return true;
            }
        );
    }


    /*********
    ** Private methods
    *********/
    /// <summary>Get all trough tiles in a map.</summary>
    /// <param name="map">The map whose tiles to check.</param>
    /// <param name="mapPath">The map path for which to cache the results.</param>
    /// <remarks>Derived from <see cref="AnimalHouse.feedAllAnimals"/>.</remarks>
    private IReadOnlyList<Vector2> GetTroughTiles(Map? map, string mapPath)
    {
        if (map is null)
            return [];

        if (!this.TroughTilesByMapPath.TryGetValue(mapPath, out IReadOnlyList<Vector2>? tiles))
        {
            List<Vector2> tileList = [];

            Layer layer = map.GetLayer("Back");
            if (layer != null)
            {
                for (int y = 0; y < layer.LayerHeight; y++)
                {
                    for (int x = 0; x < layer.LayerWidth; x++)
                    {
                        Tile? tile = layer.Tiles[x, y];
                        if (tile is not null && (tile.Properties.ContainsKey("Trough") || tile.TileIndexProperties.ContainsKey("Trough")))
                            tileList.Add(new Vector2(x, y));
                    }
                }
            }

            this.TroughTilesByMapPath[mapPath] = tiles = tileList;
        }

        return tiles;
    }
}
