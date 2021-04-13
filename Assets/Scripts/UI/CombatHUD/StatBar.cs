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
	public Slider slider;

	public GameObject statBar; // the whole healthbar object

	private EntityStatBar entityStats; // the entity's stat information

	//private Vector3 position;

	/// <summary>
	/// The health that this HealthBar is displaying
	/// </summary>
	public EntityStatBar EntityStats { get => entityStats; set => entityStats = value; }

	//public Vector3 Position { get => position; set => position = value; }

    // Update is called once per frame
    void Update()
    {
		if (entityStats != null) { 
			slider.maxValue = entityStats.MaxValue;
			slider.value = entityStats.CurrentValue;
		}

		//statBar.transform.position = position;
	}

}
