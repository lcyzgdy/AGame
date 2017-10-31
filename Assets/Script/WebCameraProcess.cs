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
	private ComputeBuffer connectGraphBuffer;
	private ComputeBuffer degubBuffer;
	private int[,] texBuffer;

	private Matrix4x4 kernel;

	struct IntVector4
	{
		public int x, y, z, w;
	}

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

		kernel = new Matrix4x4(new Vector4(1, 1, 1, 0),
							   new Vector4(1, 1, 1, 0),
							   new Vector4(1, 1, 1, 0),
							   new Vector4());

		texBuffer = new int[128, 128];
		connectGraphBuffer = new ComputeBuffer(texBuffer.Length, sizeof(int));
		print(connectGraphBuffer.count);
		int[] debugBufferF = new int[16384];
		degubBuffer = new ComputeBuffer(debugBufferF.Length, sizeof(int));

		InvokeRepeating("CameraUpdate", 0, 1 / webCameraTexture.requestedFPS);
	}

	private void CameraUpdate()
	{
		if (!webCameraTexture.isPlaying) return;

		//materials[0].SetFloat("_GrayThreshold", grayThreshold);
		//materials[0].SetTexture("_SkinTex", skinTexture);
		var rt1 = RenderTexture.GetTemporary(256, 256, 0);
		var rt2 = RenderTexture.GetTemporary(256, 256, 0);
		var binTex = new RenderTexture(128, 128, 0, RenderTextureFormat.ARGBInt);
		var edgeTex = new RenderTexture(128, 128, 0, RenderTextureFormat.ARGB32);
		Graphics.Blit(webCameraTexture, nativeCameraTexture);
		Graphics.Blit(webCameraTexture, rt1, materials[0]);

		binTex.enableRandomWrite = true;
		binTex.Create();
		edgeTex.enableRandomWrite = true;
		edgeTex.Create();

		int tex2BufferHandle = compute[0].FindKernel("Tex2Buffer");
		//int erodeHandle = compute[0].FindKernel("Erode");
		//int dilationHandle = compute[0].FindKernel("Dilation");
		int buffer2TexHandle = compute[0].FindKernel("Buffer2Tex");
		int thinHandle = compute[0].FindKernel("Thin");
		int connectHandle = compute[0].FindKernel("Connect");

		//compute[0].SetTexture(tex2BufferHandle, "Image", temp);
		//compute[0].SetBuffer(tex2BufferHandle, "TexBuffer", computeBuffer);
		//compute[0].SetTexture(buffer2TexHandle, "Tex", rt3);
		//compute[0].SetBuffer(buffer2TexHandle, "TexBuffer", computeBuffer);
		//compute[0].SetBuffer(erodeHandle, "TexBuffer", computeBuffer);
		//compute[0].SetBuffer(dilationHandle, "TexBuffer", computeBuffer);
		//compute[0].SetBuffer(thinHandle, "TexBuffer", computeBuffer);
		//
		//compute[0].Dispatch(tex2BufferHandle, 256 / 32, 256 / 32, 1);
		//compute[0].Dispatch(dilationHandle, 256 / 32, 256 / 32, 1);
		//compute[0].Dispatch(erodeHandle, 256 / 32, 256 / 32, 1);
		//compute[0].Dispatch(dilationHandle, 256 / 32, 256 / 32, 1);
		//compute[0].Dispatch(dilationHandle, 256 / 32, 256 / 32, 1);
		//compute[0].Dispatch(erodeHandle, 256 / 32, 256 / 32, 1);
		//compute[0].Dispatch(erodeHandle, 256 / 32, 256 / 32, 1);
		//compute[0].Dispatch(thinHandle, 256 / 32, 256 / 32, 1);
		//compute[0].Dispatch(buffer2TexHandle, 256 / 32, 256 / 32, 1);


		// 2 膨胀  3 腐蚀
		//materials[2].SetMatrix("_Kernel", kernel);
		//materials[3].SetMatrix("_Kernel", kernel);
		materials[2].SetMatrix("_Kernel", kernel);
		materials[3].SetMatrix("_Kernel", kernel);
		Graphics.Blit(rt1, rt2, materials[2]);
		Graphics.Blit(rt2, rt1, materials[3]);
		Graphics.Blit(rt1, rt2, materials[2]);
		Graphics.Blit(rt2, rt1, materials[3]);
		Graphics.Blit(rt1, rt2, materials[3]);
		Graphics.Blit(rt2, binTex, materials[2]);
		//Graphics.Blit(rt1, rt2, materials[2]);
		//Graphics.Blit(rt2, rt1, materials[3]);
		//Graphics.Blit(rt1, renderTexture)
		//int[] o = new int[65536];
		//for (int i = 0; i < 5; i++)
		{
			//Graphics.Blit(binTex, edgeTex, materials[1]);
			//// rt2: Egde graphic
			////Graphics.Blit(edgeTex, renderTexture);
			//
			//compute[0].SetTexture(connectHandle, "Edge", edgeTex);
			//compute[0].SetTexture(connectHandle, "Tex", binTex);
			//compute[0].SetBuffer(connectHandle, "ConnectGraphic", connectGraphBuffer);
			//compute[0].Dispatch(connectHandle, 128 / 32, 128 / 32, 1);
			//
			//compute[0].SetTexture(thinHandle, "Tex", binTex);
			//compute[0].SetTexture(thinHandle, "Edge", edgeTex);
			//compute[0].SetBuffer(thinHandle, "ConnectGraphic", connectGraphBuffer);
			//compute[0].Dispatch(thinHandle, 128 / 32, 128 / 32, 1);
		}

		int[,] buffer = new int[128, 128];
		using (var buffer2 = new ComputeBuffer(buffer.Length, sizeof(int)))
		{
			compute[0].SetTexture(tex2BufferHandle, "Image", binTex);
			compute[0].SetBuffer(tex2BufferHandle, "TexBuffer", buffer2);
			compute[0].Dispatch(tex2BufferHandle, 128 / 32, 128 / 32, 1);
			buffer2.GetData(buffer);
			int x = 0;
			int y = 0;
			for (int i = 0; i < 128; i++)
			{
				for (int j = 0; j < 128; j++)
				{
					x += buffer[i, j];
					y += buffer[i, j];
				}
			}
			print(new Vector2(x, y));
		}

		Graphics.Blit(binTex, renderTexture);

		//computeBuffer.GetData(texBuffer);
		//print(texBuffer[0, 0]);

		rt1.Release();
		rt2.Release();
		binTex.Release();
		edgeTex.Release();
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

	private void OnDestroy()
	{
		connectGraphBuffer.Release();
		degubBuffer.Release();
	}
}