using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
	[SerializeField] private GameObject targetObject;
	[SerializeField] private bool myDebug;
	private Vector3 targetPosition;
	private float currentX;
	private float currentY;
	private float currentZ;

	private float xVelocity = 0.0F;
	private float yVelocity = 0.0F;
	private float zVelocity = 0.0f;
	private float distanceSnapTime = 0.4f;

	void Update()
	{
		if (myDebug)
		{
			Move();
		}
		else
		{
			Vector3 targetPos = targetObject.transform.position;
			targetPosition.x = targetPos.x;
			targetPosition.y = targetPos.y + Camera.main.orthographicSize / 3f;// + heightAbovePlayer;
			currentX = Mathf.SmoothDamp(currentX, targetPosition.x, ref xVelocity, distanceSnapTime);
			currentY = Mathf.SmoothDamp(currentY, targetPosition.y, ref yVelocity, distanceSnapTime);
			//currentZ = Mathf.SmoothDamp(currentZ, targetPosition.z, ref zVelocity, 0.5f);
			transform.position = new Vector3(currentX, currentY, -10);
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
