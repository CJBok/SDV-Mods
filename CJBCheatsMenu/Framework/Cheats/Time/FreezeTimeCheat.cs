using System.Collections.Generic;
using CJB.Common;
using CJBCheatsMenu.Framework.Components;
using CJBCheatsMenu.Framework.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.Cheats.Time
{
    /// <summary>A cheat which stops the passage of time.</summary>
    internal class FreezeTimeCheat : BaseCheat
    {
        /*********
        ** Public methods
        *********/
        /// <summary>Get the config UI fields to show in the cheats menu.</summary>
        /// <param name="context">The cheat context.</param>
        public override IEnumerable<OptionsElement> GetFields(CheatContext context)
        {
            return new[]
            {
                new CheatsOptionsCheckbox(I18n.Time_FreezeInside(), context.Config.FreezeTimeInside, value => context.Config.FreezeTimeInside = value),
                new CheatsOptionsCheckbox(I18n.Time_FreezeCaves(), context.Config.FreezeTimeCaves, value => context.Config.FreezeTimeCaves = value),
                new CheatsOptionsCheckbox(I18n.Time_FreezeEverywhere(), context.Config.FreezeTime, value => context.Config.FreezeTime = value)
            };
        }

        /// <summary>Handle the cheat options being loaded or changed.</summary>
        /// <param name="context">The cheat context.</param>
        /// <param name="needsUpdate">Whether the cheat should be notified of game updates.</param>
        /// <param name="needsInput">Whether the cheat should be notified of button presses.</param>
        /// <param name="needsRendering">Whether the cheat should be notified of render ticks.</param>
        public override void OnConfig(CheatContext context, out bool needsInput, out bool needsUpdate, out bool needsRendering)
        {
            needsInput = context.Config.FreezeTimeKey.IsBound;
            needsUpdate = needsInput || context.Config.FreezeTime || context.Config.FreezeTimeInside || context.Config.FreezeTimeCaves;
            needsRendering = needsUpdate;
        }

        /// <summary>Handle the player pressing or releasing any buttons if <see cref="ICheat.OnSaveLoaded"/> indicated input was needed.</summary>
        /// <param name="context">The cheat context.</param>
        /// <param name="e">The input event arguments.</param>
        public override void OnButtonsChanged(CheatContext context, ButtonsChangedEventArgs e)
        {
            ModConfig config = context.Config;

            if (config.FreezeTimeKey.JustPressed())
                config.FreezeTime = !config.FreezeTime;
        }

        /// <summary>Handle a game update if <see cref="ICheat.OnSaveLoaded"/> indicated updates were needed.</summary>
        /// <param name="context">The cheat context.</param>
        /// <param name="e">The update event arguments.</param>
        public override void OnUpdated(CheatContext context, UpdateTickedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            if (this.ShouldFreezeTime(context.Config, Game1.currentLocation, out bool _))
                Game1.gameTimeInterval = 0;
        }

        /// <summary>Handle the game draws to the sprite patch in a draw tick, just before the final sprite batch is rendered to the screen.</summary>
        /// <param name="context">The cheat context.</param>
        /// <param name="spriteBatch">The sprite batch being drawn.</param>
        public override void OnRendered(CheatContext context, SpriteBatch spriteBatch)
        {
            if (Game1.displayHUD && !Game1.game1.takingMapScreenshot && this.ShouldFreezeTime(context.Config, Game1.currentLocation, out bool isCave))
                this.RenderTimeFrozenBox(context, spriteBatch, isCave);
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Get whether time should be frozen in the given location.</summary>
        /// <param name="config">The mod configuration.</param>
        /// <param name="location">The location to check.</param>
        /// <param name="isCave">Whether the given location is a cave.</param>
        private bool ShouldFreezeTime(ModConfig config, GameLocation location, out bool isCave)
        {
            isCave = location is MineShaft || location is FarmCave || location is VolcanoDungeon;
            return
                config.FreezeTime
                || (config.FreezeTimeCaves && isCave)
                || (config.FreezeTimeInside && location != null && !location.IsOutdoors && !isCave);
        }

        /// <summary>Render the 'time frozen' box in the top-left.</summary>
        /// <param name="context">The cheat context.</param>
        /// <param name="spriteBatch">The sprite batch being drawn.</param>
        /// <param name="isCave">Whether the given location is a cave.</param>
        private void RenderTimeFrozenBox(CheatContext context, SpriteBatch spriteBatch, bool isCave)
        {
            // get default draw settings
            int x = 5;
            int y = isCave ? 100 : 5;
            SpriteFont font = Game1.smallFont;
            string text = I18n.Time_TimeFrozenMessage();

            // render
            if (Constants.TargetPlatform == GamePlatform.Android)
            {
                if (Game1.activeClickableMenu != null)
                    return;

                bool hasVerticalToolbar = context.Reflection.GetField<bool>(Game1.options, "verticalToolbar").GetValue();
                float nativeZoomLevel = context.Reflection.GetProperty<float>(typeof(Game1), "NativeZoomLevel").GetValue();

                // for Android, draw in a separate sprite batch whose scaling reverses the pinch zoom to keep the text box a stable size
                if (Constants.TargetPlatform == GamePlatform.Android)
                {
                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Matrix.CreateScale(nativeZoomLevel));
                }
                CommonHelper.DrawTab(x: 80 + (hasVerticalToolbar ? 160 : 80), y, font, text);
                if (Constants.TargetPlatform == GamePlatform.Android)
                {
                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone);
                }
            }
            else
                CommonHelper.DrawTab(x, y, font, text);
        }
    }
}
