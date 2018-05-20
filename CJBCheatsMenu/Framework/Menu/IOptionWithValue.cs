namespace CJBCheatsMenu.Framework.Menu
{
    /// <summary>
    /// A helper base class for options with a Value.
    /// </summary>
    /// <remarks>
    /// You probably don't want to implement this interface directly, it will act like a normal IOption.
    /// </remarks>
    public interface IOptionWithValue<T> : IOption
    {
        /// <summary>
        /// The current value of this option.
        /// </summary>
        /// <remarks>
        /// Its setter will get called when the value is changed by the user, and if the value returned
        /// from the getter changes, the view will automatically update.
        /// </remarks>
        T Value { get; set; }
    }
}
