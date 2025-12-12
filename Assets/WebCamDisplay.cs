using UnityEngine;
using UnityEngine.UI;

public class WebCamDisplay : MonoBehaviour
{
    public RawImage rawImage;   // UI RawImage
    private WebCamTexture camTexture;

    void Start()
    {
        camTexture = new WebCamTexture();
        rawImage.texture = camTexture;
        camTexture.Play();
    }
}
