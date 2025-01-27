using System.Collections;
using UnityEngine;

public class CrossFadeController : Reference
{
    public Animator animator;
    private readonly float transitionTime = 1f;
    private string lastState = "end";

    public IEnumerator CrossFadeStart()
    {
        this.gameObject.SetActive(true);
        yield return FadeAnim("start");
    }

    public IEnumerator CrossFadeEnd()
    {
        yield return FadeAnim("end");
        this.gameObject.SetActive(false);
    }
    
    private IEnumerator FadeAnim(string state)
    {
        if (lastState == state) yield break;
        lastState = state;
        animator.SetTrigger(state);
        yield return new WaitForSeconds(transitionTime);
    }
}
