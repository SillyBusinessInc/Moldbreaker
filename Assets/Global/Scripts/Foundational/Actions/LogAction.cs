using UnityEngine;

[CreateAssetMenu(menuName = "Actions/LogAction")]
public class LogAction : OneParamAction
{
    [SerializeField] private string actionName = "Log Action";
    public override void InvokeAction(ActionMetaData _, string param)
    {
        Debug.Log($"{actionName} - {param}");
    }
}