using UnityEngine;

public class Item : MonoBehaviour
{
    public float weight = 1f;

    [HideInInspector] public Vector3 originalPosition;
    [HideInInspector] public bool isPlaced = false;

    void Start()
    {
        originalPosition = transform.position;
    }

    public void ReturnToOriginal()
    {
        transform.position = originalPosition;
        isPlaced = false;
    }
}
