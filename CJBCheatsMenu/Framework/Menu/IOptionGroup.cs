using System.Collections.Generic;

namespace CJBCheatsMenu.Framework.Menu
{
    /// <summary>
    /// A group of related options within a menu.
    /// </summary>
    public interface IOptionGroup
    {
        /// <summary>
        /// The title that is displayed above the group of options.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// The options within this group.
        /// </summary>
        List<IOption> Options { get; }
    }
}
