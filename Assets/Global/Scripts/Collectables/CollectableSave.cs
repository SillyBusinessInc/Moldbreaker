using System.Collections.Generic;

public class CollectableSave : SecureSaveSystem
{
    public string name;
    public CollectableSave(string name_) : base() {name = name_;}

    protected override string Prefix => name;

    public override void Init() {
        Add("crumbs", 0);
        Add("calories", new List<string>());
    }
}
