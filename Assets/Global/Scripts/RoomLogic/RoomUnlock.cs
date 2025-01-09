using System.Collections.Generic;
using UnityEngine;

public class RoomUnlock : MonoBehaviour
{
    private RoomSave saveData;
    private RoomTransitionDoor door;
    private string levelName;

    void Start()
    {
        door = GetComponent<RoomTransitionDoor>();
        saveData = new RoomSave();
        saveData.LoadAll();
        if (saveData.Get<List<int>>("finishedLevels").Contains(door.nextRoomId-1))
        {
            door.UnlockDoor();
        }
    }
}
