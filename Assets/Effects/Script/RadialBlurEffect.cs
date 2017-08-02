using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialBlurEffect : PostEffectBase
{
	[Range(0, 0.05f)]
	public float blurFactor = 1.0f;
	public Vector2 blurCenter;

	private void OnEnable()
	{
		blurCenter = new Vector2(0.5f, 0.5f);
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (MyMaterial)
		{
			MyMaterial.SetFloat("_BlurFactor", blurFactor);
			MyMaterial.SetVector("_BlurCenter", blurCenter);
			Graphics.Blit(source, destination, MyMaterial);
		}
		else
		{
			Graphics.Blit(source, destination);
		}
	}
}
