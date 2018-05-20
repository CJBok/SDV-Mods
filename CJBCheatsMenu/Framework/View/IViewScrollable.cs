namespace CJBCheatsMenu.Framework.View
{
    /// <summary>
    /// A view that can be scrolled.
    /// </summary>
    internal interface IViewScrollable : IView
    {
        /// <summary>
        /// Handles the scroll wheel action.
        /// </summary>
        /// <param name="direction">The direction of the scroll wheel action.</param>
        void ReceiveScrollWheelAction(int direction);
    }
}
