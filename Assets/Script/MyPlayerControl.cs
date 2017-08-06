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
	private bool grounded;
	private Vector2 normalVector;

	// Use this for initialization
	void Start()
	{
		rigidbody = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		box = GetComponent<BoxCollider2D>();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		bool grounded = IsOnGround();
		//bool castWall = IsCastWall();
		if (Input.GetKeyDown(KeyCode.Space) && grounded)
		{
			rigidbody.velocity = new Vector2(rigidbody.velocity.x, maxVSpeed);
		}

		//float speed = 0;
		float speed = rigidbody.velocity.x;
		if (Input.GetKey(KeyCode.A))// && (!castWall || (castWall && transform.localScale.x > 0)))
		{
			speed = -maxSpeed;
			transform.localScale = new Vector3(-0.1f, transform.localScale.y, transform.localScale.z);
		}
		else if (Input.GetKey(KeyCode.D))// && (!castWall || (castWall && transform.localScale.x < 0)))
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
		//var ray1 = Physics2D.Raycast(footPosition1, Vector2.down, 0.2f, whatIsGround);
		//var ray2 = Physics2D.Raycast(footPosition2, Vector2.down, 0.2f, whatIsGround);
		var ray1 = Physics2D.Raycast(footPosition1, -normalVector, 0.5f, whatIsGround);
		var ray2 = Physics2D.Raycast(footPosition2, -normalVector, 0.5f, whatIsGround);
		//Debug.DrawLine(footPosition1, ray1.point, Color.red);
		//Debug.DrawLine(footPosition2, ray2.point, Color.red);

		/*if (ray1.collider != null || ray2.collider != null)
		{
			return true;
		}
		else
		{
			return false;
		}*/

		if (ray1.collider != null || ray2.collider != null)
		{
			if (ray1.collider != null && ray2.collider != null)
			{
				var d = ray1.distance - ray2.distance;
				var x = Mathf.Sqrt(box.size.x * box.size.x - d * d);
				//print(Mathf.Atan(d / x) * Mathf.Rad2Deg);
				var lineVec = ray1.point - ray2.point;
				//normalVector = new Vector2(d, x).normalized;
				normalVector = new Vector2(lineVec.y * -1, lineVec.x) * 10 * transform.localScale.x;
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
		//Debug.DrawLine(footPosition1, ray1.point, Color.red);
		//Debug.DrawLine(footPosition2, ray2.point, Color.red);
		//Debug.DrawRay(transform.position, normalVector, Color.magenta);

		var isStand = Vector2.Angle(Vector2.up, normalVector);
		//print(isStand);
		if (!grounded)
		{
			transform.rotation = Quaternion.Euler(0f, 0f, 0f);
		}
		else if (isStand < 35f)
		{
			transform.rotation = Quaternion.Euler(0f, 0f, isStand * (normalVector.x > 0 ? -1 : 1));
		}
		else
		{
			transform.rotation = Quaternion.Euler(Vector3.up);
		}

		return grounded;
	}

	private bool IsCastWall()
	{
		var direction = new Vector2(normalVector.y * transform.localScale.x * 10, normalVector.x);
		var originPos1 = box.transform.TransformPoint(box.offset + new Vector2(box.size.x, 0) * 0.5f + Vector2.left);
		var originPos2 = box.transform.TransformPoint(box.offset + new Vector2(box.size.x, -box.size.y) * 0.5f + Vector2.left + Vector2.up);
		var ray1 = Physics2D.Raycast(originPos1, direction, 2f, whatIsGround);
		var ray2 = Physics2D.Raycast(originPos2, direction, 0.5f, whatIsGround);
		Debug.DrawLine(originPos1, ray1.point, Color.red);
		Debug.DrawLine(originPos2, ray2.point, Color.red);
		return ray1.collider != null && ray2.collider != null;
	}
}
