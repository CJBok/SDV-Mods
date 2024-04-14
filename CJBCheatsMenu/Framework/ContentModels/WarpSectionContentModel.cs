namespace CJBCheatsMenu.Framework.ContentModels
{
    /// <summary>The data for a section which groups warps in the UI.</summary>
    internal class WarpSectionContentModel
    {
        /// <summary>A unique string ID for this warp section.</summary>
        public string Id { get; set; } = "";

        /// <summary>The translated display name to show in the UI.</summary>
        public string DisplayName { get; set; } = "";

        /// <summary>The relative order in which to list it in the warp menu (default 0).</summary>
        public int Order { get; set; }
    }
}
