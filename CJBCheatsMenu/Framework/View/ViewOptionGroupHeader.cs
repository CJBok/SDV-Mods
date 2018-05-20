namespace CJBCheatsMenu.Framework.View
{
    /// <summary>
    /// Renders a group box header.
    /// </summary>
    internal class ViewOptionGroupHeader : ViewGroupItem
    {
        /// <summary>
        /// Contructor for a group box header view.
        /// </summary>
        /// <param name="optionGroup">The option group to render the title for.</param>
        public ViewOptionGroupHeader(Menu.IOptionGroup optionGroup)
            : base(optionGroup.Title, -1, -1)
        {
            this.OptionGroup = optionGroup;
        }

        /// <summary>
        /// Draw whatever the current option group title is.
        /// </summary>
        public override string DrawnLabel => this.OptionGroup.Title;

        /// <summary>
        /// The OptionGroup that holds the title to render.
        /// </summary>
        private Menu.IOptionGroup OptionGroup { get; set; }
    }
}
