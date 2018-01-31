using CJBGrindStone.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace CJBGrindStone
{
    internal class ModEntry : Mod
    {
        /*********
        ** Accessors
        *********/
        internal static Texture2D Texture;


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            ModEntry.Texture = helper.Content.Load<Texture2D>("Content/grindstone.png");

            ControlEvents.KeyReleased += ControlEvents_KeyReleased;
        }


        /*********
        ** Private methods
        *********/
        private void ControlEvents_KeyReleased(object sender, EventArgsKeyPressed e)
        {
            if (e.KeyPressed == Keys.End)
            {
                this.Monitor.Log("test", LogLevel.Info);
                Game1.player.addItemToInventory(new GrindStone(Vector2.Zero));
            }
        }
    }
}
