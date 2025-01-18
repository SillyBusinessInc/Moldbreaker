using System.Collections.Generic;
using UnityEngine;

// This is a simple utility script that enables a list of objects when a specific event is triggered.
// It is not made for any specific feature.
// instead it's purpose is to quickly change a levels functionality without having to write new code every time.
public class EnableOnEvent : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectsToEnable;
    [SerializeField] private Events enableEvent;
    
    void Start()
    {
        foreach (var obj in objectsToEnable)
        {
            obj.SetActive(false);
        }
        
        GlobalReference.SubscribeTo(this.enableEvent, EnableAll);
    }

    private void EnableAll()
    {
        foreach (var obj in objectsToEnable)
        {
            obj.SetActive(true);
        }
    }
}
