using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public Camera mainCamera;
    public LayerMask interactableLayer;
    public Balance balance;

    private Item selectedItem;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f, interactableLayer))
            {
                Item clickedItem = hit.collider.GetComponent<Item>();

                // ===== 1️⃣ 点击的是物品 =====
                if (clickedItem != null)
                {
                    HandleItemClick(clickedItem);
                    return;
                }

                // ===== 2️⃣ 点击的是天平 =====
                if (selectedItem != null)
                {
                    if (hit.collider.name.Contains("Left"))
                    {
                        balance.PlaceItem(selectedItem, true);
                        selectedItem = null;   // 放完就清空
                    }
                    else if (hit.collider.name.Contains("Right"))
                    {
                        balance.PlaceItem(selectedItem, false);
                        selectedItem = null;
                    }
                }
            }
        }
    }

    void HandleItemClick(Item clickedItem)
    {
        // ✅ 情况一：点的是【已经在天平上的物体】→ 只处理它自己
        if (clickedItem.isPlaced)
        {
            balance.PlaceItem(clickedItem, false); // 触发回归逻辑
            if (selectedItem == clickedItem)
                selectedItem = null;

            return;
        }

        // ✅ 情况二：点的是【新的未放置物体】

        // 如果之前选了别的物体（但还没放）
        if (selectedItem != null && selectedItem != clickedItem)
        {
            // 取消旧选择（这里只是逻辑层，不移动）
            selectedItem = null;
        }

        // 选中当前点击的物体
        selectedItem = clickedItem;
        Debug.Log("Selected: " + clickedItem.name);
    }
}
