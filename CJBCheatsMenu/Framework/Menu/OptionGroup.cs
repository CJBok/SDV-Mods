using System.Collections.Generic;

namespace CJBCheatsMenu.Framework.Menu
{
    /// <summary>
    /// Convenience class for creating a group of related options.
    /// </summary>
    public class OptionGroup : IOptionGroup
    {
        /// <summary>
        /// Constructor for creating an option group
        /// </summary>
        /// <param name="title">The title that is displayed above the options.</param>
        public OptionGroup(string title)
        {
            this.Title = title;
        }

        /// <summary>
        /// The title that is displayed above the options.
        /// </summary>
        public virtual string Title { get; private set; }

        /// <summary>
        /// The options within this group.
        /// </summary>
        public virtual List<IOption> Options { get; private set; } = new List<IOption>();
    }
}
