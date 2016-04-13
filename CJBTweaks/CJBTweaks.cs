using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xTile.Tiles;

namespace CJBTweaks
{
    public class CJBTweaks : Mod
    {

        public override void Entry(params object[] objects) {
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
