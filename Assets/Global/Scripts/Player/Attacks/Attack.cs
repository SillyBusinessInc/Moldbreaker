using System.Collections;
using UnityEngine;
using System;
using UnityEngine.Serialization;

public abstract class Attack : ScriptableObject
{
    [FormerlySerializedAs("Name")] 
    public new string name;
    public float damage;

    public Attack(string name_, float damage_)
    {
        name = name_;
        damage = damage_;
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
