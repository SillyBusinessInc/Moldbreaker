using Discord;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/LegDay")]
public class LegDay : OneParamAction
{
    [SerializeField] private string actionName = "LegDay";

    public int doubleJumpsCountincrease = 1;
    public override void InvokeAction(ActionMetaData _, string param)
    {
        GlobalReference
            .GetReference<PlayerReference>()
            .GetComponent<Player>().playerStatistic.DoubleJumpsCount
            .AddModifier(actionName, doubleJumpsCountincrease);
        
        AchievementManager.Grant("RISE_EVEN_HIGHER");
    }
}
