using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Objects;
using StardewValley.Tools;
using SObject = StardewValley.Object;

namespace CJBItemSpawner.Framework.ItemData
{
    /// <summary>Provides methods for searching and constructing items.</summary>
    internal class ItemRepository
    {
        /*********
        ** Fields
        *********/
        /// <summary>The custom ID offset for items don't have a unique ID in the game.</summary>
        private readonly int CustomIDOffset = 1000;

        /// <summary>A filter which matches items to include.</summary>
        private readonly Func<SearchableItem, bool> Filter;


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="filter">A filter which matches items to include.</param>
        public ItemRepository(Func<SearchableItem, bool> filter)
        {
            this.Filter = filter;
        }

        /// <summary>Get all spawnable items matching the filter passed into the constructor.</summary>
        public IEnumerable<SearchableItem> GetFiltered()
        {
            return this
                .GetAll()
                .Where(this.Filter);
        }

        /// <summary>Get all spawnable items.</summary>
        [SuppressMessage("ReSharper", "AccessToModifiedClosure", Justification = "TryCreateItem invokes the lambda immediately.")]
        public IEnumerable<SearchableItem> GetAll()
        {
            IEnumerable<SearchableItem> GetAllRaw()
            {
                // get tools
                for (int quality = Tool.stone; quality <= Tool.iridium; quality++)
                {
                    yield return this.TryCreate(ItemType.Tool, ToolFactory.axe, () => ToolFactory.getToolFromDescription(ToolFactory.axe, quality));
                    yield return this.TryCreate(ItemType.Tool, ToolFactory.hoe, () => ToolFactory.getToolFromDescription(ToolFactory.hoe, quality));
                    yield return this.TryCreate(ItemType.Tool, ToolFactory.pickAxe, () => ToolFactory.getToolFromDescription(ToolFactory.pickAxe, quality));
                    yield return this.TryCreate(ItemType.Tool, ToolFactory.wateringCan, () => ToolFactory.getToolFromDescription(ToolFactory.wateringCan, quality));
                    if (quality != Tool.iridium)
                        yield return this.TryCreate(ItemType.Tool, ToolFactory.fishingRod, () => ToolFactory.getToolFromDescription(ToolFactory.fishingRod, quality));
                }
                yield return this.TryCreate(ItemType.Tool, this.CustomIDOffset, () => new MilkPail()); // these don't have any sort of ID, so we'll just assign some arbitrary ones
                yield return this.TryCreate(ItemType.Tool, this.CustomIDOffset + 1, () => new Shears());
                yield return this.TryCreate(ItemType.Tool, this.CustomIDOffset + 2, () => new Pan());
                yield return this.TryCreate(ItemType.Tool, this.CustomIDOffset + 3, () => new Wand());

                // wallpapers
                for (int id = 0; id < 112; id++)
                    yield return this.TryCreate(ItemType.Wallpaper, id, () => new Wallpaper(id) { Category = SObject.furnitureCategory });

                // flooring
                for (int id = 0; id < 40; id++)
                    yield return this.TryCreate(ItemType.Flooring, id, () => new Wallpaper(id, isFloor: true) { Category = SObject.furnitureCategory });

                // equipment
                foreach (int id in Game1.content.Load<Dictionary<int, string>>("Data\\Boots").Keys)
                    yield return this.TryCreate(ItemType.Boots, id, () => new Boots(id));
                foreach (int id in Game1.content.Load<Dictionary<int, string>>("Data\\hats").Keys)
                    yield return this.TryCreate(ItemType.Hat, id, () => new Hat(id));
                foreach (int id in Game1.objectInformation.Keys)
                {
                    if (id >= Ring.ringLowerIndexRange && id <= Ring.ringUpperIndexRange)
                        yield return this.TryCreate(ItemType.Ring, id, () => new Ring(id));
                }

                // weapons
                foreach (int id in Game1.content.Load<Dictionary<int, string>>("Data\\weapons").Keys)
                {
                    Item weapon = (id >= 32 && id <= 34)
                        ? (Item)new Slingshot(id)
                        : new MeleeWeapon(id);
                    yield return this.TryCreate(ItemType.Weapon, id, () => weapon);
                }

                // furniture
                foreach (int id in Game1.content.Load<Dictionary<int, string>>("Data\\Furniture").Keys)
                {
                    if (id == 1466 || id == 1468)
                        yield return this.TryCreate(ItemType.Furniture, id, () => new TV(id, Vector2.Zero));
                    else
                        yield return this.TryCreate(ItemType.Furniture, id, () => new Furniture(id, Vector2.Zero));
                }

                // craftables
                foreach (int id in Game1.bigCraftablesInformation.Keys)
                    yield return this.TryCreate(ItemType.BigCraftable, id, () => new SObject(Vector2.Zero, id));

                // secret notes
                foreach (int id in Game1.content.Load<Dictionary<int, string>>("Data\\SecretNotes").Keys)
                {
                    SObject note = new SObject(79, 1);
                    note.name = $"{note.name} #{id}";
                    yield return this.TryCreate(ItemType.Object, this.CustomIDOffset + id, () => note);
                }

                // objects
                foreach (int id in Game1.objectInformation.Keys)
                {
                    if (id == 79)
                        continue; // secret note handled above
                    if (id >= Ring.ringLowerIndexRange && id <= Ring.ringUpperIndexRange)
                        continue; // handled separated

                    SObject item = new SObject(id, 1);
                    yield return this.TryCreate(ItemType.Object, id, () => item);

                    // fruit products
                    if (item.Category == SObject.FruitsCategory)
                    {
                        // wine
                        SObject wine = new SObject(348, 1)
                        {
                            Name = $"{item.Name} Wine",
                            Price = item.Price * 3
                        };
                        wine.preserve.Value = SObject.PreserveType.Wine;
                        wine.preservedParentSheetIndex.Value = item.ParentSheetIndex;
                        yield return this.TryCreate(ItemType.Object, this.CustomIDOffset * 2 + id, () => wine);

                        // jelly
                        SObject jelly = new SObject(344, 1)
                        {
                            Name = $"{item.Name} Jelly",
                            Price = 50 + item.Price * 2
                        };
                        jelly.preserve.Value = SObject.PreserveType.Jelly;
                        jelly.preservedParentSheetIndex.Value = item.ParentSheetIndex;
                        yield return this.TryCreate(ItemType.Object, this.CustomIDOffset * 3 + id, () => jelly);
                    }

                    // vegetable products
                    else if (item.Category == SObject.VegetableCategory)
                    {
                        // juice
                        SObject juice = new SObject(350, 1)
                        {
                            Name = $"{item.Name} Juice",
                            Price = (int)(item.Price * 2.25d)
                        };
                        juice.preserve.Value = SObject.PreserveType.Juice;
                        juice.preservedParentSheetIndex.Value = item.ParentSheetIndex;
                        yield return this.TryCreate(ItemType.Object, this.CustomIDOffset * 4 + id, () => juice);

                        // pickled
                        SObject pickled = new SObject(342, 1)
                        {
                            Name = $"Pickled {item.Name}",
                            Price = 50 + item.Price * 2
                        };
                        pickled.preserve.Value = SObject.PreserveType.Pickle;
                        pickled.preservedParentSheetIndex.Value = item.ParentSheetIndex;
                        yield return this.TryCreate(ItemType.Object, this.CustomIDOffset * 5 + id, () => pickled);
                    }

                    // flower honey
                    else if (item.Category == SObject.flowersCategory)
                    {
                        // get honey type
                        SObject.HoneyType? type = null;
                        switch (item.ParentSheetIndex)
                        {
                            case 376:
                                type = SObject.HoneyType.Poppy;
                                break;
                            case 591:
                                type = SObject.HoneyType.Tulip;
                                break;
                            case 593:
                                type = SObject.HoneyType.SummerSpangle;
                                break;
                            case 595:
                                type = SObject.HoneyType.FairyRose;
                                break;
                            case 597:
                                type = SObject.HoneyType.BlueJazz;
                                break;
                            case 421: // sunflower standing in for all other flowers
                                type = SObject.HoneyType.Wild;
                                break;
                        }

                        // yield honey
                        if (type != null)
                        {
                            SObject honey = new SObject(Vector2.Zero, 340, item.Name + " Honey", false, true, false, false)
                            {
                                Name = "Wild Honey"
                            };
                            honey.honeyType.Value = type;

                            if (type != SObject.HoneyType.Wild)
                            {
                                honey.Name = $"{item.Name} Honey";
                                honey.Price += item.Price * 2;
                            }
                            yield return this.TryCreate(ItemType.Object, this.CustomIDOffset * 5 + id, () => honey);
                        }
                    }
                }
            }

            return GetAllRaw().Where(p => p != null);
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Create a searchable item if valid.</summary>
        /// <param name="type">The item type.</param>
        /// <param name="id">The unique ID (if different from the item's parent sheet index).</param>
        /// <param name="createItem">Create an item instance.</param>
        private SearchableItem TryCreate(ItemType type, int id, Func<Item> createItem)
        {
            try
            {
                return new SearchableItem(type, id, createItem());
            }
            catch
            {
                return null; // if some item data is invalid, just don't show it in the item spawner
            }
        }
    }
}
