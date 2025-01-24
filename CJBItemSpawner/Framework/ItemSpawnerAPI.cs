using System;
using System.Collections.Generic;
using CJBItemSpawner.Framework.ItemData;
using StardewValley;

namespace CJBItemSpawner.Framework
{
    public class ItemSpawnerAPI : IItemSpawnerAPI
    {
        internal delegate SearchableItem? SearchableItemFactory(string type, string key, Func<SearchableItem, Item> createItem);

        private readonly HashSet<string> Blacklist = [];

        public class VariantsRequestedEventArgs : IItemSpawnerAPI.IVariantsRequestedEventArgs
        {
            private readonly SearchableItemFactory TryCreate;
            private readonly string type;
            internal readonly List<SearchableItem> Items = [];

            /// <inheritdoc/>
            public string BaseId { get; init; }

            /// <inheritdoc/>
            public void TryAddVariant(string variantId, Func<object, Item> createItem)
            {
                if (this.TryCreate(this.type,variantId, createItem) is SearchableItem result)
                    this.Items.Add(result);
            }

            internal VariantsRequestedEventArgs(string type, string baseId, SearchableItemFactory tryCreate)
            {
                this.BaseId = baseId;
                this.TryCreate = tryCreate;
                this.type = type;
            }
        }

        /// <inheritdoc/>
        public event EventHandler<IItemSpawnerAPI.IVariantsRequestedEventArgs>? VariantsRequested;

        /// <inheritdoc/>
        public void BlacklistItem(string qualifiedId)
        {
            this.Blacklist.Add(qualifiedId);
        }

        /// <summary>Gets API-Added variants for a given item.</summary>
        /// <param name="type">The item type</param>
        /// <param name="baseId">The item's unqualified id</param>
        internal IEnumerable<SearchableItem> GetVariantsFor(string type, string baseId, SearchableItemFactory TryCreate)
        {
            // skip setup and teardown if nobody is using the api.
            if (VariantsRequested is null)
                return [];

            VariantsRequestedEventArgs args = new(type, baseId, TryCreate);
            VariantsRequested(this, args);
            return args.Items;
        }

        /// <summary>Gets whether or not an item has been blacklisted.</summary>
        /// <param name="id">The qualified id for the item</param>
        internal bool IsBlacklisted(string id)
        {
            return this.Blacklist.Contains(id);
        }
    }
}
