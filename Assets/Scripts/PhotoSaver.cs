using System.Runtime.InteropServices;
using UnityEngine;

public class PhotoSaver : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void SaveImage(string imgData, string fileName);

    public void Save(Texture2D tex)
    {
        byte[] bytes = tex.EncodeToPNG();
        string base64 = System.Convert.ToBase64String(bytes);

#if UNITY_WEBGL && !UNITY_EDITOR
        SaveImage(base64, "photo.png");
#endif

        Debug.Log("Saved!");
    }
}
