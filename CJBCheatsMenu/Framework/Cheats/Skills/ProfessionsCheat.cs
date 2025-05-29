using System.Collections.Generic;
using CJBCheatsMenu.Framework.Components;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.Cheats.Skills;

/// <summary>A cheat which toggles player professions.</summary>
internal class ProfessionsCheat : BaseCheat
{
    /*********
    ** Public methods
    *********/
    /// <inheritdoc />
    public override IEnumerable<OptionsElement> GetFields(CheatContext context)
    {
        foreach ((int skillId, Dictionary<int, int[]> professionsByLevel) in this.GetProfessions())
        {
            string skillName = Farmer.getSkillDisplayNameFromIndex(skillId);

            foreach ((int level, int[] professionIds) in professionsByLevel)
            {
                foreach (int professionId in professionIds)
                {
                    string professionName = LevelUpMenu.getProfessionTitleFromNumber(professionId);

                    yield return new CheatsOptionsCheckbox(
                        label: I18n.Professions_Profession(skillName: skillName, level: level, professionName: professionName),
                        value: this.GetProfession(professionId),
                        setValue: value => this.SetProfession(professionId, value)
                    );
                }
            }
        }
    }


    /*********
    ** Private methods
    *********/
    /// <summary>Get the profession IDs by skill ID and level.</summary>
    private Dictionary<int, Dictionary<int, int[]>> GetProfessions()
    {
        return new()
        {
            [Farmer.combatSkill] = new()
            {
                [5] = [Farmer.fighter, Farmer.scout],
                [10] = [Farmer.acrobat, Farmer.brute, Farmer.defender, Farmer.desperado]
            },
            [Farmer.farmingSkill] = new()
            {
                [5] = [Farmer.rancher, Farmer.tiller],
                [10] = [Farmer.agriculturist, Farmer.artisan, Farmer.butcher, Farmer.shepherd] // butcher = coopmaster
            },
            [Farmer.fishingSkill] = new()
            {
                [5] = [Farmer.fisher, Farmer.trapper],
                [10] = [Farmer.angler, Farmer.mariner, Farmer.baitmaster, Farmer.pirate] // mariner = luremaster, and baitmaster = mariner
            },
            [Farmer.foragingSkill] = new()
            {
                [5] = [Farmer.forester, Farmer.gatherer],
                [10] = [Farmer.botanist, Farmer.lumberjack, Farmer.tapper, Farmer.tracker]
            },
            [Farmer.miningSkill] = new()
            {
                [5] = [Farmer.geologist, Farmer.miner],
                [10] = [Farmer.blacksmith, Farmer.excavator, Farmer.gemologist, Farmer.burrower] // burrower = prospecter
            }
        };
    }

    /// <summary>Get whether the player has the given profession.</summary>
    /// <param name="id">The profession ID.</param>
    private bool GetProfession(int id)
    {
        return Game1.player.professions.Contains(id);
    }

    /// <summary>Toggle a player profession.</summary>
    /// <param name="id">The profession ID.</param>
    /// <param name="enable">Whether to enable the profession (else disable).</param>
    /// <remarks>Derived from <see cref="LevelUpMenu.getImmediateProfessionPerk"/>.</remarks>
    private void SetProfession(int id, bool enable)
    {
        // skip if done
        if (enable == this.GetProfession(id))
            return;

        // get health bonus for profession
        int healthBonus = id switch
        {
            Farmer.fighter => 15,
            Farmer.defender => 25,
            _ => 0
        };

        // apply
        Farmer player = Game1.player;
        if (enable)
        {
            player.maxHealth += healthBonus;
            player.health += healthBonus;
            player.professions.Add(id);
        }
        else
        {
            player.health -= healthBonus;
            player.maxHealth -= healthBonus;
            player.professions.Remove(id);
        }
        LevelUpMenu.RevalidateHealth(player);
    }
}
