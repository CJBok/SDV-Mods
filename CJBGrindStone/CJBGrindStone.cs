using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using StardewModdingAPI.Inheritance;
using Microsoft.Xna.Framework;
using System.Xml.Serialization;
using System.Reflection;
using StardewValley.Monsters;
using StardewValley.Characters;
using StardewValley.TerrainFeatures;
using StardewValley.Quests;
using StardewValley.Objects;

namespace CJBGrindStone
{
    public class CJBGrindStone : Mod
    {

        public static Texture2D grindstoneTex;

        public override void Entry(params object[] objects) {
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

        private void ControlEvents_KeyReleased(object sender, EventArgsKeyPressed e) {
            if (e.KeyPressed == Microsoft.Xna.Framework.Input.Keys.End) {
                Log.Info("test");
                Game1.player.addItemToInventory(new GrindStone(Vector2.Zero));
            }
        }

        private void GameEvents_LoadContent(object sender, EventArgs e) {
            FileStream filestream = new FileStream(PathOnDisk + @"\Content\grindstone.png", FileMode.Open);
            grindstoneTex = Texture2D.FromStream(SGame.Instance.GraphicsDevice, filestream);
        }
    }
}
