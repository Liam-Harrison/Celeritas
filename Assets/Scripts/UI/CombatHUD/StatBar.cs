using Celeritas.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Used to control the logic of bar-based HUD elements
/// eg health, shield
/// </summary>
public class StatBar : MonoBehaviour
{
	[SerializeField]
	protected Slider slider;

	private EntityStatBar entityStats; // the entity's stat information

	/// <summary>
	/// The health that this HealthBar is displaying
	/// </summary>
	public EntityStatBar EntityStats { get => entityStats; set => entityStats = value; }

	private void Start()
	{

	}

	// Update is called once per frame
	void Update()
    {
		if (entityStats != null) { 
			slider.maxValue = entityStats.MaxValue;
			slider.value = entityStats.CurrentValue;
		}
	}

}
