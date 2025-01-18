using UnityEngine;

public class Calories : Collectable
{
    public string caloriesId; // Unique identifier for this secret
    public bool collected;

    //make a variable to give a number from 1 to 3
    [Range(1, 3)]
    [SerializeField]private int progressionOrder = 1;
    public GameObject fire;

    void Awake()
    {

        // Check if the ID has already been assigned
        if (string.IsNullOrEmpty(caloriesId))
        {
            caloriesId = gameObject.name + ">>>UNIQUE_DELIMITER>>>" + progressionOrder;
        }else{
            caloriesId += ">>>UNIQUE_DELIMITER>>>" + progressionOrder;
        }

        if (GlobalReference.GetReference<PlayerReference>().Player.playerStatistic.CaloriesCollected.Contains(caloriesId))
        {
            // gray shader or destroy or something TODO
            collected = true;
            
            // //get the material of the object
            Material material = fire.GetComponent<MeshRenderer>().material;
            material.SetFloat("_Alpha", 0.2f);
            // material.color = new Color(material.color.r, material.color.g, material.color.b, 0.2f);
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
        AudioManager.Instance.PlaySFX("CaloriePickup");
    }
}
