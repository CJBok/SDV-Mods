using System;
using System.Collections.Generic;

namespace CJBCheatsMenu.Framework.CheatMenus
{
    internal class SkillsCheatMenu : CheatMenu
    {
        public enum Skill { FARMING = 0, FORAGING = 2, MINING = 3, FISHING = 1, COMBAT = 4 }
        public static int GetCurrentLevel(Skill skill)
        {
            switch (skill)
            {
                case Skill.FARMING:
                    return StardewValley.Game1.player.FarmingLevel;
                case Skill.FORAGING:
                    return StardewValley.Game1.player.ForagingLevel;
                case Skill.MINING:
                    return StardewValley.Game1.player.MiningLevel;
                case Skill.FISHING:
                    return StardewValley.Game1.player.FishingLevel;
                case Skill.COMBAT:
                    return StardewValley.Game1.player.CombatLevel;
                default:
                    return -1;
            }
        }

        /// <summary>
        /// Constructs a cheat menu for setting skills and professions.
        /// </summary>
        /// <param name="config">The user defined preferences.</param>
        /// <param name="cheats">Helper module that has various cheat utilities.</param>
        /// <param name="i18n">Helper module for internationalization.</param>
        public SkillsCheatMenu(ModConfig config, Cheats cheats, StardewModdingAPI.ITranslationHelper I18n)
            : base(config, cheats, I18n)
        {
        }

        /// <summary>
        /// Unique id for the cheat menu.
        /// </summary>
        public override string Id => "CBJCheatsMenu_SkillsCheatMenu";

        /// <summary>
        /// The title of the cheat menu (used for tab name).
        /// </summary>
        public override string Title => I18n.Get("tabs.skills");

