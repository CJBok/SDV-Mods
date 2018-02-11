using System;
using System.Collections.Generic;
using System.Linq;
using CJBAutomation.Framework;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Locations;
using StardewValley.Objects;
using SObject = StardewValley.Object;

namespace CJBAutomation
{
    internal class ModEntry : Mod
    {
        /*********
        ** Accessors
        *********/
        internal static ModConfig Config;


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            ModEntry.Config = helper.ReadConfig<ModConfig>();

            TimeEvents.TimeOfDayChanged += TimeEvents_TimeOfDayChanged;
        }


        /*********
        ** Private methods
        *********/
        private void TimeEvents_TimeOfDayChanged(object sender, EventArgsIntChanged e)
        {
            List<GameLocation> locations = new List<GameLocation>();

            foreach (GameLocation location in Game1.locations)
            {
                if (!location.IsFarm && !location.name.Contains("GreenHouse") && location.name != "Cellar")
                    continue;

                locations.Add(location);

                if (location is BuildableGameLocation buildableLocation)
                {
                    foreach (Building build in buildableLocation.buildings)
                    {
                        if (build.indoors != null)
                            locations.Add(build.indoors);
                    }
                }
            }

            foreach (GameLocation location in locations)
            {
                foreach (KeyValuePair<Vector2, SObject> pair in location.objects)
                {
                    if (pair.Value == null)
                        continue;
                    ProcessObject(location, pair.Key, pair.Value);
                }
            }
            locations.Clear();
        }

