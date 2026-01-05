using UnityEngine;

public class Item : MonoBehaviour
{
    public float weight = 1f;
    public bool isPlaced = false;

    private Transform originalParent;
    private Vector3 originalLocalPos;
    private Quaternion originalLocalRot;

    void Awake()
    {
        originalParent = transform.parent;
        originalLocalPos = transform.localPosition;
        originalLocalRot = transform.localRotation;
    }

    public void ReturnToOriginal()
    {
        transform.SetParent(originalParent);
        transform.localPosition = originalLocalPos;
        transform.localRotation = originalLocalRot;
        isPlaced = false;
    }
}
