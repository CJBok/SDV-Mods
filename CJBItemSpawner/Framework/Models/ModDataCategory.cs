using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using StardewValley;
using SObject = StardewValley.Object;

namespace CJBItemSpawner.Framework.Models
{
    /// <summary>An item category filter rule. See the format documentation in the <c>data.json</c> file.</summary>
    internal class ModDataCategory
    {
        /*********
        ** Accessors
        *********/
        /// <summary>The translation key or literal text for the category display name.</summary>
        public string Label { get; set; }

        /// <summary>The full name of the item's C# instance type.</summary>
        public ISet<string> Class { get; set; }

        /// <summary>The object's type (i.e. <see cref="SObject.Type"/>).</summary>
        public ISet<string> ObjType { get; set; }

        /// <summary>The object's category (i.e. <see cref="SObject.Category"/>).</summary>
        public ISet<int> ObjCategory { get; set; }

        /// <summary>The item's unique ID (i.e. <see cref="Item.ParentSheetIndex"/>).</summary>
        public ISet<int> ItemId { get; set; }


        /*********
        ** Public methods
        *********/
        /// <summary>Get whether a given item matches the rules for this category.</summary>
        /// <param name="item">The item to check.</param>
        public bool IsMatch(Item item)
        {
            SObject obj = item as SObject;

            // no criteria
            if (!this.Class.Any() && !this.ObjCategory.Any() && !this.ObjType.Any() && !this.ItemId.Any())
                return false;

            // match criteria
            if (this.Class.Any() && !this.GetClassFullNames(item).Any(className => this.Class.Contains(className)))
                return false;
            if (this.ObjCategory.Any() && !this.ObjCategory.Contains(item.Category))
                return false;
            if (this.ObjType.Any() && (obj == null || !this.ObjType.Contains(obj.Type)))
                return false;
            if (this.ItemId.Any() && !this.ItemId.Contains(item.ParentSheetIndex))
                return false;
            return true;
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
            this.ObjCategory ??= new HashSet<int>();
            this.ItemId ??= new HashSet<int>();
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
