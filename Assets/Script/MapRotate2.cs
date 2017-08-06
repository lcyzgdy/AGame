using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRotate2 : MonoBehaviour
{
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			transform.RotateAround(GameObject.Find("Player").transform.position, Vector3.back, 90f);
		}
	}
}
