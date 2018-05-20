namespace CJBCheatsMenu.Framework.Menu
{
    /// <summary>
    /// A single option within an IOptionGroup.
    /// </summary>
    /// <remarks>
    /// Implementing this interface will create an option that is only a label that is not actionable.
    /// </remarks>
    public interface IOption
    {
        /// <summary>
        /// The label for this option.
        /// </summary>
        string Label { get; }

        /// <summary>
        /// true if this option is disabled, false otherwise.
        /// </summary>
        bool Disabled { get; }
    }
}
