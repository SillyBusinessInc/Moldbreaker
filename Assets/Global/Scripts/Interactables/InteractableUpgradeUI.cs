
using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractableUpgradeUI : MonoBehaviour
{
    [Header("Upgrade Option")]
    [SerializeField] private UpgradeOption option;
    [SerializeField] private int roomId = -1; // Always on if -1
    
    [Header("Interaction")]
    [SerializeField] private List<ActionParamPair> interactionActions;

    void Start()
    {
        var saveData = new RoomSave();
        saveData.LoadAll();

        // var list = saveData.Get<List<int>>("finishedLevels");
    
        // gameObject.SetActive(
        //    !saveData.Get<List<int>>("finishedLevels").Contains(roomId)
        // );
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerObject>() == null) return;
        
        GlobalReference.GetReference<UpgradeOptions>().option = option;
            
        GlobalReference.GetReference<UpgradeOptions>().ShowOption();
        GlobalReference.GetReference<UpgradeOptions>().interactionActions = interactionActions;
        Destroy(gameObject);

    }
}
