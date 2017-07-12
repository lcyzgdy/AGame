using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class InputControl : MonoBehaviour
{
	[SerializeField] private LayerMask chessMan;
	private bool seleteControl;
	private GameObject seleteObject;

	void Start()
	{
		var vuforia = VuforiaARController.Instance;
		vuforia.RegisterVuforiaStartedCallback(OnVuforiaStarted);
		vuforia.RegisterOnPauseCallback(OnPaused);
		seleteControl = false;
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.touchCount == 1)
		{
			var touchEvent = Input.GetTouch(0);
			if (touchEvent.phase == TouchPhase.Began)
			{
				var ray = Camera.main.ScreenPointToRay(touchEvent.position);
				Click(ray);
			}
		}
		else if (Input.GetMouseButtonDown(0))
		{
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			Click(ray);
		}
	}
	private void OnVuforiaStarted()
	{
		CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
	}

	private void OnPaused(bool paused)
	{
		if (!paused) // resumed
		{
			// Set again autofocus mode when app is resumed
			CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
		}
	}

	private void Click(Ray ray)
	{
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, chessMan))
		{
			//Destroy(hitInfo.collider.gameObject);
			var seletedObject = hitInfo.collider.gameObject.GetComponent<ChessManControl>();
			if (!seleteControl)
			{
				seletedObject.OnSelete();
				seleteControl = true;
			}
			else
			{
				seletedObject.OnMove();
				seleteControl = false;
			}
		}
	}
}
