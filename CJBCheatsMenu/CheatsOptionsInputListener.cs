using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardewValley.Menus;
using System.Collections.Generic;
using System.Linq;

namespace CJBCheatsMenu {
    class CheatsOptionsInputListener : OptionsElement {
        public static Rectangle setButtonSource = new Rectangle(294, 428, 21, 11);
        public List<string> buttonNames = new List<string>();
        private string listenerMessage;
        private bool listening;
        private Rectangle setbuttonBounds;

        public CheatsOptionsInputListener(string label, int whichOption, int slotWidth, int x = -1, int y = -1)
          : base(label, x, y, slotWidth - x, 11 * Game1.pixelZoom, whichOption)
        {
            this.setbuttonBounds = new Rectangle(slotWidth - 28 * Game1.pixelZoom, y + Game1.pixelZoom * 3, 21 * Game1.pixelZoom, 11 * Game1.pixelZoom);
            if (whichOption == -1)
                return;

            switch (whichOption) {
                case 1000:
                    buttonNames.Add(CJBCheatsMenu.config.openMenuKey);
                    break;
                case 1001:
                    buttonNames.Add(CJBCheatsMenu.config.freezeTimeKey);
                    break;
                case 1002:
                    buttonNames.Add(CJBCheatsMenu.config.growTreeKey);
                    break;
                case 1003:
                    buttonNames.Add(CJBCheatsMenu.config.growCropsKey);
                    break;
            }
        }

        public override void leftClickHeld(int x, int y) {
            int num = this.greyedOut ? 1 : 0;
        }

