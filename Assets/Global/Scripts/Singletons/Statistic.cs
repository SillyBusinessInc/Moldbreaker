public class Statistics : SecureSaveSystem
{
    protected override string Prefix => "statistics";

    public override void Init() {
        Add("deaths", 0);
        Add("death_by_enemy", 0);
        Add("death_by_hazard", 0);
        Add("total_crumbs_collected", 0);
        Add("total_calories_collected", 0);
        Add("times_finished_level", 0);
        Add("enemies_cleansed", 0);
    }
}