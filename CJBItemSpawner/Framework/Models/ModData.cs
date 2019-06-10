namespace CJBItemSpawner.Framework.Models
{
    /// <summary>Predefined mod data.</summary>
    internal class ModData
    {
        /// <summary>Items which should be hidden by default because they cause in-game bugs or crashes.</summary>
        public ModDataItem[] ProblematicItems { get; set; }
    }
}
