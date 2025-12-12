using UnityEngine;
using UnityEngine.UI;

public class ApplyShaderToRawImage : MonoBehaviour
{
    public RawImage rawImage;       // ÍÏÈëÄãµÄ RawImage
    public Material filterMat;      // ÍÏÈë BlackWhiteRetroMat

    public void Start()
    {
      //  if (rawImage != null && filterMat != null)
            rawImage.material = filterMat;
    }
}
