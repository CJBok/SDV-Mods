using System;
using System.Collections.Generic;
using System.Linq;
using CJBCheatsMenu.Framework.Components;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Quests;

namespace CJBCheatsMenu.Framework.Cheats.Relationships;

/// <summary>A cheat which sets the heart levels for social NPCs.</summary>
internal class HeartsCheat : BaseCheat
{
    /*********
    ** Fields
    *********/
    /// <summary>A callback to invoke when the friendship points for an NPC changes.</summary>
    private readonly Action<NPC, int> OnPointsChanged;


    /*********
    ** Public methods
    *********/
    /// <summary>Construct an instance.</summary>
    /// <param name="onPointsChanged">A callback to invoke when the friendship points for an NPC changes.</param>
    public HeartsCheat(Action<NPC, int> onPointsChanged)
    {
        this.OnPointsChanged = onPointsChanged;
    }

    /// <inheritdoc />
    public override IEnumerable<OptionsElement> GetFields(CheatContext context)
    {
        return this.SortFields(
            this.GetSocialCharacters()
                .Distinct()
                .Select(this.GetField)
                .ToArray()
        );
    }

    /// <inheritdoc />
    public override void OnConfig(CheatContext context, out bool needsInput, out bool needsUpdate, out bool needsRendering)
    {
        needsInput = false;
        needsUpdate = context.Config.NoFriendshipDecay;
        needsRendering = false;
    }


    /*********
    ** Private methods
    *********/
    /// <summary>Get the unsorted fields to display.</summary>
    private OptionsElement GetField(NPC npc)
    {
        // get friendship info
        Game1.player.friendshipData.TryGetValue(npc.Name, out Friendship? friendship);
        bool isSpouse = friendship?.IsMarried() ?? false;
        int curHearts = (friendship?.Points ?? 0) / NPC.friendshipPointsPerHeartLevel;
        int maxHearts = isSpouse ? 14 : NPC.maxFriendshipPoints / NPC.friendshipPointsPerHeartLevel;

        // get field
        return new CheatsOptionsNpcSlider(
            npc: npc,
            isMet: friendship != null,
            value: curHearts,
            maxValue: maxHearts,
            setValue: hearts => this.SetFriendshipHearts(npc, hearts)
        );
    }

    /// <summary>Set the friendship hearts for an NPC.</summary>
    /// <param name="npc">The NPC to change.</param>
    /// <param name="hearts">The friendship hearts to set.</param>
    private void SetFriendshipHearts(NPC npc, int hearts)
    {
        // add friendship if needed
        if (!Game1.player.friendshipData.TryGetValue(npc.Name, out Friendship friendship))
        {
            friendship = new Friendship();
            Game1.player.friendshipData.Add(npc.Name, friendship);
            SocializeQuest? socialQuest = Game1.player.questLog.OfType<SocializeQuest>().FirstOrDefault();
            if (socialQuest != null && !socialQuest.completed.Value)
                socialQuest.OnNpcSocialized(npc);
        }

        // update friendship points
        friendship.Points = hearts * NPC.friendshipPointsPerHeartLevel;
        this.OnPointsChanged(npc, hearts * NPC.friendshipPointsPerHeartLevel);
    }
}
