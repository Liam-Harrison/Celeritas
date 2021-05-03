using Celeritas.Game.Controllers;
using Celeritas.Game.Entities;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Draws a line between the player ship and the mouse.
/// </summary>
public class AimingLine : MonoBehaviour
{
	private LineRenderer line;

	// Displacement is to centre the line, so it points to the middle of the mouse cursor sprite.
	[SerializeField, Title("To allow line to intersect centre of cursor")]
	private float xDisplacementFromCursorCentre;

	[SerializeField]
	private float yDisplacementFromCursonCentre;

	[SerializeField]
	private bool aimAtMouse;

	private GameObject targetToAimAt;

	/// <summary>
	/// If true, aiming line's target will be the player's mouse location
	/// </summary>
	public bool AimAtMouse { get => aimAtMouse; set => aimAtMouse = value; }

	/// <summary>
	/// Line will aim at this target if AimAtMouse is false.
	/// </summary>
	public GameObject TargetToAimAt { get => targetToAimAt; set => targetToAimAt = value; }

    // Start is called before the first frame update
    void Start()
    {
		line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
		if (PlayerController.Instance!=null)
			Draw(PlayerController.Instance.PlayerShipEntity);
    }

	/// <summary>
	/// Draw line from player ship to targetToAimAt
	/// </summary>
	/// <param name="playerShip"></param>
	public void Draw(ShipEntity playerShip)
	{
		// aim at mouse if aimAtMouse, else aim at target if it isn't null
		Vector3 toAimAt;
		if (aimAtMouse)
			toAimAt = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
		else if (targetToAimAt != null)
			toAimAt = targetToAimAt.transform.position;
		else
			return;

		// draw using line renderer
		line.SetPosition(0, playerShip.transform.position);
		toAimAt.z = line.transform.position.z;

		// these additions to centre the line if required (eg cursor)
		toAimAt.x += xDisplacementFromCursorCentre;
		toAimAt.y += yDisplacementFromCursonCentre;
		line.SetPosition(1, toAimAt);
	}
}