        private void ProcessObject(GameLocation location, Vector2 tile, SObject obj)
        {
            switch (obj.name)
            {
                case "Furnace":
                    {
                        List<Chest> chests = Automation.GetConnectedChests(location, tile);
                        if (obj.heldObject != null && obj.readyForHarvest)
                        {
                            foreach (Chest chest in chests)
                            {
                                if (chest.addItem(obj.heldObject) == null)
                                {
                                    obj.heldObject = null;
                                    obj.readyForHarvest = false;
                                    break;
                                }
                            }
                        }
                        if (obj.heldObject == null && !obj.readyForHarvest)
                        {
                            if (Automation.DoChestsHaveItem(chests, 382, 1))
                            {
                                if (Automation.DoChestsHaveItem(chests, 378, 5))
                                {
                                    Automation.RemoveItemFromChests(chests, 382);
                                    Automation.RemoveItemFromChests(chests, 378, 5);
                                    obj.heldObject = new SObject(Vector2.Zero, 334, 1);
                                    obj.minutesUntilReady = 30;
                                }
                                else if (Automation.DoChestsHaveItem(chests, 380, 5))
                                {
                                    Automation.RemoveItemFromChests(chests, 382);
                                    Automation.RemoveItemFromChests(chests, 380, 5);
                                    obj.heldObject = new SObject(Vector2.Zero, 335, 1);
                                    obj.minutesUntilReady = 120;
                                }
                                else if (Automation.DoChestsHaveItem(chests, 384, 5))
                                {
                                    Automation.RemoveItemFromChests(chests, 382);
                                    Automation.RemoveItemFromChests(chests, 384, 5);
                                    obj.heldObject = new SObject(Vector2.Zero, 336, 1);
                                    obj.minutesUntilReady = 300;
                                }
                                else if (Automation.DoChestsHaveItem(chests, 386, 5))
                                {
                                    Automation.RemoveItemFromChests(chests, 382);
                                    Automation.RemoveItemFromChests(chests, 386, 5);
                                    obj.heldObject = new SObject(Vector2.Zero, 337, 1);
                                    obj.minutesUntilReady = 480;
                                }
                                else if (Automation.DoChestsHaveItem(chests, 80, 1))
                                {
                                    Automation.RemoveItemFromChests(chests, 382);
                                    Automation.RemoveItemFromChests(chests, 80);
                                    obj.heldObject = new SObject(Vector2.Zero, 338, "Refined Quartz", false, true, false, false);
                                    obj.minutesUntilReady = 90;
                                }
                            }

                            if (obj.heldObject != null)
                            {
                                obj.initializeLightSource(tile);
                                obj.showNextIndex = true;
                            }
                        }
                    }
                    break;

                case "Crystalarium":
                    {
                        List<Chest> chests = Automation.GetConnectedChests(location, tile);
                        if (obj.heldObject != null && obj.readyForHarvest)
                        {
                            foreach (Chest chest in chests)
                            {
                                if (chest.addItem(obj.heldObject.getOne()) == null)
                                {
                                    obj.minutesUntilReady = Automation.GetMinutesForCrystalarium(obj.heldObject.parentSheetIndex);
                                    obj.readyForHarvest = false;
                                    break;
                                }
                            }
                        }
                    }
                    break;

                case "Mayonnaise Machine":
                    {
                        List<Chest> chests = Automation.GetConnectedChests(location, tile);
                        if (obj.heldObject != null && obj.readyForHarvest)
                        {
                            foreach (Chest chest in chests)
                            {
                                if (chest.addItem(obj.heldObject) == null)
                                {
                                    obj.heldObject = null;
                                    obj.readyForHarvest = false;
                                    break;
                                }
                            }
                        }
                        if (obj.heldObject == null && !obj.readyForHarvest)
                        {
                            if (Automation.RemoveItemFromChests(chests, 176) || Automation.RemoveItemFromChests(chests, 180))
                            {
                                // small white egg / small brown egg -> normal mayonnaise
                                obj.heldObject = new SObject(Vector2.Zero, 306, null, false, true, false, false);
                                obj.minutesUntilReady = 180;
                            }
                            else
                            if (Automation.RemoveItemFromChests(chests, 107) || Automation.RemoveItemFromChests(chests, 174) || Automation.RemoveItemFromChests(chests, 182))
                            {
                                // dino egg / white large egg / brown large egg -> gold quality mayonnaise
                                obj.heldObject = new SObject(Vector2.Zero, 306, null, false, true, false, false)
                                {
                                    quality = 2
                                };
                                obj.minutesUntilReady = 180;
                            }
                            else
                            if (Automation.RemoveItemFromChests(chests, 442))
                            {
                                // duck egg -> duck mayonnaise
                                obj.heldObject = new SObject(Vector2.Zero, 307, null, false, true, false, false);
                                obj.minutesUntilReady = 180;
                            }
                            else
                            if (Automation.RemoveItemFromChests(chests, 305))
                            {
                                // void egg -> void mayonnaise
                                obj.heldObject = new SObject(Vector2.Zero, 308, null, false, true, false, false);
                                obj.minutesUntilReady = 180;
                            }
                        }
                    }
                    break;

                case "Keg":
                    {
                        List<Chest> chests = Automation.GetConnectedChests(location, tile);
                        if (obj.heldObject != null && obj.readyForHarvest)
                        {
                            foreach (Chest chest in chests)
                            {
                                if (chest.addItem(obj.heldObject) == null)
                                {
                                    obj.heldObject = null;
                                    obj.readyForHarvest = false;
                                    break;
                                }
                            }
                        }
                        if (obj.heldObject == null && !obj.readyForHarvest)
                        {
                            foreach (var chest in chests)
                            {
                                foreach (var stack in chest.items)
                                {
                                    switch (stack.parentSheetIndex)
                                    {
                                        case 340: // honey, regardless of flower type
                                            obj.heldObject = new SObject(Vector2.Zero, 459, "Mead", false, true, false, false);
                                            obj.minutesUntilReady = 600;
                                            obj.heldObject.name = "Mead";
                                            break;
                                        case 262: // wheat
                                            obj.heldObject = new SObject(Vector2.Zero, 346, "Beer", false, true, false, false);
                                            obj.minutesUntilReady = 1750;
                                            obj.heldObject.name = "Beer";
                                            break;
                                        case 304: // hops
                                            obj.heldObject = new SObject(Vector2.Zero, 303, "Pale Ale", false, true, false, false);
                                            obj.minutesUntilReady = 2250;
                                            obj.heldObject.name = "Pale Ale";
                                            break;
                                        case 433: // coffee bean
                                            obj.heldObject = new SObject(Vector2.Zero, 395, "Coffee", false, true, false, false);
                                            obj.heldObject.name = "Coffee";
                                            obj.minutesUntilReady = 120;
                                            break;
                                        case 256: // tomato, default would produce tomato wine instead of juice
                                            obj.heldObject = new SObject(Vector2.Zero, 350, stack.Name + " Juice", false, true, false, false);
                                            obj.heldObject.Price = (int)(((SObject)stack).Price * 2.25d);
                                            obj.heldObject.name = stack.Name + " Juice";
                                            obj.minutesUntilReady = 6000;
                                            break;
                                        case 260: // hot pepper, default would produce juice instead of wine
                                            obj.heldObject = new SObject(Vector2.Zero, 348, stack.Name + " Wine", false, true, false, false);
                                            obj.heldObject.Price = ((SObject)stack).Price * 3;
                                            obj.heldObject.name = stack.Name + " Wine";
                                            obj.minutesUntilReady = 10000;
                                            break;
                                        default:
                                            if (stack.category == -79) // fruit
                                            {
                                                obj.heldObject = new SObject(Vector2.Zero, 348, stack.Name + " Wine", false, true, false, false);
                                                obj.heldObject.name = stack.Name + " Wine";
                                                obj.heldObject.Price = ((SObject)stack).Price * 3;
                                                obj.minutesUntilReady = 10000;
                                            }
                                            else if (stack.category == -75) // veggie
                                            {
                                                obj.heldObject = new SObject(Vector2.Zero, 350, stack.Name + " Juice", false, true, false, false);
                                                obj.heldObject.Price = (int)(((SObject)stack).Price * 2.25d);
                                                obj.heldObject.name = stack.Name + " Juice";
                                                obj.minutesUntilReady = 6000;
                                            }
                                            break;
                                    }
                                    if (obj.heldObject != null) // we have put something in the keg, no need to loop over more chests/stacks
                                    {
                                        Automation.DecreaseStack(chest, stack);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    break;

                case "Charcoal Kiln":
                    {
                        List<Chest> chests = Automation.GetConnectedChests(location, tile);
                        if (obj.heldObject != null && obj.readyForHarvest)
                        {
                            foreach (Chest chest in chests)
                            {
                                if (chest.addItem(obj.heldObject) == null)
                                {
                                    obj.heldObject = null;
                                    obj.readyForHarvest = false;
                                    break;
                                }
                            }
                        }
                        if (obj.heldObject == null && !obj.readyForHarvest)
                        {
                            if (Automation.RemoveItemFromChests(chests, 388, 10))
                            {
                                obj.heldObject = new SObject(382, 1);
                                obj.minutesUntilReady = 30;
                                obj.showNextIndex = true;
                            }
                        }
                    }
                    break;

                case "Cheese Press":
                    {
                        List<Chest> chests = Automation.GetConnectedChests(location, tile);
                        if (obj.heldObject != null && obj.readyForHarvest)
                        {
                            foreach (Chest chest in chests)
                            {
                                if (chest.addItem(obj.heldObject) == null)
                                {
                                    obj.heldObject = null;
                                    obj.readyForHarvest = false;
                                    break;
                                }
                            }
                        }
                        if (obj.heldObject == null && !obj.readyForHarvest)
                        {
                            if (Automation.RemoveItemFromChests(chests, 184))
                            {
                                obj.heldObject = new SObject(Vector2.Zero, 424, null, false, true, false, false);
                                obj.minutesUntilReady = 200;
                            }
                            else if (Automation.RemoveItemFromChests(chests, 186))
                            {
                                obj.heldObject = new SObject(Vector2.Zero, 424, "Cheese (=)", false, true, false, false)
                                {
                                    quality = 2
                                };
                                obj.minutesUntilReady = 200;
                            }
                            else if (Automation.RemoveItemFromChests(chests, 436))
                            {
                                obj.heldObject = new SObject(Vector2.Zero, 426, null, false, true, false, false);
                                obj.minutesUntilReady = 200;
                            }
                            else if (Automation.RemoveItemFromChests(chests, 438))
                            {
                                obj.heldObject = new SObject(Vector2.Zero, 426, null, false, true, false, false)
                                {
                                    quality = 2
                                };
                                obj.minutesUntilReady = 200;
                            }
                        }
                    }
                    break;

                case "Preserves Jar":
                    {
                        List<Chest> chests = Automation.GetConnectedChests(location, tile);
                        if (obj.heldObject != null && obj.readyForHarvest)
                        {
                            foreach (Chest chest in chests)
                            {
                                if (chest.addItem(obj.heldObject) == null)
                                {
                                    obj.heldObject = null;
                                    obj.readyForHarvest = false;
                                    break;
                                }
                            }
                        }
                        if (obj.heldObject == null && !obj.readyForHarvest)
                        {
                            SObject item =
                                (SObject)Automation.GetItemFromChestsByCategory(chests, -79, -1)
                                ?? (SObject)Automation.GetItemFromChestsByCategory(chests, -75, -1);

                            if (item != null)
                            {
                                if (item.category == -79)
                                {
                                    obj.heldObject = new SObject(Vector2.Zero, 344, item.Name + " Jelly", false, true, false, false);
                                    obj.heldObject.Price = 50 + item.Price * 2;
                                    obj.heldObject.name = item.Name + " Jelly";
                                    obj.minutesUntilReady = 4000;
                                    Automation.RemoveItemFromChestsCategory(chests, -79, -1);
                                }
                                if (item.category == -75)
                                {
                                    obj.heldObject = new SObject(Vector2.Zero, 342, "Pickled " + item.Name, false, true, false, false);
                                    obj.heldObject.Price = 50 + item.Price * 2;
                                    obj.heldObject.name = "Pickled " + item.Name;
                                    obj.minutesUntilReady = 4000;
                                    Automation.RemoveItemFromChestsCategory(chests, -75, -1);
                                }
                            }
                        }
                    }
                    break;

                case "Loom":
                    {
                        List<Chest> chests = Automation.GetConnectedChests(location, tile);
                        if (obj.heldObject != null && obj.readyForHarvest)
                        {
                            foreach (Chest chest in chests)
                            {
                                if (chest.addItem(obj.heldObject) == null)
                                {
                                    obj.heldObject = null;
                                    obj.readyForHarvest = false;
                                    obj.showNextIndex = false;
                                    break;
                                }
                            }
                        }
                        if (obj.heldObject == null && !obj.readyForHarvest)
                        {
                            if (Automation.RemoveItemFromChests(chests, 440))
                            {
                                obj.heldObject = new SObject(Vector2.Zero, 428, null, false, true, false, false);
                                obj.minutesUntilReady = 240;
                                obj.showNextIndex = true;
                            }
                        }
                    }
                    break;

                case "Bee House":
                    {
                        if (Game1.currentSeason != "winter")
                        {
                            List<Chest> chests = Automation.GetConnectedChests(location, tile);
                            if (obj.heldObject != null && obj.readyForHarvest)
                            {
                                foreach (Chest chest in chests)
                                {
                                    if (chest.items.Count < 36)
                                    {
                                        string str = "Wild";
                                        int price = 0;
                                        if (location is Farm)
                                        {
                                            Crop crop = Utility.findCloseFlower(obj.tileLocation);
                                            if (crop != null)
                                            {
                                                str = Game1.objectInformation[crop.indexOfHarvest].Split('/')[0];
                                                price = Convert.ToInt32(Game1.objectInformation[crop.indexOfHarvest].Split('/')[1]) * 2;
                                            }
                                        }
                                        obj.heldObject.name = str + " Honey";
                                        obj.heldObject.price += price;
                                        if (chest.addItem(obj.heldObject) == null)
                                        {
                                            obj.heldObject = new SObject(Vector2.Zero, 340, null, false, true, false, false);
                                            if (Game1.currentSeason == "winter")
                                            {
                                                obj.heldObject = null;
                                            }
                                            obj.minutesUntilReady = 2400 - Game1.timeOfDay + 4320;
                                            obj.readyForHarvest = false;
                                            obj.showNextIndex = false;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    }

                case "Worm Bin":
                    {
                        List<Chest> chests = Automation.GetConnectedChests(location, tile);
                        if (obj.heldObject != null && obj.readyForHarvest)
                        {
                            foreach (Chest chest in chests)
                            {
                                if (chest.addItem(obj.heldObject) == null)
                                {
                                    obj.heldObject = new SObject(685, Game1.random.Next(2, 6));
                                    obj.minutesUntilReady = 2400 - Game1.timeOfDay;
                                    obj.readyForHarvest = false;
                                    obj.showNextIndex = false;
                                    break;
                                }
                            }
                        }
                    }
                    break;

                case "Seed Maker":
                    {
                        List<Chest> chests = Automation.GetConnectedChests(location, tile);
                        if (obj.heldObject != null && obj.readyForHarvest)
                        {
                            foreach (Chest chest in chests)
                            {
                                if (chest.addItem(obj.heldObject) == null)
                                {
                                    obj.heldObject = null;
                                    obj.readyForHarvest = false;
                                    break;
                                }
                            }
                        }
                        if (obj.heldObject == null && !obj.readyForHarvest)
                        {
                            int seedId = Automation.RemoveItemFromChestsIfCrop(chests);
                            if (seedId != -1)
                            {
                                Random random = new Random((int)(Game1.stats.DaysPlayed + (uint)((int)Game1.uniqueIDForThisGame / 2) + (uint)((int)obj.tileLocation.X) + (uint)((int)obj.tileLocation.Y * 77) + (uint)Game1.timeOfDay));
                                obj.heldObject = new SObject(seedId, random.Next(1, 4));
                                if (random.NextDouble() < 0.005)
                                {
                                    obj.heldObject = new SObject(499, 1);
                                }
                                else if (random.NextDouble() < 0.02)
                                {
                                    obj.heldObject = new SObject(770, random.Next(1, 5));
                                }
                                obj.minutesUntilReady = 20;
                            }
                        }
                    }
                    break;

                case "Recycling Machine":
                    {
                        List<Chest> chests = Automation.GetConnectedChests(location, tile);
                        if (obj.heldObject != null && obj.readyForHarvest)
                        {
                            foreach (Chest chest in chests)
                            {
                                if (chest.addItem(obj.heldObject) == null)
                                {
                                    obj.heldObject = null;
                                    obj.readyForHarvest = false;
                                    break;
                                }
                            }
                        }
                        if (obj.heldObject == null && !obj.readyForHarvest)
                        {
                            Random random2 = new Random((int)Game1.uniqueIDForThisGame / 2 + (int)Game1.stats.DaysPlayed + Game1.timeOfDay + (int)obj.tileLocation.X * 200 + (int)obj.tileLocation.Y);
                            if (Automation.RemoveItemFromChests(chests, 168))
                            {
                                obj.heldObject = new SObject((random2.NextDouble() < 0.3) ? 382 : ((random2.NextDouble() < 0.3) ? 380 : 390), random2.Next(1, 4));
                                obj.minutesUntilReady = 60;
                                Game1.stats.PiecesOfTrashRecycled += 1u;
                            }
                            else if (Automation.RemoveItemFromChests(chests, 169))
                            {
                                obj.heldObject = new SObject((random2.NextDouble() < 0.25) ? 382 : 388, random2.Next(1, 4));
                                obj.minutesUntilReady = 60;
                                Game1.stats.PiecesOfTrashRecycled += 1u;
                            }
                            else if (Automation.RemoveItemFromChests(chests, 170) || Automation.RemoveItemFromChests(chests, 171))
                            {
                                obj.heldObject = new SObject(338, 1);
                                obj.minutesUntilReady = 60;
                                Game1.stats.PiecesOfTrashRecycled += 1u;
                            }
                            else if (Automation.RemoveItemFromChests(chests, 172))
                            {
                                obj.heldObject = ((random2.NextDouble() < 0.1) ? new SObject(428, 1) : new Torch(Vector2.Zero, 3));
                                obj.minutesUntilReady = 60;
                                Game1.stats.PiecesOfTrashRecycled += 1u;
                            }
                        }
                    }
                    break;

                case "Oil Maker":
                    {
                        List<Chest> chests = Automation.GetConnectedChests(location, tile);
                        if (obj.heldObject != null && obj.readyForHarvest)
                        {
                            foreach (Chest chest in chests)
                            {
                                if (chest.addItem(obj.heldObject) == null)
                                {
                                    obj.heldObject = null;
                                    obj.readyForHarvest = false;
                                    break;
                                }
                            }
                        }
                        if (obj.heldObject == null && !obj.readyForHarvest)
                        {
                            if (Automation.RemoveItemFromChests(chests, 270))
                            {
                                obj.heldObject = new SObject(Vector2.Zero, 247, null, false, true, false, false);
                                obj.minutesUntilReady = 1000;
                            }
                            else if (Automation.RemoveItemFromChests(chests, 421))
                            {
                                obj.heldObject = new SObject(Vector2.Zero, 247, null, false, true, false, false);
                                obj.minutesUntilReady = 60;
                            }
                            else if (Automation.RemoveItemFromChests(chests, 430))
                            {
                                obj.heldObject = new SObject(Vector2.Zero, 432, null, false, true, false, false);
                                obj.minutesUntilReady = 360;
                            }
                            else if (Automation.RemoveItemFromChests(chests, 431))
                            {
                                obj.heldObject = new SObject(247, 1);
                                obj.minutesUntilReady = 3200;
                            }
                        }
                    }
                    break;

                case "Tapper":
                    {
                        List<Chest> chests = Automation.GetConnectedChests(location, tile);
                        if (obj.heldObject != null && obj.readyForHarvest)
                        {
                            foreach (Chest chest in chests)
                            {
                                if (chest.addItem(obj.heldObject) == null)
                                {

                                    int id = obj.heldObject.parentSheetIndex;

                                    if (id == 724)
                                        obj.minutesUntilReady = 16000 - Game1.timeOfDay;
                                    else if (id == 725)
                                        obj.minutesUntilReady = 13000 - Game1.timeOfDay;
                                    else if (id == 726)
                                        obj.minutesUntilReady = 10000 - Game1.timeOfDay;
                                    else if (id == 422)
                                    {
                                        obj.minutesUntilReady = 3000 - Game1.timeOfDay;
                                        obj.heldObject = new SObject(420, 1);
                                    }
                                    else if (id == 404 || id == 420)
                                    {
                                        obj.minutesUntilReady = 3000 - Game1.timeOfDay;
                                        if (Game1.currentSeason != "fall")
                                        {
                                            obj.heldObject = new SObject(404, 1);
                                            obj.minutesUntilReady = 6000 - Game1.timeOfDay;
                                        }
                                        if (Game1.dayOfMonth % 10 == 0)
                                            obj.heldObject = new SObject(422, 1);
                                        if (Game1.currentSeason == "winter")
                                            obj.minutesUntilReady = 80000 - Game1.timeOfDay;
                                    }
                                    if (obj.heldObject != null)
                                        obj.heldObject = (SObject)obj.heldObject.getOne();
                                    obj.readyForHarvest = false;
                                    break;
                                }
                            }
                        }
                    }
                    break;

                case "Lightning Rod":
                    {
                        List<Chest> chests = Automation.GetConnectedChests(location, tile);
                        if (obj.heldObject != null && obj.readyForHarvest)
                        {
                            foreach (Chest chest in chests)
                            {
                                if (chest.addItem(obj.heldObject.getOne()) == null)
                                {
                                    obj.heldObject = null;
                                    obj.readyForHarvest = false;
                                    break;
                                }
                            }
                        }
                    }
                    break;

                case "Cask":
                    {
                        if (location.Name != "Cellar")
                            return;
                        var chests = Automation.GetConnectedChests(location, tile);
                        if (obj.heldObject != null && obj.heldObject.quality == 4)
                        {
                            foreach (var chest in chests)
                            {
                                if (chest.addItem(obj.heldObject.getOne()) == null)
                                {
                                    obj.heldObject = null;
                                    obj.minutesUntilReady = 0;
                                    obj.readyForHarvest = false;
                                    ((Cask)obj).agingRate = 0.0f;
                                    ((Cask)obj).daysToMature = 0.0f;
                                    break;
                                }
                            }
                        }
                        if (obj.heldObject == null)
                        {
                            foreach (var chest in chests)
                            {
                                foreach (var stack in chest.items)
                                {
                                    if ((((SObject)stack).quality) == 4) // skip already-iridium stuff
                                        continue;
                                    float agingRate = -1f;
                                    switch (stack.parentSheetIndex)
                                    {
                                        case 424: // cheese
                                        case 426: // goat cheese
                                            agingRate = 4f;
                                            break;
                                        case 459: // mead
                                        case 346: // beer
                                            agingRate = 2f;
                                            break;
                                        case 303: // pale ale
                                            agingRate = 1.66f;
                                            break;
                                        case 348: // wine
                                            agingRate = 1f;
                                            break;
                                    }
                                    if (agingRate < 0) // not something we could age
                                        continue;
                                    obj.heldObject = (SObject)stack.getOne();
                                    switch (((SObject)stack).quality)
                                    {
                                        case 0:
                                            ((Cask)obj).daysToMature = 56f;
                                            break;
                                        case 1:
                                            ((Cask)obj).daysToMature = 42f;
                                            break;
                                        case 2:
                                            ((Cask)obj).daysToMature = 28f;
                                            break;
                                    }
                                    ((Cask)obj).agingRate = agingRate;
                                    obj.minutesUntilReady = 999999;
                                    Automation.DecreaseStack(chest, stack);
                                    return;
                                }
                            }
                        }
                    }
                    break;

                case "Slime Egg-Press":
                    {
                        List<Chest> chests = Automation.GetConnectedChests(location, tile);
                        if (obj.heldObject != null && obj.readyForHarvest)
                        {
                            foreach (Chest chest in chests)
                            {
                                if (chest.addItem(obj.heldObject.getOne()) == null)
                                {
                                    obj.heldObject = null;
                                    obj.readyForHarvest = false;
                                    break;
                                }
                            }
                        }
                        if (obj.heldObject == null)
                        {
                            if (Automation.RemoveItemFromChestsByName(chests, "Slime", -1, 99))
                            {
                                int parentSheetIndex = 680;
                                if (Game1.random.NextDouble() < 0.05)
                                    parentSheetIndex = 439;
                                else if (Game1.random.NextDouble() < 0.1)
                                    parentSheetIndex = 437;
                                else if (Game1.random.NextDouble() < 0.25)
                                    parentSheetIndex = 413;
                                obj.heldObject = new SObject(parentSheetIndex, 1);
                                obj.minutesUntilReady = 1200;
                            }
                        }
                    }
                    break;

                default:
                    if (obj.Name.Contains("Mushroom Box"))
                    {
                        if (obj.heldObject != null && obj.readyForHarvest)
                        {
                            IEnumerable<Chest> chests = Automation.GetConnectedChests(location, tile);
                            if (!chests.Any())
                            {
                                // two possible modes
                                // - output mushrooms to adjancent chests like other machines (for realism)
                                // - output to any single chest inside the mushroom cave to save space, but only as long as there is
                                //   exactly one in the whole cave
                                chests = Automation.GetChestsInLocation(location);
                                if (chests.Skip(1).Any())
                                    return;
                            }
                            foreach (Chest chest in chests)
                            {
                                if (chest.addItem(obj.heldObject) == null)
                                {
                                    obj.heldObject = null;
                                    obj.readyForHarvest = false;
                                    obj.minutesUntilReady = -1;
                                    obj.showNextIndex = false;
                                    break;
                                }
                            }
                        }
                    }
                    break;
            }
        }
    }
}
