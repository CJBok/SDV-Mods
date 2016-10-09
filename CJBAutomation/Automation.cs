using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Objects;
using System;
using System.Collections.Generic;

namespace CJBAutomation {
    public class Automation {

        private static Dictionary<int, int> cropData;

        public static List<Chest> GetChestsFromSurroundingLocation(GameLocation loc, Vector2 vec) {
            List<Chest> chests = new List<Chest>();

            if (loc == null || vec == null)
                return chests;

            for (int x = -1; x <= 1; x++) {
                for (int y = -1; y <= 1; y++) {
                    if ((CJBAutomation.config.diagonal || (x == 0 || y == 0)) && !(x == 0 && y == 0)) {
                        Vector2 index = new Vector2(vec.X - x, vec.Y - y);
                        if (loc.objects.ContainsKey(index)) {
                            StardewValley.Object o = loc.objects[index];
                            if (o is Chest)
                                chests.Add((Chest)o);
                        }
                    }
                }
            }
            return chests;
        }

        public static bool DoesChestsHaveItem(List<Chest> chests, int index, int stack) {
            foreach (Chest chest in chests) {
                foreach (Item item in chest.items) {
                    if (item.parentSheetIndex == index && item.Stack >= stack)
                        return true;
                }
            }
            return false;
        }

        public static bool RemoveItemFromChests(List<Chest> chests, int index, int stack = 1) {
            foreach (Chest chest in chests) {
                foreach (Item item in chest.items) {
                    if (item.parentSheetIndex == index && item.Stack >= stack) {
                        item.Stack -= stack;
                        if (item.Stack <= 0)
                            chest.items.Remove(item);
                        chest.clearNulls();
                        return true;
                    }
                }
            }
            return false;
        }

        public static Item GetItemFromChestsByCategory(List<Chest> chests, int category, int excludeid) {
            foreach (Chest chest in chests) {
                foreach (Item item in chest.items) {
                    if (item.category == category && item.parentSheetIndex != excludeid)
                        return item.getOne();
                }
            }
            return null;
        }

        public static void RemoveItemFromChestsCategory(List<Chest> chests, int category, int excludeid) {
            foreach (Chest chest in chests) {
                foreach (Item item in chest.items) {
                    if (item.category == category && item.parentSheetIndex != excludeid) {
                        item.Stack -= 1;
                        if (item.Stack <= 0)
                            chest.items.Remove(item);
                        chest.clearNulls();
                        return;
                    }
                }
            }
            return;
        }

        public static Item GetItemFromChestsByName(List<Chest> chests, string name, int excludeid) {
            foreach (Chest chest in chests) {
                foreach (Item item in chest.items) {
                    if (item.Name == null) continue;
                    if (item.Name == name && item.parentSheetIndex != excludeid)
                        return item.getOne();
                }
            }
            return null;
        }

        public static bool ChestsHaveEnoughItemsByName(List<Chest> chests, string name, int excludeid, int stack)
        {
            int stack_tmp = 0;
            foreach (Chest chest in chests)
            {
                foreach (Item item in chest.items)
                {
                    if (item.Name == null)
                        continue;
                    if (item.Name == name && item.parentSheetIndex != excludeid)
                        stack_tmp += item.Stack;
                    if (stack_tmp >= stack)
                        return true;
                }
            }
            return false;
        }

        public static bool RemoveItemFromChestsByName(List<Chest> chests, string name, int excludeid, int stack = 1) {
            if (stack > 1 && !ChestsHaveEnoughItemsByName(chests, name, excludeid, stack))
                return false;

            foreach (Chest chest in chests) {
                var toRemove = new List<Item>();
                foreach (Item item in chest.items) {
                    if (item.Name == null) continue;
                    if (item.Name == name && item.parentSheetIndex != excludeid) {
                        int remove = Math.Min(stack, item.Stack);
                        item.Stack -= remove;
                        stack -= remove;
                        if (item.Stack <= 0)
                            toRemove.Add(item);
                        if (stack <= 0)
                        {
                            foreach (var victim in toRemove)
                                chest.items.Remove(victim);
                            chest.clearNulls();
                            return true;
                        }
                    }
                }
                foreach (var victim in toRemove)
                    chest.items.Remove(victim);
                chest.clearNulls();
            }
            return false;
        }

        public static int RemoveItemFromChestsIfCrop(List<Chest> chests) {
            foreach (Chest chest in chests) {
                foreach (Item item in chest.items) {
                    if (item.Name == null) continue;
                    int seedId = getSeedIdFromCropId(item.parentSheetIndex);
                    if (seedId != -1) {
                        item.Stack -= 1;
                        if (item.Stack <= 0)
                            chest.items.Remove(item);
                        chest.clearNulls();
                        return seedId;
                    }
                }
            }
            return -1;
        }

        public static int getSeedIdFromCropId(int cropId) {

            if (cropData == null) {
                cropData = new Dictionary<int, int>();
                Dictionary<int, string> dictionary = Game1.content.Load<Dictionary<int, string>>("Data\\Crops");
                foreach (KeyValuePair<int, string> current in dictionary) {
                    cropData.Add(Convert.ToInt32(current.Value.Split(new char[] { '/' })[3]), current.Key);
                }
            }

            if (cropData.ContainsKey(cropId)) {
                return cropData[cropId];
            }

            return -1;
        }

        public static int getMinutesForCrystalarium(int whichGem) {
            switch (whichGem) {
                case 60:
                    return 3000;
                case 62:
                    return 2240;
                case 64:
                    return 3000;
                case 66:
                    return 1360;
                case 68:
                    return 1120;
                case 70:
                    return 2400;
                case 72:
                    return 7200;
                case 80:
                    return 420;
                case 82:
                    return 1300;
                case 84:
                    return 1120;
                case 86:
                    return 800;
                default:
                    return 5000;
            }
        }
    }
}
