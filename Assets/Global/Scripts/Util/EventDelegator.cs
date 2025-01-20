using UnityEngine;

// This script is used to delegate events from one event to another.
// It is not made for any specific feature.
// instead it's purpose is to quickly change a levels functionality without having to write new code every time.
public class EventDelegator : MonoBehaviour
{
    [SerializeField] private Events from;
    [SerializeField] private Events to;
    
    void Start() => GlobalReference.SubscribeTo(from, RunTo);
    void OnDestroy() => GlobalReference.UnsubscribeTo(from, RunTo);
    private void RunTo() => GlobalReference.AttemptInvoke(to);
}