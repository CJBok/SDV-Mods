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
        private string hoverText = "";
        private List<ClickableComponent> optionSlots = new List<ClickableComponent>();
        private List<OptionsElement> options = new List<OptionsElement>();
        private int optionsSlotHeld = -1;
        public const int itemsPerPage = 10;
        public const int indexOfGraphicsPage = 6;
        public static int currentItemIndex;
        private ClickableTextureComponent upArrow;
        private ClickableTextureComponent downArrow;
        private ClickableTextureComponent scrollBar;
        private bool scrolling;
        private Rectangle scrollBarRunner;
        private bool canClose = false;
        private List<ClickableComponent> tabs = new List<ClickableComponent>();
        private static int tab = 0;
        private ClickableComponent title;

        /*********
        ** Public methods
        *********/
        public CheatsMenu(int x, int y, int width, int height)
          : base(x, y, width, height, false)
        {
            title = new ClickableComponent(new Rectangle(this.xPositionOnScreen + width / 2, this.yPositionOnScreen, Game1.tileSize * 4, Game1.tileSize), "CJB Cheats Menu");

            int i = 0;
            int lblx = (int)(this.xPositionOnScreen - Game1.tileSize * 4.8f);
            int lbly = (int)(this.yPositionOnScreen + Game1.tileSize * 1.5f);
            int lblHeight = (int)(Game1.tileSize * 0.9F);

            this.tabs.Add(new ClickableComponent(new Rectangle(lblx, lbly + lblHeight * i++, Game1.tileSize * 5, Game1.tileSize), "Player & Tools"));
            this.tabs.Add(new ClickableComponent(new Rectangle(lblx, lbly + lblHeight * i++, Game1.tileSize * 5, Game1.tileSize), "Farm & Fishing"));
            this.tabs.Add(new ClickableComponent(new Rectangle(lblx, lbly + lblHeight * i++, Game1.tileSize * 5, Game1.tileSize), "Skills"));
            this.tabs.Add(new ClickableComponent(new Rectangle(lblx, lbly + lblHeight * i++, Game1.tileSize * 5, Game1.tileSize), "Weather"));
            this.tabs.Add(new ClickableComponent(new Rectangle(lblx, lbly + lblHeight * i++, Game1.tileSize * 5, Game1.tileSize), "Relationships"));
            this.tabs.Add(new ClickableComponent(new Rectangle(lblx, lbly + lblHeight * i++, Game1.tileSize * 5, Game1.tileSize), "Warp Locations"));
            this.tabs.Add(new ClickableComponent(new Rectangle(lblx, lbly + lblHeight * i++, Game1.tileSize * 5, Game1.tileSize), "Time"));
            this.tabs.Add(new ClickableComponent(new Rectangle(lblx, lbly + lblHeight * i++, Game1.tileSize * 5, Game1.tileSize), "Controls"));

            this.upArrow = new ClickableTextureComponent("up-arrow", new Rectangle(this.xPositionOnScreen + width + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), "", "", Game1.mouseCursors, new Rectangle(421, 459, 11, 12), (float)Game1.pixelZoom);
            this.downArrow = new ClickableTextureComponent("down-arrow", new Rectangle(this.xPositionOnScreen + width + Game1.tileSize / 4, this.yPositionOnScreen + height - Game1.tileSize, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), "", "", Game1.mouseCursors, new Rectangle(421, 472, 11, 12), (float)Game1.pixelZoom);
            this.scrollBar = new ClickableTextureComponent("scrollbar", new Rectangle(this.upArrow.bounds.X + Game1.pixelZoom * 3, this.upArrow.bounds.Y + this.upArrow.bounds.Height + Game1.pixelZoom, 6 * Game1.pixelZoom, 10 * Game1.pixelZoom), "", "", Game1.mouseCursors, new Rectangle(435, 463, 6, 10), (float)Game1.pixelZoom);
            this.scrollBarRunner = new Rectangle(this.scrollBar.bounds.X, this.upArrow.bounds.Y + this.upArrow.bounds.Height + Game1.pixelZoom, this.scrollBar.bounds.Width, height - Game1.tileSize * 2 - this.upArrow.bounds.Height - Game1.pixelZoom * 2);
            for (int index = 0; index < itemsPerPage; ++index)
                this.optionSlots.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize * 5 / 4 + Game1.pixelZoom + index * ((height - Game1.tileSize * 2) / itemsPerPage), width - Game1.tileSize / 2, (height - Game1.tileSize * 2) / itemsPerPage + Game1.pixelZoom), string.Concat((object)index)));

            if (tab == 0)
            {
                this.options.Add(new OptionsElement("Player:"));
                this.options.Add((OptionsElement)new CheatsOptionsCheckbox("Infinite Stamina", 1, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsCheckbox("Infinite Health", 2, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsCheckbox("Increased Movement Speed", 0, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsSlider("Move Speed", 0, 10, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsCheckbox("One Hit Kill", 6, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsCheckbox("Max Daily Luck", 7, -1, -1));

                this.options.Add(new OptionsElement("Tools:"));
                this.options.Add((OptionsElement)new CheatsOptionsCheckbox("Infinite Water in Can", 3, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsCheckbox("One Hit Break", 15, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsCheckbox("Harvest With Sickle", 22, -1, -1));

                this.options.Add(new OptionsElement("Money:"));
                this.options.Add((OptionsElement)new CheatsOptionsInputListener("Add 100g", 2, this.optionSlots[0].bounds.Width, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsInputListener("Add 1000g", 3, this.optionSlots[0].bounds.Width, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsInputListener("Add 10000g", 4, this.optionSlots[0].bounds.Width, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsInputListener("Add 100000g", 5, this.optionSlots[0].bounds.Width, -1, -1));

                this.options.Add(new OptionsElement("Casino Coins:"));
                this.options.Add((OptionsElement)new CheatsOptionsInputListener("Add 100", 6, this.optionSlots[0].bounds.Width, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsInputListener("Add 1000", 7, this.optionSlots[0].bounds.Width, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsInputListener("Add 10000", 8, this.optionSlots[0].bounds.Width, -1, -1));
            }

            if (tab == 1)
            {
                this.options.Add(new OptionsElement("Farm:"));
                this.options.Add((OptionsElement)new CheatsOptionsInputListener("Water All Fields", 9, this.optionSlots[0].bounds.Width, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsCheckbox("Durable Fences", 13, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsCheckbox("Instant Build", 18, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsCheckbox("Always Auto-Feed", 19, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsCheckbox("Infinite Hay", 20, -1, -1));

                this.options.Add(new OptionsElement("Fishing:"));
                this.options.Add((OptionsElement)new CheatsOptionsCheckbox("Instant Catch", 4, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsCheckbox("Instant Bite", 16, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsCheckbox("Always Throw Max Distance", 5, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsCheckbox("Always Treasure", 14, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsCheckbox("Durable Tackles", 21, -1, -1));

                this.options.Add(new OptionsElement("Fast Machine Processing:"));
                this.options.Add((OptionsElement)new CheatsOptionsCheckbox("Cask", 118, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsCheckbox("Furnace", 100, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsCheckbox("Recycling Machine", 101, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsCheckbox("Crystalarium", 102, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsCheckbox("Incubator", 103, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsCheckbox("Slime Incubator", 104, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsCheckbox("Keg", 105, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsCheckbox("Preserves Jar", 106, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsCheckbox("Cheese Press", 107, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsCheckbox("Mayonnaise Machine", 108, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsCheckbox("Loom", 109, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsCheckbox("Oil Maker", 110, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsCheckbox("Seed Maker", 111, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsCheckbox("Charcoal Kiln", 112, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsCheckbox("Slime Egg-Press", 113, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsCheckbox("Tapper", 116, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsCheckbox("Lightning Rod", 117, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsCheckbox("Bee House", 114, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsCheckbox("Mushroom Box", 115, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsCheckbox("Worm Bin", 119, -1, -1));
            }

            if (tab == 2)
            {
                this.options.Add(new OptionsElement("Skills:"));
                this.options.Add((OptionsElement)new CheatsOptionsInputListener("Incr. Farming Lvl", 200, this.optionSlots[0].bounds.Width, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsInputListener("Incr. Mining Lvl", 201, this.optionSlots[0].bounds.Width, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsInputListener("Incr. Foraging Lvl", 202, this.optionSlots[0].bounds.Width, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsInputListener("Incr. Fishing Lvl", 203, this.optionSlots[0].bounds.Width, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsInputListener("Incr. Combat Lvl", 204, this.optionSlots[0].bounds.Width, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsInputListener("RESET SKILLS!", 205, this.optionSlots[0].bounds.Width, -1, -1));
            }

            if (tab == 3)
            {
                this.options.Add(new OptionsElement("Weather Next Day:"));
                this.options.Add(new CheatsOptionsElement("Current: ", 1));
                this.options.Add((OptionsElement)new CheatsOptionsInputListener("Sunny", 10, this.optionSlots[0].bounds.Width, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsInputListener("Raining", 11, this.optionSlots[0].bounds.Width, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsInputListener("Lightning", 12, this.optionSlots[0].bounds.Width, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsInputListener("Snowing", 13, this.optionSlots[0].bounds.Width, -1, -1));
            }

            if (tab == 4)
            {
                this.options.Add(new OptionsElement("Relationships:"));
                this.options.Add((OptionsElement)new CheatsOptionsCheckbox("Give Gift Anytime", 9, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsCheckbox("No Friendship Decay", 17, -1, -1));
                this.options.Add(new OptionsElement("Friends:"));

                List<NPC> npcs = Utility.getAllCharacters();

                npcs = (from n in npcs orderby n.name select n).ToList<NPC>();

                foreach (NPC npc in npcs)
                {
                    if ((!npc.name.Equals("Sandy") || Game1.player.mailReceived.Contains("ccVault")) && !npc.name.Equals("???") && !npc.name.Equals("Bouncer") && !npc.name.Equals("Marlon") && !npc.name.Equals("Gil") && !npc.name.Equals("Gunther") && !npc.IsMonster && !(npc is Horse) && !(npc is Pet))
                    {
                        if (Game1.player.friendships.ContainsKey(npc.name))
                        {
                            this.options.Add(new CheatsOptionsNPCSlider(npc, 9999));
                        }
                    }
                }
            }

            if (tab == 5)
            {
                this.options.Add(new OptionsElement("Warp to:"));
                this.options.Add((OptionsElement)new CheatsOptionsInputListener("Farm", 100, this.optionSlots[0].bounds.Width, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsInputListener("Pierre's", 101, this.optionSlots[0].bounds.Width, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsInputListener("Blacksmith", 102, this.optionSlots[0].bounds.Width, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsInputListener("Archaeology Office", 103, this.optionSlots[0].bounds.Width, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsInputListener("Saloon", 104, this.optionSlots[0].bounds.Width, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsInputListener("Community Center", 105, this.optionSlots[0].bounds.Width, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsInputListener("Carpenter", 106, this.optionSlots[0].bounds.Width, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsInputListener("Guild", 107, this.optionSlots[0].bounds.Width, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsInputListener("Ranch", 113, this.optionSlots[0].bounds.Width, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsInputListener("Mines", 109, this.optionSlots[0].bounds.Width, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsInputListener("Willy", 110, this.optionSlots[0].bounds.Width, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsInputListener("Wizard", 114, this.optionSlots[0].bounds.Width, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsInputListener("Hats", 115, this.optionSlots[0].bounds.Width, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsInputListener("Desert", 112, this.optionSlots[0].bounds.Width, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsInputListener("Sandy", 119, this.optionSlots[0].bounds.Width, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsInputListener("Casino", 120, this.optionSlots[0].bounds.Width, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsInputListener("Quarry", 108, this.optionSlots[0].bounds.Width, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsInputListener("New Beach", 111, this.optionSlots[0].bounds.Width, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsInputListener("Secret Forest", 116, this.optionSlots[0].bounds.Width, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsInputListener("Sewer", 117, this.optionSlots[0].bounds.Width, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsInputListener("Bathhouse", 118, this.optionSlots[0].bounds.Width, -1, -1));
            }

            if (tab == 6)
            {
                this.options.Add(new OptionsElement("Time:"));
                this.options.Add((OptionsElement)new CheatsOptionsCheckbox("Freeze Time Inside", 10, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsCheckbox("Freeze Time Caves", 12, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsCheckbox("Freeze Time Everywhere", 11, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsSlider("Time", 10, 20, -1, -1, 100));
            }

            if (tab == 7)
            {
                this.options.Add(new OptionsElement("Controls:"));
                this.options.Add((OptionsElement)new CheatsOptionsInputListener("Open Menu", 1000, this.optionSlots[0].bounds.Width, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsInputListener("Freeze Time", 1001, this.optionSlots[0].bounds.Width, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsInputListener("Grow Tree", 1002, this.optionSlots[0].bounds.Width, -1, -1));
                this.options.Add((OptionsElement)new CheatsOptionsInputListener("Grow Crops", 1003, this.optionSlots[0].bounds.Width, -1, -1));
            }
            this.setScrollBarToCurrentIndex();
        }

        private void setScrollBarToCurrentIndex()
        {
            if (Enumerable.Count<OptionsElement>((IEnumerable<OptionsElement>)this.options) <= 0)
                return;
            this.scrollBar.bounds.Y = this.scrollBarRunner.Height / Math.Max(1, this.options.Count - itemsPerPage + 1) * currentItemIndex + this.upArrow.bounds.Bottom + Game1.pixelZoom;
            if (currentItemIndex != Enumerable.Count<OptionsElement>((IEnumerable<OptionsElement>)this.options) - itemsPerPage)
                return;
            this.scrollBar.bounds.Y = this.downArrow.bounds.Y - this.scrollBar.bounds.Height - Game1.pixelZoom;
        }

        public override void leftClickHeld(int x, int y)
        {
            if (GameMenu.forcePreventClose)
                return;
            base.leftClickHeld(x, y);
            if (this.scrolling)
            {
                int num = this.scrollBar.bounds.Y;
                this.scrollBar.bounds.Y = Math.Min(this.yPositionOnScreen + this.height - Game1.tileSize - Game1.pixelZoom * 3 - this.scrollBar.bounds.Height, Math.Max(y, this.yPositionOnScreen + this.upArrow.bounds.Height + Game1.pixelZoom * 5));
                currentItemIndex = Math.Min(this.options.Count - itemsPerPage, Math.Max(0, (int)((double)this.options.Count * (double)((float)(y - this.scrollBarRunner.Y) / (float)this.scrollBarRunner.Height))));
                this.setScrollBarToCurrentIndex();
                if (num == this.scrollBar.bounds.Y)
                    return;
                Game1.soundBank.PlayCue("shiny4");
            }
            else
            {
                if (this.optionsSlotHeld == -1 || this.optionsSlotHeld + currentItemIndex >= this.options.Count)
                    return;
                this.options[currentItemIndex + this.optionsSlotHeld].leftClickHeld(x - this.optionSlots[this.optionsSlotHeld].bounds.X, y - this.optionSlots[this.optionsSlotHeld].bounds.Y);
            }
        }

        public override void receiveKeyPress(Keys key)
        {
            if ((Game1.options.menuButton.Contains(new InputButton(key)) || key.ToString().Equals(CJBCheatsMenu.config.openMenuKey)) && this.readyToClose() && canClose)
            {
                Game1.exitActiveMenu();
                Game1.soundBank.PlayCue("bigDeSelect");
                return;
            }

            canClose = true;

            if (this.optionsSlotHeld == -1 || this.optionsSlotHeld + currentItemIndex >= this.options.Count)
                return;
            this.options[currentItemIndex + this.optionsSlotHeld].receiveKeyPress(key);
        }

        public override void receiveGamePadButton(Buttons b)
        {
            if (b == Buttons.LeftShoulder || b == Buttons.RightShoulder)
            {
                if (b == Buttons.LeftShoulder) tab--;
                if (b == Buttons.RightShoulder) tab++;
                if (tab > 7) tab = 0;
                if (tab < 0) tab = 7;
                Open();
            }
        }

        public override void receiveScrollWheelAction(int direction)
        {
            if (GameMenu.forcePreventClose)
                return;
            base.receiveScrollWheelAction(direction);
            if (direction > 0 && currentItemIndex > 0)
            {
                this.upArrowPressed();
            }
            else
            {
                if (direction >= 0 || currentItemIndex >= Math.Max(0, this.options.Count() - itemsPerPage))
                    return;
                this.downArrowPressed();
            }
        }

        public override void releaseLeftClick(int x, int y)
        {
            if (GameMenu.forcePreventClose)
                return;
            base.releaseLeftClick(x, y);
            if (this.optionsSlotHeld != -1 && this.optionsSlotHeld + currentItemIndex < this.options.Count)
                this.options[currentItemIndex + this.optionsSlotHeld].leftClickReleased(x - this.optionSlots[this.optionsSlotHeld].bounds.X, y - this.optionSlots[this.optionsSlotHeld].bounds.Y);
            this.optionsSlotHeld = -1;
            this.scrolling = false;
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            if (GameMenu.forcePreventClose)
                return;
            if (this.downArrow.containsPoint(x, y) && currentItemIndex < Math.Max(0, Enumerable.Count<OptionsElement>((IEnumerable<OptionsElement>)this.options) - itemsPerPage))
            {
                this.downArrowPressed();
                Game1.soundBank.PlayCue("shwip");
            }
            else if (this.upArrow.containsPoint(x, y) && currentItemIndex > 0)
            {
                this.upArrowPressed();
                Game1.soundBank.PlayCue("shwip");
            }
            else if (this.scrollBar.containsPoint(x, y))
                this.scrolling = true;
            else if (!this.downArrow.containsPoint(x, y) && x > this.xPositionOnScreen + this.width && (x < this.xPositionOnScreen + this.width + Game1.tileSize * 2 && y > this.yPositionOnScreen) && y < this.yPositionOnScreen + this.height)
            {
                this.scrolling = true;
                this.leftClickHeld(x, y);
                this.releaseLeftClick(x, y);
            }
            currentItemIndex = Math.Max(0, Math.Min(Enumerable.Count<OptionsElement>((IEnumerable<OptionsElement>)this.options) - itemsPerPage, currentItemIndex));
            for (int index = 0; index < Enumerable.Count<ClickableComponent>((IEnumerable<ClickableComponent>)this.optionSlots); ++index)
            {
                if (this.optionSlots[index].bounds.Contains(x, y) && currentItemIndex + index < Enumerable.Count<OptionsElement>((IEnumerable<OptionsElement>)this.options) && this.options[currentItemIndex + index].bounds.Contains(x - this.optionSlots[index].bounds.X, y - this.optionSlots[index].bounds.Y - 5))
                {
                    this.options[currentItemIndex + index].receiveLeftClick(x - this.optionSlots[index].bounds.X, y - this.optionSlots[index].bounds.Y + 5);
                    this.optionsSlotHeld = index;
                    break;
                }
            }

            for (int i = 0; i < tabs.Count(); i++)
            {
                ClickableComponent t = this.tabs[i];
                int w = (int)Game1.smallFont.MeasureString(t.name).Y;
                if (t.bounds.Contains(x, y))
                {
                    tab = i;
                    Game1.exitActiveMenu();
                    CheatsMenu.currentItemIndex = 0;
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
            this.hoverText = "";
            this.upArrow.tryHover(x, y, 0.1f);
            this.downArrow.tryHover(x, y, 0.1f);
            this.scrollBar.tryHover(x, y, 0.1f);
            int num = this.scrolling ? 1 : 0;
        }

        public override void draw(SpriteBatch b)
        {
            if (!Game1.options.showMenuBackground)
            {
                b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.4f);
            }
            Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true, null, false);
            CJB.drawTextBox(title.bounds.X, title.bounds.Y, Game1.borderFont, title.name, 1, 1.0f);
            b.End();
            b.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.PointClamp, (DepthStencilState)null, (RasterizerState)null);
            for (int index = 0; index < Enumerable.Count<ClickableComponent>((IEnumerable<ClickableComponent>)this.optionSlots); ++index)
            {
                if (currentItemIndex >= 0 && currentItemIndex + index < Enumerable.Count<OptionsElement>((IEnumerable<OptionsElement>)this.options))
                    this.options[currentItemIndex + index].draw(b, this.optionSlots[index].bounds.X, this.optionSlots[index].bounds.Y + 5);
            }
            b.End();
            b.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState)null, (RasterizerState)null);
            if (!GameMenu.forcePreventClose)
            {

                for (int i = 0; i < tabs.Count(); i++)
                {
                    ClickableComponent current = tabs[i];
                    CJB.drawTextBox(current.bounds.X + current.bounds.Width, current.bounds.Y, Game1.smallFont, current.name, 2, tab == i ? 1F : 0.7F);
                }

                this.upArrow.draw(b);
                this.downArrow.draw(b);
                if (this.options.Count() > itemsPerPage)
                {
                    IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(403, 383, 6, 6), this.scrollBarRunner.X, this.scrollBarRunner.Y, this.scrollBarRunner.Width, this.scrollBarRunner.Height, Color.White, (float)Game1.pixelZoom, false);
                    this.scrollBar.draw(b);
                }
            }
            if (!this.hoverText.Equals(""))
                IClickableMenu.drawHoverText(b, this.hoverText, Game1.smallFont, 0, 0, -1, (string)null, -1, (string[])null, (Item)null, 0, -1, -1, -1, -1, 1f, (CraftingRecipe)null);

            if (!Game1.options.hardwareCursor)
                b.Draw(Game1.mouseCursors, new Vector2((float)Game1.getOldMouseX(), (float)Game1.getOldMouseY()), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, Game1.options.gamepadControls ? 44 : 0, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 1f);
        }

        public static void Open()
        {
            Game1.activeClickableMenu = new CheatsMenu(Game1.viewport.Width / 2 - (600 + IClickableMenu.borderWidth * 2) / 2, Game1.viewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2, 800 + IClickableMenu.borderWidth * 2, 600 + IClickableMenu.borderWidth * 2);
        }


        /*********
        ** Private methods
        *********/
        private void downArrowPressed()
        {
            this.downArrow.scale = this.downArrow.baseScale;
            ++currentItemIndex;
            this.setScrollBarToCurrentIndex();
        }

        private void upArrowPressed()
        {
            this.upArrow.scale = this.upArrow.baseScale;
            --currentItemIndex;
            this.setScrollBarToCurrentIndex();
        }
    }
}
