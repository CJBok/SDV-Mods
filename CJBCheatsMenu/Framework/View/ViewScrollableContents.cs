using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace CJBCheatsMenu.Framework.View
{
    /// <summary>
    /// Holds a number of views and allows scrolling through them.
    /// </summary>
    /// <typeparam name="T">The type of view that represents one row in the list of scrollable views.</typeparam>
    internal class ViewScrollableContents<T> : IViewGroup<T>, IViewScrollable where T : IViewGroupItem
    {
        /// <summary>
        /// Represents an invalid row index.
        /// </summary>
        private const int INVALID_INDEX = -1;

        /// <summary>
        /// The list of views that make up the scrollable contents.
        /// </summary>
        public List<T> Children { get; set; }

        /// <summary>
        /// The scrollbar used to control which group of items are currently visible.
        /// </summary>
        public ViewScrollbar Scrollbar { get; private set; }

        /// <summary>
        /// The currently selected row index.
        /// </summary>
        private int currentlySelectedIndex;

        /// <summary>
        /// The currently selected row index.
        /// </summary>
        /// <remarks>
        /// Can only be a valid index, will reset to 0 if it is set to a value outside the valid range of items.
        /// </remarks>
        public int CurrentlySelectedIndex
        {
            get
            {
                return this.currentlySelectedIndex;
            }
            set
            {
                if (value < 0 || value >= this.Children.Count)
                {
                    this.currentlySelectedIndex = 0;
                } else
                {
                    this.currentlySelectedIndex = value;
                }
            }
        }

        /// <summary>
        /// The currently selected row (or null if there are no rows). 
        /// </summary>
        public T CurrentlySelected
        {
            get
            {
                if (this.Children.Count <= 0)
                {
                    return default(T);
                }

                return this.Children[this.currentlySelectedIndex];
            }
        }

        /// <summary>
        /// Select the next row looping back to the first row if the currently selected row is the last row.
        /// </summary>
        public void SelectNext()
        {
            if (this.CurrentlySelectedIndex >= this.Children.Count - 1)
            {
                this.CurrentlySelectedIndex = 0;
            } 
            else
            {
                this.currentlySelectedIndex++;
            }
        }

        /// <summary>
        /// Select the previous row looping to the last row if the currently selected row is the first row.
        /// </summary>
        public void SelectPrevious()
        {
            if (this.CurrentlySelectedIndex <= 0)
            {
                this.CurrentlySelectedIndex = this.Children.Count - 1;
            }
            else
            {
                this.currentlySelectedIndex--;
            }
        }

        /// <summary>
        /// Constructs a view that shows scrollable contents.
        /// </summary>
        /// <param name="scrollbar">The scrollbar that will control which rows are currently visible.</param>
        /// <param name="rows">The rows that make up the contents of the view.</param>
        /// <param name="bounds">The size and position of the scroll area.</param>
        /// <param name="currentlySelected">The currently selected row index.</param>
        public ViewScrollableContents(ViewScrollbar scrollbar, IReadOnlyList<T> rows, Rectangle bounds, int currentlySelected = 0)
        {
            this.Bounds = bounds;
            this.Children = new List<T>(rows);
            this.Scrollbar = scrollbar;
            this.Scrollbar.ItemCount = this.Children.Count;
            this.CurrentlySelectedIndex = currentlySelected;
        }

        /// <summary>
        /// The height of a row.
        /// </summary>
        private int RowHeight => this.Bounds.Height / this.Scrollbar.ItemsPerPage;

        /// <summary>
        /// The size and position of the scroll area.
        /// </summary>
        public Rectangle Bounds { get; set; }

        /// <summary>
        /// Draws the contents of the scroll area.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch used to render assets.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.Children.Count > 0)
            {
                for (int i = this.Scrollbar.CurrentVisibleStartIndex; i <= this.Scrollbar.CurrentVisibleEndIndex; i++)
                {
                    int drawIndex = i - this.Scrollbar.CurrentVisibleStartIndex;
                    Children[i].Draw(spriteBatch, this.Bounds.X, this.Bounds.Y + drawIndex * this.RowHeight);
                }
            }
        }

        /// <summary>
        /// Convert a absolute y position to the distance from the top of the row.
        /// </summary>
        /// <param name="absoluteY">The y position on the screen.</param>
        /// <returns>The distance from the top of the row to the y position.</returns>
        private int GetRelativeYForRow(int absoluteY)
        {
            return (absoluteY - this.Bounds.Y) % this.RowHeight; 
        }

        /// <summary>
        /// Get the row index at the given y position.
        /// </summary>
        /// <param name="y">The y position on the screen.</param>
        /// <returns>The row that is at the given y position.</returns>
        private int GetRowIndexForY(int y)
        {
            int itemIndex = (y - this.Bounds.Y)  / RowHeight + this.Scrollbar.CurrentVisibleStartIndex;
            if (itemIndex < this.Children.Count && itemIndex >= 0)
            {
                return itemIndex;
            }
            else
            {
                return INVALID_INDEX;
            }
        }

        /// <summary>
        /// Handle the left click.
        /// </summary>
        /// <param name="x">x position of mouse when left click occured.</param>
        /// <param name="y">y position of mouse when left click occured.</param>
        public void ReceiveLeftClick(int x, int y)
        {
            int itemIndex = this.GetRowIndexForY(y);
            if (itemIndex != INVALID_INDEX)
            {
                this.Children[itemIndex].ReceiveLeftClick(x - this.Bounds.X, this.GetRelativeYForRow(y));
            }
        }

        /// <summary>
        /// Handle the left click release.
        /// </summary>
        /// <param name="x">x position of mouse when left click released.</param>
        /// <param name="y">y position of mouse when left click released.</param>
        public void LeftClickReleased(int x, int y)
        {
            int itemIndex = this.GetRowIndexForY(y);
            if (itemIndex != INVALID_INDEX)
            {
                this.Children[itemIndex].LeftClickReleased(x - this.Bounds.X, this.GetRelativeYForRow(y));
            }
        }

        /// <summary>
        /// Handle the left held.
        /// </summary>
        /// <param name="x">x position of mouse during left click hold.</param>
        /// <param name="y">y position of mouse during left click hold.</param>
        public void LeftClickHeld(int x, int y)
        {
            int itemIndex = this.GetRowIndexForY(y);
            if (itemIndex != INVALID_INDEX)
            {
                this.Children[itemIndex].LeftClickHeld(x - this.Bounds.X, this.GetRelativeYForRow(y));
            }
        }

        /// <summary>
        /// Handle key press event.
        /// </summary>
        /// <param name="key">The key that was pressed.</param>
        public void ReceiveKeyPress(Keys key)
        {
            for (int i = this.Scrollbar.CurrentVisibleStartIndex; i <= this.Scrollbar.CurrentVisibleEndIndex; i++)
            {
                this.Children[i].ReceiveKeyPress(key);
            }
        }

        /// <summary>
        /// Scrolls the contents in the direction specified.
        /// </summary>
        /// <param name="direction">Which way to scroll the contents.</param>
        public void ReceiveScrollWheelAction(int direction)
        {
            this.Scrollbar.ReceiveScrollWheelAction(direction);
        }
    }
}
