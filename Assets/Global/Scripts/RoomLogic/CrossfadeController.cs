using System.Collections;
using UnityEngine;

public class CrossfadeController : Reference
{
    public Animator animator;
    private float transitionTime = 1f;

    public IEnumerator Crossfade_Start() {
        animator.SetTrigger("start");
        yield return new WaitForSeconds(transitionTime);
    }

    public IEnumerator Crossfade_End() {
        animator.SetTrigger("end");
        yield return new WaitForSeconds(transitionTime);
    }
}
