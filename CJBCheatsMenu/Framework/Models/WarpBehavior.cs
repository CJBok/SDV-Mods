namespace CJBCheatsMenu.Framework.Models
{
    /// <summary>Indicates special behavior that should override the normal warp logic.</summary>
    internal enum WarpBehavior
    {
        /// <summary>No special behavior.</summary>
        Default,

        /// <summary>A special warp to the casino.</summary>
        Casino,

        /// <summary>A special warp to the farm.</summary>
        Farm
    }
}
