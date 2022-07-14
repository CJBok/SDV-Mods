using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;

namespace CJB.Common.UI
{
    /**


    This code is copied from Pathoschild.Stardew.Common.UI in https://github.com/Pathoschild/StardewMods,
    available under the MIT License. See that repository for the latest version.


    **/
    /// <summary>A button UI component which lets the player trigger a dropdown list.</summary>
    internal class Dropdown<TItem> : ClickableComponent
    {
        /*********
        ** Fields
        *********/
        /// <summary>The font with which to render text.</summary>
        private readonly SpriteFont Font;

        /// <summary>The dropdown list.</summary>
        private readonly DropdownList<TItem> List;

        /// <summary>The size of the rendered button borders.</summary>
        private readonly int BorderWidth = CommonSprites.Tab.TopLeft.Width * 2 * Game1.pixelZoom;

        /// <summary>The maximum width of the form field, if any. This has no effect on the width of the expanded list shown under it.</summary>
        private readonly int? MaxTabWidth;

        /// <summary>The backing field for <see cref="IsExpanded"/>.</summary>
        private bool IsExpandedImpl;

        /// <summary>Whether the menu is being displayed on Android.</summary>
        private bool IsAndroid => Constants.TargetPlatform == GamePlatform.Android;


        /*********
        ** Accessors
        *********/
        /// <summary>Whether the dropdown list is expanded.</summary>
        public bool IsExpanded
        {
            get => this.IsExpandedImpl;
            set
            {
                this.IsExpandedImpl = value;
                this.downNeighborID = value
                    ? this.List.TopComponentId
                    : this.DefaultDownNeighborId;

                this.bounds.Width = (value ? this.List.MaxLabelWidth : this.GetTabWidth()) + this.BorderWidth;
            }
        }

        /// <summary>The selected option.</summary>
        public TItem Selected => this.List.SelectedValue;

        /// <summary>The downward neighbor ID when the dropdown is closed for controller snapping.</summary>
        public int DefaultDownNeighborId { get; set; } = -99999;


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="x">The X-position at which to draw the tab.</param>
        /// <param name="y">The Y-position at which to draw the tab.</param>
        /// <param name="font">The font with which to render text.</param>
        /// <param name="selectedItem">The selected item.</param>
        /// <param name="items">The items in the list.</param>
        /// <param name="getLabel">Get the display label for an item.</param>
        /// <param name="maxTabWidth">The maximum width of the form field, if any. This has no effect on the width of the expanded list shown under it.</param>
        public Dropdown(int x, int y, SpriteFont font, TItem? selectedItem, TItem[] items, Func<TItem, string> getLabel, int? maxTabWidth = null)
            : base(Rectangle.Empty, selectedItem != null ? getLabel(selectedItem) : string.Empty)
        {
            this.Font = font;
            this.List = new DropdownList<TItem>(selectedItem, items, getLabel, x, y, font);
            this.bounds.X = x;
            this.bounds.Y = y;
            this.MaxTabWidth = maxTabWidth;

            this.ReinitializeComponents();
        }

        /// <summary>Get whether the dropdown contains the given UI pixel position.</summary>
        /// <param name="x">The UI X position.</param>
        /// <param name="y">The UI Y position.</param>
        public override bool containsPoint(int x, int y)
        {
            return
                base.containsPoint(x, y)
                || (this.IsExpanded && this.List.containsPoint(x, y));
        }

        /// <summary>Handle a click at the given position, if applicable.</summary>
        /// <param name="x">The X-position that was clicked.</param>
        /// <param name="y">The Y-position that was clicked.</param>
        /// <returns>Returns whether the click was handled.</returns>
        public bool TryClick(int x, int y)
        {
            return this.TryClick(x, y, out _, out _);
        }

