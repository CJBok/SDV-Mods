using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu {
    public class CheatsOptionsCheckbox : OptionsElement {
        public static Rectangle sourceRectUnchecked = new Rectangle(227, 425, 9, 9);
        public static Rectangle sourceRectChecked = new Rectangle(236, 425, 9, 9);
        public const int pixelsWide = 9;
        public bool isChecked;

        public CheatsOptionsCheckbox(string label, int whichOption, int x = -1, int y = -1)
          : base(label, x, y, 9 * Game1.pixelZoom, 9 * Game1.pixelZoom, whichOption) {
            switch (whichOption) {
                case 0:
                    isChecked = CJBCheatsMenu.config.increasedMovement;
                    break;
                case 1:
                    isChecked = CJBCheatsMenu.config.infiniteStamina;
                    break;
                case 2:
                    isChecked = CJBCheatsMenu.config.infiniteHealth;
                    break;
                case 3:
                    isChecked = CJBCheatsMenu.config.infiniteWateringCan;
                    break;
                case 4:
                    isChecked = CJBCheatsMenu.config.instantCatch;
                    break;
                case 5:
                    isChecked = CJBCheatsMenu.config.throwBobberMax;
                    break;
                case 6:
                    isChecked = CJBCheatsMenu.config.oneHitKill;
                    break;
                case 7:
                    isChecked = CJBCheatsMenu.config.maxDailyLuck;
                    break;
                case 9:
                    isChecked = CJBCheatsMenu.config.alwaysGiveGift;
                    break;
                case 10:
                    isChecked = CJBCheatsMenu.config.freezeTimeInside;
                    break;
                case 11:
                    isChecked = CJBCheatsMenu.config.freezeTime;
                    break;
                case 12:
                    isChecked = CJBCheatsMenu.config.freezeTimeCaves;
                    break;
                case 13:
                    isChecked = CJBCheatsMenu.config.durableFences;
                    break;
                case 14:
                    isChecked = CJBCheatsMenu.config.alwaysTreasure;
                    break;
                case 15:
                    isChecked = CJBCheatsMenu.config.oneHitBreak;
                    break;
                case 16:
                    isChecked = CJBCheatsMenu.config.instantBite;
                    break;
                case 17:
                    isChecked = CJBCheatsMenu.config.noFriendshipDecay;
                    break;
                case 18:
                    isChecked = CJBCheatsMenu.config.instantBuild;
                    break;
                case 19:
                    isChecked = CJBCheatsMenu.config.autoFeed;
                    break;
                case 20:
                    isChecked = CJBCheatsMenu.config.infiniteHay;
                    break;
                case 21:
                    isChecked = CJBCheatsMenu.config.durableTackles;
                    break;
                case 22:
                    isChecked = CJBCheatsMenu.config.harvestSickle;
                    break;

                case 100:
                    isChecked = CJBCheatsMenu.config.fastFurnace;
                    break;
                case 101:
                    isChecked = CJBCheatsMenu.config.fastRecyclingMachine;
                    break;
                case 102:
                    isChecked = CJBCheatsMenu.config.fastCrystalarium;
                    break;
                case 103:
                    isChecked = CJBCheatsMenu.config.fastIncubator;
                    break;
                case 104:
                    isChecked = CJBCheatsMenu.config.fastSlimeIncubator;
                    break;
                case 105:
                    isChecked = CJBCheatsMenu.config.fastKeg;
                    break;
                case 106:
                    isChecked = CJBCheatsMenu.config.fastPreservesJar;
                    break;
                case 107:
                    isChecked = CJBCheatsMenu.config.fastCheesePress;
                    break;
                case 108:
                    isChecked = CJBCheatsMenu.config.fastMayonnaiseMachine;
                    break;
                case 109:
                    isChecked = CJBCheatsMenu.config.fastLoom;
                    break;
                case 110:
                    isChecked = CJBCheatsMenu.config.fastOilMaker;
                    break;
                case 111:
                    isChecked = CJBCheatsMenu.config.fastSeedMaker;
                    break;
                case 112:
                    isChecked = CJBCheatsMenu.config.fastCharcoalKiln;
                    break;
                case 113:
                    isChecked = CJBCheatsMenu.config.fastSlimeEggPress;
                    break;
                case 114:
                    isChecked = CJBCheatsMenu.config.fastBeeHouse;
                    break;
                case 115:
                    isChecked = CJBCheatsMenu.config.fastMushroomBox;
                    break;
                case 116:
                    isChecked = CJBCheatsMenu.config.fastTapper;
                    break;
                case 117:
                    isChecked = CJBCheatsMenu.config.fastLightningRod;
                    break;

            }
        }

        public override void receiveLeftClick(int x, int y) {
            if (this.greyedOut)
                return;
            Game1.soundBank.PlayCue("drumkit6");
            base.receiveLeftClick(x, y);
            this.isChecked = !this.isChecked;
            switch (whichOption) {
                case 0:
                    CJBCheatsMenu.config.increasedMovement = isChecked;
                    break;
                case 1:
                    CJBCheatsMenu.config.infiniteStamina = isChecked;
                    break;
                case 2:
                    CJBCheatsMenu.config.infiniteHealth = isChecked;
                    break;
                case 3:
                    CJBCheatsMenu.config.infiniteWateringCan = isChecked;
                    break;
                case 4:
                    CJBCheatsMenu.config.instantCatch = isChecked;
                    break;
                case 5:
                    CJBCheatsMenu.config.throwBobberMax = isChecked;
                    break;
                case 6:
                    CJBCheatsMenu.config.oneHitKill = isChecked;
                    break;
                case 7:
                    CJBCheatsMenu.config.maxDailyLuck = isChecked;
                    break;
                case 9:
                    CJBCheatsMenu.config.alwaysGiveGift = isChecked;
                    break;
                case 10:
                    CJBCheatsMenu.config.freezeTimeInside = isChecked;
                    break;
                case 11:
                    CJBCheatsMenu.config.freezeTime = isChecked;
                    break;
                case 12:
                    CJBCheatsMenu.config.freezeTimeCaves = isChecked;
                    break;
                case 13:
                    CJBCheatsMenu.config.durableFences = isChecked;
                    break;
                case 14:
                    CJBCheatsMenu.config.alwaysTreasure = isChecked;
                    break;
                case 15:
                    CJBCheatsMenu.config.oneHitBreak = isChecked;
                    break;
                case 16:
                    CJBCheatsMenu.config.instantBite = isChecked;
                    break;
                case 17:
                    CJBCheatsMenu.config.noFriendshipDecay = isChecked;
                    break;
                case 18:
                    CJBCheatsMenu.config.instantBuild = isChecked;
                    break;
                case 19:
                    CJBCheatsMenu.config.autoFeed = isChecked;
                    break;
                case 20:
                    CJBCheatsMenu.config.infiniteHay = isChecked;
                    break;
                case 21:
                    CJBCheatsMenu.config.durableTackles = isChecked;
                    break;
                case 22:
                    CJBCheatsMenu.config.harvestSickle = isChecked;
                    break;

                case 100:
                    CJBCheatsMenu.config.fastFurnace = isChecked;
                    break;
                case 101:
                    CJBCheatsMenu.config.fastRecyclingMachine = isChecked;
                    break;
                case 102:
                    CJBCheatsMenu.config.fastCrystalarium = isChecked;
                    break;
                case 103:
                    CJBCheatsMenu.config.fastIncubator = isChecked;
                    break;
                case 104:
                    CJBCheatsMenu.config.fastSlimeIncubator = isChecked;
                    break;
                case 105:
                    CJBCheatsMenu.config.fastKeg = isChecked;
                    break;
                case 106:
                    CJBCheatsMenu.config.fastPreservesJar = isChecked;
                    break;
                case 107:
                    CJBCheatsMenu.config.fastCheesePress = isChecked;
                    break;
                case 108:
                    CJBCheatsMenu.config.fastMayonnaiseMachine = isChecked;
                    break;
                case 109:
                    CJBCheatsMenu.config.fastLoom = isChecked;
                    break;
                case 110:
                    CJBCheatsMenu.config.fastOilMaker = isChecked;
                    break;
                case 111:
                    CJBCheatsMenu.config.fastSeedMaker = isChecked;
                    break;
                case 112:
                    CJBCheatsMenu.config.fastCharcoalKiln = isChecked;
                    break;
                case 113:
                    CJBCheatsMenu.config.fastSlimeEggPress = isChecked;
                    break;
                case 114:
                    CJBCheatsMenu.config.fastBeeHouse = isChecked;
                    break;
                case 115:
                    CJBCheatsMenu.config.fastMushroomBox = isChecked;
                    break;
                case 116:
                    CJBCheatsMenu.config.fastTapper = isChecked;
                    break;
                case 117:
                    CJBCheatsMenu.config.fastLightningRod = isChecked;
                    break;
            }


            CJBCheatsMenu.SaveConfig();
        }

        public override void draw(SpriteBatch b, int slotX, int slotY) {
            b.Draw(Game1.mouseCursors, new Vector2((float)(slotX + this.bounds.X), (float)(slotY + this.bounds.Y)), new Rectangle?(this.isChecked ? OptionsCheckbox.sourceRectChecked : OptionsCheckbox.sourceRectUnchecked), Color.White * (this.greyedOut ? 0.33f : 1f), 0.0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.4f);
            base.draw(b, slotX, slotY);
        }
    }
}
