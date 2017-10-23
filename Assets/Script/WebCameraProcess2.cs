using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WebCameraProcess2 : MonoBehaviour
{
	[SerializeField] private RenderTexture renderTexture;
	[SerializeField] private RenderTexture nativeCameraTexture;

	private Material[] materials;
	[SerializeField] private Shader[] shader;

	private void Start()
	{
		materials = (from i in shader
					 where i != null
					 select GenerateMaterial(i)).ToArray();
	}

	private void Update()
	{
		//var rt1 = RenderTexture.GetTemporary(256, 256, 0);
		Graphics.Blit(nativeCameraTexture, renderTexture, materials[0]);
		//rt1.Release();
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