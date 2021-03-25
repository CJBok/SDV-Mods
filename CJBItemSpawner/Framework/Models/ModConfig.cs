using StardewModdingAPI;
using StardewModdingAPI.Utilities;

namespace CJBItemSpawner.Framework.Models
{
    /// <summary>The mod settings.</summary>
    internal class ModConfig
    {
        /// <summary>The key which opens the item spawner menu.</summary>
        public KeybindList ShowMenuKey { get; set; } = new(SButton.I);

        /// <summary>Whether to show items which may cause bugs or crashes when spawned.</summary>
        public bool AllowProblematicItems { get; set; } = false;
    }
}
