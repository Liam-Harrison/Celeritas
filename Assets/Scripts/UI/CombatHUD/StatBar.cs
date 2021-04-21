using Celeritas.UI;
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

	void Update()
    {
		if (entityStats != null) { 
			slider.maxValue = entityStats.MaxValue;
			slider.value = entityStats.CurrentValue;
		}
	}

}
