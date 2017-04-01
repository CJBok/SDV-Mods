using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Monsters;
using StardewValley.Tools;

namespace CJBEndlessInventory
{
    public class Storage : StardewValley.Object
    {
        /*********
        ** Accessors
        *********/
        public const int Capacity = 36;
        public int CurrentLidFrame = 501;
        public int FrameCounter = -1;
        public int Coins;
        public List<Item> Items = new List<Item>();
        private Farmer Opener;
        public string ChestType = "";
        public Color Tint = Color.White;
        public bool IsPlayerChest;
        public bool IsGiftbox;


        /*********
        ** Public methods
        *********/
        public Storage()
        {
            this.name = "Chest";
            this.type = "interactive";
            this.boundingBox = new Rectangle((int)this.tileLocation.X * Game1.tileSize, (int)this.tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
        }

        public Storage(bool playerChest) : base(Vector2.Zero, 130, false)
        {
            this.Name = "Chest";
            this.type = "Crafting";
            if (playerChest)
            {
                this.IsPlayerChest = playerChest;
                this.CurrentLidFrame = 131;
                this.bigCraftable = true;
                this.canBeSetDown = true;
            }
        }

        public Storage(Vector2 location)
        {
            this.tileLocation = location;
            this.name = "Chest";
            this.type = "interactive";
            this.boundingBox = new Rectangle((int)this.tileLocation.X * Game1.tileSize, (int)this.tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
        }

        public Storage(string type, Vector2 location)
        {
            this.tileLocation = location;
            if (type != null)
            {
                if (!(type == "OreChest"))
                {
                    if (!(type == "dungeon"))
                    {
                        if (type == "Grand")
                        {
                            this.Tint = new Color(150, 150, 255);
                            this.Coins = (int)location.Y % 8 + 6;
                        }
                    }
                    else
                    {
                        switch ((int)location.X % 5)
                        {
                            case 1:
                                this.Coins = (int)location.Y % 3 + 2;
                                break;
                            case 2:
                                this.Items.Add(new StardewValley.Object(this.tileLocation, 382, (int)location.Y % 3 + 1));
                                break;
                            case 3:
                                this.Items.Add(new StardewValley.Object(this.tileLocation, (Game1.mine.getMineArea(-1) == 0) ? 378 : ((Game1.mine.getMineArea(-1) == 40) ? 380 : 384), (int)location.Y % 3 + 1));
                                break;
                            case 4:
                                this.ChestType = "Monster";
                                break;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 8; i++)
                    {
                        this.Items.Add(new StardewValley.Object(this.tileLocation, (Game1.random.NextDouble() < 0.5) ? 384 : 382, 1));
                    }
                }
            }
            this.name = "Chest";
            this.type = "interactive";
            this.boundingBox = new Rectangle((int)this.tileLocation.X * Game1.tileSize, (int)this.tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
        }

        public Storage(int coins, List<Item> items, Vector2 location, bool giftBox = false)
        {
            this.name = "Chest";
            this.type = "interactive";
            this.IsGiftbox = giftBox;
            if (items != null)
            {
                this.Items = items;
            }
            this.Coins = coins;
            this.tileLocation = location;
            this.boundingBox = new Rectangle((int)this.tileLocation.X * Game1.tileSize, (int)this.tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
        }

        public override bool performObjectDropInAction(StardewValley.Object dropIn, bool probe, Farmer who)
        {
            return false;
        }

        public override bool performToolAction(Tool t)
        {
            if (this.IsPlayerChest)
            {
                this.ClearNulls();
                if (this.Items.Count<Item>() == 0)
                {
                    return base.performToolAction(t);
                }
                if (t != null && t.isHeavyHitter() && !(t is MeleeWeapon))
                {
                    Game1.playSound("hammer");
                    this.shakeTimer = 100;
                }
            }
            else if (t != null && t is Pickaxe && this.CurrentLidFrame == 503 && this.FrameCounter == -1 && this.Items.Count<Item>() == 0)
            {
                return true;
            }
            return false;
        }

        public void AddContents(int coins, Item item)
        {
            this.Coins += coins;
            this.Items.Add(item);
        }

        public void DumpContents()
        {
            Random random = new Random((int)this.tileLocation.X + (int)this.tileLocation.Y + (int)Game1.uniqueIDForThisGame + ((Game1.mine != null && Game1.currentLocation is MineShaft) ? Game1.mine.mineLevel : 0));
            if (this.Coins <= 0 && this.Items.Count<Item>() <= 0)
            {
                if (this.tileLocation.X % 7f == 0f)
                {
                    this.ChestType = "Monster";
                }
                else
                {
                    this.AddContents(random.Next(4, Math.Max(8, Game1.mine.mineLevel / 10 - 5)), Utility.getUncommonItemForThisMineLevel(Game1.mine.mineLevel, new Point((int)this.tileLocation.X, (int)this.tileLocation.Y)));
                }
            }
            if (this.Items.Count<Item>() > 0 && !this.ChestType.Equals("Monster") && this.Items.Count<Item>() >= 1 && this.Opener.IsMainPlayer)
            {
                if (Game1.currentLocation is FarmHouse)
                {
                    Game1.player.addQuest(6);
                    Game1.screenOverlayTempSprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(128, 208, 16, 16), 200f, 2, 30, new Vector2((float)(Game1.dayTimeMoneyBox.questButton.bounds.Left - Game1.tileSize / 4), (float)(Game1.dayTimeMoneyBox.questButton.bounds.Bottom + Game1.pixelZoom * 2)), false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, true));
                }
                if (this.Items[0] is StardewValley.Object && !(this.Items[0] as StardewValley.Object).bigCraftable && this.Items[0].parentSheetIndex == 434)
                {
                    if (!Game1.player.mailReceived.Contains("CF_Mines"))
                    {
                        Game1.playerEatObject(this.Items[0] as StardewValley.Object, true);
                        Game1.player.mailReceived.Add("CF_Mines");
                    }
                    this.Items.Clear();
                }
                else
                {
                    this.Opener.addItemByMenuIfNecessaryElseHoldUp(this.Items[0], new ItemGrabMenu.behaviorOnItemSelect(this.OnItemTaken));
                }
                if (this.Opener.currentLocation is MineShaft)
                {
                    (this.Opener.currentLocation as MineShaft).updateMineLevelData(1, -1);
                }
            }
            if (this.ChestType.Equals("Monster"))
            {
                Monster monsterForThisLevel = Game1.mine.getMonsterForThisLevel(Game1.mine.mineLevel, (int)this.tileLocation.X, (int)this.tileLocation.Y);
                Vector2 velocityTowardPlayer = Utility.getVelocityTowardPlayer(new Point((int)this.tileLocation.X, (int)this.tileLocation.Y), 8f, this.Opener);
                monsterForThisLevel.xVelocity = velocityTowardPlayer.X;
                monsterForThisLevel.yVelocity = velocityTowardPlayer.Y;
                Game1.currentLocation.characters.Add(monsterForThisLevel);
                Game1.playSound("explosion");
                Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(362, (float)Game1.random.Next(30, 90), 6, 1, new Vector2(this.tileLocation.X * (float)Game1.tileSize, this.tileLocation.Y * (float)Game1.tileSize), false, Game1.random.NextDouble() < 0.5));
                Game1.currentLocation.objects.Remove(this.tileLocation);
                Game1.addHUDMessage(new HUDMessage("Monster in a box!", Color.Red, 3500f));
            }
            else
            {
                this.Opener.gainExperience(5, 25 + ((Game1.mine != null && Game1.currentLocation is MineShaft) ? Game1.mine.mineLevel : 0));
            }
            if (this.IsGiftbox)
            {
                Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(0, 348, 16, 19), 80f, 11, 1, this.tileLocation * (float)Game1.tileSize, false, false, this.tileLocation.Y / 10000f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
                {
                    destroyable = false,
                    holdLastFrame = true
                });
                Game1.currentLocation.removeObject(this.tileLocation, false);
            }
        }

        public void OnItemTaken(Item item, Farmer who)
        {
            if (item != null && this.Items.Count<Item>() > 0 && item.Equals(this.Items[0]))
            {
                this.Items.RemoveAt(0);
            }
        }

        public override bool checkForAction(Farmer who, bool justCheckingForActivity = false)
        {
            if (justCheckingForActivity)
            {
                return true;
            }
            if (this.IsGiftbox)
            {
                this.Opener = who;
                Game1.player.Halt();
                Game1.player.freezePause = 1000;
                Game1.playSound("Ship");
                this.DumpContents();
            }
            else if (this.IsPlayerChest && this.FrameCounter == -1)
            {
                this.Opener = who;
                this.FrameCounter = 5;
                Game1.playSound("openChest");
                Game1.player.Halt();
                Game1.player.freezePause = 1000;
            }
            else if (!this.IsPlayerChest)
            {
                if (this.CurrentLidFrame == 501 && this.FrameCounter <= -1)
                {
                    this.Opener = who;
                    this.FrameCounter = 5;
                    Game1.playSound("openChest");
                }
                else if (this.CurrentLidFrame == 503 && this.Items.Count<Item>() > 0)
                {
                    who.addItemByMenuIfNecessaryElseHoldUp(this.Items[0], new ItemGrabMenu.behaviorOnItemSelect(this.OnItemTaken));
                    if (this.Items.Count<Item>() > 0 && this.Items[0] != null && this.Items[0] is StardewValley.Object)
                    {
                        int arg_14B_0 = (this.Items[0] as StardewValley.Object).ParentSheetIndex;
                    }
                }
            }
            if (this.Items.Count<Item>() == 0 && this.Coins == 0 && !this.IsPlayerChest)
            {
                who.currentLocation.removeObject(this.tileLocation, false);
                Game1.playSound("woodWhack");
            }
            return true;
        }

        public void GrabItemFromChest(Item item, Farmer who)
        {
            if (who.couldInventoryAcceptThisItem(item))
            {
                this.Items.Remove(item);
                this.ClearNulls();
                Game1.activeClickableMenu = new ItemMenu(this.Items, new ItemMenu.BehaviorOnItemSelect(this.GrabItemFromInventory), new ItemMenu.BehaviorOnItemSelect(this.GrabItemFromChest));
            }
        }

        public Item AddItem(Item item)
        {
            for (int i = 0; i < this.Items.Count<Item>(); i++)
            {
                if (this.Items[i] != null && this.Items[i].canStackWith(item))
                {
                    item.Stack = this.Items[i].addToStack(item.Stack);
                    if (item.Stack <= 0)
                    {
                        return null;
                    }
                }
            }
            if (this.Items.Count<Item>() < 99999)
            {
                this.Items.Add(item);
                return null;
            }
            return item;
        }

        public void GrabItemFromInventory(Item item, Farmer who)
        {
            if (item.Stack == 0)
            {
                item.Stack = 1;
            }
            Item item2 = this.AddItem(item);
            if (item2 == null)
            {
                who.removeItemFromInventory(item);
            }
            else
            {
                who.addItemToInventory(item2);
            }
            this.ClearNulls();
            Game1.activeClickableMenu = new ItemMenu(this.Items, new ItemMenu.BehaviorOnItemSelect(this.GrabItemFromInventory), new ItemMenu.BehaviorOnItemSelect(this.GrabItemFromChest));
        }

        public void ClearNulls()
        {
            for (int i = this.Items.Count<Item>() - 1; i >= 0; i--)
            {
                if (this.Items[i] == null)
                {
                    this.Items.RemoveAt(i);
                }
            }
        }

        public override void updateWhenCurrentLocation(GameTime time)
        {
            if (this.shakeTimer > 0)
            {
                this.shakeTimer -= time.ElapsedGameTime.Milliseconds;
                if (this.shakeTimer <= 0)
                {
                    this.health = 10;
                }
            }
            if (this.IsPlayerChest)
            {
                if (this.FrameCounter > -1 && this.CurrentLidFrame < 136)
                {
                    this.FrameCounter--;
                    if (this.FrameCounter <= 0)
                    {
                        if (this.Opener != null)
                        {
                            if (this.CurrentLidFrame == 135)
                            {
                                Game1.activeClickableMenu = new ItemGrabMenu(this.Items, false, true, new InventoryMenu.highlightThisItem(InventoryMenu.highlightAllItems), new ItemGrabMenu.behaviorOnItemSelect(this.GrabItemFromInventory), null, new ItemGrabMenu.behaviorOnItemSelect(this.GrabItemFromChest), false, true, true, true, true, 1);
                                this.FrameCounter = -1;
                                return;
                            }
                            this.FrameCounter = 5;
                            this.CurrentLidFrame++;
                            return;
                        }
                        else
                        {
                            this.FrameCounter = 5;
                            this.CurrentLidFrame--;
                            if (this.CurrentLidFrame == 131)
                            {
                                this.FrameCounter = -1;
                                Game1.playSound("woodyStep");
                                return;
                            }
                        }
                    }
                }
                else if (this.FrameCounter == -1 && this.CurrentLidFrame > 131 && Game1.activeClickableMenu == null && this.Opener != null)
                {
                    this.Opener = null;
                    this.CurrentLidFrame = 135;
                    this.FrameCounter = 2;
                    Game1.playSound("doorCreakReverse");
                    return;
                }
            }
            else if (this.FrameCounter > -1 && this.CurrentLidFrame < 504)
            {
                this.FrameCounter--;
                if (this.FrameCounter <= 0)
                {
                    if (this.CurrentLidFrame == 503)
                    {
                        this.DumpContents();
                        this.FrameCounter = -1;
                        return;
                    }
                    this.FrameCounter = 10;
                    this.CurrentLidFrame++;
                    if (this.CurrentLidFrame == 503)
                    {
                        this.FrameCounter += 5;
                    }
                }
            }
        }

        public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber)
        {
            base.drawInMenu(spriteBatch, location, scaleSize, transparency, layerDepth, drawStackNumber);
        }

