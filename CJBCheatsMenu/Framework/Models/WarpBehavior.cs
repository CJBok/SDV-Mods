namespace CJBCheatsMenu.Framework.Models
{
    /// <summary>Indicates special behavior that should override the normal warp logic.</summary>
    internal enum WarpBehavior
    {
        /// <summary>No special behavior.</summary>
        Default,

        /// <summary>Apply special logic for the casino warp.</summary>
        Casino,

        /// <summary>Apply special logic for the community center warp.</summary>
        CommunityCenter,

        /// <summary>Apply special logic for the farm warp.</summary>
        Farm,

        /// <summary>Apply special logic for the JojaMart warp.</summary>
        JojaMart,

        /// <summary>Apply special logic for the movie theater warp when built through the community path.</summary>
        MovieTheaterCommunity,

        /// <summary>Apply special logic for the movie theater warp when built through the Joja path.</summary>
        MovieTheaterJoja
    }
}
