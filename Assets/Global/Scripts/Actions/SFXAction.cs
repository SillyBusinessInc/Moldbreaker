using UnityEngine;

[CreateAssetMenu(menuName = "Actions/SFXAction")]
public class SFXAction : OneParamAction
{
    public override void InvokeAction(ActionMetaData _, string param)
    {
        AudioManager.Instance.PlaySFX(param);
    }
}
