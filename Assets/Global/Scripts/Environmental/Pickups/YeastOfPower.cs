using System.Collections.Generic;
using UnityEngine;

public class YeastOfPower : PickupBase
{
    [Header("Upgrade Option")]
    [SerializeField] private UpgradeOption option;
    
    [Header("Interaction")]
    [SerializeField] private List<ActionParamPair> interactionActions;

    protected override void Start()
    {
        base.Start();
        CheckIfAlreadyCollected();   
    }
    
    private void CheckIfAlreadyCollected() {
        int roomId;
        try {
            roomId = GlobalReference.GetReference<GameManagerReference>().activeRoom.id;
        } catch {
            roomId = PlayerPrefs.GetInt("level");
            PlayerPrefs.DeleteKey("level");
        }

        var saveData = new RoomSave();
        saveData.LoadAll();

        var list = saveData.Get<List<int>>("finishedLevels");
        if (list.Contains(roomId)) // if player already finished the room, don't show yeast of power
            gameObject.SetActive(false);
    }

    protected override void OnTrigger()
    {
        var upgradeOptions = GlobalReference.GetReference<UpgradeOptions>();
        upgradeOptions.option = option;
            
        upgradeOptions.ShowOption();
        upgradeOptions.interactionActions = interactionActions;
        Destroy(gameObject);
    }
}