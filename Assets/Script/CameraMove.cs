using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
	private Rigidbody rigidBody;
	[SerializeField] private GameObject targetObject;
	[SerializeField] private bool myDebug;

	// Use this for initialization
	void Start()
	{
		rigidBody = GetComponent<Rigidbody>();
		//targetObject = GameObject.Find("CharacterRobotBoy");
	}

	// Update is called once per frame

	void Update()
	{
		if (myDebug)
		{
			Move();
		}
		else
		{
			var newVelocity = targetObject.transform.position - transform.position;
			newVelocity.z = 0;
			newVelocity.y += GetComponent<Camera>().orthographicSize / 3f;
			rigidBody.velocity = newVelocity * 2;
		}
	}

	private void Move()
	{
		print(1);
		if (Input.GetKey(KeyCode.RightArrow))
		{
			transform.position = new Vector3(transform.position.x + 0.1f, transform.position.y, transform.position.z);
		}
		if (Input.GetKey(KeyCode.UpArrow))
		{
			transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
		}
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			transform.position = new Vector3(transform.position.x - 0.1f, transform.position.y, transform.position.z);
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			transform.position = new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z);
		}
	}
}
