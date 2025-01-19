using UnityEngine;

[CreateAssetMenu(menuName = "Actions/HealAction")]
public class HealAction : OneParamAction
{
    public override void InvokeAction(ActionMetaData _, string amount)
    {
        Player player = GlobalReference.GetReference<PlayerReference>().Player;
        float valueFloat = float.Parse(amount);
        player.Heal(valueFloat);
    }
}