using System.Collections;
using UnityEngine;

public class CrossfadeController : Reference
{
    public Animator animator;
    private float transitionTime = 1f;
    private string lastState = "end";

    public IEnumerator Crossfade_Start()
    {
        var newState = "start";
        if (lastState == newState) yield break;
        lastState = newState;
        animator.SetTrigger(newState);
        yield return new WaitForSeconds(transitionTime);
    }

    public IEnumerator Crossfade_End() {
        lastState = "end";
        animator.SetTrigger("end");
        yield return new WaitForSeconds(transitionTime);
    }
}
