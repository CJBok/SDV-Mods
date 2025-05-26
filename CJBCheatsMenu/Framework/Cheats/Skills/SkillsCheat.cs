using System.Collections.Generic;
using CJBCheatsMenu.Framework.Components;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.Cheats.Skills;

/// <summary>A cheat which increases or resets skill levels.</summary>
internal class SkillsCheat : BaseCheat
{
    /*********
    ** Public methods
    *********/
    /// <inheritdoc />
    public override IEnumerable<OptionsElement> GetFields(CheatContext context)
    {
        return
        [
            this.GetSkillButton(context, "farming", Farmer.farmingSkill, Game1.player.FarmingLevel),
            this.GetSkillButton(context, "mining", Farmer.miningSkill, Game1.player.MiningLevel),
            this.GetSkillButton(context, "foraging", Farmer.foragingSkill, Game1.player.ForagingLevel),
            this.GetSkillButton(context, "fishing", Farmer.fishingSkill, Game1.player.FishingLevel),
            this.GetSkillButton(context, "combat", Farmer.combatSkill, Game1.player.CombatLevel)
        ];
    }


    /*********
    ** Private methods
    *********/
    /// <summary>Get the option field to increase a skill.</summary>
    /// <param name="context">The cheat context.</param>
    /// <param name="translationKey">The unique portion of its display text translation key.</param>
    /// <param name="id">The game's skill ID.</param>
    /// <param name="currentLevel">The current skill level.</param>
    private CheatsOptionsButton GetSkillButton(CheatContext context, string translationKey, int id, int currentLevel)
    {
        return new CheatsOptionsButton(
            label: I18n.GetByKey($"skills.increase-{translationKey}", new { currentLevel }),
            slotWidth: context.SlotWidth,
            toggle: () => this.IncreaseSkill(id),
            disabled: currentLevel >= 10
        );
    }

    /// <summary>Get the experience points needed to level up once to the given level.</summary>
    /// <param name="level">The next skill level.</param>
    private int GetExperiencePoints(int level)
    {
        if (level is < 0 or > 9)
            return 0;

        int[] exp = [100, 280, 390, 530, 850, 1150, 1500, 2100, 3100, 5000];

        return exp[level];
    }

    /// <summary>Increase a skill level.</summary>
    /// <param name="skillId">The skill ID.</param>
    private void IncreaseSkill(int skillId)
    {
        int expToNext = this.GetExperiencePoints(Game1.player.GetSkillLevel(skillId));
        IList<Point> newLevels = Game1.player.newLevels;

        int wasNewLevels = newLevels.Count;
        Game1.player.gainExperience(skillId, expToNext);
        if (newLevels.Count > wasNewLevels)
            newLevels.RemoveAt(newLevels.Count - 1);

        Game1.exitActiveMenu();
        Game1.activeClickableMenu = new LevelUpMenu(skillId, Game1.player.GetSkillLevel(skillId));
    }
}
