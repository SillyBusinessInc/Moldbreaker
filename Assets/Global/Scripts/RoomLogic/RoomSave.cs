using System.Collections.Generic;

public class RoomSave : SecureSaveSystem
{
    protected override string Prefix => "Levels";

    public override void Init() { 
        Add("finishedLevels", new List<int>()); 
    }
}
