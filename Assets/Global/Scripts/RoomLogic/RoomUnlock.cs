using System.Collections.Generic;
using UnityEngine;

public class RoomUnlock : MonoBehaviour
{
    private RoomSave saveData;
    private RoomTransitionDoor door;

    void Start()
    {
        door = GetComponent<RoomTransitionDoor>();
        LockAndUnLock();
        GlobalReference.SubscribeTo(Events.LEVELS_CHANGED_BY_CHEAT, LockAndUnLock);
    }

    private void LockAndUnLock()
    {
        saveData = new RoomSave();
        saveData.LoadAll();
        if (door.nextRoomId == 1)
        {
            door.IsDisabled = false;
            return;
        }
        
        door.IsDisabled = !saveData.Get<List<int>>("finishedLevels").Contains(door.nextRoomId - 1);
    }
}
