using UnityEngine;

public abstract class Reference : MonoBehaviour
{
    protected virtual void Awake() => GlobalReference.RegisterReference(this);
    void OnDestroy() => GlobalReference.UnregisterReference(this);
}