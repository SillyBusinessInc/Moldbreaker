using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractableUpgradeUI : MonoBehaviour
{
    [Header("Upgrade Option")]
    [SerializeField] private UpgradeOption option;

    [Header("Interaction")]
    [SerializeField] private List<ActionParamPair> interactionActions;

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.GetComponent<PlayerObject>() != null) {
            if (option != null) {
                GlobalReference.GetReference<UpgradeOptions>().option = option;
            }
            GlobalReference.GetReference<UpgradeOptions>().ShowOption();
            GlobalReference.GetReference<UpgradeOptions>().interactionActions = interactionActions;
            Destroy(gameObject);
        }
    }
}
