using Celeritas.Game.Entities;
using Celeritas.Game.Interfaces;
using Celeritas.UI;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Stat bar that can be moved around
/// Used for 'floating' stat bars that follow ships around
/// 
/// Logic for changing the bar's fill color (to make it easier as its a prefab)
/// and updating the bar to follow a target ship is contained here.
/// </summary>
public class MovingStatBar : StatBar, IPooledObject
{
	private ShipEntity ship; // the ship this bar's position is locked onto.

	public ShipEntity Ship { get => ship; set => ship = value; }

	[SerializeField]
	private Color barFillColor = Color.red;

	[SerializeField]
	private Vector3 displacement; // from centre of ship (eg move bar up or down)

	public void OnDespawned(){ }

	public void OnSpawned(){ }

	/// <summary>
	/// Setup the MovingStatBar
	/// </summary>
	/// <param name="shipToFollow">The ship the stat bar will follow</param>
	/// <param name="toTrack">The stats the bar will display</param>
	public void Initalize(ShipEntity shipToFollow, EntityStatBar toTrack)
	{
		ship = shipToFollow;
		slider = GetComponent<Slider>();
		EntityStats = toTrack;

		// update fill bar colour.
		GameObject fillBar = transform.Find("BarFill").gameObject;
		Image image = fillBar.GetComponent<Image>();
		image.color = barFillColor;
	}

	/// <summary>
	/// Update the location of the floating bar, so it will follow its ship.
	/// Also factors in displacement, so it ca
	/// </summary>
	public void UpdateLocation()
    {
		if (ship != null) // to avoid errors when ship is destroyed
		{
			transform.position = Camera.main.WorldToScreenPoint(ship.transform.position + displacement);
		}
	}
}
