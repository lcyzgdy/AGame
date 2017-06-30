using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRotate : MonoBehaviour
{
	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.name == "CharacterRobotBoy")
		{
			//print(1);
			var point = collision.gameObject.transform.position;
			var axis = new Vector3(0f, 0f, 0.1f);
			GameObject.Find("Map").GetComponent<Rigidbody>().angularVelocity = axis;
		}
	}
}
