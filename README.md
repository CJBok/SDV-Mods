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
Chinese    | [✓](CJBCheatsMenu/i18n/zh.json)        | ❑                                | ❑
German     | ❑                                      | ❑                                | ❑
Japanese   | [↻ partial](CJBCheatsMenu/i18n/ja.json) | ❑                                | ❑
Portuguese | [↻ partial](CJBCheatsMenu/i18n/pt.json) | [✓](CJBItemSpawner/i18n/pt.json) | [✓](CJBShowItemSellPrice/i18n/pt.json)
Russian    | [↻ partial](CJBCheatsMenu/i18n/ru.json) | [✓](CJBItemSpawner/i18n/ru.json) | [✓](CJBShowItemSellPrice/i18n/ru.json)
Spanish    | [✓](CJBCheatsMenu/i18n/es.json)        | ❑                                | [✓](CJBShowItemSellPrice/i18n/es.json)

Here's how to translate one of my mods:

1. Copy `default.json` into a new file with the right name:

   language   | file name
   ---------- | ---------
   Chinese    | `zh.json`
   German     | `de.json`
   Japanese   | `ja.json`
   Portuguese | `pt.json`
   Spanish    | `es.json`

2. Translate the second part on each line:
   ```json
   "example-key": "some text here"
                   ^-- translate this
   ```
   Don't change the quote characters, and don't translate the text inside `{{these brackets}}`.
3. Launch the game to try your translations.  
   _You can edit translations without restarting the game; just type `reload_i18n` in the SMAPI console to reload the translation files._

Create an issue or pull request here with your translations, or send them to me via Nexus or the forums. :)

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
