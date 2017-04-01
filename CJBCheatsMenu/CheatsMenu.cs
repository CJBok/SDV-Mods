using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardewValley.Characters;
using StardewValley.Menus;

namespace CJBCheatsMenu
{
    public class CheatsMenu : IClickableMenu
    {
        /*********
        ** Properties
        *********/
        private string HoverText = "";
        private List<ClickableComponent> OptionSlots = new List<ClickableComponent>();
        private List<OptionsElement> Options = new List<OptionsElement>();
        private int OptionsSlotHeld = -1;
        public const int ItemsPerPage = 10;
        public const int IndexOfGraphicsPage = 6;
        public static int CurrentItemIndex;
        private ClickableTextureComponent UpArrow;
        private ClickableTextureComponent DownArrow;
        private ClickableTextureComponent Scrollbar;
        private bool IsScrolling;
        private Rectangle ScrollbarRunner;
        private bool CanClose = false;
        private List<ClickableComponent> Tabs = new List<ClickableComponent>();
        private static int TabIndex = 0;
        private ClickableComponent Title;

        /*********
        ** Public methods
        *********/
        public CheatsMenu(int x, int y, int width, int height)
          : base(x, y, width, height, false)
        {
            this.Title = new ClickableComponent(new Rectangle(this.xPositionOnScreen + width / 2, this.yPositionOnScreen, Game1.tileSize * 4, Game1.tileSize), "CJB Cheats Menu");

            {
                int i = 0;
                int labelX = (int)(this.xPositionOnScreen - Game1.tileSize * 4.8f);
                int labelY = (int)(this.yPositionOnScreen + Game1.tileSize * 1.5f);
                int labelHeight = (int)(Game1.tileSize * 0.9F);

                this.Tabs.Add(new ClickableComponent(new Rectangle(labelX, labelY + labelHeight * i++, Game1.tileSize * 5, Game1.tileSize), "Player & Tools"));
                this.Tabs.Add(new ClickableComponent(new Rectangle(labelX, labelY + labelHeight * i++, Game1.tileSize * 5, Game1.tileSize), "Farm & Fishing"));
                this.Tabs.Add(new ClickableComponent(new Rectangle(labelX, labelY + labelHeight * i++, Game1.tileSize * 5, Game1.tileSize), "Skills"));
                this.Tabs.Add(new ClickableComponent(new Rectangle(labelX, labelY + labelHeight * i++, Game1.tileSize * 5, Game1.tileSize), "Weather"));
                this.Tabs.Add(new ClickableComponent(new Rectangle(labelX, labelY + labelHeight * i++, Game1.tileSize * 5, Game1.tileSize), "Relationships"));
                this.Tabs.Add(new ClickableComponent(new Rectangle(labelX, labelY + labelHeight * i++, Game1.tileSize * 5, Game1.tileSize), "Warp Locations"));
                this.Tabs.Add(new ClickableComponent(new Rectangle(labelX, labelY + labelHeight * i++, Game1.tileSize * 5, Game1.tileSize), "Time"));
                this.Tabs.Add(new ClickableComponent(new Rectangle(labelX, labelY + labelHeight * i++, Game1.tileSize * 5, Game1.tileSize), "Controls"));
            }

            this.UpArrow = new ClickableTextureComponent("up-arrow", new Rectangle(this.xPositionOnScreen + width + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), "", "", Game1.mouseCursors, new Rectangle(421, 459, 11, 12), (float)Game1.pixelZoom);
            this.DownArrow = new ClickableTextureComponent("down-arrow", new Rectangle(this.xPositionOnScreen + width + Game1.tileSize / 4, this.yPositionOnScreen + height - Game1.tileSize, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), "", "", Game1.mouseCursors, new Rectangle(421, 472, 11, 12), (float)Game1.pixelZoom);
            this.Scrollbar = new ClickableTextureComponent("scrollbar", new Rectangle(this.UpArrow.bounds.X + Game1.pixelZoom * 3, this.UpArrow.bounds.Y + this.UpArrow.bounds.Height + Game1.pixelZoom, 6 * Game1.pixelZoom, 10 * Game1.pixelZoom), "", "", Game1.mouseCursors, new Rectangle(435, 463, 6, 10), (float)Game1.pixelZoom);
            this.ScrollbarRunner = new Rectangle(this.Scrollbar.bounds.X, this.UpArrow.bounds.Y + this.UpArrow.bounds.Height + Game1.pixelZoom, this.Scrollbar.bounds.Width, height - Game1.tileSize * 2 - this.UpArrow.bounds.Height - Game1.pixelZoom * 2);
            for (int i = 0; i < CheatsMenu.ItemsPerPage; i++)
                this.OptionSlots.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize * 5 / 4 + Game1.pixelZoom + i * ((height - Game1.tileSize * 2) / CheatsMenu.ItemsPerPage), width - Game1.tileSize / 2, (height - Game1.tileSize * 2) / CheatsMenu.ItemsPerPage + Game1.pixelZoom), string.Concat((object)i)));

            if (CheatsMenu.TabIndex == 0)
            {
                this.Options.Add(new OptionsElement("Player:"));
                this.Options.Add((OptionsElement)new CheatsOptionsCheckbox("Infinite Stamina", 1, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsCheckbox("Infinite Health", 2, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsCheckbox("Increased Movement Speed", 0, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsSlider("Move Speed", 0, 10, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsCheckbox("One Hit Kill", 6, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsCheckbox("Max Daily Luck", 7, -1, -1));

                this.Options.Add(new OptionsElement("Tools:"));
                this.Options.Add((OptionsElement)new CheatsOptionsCheckbox("Infinite Water in Can", 3, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsCheckbox("One Hit Break", 15, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsCheckbox("Harvest With Sickle", 22, -1, -1));

                this.Options.Add(new OptionsElement("Money:"));
                this.Options.Add((OptionsElement)new CheatsOptionsInputListener("Add 100g", 2, this.OptionSlots[0].bounds.Width, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsInputListener("Add 1000g", 3, this.OptionSlots[0].bounds.Width, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsInputListener("Add 10000g", 4, this.OptionSlots[0].bounds.Width, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsInputListener("Add 100000g", 5, this.OptionSlots[0].bounds.Width, -1, -1));

                this.Options.Add(new OptionsElement("Casino Coins:"));
                this.Options.Add((OptionsElement)new CheatsOptionsInputListener("Add 100", 6, this.OptionSlots[0].bounds.Width, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsInputListener("Add 1000", 7, this.OptionSlots[0].bounds.Width, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsInputListener("Add 10000", 8, this.OptionSlots[0].bounds.Width, -1, -1));
            }

            if (CheatsMenu.TabIndex == 1)
            {
                this.Options.Add(new OptionsElement("Farm:"));
                this.Options.Add((OptionsElement)new CheatsOptionsInputListener("Water All Fields", 9, this.OptionSlots[0].bounds.Width, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsCheckbox("Durable Fences", 13, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsCheckbox("Instant Build", 18, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsCheckbox("Always Auto-Feed", 19, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsCheckbox("Infinite Hay", 20, -1, -1));

                this.Options.Add(new OptionsElement("Fishing:"));
                this.Options.Add((OptionsElement)new CheatsOptionsCheckbox("Instant Catch", 4, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsCheckbox("Instant Bite", 16, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsCheckbox("Always Throw Max Distance", 5, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsCheckbox("Always Treasure", 14, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsCheckbox("Durable Tackles", 21, -1, -1));

                this.Options.Add(new OptionsElement("Fast Machine Processing:"));
                this.Options.Add((OptionsElement)new CheatsOptionsCheckbox("Cask", 118, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsCheckbox("Furnace", 100, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsCheckbox("Recycling Machine", 101, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsCheckbox("Crystalarium", 102, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsCheckbox("Incubator", 103, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsCheckbox("Slime Incubator", 104, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsCheckbox("Keg", 105, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsCheckbox("Preserves Jar", 106, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsCheckbox("Cheese Press", 107, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsCheckbox("Mayonnaise Machine", 108, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsCheckbox("Loom", 109, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsCheckbox("Oil Maker", 110, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsCheckbox("Seed Maker", 111, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsCheckbox("Charcoal Kiln", 112, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsCheckbox("Slime Egg-Press", 113, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsCheckbox("Tapper", 116, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsCheckbox("Lightning Rod", 117, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsCheckbox("Bee House", 114, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsCheckbox("Mushroom Box", 115, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsCheckbox("Worm Bin", 119, -1, -1));
            }

            if (CheatsMenu.TabIndex == 2)
            {
                this.Options.Add(new OptionsElement("Skills:"));
                this.Options.Add((OptionsElement)new CheatsOptionsInputListener("Incr. Farming Lvl", 200, this.OptionSlots[0].bounds.Width, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsInputListener("Incr. Mining Lvl", 201, this.OptionSlots[0].bounds.Width, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsInputListener("Incr. Foraging Lvl", 202, this.OptionSlots[0].bounds.Width, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsInputListener("Incr. Fishing Lvl", 203, this.OptionSlots[0].bounds.Width, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsInputListener("Incr. Combat Lvl", 204, this.OptionSlots[0].bounds.Width, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsInputListener("RESET SKILLS!", 205, this.OptionSlots[0].bounds.Width, -1, -1));
            }

            if (CheatsMenu.TabIndex == 3)
            {
                this.Options.Add(new OptionsElement("Weather Next Day:"));
                this.Options.Add(new CheatsOptionsElement("Current: ", 1));
                this.Options.Add((OptionsElement)new CheatsOptionsInputListener("Sunny", 10, this.OptionSlots[0].bounds.Width, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsInputListener("Raining", 11, this.OptionSlots[0].bounds.Width, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsInputListener("Lightning", 12, this.OptionSlots[0].bounds.Width, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsInputListener("Snowing", 13, this.OptionSlots[0].bounds.Width, -1, -1));
            }

            if (CheatsMenu.TabIndex == 4)
            {
                this.Options.Add(new OptionsElement("Relationships:"));
                this.Options.Add((OptionsElement)new CheatsOptionsCheckbox("Give Gift Anytime", 9, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsCheckbox("No Friendship Decay", 17, -1, -1));
                this.Options.Add(new OptionsElement("Friends:"));

                List<NPC> npcs = Utility.getAllCharacters();

                npcs = (from n in npcs orderby n.name select n).ToList<NPC>();

                foreach (NPC npc in npcs)
                {
                    if ((!npc.name.Equals("Sandy") || Game1.player.mailReceived.Contains("ccVault")) && !npc.name.Equals("???") && !npc.name.Equals("Bouncer") && !npc.name.Equals("Marlon") && !npc.name.Equals("Gil") && !npc.name.Equals("Gunther") && !npc.IsMonster && !(npc is Horse) && !(npc is Pet))
                    {
                        if (Game1.player.friendships.ContainsKey(npc.name))
                        {
                            this.Options.Add(new CheatsOptionsNPCSlider(npc, 9999));
                        }
                    }
                }
            }

            if (CheatsMenu.TabIndex == 5)
            {
                this.Options.Add(new OptionsElement("Warp to:"));
                this.Options.Add((OptionsElement)new CheatsOptionsInputListener("Farm", 100, this.OptionSlots[0].bounds.Width, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsInputListener("Pierre's", 101, this.OptionSlots[0].bounds.Width, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsInputListener("Blacksmith", 102, this.OptionSlots[0].bounds.Width, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsInputListener("Archaeology Office", 103, this.OptionSlots[0].bounds.Width, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsInputListener("Saloon", 104, this.OptionSlots[0].bounds.Width, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsInputListener("Community Center", 105, this.OptionSlots[0].bounds.Width, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsInputListener("Carpenter", 106, this.OptionSlots[0].bounds.Width, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsInputListener("Guild", 107, this.OptionSlots[0].bounds.Width, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsInputListener("Ranch", 113, this.OptionSlots[0].bounds.Width, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsInputListener("Mines", 109, this.OptionSlots[0].bounds.Width, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsInputListener("Willy", 110, this.OptionSlots[0].bounds.Width, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsInputListener("Wizard", 114, this.OptionSlots[0].bounds.Width, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsInputListener("Hats", 115, this.OptionSlots[0].bounds.Width, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsInputListener("Desert", 112, this.OptionSlots[0].bounds.Width, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsInputListener("Sandy", 119, this.OptionSlots[0].bounds.Width, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsInputListener("Casino", 120, this.OptionSlots[0].bounds.Width, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsInputListener("Quarry", 108, this.OptionSlots[0].bounds.Width, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsInputListener("New Beach", 111, this.OptionSlots[0].bounds.Width, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsInputListener("Secret Forest", 116, this.OptionSlots[0].bounds.Width, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsInputListener("Sewer", 117, this.OptionSlots[0].bounds.Width, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsInputListener("Bathhouse", 118, this.OptionSlots[0].bounds.Width, -1, -1));
            }

            if (CheatsMenu.TabIndex == 6)
            {
                this.Options.Add(new OptionsElement("Time:"));
                this.Options.Add((OptionsElement)new CheatsOptionsCheckbox("Freeze Time Inside", 10, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsCheckbox("Freeze Time Caves", 12, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsCheckbox("Freeze Time Everywhere", 11, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsSlider("Time", 10, 20, -1, -1, 100));
            }

            if (CheatsMenu.TabIndex == 7)
            {
                this.Options.Add(new OptionsElement("Controls:"));
                this.Options.Add((OptionsElement)new CheatsOptionsInputListener("Open Menu", 1000, this.OptionSlots[0].bounds.Width, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsInputListener("Freeze Time", 1001, this.OptionSlots[0].bounds.Width, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsInputListener("Grow Tree", 1002, this.OptionSlots[0].bounds.Width, -1, -1));
                this.Options.Add((OptionsElement)new CheatsOptionsInputListener("Grow Crops", 1003, this.OptionSlots[0].bounds.Width, -1, -1));
            }
            this.SetScrollBarToCurrentIndex();
        }

        private void SetScrollBarToCurrentIndex()
        {
            if (Enumerable.Count<OptionsElement>((IEnumerable<OptionsElement>)this.Options) <= 0)
                return;
            this.Scrollbar.bounds.Y = this.ScrollbarRunner.Height / Math.Max(1, this.Options.Count - CheatsMenu.ItemsPerPage + 1) * CheatsMenu.CurrentItemIndex + this.UpArrow.bounds.Bottom + Game1.pixelZoom;
            if (CheatsMenu.CurrentItemIndex != Enumerable.Count<OptionsElement>((IEnumerable<OptionsElement>)this.Options) - CheatsMenu.ItemsPerPage)
                return;
            this.Scrollbar.bounds.Y = this.DownArrow.bounds.Y - this.Scrollbar.bounds.Height - Game1.pixelZoom;
        }

        public override void leftClickHeld(int x, int y)
        {
            if (GameMenu.forcePreventClose)
                return;
            base.leftClickHeld(x, y);
            if (this.IsScrolling)
            {
                int num = this.Scrollbar.bounds.Y;
                this.Scrollbar.bounds.Y = Math.Min(this.yPositionOnScreen + this.height - Game1.tileSize - Game1.pixelZoom * 3 - this.Scrollbar.bounds.Height, Math.Max(y, this.yPositionOnScreen + this.UpArrow.bounds.Height + Game1.pixelZoom * 5));
                CheatsMenu.CurrentItemIndex = Math.Min(this.Options.Count - CheatsMenu.ItemsPerPage, Math.Max(0, (int)((double)this.Options.Count * (double)((float)(y - this.ScrollbarRunner.Y) / (float)this.ScrollbarRunner.Height))));
                this.SetScrollBarToCurrentIndex();
                if (num == this.Scrollbar.bounds.Y)
                    return;
                Game1.soundBank.PlayCue("shiny4");
            }
            else
            {
                if (this.OptionsSlotHeld == -1 || this.OptionsSlotHeld + CheatsMenu.CurrentItemIndex >= this.Options.Count)
                    return;
                this.Options[CheatsMenu.CurrentItemIndex + this.OptionsSlotHeld].leftClickHeld(x - this.OptionSlots[this.OptionsSlotHeld].bounds.X, y - this.OptionSlots[this.OptionsSlotHeld].bounds.Y);
            }
        }

        public override void receiveKeyPress(Keys key)
        {
            if ((Game1.options.menuButton.Contains(new InputButton(key)) || key.ToString().Equals(CJBCheatsMenu.Config.OpenMenuKey)) && this.readyToClose() && this.CanClose)
            {
                Game1.exitActiveMenu();
                Game1.soundBank.PlayCue("bigDeSelect");
                return;
            }

            this.CanClose = true;

            if (this.OptionsSlotHeld == -1 || this.OptionsSlotHeld + CheatsMenu.CurrentItemIndex >= this.Options.Count)
                return;
            this.Options[CheatsMenu.CurrentItemIndex + this.OptionsSlotHeld].receiveKeyPress(key);
        }

        public override void receiveGamePadButton(Buttons key)
        {
            if (key == Buttons.LeftShoulder || key == Buttons.RightShoulder)
            {
                if (key == Buttons.LeftShoulder) CheatsMenu.TabIndex--;
                if (key == Buttons.RightShoulder) CheatsMenu.TabIndex++;
                if (CheatsMenu.TabIndex > 7) CheatsMenu.TabIndex = 0;
                if (CheatsMenu.TabIndex < 0) CheatsMenu.TabIndex = 7;
                Open();
            }
        }

        public override void receiveScrollWheelAction(int direction)
        {
            if (GameMenu.forcePreventClose)
                return;
            base.receiveScrollWheelAction(direction);
            if (direction > 0 && CheatsMenu.CurrentItemIndex > 0)
            {
                this.UpArrowPressed();
            }
            else
            {
                if (direction >= 0 || CheatsMenu.CurrentItemIndex >= Math.Max(0, this.Options.Count() - CheatsMenu.ItemsPerPage))
                    return;
                this.DownArrowPressed();
            }
        }

        public override void releaseLeftClick(int x, int y)
        {
            if (GameMenu.forcePreventClose)
                return;
            base.releaseLeftClick(x, y);
            if (this.OptionsSlotHeld != -1 && this.OptionsSlotHeld + CheatsMenu.CurrentItemIndex < this.Options.Count)
                this.Options[CheatsMenu.CurrentItemIndex + this.OptionsSlotHeld].leftClickReleased(x - this.OptionSlots[this.OptionsSlotHeld].bounds.X, y - this.OptionSlots[this.OptionsSlotHeld].bounds.Y);
            this.OptionsSlotHeld = -1;
            this.IsScrolling = false;
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            if (GameMenu.forcePreventClose)
                return;
            if (this.DownArrow.containsPoint(x, y) && CheatsMenu.CurrentItemIndex < Math.Max(0, Enumerable.Count<OptionsElement>((IEnumerable<OptionsElement>)this.Options) - CheatsMenu.ItemsPerPage))
            {
                this.DownArrowPressed();
                Game1.soundBank.PlayCue("shwip");
            }
            else if (this.UpArrow.containsPoint(x, y) && CheatsMenu.CurrentItemIndex > 0)
            {
                this.UpArrowPressed();
                Game1.soundBank.PlayCue("shwip");
            }
            else if (this.Scrollbar.containsPoint(x, y))
                this.IsScrolling = true;
            else if (!this.DownArrow.containsPoint(x, y) && x > this.xPositionOnScreen + this.width && (x < this.xPositionOnScreen + this.width + Game1.tileSize * 2 && y > this.yPositionOnScreen) && y < this.yPositionOnScreen + this.height)
            {
                this.IsScrolling = true;
                this.leftClickHeld(x, y);
                this.releaseLeftClick(x, y);
            }
            CheatsMenu.CurrentItemIndex = Math.Max(0, Math.Min(Enumerable.Count<OptionsElement>((IEnumerable<OptionsElement>)this.Options) - CheatsMenu.ItemsPerPage, CheatsMenu.CurrentItemIndex));
            for (int index = 0; index < Enumerable.Count<ClickableComponent>((IEnumerable<ClickableComponent>)this.OptionSlots); ++index)
            {
                if (this.OptionSlots[index].bounds.Contains(x, y) && CheatsMenu.CurrentItemIndex + index < Enumerable.Count<OptionsElement>((IEnumerable<OptionsElement>)this.Options) && this.Options[CheatsMenu.CurrentItemIndex + index].bounds.Contains(x - this.OptionSlots[index].bounds.X, y - this.OptionSlots[index].bounds.Y - 5))
                {
                    this.Options[CheatsMenu.CurrentItemIndex + index].receiveLeftClick(x - this.OptionSlots[index].bounds.X, y - this.OptionSlots[index].bounds.Y + 5);
                    this.OptionsSlotHeld = index;
                    break;
                }
            }

            for (int i = 0; i < this.Tabs.Count(); i++)
            {
                ClickableComponent tab = this.Tabs[i];
                int w = (int)Game1.smallFont.MeasureString(tab.name).Y;
                if (tab.bounds.Contains(x, y))
                {
                    CheatsMenu.TabIndex = i;
                    Game1.exitActiveMenu();
                    CheatsMenu.CurrentItemIndex = 0;
                    Open();
                    break;
                }
            }
        }

        public override void receiveRightClick(int x, int y, bool playSound = true)
        {
        }

        public override void performHoverAction(int x, int y)
        {
            if (GameMenu.forcePreventClose)
                return;
            this.HoverText = "";
            this.UpArrow.tryHover(x, y, 0.1f);
            this.DownArrow.tryHover(x, y, 0.1f);
            this.Scrollbar.tryHover(x, y, 0.1f);
            int num = this.IsScrolling ? 1 : 0;
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            if (!Game1.options.showMenuBackground)
            {
                spriteBatch.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.4f);
            }
            Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true, null, false);
            CJB.DrawTextBox(this.Title.bounds.X, this.Title.bounds.Y, Game1.borderFont, this.Title.name, 1, 1.0f);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.PointClamp, (DepthStencilState)null, (RasterizerState)null);
            for (int index = 0; index < Enumerable.Count<ClickableComponent>((IEnumerable<ClickableComponent>)this.OptionSlots); ++index)
            {
                if (CheatsMenu.CurrentItemIndex >= 0 && CheatsMenu.CurrentItemIndex + index < Enumerable.Count<OptionsElement>((IEnumerable<OptionsElement>)this.Options))
                    this.Options[CheatsMenu.CurrentItemIndex + index].draw(spriteBatch, this.OptionSlots[index].bounds.X, this.OptionSlots[index].bounds.Y + 5);
            }
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState)null, (RasterizerState)null);
            if (!GameMenu.forcePreventClose)
            {

                for (int i = 0; i < this.Tabs.Count(); i++)
                {
                    ClickableComponent tab = this.Tabs[i];
                    CJB.DrawTextBox(tab.bounds.X + tab.bounds.Width, tab.bounds.Y, Game1.smallFont, tab.name, 2, CheatsMenu.TabIndex == i ? 1F : 0.7F);
                }

                this.UpArrow.draw(spriteBatch);
                this.DownArrow.draw(spriteBatch);
                if (this.Options.Count() > CheatsMenu.ItemsPerPage)
                {
                    IClickableMenu.drawTextureBox(spriteBatch, Game1.mouseCursors, new Rectangle(403, 383, 6, 6), this.ScrollbarRunner.X, this.ScrollbarRunner.Y, this.ScrollbarRunner.Width, this.ScrollbarRunner.Height, Color.White, (float)Game1.pixelZoom, false);
                    this.Scrollbar.draw(spriteBatch);
                }
            }
            if (!this.HoverText.Equals(""))
                IClickableMenu.drawHoverText(spriteBatch, this.HoverText, Game1.smallFont, 0, 0, -1, (string)null, -1, (string[])null, (Item)null, 0, -1, -1, -1, -1, 1f, (CraftingRecipe)null);

            if (!Game1.options.hardwareCursor)
                spriteBatch.Draw(Game1.mouseCursors, new Vector2((float)Game1.getOldMouseX(), (float)Game1.getOldMouseY()), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, Game1.options.gamepadControls ? 44 : 0, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 1f);
        }

        public static void Open()
        {
            Game1.activeClickableMenu = new CheatsMenu(Game1.viewport.Width / 2 - (600 + IClickableMenu.borderWidth * 2) / 2, Game1.viewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2, 800 + IClickableMenu.borderWidth * 2, 600 + IClickableMenu.borderWidth * 2);
        }


        /*********
        ** Private methods
        *********/
        private void DownArrowPressed()
        {
            this.DownArrow.scale = this.DownArrow.baseScale;
            ++CheatsMenu.CurrentItemIndex;
            this.SetScrollBarToCurrentIndex();
        }

        private void UpArrowPressed()
        {
            this.UpArrow.scale = this.UpArrow.baseScale;
            --CheatsMenu.CurrentItemIndex;
            this.SetScrollBarToCurrentIndex();
        }
    }
}
