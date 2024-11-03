using System.Collections.Generic;
using CJBCheatsMenu.Framework.Components;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley.GameData.Crops;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.Cheats.PlayerAndTools;

/// <summary>A cheat which enables harvesting all crops with the scythe.</summary>
internal class HarvestWithScytheCheat : BaseCheat
{
    /*********
    ** Fields
    *********/
    /// <summary>An API for managing the game's content assets.</summary>
    private readonly IGameContentHelper GameContent;

    /// <summary>The last value that was applied to <c>Data/Crops</c>.</summary>
    private bool? LastValue;

    /// <summary>The asset name for the crop data.</summary>
    private const string CropAssetName = "Data\\Crops";


    /*********
    ** Public methods
    *********/
    /// <summary>Construct an instance.</summary>
    /// <param name="gameContent">An API for managing the game's content assets.</param>
    public HarvestWithScytheCheat(IGameContentHelper gameContent)
    {
        this.GameContent = gameContent;
    }

    /// <inheritdoc />
    public override IEnumerable<OptionsElement> GetFields(CheatContext context)
    {
        yield return new CheatsOptionsCheckbox(
            label: I18n.Tools_HarvestWithScythe(),
            value: context.Config.HarvestScythe,
            setValue: value => context.Config.HarvestScythe = value
        );
    }

    /// <inheritdoc />
    public override void OnConfig(CheatContext context, out bool needsInput, out bool needsUpdate, out bool needsRendering)
    {
        needsInput = false;
        needsUpdate = false;
        needsRendering = false;

        // if we've already applied changes, update the asset if needed
        if (this.LastValue.HasValue && this.LastValue != context.Config.HarvestScythe)
            this.InvalidateCache(context);
    }

    /// <inheritdoc />
    public override void OnSaveLoaded(CheatContext context)
    {
        // update crop harvest methods if needed
        if (context.Config.HarvestScythe)
        {
            if (this.LastValue != true)
                this.InvalidateCache(context);
        }
        else
            this.LastValue = false;
    }

    /// <inheritdoc cref="IContentEvents.AssetRequested"/>
    /// <param name="context">The cheat context.</param>
    /// <param name="e">The event arguments.</param>
    public void OnAssetRequested(CheatContext context, AssetRequestedEventArgs e)
    {
        if (context.Config.HarvestScythe && e.Name.IsEquivalentTo(HarvestWithScytheCheat.CropAssetName))
        {
            e.Edit(
                asset =>
                {
                    foreach (CropData crop in asset.AsDictionary<string, CropData>().Data.Values)
                        crop.HarvestMethod = HarvestMethod.Scythe;
                },
                priority: AssetEditPriority.Late + 1000
            );
        }
    }


    /*********
    ** Private methods
    *********/
    /// <summary>Invalidate the crop data asset so changes are reapplied.</summary>
    /// <param name="context">The cheat context.</param>
    private void InvalidateCache(CheatContext context)
    {
        this.LastValue = context.Config.HarvestScythe;
        this.GameContent.InvalidateCache(HarvestWithScytheCheat.CropAssetName);
    }
}
