using UnityEngine;

public class EventDelegator : MonoBehaviour
{
    [SerializeField] private Events from;
    [SerializeField] private Events to;
    
    void Start() => GlobalReference.SubscribeTo(from, RunTo);
    private void RunTo() => GlobalReference.AttemptInvoke(to);
    private void OnDestroy() => GlobalReference.UnsubscribeTo(from, RunTo);
}
