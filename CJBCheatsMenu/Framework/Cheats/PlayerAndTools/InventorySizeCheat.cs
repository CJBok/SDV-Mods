using System.Collections.Generic;
using CJBCheatsMenu.Framework.Components;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.Cheats.PlayerAndTools
{
    /// <summary>A cheat which sets the inventory size.</summary>
    internal class InventorySizeCheat : BaseCheat
    {
        /*********
        ** Fields
        *********/
        /// <summary>The number of items unlocked by each backpack upgrade.</summary>
        private readonly int ItemsPerBackpackUpgrade = 12;


        /*********
        ** Public methods
        *********/
        /// <summary>Get the config UI fields to show in the cheats menu.</summary>
        /// <param name="context">The cheat context.</param>
        public override IEnumerable<OptionsElement> GetFields(CheatContext context)
        {
            yield return new CheatsOptionsSlider(
                label: context.Text.Get("player.inventory-size"),
                value: Game1.player.MaxItems / this.ItemsPerBackpackUpgrade,
                minValue: 1,
                maxValue: Farmer.maxInventorySpace / this.ItemsPerBackpackUpgrade,
                setValue: this.SetBackpackSize,
                format: value => (value * this.ItemsPerBackpackUpgrade).ToString()
            );
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Set the player's backpack upgrade level.</summary>
        /// <param name="level">The backpack upgrade level, starting at 1 for the starter inventory size.</param>
        /// <remarks>Derived from <see cref="Farmer.increaseBackpackSize"/>.</remarks>
        private void SetBackpackSize(int level)
        {
            Farmer player = Game1.player;
            int size = level * this.ItemsPerBackpackUpgrade;
            int difference = size - player.Items.Count;

            // set max size
            player.MaxItems = size;

            // remove extra slots
            for (int slot = player.Items.Count - 1; slot >= 0 && difference < 0; slot--)
            {
                if (player.Items[slot] == null)
                {
                    player.Items.RemoveAt(slot);
                    difference++;
                }
            }

            // add missing slots
            while (difference > 0)
            {
                player.Items.Add(null);
                difference--;
            }
        }
    }
}
