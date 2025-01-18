using UnityEngine;

[CreateAssetMenu(menuName = "Actions/DestroySelf")]
public class DestroySelf : OneParamAction
{
    public override void InvokeAction(ActionMetaData metaData, string _)
    {
       Destroy(metaData.Target);
    }
}
