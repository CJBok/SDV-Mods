using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI.Events;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.Cheats
{
    /// <summary>A cheat that can be configured and applied through CJB Cheats Menu.</summary>
    internal interface ICheat
    {
        /*********
        ** Methods
        *********/
        /// <summary>Get the config UI fields to show in the cheats menu.</summary>
        /// <param name="context">The cheat context.</param>
        IEnumerable<OptionsElement> GetFields(CheatContext context);

        /// <summary>Handle the cheat options being loaded or changed.</summary>
        /// <param name="context">The cheat context.</param>
        /// <param name="needsUpdate">Whether the cheat should be notified of game updates.</param>
        /// <param name="needsInput">Whether the cheat should be notified of button presses.</param>
        /// <param name="needsRendering">Whether the cheat should be notified of render ticks.</param>
        void OnConfig(CheatContext context, out bool needsInput, out bool needsUpdate, out bool needsRendering);

        /// <summary>Handle the player loading a save file.</summary>
        /// <param name="context">The cheat context.</param>
        void OnSaveLoaded(CheatContext context);

        /// <summary>Handle the player pressing a button if <see cref="OnSaveLoaded"/> indicated input was needed.</summary>
        /// <param name="context">The cheat context.</param>
        /// <param name="e">The input event arguments.</param>
        void OnButtonPressed(CheatContext context, ButtonPressedEventArgs e);

        /// <summary>Handle the player releasing a button if <see cref="OnSaveLoaded"/> indicated input was needed.</summary>
        /// <param name="context">The cheat context.</param>
        /// <param name="e">The input event arguments.</param>
        void OnButtonReleased(CheatContext context, ButtonReleasedEventArgs e);

        /// <summary>Handle a game update if <see cref="OnSaveLoaded"/> indicated updates were needed.</summary>
        /// <param name="context">The cheat context.</param>
        /// <param name="e">The update event arguments.</param>
        void OnUpdated(CheatContext context, UpdateTickedEventArgs e);

        /// <summary>Handle the game draws to the sprite patch in a draw tick, just before the final sprite batch is rendered to the screen.</summary>
        /// <param name="context">The cheat context.</param>
        /// <param name="spriteBatch">The sprite batch being drawn.</param>
        void OnRendered(CheatContext context, SpriteBatch spriteBatch);
    }
}
