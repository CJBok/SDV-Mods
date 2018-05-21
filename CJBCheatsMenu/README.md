**CJB Cheats Menu** is a [Stardew Valley](http://stardewvalley.net/) mod that adds an in-game menu
with many different cheat features:

![](screenshot.gif)

Compatible with Stardew Valley 1.2+ on Linux, Mac, and Windows. Translated into English.

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
`OpenMenuKey` | `P` | The keyboard button which opens the menu.
`FreezeTimeKey` | `T` | The keyboard button which freezes the game clock.
`GrowTreeKey` | `NumPad1` | The keyboard button which instantly grows the tree under the tool cursor.
`GrowCropsKey` | `NumPad2` | The keyboard button which instantly grows crops adjacent to your character.

### Menu settings

setting | default | what it affects
------- | ------- | ---------------
`DefaultTab` | `PlayerAndTools` | The tab shown by default when you open the menu. Possible values: `PlayerAndTools`, `FarmAndFishing`, `Skills`, `Weather`, `Relationships`, `WarpLocations`, `Time`, `Controls`.

### Player cheats
setting | default | what it affects
------- | ------- | ---------------
`IncreasedMovement` | `false` | Whether your character moves faster. The added speed is set by the `MoveSpeed` field.
`MoveSpeed` | `1` | The player speed to add if `IncreasedMovement` is `true`. This is an added multiplier (e.g. `1` doubles the default speed).
`InfiniteHealth` | `false` | Your health never decreases.
`InfiniteStamina` | `false` | Your stamina never decreases.
`MaxDailyLuck` | `false` | Your daily luck is always at the maximum value.
`OneHitKill` | `false` | Your attacks kill any monster in one hit.
`OneHitBreak` | `false` | Your tools break things instantly.
`InfiniteWateringCan` | `false` | Your watering can never runs dry.
`HarvestSickle` | `false` | You can harvest any crop with the sickle.

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
`FastIncubator` | `false` | Incubators finish instantly.
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
`InfiniteHay` | `false` | Hay silos are always full.

## Versions
1.18 (upcoming):
* Updated for Stardew Valley 1.3 (including multiplayer support).
* Added option for fast fruit trees.
* Added support for instantly watering or growing crops in garden pots.
* Added Japanese and Russian translations (thanks to Marity and Ryofuko!).
* Fixed 'increase movement speed' checkbox disabling the speed slider.
* Fixed relationship editor not letting you decrease friendships if you have "No Friendship Decay" enabled.
* Fixed relationship list not using translated name when sorting.
* Fixed Luremaster and Mariner options being swapped.
* Fixed fast machine processing not working in constructed buildings.

1.17 (2018-02-11):
* Updated to SMAPI 2.4.
* Added translation support.
* Added update checks via SMAPI.
* Added options to change player's professions.
* Added Portuguese translations (thanks to XxIceGladiadorxX).
* Fixed issue where setting the time could leave NPCs confused (e.g. stuck in bed).

1.16 (2017-07-14):
* Fixed open-menu key working even when another menu is already open.
* Fixed freeze-time key working during cutscenes.

1.15 (2017-05-27):
* Updated for Stardew Valley 1.2 and SMAPI 1.13.
* Added option to set the default tab when opening the menu.
* Fixed relationship slider always set to 10 hearts.
* Fixed 'one hit break' no longer working on stones, trees, logs, and stumps.
* Fixed 'one hit break' not working on fruit trees.

1.14.1 (2017-04-12):
* Fixed error when used with any mod that adds multiple seeds producing the same crop.

1.14 (2017-04-05):
* Updated to SMAPI 1.9.
* Fast machines now work anywhere, not only on the farm.
* Fixed fast cask cheat not working.
* Fixed disabling the 'harvest with sickle' option not restoring existing crops to normal.
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

1.5.3:
* Fixed sound bug during fishing.
* The cheats menu now remembers scroll position.

1.5.2:
* Fixed player getting stuck during events.
* Fixed sounds stuck during fishing.

1.5.1:
* Restored `Nini.dll` in download since some users still need it.
* Added 'instant bite' option, separate from 'instant catch' option.
* Fixed wrong mod version shown on load.
* Changed 'debris' weather name to 'windy'.

1.5:
* Added 'one hit break' option.
* Fixed diagonal movement.
* Fixed player speed effected while walking.
* Removed `Nini.dll` (integrated into main DLL).

1.4.1:
* Fixed fish biting instantly when cheat isn't active.

1.4:
* Added fish 'always treasure' option.
* Added instant catch also making fish bite instantly.
* Fixed time frozen dialog overlapping mine level dialog.
* Fixed durable fences cheat not working when not on farm.

1.3.2:
* Added many warp locations.
* Added 'always give gift' option.
* Added time freeze options.
* Added shortcut key to freeze time.
* Changed weather options to next day settings.
* Fixed movement speed still active during events.
