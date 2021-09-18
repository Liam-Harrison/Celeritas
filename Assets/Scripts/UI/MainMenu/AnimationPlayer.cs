using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeritas.UI
{
	public class AnimationPlayer : MonoBehaviour
	{
		[SerializeField]
		private float waitTime = 1;

		[SerializeField]
		private new Animation animation;

		public void Play()
		{
			if (Time.time > waitTime)
				animation.Play();
		}
	}
}