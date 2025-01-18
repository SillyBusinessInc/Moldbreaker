using UnityEngine;

public class BoneTransformFollow : MonoBehaviour
{
    [SerializeField] private Transform bone;
    [SerializeField] private Vector3 boneOffsetPosition = Vector3.zero;
    [SerializeField] private Vector3 boneOffsetRotation = Vector3.zero;

    void Update()
    {
        this.transform.SetPositionAndRotation(
            bone.position + bone.rotation * boneOffsetPosition, 
            bone.rotation * Quaternion.Euler(boneOffsetRotation)
            );
    }
}