using System;

namespace CJBCheatsMenu.Framework.Menu
{
    /// <summary>
    /// Convenience class for creating a slider option.
    /// </summary>
    public class OptionSlider : OptionWithValue<int>, IOptionSlider
    {
        /// <summary>
        /// The minimum value of the slider.
        /// </summary>
        public int MinValue { get; protected set; }

        /// <summary>
        /// The maximum value of the slider.
        /// </summary>
        public int MaxValue { get; protected set; }

        /// <summary>
        /// How much the Value increases by with each step of the slider.
        /// </summary>
        public int Step { get; protected set; }

        /// <summary>
        /// A function used to convert a given Value to a user readable string.
        /// </summary>
        public virtual string ConvertValueToString(int value)
        {
            return value.ToString();
        }

        /// <summary>
        /// Constructor for creating an option with slider.
        /// </summary>
        /// <param name="label">The label displayed for this option.</param>
        /// <param name="initialValue">The initial value that is selected by the slider.</param>
        /// <param name="valueChangedCallback">Action called whenever the value of the slider changes (when mouse drag released).</param>
        /// <param name="minValue">Minimum value the slider can go to.</param>
        /// <param name="maxValue">Maximum value the slider can go to.</param>
        /// <param name="step">How much the value increases with each slider step.</param>
        public OptionSlider(string label, int initialValue, Action<int> valueChangedCallback, int minValue = 0, int maxValue = 10, int step = 1)
            : base(label, initialValue, valueChangedCallback)
        {
            this.MinValue = minValue;
            this.MaxValue = maxValue;
            this.Step = step;
        }
    }
}
