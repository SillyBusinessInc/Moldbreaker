using UnityEngine; 
using UnityEngine.UI; 
using TMPro;

public class UpgradePausePrefab : MonoBehaviour{
    public Image upgradeImage; 
    public TMP_Text amountText; 
    private int rarity;
    private Sprite upgradeOption;
    private int amount;

    public void SetData(int rarity, Sprite upgradeOption) { 
        this.rarity = rarity;
        this.upgradeOption = upgradeOption;
        upgradeImage.sprite = upgradeOption; 
        amount = 1;
    }

    public (int, Sprite) GetData() { 
        return (rarity, upgradeOption);
    }

    public void IncreaseAmount() { 
        amount += 1; 
        amountText.text = amount.ToString(); 
    }
}