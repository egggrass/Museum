using UnityEngine;
using TMPro;

public class TMPTextToParticle : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public TMP_Text text;

    void Start()
    {
        text.ForceMeshUpdate();

        var shape = particleSystem.shape;
        shape.shapeType = ParticleSystemShapeType.Mesh;
        shape.mesh = text.mesh;
    }
}
