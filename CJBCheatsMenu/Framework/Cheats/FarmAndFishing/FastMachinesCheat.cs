using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CJBCheatsMenu.Framework.Components;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Extensions;
using StardewValley.GameData.Buildings;
using StardewValley.GameData.Machines;
using StardewValley.ItemTypeDefinitions;
using StardewValley.Menus;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using StardewValley.TokenizableStrings;
using SObject = StardewValley.Object;

namespace CJBCheatsMenu.Framework.Cheats.FarmAndFishing;

/// <summary>A cheat which makes machines complete their output instantly.</summary>
internal class FastMachinesCheat : BaseCheat
{
    /*********
    ** Public methods
    *********/
    /// <inheritdoc />
    public override IEnumerable<OptionsElement> GetFields(CheatContext context)
    {
        // special cases
        List<OptionsElement> fields = new()
        {
            new CheatsOptionsCheckbox(
                label: ItemRegistry.GetDataOrErrorItem("(O)710").DisplayName,
                value: context.Config.FastMachines.Contains("(O)710"),
                setValue: value => context.Config.FastMachines.Toggle("(O)710", value)
            ),
            new CheatsOptionsCheckbox(
                label: I18n.FastMachines_FruitTrees(),
                value: context.Config.FastFruitTree,
                setValue: value => context.Config.FastFruitTree = value
            )
        };

        // from Data/Buildings
        foreach ((string buildingId, BuildingData data) in Game1.buildingData)
        {
            var rules = data?.ItemConversions;
            if (rules?.Count is not > 0)
                continue;

            fields.Add(
                new CheatsOptionsCheckbox(
                    label: TokenParser.ParseText(data!.Name) ?? buildingId,
                    value: context.Config.FastBuildings.Contains(buildingId),
                    setValue: value => context.Config.FastBuildings.Toggle(buildingId, value)
                )
            );
        }

        // from Data/Machines
        foreach (string rawId in DataLoader.Machines(Game1.content).Keys)
        {
            ParsedItemData data = ItemRegistry.GetData(rawId);
            if (data is null)
                continue;

            string label = data.DisplayName;
            if (data.QualifiedItemId == "(BC)101") // incubator
                label = I18n.FastMachines_ReadyInTheMorning(label);

            fields.Add(
                new CheatsOptionsCheckbox(
                    label: label,
                    value: context.Config.FastMachines.Contains(data.QualifiedItemId),
                    setValue: value => context.Config.FastMachines.Toggle(data.QualifiedItemId, value)
                )
            );
        }

        return this.SortFields(fields.ToArray());
    }

    /// <inheritdoc />
    public override void OnConfig(CheatContext context, out bool needsInput, out bool needsUpdate, out bool needsRendering)
    {
        needsInput = false;
        needsUpdate = this
            .GetFields(context)
            .Cast<CheatsOptionsCheckbox>()
            .Any(p => p.IsChecked);
        needsRendering = false;
    }

    /// <inheritdoc />
    public override void OnUpdated(CheatContext context, UpdateTickedEventArgs e)
    {
        if (!e.IsOneSecond || !Context.IsWorldReady)
            return;

        foreach (GameLocation location in context.GetAllLocations())
        {
            if (context.Config.FastBuildings.Count > 0)
            {
                foreach (Building building in location.buildings)
                {
                    if (this.IsFastMachine(context, building))
                        this.CompleteMachine(building);
                }
            }

            if (context.Config.FastMachines.Count > 0)
            {
                foreach (SObject obj in location.objects.Values)
                {
                    if (this.IsFastMachine(context, obj))
                        this.CompleteMachine(obj);
                }
            }

            if (context.Config.FastFruitTree)
            {
                foreach (TerrainFeature terrainFeature in location.terrainFeatures.Values)
                {
                    if (terrainFeature is FruitTree tree && tree.growthStage.Value >= FruitTree.treeStage)
                    {
                        int addFruit = FruitTree.maxFruitsOnTrees - tree.fruit.Count;
                        for (int i = 0; i < addFruit; i++)
                            tree.TryAddFruit();
                    }
                }
            }
        }
    }


    /*********
    ** Private methods
    *********/
    /// <summary>Get whether a building is a machine with 'fast processing' enabled.</summary>
    /// <param name="context">The cheat context.</param>
    /// <param name="building">The machine to check.</param>
    private bool IsFastMachine(CheatContext context, [NotNullWhen(true)] Building? building)
    {
        string? buildingType = building?.buildingType.Value;

        return
            !string.IsNullOrWhiteSpace(buildingType)
            && context.Config.FastBuildings.Contains(buildingType);
    }

    /// <summary>Get whether an object is a machine with 'fast processing' enabled.</summary>
    /// <param name="context">The cheat context.</param>
    /// <param name="obj">The machine to check.</param>
    private bool IsFastMachine(CheatContext context, [NotNullWhen(true)] SObject? obj)
    {
        string? qualifiedItemId = obj?.QualifiedItemId;
        return
            !string.IsNullOrWhiteSpace(qualifiedItemId)
            && context.Config.FastMachines.Contains(qualifiedItemId);
    }

    /// <summary>Finish a machine's processing.</summary>
    /// <param name="machine">The machine to complete.</param>
    private void CompleteMachine(Building machine)
    {
        if (machine.isUnderConstruction())
            return;

        machine.dayUpdate(Game1.dayOfMonth);
    }

    /// <summary>Finish a machine's processing.</summary>
    /// <param name="machine">The machine to complete.</param>
    private void CompleteMachine(SObject machine)
    {
        bool hasItem = machine.heldObject.Value != null;
        bool processing = machine.MinutesUntilReady > 0;

        // mark complete
        switch (machine)
        {
            case Cask cask:
                if (hasItem && processing)
                {
                    cask.heldObject.Value.Quality = SObject.bestQuality;
                    cask.daysToMature.Value = 0;
                    cask.MinutesUntilReady = 1;
                    machine.minutesElapsed(machine.MinutesUntilReady);
                }
                break;

            case CrabPot pot:
                if (!hasItem)
                    pot.DayUpdate();
                break;

            default:
                if (hasItem && processing)
                    machine.minutesElapsed(machine.MinutesUntilReady);
                break;
        }

        // restart machine if needed
        if (machine.heldObject.Value is null && machine.MinutesUntilReady <= 0 && this.HasTrigger(machine, MachineOutputTrigger.DayUpdate))
            machine.DayUpdate();
    }

    private bool HasTrigger(SObject machine, MachineOutputTrigger trigger)
    {
        MachineData? data = machine.GetMachineData();
        if (data?.OutputRules?.Count is not > 0)
            return false;

        foreach (MachineOutputRule outputRule in data.OutputRules)
        {
            if (outputRule?.Triggers?.Count is not > 0)
                continue;

            foreach (MachineOutputTriggerRule triggerRule in outputRule.Triggers)
            {
                if (triggerRule?.Trigger == trigger)
                    return true;
            }
        }

        return false;
    }
}
