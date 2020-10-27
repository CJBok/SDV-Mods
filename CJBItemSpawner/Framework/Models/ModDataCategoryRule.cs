using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using CJBItemSpawner.Framework.ItemData;
using StardewValley;
using Object = StardewValley.Object;

namespace CJBItemSpawner.Framework.Models
{
    /// <summary>The rules for matching a category.</summary>
    internal class ModDataCategoryRule
    {
        /*********
        ** Accessors
        *********/
        /// <summary>The full name of the item's C# instance type.</summary>
        public ISet<string> Class { get; set; }

        /// <summary>The object's type (i.e. <see cref="Object.Type"/>).</summary>
        public ISet<string> ObjType { get; set; }

        /// <summary>The object's category (i.e. <see cref="Item.Category"/>).</summary>
        public ISet<int> ObjCategory { get; set; }

        /// <summary>The item's unique ID (i.e. <see cref="Item.ParentSheetIndex"/>).</summary>
        public ISet<string> ItemId { get; set; }


        /*********
        ** Public methods
        *********/
        /// <summary>Get whether a given item matches the rules for this category.</summary>
        /// <param name="entry">The searchable item to check.</param>
        public bool IsMatch(SearchableItem entry)
        {
            Item item = entry.Item;
            Object obj = item as Object;

            // match criteria
            if (this.Class.Any() && this.GetClassFullNames(item).Any(className => this.Class.Contains(className)))
                return true;
            if (this.ObjCategory.Any() && this.ObjCategory.Contains(item.Category))
                return true;
            if (this.ObjType.Any() && obj != null && this.ObjType.Contains(obj.Type))
                return true;
            if (this.ItemId.Any() && this.ItemId.Contains($"{entry.Type}:{item.ParentSheetIndex}"))
                return true;

            return false;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Normalize the data model after it's deserialized.</summary>
        /// <param name="context">The deserialization context.</param>
        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            this.Class = new HashSet<string>(this.Class ?? (IEnumerable<string>)new string[0], StringComparer.OrdinalIgnoreCase);
            this.ObjType = new HashSet<string>(this.ObjType ?? (IEnumerable<string>)new string[0], StringComparer.OrdinalIgnoreCase);
            this.ItemId = new HashSet<string>(this.ItemId ?? (IEnumerable<string>)new string[0], StringComparer.OrdinalIgnoreCase);
            this.ObjCategory ??= new HashSet<int>();
        }

        /// <summary>Get the full names for each class in an item's class hierarchy.</summary>
        /// <param name="item">The item instance.</param>
        private IEnumerable<string> GetClassFullNames(Item item)
        {
            for (Type type = item.GetType(); type != null; type = type.BaseType)
                yield return type.FullName;
        }
    }
}
