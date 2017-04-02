**CJB Cheats Menu** is a [Stardew Valley](http://stardewvalley.net/) mod that adds an in-game menu
with many different cheat features:

![](screenshot.gif)

Compatible with Stardew Valley 1.1+ on Linux, Mac, and Windows.

## Usage
* Press `P` (configurable) to show the cheats menu.
* You can also enable active cheats by editing the `config.json` file.

## Installation
1. [Install the latest version of SMAPI](https://github.com/Pathoschild/SMAPI/releases).
2. [Install this mod from Nexus mods](http://www.nexusmods.com/stardewvalley/mods/4).
3. Run the game using SMAPI.

## Configuration
The mod will work fine out of the box, but you can tweak its settings by editing the `config.json`
file. You can also edit these values through the in-game settings menu. These are the available
settings.

### Keyboard buttons
(See [valid keyboard buttons](https://msdn.microsoft.com/en-us/library/microsoft.xna.framework.input.keys.aspx).)

setting | default | what it affects
------- | ------- | ---------------
`openMenuKey` | `P` | The keyboard button which opens the menu.
`freezeTimeKey` | `T` | The keyboard button which freezes the game clock.
`growTreeKey` | `NumPad1` | The keyboard button which instantly grows the tree under the tool cursor.
`growCropsKey` | `NumPad2` | The keyboard button which instantly grows crops adjacent to your character.

### Player cheats
setting | default | what it affects
------- | ------- | ---------------
`increasedMovement` | `false` | Whether your character moves faster. The added speed is set by the `moveSpeed` field.
`moveSpeed` | `1` | The player speed to add if `increasedMovement` is `true`. This is an added multiplier (e.g. `1` doubles the default speed).
`infiniteHealth` | `false` | Your health never decreases.
`infiniteStamina` | `false` | Your stamina never decreases.
`maxDailyLuck` | `false` | Your daily luck is always at the maximum value.
`oneHitKill` | `false` | Your attacks kill any monster in one hit.
`oneHitBreak` | `false` | Your tools break things instantly.
`infiniteWateringCan` | `false` | Your watering can never runs dry.
`harvestSickle` | `false` | You can harvest any crop with the sickle.

### Fishing cheats
setting | default | what it affects
------- | ------- | ---------------
`instantBite` | `false` | After casting the fishing line, the fishing minigame appears immediately.
`instantCatch` | `false` | When the fishing minigame appears, you catch the fish immediately.
`throwBobberMax` | `false` | When casting the fishing line, it always reaches the maximum distance.
`durableTackles` | `false` | Fishing tackles never break.
`alwaysTreasure` | `false` | Every fishing minigame has a treasure.

### Time cheats
setting | default | what it affects
------- | ------- | ---------------
`freezeTime` | `false` | The game clock never changes.
`freezeTimeInside` | `false` | The game clock doesn't change when you're inside a building.
`freezeTimeCaves` | `false` | The game clock doesn't change when you're inside the mines, Skull Cavern, or farm cave.
`fastCask` | `false` | Casks finish instantly.
`fastFurnace` | `false` | Furnaces finish instantly.
`fastRecyclingMachine` | `false` | Recycling machines finish instantly.
`fastCrystalarium` | `false` | Crystalariums finish instantly.
`fastIncubator` | `false` | Incubators finish instantly.
`fastSlimeIncubator ` | `false` | Slime incubators finish instantly.
`fastKeg` | `false` | Kegs finish instantly.
`fastPreservesJar` | `false` | Preserves jars finish instantly.
`fastCheesePress` | `false` | Cheese presss finish instantly.
`fastMayonnaiseMachine ` | `false` | Mayonnaise machines finish instantly.
`fastLoom` | `false` | Looms finish instantly.
`fastOilMaker` | `false` | Oil makers finish instantly.
`fastSeedMaker` | `false` | Seed makers finish instantly.
`fastCharcoalKiln` | `false` | Charcoal kilns finish instantly.
`fastSlimeEggPress` | `false` | Slime egg presss finish instantly.
`fastBeeHouse` | `false` | Beehouses finish instantly.
`fastMushroomBox` | `false` | Mushroom boxs finish instantly.
`fastTapper` | `false` | Tappers finish instantly.
`fastLightningRod` | `false` | Lightning rods finish instantly.
`fastWormBin` | `false` | Worm bins finish instantly.

### Other cheats
setting | default | what it affects
------- | ------- | ---------------
`alwaysGiveGift` | `false` | You can always give gifts to villagers, regardless of the daily and weekly limits.
`noFriendshipDecay` | `false` | A villager's friendship value no longer slowly decays if you haven't maxed it out.
`durableFences` | `false` | Fences never break.
`instantBuild` | `false` | Building new structures on the farm completes instantly.
`autoFeed` | `false` | The hay troughs in your barns and coops are refilled automatically.
`infiniteHay` | `false` | Your hay silos are always full.

## Versions
1.14 (2017-04-01):
* Updated to SMAPI 1.9.
* Fast machines now work anywhere, not only on the farm.
* Fixed fast cask cheat not working.
* Internal refactoring.

1.13 (2017-01-04):
* Updated to Stardew Valley 1.1+ and SMAPI 1.5.
* Added compatibility with Linux and Mac.
* Added support for casks and worm bins.
* Fixed instantly-grown fruit trees not producing fruit until their normal growth date.
* Fixed 'grow crops' key in settings menu not being saved.

1.12 (2016-04-09):
* Updated to Stardew Valley 1.07+ and SMAPI 0.40.0+.
* Added cheats: durable tackles, harvest with sickle, grow tree, and grow crops.
* Added changeable relationships.
* Fixed mouse cursor showing when disabled.

1.11 (2016-04-02):
* Updated for SMAPI 0.39.6.
* Added cheats: fast tapper, fast lightning rod, always auto-feed, and infinite hay.
* Added casino coins cheats.
* Added warps to bathhouse, Sandy, and casino.
* Added time slider.
* Fixed time frozen label.

1.10 (2016-03-30):
* Updated for SMAPI 0.39.4.
* Added cheats: no friendship decay, instant build.
* Fixed fast machine processing in barns and coops.

1.9 (2016-03-23):
* Updated for SMAPI 0.39.1.

1.8.2 (2016-03-23):
* Fixed cheats not working in the greenhouse.

1.8.1 (2016-03-22):
* Fixed cheats not working in the greenhouse.

1.8 (2016-03-21):
* Updated to Stardew Valley 1.0.6 and SMAPI 0.38.3.
* Fixed watering fields in the greenhouse.
* Fixed fast machine processing.
* Removed the `nini.dll` file (no longer needed, now uses save method in latest SMAPI update).

1.7.1 (2016-03-18):
* Fixed fast machine processing.
* Fixed watering fields in the greenhouse.

1.7 (2016-03-17):
* Added fast processing for all machines (each toggleable).
* Added skill reset.
* You can now switch between categories with controller (shoulder triggers).
* Fixed leveling up skills.
* Fixed watering fields in some cases.

1.6 (2016-03-11):
* Menu now contains categories (last scroll position and category will be remembered).
* Fixed 'always treasure' cheat.
