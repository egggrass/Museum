using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WebcamController : MonoBehaviour
{
    public RawImage display;
    public AspectRatioFitter fitter;

    private WebCamTexture webcamTexture;
    private WebCamDevice[] devices;
    private int currentCamIndex = 0;

    void Start()
    {
        StartCoroutine(InitCamera());
    }

    IEnumerator InitCamera()
    {
        devices = WebCamTexture.devices;

        if (devices.Length == 0)
        {
            Debug.Log("No camera detected");
            yield break;
        }

        yield return StartCoroutine(StartCamera(currentCamIndex));
    }

    IEnumerator StartCamera(int index)
    {
        if (webcamTexture != null)
        {
            webcamTexture.Stop();
        }

        string camName = devices[index].name;
        webcamTexture = new WebCamTexture(camName, Screen.width, Screen.height, 30);
        display.texture = webcamTexture;
        webcamTexture.Play();

        // µÈ´ýÁ÷¿ªÆô
        while (webcamTexture.width < 100)
            yield return null;

       fitter.aspectRatio = (float)webcamTexture.width / webcamTexture.height;
    }

    public void SwitchCamera()
    {
        currentCamIndex++;
        if (currentCamIndex >= devices.Length) currentCamIndex = 0;
        StartCoroutine(StartCamera(currentCamIndex));
    }

    public Texture2D TakePhoto()
    {
        Texture2D photo = new Texture2D(webcamTexture.width, webcamTexture.height);
        photo.SetPixels(webcamTexture.GetPixels());
        photo.Apply();
        return photo;
    }
}
