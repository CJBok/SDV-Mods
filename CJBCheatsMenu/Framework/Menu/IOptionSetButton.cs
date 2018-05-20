namespace CJBCheatsMenu.Framework.Menu
{
    /// <summary>
    /// A option with a set button.
    /// </summary>
    public interface IOptionSetButton : IOption
    {
        /// <summary>
        /// Called when the set button is pressed.
        /// </summary>
        void OnPressed();
    }
}
