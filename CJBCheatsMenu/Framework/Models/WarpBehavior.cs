namespace CJBCheatsMenu.Framework.Models
{
    /// <summary>Indicates special behavior that should override the normal warp logic.</summary>
    internal enum WarpBehavior
    {
        /// <summary>No special behavior.</summary>
        Default,

        /// <summary>Apply special logic for the casino warp.</summary>
        Casino,

        /// <summary>Apply special logic for the farm warp.</summary>
        Farm
    }
}
