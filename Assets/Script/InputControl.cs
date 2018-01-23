using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Vuforia;

public class InputControl : MonoBehaviour
{
	[SerializeField] private LayerMask chessMan;
	private bool seleteControl;
	private ChessManControl seletedObject;

	void Start()
	{
		//var vuforia = VuforiaARController.Instance;
		//vuforia.RegisterVuforiaStartedCallback(OnVuforiaStarted);
		//vuforia.RegisterOnPauseCallback(OnPaused);
		seleteControl = false;
		seletedObject = null;
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
		//CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
	}

	private void OnPaused(bool paused)
	{
		if (!paused) // resumed
		{
			// Set again autofocus mode when app is resumed
			//CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
		}
	}

	private void Click(Ray ray)
	{
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, chessMan))
		{
			var seletedObj = hitInfo.collider.gameObject.GetComponent<ChessManControl>();
			if (seletedObject == null)
			{
				print(1);
				seletedObj.OnSelete();
				seletedObject = seletedObj;
			}
			else
			{
				if (seletedObject == seletedObj)
				{
					print(2);
					seletedObject.OnUnselete();
					seletedObject = null;
				}
				else
				{
					print(3);
					seletedObject.OnUnselete();
					seletedObject = seletedObj;
					seletedObj.OnSelete();
				}
			}
		}
		else
		{
			seletedObject.OnMove();
			seletedObject = null;
		}
	}
}
