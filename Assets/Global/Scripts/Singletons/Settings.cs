public class Settings : SaveSystem
{
    protected override string Prefix => "settings";

    public override void Init() {
        Add("screen_mode", 0);
        
        Add("master_volume", 0.5f);
        Add("effects_volume", 0.5f);
        Add("music_volume", 0.5f);
        
        Add("brightness", 0.5f);
        Add("speedrun_mode", false);
    }
}