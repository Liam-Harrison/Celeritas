using Celeritas.Game.Controllers;
using Celeritas.Game.Entities;
using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Celeritas.Game
{
	public class LowHealthVignette : MonoBehaviour
	{
		private new AudioSource audio;

		[SerializeField]
		private Volume vignetteVolume;

		private ColorAdjustments pulseIntensity;

		private double temp;
		private float pulse;

		[SerializeField]
		private float vignetteThreshold;

		[SerializeField]
		private AudioClip sfx;

		private void Awake()
		{
			audio = GetComponent<AudioSource>();
			vignetteVolume.profile.TryGet(out pulseIntensity);
		}

		private void Update()
		{
			if (PlayerController.Instance?.PlayerShipEntity == null)
				return;

			temp = Math.Abs(Math.Sin(Time.time));
			pulse = (float)temp;
			pulseIntensity.postExposure.value = pulse;

			ModifyVignetteIntensity();
		}

		// Currently a simple 1:1 HP-goes-lower-intensity-goes-higher calculation.
		// This will almost certainally change, which is why it's a seperate function.

		private float CalculateIntensity()
		{
			var p = 1 - Mathf.Clamp01(PlayerController.Instance.PlayerShipEntity.Health.CurrentValue / PlayerController.Instance.PlayerShipEntity.Health.MaxValue);
			return p;
		}

		private void ModifyVignetteIntensity()
		{
			var p = PlayerController.Instance.PlayerShipEntity.Health.CurrentValue / PlayerController.Instance.PlayerShipEntity.Health.MaxValue;
			if (p < vignetteThreshold)
			{
				vignetteVolume.weight = CalculateIntensity();
				if (sfx != null)
				{
					audio.PlayOneShot(sfx);
				}
			}
			else
			{
				vignetteVolume.weight = 0f;  //in case you come into the scene with something weird, turns the vignette off again
			}
		}
	}
}
