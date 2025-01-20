using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManagerReference : Reference
{
    public bool ignoreInput = true;

    void Start()
    {
        GlobalReference.SubscribeTo(Events.INPUT_ACKNOWLEDGE, () => ignoreInput = false);
        GlobalReference.SubscribeTo(Events.INPUT_IGNORE, () => ignoreInput = true);
        // calling Initialize if scene was loaded directly (without loading screen)
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (!(scene.name == "Loading")) continue;
            if (!scene.isLoaded) Initialize();
            return;
        }
        Initialize();
    }

    protected override void OnDestroy()
    {
        GlobalReference.UnsubscribeTo(Events.INPUT_ACKNOWLEDGE, AcknowledgeInput);
        GlobalReference.UnsubscribeTo(Events.INPUT_IGNORE, IgnoreInput);
        base.OnDestroy();
    }

    public void Initialize()
    {
        // table = new(); // disabled for structure change
        AddRoom(0, RoomType.ENTRANCE, true); // added for structure change
        AddRoom(1, RoomType.PARKOUR, true); // added for structure change
        AddRoom(2, RoomType.PARKOUR); // added for structure change
        AddRoom(3, RoomType.PARKOUR); // added for structure change

        activeRoom = GetRoom(0);
        GlobalReference.GetReference<DoorManager>().Initialize();
    }

    private void AcknowledgeInput()
    {
        ignoreInput = false;
    }
    private void IgnoreInput()
    {
        ignoreInput = true;
    }

#region rooms

    public Room activeRoom;
    private readonly List<Room> rooms = new();

    public void AddRoom(int id, RoomType roomType, bool unlocked = false)
    {
        if (rooms.Where((x) => x.id == id).Count() == 0) rooms.Add(new(id, roomType, unlocked));
    }

    public void RemoveRoom(int id)
    {
        Room room = rooms.Where((x) => x.id == id).FirstOrDefault();
        if (room != null) rooms.Remove(room);
    }

    public Room GetRoom(int id) => rooms.Where((x) => x.id == id).FirstOrDefault();
    public List<Room> GetRooms() => rooms;
    public void ResetRooms() => rooms.Clear();

#endregion
}

[System.Serializable]
public class RoomAmountCombo
{
    public RoomType type;
    public int amount;
}