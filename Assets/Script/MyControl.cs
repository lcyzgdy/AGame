using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyControl : MonoBehaviour
{
	private new Rigidbody2D rigidbody;
	private bool grounded;
	private bool canJump;
	private bool canJumpFlag;
	private Animator anim;
	[SerializeField] private float runForce;
	[SerializeField] private float jumpForce;
	[SerializeField] private float maxSpeed;
	[SerializeField] private LayerMask whatIsGround;
	private Vector2 normalVector;
	private Vector3 beforePosition;
	private CapsuleCollider2D capsule;
	private BoxCollider2D box;

	// Use this for initialization
	void Start()
	{
		rigidbody = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		capsule = GetComponent<CapsuleCollider2D>();
		box = GetComponent<BoxCollider2D>();
		anim.SetBool("Ground", true);
		canJump = true;
		canJumpFlag = false;
	}

	// Update is called once per frame
	void Update()
	{
		anim.SetBool("Ground", grounded);
		anim.SetFloat("vSpeed", rigidbody.velocity.y);
		anim.SetFloat("Speed", Mathf.Abs(rigidbody.velocity.x));

		var footPosition1 = box.transform.TransformPoint(box.offset + new Vector2(box.size.x, -box.size.y) * 0.5f);
		var footPosition2 = box.transform.TransformPoint(box.offset + new Vector2(-box.size.x, -box.size.y) * 0.5f);
		var ray1 = Physics2D.Raycast(footPosition1, Vector2.down, 0.1f, whatIsGround);
		var ray2 = Physics2D.Raycast(footPosition2, Vector2.down, 0.1f, whatIsGround);
		if (ray1.collider != null || ray2.collider != null)
		{
			if (ray1.collider != null && ray2.collider != null)
			{
				var d = ray1.distance - ray2.distance;
				var x = box.size.x;
				if (Mathf.Abs(d) > 0.078f) normalVector = new Vector2(d, x);
			}
			grounded = true;
		}
		else
		{
			grounded = false;
			normalVector = Vector2.up;
		}
		//Debug.DrawLine(footPosition1, new Vector3(footPosition1.x, footPosition1.y - 0.12f, 0f), Color.red);
		//Debug.DrawLine(footPosition2, new Vector3(footPosition2.x, footPosition2.y - 0.12f, 0f), Color.red);
		Debug.DrawLine(footPosition1, ray1.point, Color.red);
		Debug.DrawLine(footPosition2, ray2.point, Color.red);
		Debug.DrawRay(transform.position, normalVector, Color.magenta);

		if (Mathf.Abs(rigidbody.velocity.x) >= 1e-2)
		{
			if (rigidbody.velocity.x < 0) GetComponent<SpriteRenderer>().flipX = true;
			else if (rigidbody.velocity.x > 0) GetComponent<SpriteRenderer>().flipX = false;
		}

		var isStand = Vector2.Angle(Vector2.up, normalVector);
		if (!grounded)
		{
			transform.rotation = Quaternion.Euler(0f, 0f, 0f);
		}
		else if (isStand < 55f)
		{
			transform.rotation = Quaternion.Euler(0f, 0f, isStand * (normalVector.x > 0 ? -1 : 1));
		}
		//print(climbed);

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

		if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.Space))
		{
			if (grounded)
			{
				Invoke("CannotJump", 0.4f);
				canJumpFlag = true;
			}
		}
		if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Space))
		{
			if (canJumpFlag)
			{
				rigidbody.velocity = new Vector2(rigidbody.velocity.x, maxSpeed * 1.25f);
				grounded = false;
				//canJumpFlag = false;
			}
		}
		if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.Space))
		{
			if (grounded)
			{
				canJumpFlag = true;
			}
			else canJumpFlag = false;
		}
	}

	void CannotJump()
	{
		canJumpFlag = false;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		bool flag = true;
		foreach (var item in collision.contacts)
		{
			if (flag)
			{
				flag = false;
				normalVector = item.normal;
			}
			else
			{
				if (Vector2.Angle(Vector2.up, item.normal) < Vector2.Angle(Vector2.up, normalVector))
				{
					normalVector = item.normal;
				}
			}

			if (item.normal.y <= -0.45f)
			{
				canJumpFlag = false;
			}
		}
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		bool flag = true;
		foreach (var item in collision.contacts)
		{
			if (flag)
			{
				flag = false;
				normalVector = item.normal;
			}
			else
			{
				if (Vector2.Angle(Vector2.up, item.normal) < Vector2.Angle(Vector2.up, normalVector))
				{
					normalVector = item.normal;
				}
			}

			if (item.normal.y <= -0.45f)
			{
				canJumpFlag = false;
			}
		}
		//if (Vector2.Angle(Vector2.up, normalVector) > 55f) normalVector = Vector2.up;
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
