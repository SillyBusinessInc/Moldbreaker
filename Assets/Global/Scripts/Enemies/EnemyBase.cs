using UnityEngine;

// Base enemy class
public abstract class EnemyBase : MonoBehaviour
{
    [Header("Base Enemy Fields")]
    [SerializeField]
    [Range(0, 250)]
    private int health = 100;

    private void Start()
    {
        GlobalReference.AttemptInvoke(Events.ENEMY_SPAWNED);
    }

    void Update()
    {
        // Base enemy update
    }

    void Move()
    {
        // Base enemy Move-function
    }

    void Attack()
    {
        // Base enemy Attack-function
    }

    void OnHit(int damage)
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
        //Debug.Log($"{this.name} OnDeath() triggered", this);
    }
}
