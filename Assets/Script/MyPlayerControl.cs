using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayerControl : MonoBehaviour
{
	new Rigidbody2D rigidbody;
	Animator anim;
	// Use this for initialization
	void Start()
	{
		rigidbody = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
		float speed = 0;
		if (Input.GetKey(KeyCode.A))
		{
			speed = -1.2f;
			
		}
		else if (Input.GetKey(KeyCode.D))
		{
			speed = 1.2f;
		}

		rigidbody.velocity = new Vector2(speed, rigidbody.velocity.y);
		anim.SetFloat("Speed", rigidbody.velocity.x);
	}
}
