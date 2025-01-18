using System;
using UnityEngine;

public class FlipCollidor : MonoBehaviour
{
    public Player player;

    public void OnTriggerEnter(Collider Collider)
    {
        if (!Collider.gameObject.CompareTag("Enemy") ||
            Collider.GetComponent<EnemiesNS.EnemyBase>() == null
        ) return;

        float actualDamage = player.Tail.tailDoDamage * player.playerStatistic.AttackDamageMultiplier.GetValue();
        Collider.GetComponent<EnemiesNS.EnemyBase>().OnHit((int)MathF.Round(actualDamage, 0));

        player.recentHits += 1;
        if (player.succesfullHitCounter == player.Tail.currentTail.currentCombo.Count - 1) player.SetRandomFeedback();
    }
}
