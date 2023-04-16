using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using CJBCheatsMenu.Framework.Components;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.Cheats.PlayerAndTools
{
    internal class SplitQualityItems : BaseCheat
    {
        private IMonitor? _monitor;

        enum Quality: int
        {
            Normal = 0,
            Silver = 1,
            Gold = 2,
            Iridium = 4
        }

        static double GetQualityFactor(int quality)
        {
            return Math.Max(1d, 1d + quality * .25);
        }

        static void ConvertibleQuantity(int srcCount, int srcQuality, out int toConsume, out int toProduce)
        {
            double factor = GetQualityFactor(srcQuality);
            double _toProduce = 0d;
            toConsume = 0;
            do
            {
                _toProduce += factor;
                ++toConsume;
            } while (Math.Floor(_toProduce) != _toProduce);
            int iterations = srcCount / toConsume;
            toConsume *= iterations;
            toProduce = iterations * (int)_toProduce;
        }

        public override IEnumerable<OptionsElement> GetFields(CheatContext context)
        {
            yield return new CheatsOptionsButton(
                label: I18n.SplitQualityItems_HeldItems(),
                slotWidth: context.SlotWidth,
                toggle: () => this.SplitInventoryStack(Game1.player.CurrentToolIndex)
            );
            yield return new CheatsOptionsButton(
                label: I18n.SplitQualityItems_WholeInventory(),
                slotWidth: context.SlotWidth,
                toggle: () => this.SplitWholeInventory()
            );
        }

        public override void OnConfig(CheatContext context, out bool needsInput, out bool needsUpdate, out bool needsRendering)
        {
            this._monitor = context.Monitor;
            base.OnConfig(context, out needsInput, out needsUpdate, out needsRendering);
        }

        private void Log(string message, LogLevel level)
        {
            if (this._monitor != null)
            {
                this._monitor.Log(message, level);
            }
        }

        private void SplitInventoryStack(int inventoryIndex)
        {
            Farmer player = Game1.player;
            IList<Item> inventory = player.Items;

            StardewValley.Object? stack = inventory[inventoryIndex] as StardewValley.Object;

            if (stack == null || stack.Quality == 0) return;

            this.Log($"Processing {stack.Stack} {stack.DisplayName} @{inventoryIndex} #{stack.ParentSheetIndex}/{stack.preservedParentSheetIndex} of quality {stack.Quality}", LogLevel.Trace);
            ConvertibleQuantity(stack.Stack, stack.Quality, out int toConsume, out int toProduce);
            this.Log($"Will consume {toConsume} to produce {toProduce}", LogLevel.Trace);
            StardewValley.Object result = (StardewValley.Object)stack.getOne();
            stack.Stack -= toConsume;
            if (stack.Stack <= 0)
            {
                player.items[inventoryIndex] = null;
            }
            result.Quality = 0;
            result.Stack = toProduce;
            Item? remainder = player.addItemToInventory(result);
            if (remainder != null)
            {
                this.Log($"Dropping excess {remainder.Stack} {remainder.DisplayName}", LogLevel.Trace);
                Game1.createItemDebris(remainder, new Vector2(Game1.player.position.X, Game1.player.position.Y), -1);
            }
        }

        private void SplitWholeInventory()
        {
            this.Log("=== Split whole inventory quality items", LogLevel.Debug);

            // index what can be split in inventory first, as we may use several quality levels for a single output
            Farmer farmer = Game1.player;

            for (int i = 0; i < farmer.Items.Count; ++i)
            {
                StardewValley.Object? obj = farmer.Items[i] as StardewValley.Object;
                if (obj != null)
                {
                    this.SplitInventoryStack(i);
                }
            }
        }
    }
}
