using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Tools;
using SObject = StardewValley.Object;
using SFarmer = StardewValley.Farmer;

namespace CJBGrindStone.Framework
{
    internal class GrindStone : SObject
    {
        /*********
        ** Properties
        *********/
        private readonly Rectangle Sprite = new Rectangle(0, 0, 16, 32);


        /*********
        ** Public methods
        *********/
        public GrindStone()
        {
            //parentSheetIndex = 3920;
            //bigCraftable = true;
            //name = "Grind Stone";
            //type = "Crafting";
            //tileLocation = Vector2.Zero;
            //boundingBox = new Rectangle(0, 0, 64, 64);
        }

        public GrindStone(Vector2 location)
            : this()
        {
            this.tileLocation = location;
        }

        public override int maximumStackSize() => 999;
        public override bool isPlaceable() => true;
        public override bool isPassable() => false;

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

        public override string getDescription() => "Stone which grinds all sort of things.";

        public override bool placementAction(GameLocation location, int x, int y, SFarmer who = null)
        {
            Vector2 vector = new Vector2(x / Game1.tileSize, y / Game1.tileSize);
            if (!location.objects.ContainsKey(vector))
            {
                location.objects.Add(vector, new GrindStone(vector));
                return true;
            }
            return false;
        }

        public override bool performToolAction(Tool tool)
        {
            if (tool is Pickaxe)
            {
                Game1.currentLocation.debris.Add(new Debris(new GrindStone(Vector2.Zero), this.tileLocation * Game1.tileSize));
                Game1.currentLocation.Objects.Remove(this.tileLocation);
                return false;
            }
            return false;
        }

        public override bool performObjectDropInAction(SObject dropIn, bool probe, SFarmer who)
        {
            if (dropIn == null || this.heldObject != null || this.readyForHarvest || probe)
                return false;

            switch (dropIn.parentSheetIndex)
            {
                case 262:
                    this.heldObject = new SObject(246, 1);
                    this.minutesUntilReady = 30;
                    Game1.playSound("Ship");
                    return true;
                case 284:
                    this.heldObject = new SObject(245, 1);
                    this.minutesUntilReady = 30;
                    Game1.playSound("Ship");
                    return true;
            }

            return false;
        }

        public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber)
        {
            spriteBatch.Draw(ModEntry.Texture, location + new Vector2(Game1.tileSize / 2, Game1.tileSize / 2), this.Sprite, Color.White * transparency, 0f, new Vector2(8f, 16f), Game1.pixelZoom * (scaleSize < 0.2 ? scaleSize : (scaleSize / 2f)), SpriteEffects.None, layerDepth);
        }

        public override void drawWhenHeld(SpriteBatch spriteBatch, Vector2 objectPosition, SFarmer f)
        {
            spriteBatch.Draw(ModEntry.Texture, objectPosition, this.Sprite, Color.White, 0f, Vector2.Zero, Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (f.getStandingY() + 2) / 10000f));
        }

        public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1)
        {
            Vector2 value = this.getScale();
            value *= Game1.pixelZoom;
            Vector2 value2 = Game1.GlobalToLocal(Game1.viewport, new Vector2(x * Game1.tileSize, y * Game1.tileSize - Game1.tileSize));
            Rectangle destinationRectangle = new Rectangle((int)(value2.X - value.X / 2f) + ((this.shakeTimer > 0) ? Game1.random.Next(-1, 2) : 0), (int)(value2.Y - value.Y / 2f) + ((this.shakeTimer > 0) ? Game1.random.Next(-1, 2) : 0), (int)(Game1.tileSize + value.X), (int)(Game1.tileSize * 2 + value.Y / 2f));
            spriteBatch.Draw(ModEntry.Texture, destinationRectangle, this.Sprite, Color.White * alpha, 0f, Vector2.Zero, SpriteEffects.None, Math.Max(0f, ((y + 1) * Game1.tileSize - Game1.pixelZoom * 6) / 10000f) + ((this.parentSheetIndex == 105) ? 0.0035f : 0f) + x * 1E-08f);
            if (this.readyForHarvest)
            {
                float num = 4f * (float)Math.Round(Math.Sin(DateTime.Now.TimeOfDay.TotalMilliseconds / 250.0), 2);
                spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2(x * Game1.tileSize - 8, y * Game1.tileSize - Game1.tileSize * 3 / 2 - 16 + num)), new Rectangle(141, 465, 20, 24), Color.White * 0.75f, 0f, Vector2.Zero, 4f, SpriteEffects.None, (y + 1) * Game1.tileSize / 10000f + 1E-06f + this.tileLocation.X / 10000f + ((this.parentSheetIndex == 105) ? 0.0015f : 0f));
                spriteBatch.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2(x * Game1.tileSize + Game1.tileSize / 2, y * Game1.tileSize - Game1.tileSize - Game1.tileSize / 8 + num)), Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.heldObject.parentSheetIndex, 16, 16), Color.White * 0.75f, 0f, new Vector2(8f, 8f), Game1.pixelZoom, SpriteEffects.None, (y + 1) * Game1.tileSize / 10000f + 1E-05f + this.tileLocation.X / 10000f + ((this.parentSheetIndex == 105) ? 0.0015f : 0f));
            }
        }
    }
}
