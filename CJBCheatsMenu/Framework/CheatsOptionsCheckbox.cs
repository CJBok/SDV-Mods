using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework
{
    internal class CheatsOptionsCheckbox : OptionsElement
    {
        /*********
        ** Properties
        *********/
        /// <summary>The mod settings.</summary>
        private readonly ModConfig Config;

        /// <summary>The method which saves the mod settings.</summary>
        private readonly Action SaveConfig;

        /// <summary>Whether the checkbox is currently checked.</summary>
        private bool IsChecked;


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="label">The checkbox label.</param>
        /// <param name="whichOption">The option ID.</param>
        /// <param name="config">The mod settings.</param>
        /// <param name="saveConfig">The method which saves the mod settings.</param>
        public CheatsOptionsCheckbox(string label, int whichOption, ModConfig config, Action saveConfig)
          : base(label, -1, -1, 9 * Game1.pixelZoom, 9 * Game1.pixelZoom, whichOption)
        {
            this.Config = config;
            this.SaveConfig = saveConfig;

            switch (whichOption)
            {
                case 0:
                    this.IsChecked = this.Config.IncreasedMovement;
                    break;
                case 1:
                    this.IsChecked = this.Config.InfiniteStamina;
                    break;
                case 2:
                    this.IsChecked = this.Config.InfiniteHealth;
                    break;
                case 3:
                    this.IsChecked = this.Config.InfiniteWateringCan;
                    break;
                case 4:
                    this.IsChecked = this.Config.InstantCatch;
                    break;
                case 5:
                    this.IsChecked = this.Config.ThrowBobberMax;
                    break;
                case 6:
                    this.IsChecked = this.Config.OneHitKill;
                    break;
                case 7:
                    this.IsChecked = this.Config.MaxDailyLuck;
                    break;
                case 9:
                    this.IsChecked = this.Config.AlwaysGiveGift;
                    break;
                case 10:
                    this.IsChecked = this.Config.FreezeTimeInside;
                    break;
                case 11:
                    this.IsChecked = this.Config.FreezeTime;
                    break;
                case 12:
                    this.IsChecked = this.Config.FreezeTimeCaves;
                    break;
                case 13:
                    this.IsChecked = this.Config.DurableFences;
                    break;
                case 14:
                    this.IsChecked = this.Config.AlwaysTreasure;
                    break;
                case 15:
                    this.IsChecked = this.Config.OneHitBreak;
                    break;
                case 16:
                    this.IsChecked = this.Config.InstantBite;
                    break;
                case 17:
                    this.IsChecked = this.Config.NoFriendshipDecay;
                    break;
                case 18:
                    this.IsChecked = this.Config.InstantBuild;
                    break;
                case 19:
                    this.IsChecked = this.Config.AutoFeed;
                    break;
                case 20:
                    this.IsChecked = this.Config.InfiniteHay;
                    break;
                case 21:
                    this.IsChecked = this.Config.DurableTackles;
                    break;
                case 22:
                    this.IsChecked = this.Config.HarvestSickle;
                    break;

                case 100:
                    this.IsChecked = this.Config.FastFurnace;
                    break;
                case 101:
                    this.IsChecked = this.Config.FastRecyclingMachine;
                    break;
                case 102:
                    this.IsChecked = this.Config.FastCrystalarium;
                    break;
                case 103:
                    this.IsChecked = this.Config.FastIncubator;
                    break;
                case 104:
                    this.IsChecked = this.Config.FastSlimeIncubator;
                    break;
                case 105:
                    this.IsChecked = this.Config.FastKeg;
                    break;
                case 106:
                    this.IsChecked = this.Config.FastPreservesJar;
                    break;
                case 107:
                    this.IsChecked = this.Config.FastCheesePress;
                    break;
                case 108:
                    this.IsChecked = this.Config.FastMayonnaiseMachine;
                    break;
                case 109:
                    this.IsChecked = this.Config.FastLoom;
                    break;
                case 110:
                    this.IsChecked = this.Config.FastOilMaker;
                    break;
                case 111:
                    this.IsChecked = this.Config.FastSeedMaker;
                    break;
                case 112:
                    this.IsChecked = this.Config.FastCharcoalKiln;
                    break;
                case 113:
                    this.IsChecked = this.Config.FastSlimeEggPress;
                    break;
                case 114:
                    this.IsChecked = this.Config.FastBeeHouse;
                    break;
                case 115:
                    this.IsChecked = this.Config.FastMushroomBox;
                    break;
                case 116:
                    this.IsChecked = this.Config.FastTapper;
                    break;
                case 117:
                    this.IsChecked = this.Config.FastLightningRod;
                    break;
                case 118:
                    this.IsChecked = this.Config.FastCask;
                    break;
                case 119:
                    this.IsChecked = this.Config.FastWormBin;
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
            switch (this.whichOption)
            {
                case 0:
                    this.Config.IncreasedMovement = this.IsChecked;
                    break;
                case 1:
                    this.Config.InfiniteStamina = this.IsChecked;
                    break;
                case 2:
                    this.Config.InfiniteHealth = this.IsChecked;
                    break;
                case 3:
                    this.Config.InfiniteWateringCan = this.IsChecked;
                    break;
                case 4:
                    this.Config.InstantCatch = this.IsChecked;
                    break;
                case 5:
                    this.Config.ThrowBobberMax = this.IsChecked;
                    break;
                case 6:
                    this.Config.OneHitKill = this.IsChecked;
                    break;
                case 7:
                    this.Config.MaxDailyLuck = this.IsChecked;
                    break;
                case 9:
                    this.Config.AlwaysGiveGift = this.IsChecked;
                    break;
                case 10:
                    this.Config.FreezeTimeInside = this.IsChecked;
                    break;
                case 11:
                    this.Config.FreezeTime = this.IsChecked;
                    break;
                case 12:
                    this.Config.FreezeTimeCaves = this.IsChecked;
                    break;
                case 13:
                    this.Config.DurableFences = this.IsChecked;
                    break;
                case 14:
                    this.Config.AlwaysTreasure = this.IsChecked;
                    break;
                case 15:
                    this.Config.OneHitBreak = this.IsChecked;
                    break;
                case 16:
                    this.Config.InstantBite = this.IsChecked;
                    break;
                case 17:
                    this.Config.NoFriendshipDecay = this.IsChecked;
                    break;
                case 18:
                    this.Config.InstantBuild = this.IsChecked;
                    break;
                case 19:
                    this.Config.AutoFeed = this.IsChecked;
                    break;
                case 20:
                    this.Config.InfiniteHay = this.IsChecked;
                    break;
                case 21:
                    this.Config.DurableTackles = this.IsChecked;
                    break;
                case 22:
                    this.Config.HarvestSickle = this.IsChecked;
                    break;

                case 100:
                    this.Config.FastFurnace = this.IsChecked;
                    break;
                case 101:
                    this.Config.FastRecyclingMachine = this.IsChecked;
                    break;
                case 102:
                    this.Config.FastCrystalarium = this.IsChecked;
                    break;
                case 103:
                    this.Config.FastIncubator = this.IsChecked;
                    break;
                case 104:
                    this.Config.FastSlimeIncubator = this.IsChecked;
                    break;
                case 105:
                    this.Config.FastKeg = this.IsChecked;
                    break;
                case 106:
                    this.Config.FastPreservesJar = this.IsChecked;
                    break;
                case 107:
                    this.Config.FastCheesePress = this.IsChecked;
                    break;
                case 108:
                    this.Config.FastMayonnaiseMachine = this.IsChecked;
                    break;
                case 109:
                    this.Config.FastLoom = this.IsChecked;
                    break;
                case 110:
                    this.Config.FastOilMaker = this.IsChecked;
                    break;
                case 111:
                    this.Config.FastSeedMaker = this.IsChecked;
                    break;
                case 112:
                    this.Config.FastCharcoalKiln = this.IsChecked;
                    break;
                case 113:
                    this.Config.FastSlimeEggPress = this.IsChecked;
                    break;
                case 114:
                    this.Config.FastBeeHouse = this.IsChecked;
                    break;
                case 115:
                    this.Config.FastMushroomBox = this.IsChecked;
                    break;
                case 116:
                    this.Config.FastTapper = this.IsChecked;
                    break;
                case 117:
                    this.Config.FastLightningRod = this.IsChecked;
                    break;
                case 118:
                    this.Config.FastCask = this.IsChecked;
                    break;
                case 119:
                    this.Config.FastWormBin = this.IsChecked;
                    break;
            }

            this.SaveConfig();
        }

        public override void draw(SpriteBatch spriteBatch, int slotX, int slotY)
        {
            spriteBatch.Draw(Game1.mouseCursors, new Vector2(slotX + this.bounds.X, slotY + this.bounds.Y), this.IsChecked ? OptionsCheckbox.sourceRectChecked : OptionsCheckbox.sourceRectUnchecked, Color.White * (this.greyedOut ? 0.33f : 1f), 0.0f, Vector2.Zero, Game1.pixelZoom, SpriteEffects.None, 0.4f);
            base.draw(spriteBatch, slotX, slotY);
        }
    }
}
