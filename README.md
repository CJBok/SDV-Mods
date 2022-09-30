﻿This repository contains my SMAPI mods for Stardew Valley. See the individual mods for
documentation and release notes:

* [CJB Cheats Menu](CJBCheatsMenu)
* [CJB Item Spawner](CJBItemSpawner)
* [CJB Show Item Sell Price](CJBShowItemSellPrice)

## Translating the mods
<!--

    This section is auto-generated using a script, there's no need to edit it manually.
    https://gist.github.com/Pathoschild/040ff6c8dc863ed2a7a828aa04447033

-->
The mods can be translated into any language supported by the game, and SMAPI will automatically
use the right translations.

Contributions are welcome! See [Modding:Translations](https://stardewvalleywiki.com/Modding:Translations)
on the wiki for help contributing translations.

(❑ = untranslated, ↻ = partly translated, ✓ = fully translated)

&nbsp;      | CJB Cheats Menu                 | CJB Item Spawner                 | CJB Show Item Sell Price
:---------- | :------------------------------ | :------------------------------- | :-------------------------------------
Chinese     | [↻](CJBCheatsMenu/i18n/zh.json) | [↻](CJBItemSpawner/i18n/zh.json) | [✓](CJBShowItemSellPrice/i18n/zh.json)
French      | [↻](CJBCheatsMenu/i18n/fr.json) | [✓](CJBItemSpawner/i18n/fr.json) | [✓](CJBShowItemSellPrice/i18n/fr.json)
German      | [↻](CJBCheatsMenu/i18n/de.json) | [↻](CJBItemSpawner/i18n/de.json) | [✓](CJBShowItemSellPrice/i18n/de.json)
Hungarian   | [↻](CJBCheatsMenu/i18n/hu.json) | [↻](CJBItemSpawner/i18n/hu.json) | [✓](CJBShowItemSellPrice/i18n/hu.json)
Italian     | [↻](CJBCheatsMenu/i18n/it.json) | [↻](CJBItemSpawner/i18n/it.json) | [✓](CJBShowItemSellPrice/i18n/it.json)
Japanese    | [↻](CJBCheatsMenu/i18n/ja.json) | [↻](CJBItemSpawner/i18n/ja.json) | [✓](CJBShowItemSellPrice/i18n/ja.json)
Korean      | [↻](CJBCheatsMenu/i18n/ko.json) | [↻](CJBItemSpawner/i18n/ko.json) | [✓](CJBShowItemSellPrice/i18n/ko.json)
[Polish]    | [✓](CJBCheatsMenu/i18n/pl.json) | [✓](CJBItemSpawner/i18n/pl.json) | [✓](CJBShowItemSellPrice/i18n/pl.json)
Portuguese  | [✓](CJBCheatsMenu/i18n/pt.json) | [↻](CJBItemSpawner/i18n/pt.json) | [✓](CJBShowItemSellPrice/i18n/pt.json)
Russian     | [↻](CJBCheatsMenu/i18n/ru.json) | [↻](CJBItemSpawner/i18n/ru.json) | [✓](CJBShowItemSellPrice/i18n/ru.json)
Spanish     | [✓](CJBCheatsMenu/i18n/es.json) | [↻](CJBItemSpawner/i18n/es.json) | [✓](CJBShowItemSellPrice/i18n/es.json)
[Thai]      | [✓](CJBCheatsMenu/i18n/th.json) | [✓](CJBItemSpawner/i18n/th.json) | [✓](CJBShowItemSellPrice/i18n/th.json)
Turkish     | [↻](CJBCheatsMenu/i18n/tr.json) | [↻](CJBItemSpawner/i18n/tr.json) | [✓](CJBShowItemSellPrice/i18n/tr.json)
[Ukrainian] | [✓](CJBCheatsMenu/i18n/uk.json) | [✓](CJBItemSpawner/i18n/uk.json) | [✓](CJBShowItemSellPrice/i18n/uk.json)

[Polish]: https://www.nexusmods.com/stardewvalley/mods/3616
[Thai]: https://www.nexusmods.com/stardewvalley/mods/7052
[Ukrainian]: https://www.nexusmods.com/stardewvalley/mods/8427

## Compiling the mods
Installing stable releases from Nexus Mods is recommended for most users. If you really want to
compile the mod yourself, read on.

These mods use the [crossplatform build config](https://www.nuget.org/packages/Pathoschild.Stardew.ModBuildConfig)
so they can be built on Linux, Mac, and Windows without changes. See [the build config documentation](https://www.nuget.org/packages/Pathoschild.Stardew.ModBuildConfig)
for troubleshooting.

### Compiling a mod for testing
To compile a mod and add it to your game's `Mods` directory:

1. Rebuild the project using [Visual Studio](https://www.visualstudio.com/vs/community/) or [MonoDevelop](http://www.monodevelop.com/).  
   <small>This will compile the code and package it into the mod directory.</small>
2. Launch the project with debugging.  
   <small>This will start the game through SMAPI and attach the Visual Studio debugger.</small>

### Compiling a mod for release
To package a mod for release:

1. Rebuild the project in _Release_ mode using [Visual Studio](https://www.visualstudio.com/vs/community/) or [MonoDevelop](http://www.monodevelop.com/).
2. Find the `bin/Release/<mod name> <version>.zip` file in the mod's project folder.
3. Upload that file.
