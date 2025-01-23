using System.Collections;
using UnityEngine;

public class CrossFadeController : Reference
{
    public Animator animator;
    private readonly float transitionTime = 1f;
    private string lastState = "end";

    public IEnumerator CrossFadeStart() => FadeAnim("start");
    public IEnumerator CrossFadeEnd() => FadeAnim("end");
    
    private IEnumerator FadeAnim(string state)
    {
        if (lastState == state) yield break;
        lastState = state;
        animator.SetTrigger(state);
        yield return new WaitForSeconds(transitionTime);
    }
}
