using UnityEngine;

public class ResetImage : MonoBehaviour
{

    [Header("目标缩放")]
    public Vector3 targetScale = new Vector3(2f, 2f, 2f); // 放大后的 Scale
    public float duration = 2f;                           // 动画时间

    private bool isScaling = false;
    private Vector3 originalScale;

    void Awake()
    {
        // 保存原始缩放
        originalScale = transform.localScale;
    }

    // 鼠标点击或手指点击调用
    public void Reset()
    {
        if (!isScaling)
            StartCoroutine(ScaleToTarget());
    }

    private System.Collections.IEnumerator ScaleToTarget()
    {
        isScaling = true;

        float time = 0f;
        Vector3 startScale = transform.localScale;

        while (time < duration)
        {
            time += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, targetScale, time / duration);
            yield return null;
        }

        transform.localScale = targetScale;
        isScaling = false;
    }

    // 可选：恢复原始缩放
    public void ResetScale()
    {
        transform.localScale = originalScale;
    }
}
