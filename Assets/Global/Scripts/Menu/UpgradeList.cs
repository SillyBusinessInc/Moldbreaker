using UnityEngine;
using System.Collections.Generic;

public class UpgradeList : MonoBehaviour
{
    public UpgradePausePrefab upgradePrefab;
    public Transform contentParent;
    public float itemSpacing = 10f;
    public int itemsPerRow = 3;
    private readonly List<UpgradePausePrefab> upgrades = new();

    public void AddUpgrade(UpgradeOption option)
    {
        AddUpgrade(option.rarity, option.image);
    }
    
    private void AddUpgrade(int rarity, Sprite upgradeOption)
    {
        foreach (var upgrade in upgrades)
        {
            var data = upgrade.GetData();
            if (data.Item1 != rarity || data.Item2 != upgradeOption) 
                continue;

            upgrade.IncreaseAmount();
            return;
        }

        var newUpgrade = Instantiate(upgradePrefab, contentParent);
        newUpgrade.SetData(rarity, upgradeOption);

        upgrades.Add(newUpgrade);
        var rectTransform = newUpgrade.GetComponent<RectTransform>();

        var index = upgrades.Count - 1;
        var row = index / itemsPerRow;
        var col = index % itemsPerRow;

        var rect = rectTransform.rect;
        var xPos = col * (rect.width + itemSpacing);
        var yPos = -row * (rect.height + itemSpacing);

        rectTransform.anchoredPosition = new(xPos, yPos);
    }
}