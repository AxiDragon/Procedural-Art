using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CustomRenderPassFeature : ScriptableRendererFeature
{
    class CustomRenderPass : ScriptableRenderPass
    {
        public Material mat;
        RenderTargetIdentifier colorBuffer, tempBuffer;

        // This method is called before executing the render pass.
        // It can be used to configure render targets and their clear state. Also to create temporary render target textures.
        // When empty this render pass will render to the active camera render target.
        // You should never call CommandBuffer.SetRenderTarget. Instead call <c>ConfigureTarget</c> and <c>ConfigureClear</c>.
        // The render pipeline will ensure target setup and clearing happens in a performant manner.
        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
			// Grab the color buffer from the renderer camera color target.
			colorBuffer = renderingData.cameraData.renderer.cameraColorTarget;
		}

        // Here you can implement the rendering logic.
        // Use <c>ScriptableRenderContext</c> to issue drawing commands or execute command buffers
        // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
        // You don't have to call ScriptableRenderContext.submit, the render pipeline will call it at specific points in the pipeline.
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
			// Grab a command buffer. We put the actual execution of the pass inside of a profiling scope.
			CommandBuffer cmd = CommandBufferPool.Get();
			
			// Blit from the color buffer to a temporary buffer and back. This is needed for a two-pass shader.
			Blit(cmd, colorBuffer, colorBuffer, mat, 0); // shader pass 0

			// Execute the command buffer and release it.
			context.ExecuteCommandBuffer(cmd);
			CommandBufferPool.Release(cmd);
		}

        // Cleanup any allocated resources that were created during the execution of this render pass.
        public override void OnCameraCleanup(CommandBuffer cmd)
        {
        }
    }

    CustomRenderPass m_ScriptablePass;
	private Material mat;

	/// <inheritdoc/>
	public override void Create()
    {
        m_ScriptablePass = new CustomRenderPass();

        // Configures where the render pass should be injected.
        m_ScriptablePass.renderPassEvent = RenderPassEvent.AfterRenderingOpaques;

        m_ScriptablePass.mat = Resources.Load<Material>("RaymarchMaterial");// new Material(Shader.Find("Custom/Raymarch2"));
        // m_ScriptablePass.mat.SetTexture("_MainTex", Resources.Load<Texture>("RaymarchTexture"));
	}

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(m_ScriptablePass);
    }
}


