using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.Cheats
{
    /// <summary>The base implementation for a cheat that can be configured and applied through CJB Cheats Menu.</summary>
    internal abstract class BaseCheat : ICheat
    {
        /*********
        ** Public methods
        *********/
        /// <summary>Get the config UI fields to show in the cheats menu.</summary>
        /// <param name="context">The cheat context.</param>
        public abstract IEnumerable<OptionsElement> GetFields(CheatContext context);

        /// <summary>Handle the cheat options being loaded or changed.</summary>
        /// <param name="context">The cheat context.</param>
        /// <param name="needsUpdate">Whether the cheat should be notified of game updates.</param>
        /// <param name="needsInput">Whether the cheat should be notified of button presses.</param>
        /// <param name="needsRendering">Whether the cheat should be notified of render ticks.</param>
        public virtual void OnConfig(CheatContext context, out bool needsInput, out bool needsUpdate, out bool needsRendering)
        {
            needsInput = false;
            needsUpdate = false;
            needsRendering = false;
        }

        /// <summary>Handle the player loading a save file.</summary>
        /// <param name="context">The cheat context.</param>
        public virtual void OnSaveLoaded(CheatContext context) { }

        /// <summary>Handle the player pressing a button if <see cref="ICheat.OnSaveLoaded"/> indicated input was needed.</summary>
        /// <param name="context">The cheat context.</param>
        /// <param name="e">The input event arguments.</param>
        public virtual void OnButtonPressed(CheatContext context, ButtonPressedEventArgs e) { }

        /// <summary>Handle the player releasing a button if <see cref="ICheat.OnSaveLoaded"/> indicated input was needed.</summary>
        /// <param name="context">The cheat context.</param>
        /// <param name="e">The input event arguments.</param>
        public virtual void OnButtonReleased(CheatContext context, ButtonReleasedEventArgs e) { }

        /// <summary>Handle a game update if <see cref="ICheat.OnSaveLoaded"/> indicated updates were needed.</summary>
        /// <param name="context">The cheat context.</param>
        /// <param name="e">The update event arguments.</param>
        public virtual void OnUpdated(CheatContext context, UpdateTickedEventArgs e) { }

        /// <summary>Handle the game draws to the sprite patch in a draw tick, just before the final sprite batch is rendered to the screen.</summary>
        /// <param name="context">The cheat context.</param>
        /// <param name="spriteBatch">The sprite batch being drawn.</param>
        public virtual void OnRendered(CheatContext context, SpriteBatch spriteBatch) { }


        /*********
        ** Protected methods
        *********/
        /// <summary>Get fields sorted by their display label.</summary>
        /// <param name="fields">The fields to sort.</param>
        protected IEnumerable<OptionsElement> SortFields(params OptionsElement[] fields)
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
        protected bool HasEvent(int id)
        {
            return Game1.player.eventsSeen.Contains(id);
        }

        /// <summary>Set whether the player has seen the given event.</summary>
        /// <param name="id">The event to set.</param>
        /// <param name="enable">Whether to add the event, as opposed to removing it.</param>
        protected void SetEvent(int id, bool enable)
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
}
