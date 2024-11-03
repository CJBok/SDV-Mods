[← back to readme](README.md)

# Release notes
## 2.5.0
Released 04 November 2024 for SMAPI 4.1.0 or later.

* Updated for Stardew Valley 1.6.9.
* Added dried items, pickled forage, smoked fish, and specific bait to menu.
* Added warning if you use the same key to open the item spawner menu and game menu, which won't work correctly.
* Fixed some wallpaper/flooring items shown twice.
* Improved translations. Thanks to Caranud (updated French), CuongNguyen0903 (added Vietnamese), mitekano23 (updated Japanese), MagoSupremo123 (updated Portuguese), and therealmate (updated Hungarian)!

## 2.4.1
Released 14 April 2024 for SMAPI 4.0.0 or later.

* Fixed 'no longer compatible' error.

## 2.4.0
Released 14 April 2024 for SMAPI 4.0.0 or later.

* Added support for [Generic Mod Config Menu](https://www.nexusmods.com/stardewvalley/mods/5098) (thanks to Tocseoj!).

## 2.3.3
Released 21 March 2024 for SMAPI 4.0.0 or later.

* Fixed error when searching if the game data has broken items with no name.

## 2.3.2
Released 19 March 2024 for SMAPI 4.0.0 or later.

* Updated for Stardew Valley 1.6.

## 2.3.1
Released 01 December 2023 for SMAPI 3.14.0 or later.

* Improved translations. Thanks to Mustafa383 (updated Turkish)!

## 2.3.0
Released 01 November 2023 for SMAPI 3.14.0 or later.

* Added `HideCategories` config option to hide certain item types in the item spawner menu.
* Added more detailed config summary to SMAPI log to simplify troubleshooting.
* Improved translations. Thanks to theRealDuda (updated Russian)!

## 2.2.7
Released 03 October 2023 for SMAPI 3.14.0 or later.

* Fixed Pickled Ginger not listed.
* Fixed 'Honey' listed instead of 'Wild Honey'.
* Fixed support for custom slingshots (thanks to DaLion via the SMAPI repo!).
* Improved translations. Thanks to CoolRabbit123 (updated German)!

## 2.2.6
Released 27 August 2023 for SMAPI 3.14.0 or later.

* Improved translations. Thanks to DanielleTlumach (updated Ukrainian)!

## 2.2.5
Released 01 July 2023 for SMAPI 3.14.0 or later.

* Fixed error loading data on Android after the Stardew Valley 1.5.6 + SMAPI updates.

## 2.2.4
Released 25 June 2023 for SMAPI 3.14.0 or later.

* Embedded `.pdb` data into the DLL, which fixes error line numbers in Linux/macOS logs.

## 2.2.3
Released 30 March 2023 for SMAPI 3.14.0 or later.

* Improved translations. Thanks to BruceWu03 (updated Chinese) and MagoSupremo123 (updated Portuguese)!

## 2.2.2
Released 09 January 2023 for SMAPI 3.14.0 or later.

* Improved translations. Thanks to wally232 (updated Korean)!

## 2.2.1
Released 10 October 2022 for SMAPI 3.14.0 or later.

* Added log messages to simplify troubleshooting when the menu doesn't open.
* Fixed option to not apply trash can upgrades not affecting items trashed with the `delete` key.
* Fixed search box too small in some languages.
* Improved translations. Thanks to ChulkyBow (updated Ukrainian)!

## 2.2.0
Released 18 August 2022 for SMAPI 3.14.0 or later.

* Added menu option to sort by sell price.
* Added `config.json` option to not apply trash can upgrades in the menu.
* Fixed error rendering menu if some translations are very long.
* Improved translations. Thanks to ellipszist (updated Thai) and Zangorr (updated Polish)!

## 2.1.7
Released 09 May 2022 for SMAPI 3.14.0 or later.

* Updated for SMAPI 3.14.0.
* Fixed spawning Journal Scraps and Secret Notes.

## 2.1.6
Released 26 February 2022 for SMAPI 3.13.0 or later.

* Improved translations. Thanks to ChulkyBow (added Ukrainian) and ChulkyBow + Zangorr (added Polish)!

## 2.1.5
Released 19 December 2021 for SMAPI 3.13.0 or later.

* Updated for Stardew Valley 1.5.5.
* Improved translations. Thanks to ellipszist (added Thai)!

## 2.1.4
Released 04 September 2021 for SMAPI 3.12.6 or later.

* Improved translations. Thanks to widfar (updated Italian)!

## 2.1.3
Released 24 July 2021 for SMAPI 3.9.5 or later.

* Fixed some vanilla shirts not shown in the menu.
* Improved translations. Thanks to randomC0der (updated German)!

## 2.1.2
Released 27 March 2021 for SMAPI 3.9.5 or later.

* Fixed compatibility with [unofficial 64-bit mode](https://stardewvalleywiki.com/Modding:Migrate_to_64-bit_on_Windows).

## 2.1.1
Released 21 March 2021 for SMAPI 3.9 or later.

* Closing the gamepad text entry UI now snaps to the item grid.
* Fixed menu exiting if you type 'e' in the search box with 2.1.
* Fixed unable to deselect textbox with 2.1.

## 2.1
Released 20 March 2021 for SMAPI 3.9 or later.

* Added support for [multi-key bindings](https://stardewvalleywiki.com/Modding:Player_Guide/Key_Bindings#Multi-key_bindings).
* Improved controller support (thanks to b-b-blueberry!).
* Fixed menu background not handling UI scaling correctly.
* Improved translations. Thanks to Alucard92 (updated Spanish), horizon98 (updated Chinese), Kareolis (updated Russian), and KediDili (updated Turkish)!

## 2.0.2
Released 21 December 2020 for SMAPI 3.8 or later.

* Updated for Stardew Valley 1.5, including split-screen mode.

## 2.0.1
Released 19 November 2020 for SMAPI 3.7 or later.

* Jukebox ring is no longer hidden by default (since it no longer causes a crash in SDV 1.4).
* Moved drum/flute blocks from _equipment: tools_ to _crafting: products_.
* Fixed clicks on a scrolled dropdown selecting the wrong option.
* Fixed sort icon overlapping text in some languages.
* Improved translations. Thanks to Becks723 (updated Chinese) and wally232 (updated Korean)!

## 2.0
Released 30 October 2020 for SMAPI 3.7 or later.

* Redesigned menu to make it more intuitive and mobile-friendly.
* Added full controller support.
* Added keyboard navigation for categories and scrolling.
* Added official Android compatibility.
* Added shirts normally only available during character customization.
* Added support for customizing the filter categories via `assets/categories.json`.
* Overhauled category grouping to be more useful.
* The item spawner now applies fish pond rules for roe items. (That mainly adds Clam Roe, Sea Urchin Roe, and custom roe from mods.)
* Rewrote mod to simplify future game updates.
* Improved translations. Thanks to Caco-o-sapo (updated Portuguese)!

(Thanks to b-b-blueberry for tweaking some of the new UI, Cakewalker for Android testing, and many users on Discord for feedback!)

## 1.12.2
Released 12 September 2020 for SMAPI 3.7 or later.

* Fixed error opening menu when some item data is invalid.

## 1.12.1
Released 14 April 2020 for SMAPI 3.4 or later.

* Fixed item slots not grayed out in spawner menu if you don't have the backpack upgrades.
* Fixed spawned 'floor TV' item not functional as a TV (thanks to Platonymous!).
* Fixed error opening menu if you have broken XNB mods.
* Improved translations. Thanks to BURAKMESE (added Turkish) and therealmate (added Hungarian)!

## 1.12
Released 09 February 2020 for SMAPI 3.2 or later.

* Clicking the search box now keeps it selected until you click elsewhere.
* Added support for searching on Android.
* Fixed spawning new flooring and rings (thanks to Mizzion!).
* Fixed spawning custom rings added by mods.
* Fixed errors when some item data is invalid.
* Fixed errors on Android.
* Fixed incorrect color for sturgeon roe.
* Fixed menu not usable with a controller when the 'controller-style menus' option is enabled.

## 1.11.5
Released 26 November 2019 for SMAPI 3.0 or later.

* Updated for Stardew Valley 1.4, including new items and upgradeable trash can.
* Improved translations. Thanks to shirutan (added Japanese) and xLPMG (added Italian)!

## 1.11.4
Released 10 June 2019 for SMAPI 2.11.1 or later.

* Improved translations. Thanks to S2SKY (improved Korean)!

## 1.11.3
Released 27 March 2019 for SMAPI 2.11 or later.

* Improved translations. Thanks to manuqp (added Spanish), S2SKY (added Korean), and VincentRoth (added French)!

## 1.11.2
Released 09 December 2018 for SMAPI 2.9 or later.

* Updated for the upcoming SMAPI 3.0.
* Fixed inventory items being reset (e.g. slingshot ammo removed) when picked up in the spawner menu.
* Improved translations. Thanks to Nanogamer7 (added German), BerryConcept and Redlnn (added Chinese)!

## 1.11.1
Released 28 August 2018 for SMAPI 2.8 or later.

* Updated for Stardew Valley 1.3.29.
* Updated list of problematic items (which are hidden by default).
* Fixed all slingshots spawned as the basic slingshot.

## 1.11
Released 04 August 2018 for SMAPI 2.6 or later.

* Updated for Stardew Valley 1.3 (including multiplayer support).
* Added controller support.
* Added support for secret notes.
* Problematic items are now hidden by default (e.g. those which can crash the game).
* Fixed pickled items not shown.
* Fixed incorrect tab name.
* Fixed errors when another mod adds invalid item data.
* Fixed search not matching translated display names (thanks to Roskonix!).
* Improved translations. Thanks to Ryofuko (added Russian)!

## 1.10
Released 11 February 2018 for SMAPI 2.4 or later.

* Updated to SMAPI 2.4.
* Added translation support.
* Added update checks via SMAPI.
* Added return scepter to spawnable items.
* Improved translations. Thanks to XxIceGladiadorxX (added Portuguese)!

## 1.9
Released 14 July 2017 for SMAPI 1.15 or later.

* Fixed some craftables no longer being listed.
* Fixed some items being listed twice.

## 1.8
Released 30 May 2017 for SMAPI 1.13 or later.

* Updated for Stardew Valley 1.2.
* Added flavored honey.
* Fixed support for non-English players, including tab filtering & translated item names.
* Fixed 'misc' tab duplicating some items shown in other tabs.

## 1.7.2
Released 11 April 2017 for SMAPI 1.9 or later.

* The hotkey can now be changed in `config.json`.
* Fixed quality & sort buttons broken in 1.7.

## 1.7.1
Released 05 April 2017 for SMAPI 1.9 or later.

* Fixed tab selection broken in 1.7.

## 1.7
Released 05 April 2017 for SMAPI 1.9 or later.

* Updated to SMAPI 1.9.
* Now only shows item sell price if you have the CJB Show Item Sell Price mod installed.
* Internal refactoring.

## 1.6
Released 06 January 2017 for SMAPI 1.5 or later. This and subsequent releases updated by Pathoschild.

* Updated to Stardew Valley 1.1+ and SMAPI 1.5.
* Added compatibility with Linux and Mac.
* Added support for new iridium quality.

## 1.5
Released 02 April 2016 for SMAPI 0.39.6 or later. Updated by CJBok.

* Added change quality button.
* Added all juice items, wine items, pickled items, and jelly items.
* Changed GUI positioning.

## 1.4
Released 23 March 2016 for SMAPI 0.39.1 or later. Updated by CJBok.

* Updated for SMAPI 0.39.1.

## 1.3
Released 22 March 2016 for SMAPI 0.38.3 or later. Updated by CJBok.

* Updated for SMAPI 0.38.3.
* Added clickable arrows for scrolling.
* Added search bar.

## 1.2
Released 18 March 2016 for SMAPI 0.37.3 or later. Updated by CJBok.

* Added another crapload of missing items.
* Added show sell price when you hover over an item.
* Fixed wallpapers and floors not being added to your inventory.
* Fixed spawned TVs not working.

## 1.1
Released 16 March 2016 for SMAPI 0.37.3 or later. Updated by CJBok.

* Added "decorating" category for furniture, floors, wallpapers, and decors.
* Added hats and boots to equipment category.
* Fixed slingshots.
* Fixed darker background in menu.

## 1.0
Released 15 March 2016 for SMAPI 0.37.3 or later. Released by CJBok.

* First release.
