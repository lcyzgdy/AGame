using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
	private Rigidbody rigidBody;
	[SerializeField]
	private GameObject targetObject;
	// Use this for initialization
	void Start()
	{
		rigidBody = GetComponent<Rigidbody>();
		//targetObject = GameObject.Find("CharacterRobotBoy");
	}

	// Update is called once per frame

	void Update()
	{
		var newVelocity = targetObject.transform.position - transform.position;
		newVelocity.z = 0;
		newVelocity.y += GetComponent<Camera>().orthographicSize / 3f;
		rigidBody.velocity = newVelocity * 2;
		//print(rigidBody.velocity);
	}
}
