#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections.Generic;
using System.IO;

[CreateAssetMenu(menuName = "Configs/LootTable")]
public class LootTable : ScriptableObject
{
    public List<WeightableEntry<List<ActionParamPair>>> Rewards = new();

    public WeightableEntry<List<ActionParamPair>> GetRandomReward()
    {
        // No need to specify the type because it's inferred from the method's return type
        return WeightedSelectionUtility.GetRandomEntry(Rewards);
    }

    public List<WeightableEntry<List<ActionParamPair>>> GetMultipleRandomRewards(int count)
    {
        return WeightedSelectionUtility.GetMultipleRandomEntries(Rewards, count, true);
    }

    [ContextMenu("Export to JSON")]
    public void ExportToJson()
    {
        string json = JsonUtility.ToJson(this, true);
        string path = Path.Combine(Application.dataPath, $"{name}_LootTable.json");
        File.WriteAllText(path, json);
        Debug.Log($"LootTable exported to JSON at: {path}");
    }

    [ContextMenu("Import from JSON")]
    public void ImportFromJson()
    {
#if UNITY_EDITOR
        // Open a file picker dialog to select the JSON file
        string path = EditorUtility.OpenFilePanel("Select LootTable JSON", Application.dataPath, "json");
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogWarning("No file selected.");
            return;
        }

        // Read and apply JSON data
        string json = File.ReadAllText(path);
        JsonUtility.FromJsonOverwrite(json, this);
        Debug.Log($"LootTable imported from JSON at: {path}");
#else
        Debug.LogError("ImportFromJson is only available in the Unity Editor.");
#endif
    }
}