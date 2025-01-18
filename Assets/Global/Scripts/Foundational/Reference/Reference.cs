using UnityEngine;

public abstract class Reference : MonoBehaviour
{
    protected virtual void Awake() => GlobalReference.RegisterReference(this);
    protected virtual void OnDestroy() => GlobalReference.UnregisterReference(this);
}