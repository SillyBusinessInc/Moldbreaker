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

    void LoadConnectedRooms()
    {
        connectedRooms = GetConnectedRooms(); 
    }

    void DeactivateExtraDoors()
    {
        int roomCount = connectedRooms.Count;

        if (roomCount < doors.Count)
        {
            int doorsToDeactivate = doors.Count - roomCount;
            for (int i = 0; i < doorsToDeactivate; i++)
            {
                int randomIndex;
                do{
                    randomIndex = Random.Range(0, doors.Count);
                } while (!doors[randomIndex].activeSelf);
                doors[randomIndex].SetActive(false);
            }
        }
    }

    void ConnectDoorsToRooms()
    {
        List<Room> remainingRooms = new List<Room>(connectedRooms);
        foreach (var doorObject in doors)
        {
            if (!doorObject.activeSelf) continue;

            RoomTransitionDoor door = doorObject.GetComponent<RoomTransitionDoor>();
            if (door == null) continue;

            int randomIndex = Random.Range(0, remainingRooms.Count);
            Room selectedRoom = remainingRooms[randomIndex];

            door.nextRoomType = selectedRoom.roomType;
            door.nextRoomId = selectedRoom.id;
            
            // set Bonus Room active false
            if(door.nextRoomType.ToString() == "BONUS") {
                doorObject.SetActive(false);
            }

            remainingRooms.RemoveAt(randomIndex);
        }
    }

    public List<Room> GetConnectedRooms()
    {
        return gameManagerReference.GetNextRooms();
    }

    void SetupDoors()
    {
        // LoadConnectedRooms(); // disabled for structure change
        // DeactivateExtraDoors(); // disabled for structure change
        // ConnectDoorsToRooms(); // disabled for structure change
        connectedRooms = gameManagerReference.GetRooms(); // added for structure change
        doors.ForEach(x => x.GetComponent<RoomTransitionDoor>().Initialize());
    }
}
