using System;

namespace CJBCheatsMenu.Framework.Menu
{
    /// <summary>
    /// Convenience class for creating a heart picker slider for an NPC
    /// </summary>
    public class OptionHeartPicker : OptionWithValue<int>, IOptionHeartPicker
    {
        /// <summary>
        /// The NPC character to display next to the heart slider.
        /// </summary>
        public StardewValley.NPC NPC { get; private set; }

        /// <summary>
        /// Constructor for heart picker option.
        /// </summary>
        /// <param name="npc">The NPC character to display next to the heart slider.</param>
        /// <param name="initialHeartValue">The heart value that is initially set on this option.</param>
        /// <param name="valueChangedCallback">Action that is called when the heart value has changed.</param>
        public OptionHeartPicker(StardewValley.NPC npc, int initialHeartValue, Action<int> valueChangedCallback)
            : base("", initialHeartValue, valueChangedCallback)
        {
            this.NPC = npc;
        }
    }
}
