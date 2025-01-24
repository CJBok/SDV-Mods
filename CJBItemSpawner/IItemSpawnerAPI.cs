using System;
using StardewValley;

namespace CJBItemSpawner
{
    public interface IItemSpawnerAPI
    {
        public interface IVariantsRequestedEventArgs
        {
            /// <summary>The item to provide variants for</summary>
            public string BaseId { get; }

            /// <summary>Add an item variant if valid</summary>
            /// <param name="variantId">A unique variant identifier. Should include the base item id.</param>
            /// <param name="createItem">Creates an instance of the item</param>
            public void TryAddVariant(string variantId, Func<object, Item> createItem);
        }

        /// <summary>
        /// Prevent an item from being displayed in the item spawner.
        /// Should only be used for placeholder items. <br/>
        /// Does not disable variants for this item.
        /// </summary>
        /// <param name="qualifiedId">The qualified item id</param>
        public void BlacklistItem(string qualifiedId);

        /// <summary>
        /// Can be used to add custom variants to an existing item.
        /// </summary>
        public event EventHandler<IVariantsRequestedEventArgs>? VariantsRequested;
    }
}
