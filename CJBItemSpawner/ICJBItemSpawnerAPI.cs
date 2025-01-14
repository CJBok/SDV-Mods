namespace CJBItemSpawner;

/// <summary>The API which lets other mods interact with CJB Item Spawner.</summary>
internal interface ICJBItemSpawnerAPI
{
    /// <summary>
    /// Open the item spawner menu.
    /// </summary>
    void OpenItemSpawnerMenu();

    /// <summary>Add an item repository.</summary>
    /// <param name="repository">An item repository which returns all spawnable items.</param>
    void AddRepository(IItemRepository repository);
}
