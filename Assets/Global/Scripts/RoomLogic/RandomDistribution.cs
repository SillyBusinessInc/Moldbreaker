using System;
using System.Collections.Generic;
using System.Linq;

using SRandom = System.Random;

public static class RandomDistribution
{
    private static readonly SRandom random_ = new(Guid.NewGuid().GetHashCode());

    public static T GetRandom<T>(Dictionary<T, int> chances, SRandom rand = null)
    {
        Dictionary<T, int> chancesCopy = new();
        SRandom random = rand;
        random ??= random_;
        int totalChance = chances.Sum((x) => x.Value);
        int picked = random.Next(0, totalChance);
        
        foreach (KeyValuePair<T, int> pair in chances)
        {
            if (picked < pair.Value) return pair.Key;
            {
                picked -= pair.Value;
            }
        }
        return default;
    }

    public static List<T> GetMultipleRandom<T>(Dictionary<T, int> chances, SRandom rand = null, int count = 1, bool allowDuplicates = false)
    {
        List<T> result = new();
        Dictionary<T, int> availableRewards = new(chances);

        for (int i = 0; i < count; i++)
        {
            T selected = GetRandom(availableRewards);
            if (selected != null)
            {
                result.Add(selected);

                if (!allowDuplicates) availableRewards.Remove(selected); // Remove to avoid duplicates
            }
        }
        return result;
    }
}
