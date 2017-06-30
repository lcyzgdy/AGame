using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateEnd : MonoBehaviour
{
	Rigidbody rigidBody;
	Vector3 before;
	// Use this for initialization
	void Start()
	{
		rigidBody = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update()
	{
		//print(before);
		if (rigidBody.angularVelocity.sqrMagnitude == 0)
		{
			before = transform.rotation.eulerAngles;
			return;
		}
		if ((transform.rotation.eulerAngles - before).z >= 90f)
		{
			rigidBody.angularVelocity = new Vector3(0f, 0f, 0f);
			var newRotation = transform.rotation.eulerAngles;
			newRotation.z = Mathf.Round(newRotation.z);
			transform.rotation = Quaternion.Euler(newRotation);
		}
	}
}
