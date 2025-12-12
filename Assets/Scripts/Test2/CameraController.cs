using UnityEngine;
using UnityEngine.UI;
using System;
using System.Runtime.InteropServices;

public class CameraController : MonoBehaviour
{
    [Header("Preview UI")]
    public RawImage previewImage;


    // Native JS bindings (only works on WebGL)
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void StartCamera(bool front, string unityObj);

    [DllImport("__Internal")]
    private static extern void TakePhoto();
#endif

    // Unity-native camera
    private WebCamTexture webcamTexture;
    private Texture2D photoTexture;

    void Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        // ---- WebGL：调用 JS ----
        StartCamera(true, gameObject.name);
#else
        // ---- Unity Standalone / Editor：使用 WebCamTexture ----
        StartUnityCamera();
#endif
    }



    // -------------------------------
    // UNITY 内置摄像头（Editor / PC / Mobile）
    // -------------------------------
    private void StartUnityCamera(bool front = true)
    {
        if (webcamTexture != null)
        {
            webcamTexture.Stop();
        }

        WebCamDevice[] devices = WebCamTexture.devices;
        if (devices.Length == 0)
        {
            Debug.LogError("No camera detected");
            return;
        }

        string selected = null;
        foreach (var d in devices)
        {
            if (front && d.isFrontFacing)
            {
                selected = d.name;
                break;
            }
            if (!front && !d.isFrontFacing)
            {
                selected = d.name;
                break;
            }
        }
        if (selected == null) selected = devices[0].name;

        webcamTexture = new WebCamTexture(selected);
        webcamTexture.Play();

        previewImage.texture = webcamTexture;
    }

    // -------------------------------
    // WebGL JS 回调：返回 base64
    // -------------------------------
    public void OnCameraFrame(string base64)
    {
        byte[] bytes = Convert.FromBase64String(base64.Replace("data:image/png;base64,", ""));
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(bytes);
        photoTexture = tex;

        previewImage.texture = tex;
    }

    // -------------------------------
    // 拍照（跨平台）
    // -------------------------------
    public void Capture()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        TakePhoto();
#else
        if (webcamTexture == null) return;

        Texture2D tex = new Texture2D(webcamTexture.width, webcamTexture.height);
        tex.SetPixels(webcamTexture.GetPixels());
        tex.Apply();

        photoTexture = tex;
        previewImage.texture = tex;
#endif
    }

    // -------------------------------
    // 切换前后摄像头（跨平台）
    // -------------------------------
    public void SwitchCamera(bool useFront)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        StartCamera(useFront, gameObject.name);
#else
        StartUnityCamera(useFront);
#endif
    }

    // -------------------------------
    // 保存照片（所有平台可用）
    // -------------------------------
    public void SavePhoto()
    {
        if (photoTexture == null) return;

        byte[] png = photoTexture.EncodeToPNG();
        string path = Application.persistentDataPath + "/photo.png";
        System.IO.File.WriteAllBytes(path, png);

        Debug.Log("Saved to: " + path);

#if UNITY_WEBGL && !UNITY_EDITOR
        // WebGL 的浏览器下载方式
        string base64 = Convert.ToBase64String(png);
        Application.ExternalEval(@"
            var a = document.createElement('a');
            a.href = 'data:image/png;base64," + base64 + @"';
            a.download = 'photo.png';
            a.click();
        ");
#endif
    }
}
