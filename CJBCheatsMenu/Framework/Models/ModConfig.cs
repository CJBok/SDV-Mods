using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using CJBCheatsMenu.Framework.ContentModels;
using Newtonsoft.Json;
using StardewModdingAPI;
using StardewModdingAPI.Utilities;

namespace CJBCheatsMenu.Framework.Models;

/// <summary>The mod configuration model.</summary>
internal class ModConfig
{
    /*********
    ** Accessors
    *********/
    /// <summary>The default values.</summary>
    public static ModConfig Defaults { get; } = new();

    /****
    ** Keyboard buttons
    ****/
    /// <summary>The button which opens the menu.</summary>
    public KeybindList OpenMenuKey { get; set; } = new(SButton.P);

    /// <summary>The button which causes the mod settings to be reloaded.</summary>
    public KeybindList ReloadConfigKey { get; set; } = new();

    /// <summary>The button which freezes the game clock.</summary>
    public KeybindList FreezeTimeKey { get; set; } = new();

    /// <summary>The button held to grow trees around the player.</summary>
    public KeybindList GrowTreeKey { get; set; } = new(SButton.NumPad1);

    /// <summary>The button held to grow crops around the player.</summary>
    public KeybindList GrowCropsKey { get; set; } = new(SButton.NumPad2);

    /// <summary>The number of tiles in each direction around the player to cover when pressing <see cref="GrowCropsKey"/> or <see cref="GrowTreeKey"/>.</summary>
    public int GrowRadius { get; set; } = 1;

    /****
    ** Menu settings
    ****/
    /// <summary>The tab shown by default when you open the menu.</summary>
    public MenuTab DefaultTab { get; set; } = MenuTab.PlayerAndTools;

    /****
    ** Player cheats
    ****/
    /// <summary>The player speed buff to add.</summary>
    public int MoveSpeed { get; set; }

    /// <summary>The player's health never decreases.</summary>
    public bool InfiniteHealth { get; set; }

    /// <summary>The player's stamina never decreases.</summary>
    public bool InfiniteStamina { get; set; }

    /// <summary>Tool and weapon cooldowns are instant.</summary>
    public bool InstantCooldowns { get; set; }

    /// <summary>The player's daily luck is always at the maximum value.</summary>
    public bool MaxDailyLuck { get; set; }

    /// <summary>The player's attacks kill any monster in one hit.</summary>
    public bool OneHitKill { get; set; }

    /// <summary>The player's tools break things instantly.</summary>
    public bool OneHitBreak { get; set; }

    /// <summary>The player's watering can never runs dry.</summary>
    public bool InfiniteWateringCan { get; set; }

    /// <summary>The player can harvest any crop with the scythe.</summary>
    public bool HarvestScythe { get; set; }

    /****
    ** Fishing cheats
    ****/
    /// <summary>After casting the fishing line, the fishing minigame appears immediately.</summary>
    public bool InstantBite { get; set; }

    /// <summary>When the fishing minigame appears, the fish is caught immediately.</summary>
    public bool InstantCatch { get; set; }

    /// <summary>When casting the fishing line, it always reaches the maximum distance.</summary>
    public bool ThrowBobberMax { get; set; }

    /// <summary>Fishing tackles never break.</summary>
    public bool DurableTackles { get; set; }

    /// <summary>Every fishing minigame has a treasure.</summary>
    public bool AlwaysTreasure { get; set; }

    /****
    ** Time cheats
    ****/
    /// <summary>The game clock never changes.</summary>
    public bool FreezeTime { get; set; }

    /// <summary>The game clock doesn't change when you're inside a building.</summary>
    public bool FreezeTimeInside { get; set; }

    /// <summary>The game clock doesn't change when you're inside the mines, Skull Cavern, or farm cave.</summary>
    public bool FreezeTimeCaves { get; set; }

    /// <summary>Whether the 'time frozen' message should fade away after a few seconds.</summary>
    public bool FadeTimeFrozenMessage { get; set; } = false;

    /// <summary>Whether fruit trees bear fruit instantly.</summary>
    public bool FastFruitTree { get; set; }

