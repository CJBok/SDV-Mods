using System;
using System.Collections.Generic;
using System.Reflection;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu
{
    public class CJBCheatsMenu : Mod
    {
        /*********
        ** Properties
        *********/
        private static IModHelper Helper;


        /*********
        ** Accessors
        *********/
        public static Settings Config;


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            CJBCheatsMenu.Helper = helper;
            CJBCheatsMenu.Config = helper.ReadConfig<Settings>();

            GameEvents.UpdateTick += Events_UpdateTick;
            GameEvents.OneSecondTick += GameEvents_OneSecondTick;
            TimeEvents.TimeOfDayChanged += TimeEvents_TimeOfDayChanged;
            ControlEvents.KeyPressed += Events_KeyPressed;
            ControlEvents.ControllerButtonPressed += ControlEvents_ControllerButtonPressed;
            GraphicsEvents.OnPostRenderEvent += GraphicsEvents_DrawTick;
            TimeEvents.DayOfMonthChanged += TimeEvents_DayOfMonthChanged;
        }

        /// <summary>Update the mod's config.json file from the current <see cref="Config"/>.</summary>
        internal static void SaveConfig()
        {
            CJBCheatsMenu.Helper.WriteConfig(CJBCheatsMenu.Config);
        }


        /*********
        ** Private methods
        *********/
        private void GameEvents_OneSecondTick(object sender, EventArgs e)
        {
            if (Game1.hasLoadedGame)
            {
                Cheats.OneSecondUpdate();
            }
        }

        private void ControlEvents_ControllerButtonPressed(object sender, EventArgsControllerButtonPressed e)
        {
            if (e.ButtonPressed.ToString().Equals(CJBCheatsMenu.Config.OpenMenuKey))
            {
                if (Game1.hasLoadedGame && Game1.activeClickableMenu == null && Game1.player.CanMove && !Game1.dialogueUp && !Game1.eventUp)
                {
                    CheatsMenu.Open();
                }
                return;
            }

            if (e.ButtonPressed.ToString().Equals(CJBCheatsMenu.Config.FreezeTimeKey))
            {
                if (Game1.hasLoadedGame && Game1.activeClickableMenu == null)
                {
                    CJBCheatsMenu.Config.FreezeTime = !CJBCheatsMenu.Config.FreezeTime;
                }
                return;
            }

            if (Game1.activeClickableMenu is GameMenu menu)
            {
                List<IClickableMenu> pages = (List<IClickableMenu>)typeof(GameMenu).GetField("pages", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(menu);

                if (pages[menu.currentTab] is CheatsMenu)
                    pages[menu.currentTab].receiveGamePadButton(e.ButtonPressed);
            }
        }

        private void Events_KeyPressed(object sender, EventArgsKeyPressed e)
        {
            Cheats.OnKeyPress(e.KeyPressed);
        }

        private void TimeEvents_DayOfMonthChanged(object sender, EventArgsIntChanged e)
        {
            if (!Game1.hasLoadedGame)
                return;

            Cheats.OnDayOfMonthChanged();
        }

        private void GraphicsEvents_DrawTick(object sender, EventArgs e)
        {
            if (!Game1.hasLoadedGame)
                return;

            Cheats.OnDrawTick();
        }

        private void TimeEvents_TimeOfDayChanged(object sender, EventArgsIntChanged e)
        {
            if (!Game1.hasLoadedGame)
                return;

            Cheats.OnTimeOfDayChanged();
        }

        private void Events_UpdateTick(object sender, EventArgs e)
        {
            if (!Game1.hasLoadedGame)
                return;
            Cheats.OnUpdate();
        }
    }
}
