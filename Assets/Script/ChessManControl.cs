using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessManControl: MonoBehaviour
{
	private float deltaX, deltaY;
	private bool seleteOrMove; // true: Selete; false: Move
	private Vector3 up, down, left, right;
	private MeshRenderer sparkControl;

	public bool SeleteOrMove
	{
		get
		{
			return seleteOrMove;
		}
	}

	private void Start()
	{
		seleteOrMove = false;
		deltaX = 1.2f;
		deltaY = 1.1f;
		up = new Vector3(0f, 0f, deltaY);
		right = new Vector3(deltaX, 0f, 0f);
		down = new Vector3(0f, 0f, -deltaY);
		left = new Vector3(-deltaY, 0f, 0f);
		sparkControl = GetComponent<MeshRenderer>();
	}

	public virtual void OnSelete()
	{
		InvokeRepeating("Sparking", 0f, 0.2f);
	}

	public virtual void OnMove()
	{
		if (IsInvoking("Sparking"))
		{
			sparkControl.enabled = true;
			CancelInvoke("Sparking");
		}
	}

	public virtual void OnUnselete()
	{
		if (IsInvoking("Sparking"))
		{
			sparkControl.enabled = true;
			CancelInvoke("Sparking");
		}
	}

	private void Sparking()
	{
		sparkControl.enabled = (!sparkControl.enabled);
	}
}
