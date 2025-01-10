using UnityEngine;
using UnityEngine.UI;

public class MoldOrbHealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private MoldOrb moldOrb;

    void Start()
    {
        // healthSlider.gameObject.SetActive(false);
        healthSlider.maxValue = moldOrb.maxHealth;
    }
    void Update()
    {
        // if (moldOrb.health != moldOrb.maxHealth) {
        //     healthSlider.gameObject.SetActive(true);
        // }

        if (moldOrb.HealthBarDestroy)
        {
            Destroy(gameObject);
        }
        Debug.Log("health : " + moldOrb.health);

        healthSlider.value = (float)moldOrb.health/(float)moldOrb.maxHealth*100;
    }
}
