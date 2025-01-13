using UnityEngine;
using UnityEngine.UI;

public class MoldOrbHealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private MoldOrb moldOrb;

    void Start()
    {
        healthSlider.maxValue = moldOrb.maxHealth;
    }
    void Update()
    {
        if (moldOrb.HealthBarDestroy)
        {
            Destroy(gameObject);
        }
        healthSlider.value = (float)moldOrb.health/(float)moldOrb.maxHealth*100;
    }
}
