using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu
{
    public class CheatsOptionsCheckbox : OptionsElement
    {
        /*********
        ** Accessors
        *********/
        public static Rectangle UncheckedSprite = new Rectangle(227, 425, 9, 9);
        public static Rectangle CheckedSprite = new Rectangle(236, 425, 9, 9);
        public const int Width = 9;
        public bool IsChecked;


        /*********
        ** Public methods
        *********/
        public CheatsOptionsCheckbox(string label, int whichOption, int x = -1, int y = -1)
          : base(label, x, y, 9 * Game1.pixelZoom, 9 * Game1.pixelZoom, whichOption)
        {
            switch (whichOption)
            {
                case 0:
                    this.IsChecked = CJBCheatsMenu.Config.IncreasedMovement;
                    break;
                case 1:
                    this.IsChecked = CJBCheatsMenu.Config.InfiniteStamina;
                    break;
                case 2:
                    this.IsChecked = CJBCheatsMenu.Config.InfiniteHealth;
                    break;
                case 3:
                    this.IsChecked = CJBCheatsMenu.Config.InfiniteWateringCan;
                    break;
                case 4:
                    this.IsChecked = CJBCheatsMenu.Config.InstantCatch;
                    break;
                case 5:
                    this.IsChecked = CJBCheatsMenu.Config.ThrowBobberMax;
                    break;
                case 6:
                    this.IsChecked = CJBCheatsMenu.Config.OneHitKill;
                    break;
                case 7:
                    this.IsChecked = CJBCheatsMenu.Config.MaxDailyLuck;
                    break;
                case 9:
                    this.IsChecked = CJBCheatsMenu.Config.AlwaysGiveGift;
                    break;
                case 10:
                    this.IsChecked = CJBCheatsMenu.Config.FreezeTimeInside;
                    break;
                case 11:
                    this.IsChecked = CJBCheatsMenu.Config.FreezeTime;
                    break;
                case 12:
                    this.IsChecked = CJBCheatsMenu.Config.FreezeTimeCaves;
                    break;
                case 13:
                    this.IsChecked = CJBCheatsMenu.Config.DurableFences;
                    break;
                case 14:
                    this.IsChecked = CJBCheatsMenu.Config.AlwaysTreasure;
                    break;
                case 15:
                    this.IsChecked = CJBCheatsMenu.Config.OneHitBreak;
                    break;
                case 16:
                    this.IsChecked = CJBCheatsMenu.Config.InstantBite;
                    break;
                case 17:
                    this.IsChecked = CJBCheatsMenu.Config.NoFriendshipDecay;
                    break;
                case 18:
                    this.IsChecked = CJBCheatsMenu.Config.InstantBuild;
                    break;
                case 19:
                    this.IsChecked = CJBCheatsMenu.Config.AutoFeed;
                    break;
                case 20:
                    this.IsChecked = CJBCheatsMenu.Config.InfiniteHay;
                    break;
                case 21:
                    this.IsChecked = CJBCheatsMenu.Config.DurableTackles;
                    break;
                case 22:
                    this.IsChecked = CJBCheatsMenu.Config.HarvestSickle;
                    break;

                case 100:
                    this.IsChecked = CJBCheatsMenu.Config.FastFurnace;
                    break;
                case 101:
                    this.IsChecked = CJBCheatsMenu.Config.FastRecyclingMachine;
                    break;
                case 102:
                    this.IsChecked = CJBCheatsMenu.Config.FastCrystalarium;
                    break;
                case 103:
                    this.IsChecked = CJBCheatsMenu.Config.FastIncubator;
                    break;
                case 104:
                    this.IsChecked = CJBCheatsMenu.Config.FastSlimeIncubator;
                    break;
                case 105:
                    this.IsChecked = CJBCheatsMenu.Config.FastKeg;
                    break;
                case 106:
                    this.IsChecked = CJBCheatsMenu.Config.FastPreservesJar;
                    break;
                case 107:
                    this.IsChecked = CJBCheatsMenu.Config.FastCheesePress;
                    break;
                case 108:
                    this.IsChecked = CJBCheatsMenu.Config.FastMayonnaiseMachine;
                    break;
                case 109:
                    this.IsChecked = CJBCheatsMenu.Config.FastLoom;
                    break;
                case 110:
                    this.IsChecked = CJBCheatsMenu.Config.FastOilMaker;
                    break;
                case 111:
                    this.IsChecked = CJBCheatsMenu.Config.FastSeedMaker;
                    break;
                case 112:
                    this.IsChecked = CJBCheatsMenu.Config.FastCharcoalKiln;
                    break;
                case 113:
                    this.IsChecked = CJBCheatsMenu.Config.FastSlimeEggPress;
                    break;
                case 114:
                    this.IsChecked = CJBCheatsMenu.Config.FastBeeHouse;
                    break;
                case 115:
                    this.IsChecked = CJBCheatsMenu.Config.FastMushroomBox;
                    break;
                case 116:
                    this.IsChecked = CJBCheatsMenu.Config.FastTapper;
                    break;
                case 117:
                    this.IsChecked = CJBCheatsMenu.Config.FastLightningRod;
                    break;
                case 118:
                    this.IsChecked = CJBCheatsMenu.Config.FastCask;
                    break;
                case 119:
                    this.IsChecked = CJBCheatsMenu.Config.FastWormBin;
                    break;
            }
        }

        public override void receiveLeftClick(int x, int y)
        {
            if (this.greyedOut)
                return;
            Game1.soundBank.PlayCue("drumkit6");
            base.receiveLeftClick(x, y);
            this.IsChecked = !this.IsChecked;
            switch (whichOption)
            {
                case 0:
                    CJBCheatsMenu.Config.IncreasedMovement = this.IsChecked;
                    break;
                case 1:
                    CJBCheatsMenu.Config.InfiniteStamina = this.IsChecked;
                    break;
                case 2:
                    CJBCheatsMenu.Config.InfiniteHealth = this.IsChecked;
                    break;
                case 3:
                    CJBCheatsMenu.Config.InfiniteWateringCan = this.IsChecked;
                    break;
                case 4:
                    CJBCheatsMenu.Config.InstantCatch = this.IsChecked;
                    break;
                case 5:
                    CJBCheatsMenu.Config.ThrowBobberMax = this.IsChecked;
                    break;
                case 6:
                    CJBCheatsMenu.Config.OneHitKill = this.IsChecked;
                    break;
                case 7:
                    CJBCheatsMenu.Config.MaxDailyLuck = this.IsChecked;
                    break;
                case 9:
                    CJBCheatsMenu.Config.AlwaysGiveGift = this.IsChecked;
                    break;
                case 10:
                    CJBCheatsMenu.Config.FreezeTimeInside = this.IsChecked;
                    break;
                case 11:
                    CJBCheatsMenu.Config.FreezeTime = this.IsChecked;
                    break;
                case 12:
                    CJBCheatsMenu.Config.FreezeTimeCaves = this.IsChecked;
                    break;
                case 13:
                    CJBCheatsMenu.Config.DurableFences = this.IsChecked;
                    break;
                case 14:
                    CJBCheatsMenu.Config.AlwaysTreasure = this.IsChecked;
                    break;
                case 15:
                    CJBCheatsMenu.Config.OneHitBreak = this.IsChecked;
                    break;
                case 16:
                    CJBCheatsMenu.Config.InstantBite = this.IsChecked;
                    break;
                case 17:
                    CJBCheatsMenu.Config.NoFriendshipDecay = this.IsChecked;
                    break;
                case 18:
                    CJBCheatsMenu.Config.InstantBuild = this.IsChecked;
                    break;
                case 19:
                    CJBCheatsMenu.Config.AutoFeed = this.IsChecked;
                    break;
                case 20:
                    CJBCheatsMenu.Config.InfiniteHay = this.IsChecked;
                    break;
                case 21:
                    CJBCheatsMenu.Config.DurableTackles = this.IsChecked;
                    break;
                case 22:
                    CJBCheatsMenu.Config.HarvestSickle = this.IsChecked;
                    break;

                case 100:
                    CJBCheatsMenu.Config.FastFurnace = this.IsChecked;
                    break;
                case 101:
                    CJBCheatsMenu.Config.FastRecyclingMachine = this.IsChecked;
                    break;
                case 102:
                    CJBCheatsMenu.Config.FastCrystalarium = this.IsChecked;
                    break;
                case 103:
                    CJBCheatsMenu.Config.FastIncubator = this.IsChecked;
                    break;
                case 104:
                    CJBCheatsMenu.Config.FastSlimeIncubator = this.IsChecked;
                    break;
                case 105:
                    CJBCheatsMenu.Config.FastKeg = this.IsChecked;
                    break;
                case 106:
                    CJBCheatsMenu.Config.FastPreservesJar = this.IsChecked;
                    break;
                case 107:
                    CJBCheatsMenu.Config.FastCheesePress = this.IsChecked;
                    break;
                case 108:
                    CJBCheatsMenu.Config.FastMayonnaiseMachine = this.IsChecked;
                    break;
                case 109:
                    CJBCheatsMenu.Config.FastLoom = this.IsChecked;
                    break;
                case 110:
                    CJBCheatsMenu.Config.FastOilMaker = this.IsChecked;
                    break;
                case 111:
                    CJBCheatsMenu.Config.FastSeedMaker = this.IsChecked;
                    break;
                case 112:
                    CJBCheatsMenu.Config.FastCharcoalKiln = this.IsChecked;
                    break;
                case 113:
                    CJBCheatsMenu.Config.FastSlimeEggPress = this.IsChecked;
                    break;
                case 114:
                    CJBCheatsMenu.Config.FastBeeHouse = this.IsChecked;
                    break;
                case 115:
                    CJBCheatsMenu.Config.FastMushroomBox = this.IsChecked;
                    break;
                case 116:
                    CJBCheatsMenu.Config.FastTapper = this.IsChecked;
                    break;
                case 117:
                    CJBCheatsMenu.Config.FastLightningRod = this.IsChecked;
                    break;
                case 118:
                    CJBCheatsMenu.Config.FastCask = this.IsChecked;
                    break;
                case 119:
                    CJBCheatsMenu.Config.FastWormBin = this.IsChecked;
                    break;
            }

            CJBCheatsMenu.SaveConfig();
        }

        public override void draw(SpriteBatch spriteBatch, int slotX, int slotY)
        {
            spriteBatch.Draw(Game1.mouseCursors, new Vector2((float)(slotX + this.bounds.X), (float)(slotY + this.bounds.Y)), new Rectangle?(this.IsChecked ? OptionsCheckbox.sourceRectChecked : OptionsCheckbox.sourceRectUnchecked), Color.White * (this.greyedOut ? 0.33f : 1f), 0.0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.4f);
            base.draw(spriteBatch, slotX, slotY);
        }
    }
}
