using System.Collections.Generic;
using CJBCheatsMenu.Framework.Components;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.Cheats.PlayerAndTools
{
    /// <summary>A cheat which adds various numbers of Qi gems to the player.</summary>
    internal class AddQiGemsCheat : BaseCheat
    {
        /*********
        ** Public methods
        *********/
        /// <summary>Get the config UI fields to show in the cheats menu.</summary>
        /// <param name="context">The cheat context.</param>
        public override IEnumerable<OptionsElement> GetFields(CheatContext context)
        {
            foreach (int amount in new[] { 1, 10, 100 })
            {
                yield return new CheatsOptionsButton(
                    label: I18n.Add_AmountOther(amount: amount),
                    slotWidth: context.SlotWidth,
                    toggle: () => this.AddGems(amount)
                );
            }
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Add an amount to the player's Qi gems balance.</summary>
        /// <param name="amount">The amount to add.</param>
        private void AddGems(int amount)
        {
            Game1.player.addItemToInventoryBool(new Object("858", amount));
        }
    }
}
