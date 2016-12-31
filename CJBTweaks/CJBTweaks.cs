using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using System.Collections.Generic;

namespace CJBTweaks
{
    public class CJBTweaks : Mod
    {
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper) {
            MenuEvents.MenuChanged += MenuEvents_MenuChanged;
        }

        private void MenuEvents_MenuChanged(object sender, EventArgsClickableMenuChanged e) {
            if (e.NewMenu is ShopMenu && !(e.PriorMenu is ShopMenu)) {
                ShopMenu menu = (ShopMenu)e.NewMenu;

                if (menu.portraitPerson?.name == "Clint") {
                    Dictionary<Item, int[]> saleList = Utility.getBlacksmithStock();
                    saleList.Add(new StardewValley.Object(Vector2.Zero, 386, int.MaxValue), new int[] { 1000, int.MaxValue });
                    Game1.activeClickableMenu = new ShopMenu(saleList, 0, "Clint");
                }
            }
        }
    }
}
