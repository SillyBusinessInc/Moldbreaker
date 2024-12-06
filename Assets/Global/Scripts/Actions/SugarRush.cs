
using UnityEngine;
[CreateAssetMenu(menuName = "Actions/SugarRush")]
public class SugarRush : ActionScriptableObject
{
    [SerializeField] private string actionName = "SugarRush";

    public float increaseSpeedMultiplier = 100f;
    public override void InvokeAction(string param)
    {
        GlobalReference
            .GetReference<PlayerReference>()
            .GetComponent<Player>().playerStatistic.Speed.AddMultiplier(actionName, increaseSpeedMultiplier, true);
    }

}
