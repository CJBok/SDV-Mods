**CJB Cheats Menu** is a [Stardew Valley](http://stardewvalley.net/) mod that adds an in-game menu
with many different cheat features:

![](screenshot.gif)

Compatible with Stardew Valley 1.5.5+ on Linux, macOS, and Windows.

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
See [valid key bindings](https://stardewvalleywiki.com/Modding:Player_Guide/Key_Bindings). Simple
key bindings can be configured through the in-game UI, but multi-key bindings can only be set in the
`config.json` file.

setting | default | what it affects
------- | ------- | ---------------
`OpenMenuKey` | `P` | The keybind which opens the menu.
`FreezeTimeKey` | `T` | The keybind which freezes the game clock.
`GrowTreeKey` | `NumPad1` | The keybind held to grow trees around the player.
`GrowCropsKey` | `NumPad2` | The keybind held to grow crops around the player.
`GrowRadius` | `1` | The number of tiles in each direction around the player to cover when pressing the `GrowCropsKey` or `GrowTreeKey`.

### Menu settings
setting | default | what it affects
------- | ------- | ---------------
`DefaultTab` | `PlayerAndTools` | The tab shown by default when you open the menu. Possible values: `PlayerAndTools`, `FarmAndFishing`, `Skills`, `Weather`, `Relationships`, `WarpLocations`, `Time`, `Advanced`, `Controls`.

### Personal warps
<table>
  <tr>
    <th>setting</th>
    <th>default</th>
    <th>what it affects</th
  </tr>
  <tr>
        <td><code>HideWarps</code><br /><code>HideWarpSections</code></td>
        <td><code>[]</code></td>
<td>

The warp IDs (`HideWarps`) or section IDs (`HideWarpSections`) to hide in the warp menu. Each ID can optionally contain
`*` wildcards.

For example, this hides the warp to Robin's shop and every warp from Stardew Valley Expanded:

```json
"HideWarps": [ "carpenter", "FlashShifter.StardewValleyExpandedCP_*" ]
```

To see warp IDs:
1. Enter `patch export Mods/CJBok.CheatsMenu/Warps` in the SMAPI console window.
2. Open the file it creates in a text editor.
3. Use the ID from the `Id` field (for warps) or `SectionId` (for sections).

</td>
  </tr>
  <tr>
        <td><code>AddWarps</code><br /><code>AddWarpSections</code></td>
        <td><code>[]</code></td>
<td>

Add custom warps or sections to the list. These use the same fields as [_edit warps_ in the author guide](author-guide.md#edit-warps).

For example, this adds a warp to Abigail's room:

```json
"AddWarps": [
    {
        "DisplayName": "Abigail's Room",
        "SectionId": "town",
        "Location": "SeedShop",
        "Tile": "3, 6"
    }
]
```

You can use the [Debug Mode](https://www.nexusmods.com/stardewvalley/mods/679) mod to see location names and tile
coordinates in-game.

</td>
  </tr>
</table>


## Editing warps
* For players, see [_personal warps_](#personal-warps) above.
* For mod authors, see the [author guide](author-guide.md) to add/edit/remove warps in the menu to match your map edits
  and custom locations.

## See also
* [Release notes](release-notes.md)
