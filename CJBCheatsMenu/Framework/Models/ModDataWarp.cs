using System;
using CJBCheatsMenu.Framework.ContentModels;
using Microsoft.Xna.Framework;

namespace CJBCheatsMenu.Framework.Models;

/// <summary>The model for a configurable warp.</summary>
/// <param name="Id">The warp ID, or <c>null</c> to get it from the <see cref="DisplayText"/>.</param>
/// <param name="DisplayText">The display text to show in the menu. This can be a translation ID, or the raw text to display.</param>
/// <param name="Location">The warp's target location name.</param>
/// <param name="Tile">The warp's target tile coordinate.</param>
/// <param name="Order">The relative order in which to list it in the warp menu (default 0).</param>
internal record ModDataWarp(string? Id, string DisplayText, string? Location, Vector2 Tile, int Order)
{
    /// <summary>A game state query which indicates whether the warp should be visible, or <c>null</c> if it should always be visible.</summary>
    public string? Condition { get; set; }

    /// <summary>The special behavior to apply.</summary>
    [Obsolete("This field is only kept for backwards compatibility with players using legacy warp files. Newer code should use " + nameof(Condition) + " instead.")]
    public WarpBehavior SpecialBehavior
    {
        set
        {
            switch (value)
            {
                case WarpBehavior.Casino:
                    this.Condition = "PLAYER_HAS_MAIL Current HasClubCard";
                    break;

                case WarpBehavior.CommunityCenter:
                    this.Condition = "IS_COMMUNITY_CENTER_COMPLETE";
                    break;

                case WarpBehavior.JojaMart:
                    this.Condition = "!IS_COMMUNITY_CENTER_COMPLETE";
                    break;

                case WarpBehavior.MovieTheaterCommunity:
                    this.Condition = "PLAYER_HAS_MAIL Host ccMovieTheater, !PLAYER_HAS_MAIL Host ccMovieTheaterJoja";
                    break;

                case WarpBehavior.MovieTheaterJoja:
                    this.Condition = "PLAYER_HAS_MAIL Host ccMovieTheater, PLAYER_HAS_MAIL Host ccMovieTheaterJoja";
                    break;
            }
        }
    }
}
