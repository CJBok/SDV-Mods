using System.Collections.Generic;
using CJBCheatsMenu.Framework.Components;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Quests;

namespace CJBCheatsMenu.Framework.Cheats.Advanced
{
    /// <summary>A cheat which completes open quests.</summary>
    internal class QuestsCheat : BaseCheat
    {
        /*********
        ** Public methods
        *********/
        /// <summary>Get the config UI fields to show in the cheats menu.</summary>
        /// <param name="context">The cheat context.</param>
        public override IEnumerable<OptionsElement> GetFields(CheatContext context)
        {
            foreach (Quest quest in Game1.player.questLog)
            {
                if (quest.completed.Value)
                    continue;

                yield return new CheatsOptionsButton(
                    label: quest.questTitle,
                    slotWidth: context.SlotWidth,
                    toggle: () => this.CompleteQuest(quest)
                );
            }
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Complete a player quest.</summary>
        /// <param name="quest">The quest to complete.</param>
        private void CompleteQuest(Quest quest)
        {
            quest.questComplete();
            Game1.exitActiveMenu();
        }
    }
}
