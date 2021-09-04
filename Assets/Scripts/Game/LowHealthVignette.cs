using Celeritas.Game.Controllers;
using Celeritas.Game.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.Universal;

public class LowHealthVignette : MonoBehaviour
{
	private AudioSource audio;
	[SerializeField]
	private Volume vignetteVolume;
	private ColorAdjustments pulseIntensity;

	private void Awake()
	{
		audio = GetComponent<AudioSource>();
		vignetteVolume.profile.TryGet(out pulseIntensity);
		if (pulseIntensity == null)
		{
			Debug.Log("pulsecolour null :(");
		}
	}


	private ShipEntity playerShip;
	private double temp;
	private float pulse;
	
	private void Update()
	{
		// In Update() because Awake() was giving nullrefs :(
		if (playerShip == null && PlayerController.Instance != null)
		{
			playerShip = PlayerController.Instance.PlayerShipEntity;
			onHealthChange(1f);
		}
		temp = Math.Abs(Math.Sin(Time.time));
		pulse = 1.5f + (float)temp;
		pulseIntensity.postExposure.value = pulse;
	}

	private float intensity;
	/// <summary>
	/// Modifies the low-health vignette when HP changes.
	/// </summary>
	/// <param name="hp">Any float (needed to attach to an onValueChange)</param>
	// Is sent current hp  from the HP bar. We ignore this, since we're using values
	// from the playerShip entity instead.
	// Intensity needed is calculated, then the vignette modified.
	public void onHealthChange(float hp)
	{
		intensity = calculateIntensity();
		modifyVignetteIntensity();
	}

	// Currently a simple 1:1 HP-goes-lower-intensity-goes-higher calculation.
	// This will almost certainally change, which is why it's a seperate function.
	private float calculatedValue;
	private float calculateIntensity()
	{
		calculatedValue = 1 - ((float) playerShip.Health.CurrentValue / (float) playerShip.Health.MaxValue);
		//calculatedValue = (float) PlayerController.Instance.PlayerShipEntity.Health.CurrentValue / (float) PlayerController.Instance.PlayerShipEntity.Health.MaxValue; //for testing!
		return calculatedValue;
	}

	[SerializeField]
	private AudioClip sfx;
	[SerializeField]
	private float sfxThreshold;
	private void modifyVignetteIntensity()
	{
		vignetteVolume.weight = intensity;

		if (intensity < sfxThreshold)
		{
			audio.PlayOneShot(sfx);
		}
	}


}
