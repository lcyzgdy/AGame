using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRotate : MonoBehaviour
{
	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.P))
		{
			var point = GameObject.Find("Player").gameObject.transform.position;
			var axis = new Vector3(0f, 0f, 0.1f);
			GameObject.Find("Map").GetComponent<Rigidbody>().angularVelocity = axis;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.name == "CharacterRobotBoy")
		{
			var point = collision.gameObject.transform.position;
			var axis = new Vector3(0f, 0f, 0.1f);
			GameObject.Find("Map").GetComponent<Rigidbody>().angularVelocity = axis;
		}
	}
}
