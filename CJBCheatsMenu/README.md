**CJB Cheats Menu** is a [Stardew Valley](http://stardewvalley.net/) mod that adds an in-game menu
with many different cheat features:

![](screenshot.gif)

Compatible with Stardew Valley 1.4+ on Linux, Mac, and Windows.

## Install
1. [Install the latest version of SMAPI](https://smapi.io/).
2. [Install this mod from Nexus mods](http://www.nexusmods.com/stardewvalley/mods/4).
3. Run the game using SMAPI.

## Use
* Press `P` (configurable) to show the cheats menu.
* You can also enable active cheats by editing the `config.json` file.

## Configure
The mod will work fine out of the box, but you can tweak its settings by editing the `config.json`
file. You can also edit these values through the in-game settings menu. These are the available
settings.

### Key bindings
(See [valid key bindings](https://stardewvalleywiki.com/Modding:Player_Guide/Key_Bindings).)

setting | default | what it affects
------- | ------- | ---------------
`OpenMenuKey` | `P` | The button which opens the menu.
`FreezeTimeKey` | `T` | The button which freezes the game clock.
`GrowTreeKey` | `NumPad1` | The button held to grow trees around the player.
`GrowCropsKey` | `NumPad2` | The button held to grow crops around the player.
`GrowRadius` | `1` | The number of tiles in each direction around the player to cover when pressing the `GrowCropsKey` or `GrowTreeKey`.

### Menu settings

setting | default | what it affects
------- | ------- | ---------------
`DefaultTab` | `PlayerAndTools` | The tab shown by default when you open the menu. Possible values: `PlayerAndTools`, `FarmAndFishing`, `Skills`, `Weather`, `Relationships`, `WarpLocations`, `Time`, `Controls`.

### Player cheats
setting | default | what it affects
------- | ------- | ---------------
`MoveSpeed` | `1` | The player speed buff to add.
`InfiniteHealth` | `false` | Your health never decreases.
`InfiniteStamina` | `false` | Your stamina never decreases.
`MaxDailyLuck` | `false` | Your daily luck is always at the maximum value.
`OneHitKill` | `false` | Your attacks kill any monster in one hit.
`OneHitBreak` | `false` | Your tools break things instantly.
`InfiniteWateringCan` | `false` | Your watering can never runs dry.
`HarvestScythe` | `false` | You can harvest any crop with the scythe.

### Fishing cheats
setting | default | what it affects
------- | ------- | ---------------
`InstantBite` | `false` | After casting the fishing line, the fishing minigame appears immediately.
`InstantCatch` | `false` | When the fishing minigame appears, the fish is caught immediately.
`ThrowBobberMax` | `false` | When casting the fishing line, it always reaches the maximum distance.
`DurableTackles` | `false` | Fishing tackles never break.
`AlwaysTreasure` | `false` | Every fishing minigame has a treasure.

### Time cheats
setting | default | what it affects
------- | ------- | ---------------
`FreezeTime` | `false` | The game clock never changes.
`FreezeTimeInside` | `false` | The game clock doesn't change when you're inside a building.
`FreezeTimeCaves` | `false` | The game clock doesn't change when you're inside the mines, Skull Cavern, or farm cave.
`FastCask` | `false` | Casks finish instantly.
`FastFruitTrees` | `false` | Fruit trees produce fruit instantly.
`FastFurnace` | `false` | Furnaces finish instantly.
`FastRecyclingMachine` | `false` | Recycling machines finish instantly.
`FastCrystalarium` | `false` | Crystalariums finish instantly.
`FastIncubator` | `false` | Egg incubators finish overnight.
`FastSlimeIncubator ` | `false` | Slime incubators finish instantly.
`FastKeg` | `false` | Kegs finish instantly.
`FastPreservesJar` | `false` | Preserves jars finish instantly.
`FastCheesePress` | `false` | Cheese presses finish instantly.
`FastMayonnaiseMachine ` | `false` | Mayonnaise machines finish instantly.
`FastLoom` | `false` | Looms finish instantly.
`FastOilMaker` | `false` | Oil makers finish instantly.
`FastSeedMaker` | `false` | Seed makers finish instantly.
`FastCharcoalKiln` | `false` | Charcoal kilns finish instantly.
`FastSlimeEggPress` | `false` | Slime egg presss finish instantly.
`FastBeeHouse` | `false` | Beehouses finish instantly.
`FastMushroomBox` | `false` | Mushroom boxs finish instantly.
`FastTapper` | `false` | Tappers finish instantly.
`FastLightningRod` | `false` | Lightning rods finish instantly.
`FastWormBin` | `false` | Worm bins finish instantly.

### Other cheats
setting | default | what it affects
------- | ------- | ---------------
`AlwaysGiveGift` | `false` | You can always give gifts to villagers, regardless of the daily and weekly limits.
`NoFriendshipDecay` | `false` | A villager's friendship value no longer slowly decays if it isn't maxed out.
`DurableFences` | `false` | Fences never break.
`InstantBuild` | `false` | Building new structures on the farm completes instantly.
`AutoFeed` | `false` | Feed troughs in barns and coops are refilled automatically.
`AutoWater` | `false` | Crops are watered automatically.
`InfiniteHay` | `false` | Hay silos are always full.

## Warp format
The Warps tab is populated based on the `assets/warps.json` file. This isn't generally meant to be
edited by players, but you can edit it to customize the warps shown in the menu.

### Warps
The `Warps` field defines the warps shown in the menu. The order they're listed in doesn't matter;
they'll be sorted alphabetically by section and display name.

Warps are grouped by the section under which to list the warp in the menu. This can match a
translation ID in the i18n files, else it'll be shown as-is. See also [_section order_](#section-order).

Each warp has four main fields:

field | purpose
----- | -------
`DisplayText` | The text to show in the menu. This can match a translation ID in the i18n files, else it'll be shown as-is.
`Location` | The internal name of the target location (not the translated name). You can use the [Debug Mode](https://www.nexusmods.com/stardewvalley/mods/679) mod to see location names in-game.
`Tile` | The target tile coordinate. You can use the [Debug Mode](https://www.nexusmods.com/stardewvalley/mods/679) mod to see tile coordinates in-game.
`Order` | The relative order in which to list it in the warp menu (default 0). Lower values are listed first. Warps with the same order are sorted alphabetically.

### Section order
Warps are grouped into UI sections for easier navigation (see [_warps_](#warps)). The `SectionOrder`
field lists sections that should be listed at the top in the listed order. Any other sections will
be listed alphabetically after these sections.

### Special warp behavior
Warps can set the `SpecialBehavior` field to enable special warp logic. The valid values are:

value             | effect
----------------- | ------
`Casino`          | Hide warp if the player doesn't have the club card.
`Farm`            | Warp to the current player's cabin or farmhouse. The `Location` and `Tile` fields are ignored.
## See also
* [Release notes](release-notes.md)
