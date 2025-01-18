using System.Collections.Generic;
using UnityEngine;

public abstract class ActionScriptableObjectBase : ScriptableObject
{
    // The method to be implemented by each action
    public abstract void InvokeAction(ActionMetaData metaData, List<string> parameters);
}

public readonly struct ActionMetaData
{
    public GameObject Source { get; }
    public GameObject Target { get; }

    public ActionMetaData(GameObject source = null, GameObject target = null)
    {
        Source = source;
        Target = target;
    }

    public static readonly ActionMetaData Empty = new();
}

public abstract class OneParamAction : ActionScriptableObjectBase
{
    // The method to be implemented by each action
    public abstract void InvokeAction(ActionMetaData metaData, string parameter);

    public override void InvokeAction(ActionMetaData metaData, List<string> parameters)
    {
        // validate the number of parameters
        if (parameters.Count < 1)
        {
            Debug.LogWarning("Not enough parameters for action.");
            return;
        }

        InvokeAction(metaData, parameters[0]);
    }
}

public abstract class TwoParamAction : ActionScriptableObjectBase
{
    public abstract void InvokeAction(ActionMetaData metaData, string parameter1, string parameter2);

    public override void InvokeAction(ActionMetaData metaData, List<string> parameters)
    {
        // validate the number of parameters
        if (parameters.Count < 2)
        {
            Debug.LogWarning("Not enough parameters for action.");
            return;
        }

        InvokeAction(metaData, parameters[0], parameters[1]);
    }

}

public abstract class ThreeParamAction : ActionScriptableObjectBase
{
    public abstract void InvokeAction(ActionMetaData metaData, string parameter1, string parameter2, string parameter3);

    public override void InvokeAction(ActionMetaData metaData, List<string> parameters)
    {
        // validate the number of parameters
        if (parameters.Count < 3)
        {
            Debug.LogWarning("Not enough parameters for action.");
            return;
        }

        InvokeAction(metaData, parameters[0], parameters[1], parameters[2]);
    }
}

[System.Serializable]
public class ActionParamPair
{
    public ActionScriptableObjectBase action;
    public List<string> parameters;

    public void InvokeAction(ActionMetaData metaData = new())
    {
        if (action == null || parameters == null)
        {
            Debug.LogWarning("Action or parameters are not assigned.");
            return;
        }

        action.InvokeAction(metaData, parameters);
    }
}