using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DoorManager : Reference
{
    private List<Room> connectedRooms = new List<Room>(); 
    private List<GameObject> doors;
    private GameManagerReference gameManagerReference;
    private int previousId;
    [HideInInspector] public int currentId;

    public void Initialize()
    {
        previousId = 0;
        currentId = 0;
        gameManagerReference = GlobalReference.GetReference<GameManagerReference>();
        doors = GameObject.FindGameObjectsWithTag("DoorPrefab").ToList();
        SetupDoors();
    }

    void Update()
    {
        if (Time.timeScale == 0) return;
        if (currentId != previousId)
        {
            previousId = currentId;
            doors = GameObject.FindGameObjectsWithTag("DoorPrefab").ToList();
            SetupDoors();
        }
    }

    void SetupDoors()
    {
        connectedRooms = gameManagerReference.GetRooms(); // added for structure change
        doors.ForEach(x => x.GetComponent<RoomTransitionDoor>().Initialize());
    }
}
