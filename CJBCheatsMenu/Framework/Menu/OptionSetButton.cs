using System;

namespace CJBCheatsMenu.Framework.Menu
{
    /// <summary>
    /// Convenience class for creating an option with a set button.
    /// </summary>
    public class OptionSetButton<T> : Option, IOptionSetButton
    {
        /// <summary>
        /// A value to pass to the press action when the set button is pressed.
        /// </summary>
        private T Value { get; set; }

        /// <summary>
        /// The action that is called when the set button is pressed.
        /// </summary>
        private Action<T> OnPressAction { get; set; }

        /// <summary>
        /// Constructor for an option with a set button.
        /// </summary>
        /// <param name="label">The label displayed for this option.</param>
        /// <param name="value">The value that is passed to the onPressAction when the set button is pressed.</param>
        /// <param name="onPressAction">Action that is called when the set butotn is pressed.</param>
        public OptionSetButton(string label, T value, Action<T> onPressAction) :
            base(label)
        {
            this.Value = value;
            this.OnPressAction = onPressAction;
        }

        /// <summary>
        /// Called when set button is pressed.
        /// </summary>
        public void OnPressed()
        {
            this.OnPressAction(Value);
        }
    }
}
