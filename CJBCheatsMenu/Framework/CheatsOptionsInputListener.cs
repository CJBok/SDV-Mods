using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Quests;
using SFarmer = StardewValley.Farmer;

namespace CJBCheatsMenu.Framework
{
    internal class CheatsOptionsInputListener : OptionsElement
    {
        /*********
        ** Properties
        *********/
        /// <summary>The mod settings.</summary>
        private readonly ModConfig Config;

        /// <summary>The cheats helper.</summary>
        private readonly Cheats Cheats;

        /// <summary>The translated 'press new key' label.</summary>
        private readonly string PressNewKeyLabel;

        private readonly Rectangle SetButtonSprite = new Rectangle(294, 428, 21, 11);
        private readonly List<string> ButtonNames = new List<string>();
        private string ListenerMessage;
        private bool Listening;
        private Rectangle SetButtonBounds;


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="label">The field label.</param>
        /// <param name="whichOption">The option ID.</param>
        /// <param name="slotWidth">The field width.</param>
        /// <param name="config">The mod settings.</param>
        /// <param name="cheats">The cheats helper.</param>
        /// <param name="i18n">Provides translations for the mod.</param>
        public CheatsOptionsInputListener(string label, int whichOption, int slotWidth, ModConfig config, Cheats cheats, ITranslationHelper i18n)
          : base(label, -1, -1, slotWidth + 1, 11 * Game1.pixelZoom, whichOption)
        {
            this.Config = config;
            this.Cheats = cheats;
            this.PressNewKeyLabel = i18n.Get("messages.press-new-key");

            this.SetButtonBounds = new Rectangle(slotWidth - 28 * Game1.pixelZoom, -1 + Game1.pixelZoom * 3, 21 * Game1.pixelZoom, 11 * Game1.pixelZoom);
            if (whichOption == -1)
                return;

            switch (whichOption)
            {
                case 1000:
                    this.ButtonNames.Add(this.Config.OpenMenuKey.ToString());
                    break;
                case 1001:
                    this.ButtonNames.Add(this.Config.FreezeTimeKey.ToString());
                    break;
                case 1002:
                    this.ButtonNames.Add(this.Config.GrowTreeKey.ToString());
                    break;
                case 1003:
                    this.ButtonNames.Add(this.Config.GrowCropsKey.ToString());
                    break;
            }
        }

        public override void receiveLeftClick(int x, int y)
        {
            if (this.greyedOut || this.Listening || !this.SetButtonBounds.Contains(x, y))
                return;
            if (!this.ButtonNames.Any())
            {
                switch (this.whichOption)
                {
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
                        this.Cheats.WaterAllFields();
                        break;
                    case 10:
                        this.Cheats.SetWeatherForNextDay(Game1.weather_sunny);
                        break;
                    case 11:
                        this.Cheats.SetWeatherForNextDay(Game1.weather_rain);
                        break;
                    case 12:
                        this.Cheats.SetWeatherForNextDay(Game1.weather_lightning);
                        break;
                    case 13:
                        this.Cheats.SetWeatherForNextDay(Game1.weather_snow);
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
                        int lvl1 = Game1.player.newLevels.Count;
                        Game1.player.gainExperience(0, CJB.GetExperiencePoints(Game1.player.FarmingLevel));
                        if (lvl1 < Game1.player.newLevels.Count)
                            Game1.player.newLevels.RemoveAt(Game1.player.newLevels.Count - 1);
                        Game1.exitActiveMenu();
                        Game1.activeClickableMenu = new LevelUpMenu(0, Game1.player.FarmingLevel);
                        break;
                    case 201:
                        int lvl2 = Game1.player.newLevels.Count;
                        Game1.player.gainExperience(3, CJB.GetExperiencePoints(Game1.player.MiningLevel));
                        if (lvl2 < Game1.player.newLevels.Count)
                            Game1.player.newLevels.RemoveAt(Game1.player.newLevels.Count - 1);
                        Game1.exitActiveMenu();
                        Game1.activeClickableMenu = new LevelUpMenu(3, Game1.player.MiningLevel);
                        break;
                    case 202:
                        int lvl3 = Game1.player.newLevels.Count;
                        Game1.player.gainExperience(2, CJB.GetExperiencePoints(Game1.player.ForagingLevel));
                        if (lvl3 < Game1.player.newLevels.Count)
                            Game1.player.newLevels.RemoveAt(Game1.player.newLevels.Count - 1);
                        Game1.exitActiveMenu();
                        Game1.activeClickableMenu = new LevelUpMenu(2, Game1.player.ForagingLevel);
                        break;
                    case 203:
                        int lvl4 = Game1.player.newLevels.Count;
                        Game1.player.gainExperience(1, CJB.GetExperiencePoints(Game1.player.FishingLevel));
                        if (lvl4 < Game1.player.newLevels.Count)
                            Game1.player.newLevels.RemoveAt(Game1.player.newLevels.Count - 1);
                        Game1.exitActiveMenu();
                        Game1.activeClickableMenu = new LevelUpMenu(1, Game1.player.FishingLevel);
                        break;
                    case 204:
                        int lvl5 = Game1.player.newLevels.Count;
                        Game1.player.gainExperience(4, CJB.GetExperiencePoints(Game1.player.CombatLevel));
                        if (lvl5 < Game1.player.newLevels.Count)
                            Game1.player.newLevels.RemoveAt(Game1.player.newLevels.Count - 1);
                        Game1.exitActiveMenu();
                        Game1.activeClickableMenu = new LevelUpMenu(4, Game1.player.CombatLevel);
                        break;
                    case 205:
                        Game1.player.maxHealth -= 5 * Game1.player.CombatLevel;
                        Game1.player.experiencePoints[0] = 0;
                        Game1.player.experiencePoints[1] = 0;
                        Game1.player.experiencePoints[2] = 0;
                        Game1.player.experiencePoints[3] = 0;
                        Game1.player.experiencePoints[4] = 0;
                        Game1.player.FarmingLevel = 0;
                        Game1.player.MiningLevel = 0;
                        Game1.player.ForagingLevel = 0;
                        Game1.player.FishingLevel = 0;
                        Game1.player.CombatLevel = 0;
                        if (Game1.player.professions.Contains(24))
                            Game1.player.maxHealth -= 15;
                        if (Game1.player.professions.Contains(27))
                            Game1.player.maxHealth -= 25;
                        Game1.player.health = Game1.player.maxHealth;
                        Game1.player.professions.Clear();
                        break;
                    case int n when (n >= 300 && n <= 399):
                        for (int i = Game1.player.questLog.Count - 1; i >= 0; i--)
                        {
                            Quest q = Game1.player.questLog[i];
                            if (this.label == q.questTitle)
                            {
                                q.questComplete();
                            }
                        }
                        Game1.exitActiveMenu();
                        break;
                }
            }
            else
            {
                this.Listening = true;
                Game1.soundBank.PlayCue("breathin");
                GameMenu.forcePreventClose = true;
                this.ListenerMessage = this.PressNewKeyLabel;
            }
        }

