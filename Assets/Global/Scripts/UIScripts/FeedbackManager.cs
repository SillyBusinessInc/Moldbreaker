using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeedbackManager : MonoBehaviour
{
    [SerializeField] private float shakeDuration = 0.8f;
    [SerializeField] private float shakeStrength = 130f;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private List<Sprite> feedbackSprites;
    [SerializeField] private Transform cam;
    [SerializeField] private Transform camShaking;
    [SerializeField] private GameObject feedbackGameObject;
    [SerializeField] private Image linesObject;

    private SpriteRenderer feedbackSpriteRenderer;
    private Vector3 originalScale;
    private Transform player;
    private bool isFeedbackActive = false;

    void Start()
    {
        feedbackSpriteRenderer = feedbackGameObject.GetComponent<SpriteRenderer>();
        originalScale = feedbackGameObject.transform.localScale;
        player = GlobalReference.GetReference<PlayerReference>().PlayerObj.transform;
    }
    void FixedUpdate()
    {
        Vector3 leftPosition = player.position - player.right * 2f;
        leftPosition.y = player.position.y + 1.41f;
        transform.position = leftPosition;
        transform.LookAt(transform.position + cam.forward);
    }

    public void SetRandomFeedback() {
        if (feedbackSprites.Count > 0) {
            int randomNumber = Random.Range(0, feedbackSprites.Count);
            feedbackSpriteRenderer.sprite = feedbackSprites[randomNumber];
            isFeedbackActive = true;
            StartCoroutine(FeedbackAppears());
        }
    }

    IEnumerator FeedbackAppears() {
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(LinesAppear());
        yield return StartCoroutine(Appear());
        yield return StartCoroutine(Shaking());
        
        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(Disappear());
        yield return StartCoroutine(LinesDisappear());
        isFeedbackActive = false;
    }

    // makes lines appear
    IEnumerator LinesAppear() {
        if (!isFeedbackActive) yield break;

        linesObject.rectTransform.localScale = new Vector3(100, 1, 1);
        float elapsedTime = 0f;
        float time = 0.2f;

        while (elapsedTime < time) {
            elapsedTime += Time.deltaTime;
            linesObject.rectTransform.localScale = Vector3.Lerp(new Vector3(100, 1, 0), Vector3.one, elapsedTime / time);
            yield return null;
        }

        linesObject.rectTransform.localScale = Vector3.one;
    }

    // makes feedback appear
    IEnumerator Appear() {
        if (!isFeedbackActive) yield break;

        Vector3 initialScale = Vector3.zero;
        float elapsedTime = 0f;
        float time = 0.3f;

        while (elapsedTime < time) {
            elapsedTime += Time.deltaTime;
            feedbackGameObject.transform.localScale = Vector3.Lerp(initialScale, originalScale, elapsedTime / time);
            yield return null;
        }

        feedbackGameObject.transform.localScale = originalScale;
    }


    // makes lines disappear
    IEnumerator LinesDisappear() {
        if (!isFeedbackActive) yield break;

        float elapsedTime = 0f;
        float time = 0.2f;

        while (elapsedTime < time) {
            elapsedTime += Time.deltaTime;
            linesObject.rectTransform.localScale = Vector3.Lerp(Vector3.one, new Vector3(100, 1, 1), elapsedTime / time);
            yield return null;
        }

        linesObject.rectTransform.localScale = new Vector3(100, 1, 1);
    }

    // makes feedback disappear
    IEnumerator Disappear() {
        if (!isFeedbackActive) yield break;

        Vector3 initialScale = originalScale;
        float elapsedTime = 0f;
        float time = 0.3f;
        
        while (elapsedTime < time) {
            elapsedTime += Time.deltaTime;
            feedbackGameObject.transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, elapsedTime / time);
            yield return null;
        }

        feedbackGameObject.transform.localScale = Vector3.zero;
    }

    IEnumerator Shaking() {
        Vector3 startPosition = camShaking.transform.localPosition;
        float elapsedTime = 0f;
        while (elapsedTime < shakeDuration) {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / shakeDuration) * shakeStrength;
            camShaking.transform.localPosition = startPosition + Random.insideUnitSphere * strength;
            yield return null;
        }

        camShaking.transform.localPosition = new Vector3(0, 1, 0);
    }
}
