using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsOnGround: MonoBehaviour
{
	bool isGround;
	Vector2 contactPoint;
	[SerializeField] LayerMask whatIsGround;
	CircleCollider2D circle;

	public bool IsGround
	{
		get
		{
			return isGround;
		}
	}

	public Vector2 ContactPoint
	{
		get
		{
			return contactPoint;
		}
	}

	private void Start()
	{
		circle = GetComponent<CircleCollider2D>();
	}

	private void Update()
	{
		var collisions = Physics2D.CircleCastAll(transform.position, circle.radius, Vector2.zero, circle.radius + 0.1f, whatIsGround);
		if (collisions.Length != 0) isGround = true;
		else isGround = false;
		for (int i = 0; i < collisions.Length; i++)
		{
			contactPoint = collisions[i].point;
		}
	}
}
