using UnityEngine;
using System.Linq;

public class DoorManager : Reference
{
    private int previousId;
    [HideInInspector] public int currentId;

    public void Initialize()
    {
        previousId = PreviousLevel.Instance.prevLevelId;
        currentId = PreviousLevel.Instance.prevLevelId;
        SetupDoors();
    }

    void Update()
    {
        if (Time.timeScale == 0) return;
        if (currentId == previousId) return;

        previousId = currentId;
        SetupDoors();
    }

    void SetupDoors()
    {
        var doors = GameObject.FindGameObjectsWithTag("DoorPrefab").ToList();
        doors.ForEach(x => x.GetComponent<GateRoomTransition>().Initialize());
    }
}
