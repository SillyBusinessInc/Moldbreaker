using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Configs/ShopItem")]
[System.Serializable]
public class ShopItem : ScriptableObject
{
    public string itemName;
    public int itemCost;
    public List<ActionParamPair> Actions = new();
}

[System.Serializable]
public class ShopTier
{
    public string tierName;
    public List<ShopItem> items = new();
}

[CreateAssetMenu(menuName = "Configs/ShopLootTable")]
public class ShopLootTable : ScriptableObject
{
    public List<WeightableEntry<ShopTier>> Tiers = new();

    [ContextMenu("Pick Shop Item")]
    public void PickShopItem()
    {
        WeightableEntry<ShopTier> reward = WeightedSelectionUtility.GetRandomEntry(Tiers);
        if (reward != null)
        {
            Debug.Log($"Selected reward: {reward}");
        }
    }

    public void PickMultipleShopItems(int count)
    {
        List<WeightableEntry<ShopTier>> selectedRewards = WeightedSelectionUtility.GetMultipleRandomEntries(Tiers, count, true);
    }
}