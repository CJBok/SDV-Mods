using System.Collections.Generic;
using CJBCheatsMenu.Framework.Components;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Constants;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.Cheats.Skills;

/// <summary>A cheat which increases mastery level.</summary>
internal class MasteryLevelCheat : BaseCheat
{
    /*********
    ** Fields
    *********/
    /// <summary>The maximum possible mastery level to support.</summary>
    /// <remarks>This sets an upper limit if a mod patches <see cref="MasteryTrackerMenu.getMasteryExpNeededForLevel"/> incorrectly. Most code should use <see cref="MaxMasteryLevel"/> instead.</remarks>
    private const int MaxPermissibleMasteryLevel = 200;

    /// <summary>The XP needed for each mastery level.</summary>
    private IReadOnlyList<int> MasteryLevelXp = [];

    /// <summary>The maximum reachable mastery level.</summary>
    private int MaxMasteryLevel;

    /// <summary>The player's current mastery level.</summary>
    private int MasteryLevel;


    /*********
    ** Public methods
    *********/
    /// <inheritdoc />
    public override IEnumerable<CheatElement> GetFields(CheatContext context)
    {
        return [this.GetMasteryButton(context)];
    }

    /// <inheritdoc />
    public override void OnConfig(CheatContext context, out bool needsInput, out bool needsUpdate, out bool needsRendering)
    {
        base.OnConfig(context, out needsInput, out needsUpdate, out needsRendering);

        needsUpdate = true;
    }

    /// <inheritdoc />
    public override void OnCheatsMenuOpening(CheatContext context)
    {
        this.MasteryLevelXp = GetMasteryLevels();
        this.MaxMasteryLevel = this.MasteryLevelXp.Count;
    }

    /// <inheritdoc />
    public override void OnUpdated(CheatContext context, UpdateTickedEventArgs e)
    {
        this.MasteryLevel = MasteryTrackerMenu.getCurrentMasteryLevel();
    }


    /*********
    ** Private methods
    *********/
    /// <summary>Get the option field to increase the mastery level.</summary>
    /// <param name="context">The cheat context.</param>
    private CheatButton GetMasteryButton(CheatContext context)
    {
        return new CheatButton(
            getLabel: GetLabel,
            slotWidth: context.SlotWidth,
            toggle: this.IncreaseMastery,
            disabled: () => this.MasteryLevel >= this.MaxMasteryLevel
        );

        string GetLabel()
        {
            return I18n.Skills_IncreaseMastery(currentLevel: this.MasteryLevel);
        }
    }

    /// <summary>Increase the mastery level.</summary>
    private void IncreaseMastery()
    {
        int curXp = (int)Game1.stats.Get(StatKeys.MasteryExp);

        foreach (int xpForLevel in this.MasteryLevelXp)
        {
            if (xpForLevel > curXp)
            {
                Game1.stats.Increment(StatKeys.MasteryExp, xpForLevel - curXp);
                break;
            }
        }
    }

    /// <summary>Get the mastery levels and the XP needed to reach them.</summary>
    /// <remarks>Most code should use <see cref="MasteryLevelXp"/> instead.</remarks>
    private static IReadOnlyList<int> GetMasteryLevels()
    {
        List<int> masteryLevels = [];

        for (int level = 1; level < MaxPermissibleMasteryLevel; level++)
        {
            int xp = MasteryTrackerMenu.getMasteryExpNeededForLevel(level);
            if (xp == int.MaxValue)
                break;

            masteryLevels.Add(xp);
        }

        return masteryLevels;
    }
}
