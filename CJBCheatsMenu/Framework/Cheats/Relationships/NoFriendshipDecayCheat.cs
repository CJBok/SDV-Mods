using System.Collections.Generic;
using System.Linq;
using CJBCheatsMenu.Framework.Components;
using Netcode;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.Cheats.Relationships;

/// <summary>A cheat which prevents NPC friendships from decaying.</summary>
internal class NoFriendshipDecayCheat : BaseCheat
{
    /*********
    ** Fields
    *********/
    /// <summary>The minimum friendship points to maintain for each NPC.</summary>
    private readonly Dictionary<string, int> PreviousFriendships = new();


    /*********
    ** Public methods
    *********/
    /// <inheritdoc />
    public override IEnumerable<OptionsElement> GetFields(CheatContext context)
    {
        yield return new CheatsOptionsCheckbox(
            label: I18n.Relationships_NoDecay(),
            value: context.Config.NoFriendshipDecay,
            setValue: value => context.Config.NoFriendshipDecay = value
        );
    }

    /// <inheritdoc />
    public override void OnConfig(CheatContext context, out bool needsInput, out bool needsUpdate, out bool needsRendering)
    {
        needsInput = false;
        needsUpdate = context.Config.NoFriendshipDecay;
        needsRendering = false;
    }

    /// <inheritdoc />
    public override void OnSaveLoaded(CheatContext context)
    {
        this.PreviousFriendships.Clear();
    }

    /// <inheritdoc />
    public override void OnUpdated(CheatContext context, UpdateTickedEventArgs e)
    {
        if (e.IsOneSecond && Context.IsWorldReady)
            this.ResetDecay();
    }

    /// <inheritdoc />
    public override void OnSaving(CheatContext context)
    {
        this.ResetDecay();
    }

    /// <summary>Update the tracked friendship points for an NPC.</summary>
    /// <param name="npc">The NPC whose friendship to update.</param>
    /// <param name="points">The new friendship points value.</param>
    public void UpdateFriendship(NPC npc, int points)
    {
        this.PreviousFriendships[npc.Name] = points;
    }


    /*********
    ** Private methods
    *********/
    /// <summary>Undo any reduction in friendship points and update the tracked values.</summary>
    protected void ResetDecay()
    {
        // undo any friendships decreases
        if (this.PreviousFriendships.Any())
        {
            foreach (string key in Game1.player.friendshipData.Keys)
            {
                Friendship friendship = Game1.player.friendshipData[key];
                if (this.PreviousFriendships.TryGetValue(key, out int oldPoints) && oldPoints > friendship.Points)
                    friendship.Points = oldPoints;
            }
        }

        // update friendship tracking
        this.PreviousFriendships.Clear();
        foreach ((string name, NetRef<Friendship> friendship) in Game1.player.friendshipData.FieldDict)
            this.PreviousFriendships[name] = friendship.Value.Points;
    }
}
