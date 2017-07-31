using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayerControl : MonoBehaviour
{
	[SerializeField] private float maxSpeed;
	[SerializeField] private float maxVSpeed;
	[SerializeField] LayerMask whatIsGround;
	private new Rigidbody2D rigidbody;
	private Animator anim;
	private BoxCollider2D box;

	// Use this for initialization
	void Start()
	{
		rigidbody = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		box = GetComponent<BoxCollider2D>();
	}

	// Update is called once per frame
	void Update()
	{
		bool grounded = IsOnGround();
		if (Input.GetKeyDown(KeyCode.Space) && grounded)
		{
			rigidbody.velocity = new Vector2(rigidbody.velocity.x, maxVSpeed);
		}

		//float speed = 0;
		float speed = rigidbody.velocity.x;
		if (Input.GetKey(KeyCode.A))
		{
			speed = -maxSpeed;
			transform.localScale = new Vector3(-0.1f, transform.localScale.y, transform.localScale.z);
		}
		else if (Input.GetKey(KeyCode.D))
		{
			speed = maxSpeed;
			transform.localScale = new Vector3(0.1f, transform.localScale.y, transform.localScale.z);
		}
		else
		{
			if (!grounded)
			{
				speed = Mathf.Lerp(speed, 0f, 0.04f);
			}
			else speed = 0;
		}

		rigidbody.velocity = new Vector2(speed, rigidbody.velocity.y);
		anim.SetFloat("Speed", Mathf.Abs(rigidbody.velocity.x));
		anim.SetFloat("vSpeed", rigidbody.velocity.y);
		anim.SetBool("Grounded", grounded);
	}

	private bool IsOnGround()
	{
		var footPosition1 = box.transform.TransformPoint(box.offset + new Vector2(box.size.x, -box.size.y) * 0.5f + Vector2.up);
		var footPosition2 = box.transform.TransformPoint(box.offset + new Vector2(-box.size.x, -box.size.y) * 0.5f + Vector2.up);
		var ray1 = Physics2D.Raycast(footPosition1, Vector2.down, 0.2f, whatIsGround);
		var ray2 = Physics2D.Raycast(footPosition2, Vector2.down, 0.2f, whatIsGround);
		Debug.DrawLine(footPosition1, ray1.point, Color.red);
		Debug.DrawLine(footPosition2, ray2.point, Color.red);

		if (ray1.collider != null || ray2.collider != null)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
