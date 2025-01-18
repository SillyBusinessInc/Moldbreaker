using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/EventAction")]
public class EventAction : OneParamAction
{
    [SerializeField] private List<Events> events;
    public override void InvokeAction(ActionMetaData _, string param)
    {
        events.ForEach(e => GlobalReference.AttemptInvoke(e));
    }
}

