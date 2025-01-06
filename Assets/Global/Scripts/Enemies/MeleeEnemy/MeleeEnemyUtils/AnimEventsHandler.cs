using UnityEngine;

public class AnimEventsHandler : MonoBehaviour
{
    [Tooltip("Reference to the enemy. NOTE: this field has a default only set manually when broken")]
    [SerializeField]
    private EnemiesNS.EnemyBase enemy;

    void Start()
    {
        if (!enemy)
        {
            enemy = this.GetComponentInParent<EnemiesNS.EnemyBase>();
            Debug.Log(enemy, this);
        }
    }

    void AttackAnimStarted() => enemy.AttackAnimStarted();

    void AttackAnimEnded() => enemy.AttackAnimEnded();

    void EnableHitBox() => enemy.EnableWeaponHitBox();

    void DisableHitBox() => enemy.DisableWeaponHitBox();
    void DeathAnimEnded() => enemy.DeathAnimEnded();
}
