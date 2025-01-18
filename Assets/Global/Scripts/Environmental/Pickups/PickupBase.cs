using System;
using System.Collections;
using UnityEngine;

public abstract class PickupBase : MonoBehaviour
{
    public float pickupRange = 2;
    public float pickupSpeed = 0.1f;
    public bool isLocked = false;
    public bool isStatic = false;

    private Transform target;

    private Rigidbody rb;
    private ParticleSystem ps;
    private MeshRenderer mr;
    private Collider c;

    protected virtual void Start() 
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null) isStatic = true;
        else if (isStatic) rb.isKinematic = true;

        ps = GetComponentInChildren<ParticleSystem>();
        mr = GetComponentInChildren<MeshRenderer>();
        c = GetComponentInChildren<Collider>();

        target = GlobalReference.GetReference<PlayerReference>().PlayerObj.transform;
    }

    void Update()
    {
        if (isLocked) {
            return;
        }
        if (target == null) {
            Debug.LogError("pickup target is undefined");
            isLocked = true;
            return;
        }

        float distance = (target.position - transform.position).magnitude;
        if (distance <= pickupRange) {
            Collect();
            return;
        }
    }

    public void Collect() {
        // freeze object
        isLocked = true;
        if (rb != null && !rb.isKinematic) {
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
        }
        c.enabled = false;

        // calculate collection path
        Vector3 startPos = transform.position;
        Vector3 targetPos = target.position;
        Vector3 disVec = targetPos - startPos;
        float distance = disVec.magnitude;

        Vector3 normal = Vector3.Cross(Vector3.Cross(disVec, Vector3.up), disVec).normalized;

        Func<float, float> arch = (x) => Mathf.Pow(x, 2) * 1/distance * -1 + x;
        Func<float, Vector3> getPos = (x) => Vector3.Lerp(startPos, targetPos, x/distance) + normal * arch(x);

        StartCoroutine(CollectAnimation(distance, getPos, pickupSpeed, 0.01f));
    }

    private void OnCollectCompleted() {
        OnTrigger();
        if (ps != null) {
            ps.Play();
            mr.enabled = false;
            Destroy(gameObject, ps.main.duration + ps.main.startLifetime.constant);
        }
        else {
            Destroy(gameObject);
        }
    }

    private IEnumerator CollectAnimation(float distance, Func<float, Vector3> getPos, float duration = 0.4f, float stepSize = 0.01f) {
        float startTime = Time.time;
        float endTime = startTime + duration;
        
        while (Time.time < endTime) {
            float pos = Mathf.Pow((Time.time-startTime) / (endTime-startTime), 2) * distance;
            transform.position = getPos(pos);
            yield return new WaitForSeconds(stepSize);
        }
        OnCollectCompleted();
    }

    protected abstract void OnTrigger();
}
