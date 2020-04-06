using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CJBCheatsMenu.Framework.Cheats;
using CJBCheatsMenu.Framework.Cheats.Advanced;
using CJBCheatsMenu.Framework.Cheats.FarmAndFishing;
using CJBCheatsMenu.Framework.Cheats.PlayerAndTools;
using CJBCheatsMenu.Framework.Cheats.Relationships;
using CJBCheatsMenu.Framework.Cheats.Skills;
using CJBCheatsMenu.Framework.Cheats.Time;
using CJBCheatsMenu.Framework.Cheats.Warps;
using CJBCheatsMenu.Framework.Cheats.Weather;
using CJBCheatsMenu.Framework.Models;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace CJBCheatsMenu.Framework
{
    /// <summary>Manages the cheat implementations.</summary>
    internal class CheatManager
    {
        /*********
        ** Fields
        *********/
        /// <summary>The available cheat implementations.</summary>
        private readonly ICheat[] Cheats;

        /// <summary>The cheat implementations which should be notified of update ticks.</summary>
        private readonly List<ICheat> CheatsWhichNeedUpdate = new List<ICheat>();

        /// <summary>The cheat implementations which should be notified of user input.</summary>
        private readonly List<ICheat> CheatsWhichNeedInput = new List<ICheat>();

        /// <summary>The cheat implementations which should be notified of render ticks.</summary>
        private readonly List<ICheat> CheatsWhichNeedRendering = new List<ICheat>();

        /// <summary>The backing field for <see cref="NoFriendshipDecay"/>.</summary>
        private readonly NoFriendshipDecayCheat NoFriendshipDecayImpl = new NoFriendshipDecayCheat();


        /*********
        ** Accessors
        *********/
        /// <summary>The cheat context.</summary>
        public CheatContext Context { get; }

        /****
        ** Player & tools
        ****/
        /// <summary>Enables infinite health.</summary>
        public ICheat InfiniteHealth { get; } = new InfiniteHealthCheat();

        /// <summary>Enables infinite stamina.</summary>
        public ICheat InfiniteStamina { get; } = new InfiniteStaminaCheat();

        /// <summary>Enables instant weapon cooldowns.</summary>
        public ICheat InstantCooldowns { get; } = new InstantCooldownCheat();

        /// <summary>Enables one-hit kill.</summary>
        public ICheat OneHitKill { get; } = new OneHitKillCheat();

        /// <summary>Sets the daily luck to its maximum value.</summary>
        public ICheat MaxDailyLuck { get; } = new MaxDailyLuckCheat();

        /// <summary>Increases the player movement speed.</summary>
        public ICheat MoveSpeed { get; } = new MoveSpeedCheat();

        /// <summary>Sets the inventory size.</summary>
        public ICheat InventorySize { get; } = new InventorySizeCheat();

        /// <summary>Harvests all crops with the scythe.</summary>
        public ICheat HarvestWithScythe { get; } = new HarvestWithScytheCheat();

        /// <summary>Enables infinite water in watering cans.</summary>
        public ICheat InfiniteWater { get; } = new InfiniteWaterCheat();

        /// <summary>Enables one-hit break.</summary>
        public ICheat OneHitBreak { get; } = new OneHitBreakCheat();

        /// <summary>Adds various amounts of money to the player.</summary>
        public ICheat AddMoney { get; } = new AddMoneyCheat();

        /// <summary>Adds various numbers of casino coins to the player.</summary>
        public ICheat AddCasinoCoins { get; } = new AddCasinoCoinsCheat();

        /****
        ** Farming & fishing
        ****/
        /// <summary>Automatically waters all crops.</summary>
        public ICheat AutoWater { get; } = new AutoWaterCheat();

        /// <summary>Gives fences infinite health.</summary>
        public ICheat DurableFences { get; } = new DurableFencesCheat();

        /// <summary>Makes building construction complete instantly.</summary>
        public ICheat InstantBuild { get; } = new InstantBuildCheat();

        /// <summary>Automatically fills animal feed troughs.</summary>
        public ICheat AlwaysAutoFeed { get; } = new AlwaysAutoFeedCheat();

        /// <summary>Enables infinite hay.</summary>
        public ICheat InfiniteHay { get; } = new InfiniteHayCheat();

        /// <summary>Enables instant catches when fishing.</summary>
        public ICheat InstantFishCatch { get; } = new InstantFishCatchCheat();

        /// <summary>Enables instant bites when fishing.</summary>
        public ICheat InstantFishBite { get; } = new InstantFishBiteCheat();

        /// <summary>Always casts the fishing rod at the maximum distance.</summary>
        public ICheat AlwaysCastMaxDistance { get; } = new AlwaysCastMaxDistanceCheat();

        /// <summary>Always catches treasure when fishing.</summary>
        public ICheat AlwaysFishTreasure { get; } = new AlwaysFishTreasureCheat();

        /// <summary>Gives fishing tackles infinite endurance.</summary>
        public ICheat DurableFishTackles { get; } = new DurableFishTacklesCheat();

        /// <summary>Makes machines complete their output instantly.</summary>
        public ICheat FastMachines { get; } = new FastMachinesCheat();

        /****
        ** Skills
        ****/
        /// <summary>Increases or resets skill levels.</summary>
        public ICheat Skills { get; } = new SkillsCheat();

        /// <summary>Toggles player professions.</summary>
        public ICheat Professions { get; } = new ProfessionsCheat();

        /****
        ** Weather
        ****/
        /// <summary>Sets the weather for tomorrow.</summary>
        public ICheat SetWeatherForTomorrow { get; } = new SetWeatherForTomorrowCheat();

        /****
        ** Relationships
        ****/
        /// <summary>Always allows giving gifts to NPCs.</summary>
        public ICheat AlwaysGiveGifts { get; } = new AlwaysGiveGiftsCheat();

        /// <summary>Prevents NPC friendships from decaying.</summary>
        public ICheat NoFriendshipDecay => this.NoFriendshipDecayImpl;

        /// <summary>Sets the heart levels for social NPCs.</summary>
        public ICheat Hearts { get; }

        /****
        ** Warps
        ****/
        /// <summary>Warps the player to selected locations.</summary>
        public ICheat Warps { get; }

        /****
        ** Time
        ****/
        /// <summary>Stops the passage of time.</summary>
        public ICheat FreezeTime { get; } = new FreezeTimeCheat();

        /// <summary>Sets the current time.</summary>
        public ICheat SetTime { get; } = new SetTimeCheat();

        /****
        ** Advanced
        ****/
        /// <summary>Completes open quests.</summary>
        public ICheat Quests { get; } = new QuestsCheat();

        /// <summary>Toggles wallet items.</summary>
        public ICheat WalletItems { get; } = new WalletItemsCheat();

        /// <summary>Unlocks a locked door.</summary>
        public ICheat UnlockDoor { get; } = new UnlockDoorCheat();

        /// <summary>Unlocks locked game content.</summary>
        public ICheat UnlockContent { get; } = new UnlockContentCheat();

        /// <summary>Toggles community center and JojaMart bundles.</summary>
        public ICheat Bundles { get; } = new BundlesCheat();

        /****
        ** Other
        ****/
        /// <summary>Grows crops and trees under the cursor.</summary>
        [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Used via reflection")]
        public ICheat Grow { get; } = new GrowCheat();


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="config">The mod configuration.</param>
        /// <param name="reflection">Simplifies access to private code.</param>
        /// <param name="text">Provides translations stored in the mod folder.</param>
        /// <param name="getAllLocations">Get a cached list of all in-game locations.</param>
        /// <param name="warps">The available warps.</param>
        public CheatManager(ModConfig config, IReflectionHelper reflection, ITranslationHelper text, Func<IEnumerable<GameLocation>> getAllLocations, ModData warps)
        {
            this.Context = new CheatContext(config, reflection, text, getAllLocations);
            this.Hearts = new HeartsCheat(onPointsChanged: (npc, points) => this.NoFriendshipDecayImpl.UpdateFriendship(npc, points));
            this.Warps = new WarpCheat(warps);

            this.Cheats = this
                .GetType()
                .GetProperties()
                .Select(prop => prop.GetValue(this))
                .OfType<ICheat>()
                .ToArray();

            this.OnOptionsChanged();
        }

        /// <summary>Reset all tracked data.</summary>
        public void OnSaveLoaded()
        {
            foreach (ICheat cheat in this.Cheats)
                cheat.OnSaveLoaded(this.Context);
        }

        /// <summary>Perform any action needed after the cheat options change.</summary>
        public void OnOptionsChanged()
        {
            // update cheats
            this.CheatsWhichNeedUpdate.Clear();
            this.CheatsWhichNeedInput.Clear();
            this.CheatsWhichNeedRendering.Clear();
            foreach (ICheat cheat in this.Cheats)
            {
                cheat.OnConfig(this.Context, out bool needsInput, out bool needsUpdate, out bool needsRendering);
                if (needsInput)
                    this.CheatsWhichNeedInput.Add(cheat);
                if (needsUpdate)
                    this.CheatsWhichNeedUpdate.Add(cheat);
                if (needsRendering)
                    this.CheatsWhichNeedRendering.Add(cheat);
            }
        }

        /// <summary>Raised after the game draws to the sprite patch in a draw tick, just before the final sprite batch is rendered to the screen.</summary>
        public void OnRendered()
        {
            foreach (ICheat cheat in this.CheatsWhichNeedRendering)
                cheat.OnRendered(this.Context, Game1.spriteBatch);
        }

        /// <summary>Raised after the game state is updated (≈60 times per second).</summary>
        /// <param name="e">The event arguments.</param>
        public void OnUpdateTicked(UpdateTickedEventArgs e)
        {
            foreach (ICheat cheat in this.CheatsWhichNeedUpdate)
                cheat.OnUpdated(this.Context, e);
        }

        /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
        /// <param name="e">The event arguments.</param>
        public void OnButtonPressed(ButtonPressedEventArgs e)
        {
            foreach (ICheat cheat in this.CheatsWhichNeedInput)
                cheat.OnButtonPressed(this.Context, e);
        }

        /// <summary>Raised after the player releases a button on the keyboard, controller, or mouse.</summary>
        /// <param name="e">The event arguments.</param>
        public void OnButtonReleased(ButtonReleasedEventArgs e)
        {
            foreach (ICheat cheat in this.CheatsWhichNeedInput)
                cheat.OnButtonReleased(this.Context, e);
        }
    }
}
