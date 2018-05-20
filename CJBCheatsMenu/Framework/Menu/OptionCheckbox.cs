using System;

namespace CJBCheatsMenu.Framework.Menu
{
    /// <summary>
    /// Convenience class for creating checkbox options. 
    /// </summary>
    public class OptionCheckbox : OptionWithValue<bool>, IOptionCheckbox
    {
        /// <summary>
        /// Constructor for a checkbox option.
        /// </summary>
        /// <param name="label">The label displayed for this option.</param>
        /// <param name="initialValue">Whether the checkbox is initially checked or not.</param>
        /// <param name="valueChangedCallback">Action that is called whenever the checkbox is checked/unchecked.</param>
        public OptionCheckbox(string label, bool initialValue, Action<bool> valueChangedCallback)
            : base(label, initialValue, valueChangedCallback)
        {
        }
    }
}