        public override void receiveLeftClick(int x, int y) {
            if (this.greyedOut || this.listening || !this.setbuttonBounds.Contains(x, y))
                return;
            if (buttonNames.Count() == 0) {
                switch (whichOption) {
                    case 2:
                        Game1.player.money += 100;
                        Game1.soundBank.PlayCue("coin");
                        break;
                    case 3:
                        Game1.player.money += 1000;
                        Game1.soundBank.PlayCue("coin");
                        break;
                    case 4:
                        Game1.player.money += 10000;
                        Game1.soundBank.PlayCue("coin");
                        break;
                    case 5:
                        Game1.player.money += 100000;
                        Game1.soundBank.PlayCue("coin");
                        break;
                    case 6:
                        Game1.player.clubCoins += 100;
                        Game1.soundBank.PlayCue("coin");
                        break;
                    case 7:
                        Game1.player.clubCoins += 1000;
                        Game1.soundBank.PlayCue("coin");
                        break;
                    case 8:
                        Game1.player.clubCoins += 10000;
                        Game1.soundBank.PlayCue("coin");
                        break;
                    case 9:
                        Game1.soundBank.PlayCue("glug");
                        Cheats.waterAllFields();
                        break;
                    case 10:
                        Cheats.setWeatherForNextDay(Game1.weather_sunny);
                        break;
                    case 11:
                        Cheats.setWeatherForNextDay(Game1.weather_rain);
                        break;
                    case 12:
                        Cheats.setWeatherForNextDay(Game1.weather_lightning);
                        break;
                    case 13:
                        Cheats.setWeatherForNextDay(Game1.weather_snow);
                        break;
                    case 14:
                        Game1.warpFarmer("FarmHouse", 9, 11, false);
                        Game1.exitActiveMenu();
                        break;


                    case 100:
                        Game1.warpFarmer("Farm", 64, 15, false);
                        Game1.exitActiveMenu();
                        break;
                    case 101:
                        Game1.warpFarmer("Town", 43, 57, false);
                        Game1.exitActiveMenu();
                        break;
                    case 102:
                        Game1.warpFarmer("Town", 94, 82, false);
                        Game1.exitActiveMenu();
                        break;
                    case 103:
                        Game1.warpFarmer("Town", 102, 90, false);
                        Game1.exitActiveMenu();
                        break;
                    case 104:
                        Game1.warpFarmer("Town", 45, 71, false);
                        Game1.exitActiveMenu();
                        break;
                    case 105:
                        Game1.warpFarmer("Town", 52, 20, false);
                        Game1.exitActiveMenu();
                        break;
                    case 106:
                        Game1.warpFarmer("Mountain", 12, 26, false);
                        Game1.exitActiveMenu();
                        break;
                    case 107:
                        Game1.warpFarmer("Mountain", 76, 9, false);
                        Game1.exitActiveMenu();
                        break;
                    case 108:
                        Game1.warpFarmer("Mountain", 127, 12, false);
                        Game1.exitActiveMenu();
                        break;
                    case 109:
                        Game1.warpFarmer("Mine", 13, 10, false);
                        Game1.exitActiveMenu();
                        break;
                    case 110:
                        Game1.warpFarmer("Beach", 30, 34, false);
                        Game1.exitActiveMenu();
                        break;
                    case 111:
                        Game1.warpFarmer("Beach", 87, 26, false);
                        Game1.exitActiveMenu();
                        break;
                    case 112:
                        Game1.warpFarmer("Desert", 18, 28, false);
                        Game1.exitActiveMenu();
                        break;
                    case 113:
                        Game1.warpFarmer("Forest", 90, 16, false);
                        Game1.exitActiveMenu();
                        break;
                    case 114:
                        Game1.warpFarmer("Forest", 5, 27, false);
                        Game1.exitActiveMenu();
                        break;
                    case 115:
                        Game1.warpFarmer("Forest", 34, 96, false);
                        Game1.exitActiveMenu();
                        break;
                    case 116:
                        Game1.warpFarmer("Woods", 58, 15, false);
                        Game1.exitActiveMenu();
                        break;
                    case 117:
                        Game1.warpFarmer("Sewer", 3, 48, false);
                        Game1.exitActiveMenu();
                        break;
                    case 118:
                        Game1.warpFarmer("Railroad", 10, 57, false);
                        Game1.exitActiveMenu();
                        break;
                    case 119:
                        Game1.warpFarmer("SandyHouse", 4, 8, false);
                        Game1.exitActiveMenu();
                        break;
                    case 120:
                        Game1.warpFarmer("Club", 8, 11, false);
                        Game1.exitActiveMenu();
                        break;

                    case 200:
                        int lvl1 = Game1.player.newLevels.Count();
                        Game1.player.gainExperience(0, CJB.getExperiencePoints(Game1.player.farmingLevel));
                        if (lvl1 < Game1.player.newLevels.Count()) {
                            Game1.player.newLevels.RemoveAt(Game1.player.newLevels.Count() - 1);
                        }
                        Game1.exitActiveMenu();
                        Game1.activeClickableMenu = new LevelUpMenu(0, Game1.player.farmingLevel);
                        break;
                    case 201:
                        int lvl2 = Game1.player.newLevels.Count();
                        Game1.player.gainExperience(3, CJB.getExperiencePoints(Game1.player.miningLevel));
                        if (lvl2 < Game1.player.newLevels.Count()) {
                            Game1.player.newLevels.RemoveAt(Game1.player.newLevels.Count() - 1);
                        }
                        Game1.exitActiveMenu();
                        Game1.activeClickableMenu = new LevelUpMenu(3, Game1.player.miningLevel);
                        break;
                    case 202:
                        int lvl3 = Game1.player.newLevels.Count();
                        Game1.player.gainExperience(2, CJB.getExperiencePoints(Game1.player.foragingLevel));
                        if (lvl3 < Game1.player.newLevels.Count()) {
                            Game1.player.newLevels.RemoveAt(Game1.player.newLevels.Count() - 1);
                        }
                        Game1.exitActiveMenu();
                        Game1.activeClickableMenu = new LevelUpMenu(2, Game1.player.foragingLevel);
                        break;
                    case 203:
                        int lvl4 = Game1.player.newLevels.Count();
                        Game1.player.gainExperience(1, CJB.getExperiencePoints(Game1.player.fishingLevel));
                        if (lvl4 < Game1.player.newLevels.Count()) {
                            Game1.player.newLevels.RemoveAt(Game1.player.newLevels.Count() - 1);
                        }
                        Game1.exitActiveMenu();
                        Game1.activeClickableMenu = new LevelUpMenu(1, Game1.player.fishingLevel);
                        break;
                    case 204:
                        int lvl5 = Game1.player.newLevels.Count();
                        Game1.player.gainExperience(4, CJB.getExperiencePoints(Game1.player.combatLevel));
                        if (lvl5 < Game1.player.newLevels.Count()) {
                            Game1.player.newLevels.RemoveAt(Game1.player.newLevels.Count() - 1);
                        }
                        Game1.exitActiveMenu();
                        Game1.activeClickableMenu = new LevelUpMenu(4, Game1.player.combatLevel);
                        break;
                    case 205:
                        Game1.player.maxHealth -= 5 * Game1.player.combatLevel;
                        Game1.player.experiencePoints[0] = 0;
                        Game1.player.experiencePoints[1] = 0;
                        Game1.player.experiencePoints[2] = 0;
                        Game1.player.experiencePoints[3] = 0;
                        Game1.player.experiencePoints[4] = 0;
                        Game1.player.farmingLevel = 0;
                        Game1.player.miningLevel = 0;
                        Game1.player.foragingLevel = 0;
                        Game1.player.fishingLevel = 0;
                        Game1.player.combatLevel = 0;
                        if (Game1.player.professions.Contains(24))
                            Game1.player.maxHealth -= 15;
                        if (Game1.player.professions.Contains(27))
                            Game1.player.maxHealth -= 25;
                        Game1.player.health = Game1.player.maxHealth;
                        Game1.player.professions.Clear();
                        break;

                }
            } else {
                this.listening = true;
                Game1.soundBank.PlayCue("breathin");
                GameMenu.forcePreventClose = true;
                this.listenerMessage = "Press new key...";
            }
        }

