using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassthoughEffect : PostEffectBase
{
	[Range(0.0f, 0.15f)]
	public float distortFactor = 1.0f;
	public Vector2 distortCenter;
	public Texture noiseTexture;
	[Range(0.0f, 2.0f)]
	public float distortStrength = 1.0f;
	// 屏幕收缩总时间
	public float passThoughTime = 4.0f;
	// 当前时间
	private float currentTime = 0.0f;
	// 曲线控制权重
	public float scaleCurveFactor = 0.2f;
	// 曲线控制效果
	public AnimationCurve scaleCurve;
	// 噪声曲线系数
	public float distortCurveFactor = 1.0f;
	// 曲线控制扰动效果
	public AnimationCurve distortCurve;

	private void OnEnable()
	{
		distortCenter = new Vector2(0.5f, 0.5f);
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (MyMaterial)
		{
			MyMaterial.SetFloat("_DistortFactor", distortFactor);
			MyMaterial.SetVector("_DistortCenter", distortCenter);
			MyMaterial.SetTexture("_NoiseTex", noiseTexture);
			MyMaterial.SetFloat("_DistortStrength", distortStrength);
			Graphics.Blit(source, destination, MyMaterial);
		}
		else
		{
			Graphics.Blit(source, destination);
		}
	}

	[ContextMenu("Play")]
	public void StartEffect()
	{
		currentTime = 0f;
		StartCoroutine(UpdateEffect());
	}

	private IEnumerator UpdateEffect()
	{
		while (currentTime < passThoughTime)
		{
			currentTime += Time.deltaTime;
			float t = currentTime / passThoughTime;
			//根据时间占比在曲线（0，1）区间采样，再乘以权重作为收缩系数
			distortFactor = scaleCurve.Evaluate(t) * scaleCurveFactor;
			distortStrength = distortCurve.Evaluate(t) * distortCurveFactor;
			yield return null;
			distortFactor = 0.0f;
			distortStrength = 0.0f;
		}
	}
}
