using System.Collections.Generic;
using System.Linq;
using UnityEngine; 

// For instantiating the item
[System.Serializable]
public class WeightableEntry<T>
{ 
    public string Name;
    public float Weight;
    public T Entry;

    public WeightableEntry(string name, float weight, T entry)
    {
        Name = name;
        Weight = weight;
        Entry = entry;
    }
}

public static class WeightedSelectionUtility
{
    // Get a single random entry based on weight
    public static WeightableEntry<T> GetRandomEntry<T>(List<WeightableEntry<T>> entries)
    {
        if (entries == null || entries.Count == 0)
        {
            Debug.LogWarning("[WARN] No entries available. [WeightedSelectionUtility.GetRandomEntry]");
            return null;
        }

        float totalWeight = entries.Sum(entry => entry.Weight);

        if (totalWeight <= 0)
        {
            Debug.LogWarning("[WARN] Total weight is zero or less. [WeightedSelectionUtility.GetRandomEntry]");
            return null;
        }

        float randomValue = Random.Range(0, totalWeight);
        float cumulativeWeight = 0;

        foreach (var entry in entries)
        {
            cumulativeWeight += entry.Weight;
            if (randomValue <= cumulativeWeight)
            {
                return entry;
            }
        }

        Debug.LogWarning("[WARN] No valid entry selected. This should not happen. [WeightedSelectionUtility.GetRandomEntry]");
        return null;
    }

    // Get multiple random entries, may allow duplicates
    public static List<WeightableEntry<T>> GetMultipleRandomEntries<T>(List<WeightableEntry<T>> entries, int count, bool allowDuplicates = false)
    {
        if (count <= 0)
        {
            Debug.LogWarning("[WARN] Count must be greater than zero. [WeightedSelectionUtility.GetMultipleRandomEntries]");
            return new List<WeightableEntry<T>>();
        }

        if (!allowDuplicates && entries.Count < count)
        {
            Debug.LogWarning("[WARN] Not enough entries available for the requested count. [WeightedSelectionUtility.GetMultipleRandomEntries]");
            return new List<WeightableEntry<T>>();
        }

        List<WeightableEntry<T>> selectedEntries = new List<WeightableEntry<T>>();

        if (allowDuplicates)
        {
            // If duplicates are allowed, just pick entries randomly and don't remove from the list
            for (int i = 0; i < count; i++)
            {
                var selected = GetRandomEntry(entries);
                if (selected != null)
                {
                    selectedEntries.Add(selected);
                }
            }
        }
        else
        {
            // Remove entries after they are selected to avoid duplicates
            List<WeightableEntry<T>> availableEntries = new(entries);

            for (int i = 0; i < count; i++)
            {
                if (availableEntries.Count == 0)
                {
                    Debug.LogWarning("[WARN] Not enough unique entries left. [WeightedSelectionUtility.GetMultipleRandomEntries]");
                    break;
                }

                var selected = GetRandomEntry(availableEntries);
                if (selected != null)
                {
                    selectedEntries.Add(selected);
                    availableEntries.Remove(selected); // Remove to avoid duplicates
                }
            }
        }

        return selectedEntries;
    }
}