    /// <summary>The building machines which finish instantly, indexed by building type.</summary>
    public HashSet<string> FastBuildings { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>The item machines which finish instantly, indexed by qualified item ID.</summary>
    public HashSet<string> FastMachines { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    /****
    ** Custom warps
    ****/
    /// <summary>The warp section IDs to hide when building the warp list. This can contain wildcard patterns.</summary>
    public HashSet<string> HideWarpSections { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>The warp IDs to hide when building the warp list. This can contain wildcard patterns.</summary>
    public HashSet<string> HideWarps { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>The custom warp sections to add to the list.</summary>
    public List<WarpSectionContentModel> AddWarpSections { get; set; } = [];

    /// <summary>The custom warps to add to the list.</summary>
    public List<WarpContentModel> AddWarps { get; set; } = [];

    /****
    ** Other cheats
    ****/
    /// <summary>The player can always give gifts to villagers, regardless of the daily and weekly limits.</summary>
    public bool AlwaysGiveGift { get; set; }

    /// <summary>A villager's friendship value no longer slowly decays if it isn't maxed out.</summary>
    public bool NoFriendshipDecay { get; set; }

    /// <summary>Fences never break.</summary>
    public bool DurableFences { get; set; }

    /// <summary>Building new structures on the farm completes instantly.</summary>
    public bool InstantBuild { get; set; }

    /// <summary>Feed troughs in your barns and coops are refilled automatically.</summary>
    public bool AutoFeed { get; set; }

    /// <summary>Farm animals are pet automatically.</summary>
    public bool AutoPetAnimals { get; set; }

    /// <summary>Pet animals are pet automatically.</summary>
    public bool AutoPetPets { get; set; }

    /// <summary>Crops are watered automatically.</summary>
    public bool AutoWater { get; set; }

    /// <summary>Hay silos are always full.</summary>
    public bool InfiniteHay { get; set; }

    /// <summary>The JSON data fields which don't have a corresponding property.</summary>
    [JsonExtensionData]
    public Dictionary<string, object>? ExtensionData { get; set; }


    /*********
    ** Public methods
    *********/
    /// <summary>Normalize the model after it's deserialized.</summary>
    /// <param name="context">The deserialization context.</param>
    [OnDeserialized]
    [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract", Justification = "This is the method which ensures the annotations are correct.")]
    public void OnDeserialized(StreamingContext context)
    {
        // make sets case-insensitive
        this.FastBuildings = this.ToCaseInsensitive(this.FastBuildings);
        this.FastMachines = this.ToCaseInsensitive(this.FastMachines);
        this.HideWarpSections = this.ToCaseInsensitive(this.HideWarpSections);
        this.HideWarps = this.ToCaseInsensitive(this.HideWarps);

        // migrate pre-1.35 fast machine options
        if (this.ExtensionData != null)
        {
            foreach ((string name, object value) in this.ExtensionData)
            {
                // fast buildings
                if (name == "FastMillMachine")
                {
                    this.FastBuildings.Add("Mill");
                    continue;
                }

                // fast machines
                string? machineItemId = name switch
                {
                    "FastBeeHouse" => "(BC)10",
                    "FastBoneMill" => "(BC)90",
                    "FastCask" => "(BC)163",
                    "FastCharcoalKiln" => "(BC)114",
                    "FastCheesePress" => "(BC)16",
                    "FastCoffeeMaker" => "(BC)246",
                    "FastCrabPot" => "(O)710",
                    "FastCrystalarium" => "(BC)21",
                    "FastDeconstructor" => "(BC)265",
                    "FastFurnace" => "(BC)13",
                    "FastGeodeCrusher" => "(BC)182",
                    "FastIncubator" => "(BC)101",
                    "FastKeg" => "(BC)12",
                    "FastLightningRod" => "(BC)9",
                    "FastLoom" => "(BC)17",
                    "FastMayonnaiseMachine" => "(BC)24",
                    "FastMushroomBox" => "(BC)128",
                    "FastOilMaker" => "(BC)19",
                    "FastOstrichIncubator" => "(BC)254",
                    "FastPreservesJar" => "(BC)15",
                    "FastRecyclingMachine" => "(BC)20",
                    "FastSeedMaker" => "(BC)25",
                    "FastSlimeEggPress" => "(BC)158",
                    "FastSlimeIncubator" => "(BC)156",
                    "FastSodaMachine" => "(BC)117",
                    "FastSolarPanel" => "(BC)231",
                    "FastStatueOfEndlessFortune" => "(BC)127",
                    "FastStatueOfPerfection" => "(BC)160",
                    "FastStatueOfTruePerfection" => "(BC)280",
                    "FastTapper" => "(BC)105",
                    "FastWoodChipper" => "(BC)211",
                    "FastWormBin" => "(BC)154",
                    _ => null
                };

                if (machineItemId is not null && value is bool enabled && enabled)
                    this.FastMachines.Add(machineItemId);
            }

            this.ExtensionData = null;
        }
    }

    /// <summary>Get a case-insensitive hash set containing the given data.</summary>
    /// <param name="set">The data to add to the set, or <c>null</c> to create an empty set.</param>
    private HashSet<string> ToCaseInsensitive(HashSet<string>? set)
    {
        return set is not null
            ? new HashSet<string>(set, StringComparer.OrdinalIgnoreCase)
            : new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    }
}
