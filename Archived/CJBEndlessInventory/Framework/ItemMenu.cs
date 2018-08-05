using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardewValley.Menus;
using SFarmer = StardewValley.Farmer;

namespace CJBEndlessInventory.Framework
{
    internal class ItemMenu : ItemMenuWithInventory
    {
        /*********
        ** Properties
        *********/
        private readonly ItemInventoryMenu ItemsToGrabMenu;
        private readonly ClickableComponent Title;
        private readonly ClickableTextureComponent UpArrow;
        private readonly ClickableTextureComponent DownArrow;
        private readonly BehaviorOnItemSelect BehaviorFunction;
        private readonly BehaviorOnItemSelect BehaviorOnItemGrab;
        private readonly bool AllowRightClick;

        private TemporaryAnimatedSprite Poof;
        private bool ShowReceivingMenu = true;
        private bool ReverseGrab = true;
        private bool DestroyItemOnClick = true;
        private bool CanExitOnKey = true;
        private bool CanClose;


        /*********
        ** Accessors
        *********/
        public delegate void BehaviorOnItemSelect(Item item, SFarmer who);


        /*********
        ** Public methods
        *********/
        public ItemMenu(List<Item> inventory, BehaviorOnItemSelect behaviorOnItemSelectFunction, BehaviorOnItemSelect behaviorOnItemGrab = null)
          : base(null, true, true)
        {
            this.BehaviorFunction = behaviorOnItemSelectFunction;
            this.BehaviorOnItemGrab = behaviorOnItemGrab;

            this.Title = new ClickableComponent(new Rectangle(this.xPositionOnScreen + (this.width / 2), this.yPositionOnScreen - Game1.tileSize * 2, Game1.tileSize * 4, Game1.tileSize), "CJB Endless Inventory");
            this.UpArrow = new ClickableTextureComponent("up-arrow", new Rectangle(this.xPositionOnScreen + this.width - Game1.tileSize / 2, this.yPositionOnScreen - Game1.tileSize, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), "", "", Game1.mouseCursors, new Rectangle(421, 459, 11, 12), Game1.pixelZoom);
            this.DownArrow = new ClickableTextureComponent("down-arrow", new Rectangle(this.xPositionOnScreen + this.width - Game1.tileSize / 2, this.yPositionOnScreen + this.height / 2 - Game1.tileSize * 2, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), "", "", Game1.mouseCursors, new Rectangle(421, 472, 11, 12), Game1.pixelZoom);

            this.AllowRightClick = true;
            this.Inventory.ShowGrayedOutSlots = false;

            this.ItemsToGrabMenu = new ItemInventoryMenu(this.xPositionOnScreen + Game1.tileSize / 2, this.yPositionOnScreen, false, inventory, null, 99999, 99999 / 12);
        }

