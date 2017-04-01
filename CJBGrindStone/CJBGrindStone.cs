using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        public static Texture2D grindstoneTex;


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            /*SaveGame.serializer = new XmlSerializer(typeof(SaveGame), new Type[28]
              {
                typeof (Tool),
                typeof (GameLocation),
                typeof (Crow),
                typeof (Duggy),
                typeof (Bug),
                typeof (BigSlime),
                typeof (Fireball),
                typeof (Ghost),
                typeof (Child),
                typeof (Pet),
                typeof (Dog),
                typeof (StardewValley.Characters.Cat),
                typeof (Horse),
                typeof (GreenSlime),
                typeof (LavaCrab),
                typeof (RockCrab),
                typeof (ShadowGuy),
                typeof (SkeletonMage),
                typeof (SquidKid),
                typeof (Grub),
                typeof (Fly),
                typeof (DustSpirit),
                typeof (Quest),
                typeof (MetalHead),
                typeof (ShadowGirl),
                typeof (Monster),
                typeof (TerrainFeature),
                typeof (GrindStone)
              });
                    SaveGame.farmerSerializer = new XmlSerializer(typeof(Farmer), new Type[2]
                    {
                typeof(Tool),
                typeof(GrindStone)
                    });
                    SaveGame.locationSerializer = new XmlSerializer(typeof(GameLocation), new Type[27]
                    {
                typeof (Tool),
                typeof (Crow),
                typeof (Duggy),
                typeof (Fireball),
                typeof (Ghost),
                typeof (GreenSlime),
                typeof (LavaCrab),
                typeof (RockCrab),
                typeof (ShadowGuy),
                typeof (SkeletonWarrior),
                typeof (Child),
                typeof (Pet),
                typeof (Dog),
                typeof (StardewValley.Characters.Cat),
                typeof (Horse),
                typeof (SquidKid),
                typeof (Grub),
                typeof (Fly),
                typeof (DustSpirit),
                typeof (Bug),
                typeof (BigSlime),
                typeof (BreakableContainer),
                typeof (MetalHead),
                typeof (ShadowGirl),
                typeof (Monster),
                typeof (TerrainFeature),
                typeof (GrindStone)
            });*/
            GameEvents.LoadContent += GameEvents_LoadContent;
            ControlEvents.KeyReleased += ControlEvents_KeyReleased;
        }


        /*********
        ** Private methods
        *********/
        private void ControlEvents_KeyReleased(object sender, EventArgsKeyPressed e)
        {
            if (e.KeyPressed == Microsoft.Xna.Framework.Input.Keys.End)
            {
                this.Monitor.Log("test", LogLevel.Info);
                Game1.player.addItemToInventory(new GrindStone(Vector2.Zero));
            }
        }

        private void GameEvents_LoadContent(object sender, EventArgs e)
        {
            using (FileStream filestream = new FileStream(Path.Combine(this.Helper.DirectoryPath, "Content", "grindstone.png"), FileMode.Open))
                grindstoneTex = Texture2D.FromStream(Game1.graphics.GraphicsDevice, filestream);
        }
    }
}
