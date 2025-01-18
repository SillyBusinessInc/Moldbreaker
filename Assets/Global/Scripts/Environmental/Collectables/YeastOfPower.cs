
using System.Collections.Generic;
using UnityEngine;

public class YeastOfPower : MonoBehaviour
{
    [Header("Upgrade Option")]
    [SerializeField] private UpgradeOption option;
    // [SerializeField] private int roomId = -1; // Always on if -1
    
    [Header("Interaction")]
    [SerializeField] private List<ActionParamPair> interactionActions;

    void Start()
    {
        CheckIfAlreadyCollected();   
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerObject>() == null) 
            return;
        
        GlobalReference.GetReference<UpgradeOptions>().option = option;
            
        GlobalReference.GetReference<UpgradeOptions>().ShowOption();
        GlobalReference.GetReference<UpgradeOptions>().interactionActions = interactionActions;
        Destroy(gameObject);
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
        if (list.Contains(roomId)) { // if player already finished the room, don't show yeast of power
            gameObject.SetActive(false);
        }
    }
}
