using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using EnemiesNS;


public class Tail : MonoBehaviour
{
    public Player player;
    public BaseTail currentTail;
    public List<BaseTail> tails;
    public List<Attack> attacks;

    public TailStatistic tailStatistic = new(); 

    [HideInInspector] public int attackIndex;

    [HideInInspector] public float tailDoDamage;
    [HideInInspector] public float cooldownTime, activeCooldownTime;
    [HideInInspector] public float activeResetComboTime;

    [HideInInspector] public bool tailCanDoDamage = false;
    
    [HideInInspector] public GameObject slamObject;
    [HideInInspector] public float slamObjectSize = 1.0f;

    public void Start() {}

    public void Update()
    {
        activeResetComboTime =
            player.currentState.GetType().Name != "AttackingState"
                ? activeResetComboTime + Time.deltaTime
                : 0.0f;

        if (activeResetComboTime >= tailStatistic.comboResetTime.GetValue())
        {
            attackIndex = 0;
            activeResetComboTime = 0.0f;
        }

        activeCooldownTime =
            player.currentState.GetType().Name != "AttackingState"
                ? activeCooldownTime + Time.deltaTime
                : activeCooldownTime;
    }

    public void OnTriggerEnter(Collider Collider)
    {
        if (!Collider.gameObject.CompareTag("Enemy")    ||
            !tailCanDoDamage                            ||
            player.collidersEnemy.Contains(Collider)    ||
            Collider.GetComponent<EnemiesNS.EnemyBase>() == null
        ) return;
        GlobalReference.GetReference<AudioManager>().PlaySFX(GlobalReference.GetReference<AudioManager>().hitEnemy);
        player.collidersEnemy.Add(Collider);
        float actualDamage = tailDoDamage * player.playerStatistic.AttackDamageMultiplier.GetValue();
        Collider.GetComponent<EnemiesNS.EnemyBase>().OnHit((int)MathF.Round(actualDamage, 0));

        // displays feedback message when there's a combo
        // if (attackIndex == currentTail.currentCombo.Count) {
        //     player.SetRandomFeedback();
        // }
        player.recentHits += 1;
        if (player.succesfullHitCounter == currentTail.currentCombo.Count - 1) player.SetRandomFeedback();
    }
}
