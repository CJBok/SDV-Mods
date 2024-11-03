using StardewValley;

namespace CJBItemSpawner.Framework;

/// <summary>Manages the gamepad text entry UI.</summary>
internal class TextEntryManager
{
    /*********
    ** Fields
    *********/
    /// <summary>The last game update tick when the UI was open.</summary>
    private long LastTickOpen;


    /*********
    ** Accessors
    *********/
    /// <summary>Whether the text entry UI is currently shown.</summary>
    public bool IsOpen => Game1.textEntry != null;


    /*********
    ** Public methods
    *********/
    /// <summary>Update the text entry manager state for a game tick.</summary>
    public void Update()
    {
        if (this.IsOpen)
            this.LastTickOpen = Game1.ticks;
    }

    /// <summary>Whether the text entry UI was just closed.</summary>
    /// <param name="tolerance">The maximum number of ticks since the menu was closed.</param>
    public bool JustClosed(int tolerance = 2)
    {
        return !this.IsOpen && this.LastTickOpen >= Game1.ticks - tolerance;
    }
}
