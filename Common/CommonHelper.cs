using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Locations;
using StardewValley.Menus;
using SObject = StardewValley.Object;

namespace CJB.Common;

/// <summary>Provides common helpers for the mods.</summary>
internal static class CommonHelper
{
    /*********
    ** Accessors
    *********/
    /// <summary>The width of the borders drawn by <c>DrawTab</c>.</summary>
    public const int ButtonBorderWidth = 4 * Game1.pixelZoom;


    /*********
    ** Public methods
    *********/
    /****
    ** UI
    ****/
    /// <summary>Draw a sprite to the screen.</summary>
    /// <param name="batch">The sprite batch.</param>
    /// <param name="sheet">The sprite sheet containing the sprite.</param>
    /// <param name="sprite">The sprite coordinates and dimensions in the sprite sheet.</param>
    /// <param name="x">The X-position at which to draw the sprite.</param>
    /// <param name="y">The X-position at which to draw the sprite.</param>
    /// <param name="width">The width to draw.</param>
    /// <param name="height">The height to draw.</param>
    /// <param name="color">The color to tint the sprite.</param>
    public static void Draw(this SpriteBatch batch, Texture2D sheet, Rectangle sprite, int x, int y, int width, int height, Color? color = null)
    {
        batch.Draw(sheet, new Rectangle(x, y, width, height), sprite, color ?? Color.White);
    }

    /// <summary>Draw a button texture fir the given text to the screen.</summary>
    /// <param name="x">The X position at which to draw.</param>
    /// <param name="y">The Y position at which to draw.</param>
    /// <param name="font">The text font.</param>
    /// <param name="text">The button text.</param>
    /// <param name="align">The button's horizontal alignment relative to <paramref name="x"/>. The possible values are 0 (left), 1 (center), or 2 (right).</param>
    /// <param name="alpha">The button opacity, as a value from 0 (transparent) to 1 (opaque).</param>
    /// <param name="drawShadow">Whether to draw a shadow under the tab.</param>
    public static void DrawTab(int x, int y, SpriteFont font, string text, int align = 0, float alpha = 1, bool drawShadow = true)
    {
        Vector2 bounds = font.MeasureString(text);

        CommonHelper.DrawTab(x, y, (int)bounds.X, (int)bounds.Y, out Vector2 drawPos, align, alpha, drawShadow: drawShadow);
        Game1.spriteBatch.DrawString(font, text, drawPos, Game1.textColor * alpha);
    }

    /// <summary>Draw a button texture to the screen.</summary>
    /// <param name="x">The X position at which to draw.</param>
    /// <param name="y">The Y position at which to draw.</param>
    /// <param name="innerWidth">The width of the button's inner content.</param>
    /// <param name="innerHeight">The height of the button's inner content.</param>
    /// <param name="innerDrawPosition">The position at which the content should be drawn.</param>
    /// <param name="align">The button's horizontal alignment relative to <paramref name="x"/>. The possible values are 0 (left), 1 (center), or 2 (right).</param>
    /// <param name="alpha">The button opacity, as a value from 0 (transparent) to 1 (opaque).</param>
    /// <param name="drawShadow">Whether to draw a shadow under the tab.</param>
    public static void DrawTab(int x, int y, int innerWidth, int innerHeight, out Vector2 innerDrawPosition, int align = 0, float alpha = 1, bool drawShadow = true)
    {
        SpriteBatch spriteBatch = Game1.spriteBatch;

        // calculate outer coordinates
        int outerWidth = innerWidth + CommonHelper.ButtonBorderWidth * 2;
        int outerHeight = innerHeight + Game1.tileSize / 3;
        int offsetX = align switch
        {
            1 => -outerWidth / 2,
            2 => -outerWidth,
            _ => 0
        };

        // draw texture
        IClickableMenu.drawTextureBox(spriteBatch, Game1.menuTexture, new Rectangle(0, 256, 60, 60), x + offsetX, y, outerWidth, outerHeight + Game1.tileSize / 16, Color.White * alpha, drawShadow: drawShadow);
        innerDrawPosition = new Vector2(x + CommonHelper.ButtonBorderWidth + offsetX, y + CommonHelper.ButtonBorderWidth);
    }

    /****
    ** Key bindings
    ****/
    /// <summary>Get whether a current mod keybind press will trigger a game keybind.</summary>
    /// <param name="inputHelper">The SMAPI input API.</param>
    /// <param name="pressedModKeys">The mod's key bind that's currently pressed.</param>
    /// <param name="boundGameKeys">The game's key bindings to check.</param>
    /// <param name="conflictingModKey">The conflicting mod key binding.</param>
    /// <param name="conflictingGameKey">The conflicting game key binding.</param>
    /// <returns>Returns whether the key bindings conflict.</returns>
    public static bool WillThisPressConflict(IInputHelper inputHelper, KeybindList pressedModKeys, InputButton[] boundGameKeys, [NotNullWhen(true)] out Keybind? conflictingModKey, out SButton conflictingGameKey)
    {
        HashSet<SButton> gameKeys = new(boundGameKeys.Select(p => p.ToSButton()));

        foreach (Keybind keybind in pressedModKeys.Keybinds)
        {
            foreach (SButton button in keybind.Buttons)
            {
                if (gameKeys.Contains(button) && inputHelper.IsDown(button))
                {
                    conflictingModKey = keybind;
                    conflictingGameKey = button;
                    return true;
                }
            }
        }

        conflictingModKey = null;
        conflictingGameKey = SButton.None;
        return false;
    }

