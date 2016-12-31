using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Monsters;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CJBEndlessInventory {
    public class Storage : StardewValley.Object {
        public const int capacity = 36;

        public int currentLidFrame = 501;

        public int frameCounter = -1;

        public int coins;

        public List<Item> items = new List<Item>();

        private Farmer opener;

        public string chestType = "";

        public Color tint = Color.White;

        public bool playerChest;

        public bool giftbox;

        public Storage() {
            this.name = "Chest";
            this.type = "interactive";
            this.boundingBox = new Rectangle((int)this.tileLocation.X * Game1.tileSize, (int)this.tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
        }

        public Storage(bool playerChest) : base(Vector2.Zero, 130, false) {
            this.Name = "Chest";
            this.type = "Crafting";
            if (playerChest) {
                this.playerChest = playerChest;
                this.currentLidFrame = 131;
                this.bigCraftable = true;
                this.canBeSetDown = true;
            }
        }

        public Storage(Vector2 location) {
            this.tileLocation = location;
            this.name = "Chest";
            this.type = "interactive";
            this.boundingBox = new Rectangle((int)this.tileLocation.X * Game1.tileSize, (int)this.tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
        }

        public Storage(string type, Vector2 location) {
            this.tileLocation = location;
            if (type != null) {
                if (!(type == "OreChest")) {
                    if (!(type == "dungeon")) {
                        if (type == "Grand") {
                            this.tint = new Color(150, 150, 255);
                            this.coins = (int)location.Y % 8 + 6;
                        }
                    } else {
                        switch ((int)location.X % 5) {
                            case 1:
                                this.coins = (int)location.Y % 3 + 2;
                                break;
                            case 2:
                                this.items.Add(new StardewValley.Object(this.tileLocation, 382, (int)location.Y % 3 + 1));
                                break;
                            case 3:
                                this.items.Add(new StardewValley.Object(this.tileLocation, (Game1.mine.getMineArea(-1) == 0) ? 378 : ((Game1.mine.getMineArea(-1) == 40) ? 380 : 384), (int)location.Y % 3 + 1));
                                break;
                            case 4:
                                this.chestType = "Monster";
                                break;
                        }
                    }
                } else {
                    for (int i = 0; i < 8; i++) {
                        this.items.Add(new StardewValley.Object(this.tileLocation, (Game1.random.NextDouble() < 0.5) ? 384 : 382, 1));
                    }
                }
            }
            this.name = "Chest";
            this.type = "interactive";
            this.boundingBox = new Rectangle((int)this.tileLocation.X * Game1.tileSize, (int)this.tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
        }

        public Storage(int coins, List<Item> items, Vector2 location, bool giftBox = false) {
            this.name = "Chest";
            this.type = "interactive";
            this.giftbox = giftBox;
            if (items != null) {
                this.items = items;
            }
            this.coins = coins;
            this.tileLocation = location;
            this.boundingBox = new Rectangle((int)this.tileLocation.X * Game1.tileSize, (int)this.tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
        }

        public override bool performObjectDropInAction(StardewValley.Object dropIn, bool probe, Farmer who) {
            return false;
        }

        public override bool performToolAction(Tool t) {
            if (this.playerChest) {
                this.clearNulls();
                if (this.items.Count<Item>() == 0) {
                    return base.performToolAction(t);
                }
                if (t != null && t.isHeavyHitter() && !(t is MeleeWeapon)) {
                    Game1.playSound("hammer");
                    this.shakeTimer = 100;
                }
            } else if (t != null && t is Pickaxe && this.currentLidFrame == 503 && this.frameCounter == -1 && this.items.Count<Item>() == 0) {
                return true;
            }
            return false;
        }

        public void addContents(int coins, Item item) {
            this.coins += coins;
            this.items.Add(item);
        }

        public void dumpContents() {
            Random random = new Random((int)this.tileLocation.X + (int)this.tileLocation.Y + (int)Game1.uniqueIDForThisGame + ((Game1.mine != null && Game1.currentLocation is MineShaft) ? Game1.mine.mineLevel : 0));
            if (this.coins <= 0 && this.items.Count<Item>() <= 0) {
                if (this.tileLocation.X % 7f == 0f) {
                    this.chestType = "Monster";
                } else {
                    this.addContents(random.Next(4, Math.Max(8, Game1.mine.mineLevel / 10 - 5)), Utility.getUncommonItemForThisMineLevel(Game1.mine.mineLevel, new Point((int)this.tileLocation.X, (int)this.tileLocation.Y)));
                }
            }
            if (this.items.Count<Item>() > 0 && !this.chestType.Equals("Monster") && this.items.Count<Item>() >= 1 && this.opener.IsMainPlayer) {
                if (Game1.currentLocation is FarmHouse) {
                    Game1.player.addQuest(6);
                    Game1.screenOverlayTempSprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(128, 208, 16, 16), 200f, 2, 30, new Vector2((float)(Game1.dayTimeMoneyBox.questButton.bounds.Left - Game1.tileSize / 4), (float)(Game1.dayTimeMoneyBox.questButton.bounds.Bottom + Game1.pixelZoom * 2)), false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, true));
                }
                if (this.items[0] is StardewValley.Object && !(this.items[0] as StardewValley.Object).bigCraftable && this.items[0].parentSheetIndex == 434) {
                    if (!Game1.player.mailReceived.Contains("CF_Mines")) {
                        Game1.playerEatObject(this.items[0] as StardewValley.Object, true);
                        Game1.player.mailReceived.Add("CF_Mines");
                    }
                    this.items.Clear();
                } else {
                    this.opener.addItemByMenuIfNecessaryElseHoldUp(this.items[0], new ItemGrabMenu.behaviorOnItemSelect(this.itemTakenCallback));
                }
                if (this.opener.currentLocation is MineShaft) {
                    (this.opener.currentLocation as MineShaft).updateMineLevelData(1, -1);
                }
            }
            if (this.chestType.Equals("Monster")) {
                Monster monsterForThisLevel = Game1.mine.getMonsterForThisLevel(Game1.mine.mineLevel, (int)this.tileLocation.X, (int)this.tileLocation.Y);
                Vector2 velocityTowardPlayer = Utility.getVelocityTowardPlayer(new Point((int)this.tileLocation.X, (int)this.tileLocation.Y), 8f, this.opener);
                monsterForThisLevel.xVelocity = velocityTowardPlayer.X;
                monsterForThisLevel.yVelocity = velocityTowardPlayer.Y;
                Game1.currentLocation.characters.Add(monsterForThisLevel);
                Game1.playSound("explosion");
                Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(362, (float)Game1.random.Next(30, 90), 6, 1, new Vector2(this.tileLocation.X * (float)Game1.tileSize, this.tileLocation.Y * (float)Game1.tileSize), false, Game1.random.NextDouble() < 0.5));
                Game1.currentLocation.objects.Remove(this.tileLocation);
                Game1.addHUDMessage(new HUDMessage("Monster in a box!", Color.Red, 3500f));
            } else {
                this.opener.gainExperience(5, 25 + ((Game1.mine != null && Game1.currentLocation is MineShaft) ? Game1.mine.mineLevel : 0));
            }
            if (this.giftbox) {
                Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(0, 348, 16, 19), 80f, 11, 1, this.tileLocation * (float)Game1.tileSize, false, false, this.tileLocation.Y / 10000f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false) {
                    destroyable = false,
                    holdLastFrame = true
                });
                Game1.currentLocation.removeObject(this.tileLocation, false);
            }
        }

        public void itemTakenCallback(Item item, Farmer who) {
            if (item != null && this.items.Count<Item>() > 0 && item.Equals(this.items[0])) {
                this.items.RemoveAt(0);
            }
        }

        public override bool checkForAction(Farmer who, bool justCheckingForActivity = false) {
            if (justCheckingForActivity) {
                return true;
            }
            if (this.giftbox) {
                this.opener = who;
                Game1.player.Halt();
                Game1.player.freezePause = 1000;
                Game1.playSound("Ship");
                this.dumpContents();
            } else if (this.playerChest && this.frameCounter == -1) {
                this.opener = who;
                this.frameCounter = 5;
                Game1.playSound("openChest");
                Game1.player.Halt();
                Game1.player.freezePause = 1000;
            } else if (!this.playerChest) {
                if (this.currentLidFrame == 501 && this.frameCounter <= -1) {
                    this.opener = who;
                    this.frameCounter = 5;
                    Game1.playSound("openChest");
                } else if (this.currentLidFrame == 503 && this.items.Count<Item>() > 0) {
                    who.addItemByMenuIfNecessaryElseHoldUp(this.items[0], new ItemGrabMenu.behaviorOnItemSelect(this.itemTakenCallback));
                    if (this.items.Count<Item>() > 0 && this.items[0] != null && this.items[0] is StardewValley.Object) {
                        int arg_14B_0 = (this.items[0] as StardewValley.Object).ParentSheetIndex;
                    }
                }
            }
            if (this.items.Count<Item>() == 0 && this.coins == 0 && !this.playerChest) {
                who.currentLocation.removeObject(this.tileLocation, false);
                Game1.playSound("woodWhack");
            }
            return true;
        }

        public void grabItemFromChest(Item item, Farmer who) {
            if (who.couldInventoryAcceptThisItem(item)) {
                this.items.Remove(item);
                this.clearNulls();
                Game1.activeClickableMenu = new ItemMenu(this.items, new ItemMenu.behaviorOnItemSelect(this.grabItemFromInventory), new ItemMenu.behaviorOnItemSelect(this.grabItemFromChest));
            }
        }

        public Item addItem(Item item) {
            for (int i = 0; i < this.items.Count<Item>(); i++) {
                if (this.items[i] != null && this.items[i].canStackWith(item)) {
                    item.Stack = this.items[i].addToStack(item.Stack);
                    if (item.Stack <= 0) {
                        return null;
                    }
                }
            }
            if (this.items.Count<Item>() < 99999) {
                this.items.Add(item);
                return null;
            }
            return item;
        }

        public void grabItemFromInventory(Item item, Farmer who) {
            if (item.Stack == 0) {
                item.Stack = 1;
            }
            Item item2 = this.addItem(item);
            if (item2 == null) {
                who.removeItemFromInventory(item);
            } else {
                who.addItemToInventory(item2);
            }
            this.clearNulls();
            Game1.activeClickableMenu = new ItemMenu(this.items, new ItemMenu.behaviorOnItemSelect(this.grabItemFromInventory), new ItemMenu.behaviorOnItemSelect(this.grabItemFromChest));
        }

        public void clearNulls() {
            for (int i = this.items.Count<Item>() - 1; i >= 0; i--) {
                if (this.items[i] == null) {
                    this.items.RemoveAt(i);
                }
            }
        }

        public override void updateWhenCurrentLocation(GameTime time) {
            if (this.shakeTimer > 0) {
                this.shakeTimer -= time.ElapsedGameTime.Milliseconds;
                if (this.shakeTimer <= 0) {
                    this.health = 10;
                }
            }
            if (this.playerChest) {
                if (this.frameCounter > -1 && this.currentLidFrame < 136) {
                    this.frameCounter--;
                    if (this.frameCounter <= 0) {
                        if (this.opener != null) {
                            if (this.currentLidFrame == 135) {
                                Game1.activeClickableMenu = new ItemGrabMenu(this.items, false, true, new InventoryMenu.highlightThisItem(InventoryMenu.highlightAllItems), new ItemGrabMenu.behaviorOnItemSelect(this.grabItemFromInventory), null, new ItemGrabMenu.behaviorOnItemSelect(this.grabItemFromChest), false, true, true, true, true, 1);
                                this.frameCounter = -1;
                                return;
                            }
                            this.frameCounter = 5;
                            this.currentLidFrame++;
                            return;
                        } else {
                            this.frameCounter = 5;
                            this.currentLidFrame--;
                            if (this.currentLidFrame == 131) {
                                this.frameCounter = -1;
                                Game1.playSound("woodyStep");
                                return;
                            }
                        }
                    }
                } else if (this.frameCounter == -1 && this.currentLidFrame > 131 && Game1.activeClickableMenu == null && this.opener != null) {
                    this.opener = null;
                    this.currentLidFrame = 135;
                    this.frameCounter = 2;
                    Game1.playSound("doorCreakReverse");
                    return;
                }
            } else if (this.frameCounter > -1 && this.currentLidFrame < 504) {
                this.frameCounter--;
                if (this.frameCounter <= 0) {
                    if (this.currentLidFrame == 503) {
                        this.dumpContents();
                        this.frameCounter = -1;
                        return;
                    }
                    this.frameCounter = 10;
                    this.currentLidFrame++;
                    if (this.currentLidFrame == 503) {
                        this.frameCounter += 5;
                    }
                }
            }
        }

        public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber) {
            base.drawInMenu(spriteBatch, location, scaleSize, transparency, layerDepth, drawStackNumber);
        }

        public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1f) {
            if (this.playerChest) {
                spriteBatch.Draw(Game1.bigCraftableSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize + ((this.shakeTimer > 0) ? Game1.random.Next(-1, 2) : 0)), (float)((y - 1) * Game1.tileSize))), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.bigCraftableSpriteSheet, 130, 16, 32)), this.tint, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(y * Game1.tileSize + 4) / 10000f);
                spriteBatch.Draw(Game1.bigCraftableSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize + ((this.shakeTimer > 0) ? Game1.random.Next(-1, 2) : 0)), (float)((y - 1) * Game1.tileSize))), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.bigCraftableSpriteSheet, this.currentLidFrame, 16, 32)), this.tint, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(y * Game1.tileSize + 5) / 10000f);
                return;
            }
            if (this.giftbox) {
                spriteBatch.Draw(Game1.shadowTexture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2 - Game1.tileSize / 4), (float)(Game1.tileSize * 5 / 6)), new Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 5f, SpriteEffects.None, 1E-07f);
                if (this.items.Count<Item>() > 0 || this.coins > 0) {
                    spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize + ((this.shakeTimer > 0) ? Game1.random.Next(-1, 2) : 0)), (float)(y * Game1.tileSize))), new Rectangle?(new Rectangle(0, 348, 16, 19)), this.tint, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(y * Game1.tileSize + 4) / 10000f);
                    return;
                }
            } else {
                spriteBatch.Draw(Game1.shadowTexture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2 - Game1.tileSize / 4), (float)(Game1.tileSize * 5 / 6)), new Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 5f, SpriteEffects.None, 1E-07f);
                spriteBatch.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), (float)(y * Game1.tileSize))), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 500, 16, 16)), this.tint, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(y * Game1.tileSize + 4) / 10000f);
                Vector2 globalPosition = new Vector2((float)(x * Game1.tileSize), (float)(y * Game1.tileSize));
                switch (this.currentLidFrame) {
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
                spriteBatch.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, globalPosition), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.currentLidFrame, 16, 16)), this.tint, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(y * Game1.tileSize + 5) / 10000f);
            }
        }
    }
}
