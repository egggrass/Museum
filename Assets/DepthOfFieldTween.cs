using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DepthOfFieldTween : MonoBehaviour
{
    [Header("Volume and DOF")]
    public Volume volume;           // Global Volume
    private DepthOfField dof;

    [Header("Focus Settings")]
    public float startFocus = 0.1f; // 初始模糊焦距
    public float endFocus = 10f;    // 最终清晰焦距
    public float duration = 2f;     // 过渡时间（秒）

    private float timer = 2f;
    private bool running = true;

    void Start()
    {
        if (volume == null)
        {
            Debug.LogError("Volume not assigned!");
            running = false;
            return;
        }

        // 获取 DepthOfField
        if (!volume.profile.TryGet<DepthOfField>(out dof))
        {
            Debug.LogError("DepthOfField not found in Volume profile!");
            running = false;
            return;
        }

        dof.focusDistance.value = startFocus;
        dof.active = true; // 确保启用
    }

    void Update()
    {
        if (!running || dof == null) return;

        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / duration);

        // 平滑插值
        dof.focusDistance.value = Mathf.Lerp(startFocus, endFocus, t);
        Debug.Log("Depth");
    }
}
