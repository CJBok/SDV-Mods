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

namespace CJBCheatsMenu.Framework.Cheats.Time;

/// <summary>A cheat which stops the passage of time.</summary>
internal class FreezeTimeCheat : BaseCheat
{
    /*********
    ** Fields
    *********/
    /// <summary>The number of milliseconds for which to show time-frozen messages on screen.</summary>
    private const int FrozenMessageDefaultTime = 1000;

    /// <summary>The opacity to reduce per tick when the time-frozen message is fading.</summary>
    private const float FrozenMessageFadeRate = 0.01f;

    /// <summary>The last location name for which the time-frozen message was shown.</summary>
    private string? FrozenMessageShownFor;

    /// <summary>The number of milliseconds until the time-frozen message should start fading.</summary>
    private int FrozenMessageTime;

    /// <summary>The opacity at which to draw the time-frozen message, as a value between 0 (transparent) and 1 (opaque).</summary>
    private float FrozenMessageAlpha;


    /*********
    ** Public methods
    *********/
    /// <inheritdoc />
    public override IEnumerable<OptionsElement> GetFields(CheatContext context)
    {
        return
        [
            new CheatsOptionsCheckbox(I18n.Time_FreezeInside(), context.Config.FreezeTimeInside, value => context.Config.FreezeTimeInside = value),
            new CheatsOptionsCheckbox(I18n.Time_FreezeCaves(), context.Config.FreezeTimeCaves, value => context.Config.FreezeTimeCaves = value),
            new CheatsOptionsCheckbox(I18n.Time_FreezeEverywhere(), context.Config.FreezeTime, value => context.Config.FreezeTime = value),
            new CheatsOptionsCheckbox(I18n.Time_FadeTimeFrozenMessage(I18n.Time_TimeFrozenMessage()), context.Config.FadeTimeFrozenMessage, value => context.Config.FadeTimeFrozenMessage = value)
        ];
    }

    /// <inheritdoc />
    public override void OnConfig(CheatContext context, out bool needsInput, out bool needsUpdate, out bool needsRendering)
    {
        needsInput = context.Config.FreezeTimeKey.IsBound;
        needsUpdate = needsInput || context.Config.FreezeTime || context.Config.FreezeTimeInside || context.Config.FreezeTimeCaves;
        needsRendering = needsUpdate;
    }

    /// <inheritdoc />
    public override void OnButtonsChanged(CheatContext context, ButtonsChangedEventArgs e)
    {
        ModConfig config = context.Config;

        if (config.FreezeTimeKey.JustPressed())
        {
            config.FreezeTime = !config.FreezeTime;

            Game1.playSound(config.FreezeTime ? "drumkit6" : "breathin");
        }
    }

    /// <inheritdoc />
    public override void OnUpdated(CheatContext context, UpdateTickedEventArgs e)
    {
        if (!Context.IsWorldReady || Game1.currentLocation is null)
            return;

        bool fadeMessage = context.Config.FadeTimeFrozenMessage;

        if (this.ShouldFreezeTime(context.Config, Game1.currentLocation, out bool _))
        {
            Game1.gameTimeInterval = 0;

            if (!fadeMessage || this.FrozenMessageShownFor != Game1.currentLocation.NameOrUniqueName)
            {
                this.FrozenMessageShownFor = Game1.currentLocation.NameOrUniqueName;
                this.FrozenMessageTime = FreezeTimeCheat.FrozenMessageDefaultTime;
                this.FrozenMessageAlpha = 1f;
            }
        }
        else if (fadeMessage && this.FrozenMessageShownFor != null)
        {
            this.FrozenMessageShownFor = null;
            this.FrozenMessageTime = 0;
            this.FrozenMessageAlpha = 0;
        }
    }

    /// <inheritdoc />
    public override void OnRendered(CheatContext context, SpriteBatch spriteBatch)
    {
        if ((this.FrozenMessageTime > 0 || this.FrozenMessageAlpha > 0) && this.ShouldFreezeTime(context.Config, Game1.currentLocation, out bool isCave))
        {
            if (Game1.displayHUD && !Game1.game1.takingMapScreenshot)
                this.RenderTimeFrozenBox(context, spriteBatch, isCave);

            if (context.Config.FadeTimeFrozenMessage)
            {
                if (this.FrozenMessageTime > 0)
                    this.FrozenMessageTime -= (int)Game1.currentGameTime.ElapsedGameTime.TotalMilliseconds;
                else
                    this.FrozenMessageAlpha -= FreezeTimeCheat.FrozenMessageFadeRate;
            }
        }
    }


    /*********
    ** Private methods
    *********/
    /// <summary>Get whether time should be frozen in the given location.</summary>
    /// <param name="config">The mod configuration.</param>
    /// <param name="location">The location to check.</param>
    /// <param name="isCave">Whether the given location is a cave.</param>
    private bool ShouldFreezeTime(ModConfig config, GameLocation? location, out bool isCave)
    {
        isCave = location is MineShaft or FarmCave or VolcanoDungeon;
        bool isInside =
            !isCave
            && location?.IsOutdoors == false
            && location is not BugLand; // incorrectly marked as indoors

        return
            config.FreezeTime
            || (config.FreezeTimeCaves && isCave)
            || (config.FreezeTimeInside && isInside);
    }

    /// <summary>Render the 'time frozen' box in the top-left.</summary>
    /// <param name="context">The cheat context.</param>
    /// <param name="spriteBatch">The sprite batch being drawn.</param>
    /// <param name="isCave">Whether the given location is a cave.</param>
    private void RenderTimeFrozenBox(CheatContext context, SpriteBatch spriteBatch, bool isCave)
    {
        // get default draw settings
        int x = 5;
        int y = isCave || Game1.currentLocation?.Name is "Club"
            ? 100
            : 5;
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
            CommonHelper.DrawTab(x: 80 + (hasVerticalToolbar ? 160 : 80), y, font, text, alpha: this.FrozenMessageAlpha);
            if (Constants.TargetPlatform == GamePlatform.Android)
            {
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone);
            }
        }
        else
            CommonHelper.DrawTab(x, y, font, text, alpha: this.FrozenMessageAlpha);
    }
}
