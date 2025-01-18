using System.Collections.Generic;

public class AudioSettingSave : SaveSystem
{
    protected override string Prefix => "AudioSettings";

    public override void Init()
    {
        Add("Master", 8.0f);
        Add("SFX", 8.0f);
        Add("Music", 8.0f);
    }
}