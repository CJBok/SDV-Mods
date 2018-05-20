using System;
using Microsoft.Xna.Framework.Input;

namespace CJBCheatsMenu.Framework.Menu
{
    /// <summary>
    /// Convenience class for creating a key picker option for selecting a keyboard key.
    /// </summary>
    public class OptionKeyPicker : OptionWithValue<Keys>, IOptionKeyPicker
    {
        /// <summary>
        /// Constructor for key picker option.
        /// </summary>
        /// <param name="label">The label displayed for this option.</param>
        /// <param name="initialValue">The initial key that is selected.</param>
        /// <param name="onKeyChangedCallback">Action to call when the selected key has changed.</param>
        public OptionKeyPicker(string label, Keys initialValue, Action<Keys> onKeyChangedCallback) 
            : base(label, initialValue, onKeyChangedCallback)
        {

        }
    }
}
