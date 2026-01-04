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
                // 点物品
                Item item = hit.collider.GetComponent<Item>();
                if (item != null)
                {
                    HandleItemClick(item);
                    return;
                }

                // 点天平左 / 右
                if (hit.collider.name.Contains("Left"))
                {
                    if (selectedItem != null)
                        balance.PlaceItem(selectedItem, true);
                }
                else if (hit.collider.name.Contains("Right"))
                {
                    if (selectedItem != null)
                        balance.PlaceItem(selectedItem, false);
                }
            }
        }
    }

    void HandleItemClick(Item item)
    {
        // 已在天平上 → 回原位
        if (item.isPlaced)
        {
            item.ReturnToOriginal();
            selectedItem = null;
            return;
        }

        // 选中物品
        selectedItem = item;
        Debug.Log("Selected: " + item.name);
    }
}
