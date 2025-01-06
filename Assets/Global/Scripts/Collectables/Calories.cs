using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Calories : Collectable
{
    private string caloriesId; // Unique identifier for this secret
    private CollectableSave saveData;
    public bool collected;

    void Awake()
    {
        saveData = new CollectableSave(gameObject.scene.name);

        // Check if the ID has already been assigned
        if (string.IsNullOrEmpty(caloriesId))
        {
            GeneratePersistentId();
        }

        List<string> calories = saveData.Get<List<string>>("calories");
        if (calories.Contains(caloriesId))
        {
            // gray shader or destroy or something TODO
            collected = true;
            // Destroy(gameObject);
        }
    }

    public override void OnCollect()
    {
        GlobalReference.GetReference<PlayerReference>().Player.playerStatistic.CollectedCalorie = true;
        if (collected)
        {
            return;
        }
        
        GlobalReference.GetReference<PlayerReference>().Player.playerStatistic.Calories.Add(caloriesId);
    }

    private void GeneratePersistentId()
    {
        if (!string.IsNullOrEmpty(caloriesId))
        {
            // ID is already assigned; no need to regenerate.
            return;
        }
        string key = gameObject.scene.name+"_"+transform.position.ToString();
        caloriesId = key;
    }
}
