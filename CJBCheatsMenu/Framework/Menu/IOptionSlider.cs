namespace CJBCheatsMenu.Framework.Menu
{
    /// <summary>
    /// A slider that allows for the choosing of an integer value.
    /// </summary>
    interface IOptionSlider : IOptionWithValue<int>
    {
        /// <summary>
        /// The minimum value of the slider.
        /// </summary>
        /// <remarks>
        /// Can be negative, but must be less than MaxValue.
        /// </remarks>
        int MinValue { get; }

        /// <summary>
        /// The maximum value of the slider.
        /// </summary>
        /// <remarks>
        /// Can be negative, but must be greater than MinValue.
        /// </remarks>
        int MaxValue { get; }

        /// <summary>
        /// How much the Value increases by with each step of the slider.
        /// </summary>
        int Step { get;  }

        /// <summary>
        /// A function used to convert a given Value to a user readable string.
        /// </summary>
        /// <remarks>
        /// This can be used if the integer value actually represents something else.
        /// </remarks>
        string ConvertValueToString(int value);
    }
}
