using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;

namespace CJBTweaks
{
    internal class ModEntry : Mod
    {
        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            MenuEvents.MenuChanged += MenuEvents_MenuChanged;
        }


        /*********
        ** Private methods
        *********/
        private void MenuEvents_MenuChanged(object sender, EventArgsClickableMenuChanged e)
        {
            if (e.NewMenu is ShopMenu menu && !(e.PriorMenu is ShopMenu))
            {
                if (menu.portraitPerson?.name == "Clint")
                {
                    Dictionary<Item, int[]> saleList = Utility.getBlacksmithStock();
                    saleList.Add(new Object(Vector2.Zero, 386, int.MaxValue), new[] { 1000, int.MaxValue });
                    Game1.activeClickableMenu = new ShopMenu(saleList, 0, "Clint");
                }
            }
        }
    }
}
