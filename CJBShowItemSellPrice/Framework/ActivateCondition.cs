namespace CJBShowItemSellPrice.Framework;

/// <summary>The conditions when the sell price UI should be shown.</summary>
internal enum ActivateCondition
{
    /// <summary>Always show it.</summary>
    Always,

    /// <summary>Only show it if the price catalogue hasn't been unlocked.</summary>
    BeforePriceCatalogue,

    /// <summary>Only show it after the price catalogue has been unlocked.</summary>
    AfterPriceCatalogue
}