        public override void receiveKeyPress(Keys key) {
            if (this.greyedOut || !this.listening)
                return;
            if (key == Keys.Escape) {
                Game1.soundBank.PlayCue("bigDeSelect");
                this.listening = false;
                GameMenu.forcePreventClose = false;
            } else {
                switch (whichOption) {
                    case 1000:
                        CJBCheatsMenu.config.openMenuKey = key.ToString();
                        CJBCheatsMenu.SaveConfig();
                        break;
                    case 1001:
                        CJBCheatsMenu.config.freezeTimeKey = key.ToString();
                        CJBCheatsMenu.SaveConfig();
                        break;
                    case 1002:
                        CJBCheatsMenu.config.growTreeKey = key.ToString();
                        CJBCheatsMenu.SaveConfig();
                        break;
                    case 1003:
                        CJBCheatsMenu.config.growCropsKey = key.ToString();
                        CJBCheatsMenu.SaveConfig();
                        break;
                }
                this.buttonNames[0] = key.ToString();
                Game1.soundBank.PlayCue("coin");
                this.listening = false;
                GameMenu.forcePreventClose = false;
            }
        }

        public override void draw(SpriteBatch b, int slotX, int slotY) {
            string lvl = "";
            Farmer plr = Game1.player;
            switch (whichOption) {
                case 200:
                    lvl = plr.farmingLevel.ToString();
                    greyedOut = plr.farmingLevel >= 10;
                    break;
                case 201:
                    lvl = plr.miningLevel.ToString();
                    greyedOut = plr.miningLevel >= 10;
                    break;
                case 202:
                    lvl = plr.foragingLevel.ToString();
                    greyedOut = plr.foragingLevel >= 10;
                    break;
                case 203:
                    lvl = plr.fishingLevel.ToString();
                    greyedOut = plr.fishingLevel >= 10;
                    break;
                case 204:
                    lvl = plr.combatLevel.ToString();
                    greyedOut = plr.combatLevel >= 10;
                    break;
            }
            if (!lvl.Equals(""))
                Utility.drawTextWithShadow(b, this.label + ": " + lvl, Game1.dialogueFont, new Vector2((float)(this.bounds.X + slotX), (float)(this.bounds.Y + slotY)), greyedOut ? Game1.textColor * 0.33f : Game1.textColor, 1f, 0.15f, -1, -1, 1f, 3);
            else if (buttonNames.Count() == 0)
                Utility.drawTextWithShadow(b, this.label, Game1.dialogueFont, new Vector2((float)(this.bounds.X + slotX), (float)(this.bounds.Y + slotY)), greyedOut ? Game1.textColor * 0.33f : Game1.textColor, 1f, 0.15f, -1, -1, 1f, 3);
            else
                Utility.drawTextWithShadow(b, this.label + ": " + Enumerable.Last(this.buttonNames) + (Enumerable.Count(this.buttonNames) > 1 ? ", " + Enumerable.First(this.buttonNames) : ""), Game1.dialogueFont, new Vector2(this.bounds.X + slotX, this.bounds.Y + slotY), greyedOut ? Game1.textColor * 0.33f : Game1.textColor, 1f, 0.15f, -1, -1, 1f, 3);
            Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2(this.setbuttonBounds.X + slotX, this.setbuttonBounds.Y + slotY), setButtonSource, Color.White, 0.0f, Vector2.Zero, (float)Game1.pixelZoom, false, 0.15f, -1, -1, 0.35f);
            if (!this.listening)
                return;
            b.Draw(Game1.staminaRect, new Rectangle(0, 0, Game1.graphics.GraphicsDevice.Viewport.Width, Game1.graphics.GraphicsDevice.Viewport.Height), new Rectangle?(new Rectangle(0, 0, 1, 1)), Color.Black * 0.75f, 0.0f, Vector2.Zero, SpriteEffects.None, 0.999f);
            b.DrawString(Game1.dialogueFont, this.listenerMessage, Utility.getTopLeftPositionForCenteringOnScreen(Game1.tileSize * 3, Game1.tileSize, 0, 0), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9999f);
        }
    }
}
