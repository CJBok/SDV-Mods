using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Tools;

namespace CJBGrindStone
{
    internal class GrindStone : StardewValley.Object
    {
        /*********
        ** Accessors
        *********/
        public static Rectangle Sprite = new Rectangle(0, 0, 16, 32);


        /*********
        ** Public methods
        *********/
        public GrindStone()
        {
            this.Init();
        }

        public GrindStone(Vector2 location)
        {
            this.Init();
            tileLocation = location;
        }

        public override int maximumStackSize()
        {
            return 999;
        }

        public override bool isPlaceable()
        {
            return true;
        }

        public override bool isPassable()
        {
            return false;
        }

        public override Item getOne()
        {
            return new GrindStone(this.tileLocation)
            {
                scale = this.scale,
                quality = this.quality,
                isSpawnedObject = this.isSpawnedObject,
                isRecipe = this.isRecipe,
                questItem = this.questItem,
                stack = 1,
                name = this.name,
                specialVariable = this.specialVariable,
                price = this.price
            };
        }

        public override string getDescription()
        {
            return "Stone which grinds all sort of things.";
        }

        public override bool placementAction(GameLocation location, int x, int y, Farmer who = null)
        {
            Vector2 vector = new Vector2((float)(x / Game1.tileSize), (float)(y / Game1.tileSize));
            if (!location.objects.ContainsKey(vector))
            {
                location.objects.Add(vector, new GrindStone(vector));
                return true;
            }
            return false;
        }

        public override bool performToolAction(Tool t)
        {
            if (t is Pickaxe)
            {
                Game1.currentLocation.debris.Add(new Debris(new GrindStone(Vector2.Zero), tileLocation * Game1.tileSize));
                Game1.currentLocation.Objects.Remove(tileLocation);
                return false;
            }
            return false;
        }

        public override bool performObjectDropInAction(StardewValley.Object dropIn, bool probe, Farmer who)
        {
            if (dropIn == null || this.heldObject != null || this.readyForHarvest || probe)
                return false;

            switch (dropIn.parentSheetIndex)
            {
                case 262:
                    this.heldObject = new StardewValley.Object(246, 1);
                    minutesUntilReady = 30;
                    Game1.playSound("Ship");
                    return true;
                case 284:
                    this.heldObject = new StardewValley.Object(245, 1);
                    minutesUntilReady = 30;
                    Game1.playSound("Ship");
                    return true;
            }

            return false;
        }

        public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber)
        {
            spriteBatch.Draw(CJBGrindStone.Texture, location + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), new Microsoft.Xna.Framework.Rectangle?(GrindStone.Sprite), Color.White * transparency, 0f, new Vector2(8f, 16f), (float)Game1.pixelZoom * (((double)scaleSize < 0.2) ? scaleSize : (scaleSize / 2f)), SpriteEffects.None, layerDepth);
        }

        public override void drawWhenHeld(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f)
        {
            spriteBatch.Draw(CJBGrindStone.Texture, objectPosition, GrindStone.Sprite, Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + 2) / 10000f));
        }

        public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1)
        {
            Vector2 value = this.getScale();
            value *= (float)Game1.pixelZoom;
            Vector2 value2 = Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), (float)(y * Game1.tileSize - Game1.tileSize)));
            Microsoft.Xna.Framework.Rectangle destinationRectangle = new Microsoft.Xna.Framework.Rectangle((int)(value2.X - value.X / 2f) + ((this.shakeTimer > 0) ? Game1.random.Next(-1, 2) : 0), (int)(value2.Y - value.Y / 2f) + ((this.shakeTimer > 0) ? Game1.random.Next(-1, 2) : 0), (int)((float)Game1.tileSize + value.X), (int)((float)(Game1.tileSize * 2) + value.Y / 2f));
            spriteBatch.Draw(CJBGrindStone.Texture, destinationRectangle, GrindStone.Sprite, Color.White * alpha, 0f, Vector2.Zero, SpriteEffects.None, Math.Max(0f, (float)((y + 1) * Game1.tileSize - Game1.pixelZoom * 6) / 10000f) + ((this.parentSheetIndex == 105) ? 0.0035f : 0f) + (float)x * 1E-08f);
            if (this.readyForHarvest)
            {
                float num = 4f * (float)Math.Round(Math.Sin(DateTime.Now.TimeOfDay.TotalMilliseconds / 250.0), 2);
                spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize - 8), (float)(y * Game1.tileSize - Game1.tileSize * 3 / 2 - 16) + num)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(141, 465, 20, 24)), Color.White * 0.75f, 0f, Vector2.Zero, 4f, SpriteEffects.None, (float)((y + 1) * Game1.tileSize) / 10000f + 1E-06f + this.tileLocation.X / 10000f + ((this.parentSheetIndex == 105) ? 0.0015f : 0f));
                spriteBatch.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize + Game1.tileSize / 2), (float)(y * Game1.tileSize - Game1.tileSize - Game1.tileSize / 8) + num)), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.heldObject.parentSheetIndex, 16, 16)), Color.White * 0.75f, 0f, new Vector2(8f, 8f), (float)Game1.pixelZoom, SpriteEffects.None, (float)((y + 1) * Game1.tileSize) / 10000f + 1E-05f + this.tileLocation.X / 10000f + ((this.parentSheetIndex == 105) ? 0.0015f : 0f));
            }
        }


        /*********
        ** Private methods
        *********/
        private void Init()
        {
            parentSheetIndex = 3920;
            bigCraftable = true;
            name = "Grind Stone";
            type = "Crafting";
            tileLocation = Vector2.Zero;
            boundingBox = new Rectangle(0, 0, 64, 64);
        }
    }
}
