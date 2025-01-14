using System.Collections.Generic;
using UnityEngine;

public class EnableOnEvent : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectsToEnable;
    [SerializeField] private Events enableEvent;
    
    void Start()
    {
        foreach (var obj in objectsToEnable)
        {
            obj.SetActive(false);
        }
        
        GlobalReference.SubscribeTo(this.enableEvent, EnableAll);
        HandleYop();
    }

    private void EnableAll()
    {
        foreach (var obj in objectsToEnable)
        {
            obj.SetActive(true);
        }
    }

    void HandleYop() {
        var roomId = GlobalReference.GetReference<GameManagerReference>().activeRoom.id;
        var saveData = new RoomSave();
        saveData.LoadAll();

        var list = saveData.Get<List<int>>("finishedLevels");
        if (list.Contains(roomId + 1)) { // if player already got the upgrade, don't show it
            gameObject.SetActive(false);
        }
    }
}
