using UnityEngine;

[CreateAssetMenu(menuName = "Actions/SFXAction")]
public class SFXAction : OneParamAction
{
    public override void InvokeAction(ActionMetaData _, string param)
    {
        GlobalReference.GetReference<AudioManager>().PlaySFX(param);
    }
}