        public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
        {
            if (this.IsPlayerChest)
            {
                spriteBatch.Draw(Game1.bigCraftableSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize + ((this.shakeTimer > 0) ? Game1.random.Next(-1, 2) : 0)), (float)((y - 1) * Game1.tileSize))), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.bigCraftableSpriteSheet, 130, 16, 32)), this.Tint, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(y * Game1.tileSize + 4) / 10000f);
                spriteBatch.Draw(Game1.bigCraftableSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize + ((this.shakeTimer > 0) ? Game1.random.Next(-1, 2) : 0)), (float)((y - 1) * Game1.tileSize))), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.bigCraftableSpriteSheet, this.CurrentLidFrame, 16, 32)), this.Tint, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(y * Game1.tileSize + 5) / 10000f);
                return;
            }
            if (this.IsGiftbox)
            {
                spriteBatch.Draw(Game1.shadowTexture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2 - Game1.tileSize / 4), (float)(Game1.tileSize * 5 / 6)), new Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 5f, SpriteEffects.None, 1E-07f);
                if (this.Items.Count<Item>() > 0 || this.Coins > 0)
                {
                    spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize + ((this.shakeTimer > 0) ? Game1.random.Next(-1, 2) : 0)), (float)(y * Game1.tileSize))), new Rectangle?(new Rectangle(0, 348, 16, 19)), this.Tint, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(y * Game1.tileSize + 4) / 10000f);
                    return;
                }
            }
            else
            {
                spriteBatch.Draw(Game1.shadowTexture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2 - Game1.tileSize / 4), (float)(Game1.tileSize * 5 / 6)), new Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 5f, SpriteEffects.None, 1E-07f);
                spriteBatch.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), (float)(y * Game1.tileSize))), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 500, 16, 16)), this.Tint, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(y * Game1.tileSize + 4) / 10000f);
                Vector2 globalPosition = new Vector2((float)(x * Game1.tileSize), (float)(y * Game1.tileSize));
                switch (this.CurrentLidFrame)
                {
                    case 501:
                        globalPosition.Y -= (float)(Game1.tileSize / 2);
                        break;
                    case 502:
                        globalPosition.Y -= 40f;
                        break;
                    case 503:
                        globalPosition.Y -= 60f;
                        break;
                }
                spriteBatch.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, globalPosition), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.CurrentLidFrame, 16, 16)), this.Tint, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(y * Game1.tileSize + 5) / 10000f);
            }
        }
    }
}
