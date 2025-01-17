using UnityEngine; 
using UnityEngine.UI; 
using TMPro;

public class UpgradePausePrefab : MonoBehaviour{
    public Image rarityImage; 
    public Image upgradeImage; 
    public TMP_Text amountText; 
    private int rarity;
    private Sprite upgradeOption;
    private int amount;

    public Sprite rarity_common;
    public Sprite rarity_uncommon;
    public Sprite rarity_rare;
    public Sprite rarity_epic;
    public Sprite rarity_legendary;


    public void SetData(int rarity, Sprite upgradeOption) { 
        this.rarity = rarity;
        this.upgradeOption = upgradeOption;
        rarityImage.sprite = GetRarityImage(rarity); 
        upgradeImage.sprite = upgradeOption; 
        amount = 1;
    }

    Sprite GetRarityImage(int rarity) {
        return rarity switch {
            1 => rarity_common,
            2 => rarity_uncommon,
            3 => rarity_rare,
            4 => rarity_epic,
            5 => rarity_legendary,
            _ => rarity_common,
        };
    }
    public (int, Sprite) GetData() { 
        return (rarity, upgradeOption);
    }

    public void IncreaseAmount() { 
        amount += 1; 
        amountText.text = amount.ToString(); 
    }
}