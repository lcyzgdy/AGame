using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
	private new Rigidbody rigidbody;
	private Vector3 direction;
	private float startTouchTime;
	private bool jumping;

	public Vector3 Direction
	{
		get { return direction; }
		set { direction = value; }
	}
	// Use this for initialization
	void Start()
	{
		rigidbody = GetComponent<Rigidbody>();
		startTouchTime = -1;
		jumping = false;
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.touchCount > 0)
		{
			if (startTouchTime < 0)
			{
				startTouchTime = 0;
				return;
			}
			else
			{
				startTouchTime += Time.deltaTime;
			}
		}
		else
		{
			jumping = true;

		}
	}
}
