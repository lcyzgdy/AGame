using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LoadNextScene : MonoBehaviour
{
	[SerializeField]
	private string nextSceneName;

	public async void Jump2NextLevel()
	{
		GetComponent<Button>().enabled = false;
		var effects = GameObject.Find("Main Camera").GetComponents<PostEffectBase>();
		foreach (var i in effects)
		{
			i.enabled = true;
		}
		float totalTime = effects[0].TotalTime;
		await Task.Delay(Convert.ToInt32(totalTime * 1000f));
		SceneManager.LoadScene(nextSceneName);
	}
}