        public override void receiveKeyPress(Keys key)
        {
            if (this.greyedOut || !this.Listening)
                return;
            if (key == Keys.Escape)
            {
                Game1.soundBank.PlayCue("bigDeSelect");
                this.Listening = false;
                GameMenu.forcePreventClose = false;
            }
            else
            {
                switch (this.whichOption)
                {
                    case 1000:
                        this.Config.OpenMenuKey = (SButton)key;
                        break;
                    case 1001:
                        this.Config.FreezeTimeKey = (SButton)key;
                        break;
                    case 1002:
                        this.Config.GrowTreeKey = (SButton)key;
                        break;
                    case 1003:
                        this.Config.GrowCropsKey = (SButton)key;
                        break;
                }
                this.ButtonNames[0] = key.ToString();
                Game1.soundBank.PlayCue("coin");
                this.Listening = false;
                GameMenu.forcePreventClose = false;
            }
        }

        public override void draw(SpriteBatch spriteBatch, int slotX, int slotY)
        {
            string lvl = "";
            SFarmer plr = Game1.player;
            switch (this.whichOption)
            {
                case 200:
                    lvl = plr.farmingLevel.ToString();
                    this.greyedOut = plr.FarmingLevel >= 10;
                    break;
                case 201:
                    lvl = plr.miningLevel.ToString();
                    this.greyedOut = plr.MiningLevel >= 10;
                    break;
                case 202:
                    lvl = plr.foragingLevel.ToString();
                    this.greyedOut = plr.ForagingLevel >= 10;
                    break;
                case 203:
                    lvl = plr.fishingLevel.ToString();
                    this.greyedOut = plr.FishingLevel >= 10;
                    break;
                case 204:
                    lvl = plr.combatLevel.ToString();
                    this.greyedOut = plr.CombatLevel >= 10;
                    break;
            }
            if (lvl != "")
                Utility.drawTextWithShadow(spriteBatch, $"{this.label}: {lvl}", Game1.dialogueFont, new Vector2(this.bounds.X + slotX, this.bounds.Y + slotY), this.greyedOut ? Game1.textColor * 0.33f : Game1.textColor, 1f, 0.15f);
            else if (this.ButtonNames.Count == 0)
                Utility.drawTextWithShadow(spriteBatch, this.label, Game1.dialogueFont, new Vector2(this.bounds.X + slotX, this.bounds.Y + slotY), this.greyedOut ? Game1.textColor * 0.33f : Game1.textColor, 1f, 0.15f);
            else
                Utility.drawTextWithShadow(spriteBatch, this.label + ": " + this.ButtonNames.Last() + (this.ButtonNames.Count > 1 ? ", " + this.ButtonNames.First() : ""), Game1.dialogueFont, new Vector2(this.bounds.X + slotX, this.bounds.Y + slotY), this.greyedOut ? Game1.textColor * 0.33f : Game1.textColor, 1f, 0.15f);
            Utility.drawWithShadow(spriteBatch, Game1.mouseCursors, new Vector2(this.SetButtonBounds.X + slotX, this.SetButtonBounds.Y + slotY), this.SetButtonSprite, Color.White, 0.0f, Vector2.Zero, Game1.pixelZoom, false, 0.15f);
            if (!this.Listening)
                return;
            spriteBatch.Draw(Game1.staminaRect, new Rectangle(0, 0, Game1.graphics.GraphicsDevice.Viewport.Width, Game1.graphics.GraphicsDevice.Viewport.Height), new Rectangle(0, 0, 1, 1), Color.Black * 0.75f, 0.0f, Vector2.Zero, SpriteEffects.None, 0.999f);
            spriteBatch.DrawString(Game1.dialogueFont, this.ListenerMessage, Utility.getTopLeftPositionForCenteringOnScreen(Game1.tileSize * 3, Game1.tileSize), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9999f);
        }
    }
}
