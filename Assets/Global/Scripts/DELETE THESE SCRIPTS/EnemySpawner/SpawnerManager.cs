using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public List<GameObject> waveTypes;
    private int currentType = 0;

    void Start() => GlobalReference.AttemptInvoke(Events.NEXT_SPAWNER);

    private void Awake() => GlobalReference.SubscribeTo(Events.NEXT_SPAWNER, NextType);

    private void OnDestroy() => GlobalReference.UnsubscribeTo(Events.NEXT_SPAWNER, NextType);
    
    private void NextType()
    {
        if (currentType >= waveTypes.Count) {
            GlobalReference.AttemptInvoke(Events.ALL_NEXT_SPAWNERS_DONE);
            return;
        }
        
        if (currentType > 0 && waveTypes[currentType - 1] != null) {
            waveTypes[currentType - 1].SetActive(false);
        }

        waveTypes[currentType].SetActive(true);
        currentType++;
    }
}