        /// <summary>Handle a click at the given position, if applicable.</summary>
        /// <param name="x">The X-position that was clicked.</param>
        /// <param name="y">The Y-position that was clicked.</param>
        /// <param name="itemClicked">Whether a dropdown item was clicked.</param>
        /// <param name="dropdownToggled">Whether the dropdown was expanded or collapsed.</param>
        /// <returns>Returns whether the click was handled.</returns>
        public bool TryClick(int x, int y, out bool itemClicked, out bool dropdownToggled)
        {
            itemClicked = false;
            dropdownToggled = false;

            // click dropdown item
            if (this.IsExpanded && this.List.TryClick(x, y, out itemClicked))
            {
                if (itemClicked)
                {
                    this.IsExpanded = false;
                    dropdownToggled = true;
                }
                return true;
            }

            // toggle expansion
            if (this.bounds.Contains(x, y) || this.IsExpanded)
            {
                this.IsExpanded = !this.IsExpanded;
                dropdownToggled = true;
                return true;
            }

            // not handled
            return false;
        }

        /// <summary>Select an item in the list matching the given value.</summary>
        /// <param name="value">The value to search.</param>
        /// <returns>Returns whether an item was selected.</returns>
        public bool TrySelect(TItem value)
        {
            return this.List.TrySelect(value);
        }

        /// <summary>A method invoked when the player scrolls the dropdown using the mouse wheel.</summary>
        /// <param name="direction">The scroll direction.</param>
        public void ReceiveScrollWheelAction(int direction)
        {
            if (this.IsExpanded)
                this.List.ReceiveScrollWheelAction(direction);
        }

        /// <summary>Render the tab UI.</summary>
        /// <param name="sprites">The sprites to render.</param>
        /// <param name="opacity">The opacity at which to draw.</param>
        public void Draw(SpriteBatch sprites, float opacity = 1)
        {
            // draw tab
            int tabWidth = this.GetTabWidth();
            CommonHelper.DrawTab(this.bounds.X, this.bounds.Y, tabWidth, this.List.MaxLabelHeight, out Vector2 textPos, drawShadow: this.IsAndroid);
            sprites.DrawString(this.Font, this.List.SelectedLabel, textPos, Color.Black * opacity);

            // draw triangle icon
            {
                Vector2 position = new(x: this.bounds.X + tabWidth + this.BorderWidth - 3 * Game1.pixelZoom, y: this.bounds.Y + 2 * Game1.pixelZoom);
                Rectangle sourceRect = new(437, 450, 10, 11);
                sprites.Draw(Game1.mouseCursors, position, sourceRect, Color.White, 0, Vector2.Zero, Game1.pixelZoom, SpriteEffects.None, 1f);
                if (this.IsExpanded)
                    sprites.Draw(Game1.mouseCursors, new Vector2(position.X + 2 * Game1.pixelZoom, position.Y + 3 * Game1.pixelZoom), new Rectangle(sourceRect.X + 2, sourceRect.Y + 3, 5, 6), Color.White, 0, Vector2.Zero, Game1.pixelZoom, SpriteEffects.FlipVertically, 1f);  // right triangle
            }

            // draw dropdown
            if (this.IsExpanded)
                this.List.Draw(sprites, opacity);
        }

        /// <summary>Recalculate dimensions and components for rendering.</summary>
        public void ReinitializeComponents()
        {
            this.bounds.Height = (int)this.Font.MeasureString("ABCDEFGHIJKLMNOPQRSTUVWXYZ").Y - 10 + this.BorderWidth; // adjust for font's broken measurement

            this.bounds.Width = this.GetTabWidth() + this.BorderWidth;

            this.List.bounds.X = this.bounds.X;
            this.List.bounds.Y = this.bounds.Bottom;

            this.List.ReinitializeComponents();
            this.ReinitializeControllerFlow();
        }

        /// <summary>Set the fields to support controller snapping.</summary>
        public void ReinitializeControllerFlow()
        {
            this.List.ReinitializeControllerFlow();
            this.IsExpanded = this.IsExpanded; // force-update down ID
        }

        /// <summary>Get the nested components for controller snapping.</summary>
        public IEnumerable<ClickableComponent> GetChildComponents()
        {
            return this.List.GetChildComponents();
        }

        /// <summary>Get the display width for the tab component.</summary>
        private int GetTabWidth()
        {
            int tabWidth = this.List.MaxLabelWidth;
            if (tabWidth > this.MaxTabWidth)
                tabWidth = this.MaxTabWidth.Value;

            return tabWidth;
        }
    }
}
