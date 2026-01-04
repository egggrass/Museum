using UnityEngine;

public class Balance : MonoBehaviour
{
    [Header("Plates")]
    public Transform leftPlate;
    public Transform rightPlate;

    [Header("Balance Settings")]
    public float heightOffset = 0.3f;
    public float smoothSpeed = 3f;

    private Item leftItem;
    private Item rightItem;

    private Vector3 leftOrigin;
    private Vector3 rightOrigin;

    private Vector3 leftTargetPos;
    private Vector3 rightTargetPos;

    void Start()
    {
        leftOrigin = leftPlate.position;
        rightOrigin = rightPlate.position;

        leftTargetPos = leftOrigin;
        rightTargetPos = rightOrigin;
    }

    void Update()
    {
        leftPlate.position = Vector3.Lerp(
            leftPlate.position,
            leftTargetPos,
            Time.deltaTime * smoothSpeed
        );

        rightPlate.position = Vector3.Lerp(
            rightPlate.position,
            rightTargetPos,
            Time.deltaTime * smoothSpeed
        );
    }

    public void PlaceItem(Item item, bool isLeft)
    {
        // 已经在天平上 → 放回原位
        if (item.isPlaced)
        {
            RemoveItem(item);
            item.ReturnToOriginal();
            UpdateBalance();
            return;
        }

        // 放到左盘
        if (isLeft)
        {
            if (leftItem != null)
            {
                leftItem.ReturnToOriginal();
                leftItem.isPlaced = false;
            }

            leftItem = item;
            item.transform.position = leftPlate.position;
        }
        // 放到右盘
        else
        {
            if (rightItem != null)
            {
                rightItem.ReturnToOriginal();
                rightItem.isPlaced = false;
            }

            rightItem = item;
            item.transform.position = rightPlate.position;
        }

        item.isPlaced = true;
        UpdateBalance();
    }

    void RemoveItem(Item item)
    {
        if (leftItem == item) leftItem = null;
        if (rightItem == item) rightItem = null;
    }

    void UpdateBalance()
    {
        // 默认回到水平
        leftTargetPos = leftOrigin;
        rightTargetPos = rightOrigin;

        if (leftItem == null || rightItem == null)
            return;

        if (leftItem.weight > rightItem.weight)
        {
            leftTargetPos = leftOrigin + Vector3.down * heightOffset;
            rightTargetPos = rightOrigin + Vector3.up * heightOffset;
        }
        else if (leftItem.weight < rightItem.weight)
        {
            rightTargetPos = rightOrigin + Vector3.down * heightOffset;
            leftTargetPos = leftOrigin + Vector3.up * heightOffset;
        }
    }
}
