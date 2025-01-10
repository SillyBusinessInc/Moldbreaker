using UnityEngine;


public class MoldOrb : MonoBehaviour
{
    [SerializeField] private MoldOrbManager moldOrbManager;
    [HideInInspector] public bool HealthBarDestroy;
    public int health = 0;
    public int maxHealth = 100;


    void Start() {
        health = maxHealth;
    }

    public void OnHit(int damage)
    {
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
