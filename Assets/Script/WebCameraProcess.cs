using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WebCameraProcess : MonoBehaviour
{
	private WebCamTexture webCameraTexture;
	[SerializeField] private RenderTexture renderTexture;
	[SerializeField] private RenderTexture nativeCameraTexture;

	private int width = 1280;
	private int height = 720;
	private int cameraFps = 60;
	[Range(0, 16)]
	[SerializeField]
	private int downSample = 0;

	private Material[] materials;
	[SerializeField] private Shader[] shader;

	[SerializeField] private ComputeShader[] compute;

	private Matrix4x4 kernel;

	private void Start()
	{
		materials = (from i in shader
					 where i != null
					 select GenerateMaterial(i)).ToArray();

		Application.RequestUserAuthorization(UserAuthorization.WebCam);
		if (!Application.HasUserAuthorization(UserAuthorization.WebCam)) return;
		WebCamDevice[] devices = WebCamTexture.devices;
		webCameraTexture = new WebCamTexture(devices[0].name, width, height, cameraFps);
		webCameraTexture.Play();

		kernel = new Matrix4x4(new Vector4(1, 1, 1),
							   new Vector4(1, 1, 1),
							   new Vector4(1, 1, 1),
							   new Vector4());

		//float s = kernel.m00 * kernel.m00 + kernel.m01 * kernel.m01 + kernel.m02 * kernel.m02 + kernel.m03 * kernel.m03
		//		+ kernel.m10 * kernel.m10 + kernel.m11 * kernel.m11 + kernel.m12 * kernel.m12 + kernel.m13 * kernel.m13
		//		+ kernel.m20 * kernel.m20 + kernel.m21 * kernel.m21 + kernel.m22 * kernel.m22 + kernel.m23 * kernel.m23
		//		+ kernel.m30 * kernel.m30 + kernel.m31 * kernel.m31 + kernel.m32 * kernel.m32 + kernel.m33 * kernel.m33;
		//for (int i = 0; i < 16; i++)
		//{
		//	kernel[i] /= s;
		//}

		InvokeRepeating("CameraUpdate", 0, 1 / webCameraTexture.requestedFPS);
	}

	private void CameraUpdate()
	{
		if (!webCameraTexture.isPlaying) return;

		//materials[0].SetFloat("_GrayThreshold", grayThreshold);
		//materials[0].SetTexture("_SkinTex", skinTexture);
		var rt1 = RenderTexture.GetTemporary(256, 256, 0);
		var rt2 = RenderTexture.GetTemporary(256, 256, 0);
		var rt3 = new RenderTexture(256, 256, 0, RenderTextureFormat.ARGB32);
		var temp = RenderTexture.GetTemporary(256, 256, 0);
		Graphics.Blit(webCameraTexture, nativeCameraTexture);
		Graphics.Blit(webCameraTexture, temp, materials[0]);

		rt3.enableRandomWrite = true;
		rt3.Create();
		int kernelHandle = compute[0].FindKernel("CSMain");
		compute[0].SetTexture(kernelHandle, "Image", temp);
		compute[0].SetTexture(kernelHandle, "Result", rt3);
		compute[0].Dispatch(kernelHandle, 256 / 8, 256 / 8, 1);
		//compute[0].Get

		//materials[2].SetMatrix("_Kernel", kernel);
		//materials[3].SetMatrix("_Kernel", kernel);
		// Graphics.Blit(temp, rt2, materials[3]);
		// Graphics.Blit(rt2, rt1, materials[2]);
		// Graphics.Blit(rt1, rt2, materials[2]);
		// Graphics.Blit(rt2, rt1, materials[3]);
		// Graphics.Blit(rt1, rt2, materials[3]);
		// Graphics.Blit(rt2, rt1, materials[2]);
		// Graphics.Blit(rt1, rt2, materials[2]);
		// Graphics.Blit(rt1, rt2, materials[3]);
		// //Graphics.Blit(rt1, renderTexture)

		//Graphics.Blit(rt1, renderTexture, materials[1]);
		Graphics.Blit(rt3, renderTexture);

		rt1.Release();
		rt2.Release();
		rt3.Release();
		temp.Release();
	}

	private Material GenerateMaterial(Shader shader)
	{
		if (shader == null)
		{
			return null;
		}
		//需要判断shader是否支持  
		if (shader.isSupported == false)
		{
			return null;
		}

		Material material = new Material(shader)
		{
			hideFlags = HideFlags.DontSave
		};
		if (material)
		{
			return material;
		}

		return null;
	}
}