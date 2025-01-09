using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Calories : Collectable
{
    private string caloriesId; // Unique identifier for this secret
    private CollectableSave saveData;
    public bool collected;

    //make a variable to give a number from 1 to 3
    [Range(1, 3)]
    [SerializeField]private int progressionOrder = 1;

    void Awake()
    {
        saveData = new CollectableSave(gameObject.scene.name);

        // Check if the ID has already been assigned
        if (string.IsNullOrEmpty(caloriesId))
        {
            GeneratePersistentId();
        }

        List<string> calories = saveData.Get<List<string>>("calories");
        if (GlobalReference.GetReference<PlayerReference>().Player.playerStatistic.CaloriesCollected.Contains(caloriesId))
        {
            // gray shader or destroy or something TODO
            collected = true;
            
            //get the material of the object
            Material material = GetComponent<MeshRenderer>().material;
            //lower opacity
            material.color = new Color(material.color.r, material.color.g, material.color.b, 0.5f);
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
        string key = gameObject.scene.name + ">>>UNIQUE_DELIMITER>>>" + transform.position.ToString() + ">>>UNIQUE_DELIMITER>>>" + progressionOrder;
        caloriesId = key;
    }
}
