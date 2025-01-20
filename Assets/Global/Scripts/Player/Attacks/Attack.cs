using System.Collections;
using UnityEngine;
using System;

public abstract class Attack : ScriptableObject
{
    public string Name;
    public float damage;

    public Attack(string Name, float damage)
    {
        this.Name = Name;
        this.damage = damage;
    }

    public abstract void Start();
    public abstract IEnumerator SetStateIdle();

    public void ClipDuration(Animator animator, float targetDuration, string clip)
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        AnimationClip clipToChange = Array.Find(clips, x => x.name == clip);
        animator.speed *= clipToChange.length / targetDuration;
    }
}
