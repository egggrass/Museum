using UnityEngine;

public class TextMaskParticleEmitter : MonoBehaviour
{
    [Header("References")]
    public ParticleSystem particleSystem;
    public Texture2D textMask;

    [Header("Text Area Size (World Units)")]
    public Vector2 areaSize = new Vector2(5f, 2f);

    [Header("Emit Settings")]
    public int emitPerFrame = 30;
    [Range(0f, 1f)]
    public float alphaThreshold = 0.5f;

    void Update()
    {
        EmitParticlesInText();
    }

    void EmitParticlesInText()
    {
        if (particleSystem == null || textMask == null) return;

        for (int i = 0; i < emitPerFrame; i++)
        {
            Vector2 localPos;
            Color pixel;

            int safety = 0;
            do
            {
                localPos = new Vector2(
                    Random.Range(-areaSize.x * 0.5f, areaSize.x * 0.5f),
                    Random.Range(-areaSize.y * 0.5f, areaSize.y * 0.5f)
                );

                float u = localPos.x / areaSize.x + 0.5f;
                float v = localPos.y / areaSize.y + 0.5f;

                pixel = textMask.GetPixelBilinear(u, v);

                safety++;
                if (safety > 20) return;

            } while (pixel.a < alphaThreshold);

            ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
            emitParams.position = transform.TransformPoint(localPos);

            particleSystem.Emit(emitParams, 1);
        }
    }
}
