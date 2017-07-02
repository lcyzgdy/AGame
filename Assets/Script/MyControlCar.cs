using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyControlCar : MonoBehaviour
{
	private new Rigidbody2D rigidbody;
	private bool grounded;
	private bool canJump;
	private bool canJumpFlag;
	[SerializeField] private float runForce;
	[SerializeField] private float jumpForce;
	[SerializeField] private float maxSpeed;
	[SerializeField] private LayerMask whatIsGround;
	private Vector2 normalVector;
	private Vector3 beforePosition;
	private CapsuleCollider2D capsule;
	private BoxCollider2D box;
	private Vector3 offect;

	// Use this for initialization
	void Start()
	{
		rigidbody = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
	{
		Move();
	}

	private void Move()
	{
		if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
		{
			rigidbody.AddForce(new Vector2(runForce, 0), ForceMode2D.Force);
			if (rigidbody.velocity.x > maxSpeed) rigidbody.velocity = new Vector2(maxSpeed, rigidbody.velocity.y);
		}
		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
		{
			rigidbody.AddForce(new Vector2(-runForce, 0), ForceMode2D.Force);
			if (rigidbody.velocity.x < -maxSpeed) rigidbody.velocity = new Vector2(-maxSpeed, rigidbody.velocity.y);
		}
	}

	float Cos(Vector2 v)
	{
		return v.x / v.magnitude;
	}

	float Sin(Vector2 v)
	{
		return v.y / v.magnitude;
	}

	float Tan(Vector2 v)
	{
		return v.y / v.x;
	}
}
