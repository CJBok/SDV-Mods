This repository contains my SMAPI mods for Stardew Valley. See the individual mods for
documentation and release notes:

* [CJB Cheats Menu](CJBCheatsMenu)
* [CJB Item Spawner](CJBItemSpawner)
* [CJB Show Item Sell Price](CJBShowItemSellPrice)

## Translating the mods
The mods can be translated into any language supported by the game, and SMAPI will automatically
use the right translations.

(❑ = untranslated, ↻ = partly translated, ✓ = fully translated)

&nbsp;     | CJB Cheats Menu                         | CJB Item Spawner                  | CJB Show Item Sell Price
---------- | :-------------------------------------- | :-------------------------------- | :----------------------------------
Chinese    | [↻ partial](CJBCheatsMenu/i18n/zh.json) | [✓](CJBItemSpawner/i18n/zh.json) | [✓](CJBShowItemSellPrice/i18n/zh.json)
French     | [✓](CJBCheatsMenu/i18n/fr.json)        | [✓](CJBItemSpawner/i18n/fr.json) | [✓](CJBShowItemSellPrice/i18n/fr.json)
German     | [↻ partial](CJBCheatsMenu/i18n/de.json) | [✓](CJBItemSpawner/i18n/de.json) | [✓](CJBShowItemSellPrice/i18n/de.json)
Hungarian  | [↻ partial](CJBCheatsMenu/i18n/hu.json) | [✓](CJBItemSpawner/i18n/hu.json) | [✓](CJBShowItemSellPrice/i18n/hu.json)
Italian    | ❑                                      | [✓](CJBItemSpawner/i18n/it.json) | [✓](CJBShowItemSellPrice/i18n/it.json)
Japanese   | [↻ partial](CJBCheatsMenu/i18n/ja.json) | [✓](CJBItemSpawner/i18n/ja.json) | [✓](CJBShowItemSellPrice/i18n/ja.json)
Korean     | [↻ partial](CJBCheatsMenu/i18n/ko.json) | [✓](CJBItemSpawner/i18n/ko.json) | [✓](CJBShowItemSellPrice/i18n/ko.json)
Portuguese | [↻ partial](CJBCheatsMenu/i18n/pt.json) | [✓](CJBItemSpawner/i18n/pt.json) | [✓](CJBShowItemSellPrice/i18n/pt.json)
Russian    | [✓](CJBCheatsMenu/i18n/ru.json)        | [✓](CJBItemSpawner/i18n/ru.json) | [✓](CJBShowItemSellPrice/i18n/ru.json)
Spanish    | [↻ partial](CJBCheatsMenu/i18n/es.json) | [✓](CJBItemSpawner/i18n/es.json) | [✓](CJBShowItemSellPrice/i18n/es.json)
Turkish    | ❑                                      | [✓](CJBItemSpawner/i18n/tr.json) | [✓](CJBShowItemSellPrice/i18n/tr.json)

Contributions are welcome! See [Modding:Translations](https://stardewvalleywiki.com/Modding:Translations)
on the wiki for help contributing translations.

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
