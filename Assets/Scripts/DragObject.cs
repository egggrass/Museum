using UnityEngine;

public class DragObject : MonoBehaviour
{
    private Camera mainCamera;
    private float cameraDistance;

    void Start()
    {
        mainCamera = Camera.main;
        cameraDistance = Vector3.Distance(transform.position, mainCamera.transform.position);
    }

    void Update()
    {
        // 同时支持鼠标和触摸
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            // 检测是否点击到这个物体
            if (IsTouchingObject())
            {
                StartDrag();
            }
        }

        if (Input.GetMouseButton(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved))
        {
            ContinueDrag();
        }
    }

    bool IsTouchingObject()
    {
        // 获取点击位置
        Vector2 inputPos = GetInputPosition();

        // 发射射线检测
        Ray ray = mainCamera.ScreenPointToRay(inputPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return hit.collider.gameObject == gameObject;
        }

        return false;
    }

    void StartDrag()
    {
        // 不需要保存偏移量，直接跟随
    }

    void ContinueDrag()
    {
        // 获取当前输入位置
        Vector2 inputPos = GetInputPosition();

        // 转换为世界坐标
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(inputPos.x, inputPos.y, cameraDistance));

        // 更新物体位置
        transform.position = worldPos;
    }

    Vector2 GetInputPosition()
    {
        // 返回鼠标或触摸位置
        if (Input.touchCount > 0)
            return Input.GetTouch(0).position;
        else
            return Input.mousePosition;
    }
}