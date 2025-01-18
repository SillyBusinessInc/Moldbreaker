using UnityEngine;

public class Calories : PickupBase
{
    public string caloriesId; // Unique identifier for this secret
    public bool collected;
    
    [Range(1, 3), SerializeField] private int progressionOrder = 1;
    public GameObject fire;

    void Awake()
    {

        // Check if the ID has already been assigned
        if (string.IsNullOrEmpty(caloriesId))
            caloriesId = gameObject.name + ">>>UNIQUE_DELIMITER>>>" + progressionOrder;
        else
            caloriesId += ">>>UNIQUE_DELIMITER>>>" + progressionOrder;

        var playerStats = GlobalReference.GetReference<PlayerReference>().Player.playerStatistic;
        if (!playerStats.CaloriesCollected.Contains(this.caloriesId)) 
            return;

        this.collected = true;
        var material = this.fire.GetComponent<MeshRenderer>().material;
        material.SetFloat("_Alpha", 0.2f);
    }

    protected override void OnTrigger()
    {
        GlobalReference.GetReference<PlayerReference>().Player.playerStatistic.CollectedCalorie = true;
        if (collected)
            return;
        
        GlobalReference.GetReference<PlayerReference>().Player.playerStatistic.Calories.Add(caloriesId);
        AudioManager.Instance.PlaySFX("CaloriePickup");
    }
}
