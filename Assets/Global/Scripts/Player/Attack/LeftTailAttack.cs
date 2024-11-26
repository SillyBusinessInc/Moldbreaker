using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;

public class LeftTailAttack : TailAttack
{
    public void Start()
    {
        //Debug.Log("In lefttail attack class");
        player.tailDoDamage = player.leftTailDamage;
        player.tailCanDoDamage = true;
        Animator animatorTailAttack = GlobalReference.GetReference<PlayerReference>().GetComponent<Player>().Tail.GetComponent<Tail>().animatorTailAttack;
        animatorTailAttack.SetTrigger("LeftAttack");
        canDoDamage(0.5f);
    }
}