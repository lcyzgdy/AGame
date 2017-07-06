using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbWall : MonoBehaviour
{
	private new Rigidbody2D rigidbody;
	[SerializeField] private GameObject[] wheel;
	private IsOnGround[] isWheelOnGround;
	private bool onGround;
	private Vector2 groundDirection;

	public bool OnGround
	{
		get
		{
			return onGround;
		}
	}

	void Start()
	{
		rigidbody = GetComponent<Rigidbody2D>();
		isWheelOnGround = new IsOnGround[wheel.Length];
		for (int i = 0; i < wheel.Length; i++)
		{
			isWheelOnGround[i] = wheel[i].GetComponent<IsOnGround>();
		}
		GC.Collect();
	}

	void Update()
	{
		onGround = true;
		groundDirection = Vector2.zero;
		foreach (var item in isWheelOnGround)
		{
			onGround &= item.IsGround;
			if (item.IsGround)
			{
				var pos = item.ContactPoint - rigidbody.position;
				groundDirection += pos;
			}
		}
		if (onGround)
		{
			Debug.DrawRay(rigidbody.position, groundDirection, Color.red);
			if (Mathf.Abs(rigidbody.velocity.magnitude) > 10f)
				rigidbody.AddForce(groundDirection.normalized * 500);
		}
	}
}