        /// <summary>
        /// The options rendered within this cheat menu.
        /// </summary>
        public override List<Menu.IOptionGroup> OptionGroups
        {
            get
            {
                List<Menu.IOptionGroup> optionGroups = new List<Menu.IOptionGroup>();

                Action<bool> resetAllSkillLevelsAction = new Action<bool>(unused =>
                {
                    StardewValley.Game1.player.maxHealth -= 5 * StardewValley.Game1.player.CombatLevel;
                    StardewValley.Game1.player.experiencePoints[0] = 0;
                    StardewValley.Game1.player.experiencePoints[1] = 0;
                    StardewValley.Game1.player.experiencePoints[2] = 0;
                    StardewValley.Game1.player.experiencePoints[3] = 0;
                    StardewValley.Game1.player.experiencePoints[4] = 0;
                    StardewValley.Game1.player.FarmingLevel = 0;
                    StardewValley.Game1.player.MiningLevel = 0;
                    StardewValley.Game1.player.ForagingLevel = 0;
                    StardewValley.Game1.player.FishingLevel = 0;
                    StardewValley.Game1.player.CombatLevel = 0;
                    if (StardewValley.Game1.player.professions.Contains(24))
                        StardewValley.Game1.player.maxHealth -= 15;
                    if (StardewValley.Game1.player.professions.Contains(27))
                        StardewValley.Game1.player.maxHealth -= 25;
                    StardewValley.Game1.player.health = StardewValley.Game1.player.maxHealth;
                    StardewValley.Game1.player.professions.Clear();
                });

                Menu.OptionGroup skillsOptionGroup = new Menu.OptionGroup($"{I18n.Get("skills.title")}:");
                skillsOptionGroup.Options.Add(new IncreaseLevelSetButton(I18n.Get("skills.increase-farming"), Skill.FARMING));
                skillsOptionGroup.Options.Add(new IncreaseLevelSetButton(I18n.Get("skills.increase-mining"), Skill.MINING));
                skillsOptionGroup.Options.Add(new IncreaseLevelSetButton(I18n.Get("skills.increase-foraging"), Skill.FORAGING));
                skillsOptionGroup.Options.Add(new IncreaseLevelSetButton(I18n.Get("skills.increase-fishing"), Skill.FISHING));
                skillsOptionGroup.Options.Add(new IncreaseLevelSetButton(I18n.Get("skills.increase-combat"), Skill.COMBAT));
                skillsOptionGroup.Options.Add(new Menu.OptionSetButton<bool>(I18n.Get("skills.reset"), true, resetAllSkillLevelsAction));
                optionGroups.Add(skillsOptionGroup);

                Menu.OptionGroup professionsOptionGroup = new Menu.OptionGroup($"{I18n.Get("professions.title")}:");
                professionsOptionGroup.Options.Add(new AddProfessionCheckbox(I18n.Get("professions.combat.fighter"), StardewValley.Farmer.fighter));
                professionsOptionGroup.Options.Add(new AddProfessionCheckbox(I18n.Get("professions.combat.scout"), StardewValley.Farmer.scout));
                professionsOptionGroup.Options.Add(new AddProfessionCheckbox(I18n.Get("professions.combat.acrobat"), StardewValley.Farmer.acrobat));
                professionsOptionGroup.Options.Add(new AddProfessionCheckbox(I18n.Get("professions.combat.brute"), StardewValley.Farmer.brute));
                professionsOptionGroup.Options.Add(new AddProfessionCheckbox(I18n.Get("professions.combat.defender"), StardewValley.Farmer.defender));
                professionsOptionGroup.Options.Add(new AddProfessionCheckbox(I18n.Get("professions.combat.desperado"), StardewValley.Farmer.desperado));
                professionsOptionGroup.Options.Add(new AddProfessionCheckbox(I18n.Get("professions.farming.rancher"), StardewValley.Farmer.rancher));
                professionsOptionGroup.Options.Add(new AddProfessionCheckbox(I18n.Get("professions.farming.tiller"), StardewValley.Farmer.tiller));
                professionsOptionGroup.Options.Add(new AddProfessionCheckbox(I18n.Get("professions.farming.agriculturist"), StardewValley.Farmer.agriculturist));
                professionsOptionGroup.Options.Add(new AddProfessionCheckbox(I18n.Get("professions.farming.artisan"), StardewValley.Farmer.artisan));
                professionsOptionGroup.Options.Add(new AddProfessionCheckbox(I18n.Get("professions.farming.coopmaster"), StardewValley.Farmer.butcher)); // butcher = coopmaster
                professionsOptionGroup.Options.Add(new AddProfessionCheckbox(I18n.Get("professions.farming.shepherd"), StardewValley.Farmer.shepherd));
                professionsOptionGroup.Options.Add(new AddProfessionCheckbox(I18n.Get("professions.fishing.fisher"), StardewValley.Farmer.fisher));
                professionsOptionGroup.Options.Add(new AddProfessionCheckbox(I18n.Get("professions.fishing.trapper"), StardewValley.Farmer.trapper));
                professionsOptionGroup.Options.Add(new AddProfessionCheckbox(I18n.Get("professions.fishing.angler"), StardewValley.Farmer.angler));
                professionsOptionGroup.Options.Add(new AddProfessionCheckbox(I18n.Get("professions.fishing.luremaster"), StardewValley.Farmer.mariner)); // mariner = luremaster (???)
                professionsOptionGroup.Options.Add(new AddProfessionCheckbox(I18n.Get("professions.fishing.mariner"), StardewValley.Farmer.baitmaster)); // baitmaster = mariner (???)
                professionsOptionGroup.Options.Add(new AddProfessionCheckbox(I18n.Get("professions.fishing.pirate"), StardewValley.Farmer.pirate));
                professionsOptionGroup.Options.Add(new AddProfessionCheckbox(I18n.Get("professions.foraging.forester"), StardewValley.Farmer.forester));
                professionsOptionGroup.Options.Add(new AddProfessionCheckbox(I18n.Get("professions.foraging.gatherer"), StardewValley.Farmer.gatherer));
                professionsOptionGroup.Options.Add(new AddProfessionCheckbox(I18n.Get("professions.foraging.botanist"), StardewValley.Farmer.botanist));
                professionsOptionGroup.Options.Add(new AddProfessionCheckbox(I18n.Get("professions.foraging.lumberjack"), StardewValley.Farmer.lumberjack));
                professionsOptionGroup.Options.Add(new AddProfessionCheckbox(I18n.Get("professions.foraging.tapper"), StardewValley.Farmer.tapper));
                professionsOptionGroup.Options.Add(new AddProfessionCheckbox(I18n.Get("professions.foraging.tracker"), StardewValley.Farmer.tracker));
                professionsOptionGroup.Options.Add(new AddProfessionCheckbox(I18n.Get("professions.mining.geologist"), StardewValley.Farmer.geologist));
                professionsOptionGroup.Options.Add(new AddProfessionCheckbox(I18n.Get("professions.mining.miner"), StardewValley.Farmer.miner));
                professionsOptionGroup.Options.Add(new AddProfessionCheckbox(I18n.Get("professions.mining.blacksmith"), StardewValley.Farmer.blacksmith));
                professionsOptionGroup.Options.Add(new AddProfessionCheckbox(I18n.Get("professions.mining.excavator"), StardewValley.Farmer.excavator));
                professionsOptionGroup.Options.Add(new AddProfessionCheckbox(I18n.Get("professions.mining.gemologist"), StardewValley.Farmer.gemologist));
                professionsOptionGroup.Options.Add(new AddProfessionCheckbox(I18n.Get("professions.mining.prospector"), StardewValley.Farmer.burrower)); // burrower = prospector
                optionGroups.Add(professionsOptionGroup);

                return optionGroups;
            }
        }

