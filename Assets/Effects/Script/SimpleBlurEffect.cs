using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SimpleBlurEffect : PostEffectBase
{
	public float blurRadius = 1.0f;
	public int downSample = 2;
	public int iteration = 3;

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (MyMaterial)
		{
			MyMaterial.SetFloat("_BlurRadius", blurRadius);
			RenderTexture rt1 = RenderTexture.GetTemporary(source.width >> downSample, source.height >> downSample, 0, source.format);
			RenderTexture rt2 = RenderTexture.GetTemporary(source.width >> downSample, source.height >> downSample, 0, source.format);
			Graphics.Blit(source, rt1);
			for (int i = 0; i < iteration; i++)
			{
				MyMaterial.SetFloat("_BulrRadius", blurRadius);
				Graphics.Blit(rt1, rt2, MyMaterial);
				Graphics.Blit(rt2, rt1, MyMaterial);
			}
			Graphics.Blit(rt1, destination);
			//Graphics.Blit(source, destination, MyMaterial);
			RenderTexture.ReleaseTemporary(rt1);
			RenderTexture.ReleaseTemporary(rt2);
		}
	}
}
