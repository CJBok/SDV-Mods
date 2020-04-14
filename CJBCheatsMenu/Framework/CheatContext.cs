using System;
using System.Collections.Generic;
using CJBCheatsMenu.Framework.Models;
using StardewModdingAPI;
using StardewValley;

namespace CJBCheatsMenu.Framework
{
    /// <summary>Context metadata available to cheat implementations.</summary>
    internal class CheatContext
    {
        /*********
        ** Fields
        *********/
        /// <summary>Get a cached list of all in-game locations.</summary>
        private readonly Func<IEnumerable<GameLocation>> GetAllLocationsImpl;


        /*********
        ** Accessors
        *********/
        /// <summary>The mod configuration.</summary>
        public ModConfig Config { get; }

        /// <summary>Simplifies access to private code.</summary>
        public IReflectionHelper Reflection { get; }

        /// <summary>Provides translations stored in the mod folder.</summary>
        public ITranslationHelper Text { get; }

        /// <summary>The display width of an option slot during the last cheats menu render.</summary>
        public int SlotWidth { get; set; }


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="config">The mod configuration.</param>
        /// <param name="reflection">Simplifies access to private code.</param>
        /// <param name="text">Provides translations stored in the mod folder.</param>
        /// <param name="getAllLocations">Get a cached list of all in-game locations.</param>
        public CheatContext(ModConfig config, IReflectionHelper reflection, ITranslationHelper text, Func<IEnumerable<GameLocation>> getAllLocations)
        {
            this.Config = config;
            this.Reflection = reflection;
            this.Text = text;
            this.GetAllLocationsImpl = getAllLocations;
        }

        /// <summary>Get all in-game locations.</summary>
        public IEnumerable<GameLocation> GetAllLocations()
        {
            return this.GetAllLocationsImpl();
        }
    }
}
