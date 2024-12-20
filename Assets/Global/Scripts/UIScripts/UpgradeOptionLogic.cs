using System;
using TMPro;
using UnityEngine;

public class UpgradeOptionLogic : MonoBehaviour
{
    public UpgradeOption data;
    public UnityEngine.UI.Image image;
    [HideInInspector] public TMP_Text upgradeName;
    [HideInInspector] public TMP_Text description;

    void Start()
    {
        upgradeName = transform.GetChild(0).GetComponent<TMP_Text>();
        description = transform.GetChild(1).GetComponent<TMP_Text>();

        image.sprite = data.image;
        upgradeName.text = data.name;
        description.text = data.description ?? "Hmm yes, yeast of power. So powerful";
    }
}