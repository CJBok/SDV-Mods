[← back to readme](README.md)

# Release notes
## 1.24
Released 14 April 2020 for Stardew Valley 3.4 or later. Updated by Pathoschild.

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
Released 18 February 2020 for SMAPI 3.2 or later. Updated by Pathoschild.

* Fixed 'fast casks' not completing until in-game clock change after 1.23.

## 1.23
Released 17 February 2020 for SMAPI 3.2 or later. Updated by Pathoschild.

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
Released 09 February 2020 for SMAPI 3.2 or later. Updated by Pathoschild.

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
Released 26 November 2019 for SMAPI 3.0 or later. Updated by Pathoschild.

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
Released 12 June 2019 for SMAPI 2.11.1 or later. Updated by Pathoschild.

* Reworked community center flag options.
* Fixed setting a community center flag not completing the in-game area.
* Fixed 'unlock community center' option not working correctly.
* Fixed instant-grow-crop not working with garden pots placed on tilled dirt or flooring.
* Improved translations. Thanks to shirutan (updated Japanese)!

## 1.20
Released 10 June 2019 for SMAPI 2.11.1 or later. Updated by Pathoschild.

* Added 'advanced' tab to set flags and wallet items; merged 'quests' tab into 'advanced'.
* Fixed max relationship meter not extended for spouse.
* Fixed being able to open the menu when a minigame is active.
* Improved translations. Thanks to S2SKY (added Korean) and TheOzonO3 (updated Russian)!

## 1.19
Released 27 March 2019 for SMAPI 2.11 or later. Updated by Pathoschild.

* Added support for setting relationships for unmet villagers.
* Fast machine processing is now much faster.
* Fast machine processing now continues working when time is paused.
* Fast machine list is now sorted by name.
* Fixed land swimming bug when warping out of the spa.
* Improved translations. Thanks to kelvindules (updated Portuguese) and VincentRoth (added French)!

## 1.18.3
Released 09 December 2018 for SMAPI 2.9 or later. Updated by Pathoschild.

* Updated for the upcoming SMAPI 3.0.
* Fixed harvest with scythe option saying 'no XP gain', which was fixed in Stardew Valley 1.3. (Thanks to SkpFX!)
* Improved translations. Thanks to Nanogamer7 (added German) and Redlnn (improved Chinese)!

## 1.18.2
Released 03 November 2018 for SMAPI 2.8 or later. Updated by Pathoschild.

* Improved translations. Thanks to Spa51 (added Spanish)!

## 1.18.1
Released 28 August 2018 for SMAPI 2.8 or later. Updated by Pathoschild.

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
Released 11 February 2018 for SMAPI 2.4 or later. Updated by Pathoschild.

* Updated to SMAPI 2.4.
* Added translation support.
* Added update checks via SMAPI.
* Added options to change player's professions.
* Fixed issue where setting the time could leave NPCs confused (e.g. stuck in bed).
* Improved translations. Thanks to XxIceGladiadorxX (added Portuguese)!

## 1.16
Released 14 July 2017 for SMAPI 1.15 or later. Updated by Pathoschild.

* Fixed open-menu key working even when another menu is already open.
* Fixed freeze-time key working during cutscenes.

## 1.15
Released 27 May 2017 for SMAPI 1.13 or later. Updated by Pathoschild.

* Updated for Stardew Valley 1.2 and SMAPI 1.13.
* Added option to set the default tab when opening the menu.
* Fixed relationship slider always set to 10 hearts.
* Fixed 'one hit break' no longer working on stones, trees, logs, and stumps.
* Fixed 'one hit break' not working on fruit trees.

## 1.14.1
Released 12 April 2017 for SMAPI 1.9 or later. Updated by Pathoschild.

* Fixed error when used with any mod that adds multiple seeds producing the same crop.

## 1.14
Released 05 April 2017 for SMAPI 1.9 or later. Updated by Pathoschild.

* Updated to SMAPI 1.9.
* Fast machines now work anywhere, not only on the farm.
* Fixed fast cask cheat not working.
* Fixed disabling the 'harvest with scythe' option not restoring existing crops to normal.
* Internal refactoring.

## 1.13
Released 04 January 2017 for SMAPI 1.5 or later. Updated by Pathoschild.

* Updated to Stardew Valley 1.1+ and SMAPI 1.5.
* Added compatibility with Linux and Mac.
* Added support for casks and worm bins.
* Fixed instantly-grown fruit trees not producing fruit until their normal growth date.
* Fixed 'grow crops' key in settings menu not being saved.

## 1.12
Released 09 April 2016 for SMAPI 0.40 or later.

* Updated to Stardew Valley 1.07+ and SMAPI 0.40.0+.
* Added cheats: durable tackles, harvest with scythe, grow tree, and grow crops.
* Added changeable relationships.
* Fixed mouse cursor showing when disabled.

## 1.11
Released 02 April 2016 for SMAPI 0.39.6 or later.

* Updated for SMAPI 0.39.6.
* Added cheats: fast tapper, fast lightning rod, always auto-feed, and infinite hay.
* Added casino coins cheats.
* Added warps to bathhouse, Sandy, and casino.
* Added time slider.
* Fixed time frozen label.

## 1.10
Released 30 March 2016 for SMAPI 0.39.4 or later.

* Updated for SMAPI 0.39.4.
* Added cheats: no friendship decay, instant build.
* Fixed fast machine processing in barns and coops.

## 1.9
* Updated for SMAPI 0.39.1.

## 1.8.2
* Fixed cheats not working in the greenhouse.

## 1.8.1
* Fixed cheats not working in the greenhouse.

## 1.8
* Updated to Stardew Valley 1.0.6 and SMAPI 0.38.3.
* Fixed watering fields in the greenhouse.
* Fixed fast machine processing.
* Removed the `nini.dll` file (no longer needed, now uses save method in latest SMAPI update).

## 1.7.1
* Fixed fast machine processing.
* Fixed watering fields in the greenhouse.

## 1.7
* Added fast processing for all machines (each toggleable).
* Added skill reset.
* You can now switch between categories with controller (shoulder triggers).
* Fixed leveling up skills.
* Fixed watering fields in some cases.

## 1.6
* Menu now contains categories (last scroll position and category will be remembered).
* Fixed 'always treasure' cheat.

## 1.5.3
* Fixed sound bug during fishing.
* The cheats menu now remembers scroll position.

## 1.5.2
* Fixed player getting stuck during events.
* Fixed sounds stuck during fishing.

## 1.5.1
* Restored `Nini.dll` in download since some users still need it.
* Added 'instant bite' option, separate from 'instant catch' option.
* Fixed wrong mod version shown on load.
* Changed 'debris' weather name to 'windy'.

## 1.5
* Added 'one hit break' option.
* Fixed diagonal movement.
* Fixed player speed effected while walking.
* Removed `Nini.dll` (integrated into main DLL).

## 1.4.1
* Fixed fish biting instantly when cheat isn't active.

## 1.4
* Added fish 'always treasure' option.
* Added instant catch also making fish bite instantly.
* Fixed time frozen dialog overlapping mine level dialog.
* Fixed durable fences cheat not working when not on farm.

## 1.3.2
* Added many warp locations.
* Added 'always give gift' option.
* Added time freeze options.
* Added shortcut key to freeze time.
* Changed weather options to next day settings.
* Fixed movement speed still active during events.
