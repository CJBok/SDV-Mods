using System;
using System.Collections.Generic;

namespace CJBCheatsMenu.Framework.Menu
{
    /// <summary>
    /// Base class for convenience classes which have an underlying value.
    /// </summary>
    /// <typeparam name="T">The currently selected value of the option.</typeparam>
    public class OptionWithValue<T> : Option, IOptionWithValue<T> where T : IComparable
    {
        /// <summary>
        /// Called whenever the selected value has changed.
        /// </summary>
        protected Action<T> ValueChangedCallback { get; set; }

        /// <summary>
        /// The currently selected value. 
        /// </summary>
        protected T value;

        /// <summary>
        /// Constructor for an option that has a value.
        /// </summary>
        /// <param name="label">The label displayed for this option.</param>
        /// <param name="initialValue">The initial value selected for this option.</param>
        /// <param name="valueChangedCallback">Action called when the selected value changes.</param>
        public OptionWithValue(string label, T initialValue, Action<T> valueChangedCallback)
            : base(label)
        {
            this.value = initialValue;
            this.ValueChangedCallback = valueChangedCallback;
        }

        /// <summary>
        /// The currently selected value. 
        /// </summary>
        public T Value
        {
            get
            {
                return this.value;
            }
            set
            {
                if (!EqualityComparer<T>.Default.Equals(this.value, value))
                {
                    this.value = value;
                    ValueChangedCallback(value);
                }
            }
        }
    }
}
