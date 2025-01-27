public class Settings : SaveSystem
{
    protected override string Prefix => "settings";

    public override void Init() {
        Add("screen_mode", 0);
        Add("resolution", 7);
        Add("framerate_mode", 0);

        Add("master_volume", 0.5f);
        Add("effects_volume", 0.5f);
        Add("music_volume", 0.5f);
        
        Add("disable_mouse_lock", false);
    }
    
    
}