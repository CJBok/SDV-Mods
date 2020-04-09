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
file. Most of the settings should be edited in-game through the cheats menu instead, so only those
you may need to edit directly are listed here.

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
`CommunityCenter` | Hide warp if the community center is demolished.
`Farm`            | Warp to the current player's cabin or farmhouse. The `Location` and `Tile` fields are ignored.
`JojaMart`        | Hide warp if the JojaMart is demolished.
`MovieTheaterCommunity` | Hide warp if the movie theater isn't built, or was built through the Joja route.
`MovieTheaterJoja`      | Hide warp if the movie theater isn't built, or was built through the community route.

## See also
* [Release notes](release-notes.md)
