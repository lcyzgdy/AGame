using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

#pragma warning disable CS0618 // 类型或成员已过时
public class UnityChanAvatarManager : BasicPlayableBehaviour
#pragma warning restore CS0618 // 类型或成员已过时
{
	[SerializeField] private Avatar avatar;

	public override void OnBehaviourPlay(Playable playable, FrameData info)
	{
		base.OnBehaviourPlay(playable, info);
		GameObject.Find("unitychan").GetComponent<Animator>().avatar = avatar;
	}
}
