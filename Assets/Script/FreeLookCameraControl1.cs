using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

#pragma warning disable CS0618 // 类型或成员已过时
[ExecuteInEditMode]
public class FreeLookCameraControl1 : BasicPlayableBehaviour
{
	[SerializeField] private AnimationCurve curve;
	private GameObject freeLookCameraGameObject;
	private float curTime;
	private float maxTime;
	private Cinemachine.CinemachineFreeLook freeLookCamera;
	[SerializeField] private string cameraName;

	public override void OnGraphStop(Playable playable)
	{
		Time.timeScale = 1;
	}

	public override void OnBehaviourPlay(Playable playable, FrameData info)
	{
		maxTime = (float)PlayableExtensions.GetDuration(playable);
		freeLookCameraGameObject = GameObject.Find(cameraName);
		freeLookCamera = freeLookCameraGameObject.GetComponent<Cinemachine.CinemachineFreeLook>();
	}

	public override void PrepareFrame(Playable playable, FrameData info)
	{
		curTime += info.deltaTime;
		freeLookCamera.m_HeadingBias = curve.Evaluate(curTime / maxTime) * 180f;
	}
}


#pragma warning restore CS0618 // 类型或成员已过时
