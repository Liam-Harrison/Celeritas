using Celeritas.Game.Controllers;
using Celeritas.Game.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LowHealthVignette : MonoBehaviour
{
	[SerializeField]
	private Volume vignetteVolume;
	private ShipEntity playerShip;
	//Update() because Awake() was giving nullrefs :(
	private void Update()
	{
		// if just starting, link stationary stat bars to PlayerShip
		if (playerShip == null && PlayerController.Instance != null)
		{
			playerShip = PlayerController.Instance.PlayerShipEntity;
			onHealthChange(1f);
		}

	}

	private float intensity;
	private float maxHP;
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

	private void modifyVignetteIntensity()
	{
		vignetteVolume.weight = intensity;
	}


}
