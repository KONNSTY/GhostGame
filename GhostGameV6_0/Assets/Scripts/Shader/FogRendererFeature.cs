using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// SeeFog Component für selektive Kamera-Anwendung
public class SeeFog : MonoBehaviour
{
    // Diese Komponente kann an Kameras angehängt werden, um Fog zu aktivieren
}

public class FogRendererFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class FogSettings
    {
        public Material fogMaterial = null;
        [Range(0.0f, 1.0f)]
        public float blendFactor = 1.0f; // Wie stark soll der Nebel angewendet werden?
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
    }

    public FogSettings settings = new FogSettings();

    class CustomRenderPass : ScriptableRenderPass
    {
        public Material blitMaterial;
        public float blendFactor;

        private RTHandle source;
        private RTHandle tempRTHandle;

        public CustomRenderPass(Material material, float blend)
        {
            blitMaterial = material;
            blendFactor = blend;
        }

        public void SetSource(RTHandle source)
        {
            this.source = source;
        }

        [System.Obsolete]
        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            // Configure temporary RT for our blit operations
            RenderTextureDescriptor descriptor = cameraTextureDescriptor;
            descriptor.depthBufferBits = 0;
            
            #pragma warning disable CS0618 // Disable obsolete warning for compatibility
            RenderingUtils.ReAllocateHandleIfNeeded(ref tempRTHandle, descriptor, FilterMode.Bilinear, TextureWrapMode.Clamp, name: "_TempFogTexture");
            #pragma warning restore CS0618
        }

        [System.Obsolete]
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (blitMaterial == null)
            {
                Debug.LogError("Missing Fog Material in FogRendererFeature!");
                return;
            }

            if (source == null || tempRTHandle == null)
            {
                Debug.LogError("Source or temp RTHandle is null in FogRendererFeature!");
                return;
            }

            CommandBuffer cmd = CommandBufferPool.Get("SelectiveFogPass");

            // Blit source to temporary texture
            #pragma warning disable CS0618 // Disable obsolete warning for compatibility
            Blitter.BlitCameraTexture(cmd, source, tempRTHandle);

            // Apply fog material and blit back to source
            cmd.SetGlobalFloat("_BlendFactor", blendFactor);
            Blitter.BlitCameraTexture(cmd, tempRTHandle, source, blitMaterial, 0);
            #pragma warning restore CS0618

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            // RTHandles are managed automatically, no manual cleanup needed
        }

        public void Dispose()
        {
            tempRTHandle?.Release();
        }
    }

    CustomRenderPass m_ScriptablePass;

    public override void Create()
    {
        m_ScriptablePass = new CustomRenderPass(settings.fogMaterial, settings.blendFactor);
        m_ScriptablePass.renderPassEvent = settings.renderPassEvent;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        // Only add the pass if the camera should see the fog
        if (renderingData.cameraData.camera.GetComponent<SeeFog>() != null)
        {
            // Use compatibility mode for older URP versions
            #pragma warning disable CS0618 // Disable obsolete warning for compatibility
            m_ScriptablePass.SetSource(renderer.cameraColorTargetHandle);
            #pragma warning restore CS0618
            renderer.EnqueuePass(m_ScriptablePass);
        }
    }

    protected override void Dispose(bool disposing)
    {
        m_ScriptablePass?.Dispose();
    }
}
