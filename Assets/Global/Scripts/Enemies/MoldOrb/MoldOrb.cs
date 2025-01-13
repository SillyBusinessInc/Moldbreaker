using UnityEngine;
using UnityEngine.UI;


public class MoldOrb : MonoBehaviour
{
    [SerializeField] private MoldOrbManager moldOrbManager;
    [SerializeField] private Slider healthSlider;

    [HideInInspector] public bool HealthBarDestroy;
    public int health = 0;
    public int maxHealth = 100;


    void Start() {
        health = maxHealth;
        healthSlider.gameObject.SetActive(false);
    }

    public void OnHit(int damage)
    {
        healthSlider.gameObject.SetActive(true);
        health -= damage;
        if (health <= 0)
        {
            OnDeath();
        }
    }

    void OnDeath()
    {
        GlobalReference.AttemptInvoke(Events.ENEMY_KILLED);
        Destroy(gameObject);
        moldOrbManager.RemoveFromList(this);
        moldOrbManager.DestroyDoor();
        HealthBarDestroy = true;
    }
}
