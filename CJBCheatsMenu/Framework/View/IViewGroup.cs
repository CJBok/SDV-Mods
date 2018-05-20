using System.Collections.Generic;

namespace CJBCheatsMenu.Framework.View
{
    /// <summary>
    /// A view that holds a group of views to be rendered to the screen.
    /// </summary>
    /// <typeparam name="T">A view that represents one view in this view group.</typeparam>
    internal interface IViewGroup<T> : IView where T : IViewGroupItem
    {
        /// <summary>
        /// The views that this view group holds.
        /// </summary>
        List<T> Children { get; }
    }
}
