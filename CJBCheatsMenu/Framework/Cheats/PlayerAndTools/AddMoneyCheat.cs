using System.Collections.Generic;
using CJBCheatsMenu.Framework.Components;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.Cheats.PlayerAndTools;

/// <summary>A cheat which adds various amounts of money to the player.</summary>
internal class AddMoneyCheat : BaseCheat
{
    /*********
    ** Public methods
    *********/
    /// <inheritdoc />
    public override IEnumerable<OptionsElement> GetFields(CheatContext context)
    {
        foreach (int amount in new[] { 100, 1000, 10000, 100000 })
        {
            yield return new CheatsOptionsButton(
                label: I18n.Add_AmountGold(amount: amount),
                slotWidth: context.SlotWidth,
                toggle: () => this.AddMoney(amount)
            );
        }
    }


    /*********
    ** Private methods
    *********/
    /// <summary>Add an amount to the player money.</summary>
    /// <param name="amount">The amount to add.</param>
    private void AddMoney(int amount)
    {
        Game1.player.Money += amount;
        Game1.playSound("coin");
    }
}
