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

    /// <summary>
    /// 放置物品到天平
    /// </summary>
    public void PlaceItem(Item item, bool isLeft)
    {
        // 情况 1：物品已经在天平上 → 移除
        if (item.isPlaced)
        {
            RemoveItem(item);
            UpdateBalance();
            return;
        }

        // 情况 2：放到左盘
        if (isLeft)
        {
            // 左盘已有物品 → 先移除
            if (leftItem != null)
            {
                leftItem.ReturnToOriginal();
                leftItem = null;
            }

            leftItem = item;
            AttachItemToPlate(item, leftPlate);
        }
        // 情况 3：放到右盘
        else
        {
            if (rightItem != null)
            {
                rightItem.ReturnToOriginal();
                rightItem = null;
            }

            rightItem = item;
            AttachItemToPlate(item, rightPlate);
        }

        UpdateBalance();
    }

    /// <summary>
    /// 将物品绑定到盘上
    /// </summary>
    private void AttachItemToPlate(Item item, Transform plate)
    {
        item.transform.SetParent(plate);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
        item.isPlaced = true;
    }

    /// <summary>
    /// 从天平移除物品
    /// </summary>
    private void RemoveItem(Item item)
    {
        if (leftItem == item) leftItem = null;
        if (rightItem == item) rightItem = null;

        item.ReturnToOriginal(); // ⭐ 关键：由 Item 自己恢复父物体与位置
    }

    /// <summary>
    /// 根据重量更新天平高度
    /// </summary>
    private void UpdateBalance()
    {
        // 默认回到水平
        leftTargetPos = leftOrigin;
        rightTargetPos = rightOrigin;

        // 少于两个物品，不倾斜
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
        // 重量相等 → 保持水平
    }
}
