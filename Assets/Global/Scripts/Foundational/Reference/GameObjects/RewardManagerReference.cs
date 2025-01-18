using System.Collections.Generic;

public class RewardManagerReference : Reference
{
    public List<UpgradeOption> upgradeOptions;

    public UpgradeOption GetRandomUpgrade()
    {
        Dictionary<UpgradeOption, int> optionsTable = new();
        for (int i = 0; i < upgradeOptions.Count; i++)
        {
            optionsTable.Add(upgradeOptions[i], GetWeight(upgradeOptions[i].rarity));
        }
        return RandomDistribution.GetRandom(optionsTable);
    }

    public List<UpgradeOption> GetRandomUpgrades(int count, bool allowDuplicates = false)
    {
        Dictionary<UpgradeOption, int> optionsTable = new();
        for (int i = 0; i < upgradeOptions.Count; i++)
        {
            optionsTable.Add(upgradeOptions[i], GetWeight(upgradeOptions[i].rarity));
        }
        return RandomDistribution.GetMultipleRandom(optionsTable, null, count, allowDuplicates);
    }


    private int GetWeight(int rarity)
    {
        return rarity switch
        {
            1 => 40,
            2 => 25,
            3 => 15,
            4 => 10,
            5 => 5,
            _ => 0,
        };
    }
}
