using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardewValley.Menus;
using System;

namespace CJBCheatsMenu.Framework.View
{
    internal class ViewScrollbar : IViewScrollable
    {
        protected ClickableTextureComponent UpArrow { get; private set; }
        protected ClickableTextureComponent DownArrow { get; private set; }
        protected ClickableTextureComponent Scrollbar { get; private set; }
        protected Rectangle ScrollbarRunner { get; private set; }

        public int ItemCount { get; set; }
        public int ItemsPerPage { get; protected set; }

        private int currentVisibleStartIndex;
        private bool IsScrolling { get; set; } = false;

        public ViewScrollbar(int currentIndex, int itemsPerPage, int x, int y, int height, int itemCount = 0)
        {
            int arrowButtonHeight = 12 * Game1.pixelZoom;
            int arrowButtonWidth = 11 * Game1.pixelZoom;
            this.Bounds = new Rectangle(x, y, arrowButtonWidth, height);

            Rectangle upArrowBounds = new Rectangle(x, y, this.Bounds.Width, arrowButtonHeight);
            this.UpArrow = new ClickableTextureComponent("up-arrow", upArrowBounds, "", "", Game1.mouseCursors, new Rectangle(421, 459, 11, 12), Game1.pixelZoom);

            Rectangle downArrowBounds = new Rectangle(x, y + height - arrowButtonHeight, arrowButtonWidth, arrowButtonHeight);
            this.DownArrow = new ClickableTextureComponent("down-arrow", downArrowBounds, "", "", Game1.mouseCursors, new Rectangle(421, 472, 11, 12), Game1.pixelZoom);

            Rectangle scrollbarBounds = new Rectangle(x + Game1.pixelZoom * 3, this.UpArrow.bounds.Bottom + Game1.pixelZoom, 6 * Game1.pixelZoom, 10 * Game1.pixelZoom);
            this.Scrollbar = new ClickableTextureComponent("scrollbar", scrollbarBounds, "", "", Game1.mouseCursors, new Rectangle(435, 463, 6, 10), Game1.pixelZoom);

            this.ScrollbarRunner = new Rectangle(this.Scrollbar.bounds.X, this.Scrollbar.bounds.Y, this.Scrollbar.bounds.Width, height - arrowButtonHeight * 2 - Game1.pixelZoom * 2);
            this.ItemsPerPage = itemsPerPage;
            this.ItemCount = itemCount;
            this.CurrentVisibleStartIndex = currentIndex;
        }

        public int CurrentVisibleStartIndex
        {
            get
            {
                return currentVisibleStartIndex;
            }
            protected set
            {
                currentVisibleStartIndex = Math.Max(0, Math.Min(value, this.LastVisibleStartIndex));
                UpdateScrollbarPosition();
            }
        }

        public int VisibleItemCount
        {
            get
            {
                if (this.currentVisibleStartIndex == this.CurrentVisibleEndIndex)
                {
                    return 0;
                }
                return this.CurrentVisibleEndIndex - this.currentVisibleStartIndex + 1;
            }
        }

        public int CurrentVisibleEndIndex => Math.Min(this.LastVisibileIndex, this.CurrentVisibleStartIndex + this.ItemsPerPage - 1);

        public bool AtBottom()
        {
            return CurrentVisibleEndIndex == LastVisibileIndex;
        }

        public void ScrollUp()
        {
            this.CurrentVisibleStartIndex--;
        }

        public void ScrollDown()
        {
            this.CurrentVisibleStartIndex++;
        }

        public void ScrollTo(int index)
        {
            this.CurrentVisibleStartIndex = index;
        }

        public void ScrollToY(int y)
        {
            this.ScrollTo((int)((y - this.ScrollbarRunner.Y) / (double)this.ScrollbarRunner.Height * (this.LastVisibleStartIndex + 1)));
        }

        private void UpdateScrollbarPosition()
        {
            if (this.ItemCount <= this.ItemsPerPage)
                return;

            this.Scrollbar.bounds.Y = this.ScrollbarRunner.Height / (this.LastVisibleStartIndex + 1) * this.CurrentVisibleStartIndex + this.UpArrow.bounds.Bottom + Game1.pixelZoom;
            if (this.AtBottom())
            {
                this.Scrollbar.bounds.Y = this.ScrollbarRunner.Bottom - this.Scrollbar.bounds.Height;
            }
        }

        public void LeftClickHeld(int x, int y)
        {
            if (this.IsScrolling)
            {
                int oldIndex = this.CurrentVisibleStartIndex;
                this.ScrollToY(y);
                if (oldIndex != this.CurrentVisibleStartIndex)
                {
                    Game1.soundBank.PlayCue("shiny4");
                }
            }
        }

        public void LeftClickReleased(int x, int y)
        {
            this.IsScrolling = false;
        }

        public void ReceiveLeftClick(int x, int y)
        {
            if (this.DownArrow.containsPoint(x, y))
            {
                this.ScrollDown();
                Game1.soundBank.PlayCue("shwip");
            }
            else if (this.UpArrow.containsPoint(x, y))
            {
                this.ScrollUp();
                Game1.soundBank.PlayCue("shwip");
            }
            else
            {
                this.IsScrolling = true;
            };
        }

        public void ReceiveScrollWheelAction(int direction)
        {
            if (direction > 0)
            {
                this.ScrollUp();
            }
            else if (direction < 0)
            {
                this.ScrollDown();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.ItemCount <= this.ItemsPerPage)
            {
                return;
            }

            this.UpArrow.draw(spriteBatch);
            this.DownArrow.draw(spriteBatch);
            IClickableMenu.drawTextureBox(spriteBatch, Game1.mouseCursors, new Rectangle(403, 383, 6, 6), this.ScrollbarRunner.X, this.ScrollbarRunner.Y, this.ScrollbarRunner.Width, this.ScrollbarRunner.Height, Color.White, Game1.pixelZoom, false);
            this.Scrollbar.draw(spriteBatch);
        }

        public void ReceiveKeyPress(Keys key)
        {

        }

        private int LastVisibileIndex => Math.Max(0, this.ItemCount - 1);

        private int LastVisibleStartIndex => Math.Max(0, this.ItemCount - this.ItemsPerPage);

        public Rectangle Bounds { get; set; }
    }
}
