using System.Collections.Generic;
using UnityEngine;

public class PanChanPoster : Interactable
{
    [SerializeField] private int posterId;
    
    public override void Start()
    {
        base.Start();
        GlobalReference.SubscribeTo(Events.LEVELS_CHANGED_BY_CHEAT, TogglePoster);
        TogglePoster();
    }

    void OnDestroy()
    {
        GlobalReference.UnsubscribeTo(Events.LEVELS_CHANGED_BY_CHEAT, TogglePoster);   
    }
    
    private void TogglePoster()
    {
        var saveRoomData = new RoomSave();
        saveRoomData.LoadAll();
        var postersFound = saveRoomData.Get<List<int>>("posters");
        var foundThisPoster = postersFound.Contains(posterId);
        // Todo: actually do something
    }
}