    /// <summary>Log a warning once if a mod's pressed menu keybind will also trigger the game's "access menu" key, so the mod menu will likely be closed immediately.</summary>
    /// <param name="inputHelper">The SMAPI input API.</param>
    /// <param name="monitor">The SMAPI logging API.</param>
    /// <param name="modKeybind">The mod's key bind that's currently pressed.</param>
    /// <param name="modMenuName">The name of the mod menu (like "item spawner menu" or "cheats menu") for the logged warning.</param>
    public static void WarnOnGameMenuKeyConflict(IInputHelper inputHelper, IMonitor monitor, KeybindList modKeybind, string modMenuName)
    {
        if (CommonHelper.WillThisPressConflict(inputHelper, modKeybind, Game1.options.menuButton, out Keybind? conflictingModKey, out SButton conflictingGameKey))
        {
            string errorPhrase = conflictingModKey.Buttons.Length == 1
                ? $"Your {modMenuName} keybind ({conflictingModKey}) matches the game's \"access menu\" key"
                : $"Your {modMenuName} keybind ({conflictingModKey}) overlaps the game's \"access menu\" key ({conflictingGameKey})";

            monitor.LogOnce($"{errorPhrase}, which won't work correctly. You should change one of the key binds to avoid that.", LogLevel.Warn);
        }
    }

    /****
    ** Game helpers
    ****/
    /// <summary>Get all game locations.</summary>
    public static IEnumerable<GameLocation> GetAllLocations()
    {
        foreach (GameLocation location in Game1.locations)
        {
            // current location
            yield return location;

            // buildings
            foreach (Building building in location.buildings)
            {
                GameLocation indoors = building.indoors.Value;
                if (indoors != null)
                    yield return indoors;
            }
        }
    }

    /// <summary>Get the tile coordinates in a map area.</summary>
    /// <param name="origin">The center tile coordinate.</param>
    /// <param name="radius">The number of tiles in each direction to include, not counting the origin.</param>
    public static IEnumerable<Vector2> GetTileArea(Vector2 origin, int radius)
    {
        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
                yield return new Vector2(origin.X + x, origin.Y + y);
        }
    }

    /// <summary>Get whether the community center is complete.</summary>
    /// <remarks>See game logic in <see cref="Town.resetLocalState"/>.</remarks>
    public static bool GetIsCommunityCenterComplete()
    {
        return Game1.MasterPlayer.mailReceived.Contains("ccIsComplete") || Game1.MasterPlayer.hasCompletedCommunityCenter();
    }

    /// <summary>Get the sell price for an item.</summary>
    /// <param name="item">The item to check.</param>
    /// <param name="forceSellable">Item categories that can be sold in shops, regardless of what <see cref="SObject.canBeShipped"/> returns.</param>
    /// <returns>Returns the sell price, or <c>null</c> if it can't be sold.</returns>
    public static int? GetSellPrice(Item item, ISet<int> forceSellable)
    {
        // skip unsellable item
        if (!CommonHelper.CanBeSold(item, forceSellable))
            return null;

        // get price
        int price = Utility.getSellToStorePriceOfItem(item, countStack: false);
        return price >= 0
            ? price
            : null;
    }

    /// <summary>Get whether an item can be sold.</summary>
    /// <param name="item">The item to check.</param>
    /// <param name="forceSellable">Item categories that can be sold in shops, regardless of what <see cref="SObject.canBeShipped"/> returns.</param>
    public static bool CanBeSold(Item item, ISet<int> forceSellable)
    {
        return
            (item is SObject obj && obj.canBeShipped())
            || forceSellable.Contains(item.Category);
    }


    /****
    ** Math helpers
    ****/
    /// <summary>Get a value's fractional position within a range, as a value between 0 (<paramref name="minValue"/>) and 1 (<paramref name="maxValue"/>).</summary>
    /// <param name="value">The value within the range.</param>
    /// <param name="minValue">The minimum value.</param>
    /// <param name="maxValue">The maximum value.</param>
    public static float GetRangePosition(int value, int minValue, int maxValue)
    {
        float position = (value - minValue) / (float)(maxValue - minValue);
        return MathHelper.Clamp(position, 0, 1);
    }

    /// <summary>Get the value from a position within a range.</summary>
    /// <param name="position">The position within the range, where 0 is the minimum value and 1 is the maximum value.</param>
    /// <param name="minValue">The minimum value.</param>
    /// <param name="maxValue">The maximum value.</param>
    public static int GetValueAtPosition(float position, int minValue, int maxValue)
    {
        float value = position * (maxValue - minValue) + minValue;
        return (int)MathHelper.Clamp(value, minValue, maxValue);
    }


    /****
    ** File handling
    ****/
    /// <summary>Remove one or more obsolete files from the mod folder, if they exist.</summary>
    /// <param name="mod">The mod for which to delete files.</param>
    /// <param name="relativePaths">The relative file path within the mod's folder.</param>
    public static void RemoveObsoleteFiles(IMod mod, params string[] relativePaths)
    {
        string basePath = mod.Helper.DirectoryPath;

        foreach (string relativePath in relativePaths)
        {
            string fullPath = Path.Combine(basePath, relativePath);
            if (File.Exists(fullPath))
            {
                try
                {
                    File.Delete(fullPath);
                    mod.Monitor.Log($"Removed obsolete file '{relativePath}'.");
                }
                catch (Exception ex)
                {
                    mod.Monitor.Log($"Failed deleting obsolete file '{relativePath}':\n{ex}");
                }
            }
        }
    }
}