        public override void receiveRightClick(int x, int y, bool playSound = true)
        {
            if (!this.AllowRightClick)
                return;
            base.receiveRightClick(x, y, false);

            if (this.HeldItem == null && this.ShowReceivingMenu)
            {
                this.HeldItem = this.ItemsToGrabMenu.RightClick(x, y, this.HeldItem, false);
                if (this.HeldItem != null)
                    this.BehaviorOnItemGrab?.Invoke(this.HeldItem, Game1.player);
                if (this.HeldItem is Object && (this.HeldItem as Object).parentSheetIndex == 326)
                {
                    this.HeldItem = null;
                    Game1.player.canUnderstandDwarves = true;
                    this.Poof = new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 320, 64, 64), 50f, 8, 0, new Vector2(x - x % Game1.tileSize + Game1.tileSize / 4, y - y % Game1.tileSize + Game1.tileSize / 4), false, false);
                    Game1.playSound("fireball");
                    return;
                }
                if (this.HeldItem is Object recipe && recipe.isRecipe)
                {
                    string key = recipe.Name.Substring(0, recipe.Name.IndexOf("Recipe") - 1);
                    try
                    {
                        if (recipe.category == -7)
                            Game1.player.cookingRecipes.Add(key, 0);
                        else
                            Game1.player.craftingRecipes.Add(key, 0);
                        this.Poof = new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 320, 64, 64), 50f, 8, 0, new Vector2(x - x % Game1.tileSize + Game1.tileSize / 4, y - y % Game1.tileSize + Game1.tileSize / 4), false, false);
                        Game1.playSound("newRecipe");
                    }
                    catch { }
                    this.HeldItem = null;
                    return;
                }
                if (Game1.player.addItemToInventoryBool(this.HeldItem))
                {
                    CJB.RemoveLastHudMessage();
                    this.HeldItem = null;
                    Game1.playSound("coin");
                }
            }
            else if (this.ReverseGrab)
            {
                this.BehaviorFunction(this.HeldItem, Game1.player);
                if (this.DestroyItemOnClick)
                    this.HeldItem = null;
            }
        }

        public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
        {
            ItemMenu.Open();
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            base.receiveLeftClick(x, y, !this.DestroyItemOnClick);

            if (this.HeldItem == null)
            {
                if (this.UpArrow.bounds.Contains(x, y))
                    this.ItemsToGrabMenu?.receiveScrollWheelAction(1);

                if (this.DownArrow.bounds.Contains(x, y))
                    this.ItemsToGrabMenu?.receiveScrollWheelAction(-1);
            }
            if (this.HeldItem == null && this.ShowReceivingMenu)
            {
                this.HeldItem = this.ItemsToGrabMenu.LeftClick(x, y, this.HeldItem, false);
                if (this.HeldItem != null)
                    this.BehaviorOnItemGrab?.Invoke(this.HeldItem, Game1.player);
                if (this.HeldItem is Object obj && obj.parentSheetIndex == 326)
                {
                    this.HeldItem = null;
                    Game1.player.canUnderstandDwarves = true;
                    this.Poof = new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 320, 64, 64), 50f, 8, 0, new Vector2(x - x % Game1.tileSize + Game1.tileSize / 4, y - y % Game1.tileSize + Game1.tileSize / 4), false, false);
                    Game1.playSound("fireball");
                }
                else if (this.HeldItem is Object && (this.HeldItem as Object).parentSheetIndex == 102)
                {
                    this.HeldItem = null;
                    Game1.player.foundArtifact(102, 1);
                    this.Poof = new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 320, 64, 64), 50f, 8, 0, new Vector2(x - x % Game1.tileSize + Game1.tileSize / 4, y - y % Game1.tileSize + Game1.tileSize / 4), false, false);
                    Game1.playSound("fireball");
                }
                else if (this.HeldItem is Object recipe && recipe.isRecipe)
                {
                    string key = recipe.Name.Substring(0, recipe.Name.IndexOf("Recipe") - 1);
                    try
                    {
                        if (recipe.category == -7)
                            Game1.player.cookingRecipes.Add(key, 0);
                        else
                            Game1.player.craftingRecipes.Add(key, 0);
                        this.Poof = new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 320, 64, 64), 50f, 8, 0, new Vector2(x - x % Game1.tileSize + Game1.tileSize / 4, y - y % Game1.tileSize + Game1.tileSize / 4), false, false);
                        Game1.playSound("newRecipe");
                    }
                    catch { }
                    this.HeldItem = null;
                }
                else if (Game1.player.addItemToInventoryBool(this.HeldItem))
                {
                    CJB.RemoveLastHudMessage();
                    this.HeldItem = null;
                    Game1.playSound("coin");
                }
            }
            else if ((this.ReverseGrab || this.BehaviorFunction != null) && this.isWithinBounds(x, y))
            {
                this.BehaviorFunction(this.HeldItem, Game1.player);
                if (this.DestroyItemOnClick)
                {
                    this.HeldItem = null;
                    return;
                }
            }
            if (this.HeldItem != null && !this.isWithinBounds(x, y) && this.HeldItem.canBeTrashed())
            {
                Game1.playSound("throwDownITem");
                Game1.createItemDebris(this.HeldItem, Game1.player.getStandingPosition(), Game1.player.FacingDirection);
                this.HeldItem = null;
            }
        }

        public bool AreAllItemsTaken()
        {
            return this.ItemsToGrabMenu.ActualInventory.All(t => t == null);
        }

        public override void receiveKeyPress(Keys key)
        {
            if ((this.CanExitOnKey || this.AreAllItemsTaken()) && ((Game1.options.doesInputListContain(Game1.options.menuButton, key) || key.ToString() == ModEntry.Settings.MenuButton) && this.readyToClose() && this.CanClose))
            {
                this.exitThisMenu();
                if (Game1.currentLocation.currentEvent != null)
                    ++Game1.currentLocation.currentEvent.CurrentCommand;

                return;
            }

            if (Game1.options.doesInputListContain(Game1.options.menuButton, key) && this.HeldItem != null)
                Game1.setMousePosition(this.TrashCan.bounds.Center);
            if (key != Keys.Delete || this.HeldItem == null || !this.HeldItem.canBeTrashed())
                return;
            if (this.HeldItem is Object obj && Game1.player.specialItems.Contains(obj.parentSheetIndex))
                Game1.player.specialItems.Remove(obj.parentSheetIndex);
            this.HeldItem = null;
            Game1.playSound("trashcan");
        }

        public override void update(GameTime time)
        {
            if (Keyboard.GetState().GetPressedKeys().Length == 0)
                this.CanClose = true;
            base.update(time);
            if (this.Poof == null || !this.Poof.update(time))
                return;
            this.Poof = null;
        }

        public override void performHoverAction(int x, int y)
        {
            if (this.ItemsToGrabMenu.isWithinBounds(x, y) && this.ShowReceivingMenu)
                this.HoveredItem = this.ItemsToGrabMenu.Hover(x, y, this.HeldItem);
            else
                base.performHoverAction(x, y);
        }

        public override void receiveScrollWheelAction(int direction)
        {
            if (GameMenu.forcePreventClose)
                return;

            this.ItemsToGrabMenu?.receiveScrollWheelAction(direction);
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            if (!Game1.options.showMenuBackground)
                spriteBatch.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.4f);
            this.Draw(spriteBatch, false, false);
            if (this.ShowReceivingMenu)
            {
                CJB.DrawTextBox(this.Title.bounds.X, this.Title.bounds.Y, Game1.dialogueFont, this.Title.name, true, 1);
                Game1.drawDialogueBox(this.ItemsToGrabMenu.xPositionOnScreen - IClickableMenu.borderWidth - IClickableMenu.spaceToClearSideBorder, this.ItemsToGrabMenu.yPositionOnScreen - IClickableMenu.borderWidth - IClickableMenu.spaceToClearTopBorder, this.ItemsToGrabMenu.width + IClickableMenu.borderWidth * 2 + IClickableMenu.spaceToClearSideBorder * 2, this.ItemsToGrabMenu.height + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth * 2, false, true);
                this.ItemsToGrabMenu.draw(spriteBatch);
                this.UpArrow.draw(spriteBatch);
                this.DownArrow.draw(spriteBatch);
            }

            this.Poof?.draw(spriteBatch, true);
            if (this.HoverText != null && (this.HoveredItem == null || this.ItemsToGrabMenu == null))
                IClickableMenu.drawHoverText(spriteBatch, this.HoverText, Game1.smallFont);
            if (this.HoveredItem != null)
                IClickableMenu.drawToolTip(spriteBatch, this.HoveredItem.getDescription(), this.HoveredItem.Name, this.HoveredItem, this.HeldItem != null);
            else if (this.HoveredItem != null && this.ItemsToGrabMenu != null)
                IClickableMenu.drawToolTip(spriteBatch, this.ItemsToGrabMenu.DescriptionText, this.ItemsToGrabMenu.DescriptionTitle, this.HoveredItem, this.HeldItem != null);
            this.HeldItem?.drawInMenu(spriteBatch, new Vector2(Game1.getOldMouseX() + 8, Game1.getOldMouseY() + 8), 1f);

            if (!Game1.options.hardwareCursor)
                spriteBatch.Draw(Game1.mouseCursors, new Vector2(Game1.getOldMouseX(), Game1.getOldMouseY()), Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 0, 16, 16), Color.White, 0.0f, Vector2.Zero, Game1.pixelZoom + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 1f);
        }

        public static void Open()
        {
            Storage storage = new Storage(0, ModEntry.StorageItems.PlayerItems, Vector2.Zero);
            Game1.activeClickableMenu = new ItemMenu(ModEntry.StorageItems.PlayerItems, storage.GrabItemFromInventory, storage.GrabItemFromChest);
        }
    }
}
