using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterWaveEffect : PostEffectBase
{
	public float distanceFactor = 60.0f;
	public float timeFactor = -30.0f;
	public float totalFactor = 1.0f;

	public float waveWidth = 0.3f;
	public float waveSpeed = 0.3f;

	private float waveStartTime;
	private Vector4 startPos = new Vector4(0.5f, 0.5f, 0, 0);

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (MyMaterial)
		{
			float curWaveDistance = (Time.time - waveStartTime) * waveSpeed;
			MyMaterial.SetFloat("_distanceFactor", distanceFactor);
			MyMaterial.SetFloat("_timeFactor", timeFactor);
			MyMaterial.SetFloat("_totalFactor", totalFactor);
			MyMaterial.SetFloat("_waveWidth", waveWidth);
			MyMaterial.SetFloat("_curWaveDis", curWaveDistance);
			MyMaterial.SetVector("_startPos", startPos);

			Graphics.Blit(source, destination, MyMaterial);
		}
		else
		{
			Graphics.Blit(source, destination);
		}
	}
	/*
	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Vector2 mousePos = Input.mousePosition;
			//将mousePos转化为（0，1）区间  
			startPos = new Vector4(mousePos.x / Screen.width, mousePos.y / Screen.height, 0, 0);
			waveStartTime = Time.time;
			//print(111);
		}
	}
	*/
	private void OnEnable()
	{
		Vector2 mousePos = Input.mousePosition;
		//将mousePos转化为（0，1）区间
		startPos = new Vector4(mousePos.x / Screen.width, mousePos.y / Screen.height, 0, 0);
		waveStartTime = Time.time;
		totalTime = 3f;
	}
}
