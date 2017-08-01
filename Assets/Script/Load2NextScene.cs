using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Load2NextScene : MonoBehaviour
{
	public async void Jump2NextLevel()
	{
		GetComponent<Button>().enabled = false;
		await Task.Run(() =>
		{

		});
		SceneManager.LoadScene("Level4");
	}
}