        /// <summary>
        /// A set button that increases the characters level in the specified skill.
        /// </summary>
        private class IncreaseLevelSetButton : Menu.IOptionSetButton
        {
            public IncreaseLevelSetButton(string label, Skill skill)
            {
                this.label = label;
                this.Skill = skill;
            }

            private Skill Skill { get; set; }

            private string label;
            public string Label => $"{label}: {SkillsCheatMenu.GetCurrentLevel(this.Skill)}";

            public bool Disabled => SkillsCheatMenu.GetCurrentLevel(this.Skill) >= 10;

            public void OnPressed()
            {
                int lvl = StardewValley.Game1.player.newLevels.Count;
                StardewValley.Game1.player.gainExperience((int)this.Skill, CJB.GetExperiencePoints(SkillsCheatMenu.GetCurrentLevel(this.Skill)));
                if (lvl < StardewValley.Game1.player.newLevels.Count)
                    StardewValley.Game1.player.newLevels.RemoveAt(StardewValley.Game1.player.newLevels.Count - 1);
                StardewValley.Game1.exitActiveMenu();
                StardewValley.Game1.activeClickableMenu = new StardewValley.Menus.LevelUpMenu((int)this.Skill, SkillsCheatMenu.GetCurrentLevel(this.Skill));
            }
        }

        /// <summary>
        /// A checkbox that adds or removed the specified profession from the character.
        /// </summary>
        private class AddProfessionCheckbox : Menu.IOptionCheckbox
        {
            public AddProfessionCheckbox(string label, int professionId)
            {
                this.Label = label;
                this.ProfessionId = professionId;
            }

            private int ProfessionId { get; set; }

            public string Label { get; private set; }

            public bool Disabled => false;

            public bool Value
            {
                get
                {
                    return StardewValley.Game1.player.professions.Contains(this.ProfessionId);
                }
                set
                {
                    if (value)
                    {
                        StardewValley.Game1.player.professions.Add(this.ProfessionId);
                    }
                    else
                    {
                        StardewValley.Game1.player.professions.Remove(this.ProfessionId);
                    }
                }
            }
        }
    }
}
