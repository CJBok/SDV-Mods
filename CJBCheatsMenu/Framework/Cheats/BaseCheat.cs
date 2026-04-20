using System.Collections.Generic;
using System.Linq;
using CJBCheatsMenu.Framework.Components;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.Cheats;

/// <summary>The base implementation for a cheat that can be configured and applied through CJB Cheats Menu.</summary>
internal abstract class BaseCheat : ICheat
{
    /*********
    ** Public methods
    *********/
    /// <inheritdoc />
    public abstract IEnumerable<CheatElement> GetFields(CheatContext context);

    /// <inheritdoc />
    public virtual void OnConfig(CheatContext context, out bool needsInput, out bool needsUpdate, out bool needsRendering)
    {
        needsInput = false;
        needsUpdate = false;
        needsRendering = false;
    }

    /// <inheritdoc />
    public virtual void OnSaveLoaded(CheatContext context) { }

    /// <inheritdoc />
    public virtual void OnCheatsMenuOpening(CheatContext context) { }

    /// <inheritdoc />
    public virtual void OnButtonsChanged(CheatContext context, ButtonsChangedEventArgs e) { }

    /// <inheritdoc />
    public virtual void OnUpdated(CheatContext context, UpdateTickedEventArgs e) { }

    /// <inheritdoc />
    public virtual void OnSaving(CheatContext context) { }

    /// <inheritdoc />
    public virtual void OnRendered(CheatContext context, SpriteBatch spriteBatch) { }


    /*********
    ** Protected methods
    *********/
    /// <summary>Get fields sorted by their display label.</summary>
    /// <param name="fields">The fields to sort.</param>
    protected IEnumerable<CheatElement> SortFields(params CheatElement[] fields)
    {
        return fields.OrderBy(p => p.label);
    }

    /// <summary>Get all NPCs which have relationship data.</summary>
    /// <remarks>Derived from the <see cref="SocialPage"/> constructor.</remarks>
    protected IEnumerable<NPC> GetSocialCharacters()
    {
        foreach (NPC npc in Utility.getAllCharacters())
        {
            if (npc.CanSocialize || Game1.player.friendshipData.ContainsKey(npc.Name))
                yield return npc;
        }
    }

    /// <summary>Get whether the player has the given mail flag.</summary>
    /// <param name="flag">The mail flag to check.</param>
    protected bool HasFlag(string flag)
    {
        return Game1.player.mailReceived.Contains(flag);
    }

    /// <summary>Set whether the player has the given mail flag.</summary>
    /// <param name="enable">Whether to add the flag, as opposed to removing it.</param>
    /// <param name="flag">The mail flag to set.</param>
    /// <returns>Returns whether the flag changed.</returns>
    protected bool SetFlag(bool enable, string flag)
    {
        // set
        if (enable)
        {
            if (!this.HasFlag(flag))
            {
                Game1.player.mailReceived.Add(flag);
                return true;
            }
            return false;
        }

        // unset
        return Game1.player.mailReceived.Remove(flag);
    }

    /// <summary>Get whether the player has seen the given event.</summary>
    /// <param name="id">The event ID to check.</param>
    protected bool HasEvent(string id)
    {
        return Game1.player.eventsSeen.Contains(id);
    }

    /// <summary>Set whether the player has seen the given event.</summary>
    /// <param name="id">The event to set.</param>
    /// <param name="enable">Whether to add the event, as opposed to removing it.</param>
    protected void SetEvent(string id, bool enable)
    {
        if (enable)
        {
            if (!this.HasEvent(id))
                Game1.player.eventsSeen.Add(id);
        }
        else
            Game1.player.eventsSeen.Remove(id);
    }
}
