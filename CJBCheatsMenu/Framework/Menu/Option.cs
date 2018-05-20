namespace CJBCheatsMenu.Framework.Menu
{
    /// <summary>
    /// Convenience class for creating options. 
    /// </summary>
    /// <remarks>
    /// This option will not be actionable, will only create a label.
    /// </remarks>
    public class Option : IOption
    {
        /// <summary>
        /// The label displayed for this option.
        /// </summary>
        public virtual string Label { get; protected set; }

        /// <summary>
        /// true if this option is disabled, false otherwise.
        /// </summary>
        public virtual bool Disabled { get; protected set; }

        /// <summary>
        /// Contructor for a Option
        /// </summary>
        /// <param name="label">The label displayed for this option.</param>
        /// <param name="disabled">Whether or not this option is disabled.</param>
        public Option(string label, bool disabled = false)
        {
            this.Label = label;
            this.Disabled = disabled;
        }
    }
}
