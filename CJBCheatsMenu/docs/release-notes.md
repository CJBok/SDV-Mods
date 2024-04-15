[← back to readme](README.md)

# Release notes
## Upcoming release
* Fixed 'auto-feed animals' no longer applied if you have no silos with hay available.
* Fixed community center warp sometimes hidden in 1.36.0.
* Fixed menu not closed when warping into mines.

## 1.36.1
Released 14 April 2024 for SMAPI 4.0.0 or later.

* Fixed 'no longer compatible' error.

## 1.36.0
Released 14 April 2024 for SMAPI 4.0.0 or later.

* Other mods [can now edit warps through Content Patcher](author-guide.md).  
  _That means you no longer need to manually download warp files (once other mods are updated), and you can now have custom warps for multiple mods at once._
* Added support for [Generic Mod Config Menu](https://www.nexusmods.com/stardewvalley/mods/5098) for the keybinds and default tab (thanks to Tocseoj!).
* Added mine level selector when warping to the mines or Skull Cavern (thanks to ThatGamerBlue!).
* Fixed 'one-hit kill' cheat causing grubs to instantly start turning into flies.
* Fixed 'auto-feed animals' only using hay stored on the farm.
* Fixed 'auto-feed animals' not watering slimes if you have no more hay.
* Fixed 'auto-pet animals' and 'auto-feed animals' not applied to animals outside the farm.
* Fixed 'auto-pet pets' sometimes no longer applied to saves whose date was edited.
* Removed `cjb_reload_warps` console command, which is no longer needed with the new warp assets.
* Updated translations. Thanks to EngurRuzgar (updated Turkish) and Tenebrosful (updated French)!

## 1.35.1
Released 29 March 2024 for SMAPI 4.0.0 or later.

* Removed 'reset skills' option.  
  _This was risky (since there's no record of the level perks previously applied) and didn't really work correctly._
* Restored pre-1.35 fast lightning rod behavior, so they can still be used to intercept lightning.
* Improved translations. Thanks to BernieAteUsername (updated Chinese)!

## 1.35.0
Released 24 March 2024 for SMAPI 4.0.0 or later.

* Overhauled machine support. This adds 'fast machines' options for...
  * all new machines in Stardew Valley 1.6;
  * all custom machines added to the new `Data/Machines` asset;
  * all custom buildings with item conversion rules in the new `Data/Buildings` asset;
  * lightning rods.
* Added mastery cave warp.
* Added explanation to weather tab to dispel common misconception that it has persistent effects on weather.

## 1.34.1
Released 21 March 2024 for SMAPI 4.0.0 or later.

* Fixed 'durable tackle' option not applied to the new second slot.

## 1.34.0
Released 19 March 2024 for SMAPI 4.0.0 or later.

* Updated for Stardew Valley 1.6.
* Removed problematic item filtering. (All problematic items were fixed in Stardew Valley 1.6.)
* Using the grow hotkey on a full-grown tea bush or fruit tree now produces tea leaves or fruit if it's in season.
* Improved translations. Thanks to ellipszist (updated Thai)!

## 1.33.5
Released 01 December 2023 for SMAPI 3.14.0 or later.

* Improved translations. Thanks to Mustafa383 (updated Turkish)!

## 1.33.4
Released 01 November 2023 for SMAPI 3.14.0 or later.

* Improved translations. Thanks to MagoSupremo123 (updated Portuguese) and theRealDuda (updated Russian)!

## 1.33.3
Released 03 October 2023 for SMAPI 3.14.0 or later.

* Improved translations. Thanks to wally232 (updated Korean)!

## 1.33.2
Released 27 August 2023 for SMAPI 3.14.0 or later.

* Improved translations. Thanks to DanielleTlumach (updated Ukrainian)!

## 1.33.1
Released 25 June 2023 for SMAPI 3.14.0 or later.

* Embedded `.pdb` data into the DLL, which fixes error line numbers in Linux/macOS logs.

## 1.33.0
Released 30 March 2023 for SMAPI 3.14.0 or later.

* Added option for 'can read Junimo text' in advanced section.
* Adjusted grow-crops cheat for compatibility with some crop mods (thanks to atravita!).
* Improved translations. Thanks to BruceWu03 (updated Chinese)!

## 1.32.2
Released 09 January 2023 for SMAPI 3.14.0 or later.

* Improved translations. Thanks to ellipszist (updated Thai), Mysti57155 (updated French), and wally232 (updated Korean)!

## 1.32.1
Released 10 October 2022 for SMAPI 3.14.0 or later.

* Added log messages to simplify troubleshooting when the menu doesn't open.
* Fixed 'no friendship decay' sometimes not applied to the overnight friendship decay when you quit after saving.
* Improved translations. Thanks to ellipszist (updated Thai)!

## 1.32.0
Released 18 August 2022 for SMAPI 3.14.0 or later.

* Added fast mills cheat.
* Improved translations. Thanks to ChulkyBow (updated Ukrainian), ellipszist (updated Thai), matheuslacet (updated Portuguese), riazaia (updated Spanish), and Zangorr (updated Polish)!

## 1.31.0
Released 09 May 2022 for SMAPI 3.14.0 or later.

* Updated for SMAPI 3.14.0.
* Added 'auto-pet pets' cheat.
* Added warp to the [Witch's Swamp](https://stardewvalleywiki.com/Witch's_Swamp).
* Added more menu sounds to improve accessibility.
* Added `cjb_reload_warps` console command to reload the `assets/warps.json` file in-game (thanks to MattPlays!).
* Fixed Mutant Bug Lair affected by the 'freeze time inside' option.
* Improved translations. Thanks to Dracoeris (updated Spanish) and Zangorr (updated Polish)!

## 1.30.0
Released 26 February 2022 for SMAPI 3.13.0 or later.

* Added warp to the [Mutant Bug Lair](https://stardewvalleywiki.com/Mutant_Bug_Lair).
* The 'auto-pet animals' cheat now provides the same friendship/happiness boost as petting manually.
* Fixed menu being semi-transparent behind faded options.
* Fixed missing translation in French.
* Improved translations. Thanks to ChulkyBow (added Ukrainian) and Zangorr (added Polish)!

## 1.29.3
Released 19 December 2021 for SMAPI 3.13.0 or later.

* Updated for Stardew Valley 1.5.5.
* Improved sewer warp position.
* Improved translations. Thanks to ellipszist (added Thai)!

## 1.29.2
Released 26 November 2021 for SMAPI 3.12.5 or later.

* Improved translations. Thanks to horizon98 (updated Chinese), kelvindules (updated Portuguese), Nerzullive (updated Russian), Tenebrosful & Lumisteria (updated French), and Tennix3 (updated German)!

## 1.29.1
Released 04 September 2021 for SMAPI 3.12.6 or later.

* Improved translations. Thanks to widfar (updated Italian) and wally232 (updated Korean)!

## 1.29.0
Released 24 July 2021 for SMAPI 3.9.5 or later.

* Added in-game date changer (thanks to kdau!).
* Added 'auto-pet animals' cheat (thanks to strobel1ght!).
* Renamed 'always auto-feed' to 'auto-feed animals' for clarity.
* Fixed 'auto-water crops' and 'harvest with scythe' in locations that are only farmable using mods.
* Fixed move speed set to +1 by default.
* Improved translations. Thanks to Aminato01 (updated French) and Fioruci (updated Portuguese)!

## 1.28.1
Released 27 March 2021 for SMAPI 3.9.5 or later.

* Fixed compatibility with [unofficial 64-bit mode](https://stardewvalleywiki.com/Modding:Migrate_to_64-bit_on_Windows).

## 1.28
Released 20 March 2021 for SMAPI 3.9 or later.

* Added support for [multi-key bindings](https://stardewvalleywiki.com/Modding:Player_Guide/Key_Bindings#Multi-key_bindings). (Multi-key bindings must be set [through the `config.json`](README.md#configure); the UI will only recognize one key.)
* Added support for binding a controller button through the 'controls' tab.
* Added fast-machine cheat for Statues of True Perfection.
* Fixed 'time frozen' box shown in map screenshot exports.
* Fixed outdoor ambient lighting not updated when you reverse time.
* Fixed menu becoming unresponsive if you press a controller shoulder button while the 'press new key' overlay is open.
* Fixed able to disable menu key accidentally using a controller.
* Improved translations. Thanks to GraceHol and Kareolis (updated Russian), horizon98 (updated Chinese), KediDili (updated Turkish), vlcoo (updated Spanish), and wally232 (updated Korean)!

## 1.27
Released 06 January 2021 for SMAPI 3.8 or later.

* Added cheats to increase Golden Walnut and Qi Gem counts.
* Fixed some options not working on the island farm (specifically auto water, harvest with scythe, and one-hit break).
* Fixed 'auto water' not watering tilled-but-empty dirt.
* Fixed 'fast casks' not fully completing casks in Stardew Valley 1.5.
* Fixed 'time frozen' box covering the floor number in the new volcano dungeon.
* Fixed warp to farm not accounting for custom farmhouse positions in Stardew Valley 1.5.
* Fixed error when split-screen player joins.
* Improved translations. Thanks to Enaium (updated Chinese)!

## 1.26
Released 21 December 2020 for SMAPI 3.8 or later.

* Updated for Stardew Valley 1.5, including support for...
  * split-screen mode and UI scaling;
  * new fast machines (bone mill, coffee maker, deconstructor, geode crusher, heavy tapper, ostrich incubator, and solar panel);
  * key to the town wallet item;
  * island warps.

## 1.25.4
Released 19 November 2020 for SMAPI 3.7 or later.

* Fixed 'time frozen' box still shown in screenshot mode.
* Fixed errors when the current location isn't ready.
* Improved translations. Thanks to Becks723 (updated Chinese) and wally232 (updated Korean)!

## 1.25.3
Released 30 October 2020 for SMAPI 3.7 or later.

* 'Always auto-feed' now works without silos if 'infinite hay' is enabled.
* Internal refactor to translation handling, and now uses game translations where possible.

## 1.25.2
Released 12 September 2020 for SMAPI 3.7 or later.

* Fixed error using one-hit kill cheat in some mod locations with invalid data.
* Improved translations. Thanks to Annosz (updated Hungarian), Rittsuka (updated Portuguese), and stefanhahn80 (updated German)!

## 1.25.1
Released 28 June 2020 for SMAPI 3.5 or later.

* Improved translations. Thanks to AndyAllanPoe (added Italian) and kdau (updated Spanish)!

## 1.25
Released 18 May 2020 for SMAPI 3.4 or later.

* Always Auto-Feed now waters slimes in Slime Hutches too.
* Fixed freeze-time not applying while a menu is open in multiplayer in 1.24.
* Fixed fast casks taking 10 in-game minutes in 1.24.
* Improved translations. Thanks to D0n-A (updated Russian) and talha12 (added Turkish)!

## 1.24
Released 14 April 2020 for SMAPI 3.4 or later.

* Added cheats:
  * instant weapon cooldowns;
  * unlock backpack upgrades;
  * auto-water crops (replaces the former 'water all fields' button).
* Overhauled warps:
  * Warps are now grouped into sections for easier navigation.
  * Added JojaMart and Movie Theater warps.
  * Community Center warp is now hidden if it was demolished.
  * Desert warp no longer triggers bus scene.
  * Desert and Sandy's Shop warps combined.
  * New Beach warp renamed to Tide Pools.
* Improved fast machine options:
  * Added crab pots, soda machines, statues of endless fortune, and statues of perfection.
  * Mushroom boxes now refill immediately.
  * Slime incubators now hatch slimes immediately.
* Improved compatibility with Android (thanks to kdau!).
* Simplified move speed option.
* 'Grow crops' and 'grow trees' can now be bound to the same button to grow both at once.
* Fixed open cheats menu not updated correctly when game window is resized.
* Fixed invalid time when changing time from after midnight.
* Fixed instant-grown wild seeds being different from slow-grown ones (e.g. not applying botanist bonus, not randomized, etc).
* Fixed instant-grown crops not generating giant crops.
* Fixed relationship sliders being slightly off on the right side.
* Internal refactoring and optimizations.
* Improved translations. Thanks to Akir0 (updated French), Annosz (added Hungarian), and D0n-A (updated Russian)!

## 1.23.1
Released 18 February 2020 for SMAPI 3.2 or later.

* Fixed 'fast casks' not completing until in-game clock change after 1.23.

## 1.23
Released 17 February 2020 for SMAPI 3.2 or later.

* Added option to increase the grow crop/tree radius.
* Added option to reset controls.
* Added support for disabling a control by pressing escape when 'Press New Key' is shown (except for the open-menu key, so you can't get locked out of the menu.)
* Added Skull Cavern warp (thanks to Enaium!).
* The 'freeze time' control is now unbound by default. (Current players aren't affected unless they reset controls.)
* Fixed grow trees not working consistently after 1.22.
* Fixed fast egg incubator not working.
* Clarified how fast slime egg incubator works.
* Improved translations. Thanks to Enaium (updated Chinese)!

## 1.22
Released 09 February 2020 for SMAPI 3.2 or later.

* Water all fields now works for indoor pots in any location.
* Grow crops/trees now affects those around the player (instead of under the cursor), to avoid confusion and for Android compatibility.
* Grow crops/trees no longer skips some in some cases.
* Harvest with scythe now works on garden pots.
* The farm/casino warps can now be overridden via `data/warp.json`.
* Fixed health bonuses not applied when changing professions.
* Fixed 'always auto-feed' changing total hay incorrectly in some cases.
* Fixed 'always auto-feed' not counting animals who aren't in their home building if it's enabled after the day already started.
* Fixed menu not usable with a controller when the 'controller-style menus' option is enabled.
* Improved translations. Thanks to ba0109 (updated Korean), jahangmar (updated German), mael-belval (updated French), Redlnn (updated Chinese), shirutan (updated Japanese), VengelmBjorn (updated Russian), and victrosantos (updated Portuguese and Spanish)!

## 1.21
Released 26 November 2019 for SMAPI 3.0 or later.

* Updated for Stardew Valley 1.4, including...
  * added wood chipper support for fast machines;
  * added unlockable dyeing & tailoring;
  * added new Community Center bundle;
  * added support for instantly growing tea bushes.
* Added support for holding the grow crop/tree keys while moving the cursor, so it's easier to grow larger fields.
* Warps are now sorted alphabetically.
* Warps can now be customised by editing `data/warps.json`.
* Rewrote increased movement speed to fix a number of speed-related bugs.
* Fixed sunflowers not dropping seeds when harvested with a scythe (via SDV 1.4).
* Improved translations. Thanks to jahangmar (updated German), overwritten (updated Korean), qqkookie (updated Korean), Redlnn (updated Chinese), Riazaia (updated Spanish), shiro2579 (updated Portuguese), and shirutan (updated Japanese)!

## 1.20.1
Released 12 June 2019 for SMAPI 2.11.1 or later.

* Reworked community center flag options.
* Fixed setting a community center flag not completing the in-game area.
* Fixed 'unlock community center' option not working correctly.
* Fixed instant-grow-crop not working with garden pots placed on tilled dirt or flooring.
* Improved translations. Thanks to shirutan (updated Japanese)!

## 1.20
Released 10 June 2019 for SMAPI 2.11.1 or later.

* Added 'advanced' tab to set flags and wallet items; merged 'quests' tab into 'advanced'.
* Fixed max relationship meter not extended for spouse.
* Fixed being able to open the menu when a minigame is active.
* Improved translations. Thanks to S2SKY (added Korean) and TheOzonO3 (updated Russian)!

## 1.19
Released 27 March 2019 for SMAPI 2.11 or later.

* Added support for setting relationships for unmet villagers.
* Fast machine processing is now much faster.
* Fast machine processing now continues working when time is paused.
* Fast machine list is now sorted by name.
* Fixed land swimming bug when warping out of the spa.
* Improved translations. Thanks to kelvindules (updated Portuguese) and VincentRoth (added French)!

## 1.18.3
Released 09 December 2018 for SMAPI 2.9 or later.

* Updated for the upcoming SMAPI 3.0.
* Fixed harvest with scythe option saying 'no XP gain', which was fixed in Stardew Valley 1.3. (Thanks to SkpFX!)
* Improved translations. Thanks to Nanogamer7 (added German) and Redlnn (improved Chinese)!

## 1.18.2
Released 03 November 2018 for SMAPI 2.8 or later.

* Improved translations. Thanks to Spa51 (added Spanish)!

## 1.18.1
Released 28 August 2018 for SMAPI 2.8 or later.

* Updated for Stardew Valley 1.3.29.
* Farmhands in multiplayer warping to the farm now land in front of their cabin, instead of the farmhouse.
* 'Grow crop' now affects crops under the cursor (instead of around the player).
* 'Grow tree' now affects the one under the cursor (instead of under the tool square).
* Fixed casino warp shown when player can't access casino yet.
* Fixed museum warp landing one tile to the right of the door.
* Fixed relationship slider not disabled if you haven't met the NPC yet.

## 1.18
Released 04 August 2018 for SMAPI 2.7 or later. Updated by CJBok (quests feature) and Pathoschild.

* Updated for Stardew Valley 1.3 (including multiplayer support).
* Added Quests tab to complete active quests instantly.
* Added option for fast fruit trees.
* Added support for instantly watering or growing crops in garden pots.
* Added support for custom greenhouse locations.
* Improved controller support.
* Fixed issues with fishing cheats.
* Fixed 'increase movement speed' checkbox disabling the speed slider.
* Fixed 'no friendship decay' preventing you from decreasing friendships through the relationships tab.
* Fixed 'no friendship decay' not resetting when you switch save.
* Fixed 'instant grow tree' making fruit trees not produce fruit.
* Fixed 'one-hit kill' cheat making monsters invincible in rare cases (thanks to Issacy!).
* Fixed relationship list not using translated name when sorting.
* Fixed relationship list not showing dwarf.
* Fixed Luremaster and Mariner professions being swapped.
* Fixed fast machine processing not working in constructed buildings.
* Fixed some artisanal items not spawning with selected quality.
* Fixed searchbox getting cleared when you change another options like quality.
* Fixed setting time manually not working if time is frozen (thanks to Issacy!).
* Fixed things happening repeatedly when time is frozen in some cases (thanks to Issacy!).
* Improved translations. Thanks to Issacy (added Chinese), Marity (added Japanese), and Ryofuko (added Russian)!

## 1.17
Released 11 February 2018 for SMAPI 2.4 or later.

* Updated to SMAPI 2.4.
* Added translation support.
* Added update checks via SMAPI.
* Added options to change player's professions.
* Fixed issue where setting the time could leave NPCs confused (e.g. stuck in bed).
* Improved translations. Thanks to XxIceGladiadorxX (added Portuguese)!

## 1.16
Released 14 July 2017 for SMAPI 1.15 or later.

* Fixed open-menu key working even when another menu is already open.
* Fixed freeze-time key working during cutscenes.

## 1.15
Released 27 May 2017 for SMAPI 1.13 or later.

* Updated for Stardew Valley 1.2 and SMAPI 1.13.
* Added option to set the default tab when opening the menu.
* Fixed relationship slider always set to 10 hearts.
* Fixed 'one hit break' no longer working on stones, trees, logs, and stumps.
* Fixed 'one hit break' not working on fruit trees.

## 1.14.1
Released 12 April 2017 for SMAPI 1.9 or later.

* Fixed error when used with any mod that adds multiple seeds producing the same crop.

## 1.14
Released 05 April 2017 for SMAPI 1.9 or later.

* Updated to SMAPI 1.9.
* Fast machines now work anywhere, not only on the farm.
* Fixed fast cask cheat not working.
* Fixed disabling the 'harvest with scythe' option not restoring existing crops to normal.
* Internal refactoring.

## 1.13
Released 04 January 2017 for SMAPI 1.5 or later. This and subsequent releases updated by Pathoschild.

* Updated to Stardew Valley 1.1+ and SMAPI 1.5.
* Added compatibility with Linux and Mac.
* Added support for casks and worm bins.
* Fixed instantly-grown fruit trees not producing fruit until their normal growth date.
* Fixed 'grow crops' key in settings menu not being saved.

## 1.12
Released 09 April 2016 for SMAPI 0.40 or later. Updated by CJBok.

* Updated to Stardew Valley 1.07+ and SMAPI 0.40.0+.
* Added cheats: durable tackles, harvest with scythe, grow tree, and grow crops.
* Added changeable relationships.
* Fixed mouse cursor showing when disabled.

## 1.11
Released 02 April 2016 for SMAPI 0.39.6 or later. Updated by CJBok.

* Updated for SMAPI 0.39.6.
* Added cheats: fast tapper, fast lightning rod, always auto-feed, and infinite hay.
* Added casino coins cheats.
* Added warps to bathhouse, Sandy, and casino.
* Added time slider.
* Fixed time frozen label.

## 1.10
Released 30 March 2016 for SMAPI 0.39.4 or later. Updated by CJBok.

* Updated for SMAPI 0.39.4.
* Added cheats: no friendship decay, instant build.
* Fixed fast machine processing in barns and coops.

## 1.9
Released 23 March 2016 for SMAPI 0.39.1 or later. Updated by CJBok.

* Updated for SMAPI 0.39.1.

## 1.8.2
Released 23 March 2016 for SMAPI 0.38.3 or later. Updated by CJBok.

* Fixed cheats not working in the greenhouse.

## 1.8.1
Released 22 March 2016 for SMAPI 0.38.3 or later. Updated by CJBok.

* Fixed cheats not working in the greenhouse.

## 1.8
Released 21 March 2016 for SMAPI 0.38.3 or later. Updated by CJBok.

* Updated to Stardew Valley 1.0.6 and SMAPI 0.38.3.
* Fixed watering fields in the greenhouse.
* Fixed fast machine processing.
* Removed the `nini.dll` file (no longer needed, now uses save method in latest SMAPI update).

## 1.7.1
Released 18 March 2016 for SMAPI 0.37.3 or later. Updated by CJBok.

* Fixed fast machine processing.
* Fixed watering fields in the greenhouse.

## 1.7
Released 17 March 2016 for SMAPI 0.37.3 or later. Updated by CJBok.

* Added fast processing for all machines (each toggleable).
* Added skill reset.
* You can now switch between categories with controller (shoulder triggers).
* Fixed leveling up skills.
* Fixed watering fields in some cases.

## 1.6
Released 11 March 2016 for SMAPI 0.37.3 or later. Updated by CJBok.

* Menu now contains categories (last scroll position and category will be remembered).
* Fixed 'always treasure' cheat.

## 1.5.3
Released 10 March 2016 for SMAPI 0.37.3 or later. Updated by CJBok.

* Fixed sound bug during fishing.
* The cheats menu now remembers scroll position.

## 1.5.2
Released 10 March 2016 for SMAPI 0.37.3 or later. Updated by CJBok.

* Fixed player getting stuck during events.
* Fixed sounds stuck during fishing.

## 1.5.1
Released 10 March 2016 for SMAPI 0.37.3 or later. Updated by CJBok.

* Restored `Nini.dll` in download since some users still need it.
* Added 'instant bite' option, separate from 'instant catch' option.
* Fixed wrong mod version shown on load.
* Changed 'debris' weather name to 'windy'.

## 1.5
Released 10 March 2016 for SMAPI 0.37.3 or later. Updated by CJBok.

* Added 'one hit break' option.
* Fixed diagonal movement.
* Fixed player speed effected while walking.
* Removed `Nini.dll` (integrated into main DLL).

## 1.4.1
Released 08 March 2016 for SMAPI 0.37.3 or later. Updated by CJBok.

* Fixed fish biting instantly when cheat isn't active.

## 1.4
Released 08 March 2016 for SMAPI 0.37.3 or later. Updated by CJBok.

* Added fish 'always treasure' option.
* Added instant catch also making fish bite instantly.
* Fixed time frozen dialog overlapping mine level dialog.
* Fixed durable fences cheat not working when not on farm.

## 1.3.2
Released 07 March 2016 for SMAPI 0.37.2 or later. Updated by CJBok.

* Added many warp locations.
* Added 'always give gift' option.
* Added time freeze options.
* Added shortcut key to freeze time.
* Changed weather options to next day settings.
* Fixed movement speed still active during events.
