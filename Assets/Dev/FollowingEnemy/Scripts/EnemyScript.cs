using UnityEngine;

// Base enemy class
public class EnemyScript : MonoBehaviour
{
    // Base enemy fields
    [SerializeField]
    [Range(0, 250)]
    private int _health = 100;

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

    void OnHit()
    {
        // Base enemy OnHit-function
    }

    void OnDeath()
    {
        // Base enemy OnDeath-function
    }
}
