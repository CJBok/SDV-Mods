namespace CJBCheatsMenu.Framework.Menu
{
    /// <summary>
    /// A option that acts as a slider which displays the NPC and can pick a heart value.
    /// </summary>
    public interface IOptionHeartPicker : IOptionWithValue<int>
    {
        /// <summary>
        /// The NPC character to display next to the heart slider.
        /// </summary>
        StardewValley.NPC NPC { get; }
    }
}
