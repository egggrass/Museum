using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VintageFeature : ScriptableRendererFeature
{
    class VintagePass : ScriptableRenderPass
    {
        private Material material;
        private RTHandle cameraColorTarget;

        public VintagePass(Material mat)
        {
            material = mat;
            // ⚡ 尝试更保险的事件
            renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
            
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            cameraColorTarget = renderingData.cameraData.renderer.cameraColorTargetHandle;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (material == null) return;

            CommandBuffer cmd = CommandBufferPool.Get("Vintage Pass");
            Blitter.BlitCameraTexture(cmd, cameraColorTarget, cameraColorTarget, material, 0);
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    public Material material;
    VintagePass pass;

    public override void Create()
    {
        pass = new VintagePass(material);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (material == null) return;
        renderer.EnqueuePass(pass);
    }
}
