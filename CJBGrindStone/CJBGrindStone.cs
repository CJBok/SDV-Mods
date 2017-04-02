using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace CJBGrindStone
{
    public class CJBGrindStone : Mod
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
            GameEvents.LoadContent += GameEvents_LoadContent;
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

        private void GameEvents_LoadContent(object sender, EventArgs e)
        {
            using (FileStream filestream = new FileStream(Path.Combine(this.Helper.DirectoryPath, "Content", "grindstone.png"), FileMode.Open))
                CJBGrindStone.Texture = Texture2D.FromStream(Game1.graphics.GraphicsDevice, filestream);
        }
    }
}
