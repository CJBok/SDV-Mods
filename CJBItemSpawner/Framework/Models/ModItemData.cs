namespace CJBItemSpawner.Framework.Models
{
    /// <summary>Predefined mod data.</summary>
    internal class ModItemData
    {
        /// <summary>Items which should be hidden by default because they cause in-game bugs or crashes.</summary>
        public ModItemDataEntry[] ProblematicItems { get; set; }
    }
}
